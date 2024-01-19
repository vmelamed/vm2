namespace vm2.RegexLib;

/// <summary>
/// Class SemVer. Contains regular expressions that match semantic version strings. See https://semver.org
/// </summary>
public static class SemVer
{
    /// <summary>
    /// The the name of a matching group representing the major version in a SemVer.
    /// </summary>
    public const string MajorGr = "major";

    /// <summary>
    /// The the name of a matching group representing the minor version in a SemVer.
    /// </summary>
    public const string MinorGr = "minor";

    /// <summary>
    /// The the name of a matching group representing the patch version in a SemVer.
    /// </summary>
    public const string PatchGr = "patch";

    /// <summary>
    /// The core gr
    /// </summary>
    public const string CoreGr = "core";

    /// <summary>
    /// The the name of a matching group representing the core version in a SemVer (maj.min.patch only).
    /// </summary>
    public const string PreReleaseGr = "pre";

    /// <summary>
    /// The the name of a matching group representing the build meta-data in a SemVer.
    /// </summary>
    public const string BuildGr = "build";

    /// <summary>
    /// BNF: <c>letter-chars = A | B | ... | Z | a | b | ... | z </c>
    /// </summary>
    const string letterChars = Ascii.AlphaChars;

    /// <summary>
    /// BNF: <c>positive-digit = 1 | 2 | ... | 9 </c>
    /// </summary>
    const string positiveDigitChars = Ascii.NonZeroDigitChars;

    /// <summary>
    /// BNF: <c>positive-digit = 1 | 2 | ... | 9 </c>
    /// </summary>
    const string positiveDigit = $@"[{positiveDigitChars}]";

    /// <summary>
    /// BNF: <c>digit = 0 | positive-digit </c>
    /// </summary>
    const string digitChars  = Ascii.DigitChars;

    /// <summary>
    /// BNF: <c>digit = 0 | positive-digit </c>
    /// </summary>
    const string digit  = Ascii.DigitRex;

    /// <summary>
    /// BNF: <c>non-digit = letter | - </c>
    /// </summary>
    const string nonDigitChars = $@"{letterChars}\-";

    /// <summary>
    /// BNF: <c>non-digit = letter | - </c>
    /// </summary>
    const string nonDigit = $@"[{nonDigitChars}]";

    /// <summary>
    /// BNF: <c>id-char = digit | non-digit</c>
    /// </summary>
    const string idChars = $@"{digitChars}{nonDigitChars}";

    /// <summary>
    /// BNF: <c>id-chars = id-char | id-char id-chars</c>
    /// </summary>
    const string idCharacter = $@"[{idChars}]";

    /// <summary>
    /// BNF: <c>numeric-id = 0 | positive-digit | positive-digit digits </c>
    /// </summary>
    const string numericId = $@"0 | {positiveDigit} {digit}*";

    /// <summary>
    /// BNF: <c>alpha-numeric-identifier = non-digit | non-digit id-chars | id-chars non-digit | id-chars non-digit id-chars </c>
    /// (alpha-numeric-identifier can be anything with at least one non-digit.)
    /// </summary>
    const string alphaNumericId = $@"(?: {numericId} | {digit}* {nonDigit} {idCharacter}* )";

    /// <summary>
    /// BNF: <c>build-identifier = alpha-numeric-identifier | digits</c>
    /// buildIdentifier can be any identifier
    /// </summary>
    const string buildIdentifier = $@"{alphaNumericId}";

    /// <summary>
    /// BNF: <c>dot-separated-build-identifiers = build-identifier | build-identifier . dot-separated-build-identifiers </c>
    /// </summary>
    const string buildIdentifiers = $@"{buildIdentifier} (?: \. {buildIdentifier} )*";

    /// <summary>
    /// BNF: <c>build = dot-separated-build-identifiers</c>
    /// </summary>
    const string build = $@"(?<{BuildGr}> {buildIdentifiers} )";

    /// <summary>
    /// BNF: <c>pre-release-identifier = alpha-numeric-identifier | digits</c>
    /// pre-release-identifier can be any identifier
    /// </summary>
    const string preReleaseIdentifier = $@"{alphaNumericId}";

    /// <summary>
    /// BNF: <c>dot-separated-pre-release-identifiers = build-identifier | build-identifier . dot-separated-build-identifiers </c>
    /// </summary>
    const string preReleaseIdentifiers = $@"{preReleaseIdentifier} (?: \. {preReleaseIdentifier} )*";

    /// <summary>
    /// BNF: <c>pre-release = dot-separated-pre-release-identifiers</c>
    /// </summary>
    const string preRelease = $@"(?<{PreReleaseGr}> {preReleaseIdentifiers} )";

    /// <summary>
    /// BNF: <c>patch = numeric-id</c>
    /// </summary>
    const string patch = $@"(?<{PatchGr}> {numericId} )";

    /// <summary>
    /// BNF: <c>minor = numeric-id</c>
    /// </summary>
    const string minor = $@"(?<{MinorGr}> {numericId} )";

    /// <summary>
    /// BNF: <c>major = numeric-id</c>
    /// </summary>
    const string major = $@"(?<{MajorGr}> {numericId} )";

    /// <summary>
    /// BNF: <c>version-core = major . minor . patch</c>
    /// </summary>
    const string versionCore = $@"(?<{CoreGr}> {major}.{minor}.{patch} )";

    /// <summary>
    /// Gets the regular expression options for parsing SemVer strings.
    /// </summary>>
    public static RegexOptions SemVerOptions => RegexOptions.Compiled
                                                | RegexOptions.CultureInvariant
                                                | RegexOptions.IgnorePatternWhitespace
                                                | RegexOptions.IgnoreCase;

    #region The Regex object
    /// <summary>
    /// Regular expression pattern which matches a valid SemVer in an input string.
    /// <para>
    /// BNF: <c>semver = version-core | version-core - pre-release | version-core + build | version-core - pre-release + build </c>
    /// </para>
    /// </summary>
    public const string SemVerRex = $@"{versionCore} (?: \- {preRelease} )? (?: \+ {build} )?";

    /// <summary>
    /// Regular expression pattern which matches a string representing a valid SemVer.
    /// </summary>
    public const string SemVerRegex = $@"^{SemVerRex}$";

    static readonly Lazy<Regex> regexSemVer = new(() => new(SemVerRegex, SemVerOptions));

    /// <summary>
    /// Gets a Regex object which matches the entire input string against the pattern &lt;see cref="SemVerRegex" /&gt;
    /// </summary>
    public static Regex Regex => regexSemVer.Value;
    #endregion
}
