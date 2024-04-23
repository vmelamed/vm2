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
    Stack<XElement> PopElements(int numberOfExpressions)
    {
        var stack = new Stack<XElement>();

        // pop the expressions:
        for (var i = 0; i < numberOfExpressions; i++)
            stack.Push(_elements.Pop());

        return stack;
    }

    static int _nameSuffix;

    static string GetName()
        => $"_name{_nameSuffix++}";

    /// <summary>
    /// Creates a sequence of XML elements for each of the <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>A sequence of elements.</returns>
    static IEnumerable<XElement> VisitParameters(IEnumerable<ParameterInfo> parameters)
    {
        if (parameters == null)
            yield break;

        foreach (var param in parameters)
            yield return new XElement(
                                ElementNames.Parameter,
                                new XAttribute(AttributeNames.Type, Transform.TypeName(param.ParameterType)),
                                new XAttribute(AttributeNames.Name, param.Name ?? GetName()),
                                param.IsOut || param.ParameterType.IsByRef ? new XAttribute(AttributeNames.IsByRef, true) : null);
    }

#pragma warning disable CA1859 // Use concrete types when possible for improved performance
    IEnumerable<XElement> PopExpressions(int numberOfExpressions)
    {
        var stack = new Stack<XElement>();

        // pop the expressions:
        for (var i = 0; i < numberOfExpressions; i++)
            stack.Push(
                _elements.Pop());

        return stack;
    }
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

    static XElement? VisitMethodInfo(BinaryExpression node)
        => node.Method is MemberInfo mi ? VisitMemberInfo(mi) : null;

    static XElement? VisitMethodInfo(UnaryExpression node)
        => node.Method is MemberInfo mi ? VisitMemberInfo(mi) : null;

    static XElement VisitMemberInfo(MemberInfo member)
    {
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
                                        new XAttribute(AttributeNames.Type, Transform.TypeName(ci.DeclaringType ?? throw new InternalTransformErrorException("ConstructorInfo's DeclaringType is null."))),
                                        visibility,
                                        ci.IsStatic ? new XAttribute(AttributeNames.Static, true) : null,
                                        new XElement(
                                                ElementNames.Parameters,
                                                VisitParameters(ci.GetParameters()))),
            PropertyInfo pi => new XElement(
                                    ElementNames.Property,
                                        new XAttribute(AttributeNames.Type, Transform.TypeName(pi.DeclaringType ?? throw new InternalTransformErrorException("PropertyInfo's DeclaringType is null."))),
                                        visibility,
                                        new XAttribute(AttributeNames.Name, pi.Name),
                                        VisitParameters(pi.GetIndexParameters())),

            MethodInfo mi => new XElement(
                                     ElementNames.Method,
                                         new XAttribute(AttributeNames.Type, Transform.TypeName(mi.DeclaringType ?? throw new InternalTransformErrorException("MethodInfo's DeclaringType is null."))),
                                         visibility,
                                         mi.IsStatic ? new XAttribute(AttributeNames.Static, true) : null,
                                         new XAttribute(AttributeNames.Name, mi.Name),
                                         new XElement(ElementNames.Parameters, VisitParameters(mi.GetParameters()))),

            EventInfo ei => new XElement(
                                    ElementNames.Property,
                                        new XAttribute(AttributeNames.Type, Transform.TypeName(ei.DeclaringType ?? throw new InternalTransformErrorException("MethodInfo's DeclaringType is null."))),
                                        new XAttribute(AttributeNames.Name, ei.Name)),

            FieldInfo fi => new XElement(
                                    ElementNames.Field,
                                        new XAttribute(AttributeNames.Type, Transform.TypeName(fi.DeclaringType ?? throw new InternalTransformErrorException("MethodInfo's DeclaringType is null."))),
                                        visibility,
                                        fi.IsStatic ? new XAttribute(AttributeNames.Static, true) : null,
                                        new XAttribute(AttributeNames.Name, fi.Name)),

            _ => throw new InternalTransformErrorException("Unknown MemberInfo.")
        };
    }
}
