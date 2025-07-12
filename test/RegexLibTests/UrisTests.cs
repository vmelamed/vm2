namespace vm2.RegexLibTests;

public class UriTheoryData : TheoryData<string, bool, string, Captures?>
{
}

public partial class UrisTests(
    RegexLibTestsFixture fixture,
    ITestOutputHelper output) : RegexTests(fixture, output)
{
    public static UriTheoryData SchemeData = new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), false, "1", null },
        { TestFileLine(), false, "1", null },
        { TestFileLine(), false, "1a", null },
        { TestFileLine(), false, ".", null },
        { TestFileLine(), false, "+", null },
        { TestFileLine(), false, "-", null },
        { TestFileLine(), false, ".a", null },
        { TestFileLine(), false, "+a", null },
        { TestFileLine(), false, "-a", null },
        { TestFileLine(), false, ".1", null },
        { TestFileLine(), false, "+2", null },
        { TestFileLine(), false, "-3", null },
        { TestFileLine(), false, "ж", null },
        { TestFileLine(), false, "%41%42", null },
        { TestFileLine(), true, "a", new() { ["scheme"] = "a" } },
        { TestFileLine(), true, "abc", new() { ["scheme"] = "abc" } },
        { TestFileLine(), true, "a1", new() { ["scheme"] = "a1" } },
        { TestFileLine(), true, "abc123", new() { ["scheme"] = "abc123" } },
        { TestFileLine(), true, "a1b2c3", new() { ["scheme"] = "a1b2c3" } },
        { TestFileLine(), true, "a1.b2+c3-", new() { ["scheme"] = "a1.b2+c3-" } },
        { TestFileLine(), true, Uri.UriSchemeFile, new() { ["scheme"] = Uri.UriSchemeFile } },
        { TestFileLine(), true, Uri.UriSchemeFtp, new() { ["scheme"] = Uri.UriSchemeFtp } },
        { TestFileLine(), true, Uri.UriSchemeFtps, new() { ["scheme"] = Uri.UriSchemeFtps } },
        { TestFileLine(), true, Uri.UriSchemeGopher, new() { ["scheme"] = Uri.UriSchemeGopher } },
        { TestFileLine(), true, Uri.UriSchemeHttp, new() { ["scheme"] = Uri.UriSchemeHttp } },
        { TestFileLine(), true, Uri.UriSchemeHttps, new() { ["scheme"] = Uri.UriSchemeHttps } },
        { TestFileLine(), true, Uri.UriSchemeMailto, new() { ["scheme"] = Uri.UriSchemeMailto } },
        { TestFileLine(), true, Uri.UriSchemeNetPipe, new() { ["scheme"] = Uri.UriSchemeNetPipe } },
        { TestFileLine(), true, Uri.UriSchemeNetTcp, new() { ["scheme"] = Uri.UriSchemeNetTcp } },
        { TestFileLine(), true, Uri.UriSchemeNews, new() { ["scheme"] = Uri.UriSchemeNews } },
        { TestFileLine(), true, Uri.UriSchemeNntp, new() { ["scheme"] = Uri.UriSchemeNntp } },
        { TestFileLine(), true, Uri.UriSchemeSftp, new() { ["scheme"] = Uri.UriSchemeSftp } },
        { TestFileLine(), true, Uri.UriSchemeSsh, new() { ["scheme"] = Uri.UriSchemeSsh } },
        { TestFileLine(), true, Uri.UriSchemeTelnet, new() { ["scheme"] = Uri.UriSchemeTelnet } },
        { TestFileLine(), true, Uri.UriSchemeWs, new() { ["scheme"] = Uri.UriSchemeWs } },
        { TestFileLine(), true, Uri.UriSchemeWss, new() { ["scheme"] = Uri.UriSchemeWss } },
    };

    [Theory]
    [MemberData(nameof(SchemeData))]
    public void TestScheme(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Scheme(), TestFileLine, shouldBe, input, captures);

    // -----

    public static UriTheoryData HostData = new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine("DnsName"), true, "maria.vtmelamed.com", new()
                                                        {
                                                            ["host"] = "maria.vtmelamed.com",
                                                            ["ipDnsName"] = "maria.vtmelamed.com",
                                                        } },
        { TestFileLine("Incomplete IPv4"), true, "1.2.3", new()
                                                        {
                                                            ["host"] = "1.2.3",
                                                            ["ipRegName"] = "1.2.3",
                                                        } },
        { TestFileLine("Complete IPv4"), true, "1.2.3.4", new()
                                                        {
                                                            ["host"] = "1.2.3.4",
                                                            ["ipv4"] = "1.2.3.4",
                                                        } },
        { TestFileLine("Complete unbracketed IPv6"), false, "1:2:3::4", null },
        { TestFileLine("Complete IPv6"), true, "[1:2:3::4]", new()
                                                        {
                                                            ["host"] = "[1:2:3::4]",
                                                            ["ipv6nz"] = "1:2:3::4",
                                                            ["ipv6"] = "1:2:3::4",
                                                        } },
        { TestFileLine("Complete IPvF"), true, "[v1a.skiledh.srethg.23546.]", new()
                                                        {
                                                            ["host"] = "[v1a.skiledh.srethg.23546.]",
                                                            ["ipvF"] = "v1a.skiledh.srethg.23546.",
                                                        } },
        { TestFileLine("Complete unbracketed IPvF"), true, "v1a.skiledh.srethg.23546.", new()
                                                        {
                                                            ["host"] = "v1a.skiledh.srethg.23546.",
                                                            ["ipRegName"] = "v1a.skiledh.srethg.23546.",
                                                        } },
        { TestFileLine("General name in Unicode"), false, "дир.бг", null },
        { TestFileLine("General name in percent URL encoded"), true, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3", new()
                                                        {
                                                            ["host"] = "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
                                                            ["ipRegName"] = "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
                                                        } },
    };

    [Theory]
    [MemberData(nameof(HostData))]
    public void TestHost(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Host(), TestFileLine, shouldBe, input, captures);

    // -----

    public static UriTheoryData NetHostData = new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine("DnsName"), true, "maria.vtmelamed.com", new()
                                                        {
                                                            ["host"] = "maria.vtmelamed.com",
                                                            ["ipDnsName"] = "maria.vtmelamed.com",
                                                        } },
        { TestFileLine("Incomplete IPv4"), false, "1.2.3", null },
        { TestFileLine("Complete IPv4"), true, "1.2.3.4", new()
                                                        {
                                                            ["host"] = "1.2.3.4",
                                                            ["ipv4"] = "1.2.3.4",
                                                        } },
        { TestFileLine("Complete unbracketed IPv6"), false, "1:2:3::4", null },
        { TestFileLine("Complete IPv6"), true, "[1:2:3::4]", new()
                                                        {
                                                            ["host"] = "[1:2:3::4]",
                                                            ["ipv6nz"] = "1:2:3::4",
                                                            ["ipv6"] = "1:2:3::4",
                                                        } },
        { TestFileLine("Complete IPvF"), true, "[v1a.skiledh.srethg.23546.]", new()
                                                        {
                                                            ["host"] = "[v1a.skiledh.srethg.23546.]",
                                                            ["ipvF"] = "v1a.skiledh.srethg.23546.",
                                                        } },
        { TestFileLine("Complete unbracketed IPvF"), false, "v1a.skiledh.srethg.23546.", null },
        { TestFileLine("General name in Unicode"), false, "дир.бг", null },
        { TestFileLine("General name in percent URL encoded"), false, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3", null },
    };

    // -----

    public static UriTheoryData FromCopilotUriData = new() {
    // Edge: Empty URI (invalid)
    { TestFileLine("empty"), false, "", null },

    // Edge: Scheme with digits, plus, dash, dot
    { TestFileLine("scheme with digits and symbols"), true, "a1+b-c.d://host", new()
        {
            ["uri"] = "a1+b-c.d://host",
            ["scheme"] = "a1+b-c.d",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
        } },

    // Edge: Host with percent-encoded characters
    { TestFileLine("host with percent-encoded"), true, "http://ex%20ample.com", new()
        {
            ["uri"] = "http://ex%20ample.com", // →uri← = →http://ex ample.com←
            ["scheme"] = "http",
            ["authority"] = "ex%20ample.com", // →authority← = →ex ample.com←
            ["endpoint"] = "ex%20ample.com", // →endpoint← = →ex ample.com←
            ["host"] = "ex%20ample.com", // →host← = →ex ample.com←
            ["ipRegName"] = "ex%20ample.com", // →ipRegName← = →ex ample.com←
        } },

    // Edge: Path with multiple consecutive slashes
    { TestFileLine("multiple slashes in path"), true, "http://host///a//b", new()
        {
            ["uri"] = "http://host///a//b",
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "///a//b",
        } },

    // Edge: Path with percent-encoded reserved characters
    { TestFileLine("percent-encoded reserved in path"), true, "http://host/a%2Fb%3Fc", new()
        {
            ["uri"] = "http://host/a%2Fb%3Fc", // →uri← = →http://host/a/b?c←
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "/a%2Fb%3Fc", // →pathAbsEmpty← = →/a/b?c←
        } },

    // Edge: Query with reserved and percent-encoded characters
    { TestFileLine("query with reserved and percent-encoded"), true, "http://host/path?foo=bar%26baz", new()
        {
            ["uri"] = "http://host/path?foo=bar%26baz", // →uri← = →http://host/path?foo=bar&baz←
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "/path",
            ["query"] = "foo=bar%26baz", // →query← = →foo=bar&baz←
        } },

    // Edge: Fragment with percent-encoded and reserved characters
    { TestFileLine("fragment with reserved and percent-encoded"), true, "http://host/path#frag%3Fment", new()
        {
            ["uri"] = "http://host/path#frag%3Fment", // →uri← = →http://host/path#frag?ment←
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "/path",
            ["fragment"] = "frag%3Fment", // →fragment← = →frag?ment←
        } },

    // Edge: Authority with userinfo and password
    { TestFileLine("authority with userinfo"), true, "ftp://user:pass@host:21/path", new()
        {
            ["uri"] = "ftp://user:pass@host:21/path",
            ["scheme"] = "ftp",
            ["authority"] = "user:pass@host:21",
            ["user"] = "user",
            ["access"] = "pass",
            ["endpoint"] = "host:21",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["port"] = "21",
            ["pathAbsEmpty"] = "/path",
        } },

    // Edge: IPv6 host
    { TestFileLine("IPv6 host"), true, "http://[2001:db8::1]/", new()
        {
            ["uri"] = "http://[2001:db8::1]/",
            ["scheme"] = "http",
            ["authority"] = "[2001:db8::1]",
            ["endpoint"] = "[2001:db8::1]",
            ["host"] = "[2001:db8::1]",
            ["ipv6"] = "2001:db8::1",
            ["ipv6nz"] = "2001:db8::1",
            ["pathAbsEmpty"] = "/",
        } },

    // Edge: IPvFuture host
    { TestFileLine("IPvFuture host"), true, "http://[v1.fe80::]/", new()
        {
            ["uri"] = "http://[v1.fe80::]/",
            ["scheme"] = "http",
            ["authority"] = "[v1.fe80::]",
            ["endpoint"] = "[v1.fe80::]",
            ["host"] = "[v1.fe80::]",
            ["ipvF"] = "v1.fe80::",
            ["pathAbsEmpty"] = "/",
        } },

    // Edge: Host with sub-delimiters
    { TestFileLine("host with sub-delimiters"), true, "http://host!$&'()*+,;=", new()
        {
            ["uri"] = "http://host!$&'()*+,;=",
            ["scheme"] = "http",
            ["authority"] = "host!$&'()*+,;=",
            ["endpoint"] = "host!$&'()*+,;=",
            ["host"] = "host!$&'()*+,;=",
            ["ipRegName"] = "host!$&'()*+,;=",
        } },

    // Edge: Path with dot segments
    { TestFileLine("path with dot segments"), true, "http://host/./../a", new()
        {
            ["uri"] = "http://host/./../a",
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "/./../a",
        } },

    // Edge: Query with multiple key-value pairs
    { TestFileLine("query with multiple kv pairs"), true, "http://host/path?foo=bar&baz=qux", new()
        {
            ["uri"] = "http://host/path?foo=bar&baz=qux",
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "/path",
            ["query"] = "foo=bar&baz=qux",
        } },

    // Edge: Fragment only
    { TestFileLine("fragment only"), true, "http://host/path#", new()
        {
            ["uri"] = "http://host/path#",
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "/path",
        } },

    // Edge: Path with allowed symbols
    { TestFileLine("path with allowed symbols"), true, "http://host/~user-name/file_name.txt", new()
        {
            ["uri"] = "http://host/~user-name/file_name.txt",
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "/~user-name/file_name.txt",
        } },

    // Edge: Path with percent-encoded slash
    { TestFileLine("path with percent-encoded slash"), true, "http://host/a%2Fb", new()
        {
            ["uri"] = "http://host/a%2Fb", // →uri← = →http://host/a/b←
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "/a%2Fb", // →pathAbsEmpty← = →/a/b←
        } },

    // Edge: Host with trailing dot (valid per RFC 3986)
    { TestFileLine("host with trailing dot"), true, "http://example.com./", new()
        {
            ["uri"] = "http://example.com./",
            ["scheme"] = "http",
            ["authority"] = "example.com.",
            ["endpoint"] = "example.com.",
            ["host"] = "example.com.",
            ["ipDnsName"] = "example.com.",
            ["pathAbsEmpty"] = "/",
        } },

    // Edge: Host with reg-name
    { TestFileLine("host with reg-name"), true, "http://.reg/", new()
        {
            ["uri"] = "http://.reg/",
            ["scheme"] = "http",
            ["authority"] = ".reg",
            ["endpoint"] = ".reg",
            ["host"] = ".reg",
            ["ipRegName"] = ".reg",
            ["pathAbsEmpty"] = "/",
        } },

    // Edge: Path with empty segment (valid)
    { TestFileLine("path with empty segment"), true, "http://host//a",  new()
        {
            ["uri"] = "http://host//a",
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "//a",
        } },

    // Edge: Query with empty value
    { TestFileLine("query with empty value"), true, "http://host/path?foo=", new()
        {
            ["uri"] = "http://host/path?foo=",
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "/path",
            ["query"] = "foo=",
        } },

    // Edge: Query with empty key (invalid)
    { TestFileLine("query with empty key"), true, "http://host/path?=bar", new()
        {
            ["uri"] = "http://host/path?=bar",
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "/path",
            ["query"] = "=bar",
        } },

    // Edge: Path with percent-encoded non-ASCII (valid percent-encoding)
    { TestFileLine("path with percent-encoded non-ASCII"), true, "http://host/%E2%82%AC", new()
        {
            ["uri"] = "http://host/%E2%82%AC", // →uri← = →http://host/€←
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "/%E2%82%AC", // →pathAbsEmpty← = →/€←
        } },

    // Edge: Path with illegal percent-encoding (invalid)
    { TestFileLine("path with illegal percent-encoding"), false, "http://host/%ZZ", null },

    // Edge: Path with reserved characters unencoded (valid)
    { TestFileLine("path with reserved characters"), true, "http://host/:@?/", new()
        {
            ["uri"] = "http://host/:@?/",
            ["scheme"] = "http",
            ["authority"] = "host",
            ["endpoint"] = "host",
            ["host"] = "host",
            ["ipDnsName"] = "host",
            ["pathAbsEmpty"] = "/:@",
            ["query"] = "/",
        } },

    // Edge: Path with Unicode (invalid per RFC 3986)
    { TestFileLine("path with Unicode"), false, "http://host/файл", null },
};

    // -----

    [Theory]
    [MemberData(nameof(NetHostData))]
    public void TestNetHost(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.Host(), TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(AddressData))]
    public void TestAddress(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Endpoint(), TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(NetAddressData))]
    public void TestNetAddress(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.Endpoint(), TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(AuthorityData))]
    public void TestAuthority(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexStringTest($"^{Uris.AuthorityRex}$", TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(NetAuthorityData))]
    public void TestNetAuthority(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexStringTest($"^{Uris.NetAuthorityRex}$", TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(PathAbsData))]
    public void TestPathAbsData(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Path(), TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(GeneralQueryData))]
    public void TestGeneralQueryData(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Query(), TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(KeyValueQueryData))]
    public void TestKeyValueQueryData(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.KvQuery(), TestFileLine, shouldBe, input, captures, allGroups: true);

    // -----

    [Theory]
    [MemberData(nameof(UriData))]
    public void TestUriData(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTestUri(Uris.Uri(), TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(NetUriData))]
    public void TestNetUriData(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTestUri(Uris.NetUri(), TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(FromCopilotUriData))]
    public void TestCopilotUriCases(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Uri(), TestFileLine, shouldBe, input, captures);
}
