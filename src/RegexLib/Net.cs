namespace vm2.RegexLib;

/// <summary>
/// Class Net. Defines regular expressions related to network operations, addresses, etc.
/// Follows closely the definitions in
/// https://datatracker.ietf.org/doc/html/rfc3986 (URI-s)
/// https://datatracker.ietf.org/doc/html/rfc1034
/// https://datatracker.ietf.org/doc/html/rfc1123
/// https://datatracker.ietf.org/doc/html/rfc952
/// https://datatracker.ietf.org/doc/html/rfc3513
/// https://datatracker.ietf.org/doc/html/rfc6874
/// </summary>
public static partial class Net
{
    #region Ipv4Address
    /// <summary>
    /// Matches a decimal number from 1 to 239 - the first decimal octet in a valid IPv4 address (0x01 - 0xEF).
    /// Includes classes: <list type="bullet">
    ///     <item>A (incl. loopback 1-127)</item>
    ///     <item>B (128-191)</item>
    ///     <item>C (192-223)</item>
    ///     <item>D (224-239)</item>
    ///     <item>Does not include class E (224-255) - used for R&amp;D and study purposes only.</item></list>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string firstDecimalOctetRex = $$"""
                                          (?:
                                             2[0-3]{{Ascii.DigitRex}} |
                                             1{{Ascii.DigitRex}}{{Ascii.DigitRex}} |
                                             {{Ascii.NonZeroDigitRex}}{{Ascii.DigitRex}} |
                                             {{Ascii.NonZeroDigitRex}}
                                          )
                                          """;

    /// <summary>
    /// Matches a decimal number from 0 to 255 - the second, third, and forth decimal octets in a valid IPv4 address (0x00 - 0xFF).
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string decimalOctetRex = $$"""
                                     (?:
                                        25[0-5] |
                                        2[0-4]{{Ascii.DigitRex}} |
                                        1{{Ascii.DigitRex}}{{Ascii.DigitRex}} |
                                        {{Ascii.NonZeroDigitRex}}{{Ascii.DigitRex}} |
                                        {{Ascii.DigitRex}}
                                     )
                                     """;

    /// <summary>
    /// Matches an IPv4 address.
    /// <para>BNF: <c>IPv4address = dec-octet "." dec-octet "." dec-octet "." dec-octet</c></para>, excluding the
    /// addresses provided for use in documentation only in RFC 5737
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string ipv4Rex = $$"""
                             (?:
                               (?! (?: 192\.0\.2\.{{Ascii.DigitRex}}{1,3} )    |
                                   (?: 198\.51\.100\.{{Ascii.DigitRex}}{1,3} ) |
                                   (?: 203\.0\.113\.{{Ascii.DigitRex}}{1,3} )
                               )
                               {{firstDecimalOctetRex}} (?: \. {{decimalOctetRex}} ){3}
                             )
                             """;

    /// <summary>
    /// The name of a matching group representing an IPv4 address.
    /// </summary>
    public const string Ipv4Gr = "ipv4";

    /// <summary>
    /// Matches an IPv4 address.
    /// <para>BNF: <c>ipv4 := dec-octet.dec-octet.dec-octet.dec-octet</c></para>
    /// <para>Named groups: <see cref="Ipv4Gr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string Ipv4AddressRex = $@"(?<{Ipv4Gr}> {ipv4Rex} )";

    /// <summary>
    /// Matches a string that represents an IPv4 address.
    /// <para>BNF: <c>ipv4 := dec-octet.dec-octet.dec-octet.dec-octet</c></para>
    /// <para>Named groups: <see cref="Ipv4Gr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string Ipv4AddressRegex = $@"^{Ipv4AddressRex}$";

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IPv4 address.
    /// <para>BNF: <c>ipv4 := dec-octet.dec-octet.dec-octet.dec-octet</c></para>
    /// <para>Named groups: <see cref="Ipv4Gr"/>.</para>
    /// </summary>
    [GeneratedRegex(Ipv4AddressRegex, Common.Options)]
    public static partial Regex Ipv4Address();
    #endregion

    #region Ipv6Address
    const string h00 = "0{1,4}";
    const string h0f = "(?: (?:0{1,4}) | (?:[fF]{1,4}) )";
    const string h16 = $"(?:{Numerical.HexDigitRex}{{1,4}})";
    const string l48 = $"{h0f}:{ipv4Rex}";  // 0:d.d.d.d or f:d.d.d.d

    /// <summary>
    /// The name of a matching group representing an IPv6 address without a zone/scope ID.
    /// </summary>
    public const string Ipv6NzGr = "ipv6nz";

    /// <summary>
    /// Matches an IPv6 address without a zone/scope ID.
    /// <para>Named groups: <see cref="Ipv6NzGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// Must have at least one non-zero number - cannot be the "unspecified address" ::0
    /// </remarks>
    public const string Ipv6NzAddressRex = $$"""
         (?=.*[1-9A-Fa-f])
         (?<{{Ipv6NzGr}}>
             (?:{{h16}}:){1,7}:
           | (?:{{h16}}:){1,6}(?: :{{h16}} )
           | (?:{{h16}}:){1,5}(?: :{{h16}} ){1,2}
           | (?:{{h16}}:){1,4}(?: :{{h16}} ){1,3}
           | (?:{{h16}}:){1,3}(?: :{{h16}} ){1,4}
           | (?:{{h16}}:){1,2}(?: :{{h16}} ){1,5}
           | (?:{{h16}}:)     (?: :{{h16}} ){1,6}
           |           :      (?: :{{h16}} ){1,7}
           | (?:{{h16}}:){7}       {{h16}}

           | (?:{{h00}}:){5}  (?:  {{l48}} )
           | (?:{{h00}}:){1,4}(?: :{{l48}} )
           | (?:{{h00}}:){1,3}(?: :{{l48}} )
           | (?:{{h00}}:){1,2}(?: :{{l48}} )
           | (?:{{h00}}:)     (?: :{{l48}} )
           |           :      (?: :{{l48}} )

           | (?:{{h00}}:){6}  (?:  {{ipv4Rex}} )
           | (?:{{h00}}:){1,5}(?: :{{ipv4Rex}} )
           |           :      (?: :{{ipv4Rex}} )
         )
         """;

    /// <summary>
    /// The name of a matching group representing a zone (site) ID of an IPv6 address.
    /// </summary>
    public const string ZoneIdGr = "zoneId";

    /// <summary>
    /// The characters that are allowed in a URI (<see cref="Uris"/>) but do not have a reserved purpose.
    /// <para>BNF: <c>unreserved  = ALPHA | DIGIT | "-" | "." | "_" | "~"</c></para>
    /// </summary>
    internal const string UnreservedChars = $@"\-\.{Ascii.AlphaNumericChars}_~";

    /// <summary>
    /// Matches a percent encoded character (<see cref="Uris"/>).
    /// <para>BNF: <c>pct-encoded := % hex_digit hex_digit</c></para>
    /// </summary>
    internal const string PctEncodedChar = $"(?:%{Numerical.HexDigitRex}{Numerical.HexDigitRex})";

    const string unreservedRex = $@"[{UnreservedChars}] | {PctEncodedChar}";

    /// <summary>
    /// Matches a Zone or Scope ID from an IPv6 address.
    /// <para>Named groups: <see cref="ZoneIdGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// Must have at least one non-zero number - cannot be the "unspecified address" ::0
    /// </remarks>
    public const string Ipv6ZoneIdRex = $@"(?<{ZoneIdGr}> (?: {unreservedRex} )+ )";

    /// <summary>
    /// Matches a string that represents an IPv6 address without zone/scope ID.
    /// <para>Named groups: <see cref="Ipv6NzGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string Ipv6NzAddressRegex = $@"^{Ipv6NzAddressRex}$";

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IPv6 address without a zone/scope ID.
    /// <para>Named groups: <see cref="Ipv6NzGr"/>.</para>
    /// </summary>
    [GeneratedRegex(Ipv6NzAddressRegex, Common.Options)]
    public static partial Regex Ipv6NzAddress();

    /// <summary>
    /// The name of a matching group representing an IPv6 address with an optional zone ID.
    /// </summary>
    public const string Ipv6Gr = "ipv6";

    /// <summary>
    /// Matches a string that represents an IPv6 address with a zone ID in a string.
    /// <para>Named groups: <see cref="Ipv6NzGr"/>, <see cref="ZoneIdGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string Ipv6AddressRex = $@"(?<{Ipv6Gr}> {Ipv6NzAddressRex} (?: % {Ipv6ZoneIdRex} )? )";

    /// <summary>
    /// Matches a whole string that represents an IPv6 address with an optional zone ID.
    /// <para>Named groups: <see cref="Ipv6NzGr"/>, <see cref="ZoneIdGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string Ipv6AddressRegex = $@"^(?<{Ipv6Gr}> {Ipv6NzAddressRex} (?: % {Ipv6ZoneIdRex} )? )$";

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IPv6 address with a zone ID.
    /// <para>Named groups: <see cref="Ipv6NzGr"/>.</para>
    /// </summary>
    [GeneratedRegex(Ipv6AddressRegex, Common.Options)]
    public static partial Regex Ipv6Address();
    #endregion

    #region IpvFutureAddress
    /// <summary>
    /// The IPvFuture character set.
    /// </summary>
    const string netNameNcNdChars = $@"!&',;=\$\(\)\*\+\-{Ascii.AlphaNumericChars}_~";

    /// <summary>
    /// The name of a matching group representing an IPv.Future address.
    /// </summary>
    public const string IpvfGr = "ipvF";

    /// <summary>
    /// Matches an IPv.future address.
    /// <para>Named groups: <see cref="IpvfGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string IpvFutureAddressRex = $@"(?<{IpvfGr}> v {Numerical.HexDigitRex}+ \. [{netNameNcNdChars}\.\:]+ )";

    /// <summary>
    /// Matches a string that represents an IPv.Future address
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string IpvFutureAddressRegex = $"^{IpvFutureAddressRex}$";

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IPv.future address.
    /// <para>Named groups: <see cref="Ipv6NzGr"/>.</para>
    /// </summary>
    [GeneratedRegex(IpvFutureAddressRegex, Common.Options)]
    public static partial Regex IpvFutureAddress();
    #endregion

    #region DnsName
    // a-zA-Z0-9-
    const string alphaDigitHyphenChars = $@"{Ascii.AlphaNumericChars}\-";

    /// <summary>
    /// Matches an alpha-digit-hyphen character from RFC1034 (ASCII only)
    /// <para>BNF: <c>alpha-digit-hyphen := alpha | digit | -</c></para>
    /// </summary>
    const string alphaDigitHyphenRex = $@"[{alphaDigitHyphenChars}]";

    /// <summary>
    /// Matches a domain name label (e.g. google in www.google.com) from RFC 1034 (ASCII only)
    /// <para>BNF: <c>label := alpha [ *61[ alpha-digit-hyphen ] alpha-digit ]</c></para>
    /// </summary>
    public const string DnsLabelRex = $@"(?: {Ascii.AlphaRex} (?: {alphaDigitHyphenRex}{{0,61}} {Ascii.AlphaNumericRex} )? )";

    /// <summary>
    /// The name of a matching group representing a name that can be looked up in DNS.
    /// </summary>
    public const string IpDnsNameGr = "ipDnsName";

    /// <summary>
    /// Matches registered DNS name.
    /// <para>BNF: <c>registered_name = *[ domainLabel . ] topLabel</c> (RFC 1034)</para>
    /// <para>Named groups: <see cref="IpDnsNameGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string DnsNameRex = $@"(?<{IpDnsNameGr}> {DnsLabelRex} (?: \. {DnsLabelRex} ){{0,127}} \.? )";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a concept.
    /// </summary>
    public const string DnsNameRegex = $@"^{DnsNameRex}$";

    /// <summary>
    /// Gets a Regex object which matches a string representing a concept.
    /// </summary>
    [GeneratedRegex(DnsNameRegex, Common.Options)]
    public static partial Regex DnsName();
    #endregion

    /// <summary>
    /// Matches an IP address.
    /// <para>BNF: <c>ip-literal-address := [ (IPv6address | IPvFuture) ]</c></para>
    /// <para>Named groups: <see cref="Ipv6NzGr"/> or <see cref="IpvfGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string ipLiteralAddressRex = $@"(?: \[ (?: {Ipv6AddressRex} | {IpvFutureAddressRex} ) \] )";

    /// <summary>
    /// Matches an IP address in numeric form IPv4, IPv6, or IPvFuture.
    /// <para>BNF: <c>ip-numeric-address := IPv4address | "[" (IPv6address | IPvFuture) "]" </c></para>
    /// <para>Named groups: <see cref="Ipv4Gr"/>, <see cref="Ipv6NzGr"/>, or <see cref="IpvfGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    internal const string IpNumericAddressRex = $@"{Ipv4AddressRex} | {ipLiteralAddressRex}";

    #region Host
    /// <summary>
    /// The name of a matching group representing a host name.
    /// </summary>
    public const string HostGr = "host";

    /// <summary>
    /// Matches a host in a string.
    /// <para>BNF: <c>host := IP-literal | IPv4address | reg-name</c></para>
    /// <para>
    /// Named groups: <see cref="HostGr"/>, and one of: <see cref="IpDnsNameGr"/>, <see cref="Ipv4Gr"/>,
    /// <see cref="Ipv6NzGr"/> or <see cref="IpvfGr"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string HostRex = $@"(?<{HostGr}> {IpNumericAddressRex} | {DnsNameRex} )";

    /// <summary>
    /// Matches a string that represents a host.
    /// <para>BNF: <c>host := IP-literal | IPv4address | reg-name</c></para>
    /// <para>
    /// Named groups: <see cref="HostGr"/>, and one of: <see cref="IpDnsNameGr"/>, <see cref="Ipv4Gr"/>,
    /// <see cref="Ipv6NzGr"/> or <see cref="IpvfGr"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string HostRegex = $@"^{HostRex}$";

    /// <summary>
    /// Gets a Regex object which matches a string representing a host.
    /// </summary>
    [GeneratedRegex(HostRegex, Common.Options)]
    public static partial Regex Host();
    #endregion

    #region Port
    /// <summary>
    /// The name of a matching group representing a port.
    /// </summary>
    public const string PortGr = "port";

    /// <summary>
    /// Matches an IP port.
    /// <para>BNF: <c>port := decimal-num</c> in the range [0-65535]</para>
    /// <para>Named groups: <see cref="PortGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PortRex = $$"""
                                    (?<{{PortGr}}>
                                        6553[0-5] |
                                        655[0-2][0-9] |
                                        65[0-4][0-9]{2} |
                                        6[0-4][0-9]{3} |
                                        [1-5][0-9]{4} |
                                        [0-9]{1,4}
                                    )
                                    """;

    /// <summary>
    /// Matches a string that represents an IP port.
    /// <para>BNF: <c>port := decimal-num</c> in the range [0-65535]</para>
    /// <para>Named groups: <see cref="PortGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PortRegex = $@"^{PortRex}$";

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IP port.
    /// <para>BNF: <c>port := decimal-num</c> in the range [0-65535]</para>
    /// <para>Named groups: <see cref="PortGr"/>.</para>
    /// </summary>
    [GeneratedRegex(PortRegex, Common.Options)]
    public static partial Regex Port();

    #endregion

    #region Endpoint (Host:Port)
    /// <summary>
    /// The name of a matching group representing a TCP endpoint.
    /// </summary>
    public const string EndpointGr = "endpoint";

    /// <summary>
    /// Matches a TCP endpoint.
    /// <para>BNF: <c>endpoint := host [: port]</c></para>
    /// <para>Named groups: <see cref="EndpointGr"/> <see cref="HostGr"/>, <see cref="PortGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string EndpointRex = $"(?<{EndpointGr}> {HostRex} (?: : {PortRex} )? )";

    /// <summary>
    /// Matches a string that represents a TCP endpoint.
    /// <para>BNF: <c>endpoint := host [: port]</c></para>
    /// <para>Named groups: <see cref="HostGr"/>, <see cref="PortGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string EndpointRegex = $"^{EndpointRex}$";

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a TCP endpoint.
    /// <para>BNF: <c>endpoint := host [: port]</c></para>
    /// <para>Named groups: <see cref="HostGr"/>, <see cref="PortGr"/>.</para>
    /// </summary>
    /// <value>The endpoint.</value>
    [GeneratedRegex(EndpointRegex, Common.Options)]
    public static partial Regex Endpoint();
    #endregion
}
