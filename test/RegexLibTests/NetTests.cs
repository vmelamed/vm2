namespace vm2.RegexLib.Tests;

public partial class NetTests(
    RegexLibTestsFixture fixture,
    ITestOutputHelper output) : RegexTests(fixture, output)
{
    [Theory]
    [MemberData(nameof(Ipv4AddressData))]
    public void TestIpv4Address(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.Ipv4Address(), TestFileLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(Ipv6AddressData))]
    public void TestIpv6Address(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.Ipv6Address(), TestFileLine, shouldBe, input, captures);

    // -----

    public static TheoryData<string, bool, string, Captures?> IpvFutureAddressData = new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), false, "1.1.1.1", null },
        { TestFileLine(), false, "2001:0000:1234:0000:0000:C1C0:ABCD:0876", null },
        { TestFileLine(), false, "vg.1.1.ab.hex", null },
        { TestFileLine(), false, "a.1.1.ab.hex", null },
        { TestFileLine(), false, "va.1.1.a b.hex", null },
        { TestFileLine(), true, "v3.1.1.ab.hex", new() { ["ipvF"] = "v3.1.1.ab.hex" }  },
        { TestFileLine(), true, "va.1.1.a-b.hex", new() { ["ipvF"] = "va.1.1.a-b.hex" }  },
        { TestFileLine(), false, "va.1.1.a-#b.hex", null },
    };

    [Theory]
    [MemberData(nameof(IpvFutureAddressData))]
    public void TestIpvFutureAddress(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.IpvFutureAddress(), TestFileLine, shouldBe, input, captures);

    // -----

    public static TheoryData<string, bool, string> DnsLabelData = new() {
        { TestFileLine(), false, "" },
        { TestFileLine(), false, " " },
        { TestFileLine(), true,  "abc" },
        { TestFileLine(), false, " abc" },
        { TestFileLine(), false, "abc " },
        { TestFileLine(), false, " abc " },
        { TestFileLine(), false, "1abc" },
        { TestFileLine(), false, "-abc" },
        { TestFileLine(), false, "@abc" },
        { TestFileLine(), false, "abc-" },
        { TestFileLine(), false, "abc@" },
        { TestFileLine(), true,  "ab12-x" },
        { TestFileLine(), true,  "ab12-9" },
        { TestFileLine(), false, "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789z" },
        { TestFileLine(), true,  "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789" },
    };

    [Theory]
    [MemberData(nameof(DnsLabelData))]
    public void TestDnsLabel(string TestFileLine, bool shouldBe, string input)
        => base.RegexStringTest($"^{Net.DnsLabelRex}$", TestFileLine, shouldBe, input);

    // -----

    public static TheoryData<string, bool, string, Captures?> DnsNameData = new() {
        { TestFileLine("empty"),
                      false, "", null },
        { TestFileLine("one or more spaces"),
                      false, " ", null },
        { TestFileLine("good short one"),
                      true,  "abc", new() { ["ipDnsName"] = "abc" } },
        { TestFileLine("one or more spaces"),
                      false, " abc", null },
        { TestFileLine("one or more spaces"),
                      false, "abc ", null },
        { TestFileLine("one or more spaces"),
                      false, " abc ", null },
        { TestFileLine("cannot start with a digit"),
                      false, "1abc", null },
        { TestFileLine("cannot start with a dash"),
                      false, "-abc", null },
        { TestFileLine("not allowed character - @"),
                      false, "@abc", null },
        { TestFileLine("cannot end with a dash"),
                      false, "abc-", null },
        { TestFileLine("not allowed character - @"),
                      false, "abc@", null },
        { TestFileLine("good short single label"),
                      true,  "ab12-x", new() { ["ipDnsName"] = "ab12-x" } },
        { TestFileLine("good short single label"),
                      true,  "ab12-9", new() { ["ipDnsName"] = "ab12-9" } },
        { TestFileLine("too long"),
                      false, "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789z", null },
        { TestFileLine("not allowed character - $"),
                      false, "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJK$LZXCVBNM012345678", null },
        { TestFileLine("good single label, max label length"),
                      true,  "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789", new() { ["ipDnsName"] = "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789" } },
        { TestFileLine("good short multiple labels"),
                      true,  "test.vm.com", new() { ["ipDnsName"] = "test.vm.com" } },
        { TestFileLine("empty first label"),
                      false,  ".vm.com", null },
        { TestFileLine("empty first labels"),
                      false,  ".com", null },
        { TestFileLine("good max number of labels"),
          true,
          "a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a",
          new() { ["ipDnsName"] = "a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a" }
        },
        { TestFileLine("max number of labels + 1"),
          false,
          "a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a",
          null
        },
        { TestFileLine(), true,  "dir.bg", new() { ["ipDnsName"] = "dir.bg" } },
        { TestFileLine("not good - unicode letters"),
                      false, "дир.бг", null },
        { TestFileLine("not good - percent URL-like coded"),
                      false, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3", null },
        { TestFileLine("punycode domain"), true, "xn--d1acufc.xn--p1ai", new() { ["ipDnsName"] = "xn--d1acufc.xn--p1ai" } },
    };

    [Theory]
    [MemberData(nameof(DnsNameData))]
    public void TestDnsName(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.DnsName(), TestFileLine, shouldBe, input, captures);

    // -----

    public static TheoryData<string, bool, string, Captures?> PortData = new() {
        { TestFileLine("empty"),
                      false, "", null },
        { TestFileLine("space"),
                      false, " ", null },
        { TestFileLine("spaces"),
                      false, "  ", null },
        { TestFileLine("has space(s)"),
                      false, "55 ", null },
        { TestFileLine("has space(s)"),
                      false, " 55", null },
        { TestFileLine("has space(s)"),
                      false, " 55 ", null },
        { TestFileLine("has space(s)"),
                      false, " 5 5 ", null },
        { TestFileLine("has letters"),
                      false, " 5 5 ", null },
        { TestFileLine("negative"),
                      false, "-1", null },
        { TestFileLine("zero"),
                      true, "0", new() { ["port"] = "0" }  },
        { TestFileLine("ftp"),
                      true, "21", new() { ["port"] = "21" }  },
        { TestFileLine("http"),
                      true, "80", new() { ["port"] = "80" }  },
        { TestFileLine("https"),
                      true, "443", new() { ["port"] = "443" }  },
        { TestFileLine("65535"),
                      true, "65535", new() { ["port"] = "65535" }  },
        { TestFileLine("65536"),
                      false, "65536", null },
        { TestFileLine("65540"),
                      false, "65540", null },
        { TestFileLine("65600"),
                      false, "65600", null },
        { TestFileLine("66600"),
                      false, "66600", null },
        { TestFileLine("76600"),
                      false, "76600", null },
        { TestFileLine("99999999"),
                      false, "99999999", null },
        { TestFileLine("negative one"), false, "-1", null },
        { TestFileLine("just above max"), false, "65536", null },
    };

    [Theory]
    [MemberData(nameof(PortData))]
    public void TestPort(string TestFileLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.Port(), TestFileLine, shouldBe, input, captures);
}
