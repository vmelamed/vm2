﻿namespace vm2.RegexLib;

/// <summary>
/// Class Uri. Defines many regular expressions with the ultimate goal to define a regular expression for URI.
/// Follows closely the definitions in
/// https://datatracker.ietf.org/doc/html/rfc3986
/// https://datatracker.ietf.org/doc/html/rfc1738
/// https://datatracker.ietf.org/doc/html/rfc1034
/// https://datatracker.ietf.org/doc/html/rfc1123
/// https://datatracker.ietf.org/doc/html/rfc952
/// https://datatracker.ietf.org/doc/html/rfc3513
/// </summary>
public static partial class Uris
{
    #region URI character sets
    /// <summary>
    /// Matches a percent encoded character.
    /// <para>BNF: <c>pct-encoded := % hex_digit hex_digit</c></para>
    /// </summary>
    public const string PctEncoded = $"%{Numerical.HexDigitChar}{Numerical.HexDigitChar}";

    /// <summary>
    /// Matches a percent encoded character.
    /// <para>BNF: <c>pct-encoded := % hex_digit hex_digit</c></para>
    /// </summary>
    public const string PctEncodedChar = $"(?:{PctEncoded})";

    /// <summary>
    /// The URI general delimiters.
    /// <para>BNF: <c>gen-delims  = ":" | "/" | "?" | "#" | "[" | "]" | "@"</c></para>
    /// </summary>
    const string genDelimiterChars = @":/\?\#\[\]@";

    /// <summary>
    /// The sub-delimiters without the equals and ampersand characters.
    /// <para>BNF: <c>sub-delims-no-eq-no-amp = "!" | "$" | "'" | "(" | ")" | "*" | "+" | "," | ";"</c></para>
    /// </summary>
    const string subDelimiterNoEqAmpChars = @"!\$'\(\)\*\+,;";

    /// <summary>
    /// The sub-delimiters.
    /// <para>BNF: <c>sub-delims = sub-delims-no-eq-no-amp | "=" | "&amp;"</c></para>
    /// </summary>
    public const string SubDelimiterChars = $@"{subDelimiterNoEqAmpChars}=&";

    /// <summary>
    /// The reserved chars.
    /// <para>BNF: <c>reserved = gen-delims | sub-delims</c></para>
    /// </summary>
#pragma warning disable IDE0051 // Remove unused private members
    const string reservedChars = $@"{genDelimiterChars}{SubDelimiterChars}";
#pragma warning restore IDE0051 // Remove unused private members

    /// <summary>
    /// The characters that are allowed in a URI but do not have a reserved purpose.
    /// <para>BNF: <c>unreserved  = ALPHA | DIGIT | "-" | "." | "_" | "~"</c></para>
    /// </summary>
    public const string UnreservedChars = $@"{Ascii.AlphaNumericChars}\-\._~";

    /// <summary>
    /// The unreserved or sub-delimiter chars
    /// </summary>
    public const string UnreservedOrSubDelimiterChars = $"{UnreservedChars}{SubDelimiterChars}";

    /// <summary>
    /// Matches an unreserved or sub-delimiter character.
    /// <para>BNF: <c>unreserved-or-sub-delimiter := unreserved | sub-delimiter</c></para>
    /// </summary>
    const string unreservedOrSubDelimiterChar = $"[{UnreservedOrSubDelimiterChars}]";
    #endregion

    #region Scheme
    /// <summary>
    /// The name of a matching group representing the scheme of a URI.
    /// </summary>
    public const string SchemeGr = "scheme";

    /// <summary>
    /// The scheme characters
    /// <para>BNF: <c>scheme-char = ALPHA | DIGIT | "+" | "-" | "."</c></para>
    /// </summary>
    const string schemeChars = $@"{Ascii.AlphaNumericChars}\.\+\-";

    const string schemeRex = $@"[{schemeChars}]*";

