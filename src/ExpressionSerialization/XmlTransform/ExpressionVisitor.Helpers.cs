namespace vm2.ExpressionSerialization.XmlTransform;

/// <summary>
/// Class ExpressionVisitor.
/// Implements the <see cref="ExpressionTransformVisitor{XNode}" />
/// </summary>
/// <seealso cref="ExpressionTransformVisitor{XNode}" />
public partial class ExpressionVisitor : ExpressionTransformVisitor<XElement>
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

    static XElement ReplaceParametersWithReferences(XElement parameters, XElement body)
    {
        if (parameters == null)
            return body;

        var varRefs = from p in parameters.Elements(ElementNames.Parameter)
                      from a in body.Descendants(ElementNames.Parameter)
                      let pName = p.Attribute(AttributeNames.Name)?.Value ?? GetName()
                      let aa = a.Attribute(AttributeNames.Name)
                      let aName = aa?.Value ?? GetName()
                      where aName == pName
                      select a;

        // ... with references to parameters (parameter-s without Type attribute)
        foreach (var a in varRefs)
        {
            a.AddAfterSelf(
                new XElement(
                    ElementNames.Parameter,
                    new XAttribute(AttributeNames.Name, a.Attribute(AttributeNames.Name)?.Value ?? GetName())));
            a.Remove();
        }

        return body;
    }

    static XElement ReplaceParameterWithReference(XElement parameter, XElement body)
    {
        if (parameter != null && body == null)
            throw new ArgumentNullException(nameof(body));

        if (parameter == null)
            return body;

        var pName = parameter.Attribute(AttributeNames.Name)?.Value ?? GetName();

        // replace all parameters in the body...
        var varRefs = from a in body.Descendants(ElementNames.Parameter)
                      let aa = a.Attribute(AttributeNames.Name)
                      let aName = aa?.Value ?? GetName()
                      where aName == pName
                      select a;

        // ... with references to the parameter (parameter without Type attribute)
        foreach (var a in varRefs)
        {
            a.AddAfterSelf(new XElement(
                                    ElementNames.Parameter,
                                    new XAttribute(
                                            AttributeNames.Name,
                                            pName)));
            a.Remove();
        }

        return body;
    }
}
