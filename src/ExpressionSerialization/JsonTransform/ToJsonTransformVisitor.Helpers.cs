namespace vm2.ExpressionSerialization.JsonTransform;
public partial class ToJsonTransformVisitor
{
    /// <summary>
    /// Pops one element from the stack
    /// <see cref="ExpressionTransformVisitor{TElement}._elements"/>.
    /// </summary>
    /// <returns>JElement.</returns>
    JElement PopElement() => _elements.Pop();

    /// <summary>
    /// Pops a specified number of elements from the stack in the order they entered (FIFO, nor LIFO).
    /// <see cref="ExpressionTransformVisitor{TElement}._elements"/>.
    /// </summary>
    /// <param name="numberOfExpressions">The number of expressions.</param>
    /// <returns>System.Collections.Generic.IEnumerable&lt;System.Xml.Linq.JElement&gt;.</returns>
    IEnumerable<JElement> PopElements(int numberOfExpressions)
    {
        // we need this intermediary stack to return the elements in FIFO order
        Stack<JElement> tempElements = new(numberOfExpressions);

        // pop the expressions:
        for (var i = 0; i < numberOfExpressions; i++)
            tempElements.Push(_elements.Pop());

        return tempElements;
    }

    /// <summary>
    /// Creates a <see cref="JElement"/> for a property `name` with the given <paramref name="identifier"/>.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    JElement? PropertyName(string identifier)
        => !string.IsNullOrWhiteSpace(identifier)
                ? new JElement(Vocabulary.Name, Transform.Identifier(identifier, options.Identifiers))
                : null;

    /// <summary>
    /// Creates a <see cref="JElement"/> for a property `type` of the given <paramref name="node"/>.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    static JElement? PropertyType(Expression? node)
        => node is not null
                ? PropertyType(node.Type)
                : null;

    /// <summary>
    /// Creates a <see cref="JElement"/> for a property `type` for the given <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The node.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    static JElement? PropertyType(Type type)
        => type is not null
                ? new(Vocabulary.Type, Transform.TypeName(type))
                : null;

    // reflection metadata

    /// <summary>
    /// Visits the binary operation implementing method (if any).
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    static JElement? VisitMethodInfo(BinaryExpression node)
        => node.Method is MemberInfo mi
                ? VisitMemberInfo(mi)
                : null;

    /// <summary>
    /// Visits the unary operation implementing method (if any).
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    static JElement? VisitMethodInfo(UnaryExpression node)
        => node.Method is MemberInfo mi
                ? VisitMemberInfo(mi)
                : null;

