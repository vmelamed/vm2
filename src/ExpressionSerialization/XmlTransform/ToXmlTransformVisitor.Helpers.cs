namespace vm2.ExpressionSerialization.XmlTransform;

using System.Reflection;
using System.Xml.Linq;

/// <summary>
/// Class XmlTransformVisitor.
/// Implements the <see cref="ExpressionTransformVisitor{XNode}" />
/// </summary>
/// <seealso cref="ExpressionTransformVisitor{XNode}" />
public partial class ToXmlTransformVisitor : ExpressionTransformVisitor<XElement>
{
    IEnumerable<XElement> PopElements(int numberOfExpressions)
    {
        Stack<XElement> tempElements = [];

        // pop the expressions:
        for (var i = 0; i < numberOfExpressions; i++)
            tempElements.Push(_elements.Pop());

        return tempElements;
    }

    XAttribute? AttributeName(string identifier)
        => !string.IsNullOrWhiteSpace(identifier)
            ? new XAttribute(AttributeNames.Name, Transform.Identifier(identifier, _options.Identifiers))
            : null;

    static XAttribute? AttributeType(Expression? node)
        => node is not null
            ? AttributeType(node.Type)
            : null;

    static XAttribute? AttributeType(Type type)
        => type is not null && type != typeof(void)
            ? new(AttributeNames.Type, Transform.TypeName(type))
            : null;

    static XElement? VisitMethodInfo(BinaryExpression node)
        => node.Method is MemberInfo mi
            ? VisitMemberInfo(mi)
            : null;

    static XElement? VisitMethodInfo(UnaryExpression node)
        => node.Method is MemberInfo mi
            ? VisitMemberInfo(mi)
            : null;

    /// <summary>
    /// Creates a sequence of XML elements for each of the <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>A sequence of elements.</returns>
    static IEnumerable<XElement> VisitParameters(IEnumerable<ParameterInfo> parameters)
    {
        if (parameters is not null)
            foreach (var param in parameters)
                yield return new XElement(
                                    ElementNames.ParameterSpec,
                                    AttributeType(param.ParameterType),
                                    param.Name is not null ? new XAttribute(AttributeNames.Name, param.Name) : null,
                                    param.IsOut || param.ParameterType.IsByRef ? new XAttribute(AttributeNames.IsByRef, true) : null);
    }

    static XElement? VisitMemberInfo(MemberInfo? member)
    {
        if (member is null)
            return null;

        XAttribute? visibility = member is MethodInfo method && !method.IsPublic
                                    ? (method.Attributes & MethodAttributes.MemberAccessMask) switch {
                                        MethodAttributes.Private     => new XAttribute(AttributeNames.Visibility, AttributeNames.Private),
                                        MethodAttributes.Assembly    => new XAttribute(AttributeNames.Visibility, AttributeNames.Assembly),
                                        MethodAttributes.Family      => new XAttribute(AttributeNames.Visibility, AttributeNames.Family),
                                        MethodAttributes.FamANDAssem => new XAttribute(AttributeNames.Visibility, AttributeNames.FamilyAndAssembly),
                                        MethodAttributes.FamORAssem  => new XAttribute(AttributeNames.Visibility, AttributeNames.FamilyOrAssembly),
                                        _                            => null
                                    }
                                    : null;

        return member switch {
            ConstructorInfo ci => new XElement(
                                        ElementNames.Constructor,
                                        visibility,
                                        AttributeType(ci.DeclaringType ?? throw new InternalTransformErrorException("ConstructorInfo's DeclaringType is null.")),
                                        ci.IsStatic ? new XAttribute(AttributeNames.Static, true) : null,
                                        new XElement(
                                                ElementNames.ParameterSpecs,
                                                VisitParameters(ci.GetParameters()))),
            PropertyInfo pi => new XElement(
                                        ElementNames.Property,
                                        visibility,
                                        AttributeType(pi.PropertyType ?? throw new InternalTransformErrorException("PropertyInfo's DeclaringType is null.")),
                                        new XAttribute(AttributeNames.Name, pi.Name),
                                        pi.GetIndexParameters().Length != 0
                                            ? new XElement(
                                                    ElementNames.ParameterSpecs,
                                                    VisitParameters(pi.GetIndexParameters()))
                                            : null),

            MethodInfo mi => new XElement(
                                        ElementNames.Method,
                                        mi.IsStatic ? new XAttribute(AttributeNames.Static, true) : null,
                                        visibility,
                                        new XAttribute(AttributeNames.Type, Transform.TypeName(mi.ReturnType)),
                                        new XAttribute(AttributeNames.Name, mi.Name),
                                        new XElement(ElementNames.ParameterSpecs, VisitParameters(mi.GetParameters()))),

            EventInfo ei => new XElement(
                                        ElementNames.Event,
                                        AttributeType(ei.EventHandlerType ?? throw new InternalTransformErrorException("EventInfo's EventHandlerType is null.")),
                                        new XAttribute(AttributeNames.Name, ei.Name)),

            FieldInfo fi => new XElement(
                                        ElementNames.Field,
                                        fi.IsStatic ? new XAttribute(AttributeNames.Static, true) : null,
                                        visibility,
                                        fi.IsInitOnly ? new XAttribute(AttributeNames.ReadOnly, true) : null,
                                        AttributeType(fi.FieldType ?? throw new InternalTransformErrorException("MethodInfo's DeclaringType is null.")),
                                        new XAttribute(AttributeNames.Name, fi.Name)),

            _ => throw new InternalTransformErrorException("Unknown MemberInfo.")
        };
    }
}
