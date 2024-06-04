namespace vm2.ExpressionSerialization.Abstractions;

static class DocumentOptionsExtensions
{
    /// <summary>
    /// Transforms the <paramref name="type"/> to a readable string according to the <see cref="DocumentOptions.TypeNames"/> convention.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="type">The type to be transformed to a readable string.</param>
    /// <returns>The human readable transformation of the parameter <paramref name="type"/>.</returns>
    internal static string TransformTypeName(this DocumentOptions options, Type type)
        => Transform.TypeName(type, options.TypeNames);

    /// <summary>
    /// Transforms the <paramref name="identifier"/> according to the <see cref="DocumentOptions.Identifiers"/> conventions.
    /// </summary>
    /// <param name="options">The options instance being extended.</param>
    /// <param name="identifier">The identifier to be transformed.</param>
    /// <returns>The transformed <paramref name="identifier"/>.</returns>
    internal static string TransformIdentifier(this DocumentOptions options, string identifier)
        => Transform.Identifier(identifier, options.Identifiers);

    /// <summary>
    /// Determines whether to validate the input documents against has expressions schema.
    /// If <paramref name="options"/> <see cref="DocumentOptions.ValidateInputDocuments"/> is <c>true</c> the method will verify
    /// that the schema was actually added.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <returns><c>true</c> if [has expressions schema] [the specified options]; otherwise, <c>false</c>.</returns>
    /// <exception cref="System.InvalidOperationException">
    /// The expressions schema was not added to the XmlOptions.Schema - use XmlOptions.SetSchemaLocation().
    /// </exception>
    internal static bool MustValidate(this DocumentOptions options)
    {
        if (options.ValidateInputDocuments == ValidateDocuments.Always)
        {
            if (!options.HasExpressionsSchema)
                throw new InvalidOperationException("The expressions schema was not added to the XmlOptions.Schema - use XmlOptions.SetSchemaLocation().");
            return true;
        }
        else
            return options.ValidateInputDocuments == ValidateDocuments.IfSchemaPresent && options.HasExpressionsSchema;
    }
}
