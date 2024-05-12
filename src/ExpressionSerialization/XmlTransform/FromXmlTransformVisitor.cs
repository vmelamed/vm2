namespace vm2.ExpressionSerialization.XmlTransform;

using System.Xml.Linq;

/// <summary>
/// Class that visits the nodes of an XML element to produce a LINQ expression tree.
/// </summary>
public partial class FromXmlTransformVisitor
{
    Dictionary<string, ParameterExpression> _parameters = [];
    Dictionary<string, LabelTarget> _labelTargets = [];

    void ResetVisitState()
    {
        _parameters = [];
        _labelTargets = [];
    }

    /// <summary>
    /// This is the starting point of the visitor.
    /// </summary>
    /// <param name="element">The element to be visited.</param>
    /// <returns>The created expression.</returns>
    public virtual Expression Visit(XElement element)
    {
        ResetVisitState();
        return _transforms[element.Name](this, element);
    }

    /// <summary>
    /// Visits an XML element representing an expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitElement(XElement e) => Visit(e.FirstChild());

    internal static ExpressionType GetExpressionType(XElement element)
        => element != null
            ? (ExpressionType)Enum.Parse(
                                typeof(ExpressionType),
                                element.Name.LocalName,
                                true)
            : throw new ArgumentNullException(nameof(element));

    #region Concrete XML element visitors
    /// <summary>
    /// Visits an XML element representing a constant expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="ConstantExpression"/> represented by the element.</returns>
    protected virtual Expression VisitConstant(XElement e)
        => FromXmlDataTransform.ConstantTransform(e.FirstChild());

    /// <summary>
    /// Visits an XML element representing a default expression (e.g. <c>default(int)</c>).
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="ParameterExpression"/> represented by the element.</returns>
    protected virtual Expression VisitDefault(XElement e)
        => Expression.Default(e.AttributeType());

    /// <summary>
    /// Visits an XML element representing a parameter expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="ParameterExpression"/> represented by the element.</returns>
    protected virtual Expression VisitParameter(XElement e)
    {
        var id = e.Attribute(AttributeNames.Id)?.Value ?? e.Attribute(AttributeNames.IdRef)?.Value
                                ?? throw new SerializationException($"Could not get the Id or IdRef of a parameter or variable in {e.Name}");

        if (!_parameters.TryGetValue(id, out var expression))
        {
            expression = Expression.Parameter(e.Type(), e.Attribute(AttributeNames.Name)?.Value);
            _parameters[id] = expression;
        }

        return expression;
    }

    /// <summary>
    /// Visits an XML element representing a list of parameter definition expressions.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="IEnumerable{ParameterExpression}"/> represented by the element.</returns>
    protected virtual IEnumerable<ParameterExpression> VisitParameterDefinitionList(XElement e)
        => e.Elements().Select(pe => VisitParameter(pe) as ParameterExpression
                                        ?? throw new SerializationException($"Got non-parameter element in the parameter list element {e.Name}"));

    /// <summary>
    /// Visits an XML element representing a lambda expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="LambdaExpression"/> represented by the element.</returns>
    protected virtual Expression VisitLambda(XElement e)
    {
        var parameters = VisitParameterDefinitionList(
                            e.Element(ElementNames.Parameters)
                                ?? throw new SerializationException($"Could not find parameters element in the element {e.Name}"));

        var body = VisitElement(
                            e.Element(ElementNames.Body)
                                ?? throw new SerializationException($"Could not find body element in the element {e.Name}"));

        return Expression.Lambda(
                    body,
                    XmlConvert.ToBoolean(e.Attribute(AttributeNames.TailCall)?.Value ?? "false"),
                    parameters.ToArray());
    }

    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitUnary(XElement e)
        => Expression.MakeUnary(
                        e.ExpressionType(),
                        VisitElement(e),
                        e.AttributeType(),
                        e.MethodInfo()
                    );

    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitBinary(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitTypeBinary(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitBlock(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitConditional(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitIndex(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitNew(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitThrow(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitMember(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitMethodCall(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitLabel(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitGoto(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitLoop(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitSwitch(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitTry(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitExpressionContainer(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitListInit(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitNewArrayInit(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitNewArrayBounds(XElement e) => throw new NotImplementedException();
    /// <summary>
    /// Visits an XML element representing a XXXX expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitMemberInit(XElement e) => throw new NotImplementedException();
    #endregion
}