    /// <summary>
    /// Matches a URI scheme.
    /// <para>BNF: <c>scheme := ALPHA *( ALPHA | DIGIT | "+" | "-" | "." )</c></para>
    /// <para>Named groups: <see cref="SchemeGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string SchemeRex = $@"(?<{SchemeGr}> {Ascii.AlphaChar} {schemeRex} )";

    /// <summary>
    /// Matches a string that represents a URI scheme.
    /// <para>BNF: <c>scheme := alpha 1*[ alpha | digit | + | - | . ]</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string SchemeRegex = $@"^{SchemeRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents a URI scheme.
    /// <para>BNF: <c>scheme := alpha 1*[ alpha | digit | + | - | . ]</c></para>
    /// </summary>
    [GeneratedRegex(SchemeRegex, Common.OptionsI)]
    public static partial Regex Scheme();
    #endregion

    #region Host
    /// <summary>
    /// The name of a matching group representing an IP general name.
    /// </summary>
    public const string IpGenNameGr = "ipGenName";

    /// <summary>
    /// Matches reg-name in RFC 3986
    /// <para>BNF: <c>registered_name = *[ unreserved | sub-delimiters | pct-encoded ]</c> - yes, it can be empty, see the RFC</para>
    /// </summary>
    const string generalNameRex = $@"(?<{IpGenNameGr}> (?: {unreservedOrSubDelimiterChar} | {PctEncodedChar} )+ )";

    /// <summary>
    /// Matches a registered name.
    /// <para>Named groups: <see cref="Net.IpDnsNameGr"/>, <see cref="IpGenNameGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string registeredNameRex = $@"{Net.DnsNameRex} | {generalNameRex}";

    /// <summary>
    /// The name of a matching group representing a host name.
    /// </summary>
    public const string HostGr = Net.HostGr;

    /// <summary>
    /// Matches a host in a string.
    /// <para>BNF: <c>host := IP-literal | IPv4address | reg-name</c></para>
    /// <para>
    /// Named groups: <see cref="HostGr"/>, and one of: <see cref="IpGenNameGr"/>, <see cref="Net.Ipv4Gr"/>,
    /// <see cref="Net.Ipv6NzGr"/> or <see cref="Net.IpvfGr"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string HostRex = $@"(?<{HostGr}> {Net.IpNumericAddressRex} | {registeredNameRex} )";

    /// <summary>
    /// Matches a string that represents a host.
    /// <para>BNF: <c>host := IP-literal | IPv4address | reg-name</c></para>
    /// <para>
    /// Named groups: <see cref="HostGr"/>, and one of: <see cref="Net.Ipv4Gr"/>, <see cref="Net.Ipv6NzGr"/>,
    /// <see cref="Net.IpvfGr"/>, <see cref="Net.IpDnsNameGr"/>, <see cref="IpGenNameGr"/>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string HostRegex = $@"^{HostRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents a host.
    /// <para>BNF: <c>host := IP-literal | IPv4address | reg-name</c></para>
    /// <para>
    /// Named groups: <see cref="HostGr"/>, and one of: <see cref="Net.Ipv4Gr"/>, <see cref="Net.Ipv6NzGr"/>,
    /// <see cref="Net.IpvfGr"/>, <see cref="Net.IpDnsNameGr"/>, <see cref="IpGenNameGr"/>
    /// </para>
    /// </summary>
    [GeneratedRegex(HostRegex, Common.OptionsI)]
    public static partial Regex Host();
    #endregion

    #region Endpoint (Host:Port)
    /// <summary>
    /// The name of a matching group representing an IP endpoint.
    /// </summary>
    public const string EndpointGr = "endpoint";

    /// <summary>
    /// Matches an IP endpoint.
    /// <para>BNF: <c>endpoint := host [: port]</c></para>
    /// <para>Named groups: <see cref="EndpointGr"/> <see cref="HostGr"/>, <see cref="Net.PortGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string EndpointRex = $"(?<{EndpointGr}> {HostRex} (?: : {Net.PortRex})? )";

