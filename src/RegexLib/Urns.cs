namespace vm2.RegexLib;

/// <summary>
/// Class Urns. Defines regular expressions for Uniform Resource Names (URNs) in accordance with RFC 8141 and RFC 1737.
/// Follows closely the definitions in
/// https://datatracker.ietf.org/doc/html/rfc8141
/// https://datatracker.ietf.org/doc/html/rfc1737
/// </summary>
public static partial class Urns
{
    /// <summary>
    /// Matches the URN schema.
    /// </summary>
    public const string Schema = "urn";

    const string alphanumericHyphenChars = $@"{Ascii.AlphaNumericChars}-";

    const string alphanumericHyphenCharRex = $@"[{alphanumericHyphenChars}]";

    /// <summary>
    /// The name of the group that matches the URN namespace identifier (NID).
    /// </summary>
    public const string NidGr = "nid";

    /// <summary>
    /// Represents a regular expression pattern for matching a namespace identifier (NID).
    /// </summary>
    public const string NidRex = $@"(?<{NidGr}> (?! urn- )(?! {Ascii.AlphaChar}{{2}}- )(?! X- ) {Ascii.AlphaNumericChar} {alphanumericHyphenCharRex}{{1,30}} {Ascii.AlphaNumericChar} )";

    const string pSlChars   = $@"{Uris.PathChars}/";

    const string pSlChar    = $@"[{pSlChars}] | {Uris.PctEncoded}";

    const string pSlCharRex = $@"(?: {pSlChar} )";

    const string pCharRex   = Uris.PCharRex;

    /// <summary>
    /// The name of the group that matches the URN namespace specific string (NSS).
    /// </summary>
    public const string NssGr = "nss";

    const string nss = $@"(?<{NssGr}> {pCharRex} {pSlCharRex}* )";

    /// <summary>
    /// The name of the group that matches the URN namespace specific string (NSS).
    /// </summary>
    public const string AssignedNameGr = "a_name";

    /// <summary>
    /// Regular expression pattern for matching and extracting an assigned URN, including the scheme, namespace identifier (NID),
    /// and namespace-specific string (NSS).
    /// identifier, and namespace string.
    /// </summary>
    public const string AssignedNameRex = $@"(?<{AssignedNameGr}> {Schema} : {NidRex} : {nss} )";

    /// <summary>
    /// Represents the regular expression pattern used to validate assigned names.
    /// </summary>
    /// <remarks>The pattern ensures that assigned names conform to the expected format, which includes a
    /// schema, a namespace identifier (NID), and a namespace-specific string (NSS).
    /// <para>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </para>
    /// </remarks>
    public const string AssignedNameRegex = $@"^{AssignedNameRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents a URN, including the optional components.
    /// </summary>
    /// <returns></returns>
    [GeneratedRegex(AssignedNameRegex, Common.OptionsI)]
    public static partial Regex AssignedName();

    const string componentNpChars = $@"{pSlChars}\?";

    const string componentChars   = $@"[{componentNpChars}] | {Uris.PctEncoded}";

    const string componentChar    = $@"(?: {componentChars} )";

    const string componentRex     = $@"{pCharRex} {componentChar}*";

    /// <summary>
    /// The name of the group that matches the r-component of a URN.
    /// </summary>
    public const string RComponentGr = "r_component";

    const string rDelimiter = @"\?\+";
    const string rComponent = $@"{rDelimiter} (?<{RComponentGr}> {componentRex} )";

    /// <summary>
    /// The name of the group that matches the r-component of a URN.
    /// </summary>
    public const string QComponentGr = "q_component";

    const string qDelimiter = @"\?=";
    const string qComponent = $@"{qDelimiter} (?<{QComponentGr}> {componentRex} )";

    const string rqComponents = $"""
                                 (?:
                                     (?: {rComponent} )(?: {qComponent} ) |
                                     (?: {rComponent} (?<!.*{qDelimiter}) ) |
                                     (?: {qComponent} )
                                 )
                                 """;

    /// <summary>
    /// The name of the group that matches the f-component of a URN.
    /// </summary>
    public const string FComponentGr = "f_component";

    const string fDelimiter = @"\#";

    /// <summary>
    /// The f-component specifies a location or region within the named resource. It is introduced by the number sign ("#") and
    /// terminated by the end of the URN.
    /// </summary>
    const string fComponent = $@"(?: {fDelimiter} (?<{FComponentGr}> {Uris.FragmentCharRex}* ) )";

    /// <summary>
    /// The name of the group that matches a URN, including the optional components.
    /// </summary>
    public const string UrnGr = "urn";

    /// <summary>
    /// Represents a regular expression pattern for matching names, including the optional components, in a string.
    /// </summary>
    public const string NameStringRex = $@"(?<{UrnGr}> {AssignedNameRex} {rqComponents}? {fComponent}?)";

    /// <summary>
    /// Represents a regular expression pattern for matching names, including the optional components.
    /// </summary>
    public const string NameStringRegex = $@"^ {NameStringRex} $";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents a URN, including the optional components.
    /// </summary>
    [GeneratedRegex(NameStringRegex, Common.Options)]
    public static partial Regex NameString();
}
