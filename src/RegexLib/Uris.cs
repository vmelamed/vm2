namespace vm2.RegexLib;

/// <summary>
/// Class Uri. Defines many regular expressions with the ultimate goal to define a regular expression for URI.
/// Follows closely the definitions in
/// https://datatracker.ietf.org/doc/html/rfc3986
/// https://datatracker.ietf.org/doc/html/rfc1034
/// https://datatracker.ietf.org/doc/html/rfc1123
/// https://datatracker.ietf.org/doc/html/rfc952
/// https://datatracker.ietf.org/doc/html/rfc3513
/// </summary>
public static class Uris
{
    #region URI character sets
    /// <summary>
    /// Matches a percent encoded character.
    /// <para>BNF: <c>pct-encoded := % hex_digit hex_digit</c></para>
    /// </summary>
    const string pctEncodedRex = $"(?:%{Numerics.HexDigitRex}{Numerics.HexDigitRex})";

    /// <summary>
    /// The URI general delimiters.
    /// <para>BNF: <c>gen-delims  = ":" | "/" | "?" | "#" | "[" | "]" | "@"</c></para>
    /// </summary>
    const string genDelimiters = @":/\?#\[\]@";

    /// <summary>
    /// The sub-delimiters without the equals and ampersand charecters.
    /// <para>BNF: <c>sub-delims-no-eq-no-amp = "!" | "$" | "'" | "(" | ")" | "*" | "+" | "," | ";"</c></para>
    /// </summary>
    const string subDelimiterNoEqAmpChars = @"!',;\$\(\)\*\+";

    /// <summary>
    /// The sub-delimiters.
    /// <para>BNF: <c>sub-delims-no-eq-no-amp = sub-delims-no-eq-no-amp | "=" | "&amp;"</c></para>
    /// </summary>
    const string subDelimiterChars = $@"{subDelimiterNoEqAmpChars}=&";

    /// <summary>
    /// The reserved chars.
    /// <para>BNF: <c>reserved = gen-delims | sub-delims</c></para>
    /// </summary>
    const string reservedChars = $@"{genDelimiters}{subDelimiterChars}";

    /// <summary>
    /// The characters that are allowed in a URI but do not have a reserved purpose.
    /// <para>BNF: <c>unreserved  = ALPHA | DIGIT | "-" | "." | "_" | "~"</c></para>
    /// </summary>
    const string unreservedChars = $@"\-\.{Ascii.AlphaNumericChars}_~";

    /// <summary>
    /// The unreserved or sub-delimiter chars
    /// </summary>
    const string unreservedOrSubDelimiterChars = $"{unreservedChars}{subDelimiterChars}";

    /// <summary>
    /// Matches an unreserved or sub-delimiter character.
    /// <para>BNF: <c>unreserved-or-sub-delimiter := unreserved | sub-delimiter</c></para>
    /// </summary>
    const string unreservedOrSubDelimiterRex = $"[{unreservedOrSubDelimiterChars}]";
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
    public const string SchemeRex = $@"(?<{SchemeGr}> {Ascii.AlphaRex} {schemeRex} )";

    /// <summary>
    /// Matches a string that represents a URI scheme.
    /// <para>BNF: <c>scheme := alpha 1*[ alpha | digit | + | - | . ]</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string SchemeRegex = $@"^{SchemeRex}$";

