namespace vm2.XmlExpressionSerialization.Abstractions;

/// <summary>
/// Interface IExpressionTransform represents a transform of a LINQ expression to some document model.
/// (Subsequently the document could be stored in some stream.)
/// </summary>
/// <typeparam name="TNode">The type of the top level document model node.</typeparam>
public interface IExpressionTransform<out TNode>
{
    /// <summary>
    /// Transform the specified expression to a document model node type `TNode`.
    /// </summary>
    /// <param name="expression">The expression to be transformed.</param>
    /// <returns>The resultant top level document model node `TNode`.</returns>
    TNode Transform(Expression expression);
}
