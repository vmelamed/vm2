namespace vm2.RegexLibTests;

public partial class UriTests(ITestOutputHelper output) : RegexTests(output)
{
    public static TheoryData<string, bool, string, string> SchemeData = new() {
        { TestLine(), false, "", "" },
        { TestLine(), false, " ", "" },
        { TestLine(), false, "1", "" },
        { TestLine(), false, "1", "" },
        { TestLine(), false, "1a", "" },
        { TestLine(), false, ".", "" },
        { TestLine(), false, "+", "" },
        { TestLine(), false, "-", "" },
        { TestLine(), false, ".a", "" },
        { TestLine(), false, "+a", "" },
        { TestLine(), false, "-a", "" },
        { TestLine(), false, ".1", "" },
        { TestLine(), false, "+2", "" },
        { TestLine(), false, "-3", "" },
        { TestLine(), false, "ж", "" },
        { TestLine(), false, "%41%42", "" },
        { TestLine(), true,  "a", "a" },
        { TestLine(), true,  "abc", "abc" },
        { TestLine(), true,  "a1", "a1" },
        { TestLine(), true,  "abc123", "abc123" },
        { TestLine(), true,  "a1b2c3", "a1b2c3" },
        { TestLine(), true,  "a1.b2+c3-", "a1.b2+c3-" },
        { TestLine(), true,  Uri.UriSchemeFile, Uri.UriSchemeFile },
        { TestLine(), true,  Uri.UriSchemeFtp, Uri.UriSchemeFtp },
        { TestLine(), true,  Uri.UriSchemeFtps, Uri.UriSchemeFtps },
        { TestLine(), true,  Uri.UriSchemeGopher, Uri.UriSchemeGopher },
        { TestLine(), true,  Uri.UriSchemeHttp, Uri.UriSchemeHttp },
        { TestLine(), true,  Uri.UriSchemeHttps, Uri.UriSchemeHttps },
        { TestLine(), true,  Uri.UriSchemeMailto, Uri.UriSchemeMailto },
        { TestLine(), true,  Uri.UriSchemeNetPipe, Uri.UriSchemeNetPipe },
        { TestLine(), true,  Uri.UriSchemeNetTcp, Uri.UriSchemeNetTcp },
        { TestLine(), true,  Uri.UriSchemeNews, Uri.UriSchemeNews },
        { TestLine(), true,  Uri.UriSchemeNntp, Uri.UriSchemeNntp },
        { TestLine(), true,  Uri.UriSchemeSftp, Uri.UriSchemeSftp },
        { TestLine(), true,  Uri.UriSchemeSsh, Uri.UriSchemeSsh },
        { TestLine(), true,  Uri.UriSchemeTelnet, Uri.UriSchemeTelnet },
        { TestLine(), true,  Uri.UriSchemeWs, Uri.UriSchemeWs },
        { TestLine(), true,  Uri.UriSchemeWss, Uri.UriSchemeWss },
    };

    [Theory]
    [MemberData(nameof(SchemeData))]
    public void TestScheme(string TestLine, bool shouldBe, string input, string group)
        => base.RegexTest(Uris.Scheme, TestLine, shouldBe, input, group);

    // -----
}
