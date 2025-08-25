namespace vm2.Linq.ExpressionSerialization.Shared.Abstractions;

/// <summary>
/// Interface IExpressionTransform represents a transform of a LINQ expression to and from some document model (XML, JSON, etc.).
/// </summary>
/// <typeparam name="TDocument">The type of the top level document model node.</typeparam>
public interface IExpressionTransform<TDocument>
{
    /// <summary>
    /// Transforms the specified expression to a document model type `TDocument`.
    /// </summary>
    /// <param name="expression">The expression to be transformed.</param>
    /// <returns>The resultant top level document model node `TDocument`.</returns>
    TDocument Transform(Expression expression);

    /// <summary>
    /// Transforms the specified document model node of type `TDocument` to a LINQ expression.
    /// </summary>
    /// <param name="document">The document node to be transformed.</param>
    /// <returns>The resultant expression.</returns>
    Expression Transform(TDocument document);
}
