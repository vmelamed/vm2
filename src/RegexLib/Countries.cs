namespace vm2.RegexLib;

/// <summary>
/// Class Countries. Contains country-related regular expressions.
/// </summary>
public static partial class Countries
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
    public const string CountryCode2Rex = $@"{HighAlphaChar}{{2}}";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a
    /// <seealso href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">2-letter country code</seealso>, e.g. US.
    /// </summary>
    /// <remarks>
    /// Note that this expression matches upper-case letters only as defined by the standard. Combine with <c>"(?i)"</c>
    /// or use <see cref="RegexOptions.IgnoreCase"/> to match case-insensitively.
    /// </remarks>
    public const string CountryCode2Regex = $@"^{CountryCode2Rex}$";

    /// <summary>
    /// Gets a Regex object which matches a string representing a
    /// <seealso href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">2-letter country code</seealso>, e.g. US.
    /// </summary>
    [GeneratedRegex(CountryCode2Regex, Common.Options)]
    public static partial Regex CountryCode2();

    /// <summary>
    /// Gets a Regex object which matches <b>case-insensitively</b> a string representing a
    /// <seealso href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2">2-letter country code</seealso>, e.g. us.
    /// </summary>
    [GeneratedRegex(CountryCode2Regex, Common.OptionsI)]
    public static partial Regex CountryCode2I();
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
    public const string CountryCode3Rex = $@"{HighAlphaChar}{{3}}";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a 3-letter country code
    /// (https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3), e.g. USA.
    /// </summary>
    /// <remarks>
    /// Note that this expression matches upper-case letters only as defined by the standard. Combine with <c>"(?i)"</c>
    /// or use <see cref="RegexOptions.IgnoreCase"/> to match case-insensitively.
    /// </remarks>
    public const string CountryCode3Regex = $@"^{CountryCode3Rex}$";

    /// <summary>
    /// Gets a Regex object which matches a string representing a 3-letter country code
    /// (https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3), e.g. USA.
    /// </summary>
    [GeneratedRegex(CountryCode3Regex, Common.Options)]
    public static partial Regex CountryCode3();

    /// <summary>
    /// Gets a Regex object which matches <b>case-insensitively</b> a string representing a 3-letter country code
    /// (https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3), e.g. USA.
    /// </summary>
    [GeneratedRegex(CountryCode3Regex, Common.OptionsI)]
    public static partial Regex CountryCode3I();
    #endregion

    #region CurrencyCode
    /// <summary>
    /// Regular expression pattern which matches a currency code in a string.
    /// </summary>
    /// <remarks>
    /// Note that this expression matches upper-case letters only as defined by the standard. Combine with <c>"(?i)"</c>
    /// or use <see cref="RegexOptions.IgnoreCase"/> to match case-insensitively.
    /// </remarks>
    public const string CurrencyCodeRex = $@"{CountryCode2Rex}{HighAlphaChar}";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a currency code.
    /// </summary>
    /// <remarks>
    /// Note that this expression matches upper-case letters only as defined by the standard. Combine with <c>"(?i)"</c>
    /// or use <see cref="RegexOptions.IgnoreCase"/> to match case-insensitively.
    /// </remarks>
    public const string CurrencyCodeRegex = $@"^{CurrencyCodeRex}$";

    /// <summary>
    /// Gets a Regex object which matches a string representing a currency code.
    /// </summary>
    [GeneratedRegex(CurrencyCodeRegex, Common.Options)]
    public static partial Regex CurrencyCode();

    /// <summary>
    /// Gets a Regex object which matches a string representing a currency code.
    /// </summary>
    [GeneratedRegex(CurrencyCodeRegex, Common.OptionsI)]
    public static partial Regex CurrencyCodeI();
    #endregion

    const string telephoneSeparatorChars = $@"\(\)\-{Space}";

    const string telephoneSeparatorRex = $@"[{telephoneSeparatorChars}]";

    #region TelephoneCode
    /// <summary>
    /// Regular expression pattern which matches a country telephone code in a string.
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string TelephoneCodeRex = $@"(?: {NzDigitChar}{DigitChar}{{0,2}} )";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a country telephone code.
    /// </summary>
    /// <remarks>
    /// Requires "(?x)" or <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string TelephoneCodeRegex = $@"^{TelephoneCodeRex}$";

    /// <summary>
    /// Gets a Regex object which matches a string representing a country telephone code.
    /// </summary>
    [GeneratedRegex(TelephoneCodeRegex, Common.Options)]
    public static partial Regex TelephoneCode();
    #endregion

    #region TelephoneNumber
    /// <summary>
    /// Regular expression pattern which matches a telephone number in a string.
    /// </summary>
    public const string TelephoneNumberRex = $@"{DigitChar}{{4,15}}";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a telephone number.
    /// </summary>
    public const string TelephoneNumberRegex = $@"^{TelephoneNumberRex}$";

    /// <summary>
    /// Gets a Regex object which matches a string representing a telephone number.
    /// </summary>
    [GeneratedRegex(TelephoneNumberRegex, Common.Options)]
    public static partial Regex TelephoneNumber();
    #endregion

    #region TelephoneNumberE164
    /// <summary>
    /// Regular expression pattern which matches a telephone number by E.164 in a string.
    /// </summary>
    public const string TelephoneNumberE164Rex = $@"\+{NzDigitChar}{DigitChar}{{2,14}}";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a telephone number by E.164.
    /// </summary>
    public const string TelephoneNumberE164Regex = $@"^{TelephoneNumberE164Rex}$";

    /// <summary>
    /// Gets a Regex object which matches a string representing a telephone number by E.164.
    /// </summary>
    [GeneratedRegex(TelephoneNumberE164Regex, Common.Options)]
    public static partial Regex TelephoneNumberE164();
    #endregion

    #region TelephoneNumberExtra
    /// <summary>
    /// Regular expression pattern which matches a telephone number expressed with the accepted extra characters:
    /// <c>-() </c> (incl. space).
    /// </summary>
    public const string TelephoneNumberExtraRex = $$"""
                                                    \+?{{NzDigitChar}}
                                                    (?:{{telephoneSeparatorRex}}* {{DigitChar}}){2,14}
                                                    {{telephoneSeparatorRex}}*
                                                    """;

    /// <summary>
    /// Regular expression pattern which matches a string that represents a telephone number expressed with the accepted
    /// extra characters: <c>-() </c> (incl. space).
    /// </summary>
    public const string TelephoneNumberExtraRegex = $@"^{TelephoneNumberExtraRex}$";

    /// <summary>
    /// Gets a Regex object which matches a string representing a telephone number expressed with the accepted extra characters:
    /// <c>-() </c> (incl. space).
    /// </summary>
    [GeneratedRegex(TelephoneNumberExtraRegex, Common.Options)]
    public static partial Regex TelephoneNumberExtra();
    #endregion

    /// <summary>
    /// Class US defines USA specific regular expressions
    /// </summary>
    public static partial class US
    {
        /// <summary>
        /// The name of a matching group representing an area code.
        /// </summary>
        public const string AreaGr = "area";

        /// <summary>
        /// The name of a matching group representing an 7-digit telephone number.
        /// </summary>
        public const string NumberGr = "number";

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
            (?<{{AreaGr}}>   [2-9] (?!11) {{DigitChar}}{2} ) {{telephoneSeparatorRex}}*
            (?<{{NumberGr}}> [2-9]        {{DigitChar}}{2}   {{telephoneSeparatorRex}}* {{DigitChar}}{4} {{telephoneSeparatorRex}}* )
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

        /// <summary>
        /// Gets a Regex object which matches a string representing a US telephone number.
        /// </summary>
        [GeneratedRegex(TelephoneNumberRegex, Common.Options)]
        public static partial Regex TelephoneNumber();
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
             (?:\+?1)? {{Space}}? \( (?<{{AreaGr}}> [2-9] (?!11) {{DigitChar}}{2} ) \)
             {{Space}}? (?<{{NumberGr}}> [2-9]{{DigitChar}}{2} \-?
             {{DigitChar}}{4} {{telephoneSeparatorRex}}* )
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

        /// <summary>
        /// Gets a Regex object which matches a string representing a US telephone number.
        /// </summary>
        [GeneratedRegex(TelephoneNumberStrictRegex, Common.Options)]
        public static partial Regex TelephoneNumberStrict();
        #endregion

        #region StateCode
        /// <summary>
        /// Regular expression pattern which matches a state code in a string.
        /// </summary>
        public const string StateCodeRex = $@"{HighAlphaChar}{{2}}";

        /// <summary>
        /// Regular expression pattern which matches a string that represents a state code.
        /// </summary>
        public const string StateCodeRegex = $@"^{StateCodeRex}$";

        /// <summary>
        /// Gets a Regex object which matches a string representing a state code.
        /// </summary>
        [GeneratedRegex(StateCodeRegex, Common.Options)]
        public static partial Regex StateCode();

        /// <summary>
        /// Gets a Regex object which matches a string representing a state code.
        /// </summary>
        /// <remarks>
        /// Note that this expression matches upper-case letters only as defined by the standard. Combine with <c>"(?i)"</c>
        /// or use <see cref="RegexOptions.IgnoreCase"/> to match case-insensitively.
        /// </remarks>
        [GeneratedRegex(StateCodeRegex, Common.OptionsI)]
        public static partial Regex StateCodeI();
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
        public const string ZipCode5Rex = $@"{DigitChar}{{5}}";

        /// <summary>
        /// Regular expression pattern which matches a string that represents a US postal code (ZIP code).
        /// <para>Named groups: <see cref="ZipGr"/> and <see cref="ExtGr"/>.</para>
        /// </summary>
        /// <remarks>
        /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
        /// </remarks>
        public const string ZipCode5Regex = $@"^(?<{ZipGr}> {ZipCode5Rex} )$";

        /// <summary>
        /// Gets a Regex object which matches a string representing a US postal code (ZIP code).
        /// </summary>
        [GeneratedRegex(ZipCode5Regex, Common.Options)]
        public static partial Regex ZipCode5();
        #endregion

        #region ZipCode5x4
        /// <summary>
        /// Regular expression pattern which matches a 5+4 US postal code (ZIP code) in a string.
        /// <para>Named groups: <see cref="ZipGr"/> and <see cref="ExtGr"/>.</para>
        /// </summary>
        /// <remarks>
        /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
        /// </remarks>
        public const string ZipCode5x4Rex = $@"(?<{ZipGr}> {DigitChar}{{5}} )( - (?<{ExtGr}> {DigitChar}{{4}} ) )";

        /// <summary>
        /// Regular expression pattern which matches a string that represents a 5+4 US postal code (ZIP code).
        /// </summary>
        public const string ZipCode5x4Regex = $@"^{ZipCode5x4Rex}$";

        /// <summary>
        /// Gets a Regex object which matches a string representing a 5+4 US postal code (ZIP code).
        /// </summary>
        [GeneratedRegex(ZipCode5x4Regex, Common.Options)]
        public static partial Regex ZipCode5x4();
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
        public const string ZipCode5o4Rex = $@"(?<{ZipGr}>{DigitChar}{{5}}) (?: - (?<{ExtGr}> {DigitChar}{{4}} ) )?";

        /// <summary>
        /// Regular expression pattern which matches a string that represents a 5 (+4 optional) US postal code (ZIP code).
        /// </summary>
        public const string ZipCode5o4Regex = $@"^{ZipCode5o4Rex}$";

        /// <summary>
        /// Gets a Regex object which matches a string representing a 5 (+4 optional) US postal code (ZIP code).
        /// </summary>
        [GeneratedRegex(ZipCode5o4Regex, Common.Options)]
        public static partial Regex ZipCode5o4();
        #endregion

        #region SocialSecurityNumber
        /// <summary>
        /// Regular expression pattern which matches a social security number in a string with optional SSN groups separator '-'.
        /// </summary>
        public const string SocialSecurityNumberRex = "(?: (?! [8-9] | 7[8-9] | 77[3-9] | 000 | 666 | 001-?01-?0001 | 078-?05-?1120 | 433-?54-?3937)" +     // well-known invalid patterns and numbers
                                                      $@"{DigitChar}{{3}} ) -? (?: (?! 00) {DigitChar}{{2}} ) -? (?: (?! 0000 ) {DigitChar}{{4}} )";

        /// <summary>
        /// Regular expression pattern which matches a string that represents a social security number.
        /// </summary>
        public const string SocialSecurityNumberRegex = $@"^{SocialSecurityNumberRex}$";

        /// <summary>
        /// Gets a Regex object which matches a string representing a social security number.
        /// </summary>
        [GeneratedRegex(SocialSecurityNumberRegex, Common.Options)]
        public static partial Regex SocialSecurityNumber();
        #endregion

        #region Itin
        /// <summary>
        /// Regular expression pattern which matches a Individual Taxpayer Identification Number in a string with optional SSN groups separator '-'.
        /// </summary>
        public const string ItinRex = $@"9{DigitChar}{{2}} -? [7-9]{DigitChar} -? {DigitChar}{{4}}";

        /// <summary>
        /// Regular expression pattern which matches a string that represents a Individual Taxpayer Identification Number.
        /// </summary>
        public const string ItinRegex = $@"^{ItinRex}$";

        /// <summary>
        /// Gets a Regex object which matches a string representing a Individual Taxpayer Identification Number.
        /// </summary>
        [GeneratedRegex(ItinRegex, Common.Options)]
        public static partial Regex Itin();
        #endregion
    }
}
