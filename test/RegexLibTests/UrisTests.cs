namespace vm2.RegexLibTests;

public class UriTheoryData : TheoryData<string, bool, string, Captures?>
{
}

public partial class UrisTests(ITestOutputHelper output) : RegexTests(output)
{
    public static UriTheoryData SchemeData = new() {
        { TestLine(), false, "", null },
        { TestLine(), false, " ", null },
        { TestLine(), false, "1", null },
        { TestLine(), false, "1", null },
        { TestLine(), false, "1a", null },
        { TestLine(), false, ".", null },
        { TestLine(), false, "+", null },
        { TestLine(), false, "-", null },
        { TestLine(), false, ".a", null },
        { TestLine(), false, "+a", null },
        { TestLine(), false, "-a", null },
        { TestLine(), false, ".1", null },
        { TestLine(), false, "+2", null },
        { TestLine(), false, "-3", null },
        { TestLine(), false, "ж", null },
        { TestLine(), false, "%41%42", null },
        { TestLine(), true, "a", new() { ["scheme"] = "a" } },
        { TestLine(), true, "abc", new() { ["scheme"] = "abc" } },
        { TestLine(), true, "a1", new() { ["scheme"] = "a1" } },
        { TestLine(), true, "abc123", new() { ["scheme"] = "abc123" } },
        { TestLine(), true, "a1b2c3", new() { ["scheme"] = "a1b2c3" } },
        { TestLine(), true, "a1.b2+c3-", new() { ["scheme"] = "a1.b2+c3-" } },
        { TestLine(), true, Uri.UriSchemeFile, new() { ["scheme"] = Uri.UriSchemeFile } },
        { TestLine(), true, Uri.UriSchemeFtp, new() { ["scheme"] = Uri.UriSchemeFtp } },
        { TestLine(), true, Uri.UriSchemeFtps, new() { ["scheme"] = Uri.UriSchemeFtps } },
        { TestLine(), true, Uri.UriSchemeGopher, new() { ["scheme"] = Uri.UriSchemeGopher } },
        { TestLine(), true, Uri.UriSchemeHttp, new() { ["scheme"] = Uri.UriSchemeHttp } },
        { TestLine(), true, Uri.UriSchemeHttps, new() { ["scheme"] = Uri.UriSchemeHttps } },
        { TestLine(), true, Uri.UriSchemeMailto, new() { ["scheme"] = Uri.UriSchemeMailto } },
        { TestLine(), true, Uri.UriSchemeNetPipe, new() { ["scheme"] = Uri.UriSchemeNetPipe } },
        { TestLine(), true, Uri.UriSchemeNetTcp, new() { ["scheme"] = Uri.UriSchemeNetTcp } },
        { TestLine(), true, Uri.UriSchemeNews, new() { ["scheme"] = Uri.UriSchemeNews } },
        { TestLine(), true, Uri.UriSchemeNntp, new() { ["scheme"] = Uri.UriSchemeNntp } },
        { TestLine(), true, Uri.UriSchemeSftp, new() { ["scheme"] = Uri.UriSchemeSftp } },
        { TestLine(), true, Uri.UriSchemeSsh, new() { ["scheme"] = Uri.UriSchemeSsh } },
        { TestLine(), true, Uri.UriSchemeTelnet, new() { ["scheme"] = Uri.UriSchemeTelnet } },
        { TestLine(), true, Uri.UriSchemeWs, new() { ["scheme"] = Uri.UriSchemeWs } },
        { TestLine(), true, Uri.UriSchemeWss, new() { ["scheme"] = Uri.UriSchemeWss } },
    };

    [Theory]
    [MemberData(nameof(SchemeData))]
    public void TestScheme(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Scheme, TestLine, shouldBe, input, captures);

    // -----