    /// <summary>
    /// Visits a type member's runtime information.
    /// </summary>
    /// <param name="member">The member.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    static JElement? VisitMemberInfo(MemberInfo? member)
    {
        if (member is null)
            return null;

        JElement? declaringType = member.DeclaringType is Type dt ? new JElement(Vocabulary.DeclaringType, Transform.TypeName(dt)) : null;
        JElement? nameAttribute = member.Name is not null ? new JElement(Vocabulary.Name, member.Name) : null;
        JElement? visibility = member switch
            {
                ConstructorInfo ci => ci.IsPublic
                                        ? null
                                        : (ci.Attributes & MethodAttributes.MemberAccessMask) switch
                                            {
                                                MethodAttributes.Private     => new JElement(Vocabulary.Visibility, Vocabulary.Private),
                                                MethodAttributes.Assembly    => new JElement(Vocabulary.Visibility, Vocabulary.Assembly),
                                                MethodAttributes.Family      => new JElement(Vocabulary.Visibility, Vocabulary.Family),
                                                MethodAttributes.FamANDAssem => new JElement(Vocabulary.Visibility, Vocabulary.FamilyAndAssembly),
                                                MethodAttributes.FamORAssem  => new JElement(Vocabulary.Visibility, Vocabulary.FamilyOrAssembly),
                                                _                            => null
                                            },
                MethodInfo mi => mi.IsPublic
                                        ? null
                                        : (mi.Attributes & MethodAttributes.MemberAccessMask) switch
                                            {
                                                MethodAttributes.Private     => new JElement(Vocabulary.Visibility, Vocabulary.Private),
                                                MethodAttributes.Assembly    => new JElement(Vocabulary.Visibility, Vocabulary.Assembly),
                                                MethodAttributes.Family      => new JElement(Vocabulary.Visibility, Vocabulary.Family),
                                                MethodAttributes.FamANDAssem => new JElement(Vocabulary.Visibility, Vocabulary.FamilyAndAssembly),
                                                MethodAttributes.FamORAssem  => new JElement(Vocabulary.Visibility, Vocabulary.FamilyOrAssembly),
                                                _                            => null
                                            },
                FieldInfo fi =>  fi.IsPublic
                                        ? null
                                        : (fi.Attributes & FieldAttributes.FieldAccessMask) switch
                                            {
                                                FieldAttributes.Private     => new JElement(Vocabulary.Visibility, Vocabulary.Private),
                                                FieldAttributes.Assembly    => new JElement(Vocabulary.Visibility, Vocabulary.Assembly),
                                                FieldAttributes.Family      => new JElement(Vocabulary.Visibility, Vocabulary.Family),
                                                FieldAttributes.FamANDAssem => new JElement(Vocabulary.Visibility, Vocabulary.FamilyAndAssembly),
                                                FieldAttributes.FamORAssem  => new JElement(Vocabulary.Visibility, Vocabulary.FamilyOrAssembly),
                                                _                            => null
                                            },
                _ => null
            };

        return member switch {
            ConstructorInfo ci => !ci.IsStatic
                                    ? new JElement(
                                        Vocabulary.Constructor,
                                        declaringType,
                                        visibility,
                                        new JElement(Vocabulary.ParameterSpecs, VisitParameters(ci.GetParameters())))
                                    : throw new InternalTransformErrorException($"Don't know how to use static constructors."),

            PropertyInfo pi => new JElement(
                                        Vocabulary.Property,
                                        declaringType,
                                        visibility,
                                        PropertyType(pi.PropertyType ?? throw new InternalTransformErrorException("PropertyInfo's DeclaringType is null.")),
                                        nameAttribute,
                                        pi.GetIndexParameters().Length != 0
                                            ? new JElement(Vocabulary.ParameterSpecs, VisitParameters(pi.GetIndexParameters()))
                                            : null),

            MethodInfo mi => new JElement(
                                        Vocabulary.Method,
                                        declaringType,
                                        mi.IsStatic ? new JElement(Vocabulary.Static, true) : null,
                                        visibility,
                                        PropertyType(mi.ReturnType),
                                        nameAttribute,
                                        new JElement(Vocabulary.ParameterSpecs, VisitParameters(mi.GetParameters()))),

            EventInfo ei => new JElement(
                                        Vocabulary.Event,
                                        declaringType,
                                        PropertyType(ei.EventHandlerType ?? throw new InternalTransformErrorException("EventInfo's EventHandlerType is null.")),
                                        nameAttribute),

            FieldInfo fi => new JElement(
                                        Vocabulary.Field,
                                        declaringType,
                                        fi.IsStatic ? new JElement(Vocabulary.Static, true) : null,
                                        visibility,
                                        fi.IsInitOnly ? new JElement(Vocabulary.ReadOnly, true) : null,
                                        PropertyType(fi.FieldType ?? throw new InternalTransformErrorException("GetMethodInfo's DeclaringType is null.")),
                                        nameAttribute),

            _ => throw new InternalTransformErrorException("Unknown MemberInfo.")
        };
    }

    /// <summary>
    /// Creates a sequence of XML elements for each of the <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>A sequence of elements.</returns>
    static IEnumerable<JElement> VisitParameters(IEnumerable<ParameterInfo> parameters)
        => parameters.Select(param => new JElement(
                                            Vocabulary.ParameterSpec,
                                            PropertyType(param.ParameterType),
                                            param.Name is not null ? new JElement(Vocabulary.Name, param.Name) : null,
                                            param.ParameterType.IsByRef || param.IsOut ? new JElement(Vocabulary.IsByRef, true) : null));
}
