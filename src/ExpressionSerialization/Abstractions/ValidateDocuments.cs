namespace vm2.ExpressionSerialization.Abstractions;
/// <summary>
/// Enum ValidateDocuments specifies whether to validate the input XML documents that are to be transformed to <see cref="Expression"/>-s.
/// </summary>
public enum ValidateDocuments
{
    /// <summary>
    /// Do not validate the input - improves performance by skipping the validation. Using only when you are certain that
    /// the document is coming from a trusted source.
    /// </summary>
    Never,

    /// <summary>
    /// Always validate the input documents. Requires that the schema &quot;urn:schemas-vm-com:Linq.Expressions.Serialization&quot;
    /// is added to <see cref="XmlTransform.XmlOptions.Schemas"/>. If the schema is not added, this option will cause throwing an exception.
    /// </summary>
    Always,

    /// <summary>
    /// If the schema &quot;urn:schemas-vm-com:Linq.Expressions.Serialization&quot; is present in the 
    /// <see cref="XmlTransform.XmlOptions.Schemas"/> the transform will validate all input documents. Otherwise will quietly proceed 
    /// with the transform process.
    /// </summary>
    IfSchemaPresent,
}
