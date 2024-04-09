namespace vm2.ExpressionSerialization.Xml;

partial class ToXmlTransformVisitor(TransformOptions? options = null) : ExpressionTransformVisitor<XNode>(options)
{
    protected override XNode GetEmptyNode(ExpressionType nodeType)
        => new XElement(XmlNamespace.Xxp + TransformOptions.DoTransformIdentifier(nodeType.ToString(), IdentifierTransformConvention.Camel));

    DataTransform _dataTransform = new(options);

    protected override Expression VisitConstant(ConstantExpression node)
        => GenericVisit(
                node,
                base.VisitConstant,
                (node, element) => _dataTransform.GetTransform(node.Type)(node, (XElement)element));
}
