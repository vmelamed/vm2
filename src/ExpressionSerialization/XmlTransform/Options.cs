namespace vm2.XmlExpressionSerialization.XmlTransform;

using System.Linq.Expressions;

using vm2.XmlExpressionSerialization.Conventions;
using vm2.XmlExpressionSerialization.Utilities;

/// <summary>
/// Class Options transforms C# identifiers to XML names.
/// </summary>
public partial class Options
{
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

    #region Internal interface
    /// <summary>
    /// Gets the encoding.
    /// </summary>
    /// <value>The encoding.</value>
    internal Encoding GetEncoding()
        => CharacterEncoding switch {
            "ascii" => Encoding.ASCII,
            "utf-8" => new UTF8Encoding(ByteOrderMark, true),
            "utf-16" => new UnicodeEncoding(BigEndian, ByteOrderMark, true),
            "utf-32" => new UTF32Encoding(BigEndian, ByteOrderMark, true),
            "iso-8859-1" => Encoding.Latin1,
            _ => throw new NotSupportedException($@"The encoding ""{CharacterEncoding}"" is not supported. " +
                                    @"The supported character encodings are: ""ascii"", ""utf-8"", ""utf-16"", ""utf-32"", and ""iso-8859-1"" (or ""Latin1"")."),
        };

    /// <summary>
    /// Gets the document declaration from the current options.
    /// </summary>
    /// <value>The document declaration.</value>
    internal XDeclaration? DocumentDeclaration => AddDocumentDeclaration ? new XDeclaration("1.0", CharacterEncoding, null) : null;

    /// <summary>
    /// Builds an XML comment object with the specified comment text.
    /// </summary>
    /// <param name="comment">The comment.</param>
    /// <returns>System.Nullable&lt;XComment&gt;.</returns>
    internal XComment? Comment(string comment)
        => AddComments ? new XComment(comment) : null;

    /// <summary>
    /// Builds an XML comment object with the specified comment text.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>System.Nullable&lt;XComment&gt;.</returns>
    internal XComment? Comment(Expression expression)
        => AddComments ? Comment($" {expression} ") : null;

    /// <summary>
    /// Adds the comment.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="expression">The expression.</param>
    internal void AddComment(XContainer parent, Expression expression)
    {
        if (AddComments)
            parent.Add(new XComment($" {expression} "));
    }

    /// <summary>
    /// Adds the comment.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="comment">The comment.</param>
    internal void AddComment(XContainer parent, string comment)
    {
        if (AddComments)
            parent.Add(new XComment($" {comment} "));
    }

    /// <summary>
    /// Creates an <see cref="XComment"/> with the human readable name of the <paramref name="type"/> as a comment. 
    /// The type must be non-basic type.
    /// </summary>
    /// <param name="type">The type.</param>
    internal XComment? TypeComment(Type type)
        => TypeNames != TypeNameConventions.AssemblyQualifiedName &&
           (!type.IsBasicType() || type.IsEnum)
                ? Comment($" {Transform.TypeName(type, TypeNames)} ")
                : null;

    /// <summary>
    /// Transform the type c a string according c the <see cref="TypeNames"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>System.String.</returns>
    internal string TransformTypeName(Type type)
        => Transform.TypeName(type, TypeNames);

    /// <summary>
    /// Transform the identifier according c the <see cref="Identifiers"/>.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    /// <returns>string.</returns>
    /// <exception cref="InternalTransformErrorException">Invalid identifier.</exception>
    internal string TransformIdentifier(string identifier)
        => Transform.Identifier(identifier, Identifiers);
    #endregion
}
