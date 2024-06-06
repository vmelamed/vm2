﻿namespace vm2.ExpressionSerialization.JsonTransform;

/// <summary>
/// Class ExpressionTransform.
/// Implements the <see cref="IExpressionTransform{JsonNode}"/>: transforms a Linq expression to/from a JSON document object.
/// </summary>
/// <seealso cref="IExpressionTransform{XNode}" />
public class ExpressionTransform(JsonOptions? options = null) : IExpressionTransform<JsonObject>
{
    JsonOptions _options = options ?? new();
    JsonNodeOptions _nodeOptions = new() { PropertyNameCaseInsensitive = false };
    ToJsonTransformVisitor? _expressionVisitor;

    #region IExpressionTransform<JsonObject>
    /// <summary>
    /// Transforms the specified <see cref="Expression"/> to a <see cref="JsonNode"/> model.
    /// </summary>
    /// <param name="expression">The expression to be transformed.</param>
    /// <returns>The resultant top level document model document <see cref="JsonNode"/>.</returns>
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
    /// <param name="document">The document document to be transformed.</param>
    /// <returns>The resultant <see cref="Expression"/>.</returns>
    public Expression Transform(JsonObject document)
    {
        _options.Validate(document);
        throw new NotImplementedException();
    }
    #endregion

    /// <summary>
    /// Serializes the specified expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="stream">The stream to put the XML document to.</param>
    /// <returns>Stream.</returns>
    public void Serialize(
        Expression expression,
        Stream stream)
    {
        var document = Transform(expression);
        using var writer = new Utf8JsonWriter(
                                    stream,
                                    new JsonWriterOptions()
                                    {
                                        Indented       = _options.Indent,
                                        SkipValidation = false,
                                    });

        document.WriteTo(writer, _options.JsonSerializerOptions);
    }

    /// <summary>
    /// Serializes the specified expression.
    /// </summary>
    /// <param name="stream">The stream to get the XML document from.</param>
    /// <returns>Stream.</returns>
    public Expression Deserialize(
        Stream stream)
    {
        var document = JsonNode.Parse(
                        stream,
                        new JsonNodeOptions()
                        {
                            PropertyNameCaseInsensitive = false
                        },
                        new JsonDocumentOptions()
                        {
                            AllowTrailingCommas = true,
                            CommentHandling = JsonCommentHandling.Skip,
                            MaxDepth = 1000
                        })
                        ??
                        throw new SerializationException("Could not load JSON object;");

        if (document.GetValueKind() != JsonValueKind.Object)
            throw new SerializationException($"The document does not contain a JSON object but {document.GetValueKind()}");

        return Transform(document.AsObject());
    }
}