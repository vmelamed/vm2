namespace vm2.RegexLib;

/// <summary>
/// Class Countries. Contains country-related regular expressions.
/// </summary>
public static class Countries
{
    #region CountryCode2
    /// <summary>
    /// Regular expression pattern which matches <seealso href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">a 
    /// 2-letter country code</seealso>, e.g. US, in a string.
    /// </summary>
    /// <remarks>
    /// Note that this expression matches upper-case letters only as defined by the standard. Combine with <c>"(?i)"</c>
    /// or use <see cref="RegexOptions.IgnoreCase"/> to match case-insensitively.
    /// </remarks>
    public const string CountryCode2Rex = $@"{Ascii.HighAlphaRex}{{2}}";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a 
    /// <seealso href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">2-letter country code</seealso>, e.g. US.
    /// </summary>
    /// <remarks>
    /// Note that this expression matches upper-case letters only as defined by the standard. Combine with <c>"(?i)"</c>
    /// or use <see cref="RegexOptions.IgnoreCase"/> to match case-insensitively.
    /// </remarks>
    public const string CountryCode2Regex = $@"^{CountryCode2Rex}$";

    static readonly Lazy<Regex> regexCountryCode2 = new(() => new(CountryCode2Regex, RegexOptions.Compiled));

    /// <summary>
    /// Gets a Regex object which matches a string representing a 
    /// <seealso href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">2-letter country code</seealso>, e.g. US.
    /// </summary>
    public static Regex CountryCode2 => regexCountryCode2.Value;

    static readonly Lazy<Regex> regexCountryCode2I = new(() => new(CountryCode2Regex, RegexOptions.Compiled |
                                                                                      RegexOptions.IgnoreCase));

    /// <summary>
    /// Gets a Regex object which matches <b>case-insensitively</b> a string representing a 
    /// <seealso href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">2-letter country code</seealso>, e.g. US.
    /// </summary>
    public static Regex CountryCode2I => regexCountryCode2I.Value;
    #endregion

    #region CountryCode3
    /// <summary>
    /// Regular expression pattern which matches a 3-letter country code
    /// (https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3), e.g. USA, in a string.
    /// </summary>
    /// <remarks>
    /// Note that this expression matches upper-case letters only as defined by the standard. Combine with <c>"(?i)"</c>
    /// or use <see cref="RegexOptions.IgnoreCase"/> to match case-insensitively.
    /// </remarks>
    public const string CountryCode3Rex = $@"{Ascii.HighAlphaRex}{{3}}";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a 3-letter country code
    /// (https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3), e.g. USA.
    /// </summary>
    /// <remarks>
    /// Note that this expression matches upper-case letters only as defined by the standard. Combine with <c>"(?i)"</c>
    /// or use <see cref="RegexOptions.IgnoreCase"/> to match case-insensitively.
    /// </remarks>
    public const string CountryCode3Regex = $@"^{CountryCode3Rex}$";

    static readonly Lazy<Regex> regexCountryCode3 = new(() => new(CountryCode3Regex, RegexOptions.Compiled));

    /// <summary>
    /// Gets a Regex object which matches a string representing a 3-letter country code
    /// (https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3), e.g. USA.
    /// </summary>
    public static Regex CountryCode3 => regexCountryCode3.Value;

    static readonly Lazy<Regex> regexCountryCode3I = new(() => new(CountryCode3Regex, RegexOptions.Compiled |
                                                                                      RegexOptions.IgnoreCase));

    /// <summary>
    /// Gets a Regex object which matches <b>case-insensitively</b> a string representing a 3-letter country code
    /// (https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3), e.g. USA.
    /// </summary>
    public static Regex CountryCode3I => regexCountryCode3I.Value;
    #endregion

