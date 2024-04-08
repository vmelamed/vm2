namespace vm2.RegexLib;

/// <summary>
/// Class Net. Defines regular expressions related to network operations, addresses, etc.
/// Follows closely the definitions in
/// https://datatracker.ietf.org/doc/html/rfc3986 (URI-s)
/// https://datatracker.ietf.org/doc/html/rfc1034
/// https://datatracker.ietf.org/doc/html/rfc1123
/// https://datatracker.ietf.org/doc/html/rfc952
/// https://datatracker.ietf.org/doc/html/rfc3513
/// </summary>
public static class Net
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

    static readonly Lazy<Regex> ipv4AddressRegex = new(() => new(Ipv4AddressRegex, RegexOptions.Compiled |
                                                                                   RegexOptions.IgnorePatternWhitespace, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IPv4 address.
    /// <para>BNF: <c>ipv4 := dec-octet.dec-octet.dec-octet.dec-octet</c></para>
    /// <para>Named groups: <see cref="Ipv4Gr"/>.</para>
    /// </summary>
    public static Regex Ipv4Address => ipv4AddressRegex.Value;
    #endregion

    #region Ipv6Address

    const string h00 = "0{1,4}";
    const string h0f = "(?: (?:0{1,4}) | (?:[fF]{1,4}) )";
    const string h16 = $"(?:{Numerics.HexDigitRex}{{1,4}})";
    const string l48 = $"{h0f}:{ipv4Rex}";  // 0:d.d.d.d or f:d.d.d.d

    /// <summary>
    /// The name of a matching group representing an IPv6 address.
    /// </summary>
    public const string Ipv6Gr = "ipv6";

    /// <summary>
    /// Matches an IPv6 address.
    /// <para>Named groups: <see cref="Ipv6Gr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// Must have at least one non-zero number - cannot be the "unspecified address" ::0
    /// </remarks>
    public const string Ipv6AddressRex = $$"""
         (?=.*[1-9A-Fa-f])
         (?<{{Ipv6Gr}}>
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
    /// Matches a string that represents an IPv6 address.
    /// <para>Named groups: <see cref="Ipv6Gr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string Ipv6AddressRegex = $@"^{Ipv6AddressRex}$";

    static readonly Lazy<Regex> ipv6AddressRegex = new(() => new(Ipv6AddressRegex, RegexOptions.Compiled |
                                                                                   RegexOptions.IgnorePatternWhitespace, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IPv6 address.
    /// <para>Named groups: <see cref="Ipv6Gr"/>.</para>
    /// </summary>
    public static Regex Ipv6Address => ipv6AddressRegex.Value;
    #endregion

    /// <summary>
    /// The IPvFuture character set.
    /// </summary>
    const string netNameNcNdChars = $@"!&',;=\$\(\)\*\+\-{Ascii.AlphaNumericChars}_~";

    #region IpvFutureAddress
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
    public const string IpvFutureAddressRex = $@"(?<{IpvfGr}> v {Numerics.HexDigitRex}+ \. [{netNameNcNdChars}\.\:]+ )";

    /// <summary>
    /// Matches a string that represents an IPv.Future address
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string IpvFutureAddressRegex = $"^{IpvFutureAddressRex}$";

    static readonly Lazy<Regex> ipvFutureAddressRegex = new(() => new(IpvFutureAddressRegex, RegexOptions.Compiled |
                                                                                             RegexOptions.IgnorePatternWhitespace, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IPv.future address.
    /// <para>Named groups: <see cref="Ipv6Gr"/>.</para>
    /// </summary>
    public static Regex IpvFutureAddress => ipvFutureAddressRegex.Value;
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

    static readonly Lazy<Regex> regexDnsName = new(() => new(DnsNameRegex, RegexOptions.Compiled |
                                                                           RegexOptions.IgnorePatternWhitespace, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// Gets a Regex object which matches a string representing a concept.
    /// </summary>
    public static Regex DnsName => regexDnsName.Value;
    #endregion

    /// <summary>
    /// Matches a IP address.
    /// <para>BNF: <c>ip-literal-address := [ (IPv6address | IPvFuture) ]</c></para>
    /// <para>Named groups: <see cref="Ipv6Gr"/> or <see cref="IpvfGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string ipLiteralAddressRex = $@"(?: \[ (?: {Ipv6AddressRex} | {IpvFutureAddressRex} ) \] )";

    /// <summary>
    /// Matches an IP address in numeric form IPv4, IPv6, or IPvFuture.
    /// <para>BNF: <c>ip-numeric-address := IPv4address | "[" (IPv6address | IPvFuture) "]" </c></para>
    /// <para>Named groups: <see cref="Ipv4Gr"/>, <see cref="Ipv6Gr"/>, or <see cref="IpvfGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    internal const string IpNumericAddressRex = $@"{Ipv4AddressRex} | {ipLiteralAddressRex}";

    /// <summary>
    /// The name of a matching group representing a host name.
    /// </summary>
    public const string HostGr = "host";

    #region Host
    /// <summary>
    /// Matches a host in a string.
    /// <para>BNF: <c>host := IP-literal | IPv4address | reg-name</c></para>
    /// <para>
    /// Named groups: <see cref="HostGr"/>, and one of: <see cref="IpDnsNameGr"/>, <see cref="Ipv4Gr"/>, 
    /// <see cref="Ipv6Gr"/> or <see cref="IpvfGr"/>.
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
    /// <see cref="Ipv6Gr"/> or <see cref="IpvfGr"/>.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string HostRegex = $@"^{HostRex}$";

    static readonly Lazy<Regex> regexHost = new(() => new(HostRegex, RegexOptions.Compiled |
                                                                     RegexOptions.IgnorePatternWhitespace, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// Gets a Regex object which matches a string representing a host.
    /// </summary>
    public static Regex Host => regexHost.Value;
    #endregion

    #region Address (Host:Port)
    /// <summary>
    /// The name of a matching group representing an IP address.
    /// </summary>
    public const string AddressGr = "address";

    /// <summary>
    /// Matches an IP address.
    /// <para>BNF: <c>address := host [: port]</c></para>
    /// <para>Named groups: <see cref="AddressGr"/> <see cref="HostGr"/>, <see cref="PortGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string AddressRex = $"(?<{AddressGr}> {HostRex} (?: : {PortRex})? )";

    /// <summary>
    /// Matches a string that represents an IP address.
    /// <para>BNF: <c>address := host [: port]</c></para>
    /// <para>Named groups: <see cref="HostGr"/>, <see cref="PortGr"/>.</para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string AddressRegex = $"^{AddressRex}$";

    static readonly Lazy<Regex> addressRegex = new(() => new(AddressRegex, RegexOptions.Compiled |
                                                                           RegexOptions.IgnorePatternWhitespace, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IP address.
    /// <para>BNF: <c>address := host [: port]</c></para>
    /// <para>Named groups: <see cref="HostGr"/>, <see cref="PortGr"/>.</para>
    /// </summary>
    /// <value>The address.</value>
    public static Regex Address => addressRegex.Value;
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

    static readonly Lazy<Regex> portRegex = new(() => new(PortRegex, RegexOptions.Compiled |
                                                                     RegexOptions.IgnorePatternWhitespace, TimeSpan.FromMilliseconds(500)));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IP port.
    /// <para>BNF: <c>port := decimal-num</c> in the range [0-65535]</para>
    /// <para>Named groups: <see cref="PortGr"/>.</para>
    /// </summary>
    public static Regex Port => portRegex.Value;

    #endregion
}
