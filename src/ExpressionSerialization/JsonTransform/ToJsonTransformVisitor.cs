namespace vm2.ExpressionSerialization.JsonTransform;
/// <summary>
/// Class XmlTransformVisitor.
/// Implements the <see cref="ExpressionTransformVisitor{XNode}" />
/// </summary>
/// <seealso cref="ExpressionTransformVisitor{XNode}" />
public partial class ToJsonTransformVisitor(JsonOptions options) : ExpressionTransformVisitor<JElement>
{
    ToJsonDataTransform _dataTransform = new(options);

    // labels and parameters/variables are created in one value n and references to them are used in another.
    // These dictionaries keep their id-s so we can create `XAttribute` id-s and idRef-s to them.
    //Dictionary<LabelTarget, XElement> _labelTargets = [];
    //Dictionary<ParameterExpression, XElement> _parameters = [];
    //int _lastLabelIdNumber;
    //int _lastParamIdNumber;

    /// <inheritdoc/>>
    protected override void Reset()
    {
        base.Reset();

        //_parameters = [];
        //_labelTargets = [];
        //_lastParamIdNumber = 0;
        //_lastLabelIdNumber = 0;
    }

    /// <summary>
    /// Gets a properly named node corresponding to the current expression node.
    /// </summary>
    /// <param name="node">The expression node for which to create an empty document node.</param>
    /// <returns>TDocument.</returns>
    protected override JElement GetEmptyNode(Expression node)
        => new(
            Transform.Identifier(node.NodeType.ToString(), IdentifierConventions.Camel),
            node switch {
                // omit the type for expressions where their element says it all
                ConstantExpression => null,
                ListInitExpression => null,
                NewExpression => null,
                NewArrayExpression => null,
                LabelExpression => null,
                // do not omit the void return type for these nodes:
                LambdaExpression n => new(Vocabulary.Type, Transform.TypeName(n.ReturnType)),
                MethodCallExpression n => new(Vocabulary.Type, Transform.TypeName(n.Method.ReturnType)),
                InvocationExpression n => new(Vocabulary.Type, Transform.TypeName(n.Expression.Type)),
                // for the rest: add attribute type if it is not void:
                _ => PropertyType(node),
            });

    /// <inheritdoc/>
    protected override Expression VisitConstant(ConstantExpression node)
    {
        try
        {
            return GenericVisit(
                     node,
                     base.VisitConstant,
                     (n, x) =>
                     {
                         options.AddComment(x, n);
                         x.Add(
                            options.TypeComment(n.Type),
                            _dataTransform.TransformNode(n));
                     });
        }
        catch (Exception x)
        {
            throw new SerializationException($"Don't know how to serialize {node.Type}", x);
        }
    }
}
