namespace vm2.ExpressionSerialization.Xml;

using System.Linq.Expressions;
using System.Xml.Linq;

/// <summary>
/// Class XmlExpressionTransform.
/// Implements the <see cref="IExpressionTransform{XNode}"/>: transforms a Linq expression to an XML Node object.
/// </summary>
/// <seealso cref="IExpressionTransform{XNode}" />
public class XmlExpressionTransform : IExpressionTransform<XNode>
{
    TransformOptions _options;
    ToXmlTransformVisitor _visitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlExpressionTransform"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public XmlExpressionTransform(TransformOptions? options = null)
    {
        _options = options ?? new TransformOptions();
        _visitor = new ToXmlTransformVisitor(_options);
    }

    /// <summary>
    /// Transforms the specified expression to a document model node type `XNode` (XML).
    /// </summary>
    /// <param name="expression">The expression to be transformed.</param>
    /// <returns>The resultant top level document model node `XNode`.</returns>
    public XNode Transform(Expression expression)
    {
        _visitor.Visit(expression);
        return new XElement(
                        XmlElement.Expression,
                        new XAttribute(
                            "xmlns", XmlNamespace.Xxp),
                            _visitor.Result);
    }

    /// <summary>
    /// Transforms the specified expression to a document model node type `XNode` (XML).
    /// </summary>
    /// <param name="expression">The expression to be transformed.</param>
    /// <returns>The resultant top level document model node `XNode`.</returns>
    public XNode TransformToDocument(Expression expression)
        => new XDocument(
                    new XDeclaration(null, null, null),
                    _options.Comment(expression),
                    Transform(expression));

    /// <summary>
    /// Serializes the specified expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="stream">The stream.</param>
    /// <param name="settings">The settings.</param>
    /// <returns>Stream.</returns>
    public Stream Serialize(Expression expression, Stream stream, XmlWriterSettings? settings = null)
    {
        var doc = TransformToDocument(expression);

        settings ??= new XmlWriterSettings() {
            Indent = _options.Pretty,
            IndentChars = new(' ', _options.IndentSize),
            NamespaceHandling = NamespaceHandling.OmitDuplicates,
            NewLineOnAttributes = _options.Pretty,
            WriteEndDocumentOnClose = true,
            // OmitXmlDeclaration   = true,
        };

        using var writer = new StreamWriter(stream);
        using var xmlWriter = XmlWriter.Create(writer, settings);

        doc.WriteTo(xmlWriter);
        xmlWriter.Close();
        writer.Flush();
        stream.Flush();

        return stream;
    }

    /// <summary>
    /// Serialize as an asynchronous operation.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="stream">The stream.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;Stream&gt; representing the asynchronous operation.</returns>
    public async Task<Stream> SerializeAsync(
        Expression expression,
        Stream stream,
        XmlWriterSettings? settings = null,
        CancellationToken cancellationToken = default)
    {
        var doc = TransformToDocument(expression);

        settings ??= new XmlWriterSettings() {
            Async = true,
            Indent = _options.Pretty,
            IndentChars = new(' ', _options.IndentSize),
            NamespaceHandling = NamespaceHandling.OmitDuplicates,
            NewLineOnAttributes = _options.Pretty,
            WriteEndDocumentOnClose = true,
            // OmitXmlDeclaration   = true,
        };

        using var writer = new StreamWriter(stream);
        using var xmlWriter = XmlWriter.Create(writer, settings);

        await doc.WriteToAsync(xmlWriter, cancellationToken);
        await xmlWriter.FlushAsync();
        await writer.FlushAsync(cancellationToken);
        await stream.FlushAsync(cancellationToken);

        return stream;
    }
}
