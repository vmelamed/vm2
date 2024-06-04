namespace vm2.ExpressionSerialization.Abstractions;

/// <summary>
/// XmlOptions when serializing/deserializing documents.
/// </summary>
public abstract class DocumentOptions
{
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
    /// Gets or sets a value indicating whether to indent the output document.
    /// </summary>
    /// <value><c>true</c> if indent; otherwise, <c>false</c>.</value>
    public bool Indent { get; set; } = true;

    /// <summary>
    /// Gets or sets the size of the document's tab indention.
    /// </summary>
    /// <value>The size of the indent.</value>
    public int IndentSize { get; set; } = 2;

    /// <summary>
    /// Gets or sets a value indicating whether to add comments to the resultant node.
    /// </summary>
    /// <value><c>true</c> if comments are to be added; otherwise, <c>false</c>.</value>
    public bool AddComments { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether to validate the input documents that are to be transformed to <see cref="Expression"/>-s.
    /// </summary>
    public ValidateDocuments ValidateInputDocuments { get; set; } = ValidateDocuments.IfSchemaPresent;

    /// <summary>
    /// Determines whether has expressions schema was added.
    /// </summary>
    /// <returns><c>true</c> if [has expressions schema] [the specified options]; otherwise, <c>false</c>.</returns>
    internal abstract bool HasExpressionsSchema { get; }
}
