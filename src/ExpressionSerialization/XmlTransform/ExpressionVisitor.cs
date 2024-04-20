namespace vm2.ExpressionSerialization.XmlTransform;

using System.Xml.Linq;

/// <summary>
/// Class ExpressionVisitor.
/// Implements the <see cref="ExpressionTransformVisitor{XNode}" />
/// </summary>
/// <seealso cref="ExpressionTransformVisitor{XNode}" />
public partial class ExpressionVisitor(Options? options = null) : ExpressionTransformVisitor<XElement>
{
    /// <summary>
    /// The transform options.
    /// </summary>
    protected Options _options = options ?? new();

    /// <summary>
    /// Gets a properly named n corresponding to the current expression n.
    /// </summary>
    /// <param name="nodeType">Type of the n.</param>
    /// <returns>TNode.</returns>
    protected override XElement GetEmptyNode(ExpressionType nodeType)
        => new(Namespaces.Exs + Transform.Identifier(nodeType.ToString(), IdentifierConventions.Camel));

    DataTransform _dataTransform = new(options);

    /// <summary>
    /// Visits the <see cref="ConstantExpression" />.
    /// </summary>
    /// <param name="node">The expression to visit.</param>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
    protected override Expression VisitConstant(ConstantExpression node)
    {
        try
        {
            return GenericVisit(
                     node,
                     base.VisitConstant,
                     (n, e) =>
                     {
                         _options.AddComment(e, n);
                         e.Add(
                            _options.TypeComment(n.Type),
                            _dataTransform.TransformNode(n));
                     });
        }
        catch (InvalidOperationException x)
        {
            throw new NonSerializableObjectException(node.Type, x);
        }
        catch (InvalidDataContractException x)
        {
            throw new NonSerializableObjectException(null, x);
        }
    }

    /// <summary>
    /// Visits a <see cref="DefaultExpression" /> expression n.
    /// </summary>
    /// <param name="node">The expression to visit.</param>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
    protected override Expression VisitDefault(DefaultExpression node)
        => GenericVisit(
            node,
            base.VisitDefault,
            (n, e) => e.Add(
                        _options.TypeComment(n.Type),
                        new XAttribute(AttributeNames.Type, Transform.TypeName(node.Type))));

    /// <summary>
    /// Visits a <see cref="ParameterExpression"/> n.
    /// </summary>
    /// <param name="node">The n.</param>
    /// <returns>System.Linq.Expressions.Expression.</returns>
    protected override Expression VisitParameter(ParameterExpression node)
        => GenericVisit(
            node,
            base.VisitParameter,
            (n, e) => e.Add(
                        _options.TypeComment(n.Type),
                        new XAttribute(AttributeNames.Type, Transform.TypeName(node.Type)),
                        new XAttribute(AttributeNames.Name, node.Name ?? "_"),
                        n.IsByRef ? new XAttribute(AttributeNames.IsByRef, n.IsByRef) : null));

    /// <summary>
    /// Visits the children of a lambda expression - <see cref="Expression{TDelegate}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="node">The n.</param>
    /// <returns>System.Linq.Expressions.Expression.</returns>
    protected override Expression VisitLambda<T>(Expression<T> node)
        => GenericVisit(
            node,
            base.VisitLambda,
            (n, e) => e.Add(
                        n.TailCall ? new XAttribute(AttributeNames.TailCall, n.TailCall) : null,
                        string.IsNullOrWhiteSpace(n.Name) ? null : new XAttribute(AttributeNames.Name, n.Name),
                        n.ReturnType == n.Body.Type ? null : new XAttribute(AttributeNames.DelegateType, n.ReturnType),
                        new XElement(ElementNames.Parameters, PopElements(n.Parameters.Count)),
                        new XElement(ElementNames.Body, _elements.Pop())));
}