    /// <summary>
    /// Matches a string that represents an IP endpoint.
    /// <para>BNF: <c>endpoint := host [: port]</c></para>
    /// <para>Named groups: <see cref="HostGr"/>, <see cref="Net.PortGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string EndpointRegex = $"^{EndpointRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents an IP endpoint.
    /// <para>BNF: <c>endpoint := host [: port]</c></para>
    /// <para>Named groups: <see cref="HostGr"/>, <see cref="Net.PortGr"/>.</para>
    /// </summary>
    /// <value>The endpoint.</value>
    [GeneratedRegex(EndpointRegex, Common.OptionsI)]
    public static partial Regex Endpoint();
    #endregion

    #region Authority
    /// <summary>
    /// The name of a matching group representing the user name.
    /// </summary>
    public const string UserNameGr = "user";

    /// <summary>
    /// The name of a matching group representing the part of the authority that specifies scheme-specific information
    /// about how to gain authorization to access the resource.
    /// </summary>
    /// <remarks>
    /// Use of the format "user:password" in the user-info field is deprecated.  Applications should not render as clear
    /// text any data after the first colon(":") character found within a user-info sub-component unless the data after
    /// the colon is the empty string (indicating no password).  Applications may choose to ignore or reject such data
    /// when it is received as part of a reference and should reject the storage of such data in unencrypted form.The
    /// passing of authentication information in clear text has proven to be a security risk in almost every case where
    /// it has been used.
    /// </remarks>
    public const string AccessGr = "access";

    /// <summary>
    /// Matches URI's user info (name and password).
    /// <para>BNF: <c>user-info := *[ unreserved | sub-delimiters | : | pct-encoded ]</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// <para>
    /// Note: the use of the format 'user:password' in the 'userInfo' field is deprecated.  Applications should not
    /// render as clear text any data after the first colon (':') character found within a 'userInfo' sub-component
    /// unless the data after the colon is the empty string (indicating no password).  Applications may choose to ignore
    /// or reject such data when it is received as part of a reference and should reject the storage of such data in
    /// un-encrypted form.
    /// </para>
    /// <para>
    /// </para>
    /// </remarks>
    public const string UserInfoRex = $"""
                                       (?:
                                         (?<{UserNameGr}> (?:{unreservedOrSubDelimiterChar} | {PctEncodedChar})+ )
                                         :?
                                         (?<{AccessGr}>   (?:{unreservedOrSubDelimiterChar} | {PctEncodedChar})* )
                                       )
                                       """;

    /// <summary>
    /// authority in a URI.
    /// </summary>
    public const string AuthorityGr = "authority";

    /// <summary>
    /// Matches the URI's authority.
    /// <para>BNF: <c>authority := [ user-info @ ] host [ : port ]</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string AuthorityRex = $"(?<{AuthorityGr}> (?: {UserInfoRex} @ )? {EndpointRex} )";

    /// <summary>
    /// Matches the URI's authority with network address.
    /// <para>BNF: <c>authority := [ user-info @ ] host [ : port ]</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string NetAuthorityRex = $"(?<{AuthorityGr}> (?: {UserInfoRex} @ )? {Net.EndpointRex} )";
    #endregion

    #region Path
    /// <summary>
    /// Path characters (without the colon char).
    /// <para>BNF: <c>path-nc-chars := unreserved | sub-delimiters | @</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathNcChars = $"{UnreservedOrSubDelimiterChars}@";

    /// <summary>
    /// Matches a character from a path no colon char (NC).
    /// <para>BNF: <c>path-nc-chars := unreserved | sub-delimiters | @</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathNcChar = $"[{UnreservedOrSubDelimiterChars}@]";

    /// <summary>
    /// Matches a character from a path no colon char (NC).
    /// <para>BNF: <c>path-nc-char := unreserved | sub-delimiters | @ | pct-encoded</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathNcCharRex = $"(?: {pathNcChar} | {PctEncodedChar} )";

