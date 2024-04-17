namespace vm2.ExpressionSerialization.Conventions;

/// <summary>
/// Enum IdentifierConventions specify how to transform C# identifiers to XML names.
/// </summary>
public enum IdentifierConventions
{
    /// <summary>
    /// Transform identifiers to camel-case convention: ThisIsName -> ThisIsName
    /// </summary>
    Preserve,
    /// <summary>
    /// Transform identifiers to camel-case convention: ThisIsName -> thisIsName
    /// </summary>
    Camel,
    /// <summary>
    /// Transform identifiers to camel-case convention: thisIsName -> ThisIsName
    /// </summary>
    Pascal,
    /// <summary>
    /// Transform identifiers to snake-lower-case convention: ThisIsName -> this_is_name
    /// </summary>
    SnakeLower,
    /// <summary>
    /// Transform identifiers to snake-lower-case convention: ThisIsName -> THIS_IS_NAME
    /// </summary>
    SnakeUpper,
    /// <summary>
    /// Transform identifiers to snake-lower-case convention: ThisIsName -> this-is-name
    /// </summary>
    KebabLower,
    /// <summary>
    /// Transform identifiers to snake-lower-case convention: ThisIsName -> THIS-IS-NAME
    /// </summary>
    KebabUpper,
}
