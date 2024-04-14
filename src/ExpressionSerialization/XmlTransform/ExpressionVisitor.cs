namespace vm2.ExpressionSerialization.XmlTransform;

using vm2.ExpressionSerialization.Conventions;

partial class ExpressionVisitor(Options? options = null) : ExpressionTransformVisitor<XNode>(options)
{
    protected override XNode GetEmptyNode(ExpressionType nodeType)
        => new XElement(XmlNamespace.Xxp + Transform.Identifier(nodeType.ToString(), IdentifierConventions.Camel));

    DataTransform _dataTransform = new(options);

    protected override System.Linq.Expressions.Expression VisitConstant(ConstantExpression node)
        => GenericVisit(
                node,
                base.VisitConstant,
                (node, element) => _dataTransform.Get(node.Type)(node, (XElement)element));
}
