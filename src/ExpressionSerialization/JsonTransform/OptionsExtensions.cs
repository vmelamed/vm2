namespace vm2.ExpressionSerialization.JsonTransform;

static class OptionsExtensions
{
    /// <summary>
    /// Builds an XML comment object with the specified comment text if comments are enabled.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="comment">The comment.</param>
    /// <returns>The comment object System.Nullable&lt;XComment&gt;.</returns>
    internal static JElement? Comment(this JsonOptions options, string comment)
        => options.AddComments
                    ? new JElement(Vocabulary.Comment, comment)
                    : null;

    /// <summary>
    /// Builds an XML comment object with the text of the expression if comments are enabled.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="expression">The expression.</param>
    /// <returns>System.Nullable&lt;XComment&gt;.</returns>
    internal static JElement? Comment(this JsonOptions options, Expression expression)
        => options.AddComments
                    ? options.Comment($" {expression} ")
                    : null;

    /// <summary>
    /// Adds the expression comment to the specified XML container if comments are enabled.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="expression">The expression.</param>
    internal static void AddComment(this JsonOptions options, JsonObject parent, Expression expression)
    {
        if (options.AddComments)
            parent.Add(Vocabulary.Comment, $" {expression} ");
    }

    /// <summary>
    /// Adds the comment to the specified XML container if comments are enabled.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="comment">The comment.</param>
    internal static void AddComment(this JsonOptions options, JElement parent, string comment)
    {
        if (options.AddComments)
            parent.Add(options.Comment($" {comment} ")!);
    }

    /// <summary>
    /// Adds the expression comment to the specified XML container if comments are enabled.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="expression">The expression.</param>
    internal static void AddComment(this JsonOptions options, JElement parent, Expression expression)
    {
        if (options.AddComments)
            parent.Add(options.Comment($" {expression} ")!);
    }

    /// <summary>
    /// Creates an <see cref="XComment" /> with the human readable name of the <paramref name="type" /> if comments are enabled.
    /// The type must be non-basic type.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="type">The type.</param>
    /// <returns>The comment as System.Nullable&lt;XComment&gt;.</returns>
    internal static JElement? TypeComment(this JsonOptions options, Type type)
        => options.TypeNames != TypeNameConventions.AssemblyQualifiedName &&
           (!type.IsBasicType() && type != typeof(object) || type.IsEnum)
                ? options.Comment($" {Transform.TypeName(type, options.TypeNames)} ")
                : null;

}
