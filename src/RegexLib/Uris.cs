
using System.Net.Sockets;

namespace vm2.RegexLib;

/// <summary>
/// Class Uri. Defines many regular expressions with the ultimate goal to define a regular expression for URI.
/// Follows closely the definitions in
/// RFC 3986: https://datatracker.ietf.org/doc/html/rfc3986 and
/// RFC 1034 https://datatracker.ietf.org/doc/html/rfc1034
/// </summary>
public class Uris
{
    /// <summary>
    /// Matches an empty string.
    /// </summary>
    public const string EmptyRex = "(?:)";

    const string alphaNumChars = $"{Ascii.AlphaChars} {Numerics.DecDigitChars}";

    /// <summary>
    /// Matches an alpha numeric character.
    /// <para>BNF: <c>alpha-digit := alpha | digit</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string AlphaDigitRex = $"[{alphaNumChars}]";

    // :/?#[]@
    const string genDelimiterChars = @": / # @ \? \[ \]";

    /// <summary>
    /// Matches a generic delimiter character.
    /// <para>BNF: <c>generic-delimiter := : | / | ? | # | [ | ] | @</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string genDelimiterRex = $"[{genDelimiterChars}]";

    // !$'()*+,;
    const string subDelimiterNoEqAmpChars = @"! ' , ; \$ \( \) \* \+";

    /// <summary>
    /// Matches a sub-delimiter character.
    /// <para>BNF: <c>sub-delimiter-not-na := ! | $ | ' | ( | ) | * | + | , | ;</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string subDelimiterNoEqAmpRex = $"[{subDelimiterNoEqAmpChars}]";

    // !$'()*+,;=&
    const string subDelimiterChars = $@"{subDelimiterNoEqAmpChars} = &";

    /// <summary>
    /// Matches a sub-delimiter character.
    /// <para>BNF: <c>sub-delimiter := sub-delimiter-not-na | = | &amp;</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string subDelimitersRex = $"[{subDelimiterChars}]";

    // :/?#[]@!$'()*+,;=&
    const string reservedChars = $@"{genDelimiterChars} {subDelimiterChars}";

    /// <summary>
    /// Matches a reserved character.
    /// <para>BNF: <c>reserved := generic-delimiter | sub-delimiter</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string reservedRex = $"[{reservedChars}]";

    /// <summary>
    /// Matches a percent encoded character.
    /// <para>BNF: <c>pct-encoded := % hex_digit hex_digit</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pctEncodedRex = $"(?: % {Numerics.HexDigitRex} {Numerics.HexDigitRex} )";

    // a-zA-Z0-9_~.-
    const string unreservedChars = $@"{alphaNumChars} _ ~ \. \-";

    /// <summary>
    /// Matches unreserved character.
    /// <para>BNF: <c>unreserved := alpha-digit | _ | ~ | . | -</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string unreservedRex = $@"[{unreservedChars}]";

    const string unreservedOrSubDelimiterChars = $"{unreservedChars} {subDelimiterChars}";

    /// <summary>
    /// Matches an unreserved or sub-delimiter character.
    /// <para>BNF: <c>unreserved-or-sub-delimiter := unreserved | sub-delimiter</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string unreservedOrSubDelimiterRex = $"[{unreservedOrSubDelimiterChars}]";

    /// <summary>
    /// The name of a matching group representing the scheme of a URI.
    /// </summary>
    public const string G_SCHEME = "scheme";

    /// <summary>
    /// Matches a URI scheme.
    /// <para>BNF: <c>scheme := 1*[ alpha | digit | + | - | . ]</c></para>
    /// Named groups: <see cref="G_SCHEME"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string SchemeRex = $@"(?<{G_SCHEME}>{Ascii.AlphaRex}[{alphaNumChars} \. \+ \-]*)";

    /// <summary>
    /// Matches a string that represents a URI scheme.
    /// <para>BNF: <c>scheme := 1*[ alpha | digit | + | - | . ]</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string SchemeRegex = $@"^{SchemeRex}$";

