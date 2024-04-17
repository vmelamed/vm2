namespace vm2.ExpressionSerialization.XmlTransform;

using System.Xml.Linq;

/// <summary>
/// Class ExpressionTransform.
/// Implements the <see cref="IExpressionTransform{XNode}"/>: transforms a Linq expression to an XML Node object.
/// </summary>
/// <seealso cref="IExpressionTransform{XNode}" />
public class ExpressionTransform : IExpressionTransform<XNode>
{
    Options _options;
    ExpressionVisitor _visitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionTransform"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public ExpressionTransform(Options? options = null)
    {
        _options = options ?? new Options();
        _visitor = new ExpressionVisitor(_options);
    }

    /// <summary>
    /// Transform the specified expression to a document model node type `XNode` (XML).
    /// </summary>
    /// <param name="expression">The expression to be transformed.</param>
    /// <returns>The resultant top level document model node `XNode`.</returns>
    public XNode Transform(Expression expression)
    {
        _visitor.Visit(expression);
        return new XElement(
                        ElementNames.Expression,
                        new XAttribute("xmlns", Namespaces.Exs),
                        //new XAttribute(XNamespace.Xmlns + "xs", Namespaces.Xsd),
                        new XAttribute(XNamespace.Xmlns + "i", Namespaces.Xsi),
                        _visitor.Result);
    }

    /// <summary>
    /// Transform the specified expression to a document model node type `XNode` (XML).
    /// </summary>
    /// <param name="expression">The expression to be transformed.</param>
    /// <returns>The resultant top level document model node `XNode`.</returns>
    public XDocument ToDocument(Expression expression)
        => new(
            _options.DocumentDeclaration,
            _options.Comment(expression),
            Transform(expression));

    /// <summary>
    /// Serializes the specified expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="stream">The stream.</param>
    /// <returns>Stream.</returns>
    public Stream Serialize(
        Expression expression,
        Stream stream)
    {
        var doc = ToDocument(expression);
        var settings = new XmlWriterSettings() {
            Encoding = _options.GetEncoding(),
            Indent = _options.Indent,
            IndentChars = new(' ', _options.IndentSize),
            NamespaceHandling = _options.OmitDuplicateNamespaces ? NamespaceHandling.OmitDuplicates : NamespaceHandling.Default,
            NewLineOnAttributes = _options.AttributesOnNewLine,
            OmitXmlDeclaration = !_options.AddDocumentDeclaration,
            WriteEndDocumentOnClose = true,
        };
        using var writer = new StreamWriter(stream, _options.GetEncoding());
        using var xmlWriter = XmlWriter.Create(writer, settings);

        doc.WriteTo(xmlWriter);
        xmlWriter.Flush();
        writer.Flush();
        stream.Flush();

        return stream;
    }

    /// <summary>
    /// Serialize as an asynchronous operation.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="stream">The stream.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;Stream&gt; representing the asynchronous operation.</returns>
    public async Task<Stream> SerializeAsync(
        Expression expression,
        Stream stream,
        CancellationToken cancellationToken = default)
    {
        var doc = ToDocument(expression);
        var encoding = _options.GetEncoding();
        var settings = new XmlWriterSettings() {
            Async = true,
            Encoding = encoding,
            Indent = _options.Indent,
            IndentChars = new(' ', _options.IndentSize),
            NamespaceHandling = _options.OmitDuplicateNamespaces ? NamespaceHandling.OmitDuplicates : NamespaceHandling.Default,
            NewLineOnAttributes = _options.AttributesOnNewLine,
            OmitXmlDeclaration = !_options.AddDocumentDeclaration,
            WriteEndDocumentOnClose = true,
        };
        using var writer = new StreamWriter(stream, encoding);
        using var xmlWriter = XmlWriter.Create(writer, settings);

        await doc.WriteToAsync(xmlWriter, cancellationToken);
        await xmlWriter.FlushAsync();
        await writer.FlushAsync(cancellationToken);
        await stream.FlushAsync(cancellationToken);

        return stream;
    }
}
