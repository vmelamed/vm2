﻿namespace vm2.RegexLib;

/// <summary>
/// Class SemVer. Contains regular expressions that match semantic version strings. See https://semver.org
/// </summary>
public static partial class SemVer
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
    const string letterChars = AlphaChars;

    /// <summary>
    /// BNF: <c>positive-digit = 1 | 2 | ... | 9 </c>
    /// </summary>
    const string positiveDigitChars = NzDigitChars;

    /// <summary>
    /// BNF: <c>positive-digit = 1 | 2 | ... | 9 </c>
    /// </summary>
    const string positiveDigit = $@"[{positiveDigitChars}]";

    /// <summary>
    /// BNF: <c>digit = 0 | positive-digit </c>
    /// </summary>
    const string digitChars  = DigitChars;

    /// <summary>
    /// BNF: <c>digit = 0 | positive-digit </c>
    /// </summary>
    const string digit = DigitChar;

    /// <summary>
    /// BNF: <c>digit = 0 | positive-digit </c>
    /// </summary>
    const string digits = $"{digit}+";

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
    /// BNF: <c>id-chars = id-char | id-char id-chars</c>
    /// </summary>
    const string idCharacters = $@"{idCharacter}+";

    /// <summary>
    /// BNF: <c>numeric-id = 0 | positive-digit | positive-digit digits </c>
    /// </summary>
    const string numericId = $@"0 | {positiveDigit} {digit}*";

    /// <summary>
    /// BNF: <c>alpha-numeric-identifier = non-digit | non-digit id-chars | id-chars non-digit | id-chars non-digit id-chars </c>
    /// (alpha-numeric-identifier can be anything with at least one non-digit.)
    /// </summary>
    const string alphaNumericId = $"(?: {idCharacters} )";

    /// <summary>
    /// BNF: <c>pre-release-identifier = alpha-numeric-identifier | digits</c>
    /// pre-release-identifier can be any identifier
    /// </summary>
    const string preReleaseIdentifier = $@"{alphaNumericId} | {numericId}";

    /// <summary>
    /// BNF: <c>dot-separated-pre-release-identifiers = build-identifier | build-identifier . dot-separated-build-identifiers </c>
    /// </summary>
    const string preReleaseIdentifiers = $@"(?: {preReleaseIdentifier} ) (?: \. {preReleaseIdentifier} )*";

    /// <summary>
    /// BNF: <c>pre-release = dot-separated-pre-release-identifiers</c>
    /// </summary>
    const string preRelease = $@"(?<{PreReleaseGr}> {preReleaseIdentifiers} )";

    /// <summary>
    /// BNF: <c>build-identifier = alpha-numeric-identifier | digits</c>
    /// buildIdentifier can be any identifier
    /// </summary>
    const string buildIdentifier = $@"{alphaNumericId} | {numericId}";

    /// <summary>
    /// BNF: <c>dot-separated-build-identifiers = build-identifier | build-identifier . dot-separated-build-identifiers </c>
    /// </summary>
    const string buildIdentifiers = $@"(?: {buildIdentifier} ) (?: \. {buildIdentifier} )*";

    /// <summary>
    /// BNF: <c>build = dot-separated-build-identifiers</c>
    /// </summary>
    const string build = $@"(?<{BuildGr}> {buildIdentifiers} )";

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
    /// Regular expression pattern which matches a valid SemVer in an input string.
    /// <para>
    /// BNF: <c>semver = version-core | version-core - pre-release | version-core + build | version-core - pre-release + build </c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string SemVerRex = $@"{versionCore} (?: \- {preRelease} )? (?: \+ {build} )?";

    /// <summary>
    /// Regular expression pattern which matches a string representing a valid SemVer.
    /// <para>
    /// BNF: <c>semver = version-core | version-core - pre-release | version-core + build | version-core - pre-release + build </c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string SemVerRegex = $@"^{SemVerRex}$";

    /// <summary>
    /// Gets a Regex object which matches the entire input string against the pattern <see cref="SemVerRegex"/>
    /// <para>
    /// BNF: <c>semver = version-core | version-core - pre-release | version-core + build | version-core - pre-release + build </c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    [GeneratedRegex(SemVerRegex, Common.Options)]
    public static partial Regex SemanticVersion();
}
