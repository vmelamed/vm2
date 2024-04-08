namespace vm2.ExpressionSerialization.Xml;

partial class ToXmlNodeTransform : ExpressionTransformVisitor<XNode>
{
    protected override XNode GetEmptyNode(ExpressionType nodeType) => new XElement(XmlNamespace.Xxp + CamelCase(nodeType.ToString()));

    protected override Expression VisitConstant(ConstantExpression node)
        => GenericVisit(
                node,
                base.VisitConstant,
                (node, element) => new DataTransform().GetTransform(node.Type)(node, (XElement)element));
}
