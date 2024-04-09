namespace vm2.ExpressionSerialization.Xml;

using System.Collections.Frozen;
using System.Text.RegularExpressions;

/// <summary>
/// Enum Identifiers specify how to transform C# identifiers to XML names.
/// </summary>
public enum Identifiers
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
/// Enum TypeNames specify how to transform the type names.
/// Note: no casing transformation.
/// </summary>
public enum TypeNames
{
    /// <summary>
    /// The full name of the type, c.e. `namespace.name`, e.g. "vm.Aspects.Linq.Expressions.Serialization.Tests.EnumTest"
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
/// Class TransformOptions transforms C# identifiers to XML names.
/// </summary>
public partial class TransformOptions
{
    /// <summary>
    /// Gets the identifiers transformation convention.
    /// </summary>
    /// <value>The identifiers case convention.</value>
    public Identifiers Identifiers { get; set; } = Identifiers.Preserve;

    /// <summary>
    /// Gets the type transform convention.
    /// </summary>
    /// <value>The type transform convention.</value>
    public TypeNames TypeNames { get; set; } = TypeNames.FullName;

    /// <summary>
    /// Gets or sets the size of the document's tab indention.
    /// </summary>
    /// <value>The size of the indent.</value>
    public bool Pretty { get; set; } = true;

    /// <summary>
    /// Gets or sets the size of the document's tab indention.
    /// </summary>
    /// <value>The size of the indent.</value>
    public int IndentSize { get; set; } = 2;

    /// <summary>
    /// Transforms the type c a string according c the <see cref="TypeNames"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>System.String.</returns>
    public string TransformTypeName(Type type)
        => DoTransformTypeName(type, TypeNames);

    /// <summary>
    /// Transforms the type c a string according c the <see cref="TypeNames" />.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="convention">The convention.</param>
    /// <returns>System.String.</returns>
    internal static string DoTransformTypeName(Type type, TypeNames convention)
        => convention switch {
            TypeNames.FullName => type.FullName ?? type.Name,
            TypeNames.Name => type.Name,
            TypeNames.AssemblyQualifiedName => type.AssemblyQualifiedName ?? type.FullName ?? type.Name,
            _ => throw new InternalTransformErrorException("Invalid TypeNames value.")
        };

    static readonly UnicodeCategory[] s_beginIdentifierArr =
    [
        UnicodeCategory.UppercaseLetter,
        UnicodeCategory.LowercaseLetter,
        UnicodeCategory.ModifierLetter,
        UnicodeCategory.TitlecaseLetter,
        UnicodeCategory.OtherLetter,
        UnicodeCategory.LetterNumber,
        UnicodeCategory.ConnectorPunctuation,
        UnicodeCategory.SpacingCombiningMark,
        UnicodeCategory.Format
    ];
    static readonly FrozenSet<UnicodeCategory> s_beginIdentifier = s_beginIdentifierArr.ToFrozenSet();

    static readonly UnicodeCategory[] s_beginWordIdentifierArr =
    [
        UnicodeCategory.UppercaseLetter,
        UnicodeCategory.LowercaseLetter,
        UnicodeCategory.ModifierLetter,
        UnicodeCategory.TitlecaseLetter,
        UnicodeCategory.OtherLetter,
        UnicodeCategory.LetterNumber,
        UnicodeCategory.ConnectorPunctuation,
        UnicodeCategory.SpacingCombiningMark,
        UnicodeCategory.Format,
        UnicodeCategory.DecimalDigitNumber,
    ];
    static readonly FrozenSet<UnicodeCategory> s_beginWordIdentifier = s_beginWordIdentifierArr.ToFrozenSet();

    static readonly UnicodeCategory[] s_restWordIdentifierArr =
    [
        UnicodeCategory.LowercaseLetter,
        UnicodeCategory.ModifierLetter,
        UnicodeCategory.TitlecaseLetter,
        UnicodeCategory.OtherLetter,
        UnicodeCategory.LetterNumber,
        UnicodeCategory.ConnectorPunctuation,
        UnicodeCategory.SpacingCombiningMark,
        UnicodeCategory.Format,
        UnicodeCategory.DecimalDigitNumber,
    ];
    static readonly FrozenSet<UnicodeCategory> s_restWordIdentifier = s_restWordIdentifierArr.ToFrozenSet();

