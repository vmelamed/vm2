namespace vm2.ExpressionSerialization.Shared.Conventions;

/// <summary>
/// Enum IdentifierConventions specify how to transform C# identifiers to XML names.
/// </summary>
public enum IdentifierConventions
{
    /// <summary>
    /// Transformer identifiers to camel-case convention: ThisIsName -> ThisIsName
    /// </summary>
    Preserve,
    /// <summary>
    /// Transformer identifiers to camel-case convention: ThisIsName -> thisIsName
    /// </summary>
    Camel,
    /// <summary>
    /// Transformer identifiers to camel-case convention: thisIsName -> ThisIsName
    /// </summary>
    Pascal,
    /// <summary>
    /// Transformer identifiers to snake-lower-case convention: ThisIsName -> this_is_name
    /// </summary>
    SnakeLower,
    /// <summary>
    /// Transformer identifiers to snake-lower-case convention: ThisIsName -> THIS_IS_NAME
    /// </summary>
    SnakeUpper,
    /// <summary>
    /// Transformer identifiers to snake-lower-case convention: ThisIsName -> this-is-name
    /// </summary>
    KebabLower,
    /// <summary>
    /// Transformer identifiers to snake-lower-case convention: ThisIsName -> THIS-IS-NAME
    /// </summary>
    KebabUpper,
}
