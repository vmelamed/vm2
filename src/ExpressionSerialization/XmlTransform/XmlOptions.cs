namespace vm2.ExpressionSerialization.XmlTransform;

/// <summary>
/// Class XmlOptions holds options that control certain aspects of the transformations to/from LINQ expressions from/to 
/// XML documents.
/// </summary>
public partial class XmlOptions : DocumentOptions
{
    /// <summary>
    /// The expression transformation XML schemaUri
    /// </summary>
    public const string Exs = "urn:schemas-vm-com:Linq.Expressions.Serialization.Xml";

    /// <summary>
    /// The W3C schemaUri definition.
    /// </summary>
    public const string Xsd = "http://www.w3.org/2001/XMLSchema";

    /// <summary>
    /// The W3C instance schemaUri definition.
    /// </summary>
    public const string Xsi = "http://www.w3.org/2001/XMLSchema-instance";

    /// <summary>
    /// The Microsoft serialization schemaUri definition.
    /// </summary>
    public const string Ser = "http://schemas.microsoft.com/2003/10/Serialization/";

    /// <summary>
    /// The SOAP data contracts.
    /// </summary>
    public const string Dcs = "http://schemas.datacontract.org/2004/07/System";

    static readonly object _sync = new();

    /// <summary>
    /// Gets the schemas.
    /// </summary>
    /// <value>The schemas.</value>
    public static XmlSchemaSet Schemas { get; private set; } = new();

    /// <summary>
    /// Resets the schemas.
    /// </summary>
    public static void ResetSchemas()
    {
        lock (_sync)
            Schemas = new();
    }

    /// <summary>
    /// Sets the schemaUri path.
    /// </summary>
    /// <param name="schemaUri">The schema identifier (most likely <see cref="Exs"/> which is not URL).</param>
    /// <param name="url">The location of the schema file.</param>
    public static void SetSchemaLocation(string schemaUri, string? url)
    {
        lock (_sync)
        {
            if (Schemas.Contains(schemaUri))
                return;

            using var reader = new XmlTextReader(url ?? schemaUri);
            Schemas.Add(schemaUri, reader);
        }
    }

    string _characterEncoding = "utf-8";

    /// <summary>
    /// Gets or sets the transformed document encoding.
    /// </summary>
    /// <value>The encoding.</value>
    public string CharacterEncoding
    {
        get => _characterEncoding;
        set
        {
            _characterEncoding = value.ToUpperInvariant() switch {
                "ASCII" => "ascii",
                "UTF-8" => "utf-8",
                "UTF-16" => "utf-16",
                "UTF-32" => "utf-32",
                "ISO-8859-1" or
                "LATIN1" => "iso-8859-1",
                _ => throw new NotSupportedException($@"The encoding ""{CharacterEncoding}"" is not supported." +
                                    @"The supported character encodings are: ""ascii"", ""utf-8"", ""utf-16"", ""utf-32"", and ""iso-8859-1"" (or ""Latin1"")."),
            };
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to put in the output stream a byte order mark (BOM). Not recommended for UTF-8.
    /// </summary>
    /// <value><c>true</c> if byte order mark should be added; otherwise, <c>false</c>.</value>
    public bool ByteOrderMark { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating the endianness of the transformed document.
    /// </summary>
    /// <value><c>true</c> if it must be big endian; otherwise, <c>false</c>.</value>
    public bool BigEndian { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to add an XML document declaration.
    /// </summary>
    /// <value><c>true</c> if XML document declaration should be added; otherwise, <c>false</c>.</value>
    public bool AddDocumentDeclaration { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to eliminate duplicate namespaces.
    /// </summary>
    /// <value><c>true</c> if duplicate namespaces are to be omitted; otherwise, <c>false</c>.</value>
    public bool OmitDuplicateNamespaces { get; set; } = true;

    /// <summary>
    /// Outputs all XML attribute on a new line - below their XML element and indented.
    /// </summary>
    /// <value><c>true</c> if output the attributes on a new line; otherwise, <c>false</c>.</value>
    public bool AttributesOnNewLine { get; set; } = false;

    /// <summary>
    /// Determines whether the expressions schemaUri <see cref="Exs"/> was added.
    /// </summary>
    /// <returns><c>true</c> if [has expressions schemaUri] [the specified options]; otherwise, <c>false</c>.</returns>
    internal override bool HasExpressionsSchema => Schemas.Contains(Exs);
}