    #region CurrencyCode
    /// <summary>
    /// Regular expression pattern which matches a currency code in a string.
    /// </summary>
    /// <remarks>
    /// Note that this expression matches upper-case letters only as defined by the standard. Combine with <c>"(?i)"</c>
    /// or use <see cref="RegexOptions.IgnoreCase"/> to match case-insensitively.
    /// </remarks>
    public const string CurrencyCodeRex = $@"{CountryCode2Rex}{Ascii.HighAlphaRex}";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a currency code.
    /// </summary>
    /// <remarks>
    /// Note that this expression matches upper-case letters only as defined by the standard. Combine with <c>"(?i)"</c>
    /// or use <see cref="RegexOptions.IgnoreCase"/> to match case-insensitively.
    /// </remarks>
    public const string CurrencyCodeRegex = $@"^{CurrencyCodeRex}$";

    static readonly Lazy<Regex> regexCurrencyCode = new(() => new(CurrencyCodeRegex, RegexOptions.Compiled));

    /// <summary>
    /// Gets a Regex object which matches a string representing a currency code.
    /// </summary>
    public static Regex CurrencyCode => regexCurrencyCode.Value;

    static readonly Lazy<Regex> regexCurrencyCodeI = new(() => new(CurrencyCodeRegex, RegexOptions.Compiled |
                                                                                      RegexOptions.IgnoreCase));

    /// <summary>
    /// Gets a Regex object which matches a string representing a currency code.
    /// </summary>
    public static Regex CurrencyCodeI => regexCurrencyCodeI.Value;
    #endregion

    const string telephoneSeparatorChars = $@"\(\)\-{Ascii.Space}";

    const string telephoneSeparatorRex = $@"[{telephoneSeparatorChars}]";

    #region TelephoneCode
    /// <summary>
    /// Regular expression pattern which matches a country telephone code in a string.
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string TelephoneCodeRex = $@"(?: {Ascii.NonZeroDigitRex}{Ascii.DigitRex}{{0,2}} )";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a country telephone code.
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string TelephoneCodeRegex = $@"^{TelephoneCodeRex}$";

