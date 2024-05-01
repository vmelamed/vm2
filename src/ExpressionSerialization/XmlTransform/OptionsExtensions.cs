namespace vm2.ExpressionSerialization.XmlTransform;
/// <summary>
/// Extends the objects of class Options with some useful internal extensions.
/// </summary>
static class OptionsExtensions
{
    /// <summary>
    /// Gets the set  XML document encoding.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <returns>The document encoding.</returns>
    internal static Encoding GetEncoding(this Options options)
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
    internal static XDeclaration? DocumentDeclaration(this Options options)
        => options.AddDocumentDeclaration
                        ? new XDeclaration("1.0", options.CharacterEncoding, null)
                        : null;

    /// <summary>
    /// Builds an XML comment object with the specified comment text if comments are enabled.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="comment">The comment.</param>
    /// <returns>The comment object System.Nullable&lt;XComment&gt;.</returns>
    internal static XComment? Comment(this Options options, string comment)
        => options.AddComments
                    ? new XComment(comment)
                    : null;

    /// <summary>
    /// Builds an XML comment object with the text of the expression if comments are enabled.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="expression">The expression.</param>
    /// <returns>System.Nullable&lt;XComment&gt;.</returns>
    internal static XComment? Comment(this Options options, Expression expression)
        => options.AddComments
                    ? options.Comment($" {expression} ")
                    : null;

    /// <summary>
    /// Adds the expression comment to the specified XML container if comments are enabled.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="expression">The expression.</param>
    internal static void AddComment(this Options options, XContainer parent, Expression expression)
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
    internal static void AddComment(this Options options, XContainer parent, string comment)
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
    internal static XComment? TypeComment(this Options options, Type type)
        => options.TypeNames != TypeNameConventions.AssemblyQualifiedName &&
           (!type.IsBasicType() || type.IsEnum)
                ? options.Comment($" {Transform.TypeName(type, options.TypeNames)} ")
                : null;

    /// <summary>
    /// Transforms the <paramref name="type"/> to a readable string according to the <see cref="Options.TypeNames"/> convention.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="type">The type to be transformed to a readable string.</param>
    /// <returns>The human readable transformation of the parameter <paramref name="type"/>.</returns>
    internal static string TransformTypeName(this Options options, Type type)
        => Transform.TypeName(type, options.TypeNames);

    /// <summary>
    /// Transforms the <paramref name="identifier"/> according to the <see cref="Options.Identifiers"/> conventions.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="identifier">The identifier to be transformed.</param>
    /// <returns>The transformed <paramref name="identifier"/>.</returns>
    internal static string TransformIdentifier(this Options options, string identifier)
        => Transform.Identifier(identifier, options.Identifiers);

    /// <summary>
    /// Determines whether has expressions schema <see cref="Options.Exs"/> was added to the <see cref="Options.Schemas"/>.
    /// </summary>
    /// <returns><c>true</c> if [has expressions schema] [the specified options]; otherwise, <c>false</c>.</returns>
    internal static bool HasExpressionsSchema(this Options _)
        => Options.Schemas.Contains(Options.Exs);

    /// <summary>
    /// Determines whether to validate the input XML documents against has expressions schema <see cref="Options.Exs"/>.
    /// If <paramref name="options"/> <see cref="Options.ValidateInputDocuments"/> is <c>true</c> the method will verify
    /// that the schema was actually added.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <returns><c>true</c> if [has expressions schema] [the specified options]; otherwise, <c>false</c>.</returns>
    /// <exception cref="System.InvalidOperationException">
    /// The expressions schema was not added to the Options.Schema - use Options.SetSchemaLocation().
    /// </exception>
    internal static bool MustValidate(this Options options)
    {
        if (options.ValidateInputDocuments == ValidateDocuments.Always)
        {
            if (!options.HasExpressionsSchema())
                throw new InvalidOperationException("The expressions schema was not added to the Options.Schema - use Options.SetSchemaLocation().");
            return true;
        }
        else
            return options.ValidateInputDocuments == ValidateDocuments.IfSchemaPresent && options.HasExpressionsSchema();
    }
}
