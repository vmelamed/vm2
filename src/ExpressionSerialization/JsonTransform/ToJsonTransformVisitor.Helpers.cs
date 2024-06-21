namespace vm2.ExpressionSerialization.JsonTransform;
public partial class ToJsonTransformVisitor
{
    ToJsonDataTransform _dataTransform = new(options);

    int _lastParamIdNumber;
    int _lastLabelIdNumber;
    // labels and parameters/variables are created in one value n and references to them are used in another.
    // These dictionaries keep their id-s so we can create `XAttribute` id-s and idRef-s to them.
    Dictionary<ParameterExpression, JElement> _parameters = [];
    Dictionary<LabelTarget, JElement> _labelTargets = [];

    /// <inheritdoc/>>
    protected override void Reset()
    {
        base.Reset();

        _parameters = [];
        _labelTargets = [];
        _lastParamIdNumber = 0;
        _lastLabelIdNumber = 0;
    }

    string NewParameterId => $"P{++_lastParamIdNumber}";

    string NewLabelId => $"L{++_lastLabelIdNumber}";

    bool IsDefined(ParameterExpression parameterExpression)
        => _parameters.ContainsKey(parameterExpression);

    bool IsDefined(LabelTarget labelTarget)
        => _labelTargets.ContainsKey(labelTarget);

    JElement GetParameter(ParameterExpression parameterExpression)
        => _parameters.TryGetValue(parameterExpression, out var parameterElement)
                ? parameterElement.DeepClone()
                : _parameters[parameterExpression] =
                    new JElement(
                        Vocabulary.Parameter,
                            PropertyType(parameterExpression),
                            PropertyName(parameterExpression.Name),
                            parameterExpression.IsByRef ? new JElement(Vocabulary.IsByRef, parameterExpression.IsByRef) : null,
                            new JElement(Vocabulary.Id, NewParameterId));

    JElement GetLabelTarget(LabelTarget labelTarget)
        => _labelTargets.TryGetValue(labelTarget, out var targetElement)
                ? targetElement.DeepClone()
                : _labelTargets[labelTarget] =
                        new JElement(
                                Vocabulary.LabelTarget,
                                    labelTarget.Type != typeof(void) ? PropertyType(labelTarget.Type) : null,
                                    PropertyName(labelTarget.Name),
                                    new JElement(Vocabulary.Id, NewLabelId));

    /// <summary>
    /// Pops one element from the stack
    /// <see cref="ExpressionTransformVisitor{TElement}._elements"/>.
    /// </summary>
    /// <returns>JElement.</returns>
    JElement PopElement() => _elements.Pop();

    /// <summary>
    /// Pops one element from the stack and returns its <see cref="JElement.Value"/>
    /// <see cref="ExpressionTransformVisitor{TElement}._elements"/>.
    /// </summary>
    /// <returns>JElement.</returns>
    JsonNode? PopElementValue() => _elements.Pop().Value;

    /// <summary>
    /// Pops one element from the stack and returns its <see cref="JElement.Value"/> wrapped in a new <see cref="JsonObject"/>
    /// <see cref="ExpressionTransformVisitor{TElement}._elements"/>.
    /// </summary>
    /// <returns>JsonObject.</returns>
    JsonObject PopWrappedValue() => new JsonObject().Add(_elements.Pop(), null);

    /// <summary>
    /// Pops a specified number of elements from the stack in the order they entered (FIFO, nor LIFO).
    /// <see cref="ExpressionTransformVisitor{TElement}._elements"/>.
    /// </summary>
    /// <param name="numberOfExpressions">The number of expressions.</param>
    /// <returns>System.Collections.Generic.IEnumerable&lt;JElement&gt;.</returns>
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
    /// Pops a specified number of elements from the stack in the order they entered (FIFO, not LIFO) and returns 
    /// <see cref="IEnumerable{JsonNode}"/> of their <see cref="JElement.Value"/>.
    /// <see cref="ExpressionTransformVisitor{TElement}._elements"/>.
    /// </summary>
    /// <param name="numberOfExpressions">The number of expressions.</param>
    /// <returns>System.Collections.Generic.IEnumerable&lt;JElement&gt;.</returns>
    IEnumerable<JsonNode?> PopElementsValues(int numberOfExpressions)
    {
        // we need this intermediary stack to return the elements in FIFO order
        Stack<JsonNode?> tempElements = new(numberOfExpressions);

        // pop the expressions:
        for (var i = 0; i < numberOfExpressions; i++)
            tempElements.Push(_elements.Pop().Value);

        return tempElements;
    }

