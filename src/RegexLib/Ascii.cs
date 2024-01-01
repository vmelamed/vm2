namespace vm2.RegexLib;

/// <summary>
/// Class Ascii. Contains regular expressions that match strings composed of ASCII characters only.
/// </summary>
public static class Ascii
{
    /// <summary>
    /// The set of Latin uppercase characters.
    /// </summary>
    internal const string HighAlphaChars = "A-Z";

    /// <summary>
    /// Matches a Latin uppercase character.
    /// <para>BNF: <c>high_alpha = A | B | ... | Z</c></para>
    /// </summary>
    public const string HighAlphaRex = $"[{HighAlphaChars}]";

    /// <summary>
    /// The Latin lowercase characters.
    /// </summary>
    internal const string LowAlphaChars = "a-z";

    /// <summary>
    /// Matches a Latin lowercase character.
    /// <para>BNF: <c>low_alpha = a | b | ... | z</c></para>
    /// </summary>
    public const string LowAlphaRex = $"[{LowAlphaChars}]";

    /// <summary>
    /// The Latin characters.
    /// <para>BNF: <c>alpha = A | B | ... | Z | a | b | ... | z</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    internal const string AlphaChars = $"{HighAlphaChars} {LowAlphaChars}";

    /// <summary>
    /// Matches a Latin character.
    /// <para>BNF: <c>alpha = low_alpha | high_alpha</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string AlphaRex = $"[{AlphaChars}]";

    /// <summary>
    /// The characters that can be used for string representation of a base64 encoded data without the padding.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string base64Chars = $@"{AlphaChars} 0-9 / \+";

    /// <summary>
    /// Matches a character of base64 encoded data.
    /// <para>BNF: <c>base64char = A | B | ... | Z | a | b | ... | z | 0 | 1 | ... | 9 | / | + </c> </para>
    /// </summary>
    /// <remarks>
    /// Note that \r and \n may break-to-continue a base64 string but have no meaning when decoded and are ignored.
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public static string Base64CharRex = $"[{base64Chars}]";

    /// <summary>
    /// Matches a base64 encoded string fragment possibly padded with `=`-s.
    /// </summary>
    /// <remarks>
    /// Requires the matching to be done with <see cref="RegexOptions.Multiline"/> and <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public static string Base64Rex = @$"(?:^ {Base64CharRex}+ \r $)* (?:^ {Base64CharRex}+ ={{0,2}} \r $)";

    /// <summary>
    /// Matches a base64 encoded string possibly padded with `=`-s.
    /// </summary>
    /// <remarks>
    /// Requires the matching to be done with <see cref="RegexOptions.Multiline"/> and <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public static string Base64Regex = @$"\A (?:^ {Base64CharRex}+ \r $)* (?:^ {Base64CharRex}+ ={{0,2}} \r $) \z";

    static readonly Lazy<Regex> rexBase64 = new(() => new Regex(Base64Regex, RegexOptions.Compiled|
                                                                             RegexOptions.CultureInvariant|
                                                                             RegexOptions.Multiline|
                                                                             RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a hexadecimal number.
    /// </summary>
    public static Regex Base64 => rexBase64.Value;
}
