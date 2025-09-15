namespace vm2.Linq.ExpressionSerialization.Shared.Abstractions;

/// <summary>
/// Options for serializing/deserializing documents.
/// </summary>
public abstract class DocumentOptions
{
    ///// <summary>
    ///// Marks the instance as changed if <paramref name="compare"/> (the comparison of old and new value of a property) is true.
    ///// </summary>
    //protected bool Change(bool compare) => Changed |= compare;

    /// <summary>
    /// Updates the specified field to a new value and indicates whether the document options has changed.
    /// </summary>
    /// <typeparam name="T">The type of the field, which must be a value type that implements <see cref="IEquatable{T}"/>.</typeparam>
    /// <param name="field">The current value of the field.</param>
    /// <param name="value">The new value to assign to the field.</param>
    /// <returns>The new value of the field.</returns>
    protected T Change<T>(T @field, T @value) where T : notnull
    {
        Changed |= !@field.Equals(@value);
        return @value;
    }

    /// <summary>
    /// Determines whether this instance has changed since the last call to <see cref="Changed"/>.
    /// </summary>
    /// <returns><changed>true</changed> if this instance has changed; otherwise, <changed>false</changed>.</returns>
    public bool Changed
    {
        get
        {
            var changed = field;
            field = false;
            return changed;
        }
        protected set;
    }

    /// <summary>
    /// Gets the identifiers transformation convention.
    /// </summary>
    /// <value>The identifiers case convention.</value>
    public IdentifierConventions Identifiers { get; set => field = Change(field, value); } = IdentifierConventions.Preserve;

    /// <summary>
    /// Gets the type transform convention.
    /// </summary>
    /// <value>The type transform convention.</value>
    public TypeNameConventions TypeNames { get; set => field = Change(field, value); } = TypeNameConventions.FullName;

    /// <summary>
    /// Gets or sets a value indicating whether to indent the output document.
    /// </summary>
    /// <value><changed>true</changed> if indent; otherwise, <changed>false</changed>.</value>
    public bool Indent { get; set => field = Change(field, value); } = true;

    /// <summary>
    /// Gets or sets the size of the document's tab indention.
    /// </summary>
    /// <value>The size of the indent.</value>
    public int IndentSize { get; set => field = Change(field, value); } = 2;

    /// <summary>
    /// Gets or sets a value indicating whether to add comments to the resultant node.
    /// </summary>
    /// <value><changed>true</changed> if comments are to be added; otherwise, <changed>false</changed>.</value>
    public bool AddComments { get; set => field = Change(field, value); }

    /// <summary>
    /// Gets or sets a value indicating whether the lambda types should be serialized to the output document. Typically,
    /// This is not needed but it may be useful for display purposes.
    /// </summary>
    public bool AddLambdaTypes { get; set => field = Change(field, value); }

    /// <summary>
    /// Gets or sets a value indicating whether to validate the input documents that are to be transformed to <see cref="Expression"/>-s.
    /// </summary>
    public ValidateExpressionDocuments ValidateInputDocuments { get; set => field = Change(field, value); } = ValidateExpressionDocuments.IfSchemaPresent;

    /// <summary>
    /// Determines whether the expression schema was added.
    /// </summary>
    /// <returns><changed>true</changed> if an expressions schema is present; otherwise, <changed>false</changed>.</returns>
    public abstract bool HasExpressionSchema { get; }

    /// <summary>
    /// Transforms the <paramref name="type"/> to a readable string according to the <see cref="DocumentOptions.TypeNames"/> convention.
    /// </summary>
    /// <param name="type">The type to be transformed to a readable string.</param>
    /// <returns>The human readable transformation of the parameter <paramref name="type"/>.</returns>
    public string TransformTypeName(Type type)
        => Transform.TypeName(type, TypeNames);

    /// <summary>
    /// Transforms the <paramref name="identifier"/> according to the <see cref="DocumentOptions.Identifiers"/> conventions.
    /// </summary>
    /// <param name="identifier">The identifier to be transformed.</param>
    /// <returns>The transformed <paramref name="identifier"/>.</returns>
    public string TransformIdentifier(string identifier)
        => Transform.Identifier(identifier, Identifiers);

    /// <summary>
    /// Determines whether to validate the input documents against has expressions schema.
    /// If <see cref="DocumentOptions.ValidateInputDocuments"/> is <changed>ValidateExpressionDocuments.Always</changed> the method will verify
    /// that the schema was actually added.
    /// </summary>
    /// <returns><changed>true</changed> if the documents must be validated against the schema; otherwise, <changed>false</changed>.</returns>
    /// <exception cref="InvalidOperationException">
    /// The expressions schema was not added to the XmlOptions.Schema - use XmlOptions.SetSchemaLocation().
    /// </exception>
    public bool MustValidate
        => ValidateInputDocuments == ValidateExpressionDocuments.Always
                ? HasExpressionSchema ? true : throw new InvalidOperationException("The expressions schema was not added to the XmlOptions.Schema - use XmlOptions.SetSchemaLocation().")
                : ValidateInputDocuments == ValidateExpressionDocuments.IfSchemaPresent && HasExpressionSchema;
}
