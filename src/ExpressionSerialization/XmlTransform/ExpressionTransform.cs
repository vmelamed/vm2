namespace vm2.ExpressionSerialization.XmlTransform;

using System.Xml;
using System.Xml.Linq;

/// <summary>
/// Class ExpressionTransform.
/// Implements the <see cref="IExpressionTransform{XNode}"/>: transforms a Linq expression to an XML Node object.
/// </summary>
/// <seealso cref="IExpressionTransform{XNode}" />
public class ExpressionTransform : IExpressionTransform<XDocument>, IExpressionTransform<XElement>
{
    Options _options;
    ToXmlTransformVisitor? _expressionVisitor;
    FromXmlTransformVisitor? _xmlVisitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressionTransform"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public ExpressionTransform(Options? options = null)
    {
        _options = options ?? new Options();
        _expressionVisitor = new ToXmlTransformVisitor(_options);
    }

    #region IExpressionTransform<XElement>
    /// <summary>
    /// Transform the specified expression to a document model node type `XNode` (XML).
    /// </summary>
    /// <param name="expression">The expression to be transformed.</param>
    /// <returns>The resultant top level document model node `XNode`.</returns>
    XElement IExpressionTransform<XElement>.Transform(Expression expression)
    {
        _expressionVisitor ??= new ToXmlTransformVisitor(_options);
        _expressionVisitor.Visit(expression);

        return new XElement(
                        ElementNames.Expression,
                        new XAttribute("xmlns", Namespaces.Exs),
                        new XAttribute(XNamespace.Xmlns + "i", Namespaces.Xsi),
                        _expressionVisitor.Result);
    }

    /// <summary>
    /// Transform the specified document model node of type `TDocument` to a LINQ expression.
    /// </summary>
    /// <param name="element">The document node to be transformed.</param>
    /// <returns>The resultant expression.</returns>
    Expression IExpressionTransform<XElement>.Transform(XElement element)
    {
        _xmlVisitor ??= new FromXmlTransformVisitor(_options);
        return Expression.Constant(null);
    }
    #endregion

    #region IExpressionTransform<XDocument>
    /// <summary>
    /// Transform the specified expression to a document model node type `XNode` (XML).
    /// </summary>
    /// <param name="expression">The expression to be transformed.</param>
    /// <returns>The resultant top level document model node `XNode`.</returns>
    public XDocument Transform(Expression expression)
        => new(
            _options.DocumentDeclaration(),
            _options.Comment(expression),
            ((IExpressionTransform<XElement>)this).Transform(expression));

    /// <summary>
    /// Transform the specified document model node of type `TDocument` to a LINQ expression.
    /// </summary>
    /// <param name="document">The document node to be transformed.</param>
    /// <returns>The resultant expression.</returns>
    public Expression Transform(XDocument document)
    {
        return ((IExpressionTransform<XElement>)this).Transform(document.Root ?? new XElement(ElementNames.Object, new XAttribute(AttributeNames.Nil, true)));
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
        var doc = Transform(expression);
        using var writer = new StreamWriter(stream, _options.GetEncoding());
        using var xmlWriter = XmlWriter.Create(writer, new() {
            Encoding = _options.GetEncoding(),
            Indent = _options.Indent,
            IndentChars = new(' ', _options.IndentSize),
            NamespaceHandling = _options.OmitDuplicateNamespaces ? NamespaceHandling.OmitDuplicates : NamespaceHandling.Default,
            NewLineOnAttributes = _options.AttributesOnNewLine,
            OmitXmlDeclaration = !_options.AddDocumentDeclaration,
            WriteEndDocumentOnClose = true,
        });

        doc.WriteTo(xmlWriter);
        xmlWriter.Flush();
        writer.Flush();
        stream.Flush();
    }

    /// <summary>
    /// Serializes the specified expression.
    /// </summary>
    /// <param name="stream">The stream to get the XML document from.</param>
    /// <returns>Stream.</returns>
    public Expression Deserialize(
        Stream stream)
    {
        using var reader = new StreamReader(stream, _options.GetEncoding());
        using var xmlReader = XmlReader.Create(reader, new XmlReaderSettings() {
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
            IgnoreWhitespace = true,
            Schemas = Options.Schemas,
            ValidationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints,
            ValidationType = ValidationType.Schema,
        });

        return Transform(XDocument.Load(xmlReader));
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
        var doc = Transform(expression);
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

    /// <summary>
    /// Deserializes an expression from the specified document.
    /// </summary>
    /// <param name="stream">The stream to get the XML document from.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Stream.</returns>
    public async Task<Expression> DeserializeAsync(
        Stream stream,
        CancellationToken cancellationToken = default)
    {
        using var reader = new StreamReader(stream, _options.GetEncoding());
        using var xmlReader = XmlReader.Create(reader, new XmlReaderSettings() {
            Async = true,
            IgnoreComments = true,
            IgnoreProcessingInstructions = true,
            IgnoreWhitespace = true,
            Schemas = Options.Schemas,
            ValidationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints,
            ValidationType = ValidationType.Schema,
        });

        return Transform(
            await XDocument.LoadAsync(
                xmlReader,
                LoadOptions.SetLineInfo,
                cancellationToken));
    }
}
