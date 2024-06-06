namespace vm2.ExpressionSerialization.Abstractions;

/// <summary>
/// Options for serializing/deserializing documents.
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

    /// <summary>
    /// Transforms the <paramref name="type"/> to a readable string according to the <see cref="DocumentOptions.TypeNames"/> convention.
    /// </summary>
    /// <param name="type">The type to be transformed to a readable string.</param>
    /// <returns>The human readable transformation of the parameter <paramref name="type"/>.</returns>
    internal string TransformTypeName(Type type)
        => Transform.TypeName(type, TypeNames);

    /// <summary>
    /// Transforms the <paramref name="identifier"/> according to the <see cref="DocumentOptions.Identifiers"/> conventions.
    /// </summary>
    /// <param name="identifier">The identifier to be transformed.</param>
    /// <returns>The transformed <paramref name="identifier"/>.</returns>
    internal string TransformIdentifier(string identifier)
        => Transform.Identifier(identifier, Identifiers);

    /// <summary>
    /// Determines whether to validate the input documents against has expressions schema.
    /// If <see cref="DocumentOptions.ValidateInputDocuments"/> is <c>ValidateDocuments.Always</c> the method will verify
    /// that the schema was actually added.
    /// </summary>
    /// <returns><c>true</c> if the documents must be validated against the schema; otherwise, <c>false</c>.</returns>
    /// <exception cref="InvalidOperationException">
    /// The expressions schema was not added to the XmlOptions.Schema - use XmlOptions.SetSchemaLocation().
    /// </exception>
    internal bool MustValidate
        => ValidateInputDocuments == ValidateDocuments.Always
                ? HasExpressionsSchema ? true : throw new InvalidOperationException("The expressions schema was not added to the XmlOptions.Schema - use XmlOptions.SetSchemaLocation().")
                : ValidateInputDocuments == ValidateDocuments.IfSchemaPresent && HasExpressionsSchema;
}
