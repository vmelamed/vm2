namespace vm2.RegexLib;

/// <summary>
/// Class Ascii. Contains regular expressions that match strings composed of the first 128 Unicode characters (that point 
/// to ASCII characters).
/// </summary>
public static class Ascii
{
    /// <summary>
    /// The space. It must be escaped in regular expressions with <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </summary>
    public const string Space = @"\x20";

    /// <summary>
    /// The hash sign. It must be escaped not as "\#" in regular expressions with <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </summary>
    public const string Hash = @"\x23";


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
    internal const string AlphaChars = $"{HighAlphaChars}{LowAlphaChars}";

    /// <summary>
    /// Matches a Latin character.
    /// <para>BNF: <c>alpha = low_alpha | high_alpha</c></para>
    /// </summary>
    public const string AlphaRex = $"[{AlphaChars}]";

    /// <summary>
    /// The non-zero digit characters.
    /// <para>BNF: <c>non-zero-digit = 1 | 2 | ... | 9</c></para>
    /// </summary>
    public const string NonZeroDigitChars = "1-9";

    /// <summary>
    /// Matches a non-zero digit.
    /// <para>BNF: <c>non-zero-digit = 1 | 2 | ... | 9</c></para>
    /// </summary>
    public const string NonZeroDigitRex = $"[{NonZeroDigitChars}]";

    /// <summary>
    /// The digit characters.
    /// <para>BNF: <c>digit = 0 | non-zero-digit</c></para>
    /// </summary>
    public const string DigitChars = $@"0-9";   // <==> $@"0{NonZeroDigitChars}";

    /// <summary>
    /// Matches a digit.
    /// <para>BNF: <c>digit = 0 | 1 | ... | 9</c></para>
    /// </summary>
    public const string DigitRex = $"[{DigitChars}]";

    /// <summary>
    /// The set of high-alpha-numeric chars.
    /// <para>BNF: <c>digit = 0 | 1 | ... | 9 | A | B | ... | Z </c></para>
    /// </summary>
    public const string HighAlphaNumericChars = $"{DigitChars}{HighAlphaChars}";

    /// <summary>
    /// Matches an high-alpha-numeric char.
    /// <para>BNF: <c>digit = 0 | 1 | ... | 9 | A | B | ... | Z </c></para>
    /// </summary>
    public const string HighAlphaNumericRex = $"[{HighAlphaNumericChars}]";

    /// <summary>
    /// The set of low-alpha-numeric chars.
    /// <para>BNF: <c>digit = 0 | 1 | ... | 9 | a | b | ... | z </c></para>
    /// </summary>
    public const string LowAlphaNumericChars = $"{DigitChars}{LowAlphaChars}";

    /// <summary>
    /// Matches an low-alpha-numeric char.
    /// <para>BNF: <c>digit = 0 | 1 | ... | 9 | a | b | ... | z </c></para>
    /// </summary>
    public const string LowAlphaNumericRex = $"[{LowAlphaNumericChars}]";

    /// <summary>
    /// The set of alpha-numeric chars.
    /// <para>BNF: <c>digit = 0 | 1 | ... | 9 | A | B | ... | Z | a | b | ... | z</c></para>
    /// </summary>
    public const string AlphaNumericChars = $"{DigitChars}{AlphaChars}";

    /// <summary>
    /// Matches an alpha-numeric char.
    /// <para>BNF: <c>digit = 0 | 1 | ... | 9 | A | B | ... | Z | a | b | ... | z</c></para>
    /// </summary>
    public const string AlphaNumericRex = $"[{AlphaNumericChars}]";

    #region Base64
    // See https://datatracker.ietf.org/doc/html/rfc4648#section-4

    /// <summary>
    /// The characters that can be used for string representation of a base64 encoded data without the padding.
    /// </summary>
    const string base64Chars = $@"{AlphaChars}0-9/\+";

    /// <summary>
    /// Matches a character of base64 encoded data.
    /// <para>BNF: <c>base64char = A | B | ... | Z | a | b | ... | z | 0 | 1 | ... | 9 | / | + </c> </para>
    /// </summary>
    /// <remarks>
    /// Note that \r and \n may break-to-continue a base64 string but have no meaning when decoded and are ignored.
    /// </remarks>
    public static string Base64CharRex = $"[{base64Chars}]";

    /// <summary>
    /// Matches a base64 encoded multiline string fragment possibly padded with `=`-s.
    /// </summary>
    /// <remarks>
    /// Requires "(?mx)" or <see cref="RegexOptions.Multiline"/> and <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public static string Base64Rex = @$"(?: ^{Base64CharRex}+ \r?$\n? )* (?: ^{Base64CharRex}+ ={{1,2}} \r?$\n? )?";

    /// <summary>
    /// Matches a base64 encoded multiline string possibly padded with `=`-s.
    /// </summary>
    /// <remarks>
    /// Requires "(?mx)" or <see cref="RegexOptions.Multiline"/> and <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public static string Base64Regex = @$"\A {Base64Rex} \z";

    static readonly Lazy<Regex> rexBase64 = new(() => new Regex(Base64Regex, RegexOptions.Compiled |
                                                                             RegexOptions.IgnorePatternWhitespace |
                                                                             RegexOptions.Multiline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a base64 encoded multiline string possibly padded with `=`-s.
    /// </summary>
    public static Regex Base64 => rexBase64.Value;
    #endregion
}
