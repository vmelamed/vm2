namespace vm2.ExpressionSerialization.XmlTransform;

/// <summary>
/// Extends the objects of class XmlOptions with some useful internal extensions.
/// </summary>
static class OptionsExtensions
{
    /// <summary>
    /// Gets the set  XML document encoding.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <returns>The document encoding.</returns>
    internal static Encoding GetEncoding(this XmlOptions options)
        => options.CharacterEncoding switch {
            "ascii" => Encoding.ASCII,
            "utf-8" => new UTF8Encoding(options.ByteOrderMark, true),
            "utf-16" => new UnicodeEncoding(options.BigEndian, options.ByteOrderMark, true),
            "utf-32" => new UTF32Encoding(options.BigEndian, options.ByteOrderMark, true),
            "iso-8859-1" => Encoding.Latin1,
            _ => throw new NotSupportedException($@"The encoding ""{options.CharacterEncoding}"" is not supported. " +
                                    @"The supported character encodings are: ""ascii"", ""utf-8"", ""utf-16"", ""utf-32"", and ""iso-8859-1"" (or ""Latin1"")."),
        };

    /// <summary>
    /// Gets the document declaration from the current options if declarations are enabled.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <returns>The document declaration System.Nullable&lt;XDeclaration&gt;.</returns>
    internal static XDeclaration? DocumentDeclaration(this XmlOptions options)
        => options.AddDocumentDeclaration
                        ? new XDeclaration("1.0", options.CharacterEncoding, null)
                        : null;

    /// <summary>
    /// Builds an XML comment object with the specified comment text if comments are enabled.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="comment">The comment.</param>
    /// <returns>The comment object System.Nullable&lt;XComment&gt;.</returns>
    internal static XComment? Comment(this XmlOptions options, string comment)
        => options.AddComments
                    ? new XComment(comment)
                    : null;

    /// <summary>
    /// Builds an XML comment object with the text of the expression if comments are enabled.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="expression">The expression.</param>
    /// <returns>System.Nullable&lt;XComment&gt;.</returns>
    internal static XComment? Comment(this XmlOptions options, Expression expression)
        => options.AddComments
                    ? options.Comment($" {expression} ")
                    : null;

    /// <summary>
    /// Adds the expression comment to the specified XML container if comments are enabled.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="expression">The expression.</param>
    internal static void AddComment(this XmlOptions options, XContainer parent, Expression expression)
    {
        if (options.AddComments)
            parent.Add(new XComment($" {expression} "));
    }

    /// <summary>
    /// Adds the comment to the specified XML container if comments are enabled.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="comment">The comment.</param>
    internal static void AddComment(this XmlOptions options, XContainer parent, string comment)
    {
        if (options.AddComments)
            parent.Add(new XComment($" {comment} "));
    }

    /// <summary>
    /// Creates an <see cref="XComment" /> with the human readable name of the <paramref name="type" /> if comments are enabled.
    /// The type must be non-basic type.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="type">The type.</param>
    /// <returns>The comment as System.Nullable&lt;XComment&gt;.</returns>
    internal static XComment? TypeComment(this XmlOptions options, Type type)
        => options.TypeNames != TypeNameConventions.AssemblyQualifiedName &&
           (!type.IsBasicType() && type != typeof(object) || type.IsEnum)
                ? options.Comment($" {Transform.TypeName(type, options.TypeNames)} ")
                : null;
}