    /// <summary>
    /// Pops a specified number of elements from the stack in the order they entered (FIFO, not LIFO) and returns 
    /// <see cref="IEnumerable{JsonNode}"/> of their <see cref="JElement.Value"/>.
    /// <see cref="ExpressionTransformVisitor{TElement}._elements"/>.
    /// </summary>
    /// <param name="numberOfExpressions">The number of expressions.</param>
    /// <returns>System.Collections.Generic.IEnumerable&lt;JElement&gt;.</returns>
    IEnumerable<JsonObject> PopWrappedValues(int numberOfExpressions)
    {
        // we need this intermediary stack to return the elements in FIFO order
        Stack<JsonObject> tempElements = new(numberOfExpressions);

        // pop the expressions:
        for (var i = 0; i < numberOfExpressions; i++)
            tempElements.Push(PopWrappedValue());

        return tempElements;
    }

    /// <summary>
    /// Creates a <see cref="JElement"/> for a property `name` with the given <paramref name="identifier"/>.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    JElement? PropertyName(string? identifier)
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
    static JElement? PropertyType(Type? type)
        => type is not null
                ? new(Vocabulary.Type, Transform.TypeName(type))
                : null;

    // reflection metadata

    /// <summary>
    /// Visits the binary operation implementing method (if any).
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    JElement? VisitMethodInfo(BinaryExpression node)
        => node.Method is MemberInfo mi
                ? VisitMemberInfo(mi)
                : null;

    /// <summary>
    /// Visits the unary operation implementing method (if any).
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    JElement? VisitMethodInfo(UnaryExpression node)
        => node.Method is MemberInfo mi
                ? VisitMemberInfo(mi)
                : null;

    /// <summary>
    /// Visits a type member's runtime information.
    /// </summary>
    /// <param name="member">The member.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    JElement? VisitMemberInfo(MemberInfo? member)
    {
        if (member is null)
            return null;

        JElement? declaringTypeProperty = member.DeclaringType is Type dt ? new JElement(Vocabulary.DeclaringType, Transform.TypeName(dt)) : null;
        JElement? nameProperty = PropertyName(member.Name);
        JElement? visibilityProperty = member switch
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
                                                _                           => null
                                            },
                _ => null
            };

        return member switch {
            ConstructorInfo ci => !ci.IsStatic
                                    ? new JElement(
                                            Vocabulary.Constructor,
                                                declaringTypeProperty,
                                                visibilityProperty,
                                                new JElement(Vocabulary.ParameterSpecs, VisitParameters(ci.GetParameters())))
                                    : throw new InternalTransformErrorException($"Don't know how to use static constructors."),

            PropertyInfo pi => new JElement(
                                        Vocabulary.Property,
                                            declaringTypeProperty,
                                            visibilityProperty,
                                            PropertyType(pi.PropertyType ?? throw new InternalTransformErrorException("PropertyInfo's DeclaringType is null.")),
                                            nameProperty,
                                            pi.GetIndexParameters().Length != 0
                                                ? new JElement(Vocabulary.ParameterSpecs, VisitParameters(pi.GetIndexParameters()))
                                                : null),

            MethodInfo mi => new JElement(
                                        Vocabulary.Method,
                                            declaringTypeProperty,
                                            mi.IsStatic ? new JElement(Vocabulary.Static, true) : null,
                                            visibilityProperty,
                                            PropertyType(mi.ReturnType),
                                            nameProperty,
                                            new JElement(Vocabulary.ParameterSpecs, VisitParameters(mi.GetParameters()))),

            EventInfo ei => new JElement(
                                        Vocabulary.Event,
                                            declaringTypeProperty,
                                            PropertyType(ei.EventHandlerType ?? throw new InternalTransformErrorException("EventInfo's EventHandlerType is null.")),
                                            nameProperty),

            FieldInfo fi => new JElement(
                                        Vocabulary.Field,
                                            declaringTypeProperty,
                                            fi.IsStatic ? new JElement(Vocabulary.Static, true) : null,
                                            visibilityProperty,
                                            fi.IsInitOnly ? new JElement(Vocabulary.ReadOnly, true) : null,
                                            PropertyType(fi.FieldType ?? throw new InternalTransformErrorException("GetMethodInfo's DeclaringType is null.")),
                                            nameProperty),

            _ => throw new InternalTransformErrorException("Unknown MemberInfo.")
        };
    }

    /// <summary>
    /// Creates a sequence of JSON elements for each of the <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>A sequence of elements.</returns>
    static IEnumerable<JsonObject> VisitParameters(IEnumerable<ParameterInfo> parameters)
        => parameters.Select(
                param => new JsonObject()
                                .Add(
                                    PropertyType(param.ParameterType),
                                    param.Name is not null ? new JElement(Vocabulary.Name, param.Name) : null,
                                    param.ParameterType.IsByRef || param.IsOut ? new JElement(Vocabulary.IsByRef, true) : null));
}
