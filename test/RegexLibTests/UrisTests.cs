namespace vm2.RegexLibTests;

public class UriTheoryData : TheoryData<string, bool, string, Captures?>
{
}

public partial class UrisTests(ITestOutputHelper output) : RegexTests(output)
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
        => base.RegexTest(Uris.Scheme, TestFileLine, shouldBe, input, captures);

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
                                                            ["ipGenName"] = "1.2.3",
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
                                                            ["ipGenName"] = "v1a.skiledh.srethg.23546.",
                                                        } },
        { TestFileLine("General name in Unicode"), false, "дир.бг", null },
        { TestFileLine("General name in percent URL encoded"), true, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3", new()
                                                        {
                                                            ["host"] = "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
                                                            ["ipGenName"] = "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
                                                        } },
    };

    [Theory]
    [MemberData(nameof(HostData))]
    public void TestHost(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Host, TestFileLine, shouldBe, input, captures);

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

    [Theory]
    [MemberData(nameof(NetHostData))]
    public void TestNetHost(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.Host, TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(AddressData))]
    public void TestAddress(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Endpoint, TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(NetAddressData))]
    public void TestNetAddress(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.Endpoint, TestFileLine, shouldBe, input, captures);

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
        => base.RegexTest(Uris.Path, TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(GeneralQueryData))]
    public void TestGeneralQueryData(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Query, TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(KeyValueQueryData))]
    public void TestKeyValueQueryData(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.KvQuery, TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(UriData))]
    public void TestUriData(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTestUri(Uris.Uri, TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(NetUriData))]
    public void TestNetUriData(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTestUri(Uris.NetUri, TestFileLine, shouldBe, input, captures);
}
