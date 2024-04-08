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

    static readonly Lazy<Regex> regexAbaRoutingNumber = new(() => new(AbaRoutingNumberRegex, RegexOptions.Compiled, TimeSpan.FromMilliseconds(500)));

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
    /// Named groups: <see cref="BankGr"/>, <see cref="CountryGr"/>, <see cref="LocationGr"/>, and <see cref="BranchGr"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string SwiftCodeRex = $$"""
        (?<{{BankGr}}>     {{Ascii.HighAlphaRex}}{4}        )
        (?<{{CountryGr}}>  {{Countries.CountryCode2Rex}}    )
        (?<{{LocationGr}}> {{Ascii.HighAlphaNumericRex}}{2} )
        (?<{{BranchGr}}>   {{Ascii.HighAlphaNumericRex}}{3} )?
        """;

    /// <summary>
    /// Regular expression pattern which matches a string that represents a SWIFT bank code, a.k.a. BIC.
    /// <para>
    /// Named groups: <see cref="BankGr"/>, <see cref="CountryGr"/>, <see cref="LocationGr"/>, and <see cref="BranchGr"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string SwiftCodeRegex = $@"^{SwiftCodeRex}$";

    static readonly Lazy<Regex> regexSwiftCode = new(() => new(SwiftCodeRegex, RegexOptions.Compiled |
                                                                               RegexOptions.IgnorePatternWhitespace, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// Gets a Regex object which matches a string representing a SWIFT bank code, a.k.a. BIC.
    /// <para>
    /// Named groups: <see cref="BankGr"/>, <see cref="CountryGr"/>, <see cref="LocationGr"/>, and <see cref="BranchGr"/>.
    /// </para>
    /// </summary>
    public static Regex SwiftCode => regexSwiftCode.Value;

    static readonly Lazy<Regex> regexSwiftCodeI = new(() => new(SwiftCodeRegex, RegexOptions.Compiled |
                                                                                RegexOptions.IgnorePatternWhitespace |
                                                                                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// Gets a Regex object which matches <b>case-insensitevely</b> a string representing a SWIFT bank code, a.k.a. BIC.
    /// <para>
    /// Named groups: <see cref="BankGr"/>, <see cref="CountryGr"/>, <see cref="LocationGr"/>, and <see cref="BranchGr"/>.
    /// </para>
    /// </summary>
    public static Regex SwiftCodeI => regexSwiftCodeI.Value;
    #endregion

    #region Iban
    /// <summary>
    /// The name of a matching group representing check-sum characters.
    /// </summary>
    public const string CheckGr = "check";

    /// <summary>
    /// The name of a matching group representing the basic bank account number from the IBAN.
    /// </summary>
    public const string BasicBanGr = "accn";

    /// <summary>
    /// Regular expression pattern which matches an 
    /// <seealso href="https://en.wikipedia.org/wiki/International_Bank_Account_Number">
    /// International Bank Account Number - IBAN (ISO 13616) in a string.
    /// </seealso>
    /// <para>Named groups: <see cref="BankGr"/>, <see cref="CheckGr"/>, <see cref="BasicBanGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string IbanRex = $$"""
       (?<{{BankGr}}>      {{Countries.CountryCode2Rex}} )
       (?<{{CheckGr}}>     {{Ascii.DigitRex}}{2} ) {{Ascii.Space}}? 
       (?<{{BasicBanGr}}>  {{Ascii.HighAlphaNumericRex}}{4}
                           (?: {{Ascii.Space}}? {{Ascii.HighAlphaNumericRex}}{4}   ){2,6} 
                           (?: {{Ascii.Space}}? {{Ascii.HighAlphaNumericRex}}{1,4} )?     )
       """;

    /// <summary>
    /// Regular expression pattern which matches a string that represents an International Bank Account Number.
    /// </summary>
    public const string IbanRegex = $@"^{IbanRex}$";

    static readonly Lazy<Regex> regexIban = new(() => new(IbanRegex, RegexOptions.Compiled |
                                                                     RegexOptions.IgnorePatternWhitespace, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// Gets a Regex object which matches a string representing an International Bank Account Number.
    /// <para>Named groups: <see cref="BankGr"/>, <see cref="CheckGr"/>, <see cref="BasicBanGr"/>.</para>
    /// </summary>
    public static Regex Iban => regexIban.Value;

    static readonly Lazy<Regex> regexIbanI = new(() => new(IbanRegex, RegexOptions.Compiled |
                                                                      RegexOptions.IgnorePatternWhitespace |
                                                                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// Gets a Regex object which matches <b>case insensitevely</b> a string representing an International Bank Account Number.
    /// <seealso href="https://en.wikipedia.org/wiki/International_Bank_Account_Number">
    /// International Bank Account Number - IBAN (ISO 13616)
    /// </seealso>.
    /// <para>Named groups: <see cref="BankGr"/>, <see cref="CheckGr"/>, <see cref="BasicBanGr"/>.</para>
    /// </summary>
    public static Regex IbanI => regexIbanI.Value;
    #endregion
}