    static readonly Lazy<Regex> rexSchemeRegex = new(() => new(SchemeRegex, RegexOptions.Compiled |
                                                                            RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a URI scheme.
    /// <para>BNF: <c>scheme := alpha 1*[ alpha | digit | + | - | . ]</c></para>
    /// </summary>
    public static Regex Scheme => rexSchemeRegex.Value;
    #endregion

    #region Host
    /// <summary>
    /// Matches a IP address.
    /// <para>BNF: <c>ip-literal-address := [ (IPv6address | IPvFuture) ]</c></para>
    /// <para>Named groups: <see cref="Net.Ipv6Gr"/> or <see cref="Net.IpvfGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string ipLiteralAddressRex = $@"(?: \[ {Net.Ipv6AddressRex} | {Net.IpvFutureAddressRex} \] )";

    /// <summary>
    /// Matches an IP address in numeric form IPv4, IPv6, or IPvFuture.
    /// <para>BNF: <c>ip-numeric-address := [ IPv4address | IPv6address | IPvFuture ]</c></para>
    /// <para>Named groups: <see cref="Net.Ipv4Gr"/>, <see cref="Net.Ipv6Gr"/>, or <see cref="Net.IpvfGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string ipNumericAddressRex = $@"{Net.Ipv4AddressRex} | {ipLiteralAddressRex}";

    /// <summary>
    /// The name of a matching group representing an IP general name.
    /// </summary>
    public const string IpGenNameGr = "ipGenName";

    /// <summary>
    /// Matches reg-name in RFC 3986
    /// <para>BNF: <c>registered_name = *[ unreserved | sub-delimiters | pct-encoded ]</c> - yes, it can be empty, see the RFC</para>
    /// </summary>
    const string generalNameRex = $@"(?<{IpGenNameGr}> (?: [{unreservedOrSubDelimiterChars}\.] | {pctEncodedRex} )+ )";

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
    public const string HostGr = "host";

    /// <summary>
    /// Matches a host.
    /// <para>BNF: <c>host := IP-literal | IPv4address | reg-name</c></para>
    /// <para>
    /// Named groups: <see cref="HostGr"/>, and one of: <see cref="IpGenNameGr"/>, <see cref="Net.Ipv4Gr"/>, 
    /// <see cref="Net.Ipv6Gr"/> or <see cref="Net.IpvfGr"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string HostRex = $@"(?<{HostGr}> {ipNumericAddressRex} | {registeredNameRex} )";

    /// <summary>
    /// Matches a string that represents a host.
    /// <para>BNF: <c>host := IP-literal | IPv4address | reg-name</c></para>
    /// <para>
    /// Named groups: <see cref="HostGr"/>, and one of: <see cref="IpGenNameGr"/>, <see cref="Net.IpDnsNameGr"/>, 
    /// <see cref="Net.Ipv4Gr"/>, <see cref="Net.Ipv6Gr"/> or <see cref="Net.IpvfGr"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string HostRegex = $@"^[{HostRex}]$";

    static readonly Lazy<Regex> hostRegex = new(() => new(HostRegex, RegexOptions.Compiled |
                                                                     RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a host.
    /// <para>BNF: <c>host := IP-literal | IPv4address | reg-name</c></para>
    /// <para>
    /// Named groups: <see cref="HostGr"/>, and one of: <see cref="Net.IpDnsNameGr"/>, <see cref="IpGenNameGr"/>, 
    /// <see cref="Net.Ipv4Gr"/>, <see cref="Net.Ipv6Gr"/> or <see cref="Net.IpvfGr"/>.
    /// </para>
    /// </summary>
    public static Regex Host => hostRegex.Value;
    #endregion

    #region Address (Host:Port)
    /// <summary>
    /// The name of a matching group representing an IP address.
    /// </summary>
    public const string AddressGr = "address";

    /// <summary>
    /// Matches an IP address.
    /// <para>BNF: <c>address := host [: port]</c></para>
    /// <para>Named groups: <see cref="AddressGr"/> <see cref="HostGr"/>, <see cref="Net.PortGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string AddressRex = $"(?<{AddressGr}> {HostRex} (?: : {Net.PortRex})? )";

    /// <summary>
    /// Matches a string that represents an IP address.
    /// <para>BNF: <c>address := host [: port]</c></para>
    /// <para>Named groups: <see cref="HostGr"/>, <see cref="Net.PortGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string AddressRegex = $"^{AddressRex}$";

    static readonly Lazy<Regex> addressRegex = new(() => new(AddressRegex, RegexOptions.Compiled |
                                                                           RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IP address.
    /// <para>BNF: <c>address := host [: port]</c></para>
    /// <para>Named groups: <see cref="HostGr"/>, <see cref="Net.PortGr"/>.</para>
    /// </summary>
    /// <value>The address.</value>
    public static Regex Address => addressRegex.Value;
    #endregion

    #region Authority
    /// <summary>
    /// Matches URI's user info (name and password).
    /// <para>
    /// The use of the format 'user:password' in the 'userInfo' field is deprecated.  Applications should not render as 
    /// clear text any data after the first colon (':') character found within a 'userInfo' subcomponent unless the 
    /// data after the colon is the empty string (indicating no password).  Applications may choose to ignore or reject
    /// such data when it is received as part of a reference and should reject the storage of such data in un-encrypted
    /// form.
    /// </para>
    /// <para>BNF: <c>user-info := *[ unreserved | sub-delimiters | : | pct-encoded ]</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string UserInfoRex = $@"(?: {unreservedOrSubDelimiterRex} | {pctEncodedRex} | : )*";

    /// <summary>
    /// The name of a matching group representing the authority in a URI.
    /// </summary>
    public const string AuthorityGr = "authority";

    /// <summary>
    /// Matches the URI's authority.
    /// <para>BNF: <c>authority := [ user-info @ ] host [ : port ]</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string AuthorityRex = $"(?<{AuthorityGr}> {UserInfoRex} @ )? {AddressRex}";
    #endregion

    #region Path
    /// <summary>
    /// Path characters (without the colon char).
    /// <para>BNF: <c>path-nc-chars := unreserved | sub-delimiters | @</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathNcChars = $"{unreservedOrSubDelimiterChars}@";

    /// <summary>
    /// Matches a character from a path (without the colon char).
    /// <para>BNF: <c>path-nc-char := unreserved | sub-delimiters | @ | pct-encoded</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathNcChar = $"(?: [{pathNcChars}] | {pctEncodedRex} )";

    /// <summary>
    /// Matches non-zero length path segment without colon
    /// <para>BNF: <c>segment-nz-nc := 1*( unreserved | sub-delimiters | @ | pct-encoded )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathSegmentNzNcRex = $"[{pathNcChar}]+";

    /// <summary>
    /// Path characters (incl. the colon char).
    /// <para>BNF: <c>path-nc-chars := path-nc-chars | :</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathChars = $"{pathNcChars}:";

    /// <summary>
    /// Matches a path character (incl. the colon char).
    /// <para>BNF: <c>path-nc-char := path-nc-char | : | pct-encoded</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathChar = $"(?: [{pathChars}] | {pctEncodedRex} )";

    /// <summary>
    /// Matches path segment incl. colon (can be empty)
    /// <para>BNF: <c>segment := *( unreserved | sub-delimiters | @ | : | pct-encoded )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathSegmentRex = $"[{pathChar}]*";

    /// <summary>
    /// Matches non-zero length path segment incl. colon
    /// <para>BNF: <c>segment-nz := 1*( unreserved | sub-delimiters | @ | : | pct-encoded )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathSegmentNzRex = $"[{pathChar}]+";

    /// <summary>
    /// The name of a matching group representing a URI path
    /// </summary>
    public const string NoRootPathGr = "noRootPath";

    /// <summary>
    /// Matches path without the root ('/').
    /// <para>BNF: <c>path-rootless := segment-nz *( / segment )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string noRootPathRex = $"(?<{NoRootPathGr}> {pathSegmentNzRex} (?: / {pathSegmentRex})* )";

    /// <summary>
    /// Matches a path with no scheme.
    /// <para>BNF: <c>path-no-scheme := segment-nz-nc *( / segment )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string noSchemePathRex = $"(?:{pathSegmentNzNcRex} (?: / {pathSegmentRex}) *)";

    /// <summary>
    /// The name of a matching group representing a URI path
    /// </summary>
    public const string AbsPathGr = "absPath";

    /// <summary>
    /// Matches an absolute path starting with '/' but not '//'.
    /// <para>BNF: <c>path-absolute := / path-rootless</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string absolutePathRex = $"(?<{AbsPathGr}> / {noRootPathRex}? )";

    /// <summary>
    /// Matches an absolute or empty path.
    /// <para>BNF: <c>path-abs-empty := *( / segment )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string absoluteOrEmptyPathRex = $"(?{absolutePathRex})?";

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
    const string pathRex = $"(?P<{UriPathGr}> {absoluteOrEmptyPathRex} | {absolutePathRex} | {noSchemePathRex} | {noRootPathRex})";

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

    static readonly Lazy<Regex> pathRegex = new(() => new(PathRegex, RegexOptions.Compiled |
                                                                     RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a URI's path.
    /// <para>BNF:
    /// <code>
    /// path = path-abs-empty  | ; begins with / or is empty
    ///        path-absolute   | ; begins with / but not //
    ///        path-no-scheme  | ; begins with a non-colon segment
    ///        path-rootless   | ; begins with a segment
    ///        path-empty        ; zero characters (???)</code></para>
    /// </summary>
    public static Regex Path => pathRegex.Value;
    #endregion

    #region General Query
    /// <summary>
    /// The generic query chars.
    /// <para>BNF: <c>generic-query-chars := a-zA-Z0-9._~-!$'()*+,;=&amp;@:/?</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string genericQueryChars = $@"{pathChars}/\?";

    /// <summary>
    /// Matches a generic query char.
    /// <para>BNF: <c>generic-query-char := generic-query-chars | pct-encoded</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string genericQueryCharRex = $"(?: [{genericQueryChars}] | {pctEncodedRex} )";

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
    const string genericQueryRex = $@"(?<{QueryGr}> {genericQueryCharRex}* )";

    /// <summary>
    /// Matches a string that represents a generic query.
    /// <para>BNF: <c>gen-query := *( path-char | / | ? )</c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string GenericQueryRegex = $@"^{genericQueryRex}$";

    static readonly Lazy<Regex> genericQueryRegex = new(() => new(GenericQueryRegex, RegexOptions.Compiled |
                                                                                     RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a generic query.
    /// </summary>
    public static Regex GenericQuery => genericQueryRegex.Value;
    #endregion

    #region Key-Value query
    /// <summary>
    /// Key-value query character set: ! ' , ; \$ \( \) \* \+
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string keyValueChars = $@"{unreservedChars}{subDelimiterNoEqAmpChars}:@/\?";

    /// <summary>
    /// Matches a key-value query character.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string keyValueCharRex = $"[{keyValueChars}]";

    /// <summary>
    /// The name of a matching group representing a key from a key-value query in a URI
    /// </summary>
    public const string KeyGr = "key";

    /// <summary>
    /// Matches a key from a URI key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string queryKeyRex   = $"(?<{KeyGr}> {keyValueCharRex}+ )";

    /// <summary>
    /// The name of a matching group representing a value from a key-value query in a URI
    /// </summary>
    public const string ValueGr = "value";

    /// <summary>
    /// Matches a value from a URI key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string queryValueRex = $"(?<{ValueGr}> {keyValueCharRex}+ )";

    /// <summary>
    /// Matches a URI's key-value query
    /// <para>BNF: <c>key-value-query := key = value *(&amp; key = value)</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string queryKeyValueRex = $"{queryKeyRex} = {queryValueRex}";

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
    /// </remarks>
    const string keyValueQueryRex = $"(?<{KvQueryGr}> {queryKeyValueRex} (?: & {queryKeyValueRex} )* )";

    /// <summary>
    /// Matches a string that represents a URI's key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string KeyValueQueryRegex = $"^{keyValueQueryRex}$";

    static readonly Lazy<Regex> keyValueQueryRegex = new(() => new(KeyValueQueryRegex, RegexOptions.Compiled |
                                                                                       RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a URI's key-value query
    /// </summary>
    public static Regex KeyValueQuery => keyValueQueryRegex.Value;
    #endregion

    ///// <summary>
    ///// Matches a URI's query.
    ///// <para>BNF: <c>query := key-value-query | generic-query</c></para>
    ///// </summary>
    // TODO: const string queryRex = $"(?:{keyValueQueryRex} | {genericQueryRex})";  // didn't work before?

    #region Fragment
    const string fragmentChars = $@"{pathChars}/\?";

    const string fragmentCharRex = $@"(?:[{fragmentChars}]|{pctEncodedRex})";

    /// <summary>
    /// The name of a matching group representing a URI fragment
    /// </summary>
    public const string FragmentGr = "fragment";

    const string fragmentRex = $"(?<{FragmentGr}> {fragmentCharRex}*)";

    /// <summary>
    /// Matches a URI's fragment.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string FragmentRegex = $"^{fragmentRex}$";

    static readonly Lazy<Regex> fragmentRegex = new(() => new(FragmentRegex, RegexOptions.Compiled |
                                                                             RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a URI's fragment
    /// </summary>
    public static Regex Fragment = fragmentRegex.Value;
    #endregion

    /// <summary>
    /// Matches a URI's hierarchical part
    /// <para>BNF: <c>hierarchical-part = '//' authority path-ab-empty | path-absolute | path-rootless | path-empty</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string hierarchicalPartRex = $"// {AuthorityRex} (?: {absoluteOrEmptyPathRex} | {absolutePathRex} | {noRootPathRex} )";

    /// <summary>
    /// The name of a matching group representing a URI
    /// </summary>
    public const string UriGr = "uri";

    /// <summary>
    /// Matches a URI with a key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string uriKeyValueQueryRex = $@"(?<{UriGr}> {SchemeRex} : {hierarchicalPartRex} (?: \? {keyValueQueryRex} )? (?: {Ascii.Hash} {fragmentRex} )? )";

    /// <summary>
    /// Matches a string that represents a URI with a key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string UriKeyValueQueryRegex = $"^{uriKeyValueQueryRex}$";

    static readonly Lazy<Regex> uriKeyValueQueryRegex = new(() => new(UriKeyValueQueryRegex, RegexOptions.Compiled |
                                                                                             RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a URI with a key-value query
    /// </summary>
    public static Regex UriKeyValueQuery => uriKeyValueQueryRegex.Value;

    /// <summary>
    /// Matches a URI with a key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string uriGenericQueryRex = $@"(?<{UriGr}> {SchemeRex} : {hierarchicalPartRex} (?: \? {genericQueryRex} )? (?: {Ascii.Hash} {fragmentRex} )? )";

    /// <summary>
    /// Matches a string that represents a URI with a key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string UriGenericQueryRegex = $"^{uriGenericQueryRex}$";

    static readonly Lazy<Regex> uriGenericQueryRegex = new(() => new(UriGenericQueryRegex, RegexOptions.Compiled |
                                                                                           RegexOptions.IgnorePatternWhitespace));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a URI with a key-value query
    /// </summary>
    public static Regex UriGenericQuery => uriGenericQueryRegex.Value;

    #region Uri
    ///// <summary>
    ///// Regular expression pattern which matches a URI in a string.
    ///// <para>BNF: <c>uri = scheme ":" hierachical-part [ "?" query ] [ "#" fragment]</c></para>
    ///// </summary>
    //public const string UriRex = $@"(?<{UriGr}> {SchemeRex} : {hierarchicalPartRex} (?: \? {queryRex} )? (?: {Ascii.Hash} {fragmentRex} )? )";

    ///// <summary>
    ///// Regular expression pattern which matches a string that represents a URI.
    ///// </summary>
    //public const string UriRegex = $@"^{UriRex}$";

    //static readonly Lazy<Regex> regexUri = new(() => new(UriRegex, RegexOptions.Compiled | RegexOptions.CultureInvariant));

    ///// <summary>
    ///// Gets a Regex object which matches a string representing a URI.
    ///// </summary>
    //public static Regex Uri => regexUri.Value;
    #endregion

    // TODO: relative URI-s
}
