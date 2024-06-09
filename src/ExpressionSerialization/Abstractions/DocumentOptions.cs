namespace vm2.ExpressionSerialization.Abstractions;

/// <summary>
/// Options for serializing/deserializing documents.
/// </summary>
public abstract class DocumentOptions
{
    /// <summary>
    /// Flag whether any of the options have changed.
    /// </summary>
    bool _changed = false;

    IdentifierConventions _identifiers = IdentifierConventions.Preserve;
    TypeNameConventions _typeNames = TypeNameConventions.FullName;
    ValidateDocuments _validateInputDocuments = ValidateDocuments.IfSchemaPresent;
    bool _indent = true;
    int _indentSize = 2;
    bool _addComments = false;

    /// <summary>
    /// Marks the instance as changed if <paramref name="compare"/> (the comparison of old and new value of a property) is true.
    /// </summary>
    protected bool Changed(bool compare) => _changed |= compare;

    /// <summary>
    /// Determines whether this instance has changed since the last call to <see cref="HasChanged"/> and then marks the
    /// instance as not-changed.
    /// </summary>
    /// <returns><c>true</c> if this instance has changed; otherwise, <c>false</c>.</returns>
    public bool HasChanged()
    {
        var c = _changed;
        _changed = false;
        return c;
    }

    /// <summary>
    /// Gets the identifiers transformation convention.
    /// </summary>
    /// <value>The identifiers case convention.</value>
    public IdentifierConventions Identifiers
    {
        get => _identifiers;
        set
        {
            if (Changed(_identifiers != value))
                _identifiers = value;
        }
    }

    /// <summary>
    /// Gets the type transform convention.
    /// </summary>
    /// <value>The type transform convention.</value>
    public TypeNameConventions TypeNames
    {
        get => _typeNames;
        set
        {
            if (Changed(_typeNames != value))
                _typeNames = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to indent the output document.
    /// </summary>
    /// <value><c>true</c> if indent; otherwise, <c>false</c>.</value>
    public bool Indent
    {
        get => _indent;
        set
        {
            if (Changed(_indent != value))
                _indent = value;
        }
    }

    /// <summary>
    /// Gets or sets the size of the document's tab indention.
    /// </summary>
    /// <value>The size of the indent.</value>
    public int IndentSize
    {
        get => _indentSize;
        set
        {
            if (Changed(_indentSize != value))
                _indentSize = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to add comments to the resultant node.
    /// </summary>
    /// <value><c>true</c> if comments are to be added; otherwise, <c>false</c>.</value>
    public bool AddComments
    {
        get => _addComments;
        set
        {
            if (Changed(_addComments != value))
                _addComments = value;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to validate the input documents that are to be transformed to <see cref="Expression"/>-s.
    /// </summary>
    public ValidateDocuments ValidateInputDocuments
    {
        get => _validateInputDocuments;
        set
        {
            if (Changed(_validateInputDocuments != value))
                _validateInputDocuments = value;
        }
    }

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
