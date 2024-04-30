﻿namespace vm2.ExpressionSerialization.XmlTransform;
/// <summary>
/// Class Options transforms C# identifiers to XML names.
/// </summary>
public partial class Options
{
    /// <summary>
    /// The expression serialization XML schema
    /// </summary>
    public const string Exs = "urn:schemas-vm-com:Linq.Expressions.Serialization";

    /// <summary>
    /// The W3C schema definition.
    /// </summary>
    public const string Xsd = "http://www.w3.org/2001/XMLSchema";

    /// <summary>
    /// The W3C instance schema definition.
    /// </summary>
    public const string Xsi = "http://www.w3.org/2001/XMLSchema-instance";

    /// <summary>
    /// The Microsoft serialization schema definition.
    /// </summary>
    public const string Ser = "http://schemas.microsoft.com/2003/10/Serialization/";

    /// <summary>
    /// The SOAP data contracts.
    /// </summary>
    public const string Dcs = "http://schemas.datacontract.org/2004/07/System";

    /// <summary>
    /// Gets the schemas.
    /// </summary>
    /// <value>The schemas.</value>
    public static XmlSchemaSet Schemas => _schemas;
    static readonly ReaderWriterLockSlim _lock = new();
    static readonly XmlSchemaSet _schemas = new();

    /// <summary>
    /// Sets the schema location.
    /// </summary>
    /// <param name="schema">The schema.</param>
    /// <param name="location">The location.</param>
    /// <returns><c>true</c> if the schema was added successfully, <c>false</c> if the schema has been already added.</returns>
    public static bool SetSchemaLocation(string schema, string? location)
    {
        using (_lock.UpgradableReaderLock())
        {
            if (_schemas.Contains(schema))
                return false;

            using (_lock.WriterLock())
            {
                using var reader = new XmlTextReader(location ?? schema);
                _schemas.Add(schema, reader);
                return true;
            }
        }
    }

    string _characterEncoding = "utf-8";

    /// <summary>
    /// Gets or sets the serialized document encoding.
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
    /// Gets or sets a value indicating the endianness of the (de)serialized document.
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
    /// Gets the identifiers transformation convention.
    /// </summary>
    /// <value>The identifiers case convention.</value>
    public IdentifierConventions Identifiers { get; set; } = IdentifierConventions.Preserve;

    /// <summary>
    /// Gets the type transform convention.
    /// </summary>
    /// <value>The type transform convention.</value>
    public TypeNameConventions TypeNames { get; set; } = TypeNameConventions.FullName;

    /// <summary>
    /// Gets or sets the size of the document's tab indention.
    /// </summary>
    /// <value>The size of the indent.</value>
    public bool Indent { get; set; } = true;

    /// <summary>
    /// Gets or sets the size of the document's tab indention.
    /// </summary>
    /// <value>The size of the indent.</value>
    public int IndentSize { get; set; } = 2;

    /// <summary>
    /// Outputs all XML attribute on a new line - below their XML element and indented.
    /// </summary>
    /// <value><c>true</c> if output the attributes on a new line; otherwise, <c>false</c>.</value>
    public bool AttributesOnNewLine { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to add comments to the resultant node.
    /// </summary>
    /// <value><c>true</c> if comments are to be added; otherwise, <c>false</c>.</value>
    public bool AddComments { get; set; } = false;
}