    static readonly Lazy<Regex> regexTelephoneCode = new(() => new(TelephoneCodeRegex, RegexOptions.Compiled |
                                                                                       RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// Gets a Regex object which matches a string representing a country telephone code.
    /// </summary>
    public static Regex TelephoneCode => regexTelephoneCode.Value;
    #endregion

    #region TelephoneNumber
    /// <summary>
    /// Regular expression pattern which matches a telephone number in a string.
    /// </summary>
    public const string TelephoneNumberRex = $@"{Ascii.DigitRex}{{4,15}}";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a telephone number.
    /// </summary>
    public const string TelephoneNumberRegex = $@"^{TelephoneNumberRex}$";

    static readonly Lazy<Regex> regexTelephoneNumber = new(() => new(TelephoneNumberRegex, RegexOptions.Compiled));

    /// <summary>
    /// Gets a Regex object which matches a string representing a telephone number.
    /// </summary>
    public static Regex TelephoneNumber => regexTelephoneNumber.Value;
    #endregion

    #region TelephoneNumberE164
    /// <summary>
    /// Regular expression pattern which matches a telephone number by E.164 in a string.
    /// </summary>
    public const string TelephoneNumberE164Rex = $@"\+{Ascii.NonZeroDigitRex}{Ascii.DigitRex}{{2,14}}";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a telephone number by E.164.
    /// </summary>
    public const string TelephoneNumberE164Regex = $@"^{TelephoneNumberE164Rex}$";

    static readonly Lazy<Regex> regexTelephoneNumberE164 = new(() => new(TelephoneNumberE164Regex, RegexOptions.Compiled));

    /// <summary>
    /// Gets a Regex object which matches a string representing a telephone number by E.164.
    /// </summary>
    public static Regex TelephoneNumberE164 => regexTelephoneNumberE164.Value;
    #endregion

    #region TelephoneNumberExtra
    /// <summary>
    /// Regular expression pattern which matches a telephone number expressed with the accepted extra characters:
    /// <c>-() </c> (incl. space).
    /// </summary>
    public const string TelephoneNumberExtraRex = $$"""
                                                \+?
                                                ({{telephoneSeparatorRex}}*{{Ascii.NonZeroDigitRex}})
                                                (?:{{telephoneSeparatorRex}}? {{Ascii.DigitRex}}){2,14}
                                                {{telephoneSeparatorRex}}*;
                                                """;

    /// <summary>
    /// Regular expression pattern which matches a string that represents a telephone number expressed with the accepted 
    /// extra characters: <c>-() </c> (incl. space).
    /// </summary>
    public const string TelephoneNumberExtraRegex = $@"^{TelephoneNumberExtraRex}$";

    static readonly Lazy<Regex> regexTelephoneNumberExtra = new(() => new(TelephoneNumberExtraRegex, RegexOptions.Compiled |
                                                                                                     RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// Gets a Regex object which matches a string representing a telephone number expressed with the accepted extra characters:
    /// <c>-() </c> (incl. space).
    /// </summary>
    public static Regex TelephoneNumberExtra => regexTelephoneNumberExtra.Value;
    #endregion

    /// <summary>
    /// Class US defines USA specific regular expressions
    /// </summary>
    public static class US
    {
        /// <summary>
        /// The name of a matching group representing an area code.
        /// </summary>
        public const string AreaGr = "area";

        /// <summary>
        /// The name of a matching group representing an 7-digit telephone number.
        /// </summary>
        public const string NumberGr = "area";

        #region TelephoneNumber
        /// <summary>
        /// Regular expression pattern which matches a US telephone number in a string expressed, possibly with a 
        /// country code (+1), and using the traditional telephone separators ('-', '+', '(', ')')
        /// </summary>
        /// <remarks>
        /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
        /// </remarks>
        public const string TelephoneNumberRex = $$"""
                                               (?:\+?1)? {{telephoneSeparatorRex}}*
                                               (            ?<{{AreaGr}}> [2-9] (?!11) {{Ascii.DigitRex}}{2} )
                                                         {{telephoneSeparatorRex}}* 
                                               (?<{{NumberGr}}> [2-9]{{Ascii.DigitRex}}{2} {{telephoneSeparatorRex}}* {{Ascii.DigitRex}}{4} {{telephoneSeparatorRex}}* )
                                               """;

        /// <summary>
        /// Regular expression pattern which matches a string that represents a US telephone number.
        /// <para>
        /// <para>Named groups: <see cref="AreaGr"/> and <see cref="NumberGr"/>.</para>
        /// </para>
        /// </summary>
        /// <remarks>
        /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
        /// </remarks>
        public const string TelephoneNumberRegex = $@"^{TelephoneNumberRex}$";

        static readonly Lazy<Regex> regexTelephoneNumber = new(() => new(TelephoneNumberRegex, RegexOptions.Compiled |
                                                                                               RegexOptions.IgnorePatternWhitespace));

        /// <summary>
        /// Gets a Regex object which matches a string representing a US telephone number.
        /// </summary>
        public static Regex TelephoneNumber => regexTelephoneNumber.Value;
        #endregion

        #region TelephoneNumberStrict
        /// <summary>
        /// Regular expression pattern which matches a US telephone number in a string expressed, possibly with a 
        /// country code (+1), and using the traditional telephone separators ('-', or space)
        /// </summary>
        /// <remarks>
        /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
        /// </remarks>
        public const string TelephoneNumberStrictRex = $$"""
                                                     (?:\+?1)? {{Ascii.Space}}? \( (?<{{AreaGr}}> [2-9] (?!11) {{Ascii.DigitRex}}{2} ) \)
                                                     {{Ascii.Space}}? (?<{{NumberGr}}> [2-9]{{Ascii.DigitRex}}{2} \- {{Ascii.DigitRex}}{4} {{telephoneSeparatorRex}}* )";
                                                     """;

        /// <summary>
        /// Regular expression pattern which matches a string that represents a US telephone number.
        /// <para>
        /// <para>Named groups: <see cref="AreaGr"/> and <see cref="NumberGr"/>.</para>
        /// </para>
        /// </summary>
        /// <remarks>
        /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
        /// </remarks>
        public const string TelephoneNumberStrictRegex = $@"^{TelephoneNumberStrictRex}$";

        static readonly Lazy<Regex> regexTelephoneNumberStrict = new(() => new(TelephoneNumberStrictRegex, RegexOptions.Compiled |
                                                                                                           RegexOptions.IgnorePatternWhitespace));

        /// <summary>
        /// Gets a Regex object which matches a string representing a US telephone number.
        /// </summary>
        public static Regex TelephoneNumberStrict => regexTelephoneNumberStrict.Value;
        #endregion

        #region StateCode
        /// <summary>
        /// Regular expression pattern which matches a state code in a string.
        /// </summary>
        public const string StateCodeRex = $@"{Ascii.HighAlphaRex}{{2}}";

        /// <summary>
        /// Regular expression pattern which matches a string that represents a state code.
        /// </summary>
        public const string StateCodeRegex = $@"^{StateCodeRex}$";

        static readonly Lazy<Regex> regexStateCode = new(() => new(StateCodeRegex, RegexOptions.Compiled));

        /// <summary>
        /// Gets a Regex object which matches a string representing a state code.
        /// </summary>
        public static Regex StateCode => regexStateCode.Value;

        static readonly Lazy<Regex> regexStateCodeI = new(() => new(StateCodeRegex, RegexOptions.Compiled));

        /// <summary>
        /// Gets a Regex object which matches a string representing a state code.
        /// </summary>
        /// <remarks>
        /// Note that this expression matches upper-case letters only as defined by the standard. Combine with <c>"(?i)"</c>
        /// or use <see cref="RegexOptions.IgnoreCase"/> to match case-insensitively.
        /// </remarks>
        public static Regex StateCodeI => regexStateCodeI.Value;
        #endregion

        /// <summary>
        /// The name of a matching group representing a 5-digit ZIP code.
        /// </summary>
        public const string ZipGr = "zip";
        /// <summary>
        /// The name of a matching group representing the 4 extension digits in a 5+4 digit ZIP code.
        /// </summary>
        public const string ExtGr = "ext";

        #region ZipCode5
        /// <summary>
        /// Regular expression pattern which matches a 5-digit US postal code (ZIP code) in a string.
        /// </summary>
        public const string ZipCode5Rex = $@"{Ascii.DigitRex}{{5}}";

        /// <summary>
        /// Regular expression pattern which matches a string that represents a US postal code (ZIP code).
        /// <para>Named groups: <see cref="ZipGr"/> and <see cref="ExtGr"/>.</para>
        /// </summary>
        /// <remarks>
        /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
        /// </remarks>
        public const string ZipCode5Regex = $@"^(?<{ZipGr}> {ZipCode5Rex} )$";

        static readonly Lazy<Regex> regexZipCode5 = new(() => new(ZipCode5Regex, RegexOptions.Compiled |
                                                                                 RegexOptions.IgnorePatternWhitespace));

        /// <summary>
        /// Gets a Regex object which matches a string representing a US postal code (ZIP code).
        /// </summary>
        public static Regex ZipCode5 => regexZipCode5.Value;
        #endregion

        #region ZipCode5x4
        /// <summary>
        /// Regular expression pattern which matches a 5+4 US postal code (ZIP code) in a string.
        /// <para>Named groups: <see cref="ZipGr"/> and <see cref="ExtGr"/>.</para>
        /// </summary>
        /// <remarks>
        /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
        /// </remarks>
        public const string ZipCode5x4Rex = $@"(?<{ZipGr}> {Ascii.DigitRex}{{5}} )( - (?<{ExtGr}> {Ascii.DigitRex}{{4}} ) )";

        /// <summary>
        /// Regular expression pattern which matches a string that represents a 5+4 US postal code (ZIP code).
        /// </summary>
        public const string ZipCode5x4Regex = $@"^{ZipCode5x4Rex}$";

        static readonly Lazy<Regex> regexZipCode5x4 = new(() => new(ZipCode5x4Regex, RegexOptions.Compiled |
                                                                                     RegexOptions.IgnorePatternWhitespace));

        /// <summary>
        /// Gets a Regex object which matches a string representing a 5+4 US postal code (ZIP code).
        /// </summary>
        public static Regex ZipCode5x4 => regexZipCode5x4.Value;
        #endregion

        #region ZipCode5o4
        /// <summary>
        /// Regular expression pattern which matches a 5 (+4 optional) US postal code (ZIP code) in a string.
        /// <para>
        /// <para>Named groups: <see cref="ZipGr"/> and <see cref="ExtGr"/>.</para>
        /// </para>
        /// </summary>
        /// <remarks>
        /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
        /// </remarks>
        public const string ZipCode5o4Rex = $@"(?<{ZipGr}>{Ascii.DigitRex}{{5}}) (?: - (?<{ExtGr}> {Ascii.DigitRex}{{4}} ) )?";

        /// <summary>
        /// Regular expression pattern which matches a string that represents a 5 (+4 optional) US postal code (ZIP code).
        /// </summary>
        public const string ZipCode5o4Regex = $@"^{ZipCode5o4Rex}$";

        static readonly Lazy<Regex> regexZipCode5o4 = new(() => new(ZipCode5o4Regex, RegexOptions.Compiled |
                                                                                     RegexOptions.IgnorePatternWhitespace));

        /// <summary>
        /// Gets a Regex object which matches a string representing a 5 (+4 optional) US postal code (ZIP code).
        /// </summary>
        public static Regex ZipCode5o4 => regexZipCode5o4.Value;
        #endregion

        #region SocialSecurityNumber
        /// <summary>
        /// Regular expression pattern which matches a social security number in a string with optional SSN groups separator '-'.
        /// </summary>
        public const string SocialSecurityNumberRex = "(?: (?! [8-9] | 7[8-9] | 77[3-9] | 000 | 666 | 001-?01-?0001 | 078-?05-?1120 | 433-?54-?3937)" +     // well-known invalid patterns and numbers
                                                      $@"{Ascii.DigitRex}{{3}} ) -? (?: (?! 00) {Ascii.DigitRex}{{2}} ) -? (?: (?! 0000 ) {Ascii.DigitRex}{{4}} )";

        /// <summary>
        /// Regular expression pattern which matches a string that represents a social security number.
        /// </summary>
        public const string SocialSecurityNumberRegex = $@"^{SocialSecurityNumberRex}$";

        static readonly Lazy<Regex> regexSocialSecurityNumber = new(() => new(SocialSecurityNumberRegex, RegexOptions.Compiled |
                                                                                                         RegexOptions.IgnorePatternWhitespace));

        /// <summary>
        /// Gets a Regex object which matches a string representing a social security number.
        /// </summary>
        public static Regex SocialSecurityNumber => regexSocialSecurityNumber.Value;
        #endregion

        #region Itin
        /// <summary>
        /// Regular expression pattern which matches a Individual Taxpayer Identification Number in a string with optional SSN groups separator '-'.
        /// </summary>
        public const string ItinRex = $@"9{Ascii.DigitRex}{{2}} -? [7-9]{Ascii.DigitRex} -? {Ascii.DigitRex}{{4}}";

        /// <summary>
        /// Regular expression pattern which matches a string that represents a Individual Taxpayer Identification Number.
        /// </summary>
        public const string ItinRegex = $@"^{ItinRex}$";

        static readonly Lazy<Regex> regexItin = new(() => new(ItinRegex, RegexOptions.Compiled |
                                                                         RegexOptions.IgnorePatternWhitespace));

        /// <summary>
        /// Gets a Regex object which matches a string representing a Individual Taxpayer Identification Number.
        /// </summary>
        public static Regex Itin => regexItin.Value;
        #endregion
    }
}