    public static UriTheoryData HostData = new() {
        { TestLine(), false, "", null },
        { TestLine(), false, " ", null },
        { TestLine("DnsName"), true, "maria.vtmelamed.com", new()
                                                        {
                                                            ["host"] = "maria.vtmelamed.com",
                                                            ["ipDnsName"] = "maria.vtmelamed.com",
                                                        } },
        { TestLine("Incomplete IPv4"), true, "1.2.3", new()
                                                        {
                                                            ["host"] = "1.2.3",
                                                            ["ipGenName"] = "1.2.3",
                                                        } },
        { TestLine("Complete IPv4"), true, "1.2.3.4", new()
                                                        {
                                                            ["host"] = "1.2.3.4",
                                                            ["ipv4"] = "1.2.3.4",
                                                        } },
        { TestLine("Complete unbracketed IPv6"), false, "1:2:3::4", null },
        { TestLine("Complete IPv6"), true, "[1:2:3::4]", new()
                                                        {
                                                            ["host"] = "[1:2:3::4]",
                                                            ["ipv6"] = "1:2:3::4",
                                                        } },
        { TestLine("Complete IPvF"), true, "[v1a.skiledh.srethg.23546.]", new()
                                                        {
                                                            ["host"] = "[v1a.skiledh.srethg.23546.]",
                                                            ["ipvF"] = "v1a.skiledh.srethg.23546.",
                                                        } },
        { TestLine("Complete unbracketed IPvF"), true, "v1a.skiledh.srethg.23546.", new()
                                                        {
                                                            ["host"] = "v1a.skiledh.srethg.23546.",
                                                            ["ipGenName"] = "v1a.skiledh.srethg.23546.",
                                                        } },
        { TestLine("General name in Unicode"), false, "дир.бг", null },
        { TestLine("General name in percent URL encoded"), true, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3", new()
                                                        {
                                                            ["host"] = "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
                                                            ["ipGenName"] = "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3",
                                                        } },
    };

    [Theory]
    [MemberData(nameof(HostData))]
    public void TestHost(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Host, TestLine, shouldBe, input, captures);

    // -----

    public static UriTheoryData NetHostData = new() {
        { TestLine(), false, "", null },
        { TestLine(), false, " ", null },
        { TestLine("DnsName"), true, "maria.vtmelamed.com", new()
                                                        {
                                                            ["host"] = "maria.vtmelamed.com",
                                                            ["ipDnsName"] = "maria.vtmelamed.com",
                                                        } },
        { TestLine("Incomplete IPv4"), false, "1.2.3", null },
        { TestLine("Complete IPv4"), true, "1.2.3.4", new()
                                                        {
                                                            ["host"] = "1.2.3.4",
                                                            ["ipv4"] = "1.2.3.4",
                                                        } },
        { TestLine("Complete unbracketed IPv6"), false, "1:2:3::4", null },
        { TestLine("Complete IPv6"), true, "[1:2:3::4]", new()
                                                        {
                                                            ["host"] = "[1:2:3::4]",
                                                            ["ipv6"] = "1:2:3::4",
                                                        } },
        { TestLine("Complete IPvF"), true, "[v1a.skiledh.srethg.23546.]", new()
                                                        {
                                                            ["host"] = "[v1a.skiledh.srethg.23546.]",
                                                            ["ipvF"] = "v1a.skiledh.srethg.23546.",
                                                        } },
        { TestLine("Complete unbracketed IPvF"), false, "v1a.skiledh.srethg.23546.", null },
        { TestLine("General name in Unicode"), false, "дир.бг", null },
        { TestLine("General name in percent URL encoded"), false, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3", null },
    };

    [Theory]
    [MemberData(nameof(NetHostData))]
    public void TestNetHost(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.Host, TestLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(AddressData))]
    public void TestAddress(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Address, TestLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(NetAddressData))]
    public void TestNetAddress(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.Address, TestLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(AuthorityData))]
    public void TestAuthority(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexStringTest($"^{Uris.AuthorityRex}$", TestLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(NetAuthorityData))]
    public void TestNetAuthority(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexStringTest($"^{Uris.NetAuthorityRex}$", TestLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(PathAbsData))]
    public void TestPathAbsData(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Path, TestLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(GeneralQueryData))]
    public void TestGeneralQueryData(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.Query, TestLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(KeyValueQueryData))]
    public void TestKeyValueQueryData(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Uris.KvQuery, TestLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(UriData))]
    public void TestUriData(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTestUri(Uris.Uri, TestLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(NetUriData))]
    public void TestNetUriData(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTestUri(Uris.NetUri, TestLine, shouldBe, input, captures);
}
