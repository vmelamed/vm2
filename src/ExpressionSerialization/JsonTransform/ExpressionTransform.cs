namespace vm2.ExpressionSerialization.JsonTransform;

/// <summary>
/// Class ExpressionTransform.
/// Implements the <see cref="IExpressionTransform{JsonNode}"/>: transforms a Linq expression to/from a JSON node object.
/// </summary>
/// <seealso cref="IExpressionTransform{XNode}" />
public class ExpressionTransform(JsonOptions? options = null) : IExpressionTransform<JsonObject>
{
    JsonOptions _options = options ?? new();
    JsonNodeOptions _nodeOptions = new() { PropertyNameCaseInsensitive = false };
    ToJsonTransformVisitor? _expressionVisitor;

    /// <summary>
    /// Transforms the specified <see cref="Expression"/> to a <see cref="JsonNode"/> model.
    /// </summary>
    /// <param name="expression">The expression to be transformed.</param>
    /// <returns>The resultant top level document model node <see cref="JsonNode"/>.</returns>
    public JsonObject Transform(Expression expression)
    {
        _expressionVisitor ??= new ToJsonTransformVisitor(_options);
        _expressionVisitor.Visit(expression);

        var docObject = new JsonObject(_nodeOptions)
        {
            { "$schema", JsonOptions.Exs },
            _expressionVisitor.Result
        };
        return docObject;
    }

    /// <summary>
    /// Transforms the specified document of type <see cref="JsonNode"/> to a LINQ <see cref="Expression"/>.
    /// </summary>
    /// <param name="document">The document node to be transformed.</param>
    /// <returns>The resultant <see cref="Expression"/>.</returns>
    public Expression Transform(JsonObject document) => throw new NotImplementedException();
}
