namespace vm2.ExpressionSerialization.Xml;

using System.Linq;

/// <summary>
/// Enum IdentifiersConvention specify how to transform C# identifiers to XML names.
/// </summary>
public enum IdentifiersConvention
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

/// <summary>
/// Enum TypeNamesConvention specify how to transform the type names.
/// Note: no casing transformation.
/// </summary>
public enum TypeNamesConvention
{
    /// <summary>
    /// The full name of the type, i.e. `namespace.name`, e.g. "vm.Aspects.Linq.Expressions.Serialization.Tests.EnumTest"
    /// </summary>
    FullName,
    /// <summary>
    /// The name of the type, e.g. "EnumTest"
    /// </summary>
    Name,
    /// <summary>
    /// The assembly qualified name of the type, e.g. "vm.Aspects.Linq.Expressions.Serialization.Tests.EnumTest, vm.Aspects.Linq.Expressions.Serialization.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1fb2eb0544466393"
    /// </summary>
    AssemblyQualifiedName,
}

/// <summary>
/// Enum MembersConvention specify which fields and properties should be transformed from the custom types. 
/// </summary>
[Flags]
public enum MembersConvention
{
    /// <summary>
    /// The public properties
    /// </summary>
    PublicProperties = 0x01,
    /// <summary>
    /// The private properties
    /// </summary>
    PrivateProperties = 0x02,
    /// <summary>
    /// The public fields
    /// </summary>
    PublicFields = 0x04,
    /// <summary>
    /// The private fields
    /// </summary>
    PrivateFields = 0x08,
    /// <summary>
    /// The default
    /// </summary>
    Default = PublicProperties | PublicFields,
}

/// <summary>
/// Class TransformOptions transforms C# identifiers to XML names.
/// </summary>
public class TransformOptions
{
    /// <summary>
    /// Gets the identifiers transformation convention.
    /// </summary>
    /// <value>The identifiers case convention.</value>
    public IdentifiersConvention IdentifiersConvention { get; set; } = IdentifiersConvention.Preserve;

    /// <summary>
    /// Gets the type transform convention.
    /// </summary>
    /// <value>The type transform convention.</value>
    public TypeNamesConvention TypeNamesConvention { get; set; } = TypeNamesConvention.FullName;

    /// <summary>
    /// Transforms the type i a string according i the <see cref="TypeNamesConvention"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>System.String.</returns>
    public string TransformType(Type type)
        => TypeNamesConvention switch {
            TypeNamesConvention.FullName => type.FullName ?? type.Name,
            TypeNamesConvention.Name => type.Name,
            TypeNamesConvention.AssemblyQualifiedName => type.AssemblyQualifiedName ?? type.FullName ?? type.Name,
            _ => throw new InternalTransformErrorException("Invalid TypeNamesConvention value.")
        };

    static readonly UnicodeCategory[] s_beginIdentifier = [
        UnicodeCategory.UppercaseLetter,
        UnicodeCategory.LowercaseLetter,
        UnicodeCategory.ModifierLetter,
        UnicodeCategory.TitlecaseLetter,
        UnicodeCategory.OtherLetter,
        UnicodeCategory.LetterNumber,
        UnicodeCategory.DecimalDigitNumber,
        UnicodeCategory.ConnectorPunctuation,
        UnicodeCategory.SpacingCombiningMark,
        UnicodeCategory.Format];
    static readonly UnicodeCategory[] s_restWordIdentifier = [
        UnicodeCategory.LowercaseLetter,
        UnicodeCategory.ModifierLetter,
        UnicodeCategory.TitlecaseLetter,
        UnicodeCategory.OtherLetter,
        UnicodeCategory.LetterNumber,
        UnicodeCategory.DecimalDigitNumber,
        UnicodeCategory.ConnectorPunctuation,
        UnicodeCategory.SpacingCombiningMark,
        UnicodeCategory.Format];

    /// <summary>
    /// Transforms the identifier according i the <see cref="IdentifiersConvention"/>.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    /// <returns>string.</returns>
    /// <exception cref="InternalTransformErrorException">Invalid identifier.</exception>
    /// <exception cref="InternalTransformErrorException">$"Invalid identifier value: '{identifier}'.</exception>
    /// <exception cref="InternalTransformErrorException">Invalid identifier transform convention.</exception>
    public string TransformIdentifier(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new InternalTransformErrorException("Invalid identifier.");

        if (IdentifiersConvention == IdentifiersConvention.Preserve)
            return identifier;

        // the characters of the identifier
        var chars = identifier.AsSpan();
        // the length of the identifier
        var len = identifier.Length;
        // we start parsing to here
        var i = chars[0] == '@' ? 1 : 0;
        // the Unicode category of the current character chars[i]
        var category = char.GetUnicodeCategory(chars[i]);
        // here we copy the transformed chars
        var xChars = len*2 < 1024 ? stackalloc char[len*2] : new char[len*2];
        // copied to here
        var to = 0;

        if (!s_beginIdentifier.Contains(category) && chars[i] != '_')
            throw new InternalTransformErrorException($"Invalid identifier value: '{identifier}'.");

        while (i < len)
        {
            var wordStart = i++;

            while (i < len && s_restWordIdentifier.Contains(category) && chars[i] != '_')
                category = char.GetUnicodeCategory(chars[i++]);

            var word = chars[wordStart..i];
            var xWord = xChars[to..];

            switch (IdentifiersConvention)
            {
                case IdentifiersConvention.Camel:
                    word.CopyTo(xWord);
                    xWord[to] = to == 0 ? char.ToLower(xWord[to]) : char.ToUpper(xWord[to]);
                    to += word.Length;
                    break;

                case IdentifiersConvention.Pascal:
                    word.CopyTo(xWord);
                    xWord[to] = char.ToUpper(xWord[to]);
                    to += word.Length;
                    break;

                case IdentifiersConvention.SnakeLower:
                    word.CopyTo(xWord);
                    for (var j = 0; j < i; j++)
                        xWord[to++] = char.ToLower(word[j]);
                    xWord[to++] = '_';
                    break;

                case IdentifiersConvention.SnakeUpper:
                    word.CopyTo(xWord);
                    for (var j = 0; j < i; j++)
                        xWord[to++] = char.ToUpper(word[j]);
                    xWord[to++] = '_';
                    break;

                case IdentifiersConvention.KebabLower:
                    word.CopyTo(xWord);
                    for (var j = 0; j < i; j++)
                        xWord[to++] = char.ToLower(word[j]);
                    xWord[to++] = '-';
                    break;

                case IdentifiersConvention.KebabUpper:
                    word.CopyTo(xWord);
                    for (var j = 0; j < i; j++)
                        xWord[to++] = char.ToUpper(word[j]);
                    xWord[to++] = '-';
                    break;

                default:
                    throw new InternalTransformErrorException("Invalid identifier transform convention.");
            }
            while (i < len && chars[i] == '_')
                i++;
            category = char.GetUnicodeCategory(chars[i]);
        }

        return xChars.ToString();
    }
}