    /// <summary>
    /// Matches non-zero length (NZ) path segment no colon (NC)
    /// <para>BNF: <c>segment-nz-nc := 1*( unreserved | sub-delimiters | @ | pct-encoded )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathSegmentNzNcRex = $"{pathNcCharRex}+";

    /// <summary>
    /// Path characters (incl. the colon char).
    /// <para>BNF: <c>path-nc-chars := path-nc-chars | :</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PathChars = $"{pathNcChars}:";

    /// <summary>
    /// Path characters (incl. the colon char).
    /// <para>BNF: <c>path-nc-chars := path-nc-chars | :</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PathChar = $"[{PathChars}]";

    /// <summary>
    /// Matches a path character (incl. the colon char).
    /// <para>BNF: <c>pchar := path-nc-char | : | pct-encoded</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PChar = $"{PathChar} | {PctEncoded}";

    /// <summary>
    /// Matches a path character (incl. the colon char).
    /// <para>BNF: <c>pchar := path-nc-char | : | pct-encoded</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PCharRex = $"(?: {PChar} )";

    /// <summary>
    /// Matches non-zero length path segment incl. colon
    /// <para>BNF: <c>segment-nz := 1*( unreserved | sub-delimiters | @ | : | pct-encoded )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathSegmentNzRex = $"{PCharRex}+";

    /// <summary>
    /// Matches path segment incl. colon (can be empty)
    /// <para>BNF: <c>segment := *( unreserved | sub-delimiters | @ | : | pct-encoded )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathSegmentRex = $"{PCharRex}*";

    /// <summary>
    /// The name of a matching group representing a URI path
    /// </summary>
    public const string PathRootlessGr = "pathRootless";

    /// <summary>
    /// Matches path without the root ('/').
    /// <para>BNF: <c>path-rootless := segment-nz *( / segment )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathRootlessRex = $"(?<{PathRootlessGr}> {pathSegmentNzRex} (?: / {pathSegmentRex} )* )";

    /// <summary>
    /// The name of a matching group representing a no-scheme URI path
    /// </summary>
    public const string PathNoSchemeGr = "pathNoScheme";

    /// <summary>
    /// Matches a path with no scheme (no colon).
    /// <para>BNF: <c>path-no-scheme := segment-nz-nc *( / segment )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathNoSchemeRex = $"(?<{PathNoSchemeGr}> {pathSegmentNzNcRex} (?: / {pathSegmentRex} )* )";

    /// <summary>
    /// The name of a matching group representing a URI path
    /// </summary>
    public const string PathAbsGr = "pathAbs";

    /// <summary>
    /// Matches an absolute path starting with '/' but not '//'.
    /// <para>BNF: <c>path-absolute := / path-rootless</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathAbsoluteRex = $"(?<{PathAbsGr}> / (?: {pathSegmentNzRex} (?: / {pathSegmentRex} )* )? )";

    /// <summary>
    /// The name of a matching group representing an absolute or empty URI path
    /// </summary>
    public const string PathAbsEmptyGr = "pathAbsEmpty";

    /// <summary>
    /// Matches an absolute or empty path.
    /// <para>BNF: <c>path-abs-empty := *( / segment )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathAbsoluteOrEmptyRex = $"(?<{PathAbsEmptyGr}> (?: / {pathSegmentRex} )* )";

    /// <summary>
    /// The name of a matching group representing the path in a URI
    /// </summary>
    public const string UriPathGr = "path";

