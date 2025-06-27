namespace vm2.RegexLib;

/// <summary>
/// Class Uri. Defines many regular expressions with the ultimate goal to define a regular expression for URI.
/// Follows closely the definitions in
/// https://datatracker.ietf.org/doc/html/rfc1737
/// https://datatracker.ietf.org/doc/html/rfc2141
/// </summary>
public static partial class Urns
{
    //    <excluded> ::= octets 1-32 (1-20 hex) | "\" | """ | "&" | "<" | ">" | "[" | "]" | "^" | "`" | "{" | "|" | "}" | "~"
    //                   | octets 127-255 (7F-FF hex)
    const string excludedChars = @"[\\""&<>[]^`{|}~\x00-\x20\x7F-\xFF]";

    //    <reserved>    ::= '%" | "/" | "?" | "#"
    const string reservedChars = @"%/?\#";

    //    <other>       ::= "(" | ")" | "+" | "," | "-" | "." |
    //                      ":" | "=" | "@" | ";" | "$" |
    //                      "_" | "!" | "*" | "'"
    const string otherChars = @"\(\)\+,\-\.:\=@;\$!\'\*";

    //    trans         ::= alphanumeric | other | reserved
    const string transChars = $"{Ascii.AlphaNumericChars} {otherChars} {reservedChars}";

    //    URNchars      ::= trans | % hex hex
    const string urnChar = $"(?: [{transChars}] | {Uris.PctEncodedChar} )";

    /// <summary>
    /// The name of the group that matches the URN namespace specific string (NSS).
    /// </summary>
    public const string NssGr = "nss";

    //    NSS         ::= 1*URNchars
    const string nss = $"(?<{NssGr}> {urnChar}+ )";

    //    lnh         ::= alphanumeric | "-"
    const string alphanumericHyphenChars = $"{Ascii.HighAlphaNumericChars} -";

    /// <summary>
    /// The name of the group that matches the URN namespace identifier (NID).
    /// </summary>
    public const string NidGr = "nid";

    //    NID         ::= alphanumeric [1, 31 lnh ]
    const string nid = $"(?<{NidGr}> [{alphanumericHyphenChars}] {{1,31}} )";

    /// <summary>
    /// Matches the URN schema.
    /// </summary>
    public const string Schema = "urn";

    /// <summary>
    /// Matches a URN in a string.
    /// <para>BNF: <c>URN := "urn:" NID ":" NSS</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string UrnRex = $@"(?<urn> {Schema} : {nid} : {nss} )";

    /// <summary>
    /// A regular expression that matches a string that represents a URN.
    /// </summary>
    public const string UrnRegex = $@"^(?!{excludedChars}) (?<urn> {Schema} : {nid} : {nss} )$";

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a URN.
    /// </summary>
    [GeneratedRegex(UrnRegex, Common.Options)]
    public static partial Regex Urn();
}
