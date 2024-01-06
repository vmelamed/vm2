namespace vm2.RegexLib;

/// <summary>
/// Class Banking. Contains regular expressions related to the Bank industry.
/// </summary>
public static class Banking
{
    #region AbaRoutingNumber
    /// <summary>
    /// Regular expression pattern which matches a US ABA routing number in a string.
    /// </summary>
    public const string AbaRoutingNumberRex = $@"{Ascii.DigitRex}{{9}}";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a US ABA routing number.
    /// </summary>
    public const string AbaRoutingNumberRegex = $@"^{AbaRoutingNumberRex}$";

    static readonly Lazy<Regex> regexAbaRoutingNumber = new(() => new(AbaRoutingNumberRegex, RegexOptions.Compiled | RegexOptions.CultureInvariant));

    /// <summary>
    /// Gets a Regex object which matches a string representing a US ABA routing number.
    /// </summary>
    public static Regex AbaRoutingNumber => regexAbaRoutingNumber.Value;
    #endregion

    #region SwiftCode
    /// <summary>
    /// The name of a matching group representing a bank code in a SWIFT code.
    /// </summary>
    public const string BankGr = "bank";

    /// <summary>
    /// The name of a matching group representing a country code (ISO 3166-1 Alpha 2) in a SWIFT code.
    /// </summary>
    public const string CountryGr = "country";

    /// <summary>
    /// The name of a matching group representing a location code in a SWIFT code.
    /// </summary>
    public const string LocationGr = "location";


    /// <summary>
    /// The name of a matching group representing an optional branch code in a SWIFT code.
    /// </summary>
    public const string BranchGr = "branch";

    /// <summary>
    /// Regular expression pattern which matches a 
    /// <seealso href="https://en.wikipedia.org/wiki/ISO_9362">SWIFT bank code (ISO 9362)</seealso> in a string, a.k.a. BIC.
    /// <para>
    /// <para>Named groups: <see cref="BankGr"/>, <see cref="CountryGr"/>, <see cref="LocationGr"/>, and <see cref="BranchGr"/>.</para>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string SwiftCodeRex = $@"(?<bank>{Ascii.HighAlphaChars}{{4}}) (?<country>{Countries.CountryCode2Rex}) (?<location>{Ascii.AlphaNumericRex}{{2}}) (?<branch>{Ascii.AlphaNumericRex}{{3}})?";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a SWIFT bank code, a.k.a. BIC.
    /// <para>
    /// <para>Named groups: <see cref="BankGr"/>, <see cref="CountryGr"/>, LocationGr, and <see cref="BranchGr"/>.</para>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string SwiftCodeRegex = $@"^{SwiftCodeRex}$";

    static readonly Lazy<Regex> regexSwiftCode = new(() => new(SwiftCodeRegex, RegexOptions.Compiled |
                                                                               RegexOptions.IgnorePatternWhitespace |
                                                                               RegexOptions.CultureInvariant));

    /// <summary>
    /// Gets a Regex object which matches a string representing a SWIFT bank code, a.k.a. BIC.
    /// <para>
    /// <para>Named groups: <see cref="BankGr"/>, <see cref="CountryGr"/>, LocationGr, and <see cref="BranchGr"/>.</para>
    /// </para>
    /// </summary>
    public static Regex SwiftCode => regexSwiftCode.Value;

    static readonly Lazy<Regex> regexSwiftCodeI = new(() => new(SwiftCodeRegex, RegexOptions.Compiled |
                                                                                RegexOptions.IgnorePatternWhitespace |
                                                                                RegexOptions.IgnoreCase |
                                                                                RegexOptions.CultureInvariant));

    /// <summary>
    /// Gets a Regex object which matches <b>case-insensitevely</b> a string representing a SWIFT bank code, a.k.a. BIC.
    /// <para>
    /// <para>Named groups: <see cref="BankGr"/>, <see cref="CountryGr"/>, LocationGr, and <see cref="BranchGr"/>.</para>
    /// </para>
    /// </summary>
    public static Regex SwiftCodeI => regexSwiftCodeI.Value;
    #endregion
}
