namespace vm2.Linq.ExpressionSerialization.Shared.Abstractions;

/// <summary>
/// Enum ValidateExpressionDocuments specifies whether to validate the input XML or JSON expression documents that are to be transformed to 
/// <see cref="Expression"/>-s.
/// </summary>
public enum ValidateExpressionDocuments
{
    /// <summary>
    /// Do not validate the input - improves performance by skipping the validation. Use only when you are certain that
    /// the document is coming from a trusted source.
    /// </summary>
    Never,

    /// <summary>
    /// Always validate the input documents. Requires that the schema is added to the respective options.
    /// If the schema is not added, this option will cause throwing an exception.
    /// </summary>
    Always,

    /// <summary>
    /// If the schema is present in the respective options, the transform will validate all input documents.
    /// Otherwise will quietly proceed with the transform process.
    /// </summary>
    IfSchemaPresent,
}