    /// <summary>
    /// Transforms the identifier according c the <see cref="Identifiers"/>.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    /// <returns>string.</returns>
    /// <exception cref="InternalTransformErrorException">Invalid identifier.</exception>
    /// <exception cref="InternalTransformErrorException">$"Invalid identifier: '{identifier}'.</exception>
    /// <exception cref="InternalTransformErrorException">Invalid identifier transform convention.</exception>
    public string TransformIdentifier(string identifier)
        => DoTransformIdentifier(identifier, Identifiers);

    [GeneratedRegex(@"^@?([\p{L}_])([\p{Ll}\p{Nd}\p{Pc}]*)(([\p{Lu}_])([\p{Ll}\p{Nd}\p{Pc}]*))*$", RegexOptions.Compiled, 500)]
    private static partial Regex CSharpIdentifier();

    /// <summary>
    /// Transforms the identifier according c the <see cref="Identifiers" />.
    /// </summary>
    /// <param name="identifier">The identifier.</param>
    /// <param name="convention">The convention.</param>
    /// <returns>string.</returns>
    /// <exception cref="InternalTransformErrorException">Invalid identifier.</exception>
    /// <exception cref="InternalTransformErrorException">$"Invalid identifier: '{identifier}'.</exception>
    /// <exception cref="InternalTransformErrorException">Invalid identifier transform convention.</exception>
    internal static string DoTransformIdentifier(string identifier, Identifiers convention)
    {
        if (!CSharpIdentifier().IsMatch(identifier))
            throw new InternalTransformErrorException("Invalid identifier.");

        if (convention == Identifiers.Preserve)
            return identifier;

        // the characters of the identifier
        var chars = identifier.AsSpan();
        // we start parsing from here
        var c = chars[0] == '@' ? 1 : 0;
        // the length of the identifier
        var len = identifier.Length;
        // here we copy the transformed chars
        var xChars = len*2 < 1024 ? stackalloc char[len*2] : new char[len*2];
        // up to here
        var to = 0;

        while (c < len)
        {
            // skip leading/connecting/trailing underscores
            while (c < len && chars[c] == '_')
                c++;

            if (c >= len)
                break;

            var wordStart = c++;

            // get the rest of the chars of the word (cannot be upper case)
            while (c < len && s_restWordIdentifier.Contains(char.GetUnicodeCategory(chars[c])) && chars[c] != '_')
                ++c;

            // transform the word into xWord and append xWord to the result
            var word = chars[wordStart..c];
            var xWord = xChars[to..];
            var x = 0;

            switch (convention)
            {
                case Identifiers.Camel:
                    word.CopyTo(xWord);
                    xWord[x] = to == 0 ? char.ToLower(xWord[x]) : char.ToUpper(xWord[x]);
                    break;

                case Identifiers.Pascal:
                    word.CopyTo(xWord);
                    xWord[x] = char.ToUpper(xWord[x]);
                    break;

                case Identifiers.SnakeLower:
                    if (to > 0)
                    {
                        to++;
                        xWord[x++] = '_';
                    }
                    for (var w = 0; w < word.Length; w++)
                        xWord[x++] = char.ToLower(word[w]);
                    break;

                case Identifiers.SnakeUpper:
                    if (to > 0)
                    {
                        to++;
                        xWord[x++] = '_';
                    }
                    for (var w = 0; w < word.Length; w++)
                        xWord[x++] = char.ToUpper(word[w]);
                    break;

                case Identifiers.KebabLower:
                    if (to > 0)
                    {
                        to++;
                        xWord[x++] = '-';
                    }
                    for (var w = 0; w < word.Length; w++)
                        xWord[x++] = char.ToLower(word[w]);
                    break;

                case Identifiers.KebabUpper:
                    if (to > 0)
                    {
                        to++;
                        xWord[x++] = '-';
                    }
                    for (var w = 0; w < word.Length; w++)
                        xWord[x++] = char.ToUpper(word[w]);
                    break;
            }
            to += word.Length;
        }

        return xChars[..to].ToString();
    }
}