    static readonly Lazy<Regex> rexSchemeRegex = new(() => new(SchemeRegex, RegexOptions.Compiled|
                                                                            RegexOptions.CultureInvariant|
                                                                            RegexOptions.IgnorePatternWhitespace|
                                                                            RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a URI scheme.
    /// <para>BNF: <c>scheme := 1*[ alpha | digit | + | - | . ]</c></para>
    /// </summary>
    public static Regex Scheme => rexSchemeRegex.Value;

    /// <summary>
    /// Matches a decimal number from 0 to 255 (0x00 - 0xFF)
    /// </summary>
    const string decimalOctetRex =
         $"(?: 25[0-5] | "+
         $"2[0-4]{Numerics.DecDigitRex} | "+
         $"1{Numerics.DecDigitRex}{Numerics.DecDigitRex} | "+
         $"[1-9]{Numerics.DecDigitRex} | "+
         $"{Numerics.DecDigitRex} )";

    /// <summary>
    /// Matches an IPv4 address.
    /// </summary>
    const string ipv4Rex = $@"{decimalOctetRex}(?:\.{decimalOctetRex}){{3}}";

    /// <summary>
    /// The name of a matching group representing an IPv4 address.
    /// </summary>
    public const string G_IPV4 = "ipv4";

    /// <summary>
    /// Matches an IPv4 address.
    /// <para>BNF: <c>ipv4 := dec-octet.dec-octet.dec-octet.dec-octet</c></para>
    /// Named groups: <see cref="G_IPV4"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string Ipv4AddressRex = $@"(?<{G_IPV4}>{ipv4Rex})";

    /// <summary>
    /// Matches a string that represents an IPv4 address.
    /// <para>BNF: <c>ipv4 := dec-octet.dec-octet.dec-octet.dec-octet</c></para>
    /// Named groups: <see cref="G_IPV4"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string Ipv4AddressRegex = $@"^{Ipv4AddressRex}$";

    static readonly Lazy<Regex> ipv4AddressRegex = new(() => new(Ipv4AddressRegex, RegexOptions.Compiled|
                                                                                   RegexOptions.CultureInvariant|
                                                                                   RegexOptions.IgnorePatternWhitespace|
                                                                                   RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IPv4 address.
    /// <para>BNF: <c>ipv4 := dec-octet.dec-octet.dec-octet.dec-octet</c></para>
    /// Named groups: <see cref="G_IPV4"/>.
    /// </summary>
    public static Regex Ipv4Address => ipv4AddressRegex.Value;

    // Numeric IPv6 addresses, e.g. 1:2:3:4:5::8 or 1:2:3::4.5.6.7
    //
    // IPv6address =                              6( h16 ":" ) l32
    //               |                       "::" 5( h16 ":" ) l32
    //               | [               h16 ] "::" 4( h16 ":" ) l32
    //               | [ *1( h16 ":" ) h16 ] "::" 3( h16 ":" ) l32
    //               | [ *2( h16 ":" ) h16 ] "::" 2( h16 ":" ) l32
    //               | [ *3( h16 ":" ) h16 ] "::"    h16 ":"   l32
    //               | [ *4( h16 ":" ) h16 ] "::"              l32
    //               | [ *5( h16 ":" ) h16 ] "::"              h16
    //               | [ *6( h16 ":" ) h16 ] "::"
    //
    // h16         = 1*4HEXDIG
    //               ; 16 bits of address represented in hex_digit
    //
    // l32        = ( h16 ":" h16 ) | IPv4address
    //               ; least-significant 32 bits of address

    const string h16 = $@"{Numerics.HexDigitRex}{{1,4}}";
    const string l32 = $@"(? (?: {h16}:{h16} )|(?: {ipv4Rex} ) )";  // 7:8 or 10.11.12.13

    /// <summary>
    /// The name of a matching group representing an IPv6 address.
    /// </summary>
    public const string G_IPV6 = "ipv6";

    /// <summary>
    /// Matches an IPv6 address.
    /// Named groups: <see cref="G_IPV6"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string Ipv6AddressRex =
        $"(?<{G_IPV6}> (?: {h16} :){{6}} {l32} |" +                         // h:h:h:h:h:h:dd.dd.dd.dd  or  h:h:h:h:h:h:h:h
        $"::(?: {h16} :){{5}} {l32}" +                                      // ::h:h:h:h:h:dd.dd.dd.dd  or  ::h:h:h:h:h:h:h
        $"(?: {h16} )? :: (?: {h16} :){{4}} {l32}" +                        // h::h:h:h:h:dd.dd.dd.dd   or  h::h:h:h:h:h:h
        $"(?: (?: {h16} :){{0,1}}{h16} )? :: (?: {h16} : ){{3}} {l32}" +    // h:h::h:h:h:dd.dd.dd.dd   or  h::h:h:h:dd.dd.dd.dd  or  h:h::h:h:h:h:h      or  h::h:h:h:h:h                                                                  
        $"(?: (?: {h16} :){{0,2}}{h16} )? :: (?: {h16} : ){{2}} {l32}" +    // h:h:h::h:h:dd.dd.dd.dd   or  h:h::h:h:dd.dd.dd.dd  or  h::h:h:dd.dd.dd.dd  or  h:h:h::h:h:h:h    or  h:h::h:h:h:h    or  h::h:h:h:h                                          
        $"(?: (?: {h16} :){{0,3}}{h16} )? :: (?: {h16} : ) {l32}" +         // h:h:h:h::h:dd.dd.dd.dd   or  h:h:h::h:dd.dd.dd.dd  or  h:h::h:dd.dd.dd.dd  or  h::h:dd.dd.dd.dd  or  h:h:h:h::h:h:h  or  h:h:h::h:h:h     or  h:h::h:h:h    or  h::h:h:h                    
        $"(?: (?: {h16} :){{0,4}}{h16} )? :: {l32}" +                       // h:h:h:h:h::dd.dd.dd.dd   or  h:h:h:h::dd.dd.dd.dd  or  h:h:h::dd.dd.dd.dd  or  h:h::dd.dd.dd.dd  or  h::dd.dd.dd.dd  or  h:h:h:h:h::h:h   or  h:h:h:h::h:h  or  h:h:h::h:h  or  h:h::h:h  or  h::h:h
        $"(?: (?: {h16} :){{0,5}}{h16} )? :: {h16}" +                       // h:h:h:h:h:h::h           or  h:h:h:h:h::h          or  h:h:h:h::h          or  h:h:h::h          or  h:h::h          or  h::h
        $"(?: (?: {h16} :){{0,6}}{h16} )? :: )";                            // h:h:h:h:h:h:h::          or  h:h:h:h:h:h::         or  h:h:h:h:h::         or  h:h:h:h::         or  h:h:h::         or  h:h::  or  h::

    /// <summary>
    /// Matches a string that represents an IPv6 address.
    /// Named groups: <see cref="G_IPV6"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string Ipv6AddressRegex = $@"^{Ipv6AddressRex}$";

    static readonly Lazy<Regex> ipv6AddressRegex = new(() => new(Ipv6AddressRegex, RegexOptions.Compiled|
                                                                                   RegexOptions.CultureInvariant|
                                                                                   RegexOptions.IgnorePatternWhitespace|
                                                                                   RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IPv6 address.
    /// Named groups: <see cref="G_IPV6"/>.
    /// </summary>
    public static Regex Ipv6Address => ipv6AddressRegex.Value;

    /// <summary>
    /// The name of a matching group representing an IPv.Future address.
    /// </summary>
    public const string G_IPVF = "ipvF";

    /// <summary>
    /// Matches an IPv.future address.
    /// Named groups: <see cref="G_IPVF"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string IpvFutureAddressRex = $@"(?<{G_IPVF}> v{Numerics.HexDigitRex}+ \. [{unreservedOrSubDelimiterChars}:]+ )";

    /// <summary>
    /// Matches a string that represents an IPv.future address
    /// </summary>
    public const string IpvFutureAddressRegex = $"^{IpvFutureAddressRex}$";

    static readonly Lazy<Regex> ipvFutureAddressRegex = new(() => new(IpvFutureAddressRegex, RegexOptions.Compiled|
                                                                                             RegexOptions.CultureInvariant|
                                                                                             RegexOptions.IgnorePatternWhitespace|
                                                                                             RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IPv.future address.
    /// Named groups: <see cref="G_IPV6"/>.
    /// </summary>
    public static Regex IpvFutureAddress => ipvFutureAddressRegex.Value;

    /// <summary>
    /// Matches a IP address.
    /// <para>BNF: <c>ip-literal-address := [ (IPv6address | IPvFuture) ]</c></para>
    /// Named groups: <see cref="G_IPV6"/> or <see cref="G_IPVF"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string ipLiteralAddressRex = $@"[ {Ipv6AddressRex} | {IpvFutureAddressRex} ]";

    // a-zA-Z0-9-
    const string alphaDigitHyphenChars = $@"{Ascii.AlphaChars} {Numerics.DecDigitChars} \-";

    /// <summary>
    /// Matches an alpha-digit-hyphen character from RFC1034
    /// <para>BNF: <c>alpha-digit-hyphen := alpha | digit | -</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string alphaDigitHyphenRex = $@"[{alphaDigitHyphenChars}]";

    /// <summary>
    /// Matches a domain name label (e.g. google in www.google.com) from RFC 1034
    /// <para>BNF: <c>label := alpha [ *61[ alpha-digit-hyphen ] alpha-digit ]</c></para>
    /// </summary>
    public const string LabelRex = $@"(?: {Ascii.AlphaRex}(?: {alphaDigitHyphenRex}{{0,61}} {AlphaDigitRex} )? )";

    /// <summary>
    /// The name of a matching group representing a name that can be looked up in DNS.
    /// </summary>
    public const string G_IP_DNS_NAME = "ipDnsName";

    /// <summary>
    /// Matches registered DNS name.
    /// <para>BNF: <c>registered_name = *[ domainLabel . ] topLabel</c> (RFC 1034)</para>
    /// Named groups: <see cref="G_IP_DNS_NAME"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string DnsNameRex = $@"(?<{G_IP_DNS_NAME}> {LabelRex} (?: \. {LabelRex} ){{0,127}})";

    /// <summary>
    /// The name of a matching group representing an IP general name.
    /// </summary>
    public const string G_GEN_NAME = "ipGenName";

    /// <summary>
    /// Matches reg-name in RFC 3986
    /// <para>BNF: <c>registered_name = *[ unreserved | sub-delimiters | pct-encoded ]</c> - yes, it can be empty, see the RFC</para>
    /// </summary>
    const string generalNameRex = $@"(?<{G_GEN_NAME}> ([{unreservedOrSubDelimiterChars}] | {pctEncodedRex})* )";

    /// <summary>
    /// Matches a registered name.
    /// TODO: According to the RFC 3986 registered_name should be dns_name or general_name but is this practical as the
    /// domain names are subset of the general names and are used most of the time?
    /// Named groups: <see cref="G_IP_DNS_NAME"/>, <see cref="G_GEN_NAME"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string RegisteredNameRex = $@"{DnsNameRex} | {generalNameRex}";
    // it used to be: `const string registeredNameRex = $@"{dnsNameRex}";`

    /// <summary>
    /// The name of a matching group representing a host name.
    /// </summary>
    public const string G_HOST = "host";

    /// <summary>
    /// Matches a host.
    /// <para>BNF: <c>host := IP-literal | IPv4address | reg-name</c></para>
    /// Named groups: <see cref="G_HOST"/>, and one of: <see cref="G_GEN_NAME"/>, <see cref="G_IPV4"/>, <see cref="G_IPV6"/> or <see cref="G_IPVF"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string HostRex = $@"(?<{G_HOST}> {ipv4Rex} | {ipLiteralAddressRex} | {RegisteredNameRex} )";

    /// <summary>
    /// Matches a string that represents a host.
    /// <para>BNF: <c>host := IP-literal | IPv4address | reg-name</c></para>
    /// Named groups: <see cref="G_HOST"/>, and one of: <see cref="G_GEN_NAME"/>, <see cref="G_IP_DNS_NAME"/>, <see cref="G_IPV4"/>, <see cref="G_IPV6"/> or <see cref="G_IPVF"/>.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string HostRegex = $@"^[{HostRex}]$";

    static readonly Lazy<Regex> hostRegex = new(() => new(HostRegex, RegexOptions.Compiled|
                                                                     RegexOptions.CultureInvariant|
                                                                     RegexOptions.IgnorePatternWhitespace|
                                                                     RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a host.
    /// <para>BNF: <c>host := IP-literal | IPv4address | reg-name</c></para>
    /// Named groups: <see cref="G_HOST"/>, and one of: <see cref="G_GEN_NAME"/>, <see cref="G_IP_DNS_NAME"/>, <see cref="G_IPV4"/>, <see cref="G_IPV6"/> or <see cref="G_IPVF"/>.
    /// </summary>
    public static Regex Host => hostRegex.Value;

    /// <summary>
    /// The name of a matching group representing a port.
    /// </summary>
    public const string G_PORT = "port";

    /// <summary>
    /// Matches an IP port.
    /// <para>BNF: <c>port := decimal-num</c> in the range [0-65535]</para>
    /// Named groups: <see cref="G_PORT"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PortRex = 
        $@"(?<{G_PORT}>" +
        "6553[0-5] |" +
        "655[0-2][0-9] |" +
        "65[0-4][0-9]{2} |" +
        "6[0-4][0-9]{3} |" +
        "[1-5][0-9]{4} |" +
        "[0-9]{1,4}" +
        ")";

    /// <summary>
    /// Matches a string that represents an IP port.
    /// <para>BNF: <c>port := decimal-num</c> in the range [0-65535]</para>
    /// Named groups: <see cref="G_PORT"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PortRegex = $@"^{PortRex}$";

    static readonly Lazy<Regex> portRegex = new(() => new(PortRegex, RegexOptions.Compiled|
                                                                     RegexOptions.CultureInvariant|
                                                                     RegexOptions.IgnorePatternWhitespace|
                                                                     RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IP port.
    /// <para>BNF: <c>port := decimal-num</c> in the range [0-65535]</para>
    /// Named groups: <see cref="G_PORT"/>
    /// </summary>
    public static Regex Port => portRegex.Value;

    /// <summary>
    /// The name of a matching group representing an IP address.
    /// </summary>
    public const string G_ADDRESS = "address";

    /// <summary>
    /// Matches an IP address.
    /// <para>BNF: <c>address := host [: port]</c></para>
    /// Named groups: <see cref="G_ADDRESS"/> <see cref="G_HOST"/>, <see cref="G_PORT"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string AddressRex = $"(?<{G_ADDRESS}> {HostRex} (?: : {PortRex})? )";

    /// <summary>
    /// Matches a string that represents an IP address.
    /// <para>BNF: <c>address := host [: port]</c></para>
    /// Named groups: <see cref="G_HOST"/>, <see cref="G_PORT"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string AddressRegex = $"^{AddressRex}$";

    static readonly Lazy<Regex> addressRegex = new(() => new(AddressRegex, RegexOptions.Compiled|
                                                                           RegexOptions.CultureInvariant|
                                                                           RegexOptions.IgnorePatternWhitespace|
                                                                           RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an IP address.
    /// <para>BNF: <c>address := host [: port]</c></para>
    /// Named groups: <see cref="G_HOST"/>, <see cref="G_PORT"/>
    /// </summary>
    /// <value>The address.</value>
    public static Regex Address => addressRegex.Value;

    /// <summary>
    /// The name of a matching group representing a URI address.
    /// </summary>
    public const string G_URI_ADDR = "uriAddr";

    /// <summary>
    /// Matches a URI address.
    /// <para>BNF: <c>uri-address := scheme :// address </c></para>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// Named groups: <see cref="G_SCHEME"/>, <see cref="G_URI_ADDR"/>, <see cref="G_ADDRESS"/>, <see cref="G_HOST"/>, <see cref="G_PORT"/>
    /// </summary>
    public const string UriAddressRex = $@"(?<{G_URI_ADDR}> {SchemeRex} :// {AddressRex})";

    /// <summary>
    /// Matches a string that represents a URI address.
    /// <para>BNF: <c>uri-address := scheme :// address </c></para>
    /// Named groups: <see cref="G_SCHEME"/>, <see cref="G_URI_ADDR"/>, <see cref="G_ADDRESS"/>, <see cref="G_HOST"/>, <see cref="G_PORT"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public static string UriAddressRegex = $"^{UriAddressRex}$";

    static readonly Lazy<Regex> uriAddressRegex = new(() => new(UriAddressRegex, RegexOptions.Compiled|
                                                                                 RegexOptions.CultureInvariant|
                                                                                 RegexOptions.IgnorePatternWhitespace|
                                                                                 RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an URI address.
    /// <para>BNF: <c>uri-address := scheme :// address </c></para>
    /// Named groups: <see cref="G_SCHEME"/>, <see cref="G_HOST"/>, <see cref="G_PORT"/>
    /// </summary>
    public static Regex UriAddress = uriAddressRegex.Value;

    /// <summary>
    /// The name of a matching group representing a user name.
    /// </summary>
    public const string G_USER = "user";

    /// <summary>
    /// Matches URI's user name.
    /// <para>BNF: <c>user := *[ unreserved | sub-delimiters | pct-encoded ]</c></para>
    /// Named groups: <see cref="G_USER"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string UserRex = $@"(?<{G_USER}> (?: {unreservedOrSubDelimiterRex} | {pctEncodedRex} )* )";

    /// <summary>
    /// Matches a string that represents a URI's user name.
    /// <para>BNF: <c>user := *[ unreserved | sub-delimiters | pct-encoded ]</c></para>
    /// Named groups: <see cref="G_USER"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string UserRegex = $@"^{UserRex}$";

    static readonly Lazy<Regex> userRegex = new(() => new(UserRegex, RegexOptions.Compiled|
                                                                     RegexOptions.CultureInvariant|
                                                                     RegexOptions.IgnorePatternWhitespace|
                                                                     RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an URI's user name.
    /// Named groups: <see cref="G_USER"/>
    /// </summary>
    public static Regex User => userRegex.Value;

    /// <summary>
    /// The name of a matching group representing the user's password.
    /// </summary>
    public const string G_PASSWORD = "password";

    /// <summary>
    /// Matches URI's password.
    /// <para>BNF: <c>password := *[ unreserved | sub-delimiters | : | pct-encoded ]</c></para>
    /// Named groups: <see cref="G_PASSWORD"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PasswordRex = $@"(?<{G_PASSWORD}> (?: {unreservedOrSubDelimiterRex} | : | {pctEncodedRex} )* )";

    /// <summary>
    /// Matches a string that represents a URI's password.
    /// <para>BNF: <c>password := *[ unreserved | sub-delimiters | : | pct-encoded ]</c></para>
    /// Named groups: <see cref="G_PASSWORD"/>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string PasswordRegex = $@"^{PasswordRex}$";

    static readonly Lazy<Regex> passwordRegex = new(() => new(PasswordRegex, RegexOptions.Compiled|
                                                                             RegexOptions.CultureInvariant|
                                                                             RegexOptions.IgnorePatternWhitespace|
                                                                             RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents an URI's password name.
    /// <para>BNF: <c>password := *[ unreserved | sub-delimiters | : | pct-encoded ]</c></para>
    /// Named groups: <see cref="G_PASSWORD"/>
    /// </summary>
    public static Regex Password => passwordRegex.Value;

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

    //[Obsolete]
    //public const string UserInfoRex = $@"{UserRex}(?:{PasswordRex})?";

    /// <summary>
    /// Matches the URI's authority.
    /// <para>BNF: <c>authority := [ user-info @ ] host [ : port ]</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string AuthorityRex = $"(?: {UserInfoRex} @ ) {AddressRex}";

    /// <summary>
    /// Path characters (without the colon char).
    /// <para>BNF: <c>path-nc-chars := unreserved | sub-delimiters | @</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathNcChars = $"{unreservedOrSubDelimiterChars} @";

    /// <summary>
    /// Matches a character from a path (without the colon char).
    /// <para>BNF: <c>path-nc-char := unreserved | sub-delimiters | @ | pct-encoded</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathNcRex = $"(?:[{pathNcChars}] | {pctEncodedRex})";

    /// <summary>
    /// Path characters (incl. the colon char).
    /// <para>BNF: <c>path-nc-chars := path-nc-chars | :</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathChars = $"{pathNcChars} :";

    /// <summary>
    /// Matches a path character (incl. the colon char).
    /// <para>BNF: <c>path-nc-char := path-nc-char | : | pct-encoded</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathCharRex = $"(?:[{pathChars}] | {pctEncodedRex})";

    /// <summary>
    /// Matches non-zero length path segment without colon
    /// <para>BNF: <c>segment-nz-nc := 1*( unreserved | sub-delimiters | @ | pct-encoded )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathSegmentNzNcRex = $"[{pathNcRex}]+";

    /// <summary>
    /// Matches non-zero length path segment incl. colon
    /// <para>BNF: <c>segment-nz := 1*( unreserved | sub-delimiters | @ | : | pct-encoded )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathSegmentNzRex = $"[{pathCharRex}]+";


    /// <summary>
    /// Matches path segment incl. colon (can be empty)
    /// <para>BNF: <c>segment := *( unreserved | sub-delimiters | @ | : | pct-encoded )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathSegmentRex = $"[{pathCharRex}]*";

    /// <summary>
    /// Matches empty path segment.
    /// <para>BNF: <c>segment := 0( any-char )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathEmptyRex = EmptyRex;

    /// <summary>
    /// The name of a matching group representing a URI path
    /// </summary>
    public const string G_PATH = "path";

    /// <summary>
    /// Matches path without the root ('/').
    /// <para>BNF: <c>path-rootless := segment-nz *( / segment )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathRootlessRex = $"(?<{G_PATH}> {pathSegmentNzRex} (?: / {pathSegmentRex})*)";

    /// <summary>
    /// Matches a path with no scheme.
    /// <para>BNF: <c>path-no-scheme := segment-nz-nc *( / segment )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathNoSchemeRex = $"(?:{pathSegmentNzNcRex} (?: / {pathSegmentRex})*)";

    /// <summary>
    /// Matches an absolute path starting with '/' but not '//'.
    /// <para>BNF: <c>path-absolute := / path-rootless</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathAbsoluteRex = $"(?<{G_ABS_PATH}> / (?: {pathSegmentNzRex} (?: / {pathSegmentRex})* )? )";

    /// <summary>
    /// The name of a matching group representing a URI path
    /// </summary>
    public const string G_ABS_PATH = "absPath";

    /// <summary>
    /// Matches an absolute or empty path.
    /// <para>BNF: <c>path-abs-empty := *( / segment )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string pathAbsoluteOrEmptyRex = $"(?<{G_ABS_PATH}> / {pathSegmentRex})*";

    /// <summary>
    /// The name of a matching group representing the path in a URI
    /// </summary>
    public const string G_URI_PATH = "path";

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
    const string pathRex = $"(?P<{G_URI_PATH}> {pathAbsoluteOrEmptyRex} | {pathAbsoluteRex} | {pathNoSchemeRex} | {pathRootlessRex} | {pathEmptyRex})";

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

    static readonly Lazy<Regex> pathRegex = new(() => new(PathRegex, RegexOptions.Compiled|
                                                                     RegexOptions.CultureInvariant|
                                                                     RegexOptions.IgnorePatternWhitespace|
                                                                     RegexOptions.Singleline));

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

    /// <summary>
    /// The generic query chars.
    /// <para>BNF: <c>generic-query-chars := a-zA-Z0-9._~-!$'()*+,;=&amp;@:/?</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string genericQueryChars = $@"{pathChars} / \?";

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
    public const string G_GEN_QUERY = "query";

    /// <summary>
    /// Matches a URI query.
    /// <para>BNF: <c>gen-query := *( path-char | / | ? )</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string genericQueryRex = $@"(?<{G_GEN_QUERY}> {genericQueryCharRex}*)";

    /// <summary>
    /// Matches a string that represents a generic query.
    /// <para>BNF: <c>gen-query := *( path-char | / | ? )</c>
    /// </para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string GenericQueryRegex = $@"^{genericQueryRex}$";

    static readonly Lazy<Regex> genericQueryRegex = new(() => new(GenericQueryRegex, RegexOptions.Compiled|
                                                                                     RegexOptions.CultureInvariant|
                                                                                     RegexOptions.IgnorePatternWhitespace|
                                                                                     RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a generic query.
    /// </summary>
    public static Regex GenericQuery => genericQueryRegex.Value;

    /// <summary>
    /// Key-value query character set: ! ' , ; \$ \( \) \* \+
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string keyValueChars = $@"{unreservedChars} {subDelimiterNoEqAmpChars} : @ / \?";

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
    public const string G_KEY = "key";

    /// <summary>
    /// The name of a matching group representing a value from a key-value query in a URI
    /// </summary>
    public const string G_VALUE = "value";

    /// <summary>
    /// Matches a key from a URI key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string queryKeyRex   = $"(?<{G_KEY}> {keyValueCharRex}+ )";

    /// <summary>
    /// Matches a value from a URI key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string queryValueRex = $"(?<{G_VALUE}> {keyValueCharRex}+ )";

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
    public const string G_KV_QUERY = "kv_query";

    /// <summary>
    /// Matches a URI's key-value query.
    /// <para>BNF: <c>key-value-query := key = value *(&amp; key = value)</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string keyValueQueryRex = $"(?<{G_KV_QUERY}> {queryKeyValueRex} (?: & {queryKeyValueRex} )* )";

    /// <summary>
    /// Matches a string that represents a URI's key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string KeyValueQueryRegex = $"^{keyValueQueryRex}$";

    static readonly Lazy<Regex> keyValueQueryRegex = new(() => new(KeyValueQueryRegex, RegexOptions.Compiled|
                                                                                       RegexOptions.CultureInvariant|
                                                                                       RegexOptions.IgnorePatternWhitespace|
                                                                                       RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a URI's key-value query
    /// </summary>
    public static Regex KeyValueQuery => keyValueQueryRegex.Value;

    /// <summary>
    /// Matches a URI's query.
    /// <para>BNF: <c>query := key-value-query | generic-query</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string queryRex = $"(?:{keyValueQueryRex} | {genericQueryRex})";  // didn't work before?

    const string fragmentChars = $@"{pathChars} / \?";

    const string fragmentCharRex = $@"(?:[{fragmentChars}] | {pctEncodedRex})";

    /// <summary>
    /// The name of a matching group representing a URI fragment
    /// </summary>
    public const string G_FRAGMENT = "fragment";

    const string fragmentRex = $"(?<{G_FRAGMENT}> {fragmentCharRex}*)";

    /// <summary>
    /// Matches a URI's fragment.
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string FragmentRegex = $"^{fragmentRex}$";

    static readonly Lazy<Regex> fragmentRegex = new(() => new(FragmentRegex, RegexOptions.Compiled|
                                                                             RegexOptions.CultureInvariant|
                                                                             RegexOptions.IgnorePatternWhitespace|
                                                                             RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a URI's fragment
    /// </summary>
    public static Regex Fragment = fragmentRegex.Value;

    /// <summary>
    /// Matches a URI's hierarchical part
    /// <para>BNF: <c>hierarchical-part = '//' authority path-ab-empty | path-absolute | path-rootless | path-empty</c></para>
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string hierarchicalPartRex = $"//{AuthorityRex}(?: {pathAbsoluteOrEmptyRex} | {pathAbsoluteRex} | {pathRootlessRex} | {pathEmptyRex})";

    /// <summary>
    /// The name of a matching group representing a URI
    /// </summary>
    public const string G_URI = "uri";

    /// <summary>
    /// Matches a URI with a key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    const string uriKeyValueQueryRex = $@"(?<{G_URI}> {SchemeRex} : {hierarchicalPartRex} (?: \? {keyValueQueryRex} )? (?: {Ascii.Hash} {fragmentRex} )? )";

    /// <summary>
    /// Matches a string that represents a URI with a key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string UriKeyValueQueryRegex = $"^{uriKeyValueQueryRex}$";

    static readonly Lazy<Regex> uriKeyValueQueryRegex = new(() => new(UriKeyValueQueryRegex, RegexOptions.Compiled|
                                                                                             RegexOptions.CultureInvariant|
                                                                                             RegexOptions.IgnorePatternWhitespace|
                                                                                             RegexOptions.Singleline));

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
    const string uriGenericQueryRex = $@"(?<{G_URI}> {SchemeRex} : {hierarchicalPartRex} (?: \? {genericQueryRex} )? (?: {Ascii.Hash} {fragmentRex} )? )";

    /// <summary>
    /// Matches a string that represents a URI with a key-value query
    /// </summary>
    /// <remarks>
    /// Requires <see cref="RegexOptions.IgnorePatternWhitespace"/>.
    /// </remarks>
    public const string UriGenericQueryRegex = $"^{uriGenericQueryRex}$";

    static readonly Lazy<Regex> uriGenericQueryRegex = new(() => new(UriGenericQueryRegex, RegexOptions.Compiled|
                                                                                           RegexOptions.CultureInvariant|
                                                                                           RegexOptions.IgnorePatternWhitespace|
                                                                                           RegexOptions.Singleline));

    /// <summary>
    /// A <see cref="Regex"/> object that matches a string that represents a URI with a key-value query
    /// </summary>
    public static Regex UriGenericQuery => uriGenericQueryRegex.Value;

    // TODO: relative URI-s
}