    /// <summary>
    /// Matches a URI path.
    /// <para>BNF:
    /// <code>
    /// path = path-abs-empty  | ; begins with / or is empty
    ///        path-absolute   | ; begins with / but not //
    ///        path-no-scheme  | ; begins with a non-colon segment
    ///        path-rootless   | ; begins with a segment
    ///        path-empty        ; zero characters (???)</code></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathRex = $"""
                            (?<{UriPathGr}>
                                {pathAbsoluteOrEmptyRex} |
                                {pathAbsoluteRex} |
                                {pathNoSchemeRex} |
                                {pathRootlessRex} |
                            )?
                            """;

    /// <summary>
    /// Matches a string that represents a URI's path.
    /// <para>BNF:
    /// <code>
    /// path = path-abs-empty  | ; begins with / or is empty
    ///        path-absolute   | ; begins with / but not //
    ///        path-no-scheme  | ; begins with a non-colon segment
    ///        path-rootless   | ; begins with a segment
    ///        path-empty        ; zero characters (???)</code></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PathRegex = $"^{pathRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents a URI's path.
    /// <para>BNF:
    /// <code>
    /// path = path-abs-empty  | ; begins with / or is empty
    ///        path-absolute   | ; begins with / but not //
    ///        path-no-scheme  | ; begins with a non-colon segment
    ///        path-rootless   | ; begins with a segment
    ///        path-empty        ; zero characters (???)</code></para>
    /// </summary>
    [GeneratedRegex(PathRegex, Common.OptionsI)]
    public static partial Regex Path();
    #endregion

    #region General Query
    /// <summary>
    /// The generic query chars.
    /// <para>BNF: <c>generic-query-chars := a-zA-Z0-9._~-!$'()*+,;=&amp;@:/?</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string queryChars = $@"{PathChars}/\?";

    /// <summary>
    /// Matches a generic query char.
    /// <para>BNF: <c>generic-query-char := generic-query-chars | pct-encoded</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string queryCharRex = $"(?: [{queryChars}] | {PctEncodedChar} )";

    /// <summary>
    /// The name of a matching group representing the query in a URI
    /// </summary>
    public const string QueryGr = "query";

    /// <summary>
    /// Matches a URI query.
    /// <para>BNF: <c>gen-query := *( path-char | / | ? )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string queryRex = $@"(?<{QueryGr}> {queryCharRex}* )";

    /// <summary>
    /// Matches a string that represents a generic query.
    /// <para>BNF: <c>gen-query := *( path-char | / | ? )</c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string QueryRegex = $@"^{queryRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents a generic query.
    /// </summary>
    [GeneratedRegex(QueryRegex, Common.OptionsI)]
    public static partial Regex Query();
    #endregion

    #region Key-Value query
    /// <summary>
    /// Key-value query character set
    /// <para>
    /// BNF: <c>key-value-chars := path-char | / | ? </c>
    /// </para>
    /// </summary>
    const string kvChars = $@"{UnreservedChars}{subDelimiterNoEqAmpChars}@:/\?";

    /// <summary>
    /// Key character set
    /// <para>
    /// BNF: <c>key-chars := key-value-chars | &amp; </c>
    /// </para>
    /// </summary>
    const string keyChars = $@"{kvChars}&";

    /// <summary>
    /// Key character
    /// <para>
    /// BNF: <c>key-chars := key-char | pct-encoded </c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string keyCharRex = $@"(?: [{keyChars}] | {PctEncodedChar} )";

    /// <summary>
    /// Matches a key from a URI key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string queryKeyRex   = $"( {keyCharRex}+ )";

    /// <summary>
    /// Value query character set
    /// <para>
    /// BNF: <c>value-chars := key-value-chars | = </c>
    /// </para>
    /// </summary>
    const string valueChars = $@"{kvChars}=";

    /// <summary>
    /// Key character
    /// <para>
    /// BNF: <c>value-char := value-char | pct-encoded </c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string valueCharRex = $@"(?: [{valueChars}] | {PctEncodedChar} )";

    /// <summary>
    /// Matches a value from a URI key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string queryValueRex = $"( {valueCharRex}* )";

    /// <summary>
    /// Matches a URI's key-value query
    /// <para>BNF: <c>key-value-query := key = value *(&amp; key = value)</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string queryKvPairRex = $"{queryKeyRex} = {queryValueRex}";

    /// <summary>
    /// The name of a matching group representing a key-value query in a URI
    /// </summary>
    public const string KvQueryGr = "kvQuery";

    /// <summary>
    /// Matches a URI's key-value query.
    /// <para>BNF: <c>key-value-query := key = value *(&amp; key = value)</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// You can get up to 10 key-value pair URI parameters and they are in capturing groups 1 and 2,  3 and 4, ... 19 and 20
    /// </remarks>
    const string queryKvRex = $"(?<{KvQueryGr}> (?: {queryKvPairRex} ) (?: & {queryKvPairRex} (?: & {queryKvPairRex} (?: & {queryKvPairRex} (?: & {queryKvPairRex} (?: & {queryKvPairRex} (?: & {queryKvPairRex} (?: & {queryKvPairRex} (?: & {queryKvPairRex} (?: & {queryKvPairRex} )? )? )? )? )? )? )? )? )? )?";

    /// <summary>
    /// Matches a string that represents a URI's key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string KeyValueQueryRegex = $"^{queryKvRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents a URI's key-value query
    /// </summary>
    [GeneratedRegex(KeyValueQueryRegex, Common.OptionsI)]
    public static partial Regex KvQuery();
    #endregion

    #region Fragment
    const string fragmentChars = $@"{PathChars}/\?";

    /// <summary>
    /// Matches a character from a URI fragment
    /// <para>BNF: <c>fragment-char := fragment-chars | pct-encoded</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    /// <remarks>
    /// Note: the fragment is not percent-encoded, so it can contain characters that are not allowed in the query.
    /// </remarks>
    public const string FragmentCharRex = $@"(?: [{fragmentChars}] | {PctEncodedChar} )";

    /// <summary>
    /// The name of a matching group representing a URI fragment
    /// </summary>
    public const string FragmentGr = "fragment";

    /// <summary>
    /// Matches a URI fragment.
    /// <para>BNF: <c>fragment := *( fragment-char )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string FragmentRex = $"(?<{FragmentGr}> {FragmentCharRex}*)";
    #endregion

    #region Relative URI
    /// <summary>
    /// Matches the URI relative part
    /// <para>
    /// BNF: <c>relative-part = "//" authority path-abempty | path-absolute | path-noscheme | path-empty</c>
    /// </para>
    /// </summary>
    const string relativeUriPartRex = $"(?: (?: // {AuthorityRex} {pathAbsoluteOrEmptyRex} ) | {pathAbsoluteRex} | {pathNoSchemeRex} )?";

    #region RelativeUriKvQueryRef
    /// <summary>
    /// The name of a matching group representing a relative URI with optional general query
    /// </summary>
    public const string RelativeUriKvGr = "relUriKv";

    /// <summary>
    /// Matches the URI relative reference with an optional key-value query in a string
    /// <para>
    /// BNF: <c>relative-ref = relative-part [ "?" query-kv ] [ "#" fragment ]</c>
    /// </para>
    /// </summary>
    const string relativeUriKvQueryRefRex = $@"(?<{RelativeUriKvGr}> {relativeUriPartRex} (?: \? {queryKvRex} )? (?: \# {FragmentRex} {FragmentRex} )?";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a relative URI with key-value pairs query.
    /// </summary>
    public const string RelativeUriKvQueryRefRegex = $@"^{relativeUriKvQueryRefRex}$";

    // <summary>
    // Gets a Regex object which matches a string representing a relative URI with key-value pairs query.
    // </summary>
    //[GeneratedRegex(RelativeUriKvQueryRefRegex, Common.Options)]
    //public static partial Regex RelativeUriKvQueryRef();
    #endregion

    #region RelativeUriRef
    /// <summary>
    /// The name of a matching group representing a relative URI with optional general query
    /// </summary>
    public const string RelativeUriGr = "relUri";

    /// <summary>
    /// Matches the URI relative reference  in a string
    /// <para>
    /// BNF: <c>relative-ref = relative-part [ "?" query-gen ] [ "#" fragment ]</c>
    /// </para>
    /// </summary>
    const string relativeUriRefRex = $@"(?<{RelativeUriGr}> {relativeUriPartRex} (?: \? {queryRex} )? (?: \# {FragmentRex} )?";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a relative URI with general query.
    /// </summary>
    public const string RelativeUriRefRegex = $@"^{relativeUriRefRex}$";

    // <summary>
    // Gets a Regex object which matches a string representing a concept.
    // </summary>
    //[GeneratedRegex(RelativeUriRefRegex, Common.OptionsI)]
    //public static partial Regex RelativeUriRef();
    #endregion
    #endregion

    #region Absolute URI
    /// <summary>
    /// Matches the hierarchical part  in a string
    /// <para>
    /// BNF: <c>relative-part = "//" authority path-abempty | path-absolute | path-rootless | path-empty</c>
    /// </para>
    /// </summary>
    const string uriHierarchicalPartRex = $"(?: (?: // {AuthorityRex} {pathAbsoluteOrEmptyRex} ) | {pathAbsoluteRex} | {pathNoSchemeRex} )?";

    #region UriKeyValueQuery
    /// <summary>
    /// The name of a matching group representing an absolute URI
    /// </summary>
    public const string UriKvQueryGr = "uriKvQuery";

    /// <summary>
    /// Matches a URI with an optional key-value query
    /// <para>
    /// BNF: <c>URI = scheme : hier-part [ ? kv-query ] [ # fragment ]</c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string uriKvQueryRex = $@"(?<{UriKvQueryGr}> {SchemeRex} : {uriHierarchicalPartRex} (?: \? {queryKvRex} )? (?: \# {FragmentRex} )? )";

    /// <summary>
    /// Matches a string that represents a URI with an optional key-value query
    /// <para>
    /// BNF: <c>URI = scheme : hier-part [ ? kv-query ] [ # fragment ]</c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string UriKvQueryRegex = $"^{uriKvQueryRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents a URI with a key-value query
    /// </summary>
    [GeneratedRegex(UriKvQueryRegex, Common.OptionsI)]
    public static partial Regex UriKeyValueQuery();
    #endregion
    #endregion

    #region Uri
    /// <summary>
    /// The name of a matching group representing an absolute URI with an optional general query
    /// </summary>
    public const string UriGr = "uri";

    /// <summary>
    /// Matches a URI with an optional general query.
    /// <para>
    /// BNF: <c>URI = scheme : hier-part [ ? query ] [ # fragment ]</c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string uriRex = $@"(?<{UriGr}> {SchemeRex} : {uriHierarchicalPartRex} (?: \? {queryRex} )? (?: \# {FragmentRex} )? )";

    /// <summary>
    /// Matches a string that represents a URI with an optional general query
    /// <para>
    /// BNF: <c>URI = scheme : hier-part [ ? query ] [ # fragment ]</c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string UriRegex = $"^{uriRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents a URI with an optional general query
    /// </summary>
    [GeneratedRegex(UriRegex, Common.OptionsI)]
    public static partial Regex Uri();
    #endregion

    #region Relative URI with network address
    /// <summary>
    /// Matches the URI (net) relative part
    /// <para>
    /// BNF: <c>relative-part = "//" authority path-abempty | path-absolute | path-noscheme | path-empty</c>
    /// </para>
    /// </summary>
    const string netRelativeUriPartRex = $"(?: (?: // {NetAuthorityRex} {pathAbsoluteOrEmptyRex} ) | {pathAbsoluteRex} | {pathNoSchemeRex} )?";

    #region RelativeUriKvQueryRef with network address

    /// <summary>
    /// Matches the URI relative reference with an optional key-value query in a string
    /// <para>
    /// BNF: <c>relative-ref = relative-part [ "?" query-kv ] [ "#" fragment ]</c>
    /// </para>
    /// </summary>
    const string netRelativeUriKvQueryRefRex = $@"(?<{RelativeUriKvGr}> {netRelativeUriPartRex} (?: \? {queryKvRex} )? (?: \# {FragmentRex} {FragmentRex} )?";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a relative URI with key-value pairs query.
    /// </summary>
    public const string NetRelativeUriKvQueryRefRegex = $@"^{netRelativeUriKvQueryRefRex}$";

    // <summary>
    // Gets a Regex object which matches a string representing a relative URI with key-value pairs query.
    // </summary>
    //[GeneratedRegex(NetRelativeUriKvQueryRefRegex, Common.OptionsI)]
    //public static partial Regex NetRelativeUriKvQueryRef();
    #endregion

    #region RelativeUriRef with network address
    /// <summary>
    /// Matches the URI relative reference  in a string
    /// <para>
    /// BNF: <c>relative-ref = relative-part [ "?" query-gen ] [ "#" fragment ]</c>
    /// </para>
    /// </summary>
    const string netRelativeUriRefRex = $@"(?<{RelativeUriGr}> {netRelativeUriPartRex} (?: \? {queryRex} )? (?: \# {FragmentRex} )?";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a relative URI with general query.
    /// </summary>
    public const string NetRelativeUriRefRegex = $@"^{netRelativeUriRefRex}$";

    // <summary>
    // Gets a Regex object which matches a string representing a concept.
    // </summary>
    //[GeneratedRegex(NetRelativeUriRefRegex, Common.OptionsI)]
    //public static partial Regex NetRelativeUriRef();
    #endregion
    #endregion

    #region Absolute URI with network address
    /// <summary>
    /// Matches the hierarchical part  in a string
    /// <para>
    /// BNF: <c>relative-part = "//" authority path-abempty | path-absolute | path-rootless | path-empty</c>
    /// </para>
    /// </summary>
    const string netUriHierarchicalPartRex = $"(?: (?: // {NetAuthorityRex} {pathAbsoluteOrEmptyRex} ) | {pathAbsoluteRex} | {pathNoSchemeRex} )?";

    #region UriKeyValueQuery with network address
    /// <summary>
    /// Matches a URI with an optional key-value query
    /// <para>
    /// BNF: <c>URI = scheme : hier-part [ ? kv-query ] [ # fragment ]</c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string netUriKvQueryRex = $@"(?<{UriKvQueryGr}> {SchemeRex} : {netUriHierarchicalPartRex} (?: \? {queryKvRex} )? (?: \# {FragmentRex} )? )";

    /// <summary>
    /// Matches a string that represents a URI with an optional key-value query
    /// <para>
    /// BNF: <c>URI = scheme : hier-part [ ? kv-query ] [ # fragment ]</c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string NetUriKvQueryRegex = $"^{netUriKvQueryRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents a URI with a key-value query
    /// </summary>
    [GeneratedRegex(NetUriKvQueryRegex, Common.OptionsI)]
    public static partial Regex NetUriKeyValueQuery();
    #endregion
    #endregion

    #region Uri with network address
    /// <summary>
    /// Matches a URI with an optional general query.
    /// <para>
    /// BNF: <c>URI = scheme : hier-part [ ? query ] [ # fragment ]</c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string netUriRex = $@"(?<{UriGr}> {SchemeRex} : {netUriHierarchicalPartRex} (?: \? {queryRex} )? (?: \# {FragmentRex} )? )";

    /// <summary>
    /// Matches a string that represents a URI with an optional general query
    /// <para>
    /// BNF: <c>URI = scheme : hier-part [ ? query ] [ # fragment ]</c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string NetUriRegex = $"^{netUriRex}$";

    /// <summary>
    /// Gets a <see cref="Regex"/> object that matches a string that represents a URI with an optional general query
    /// </summary>
    [GeneratedRegex(NetUriRegex, Common.OptionsI)]
    public static partial Regex NetUri();
    #endregion
}
