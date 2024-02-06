namespace vm2.RegexLibTests;

public partial class NetTests(ITestOutputHelper output) : RegexTests(output)
{
    [Theory]
    [MemberData(nameof(Ipv4AddressData))]
    public void TestIpv4Address(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.Ipv4Address, TestLine, shouldBe, input, captures);

    // -----

    [Theory]
    [MemberData(nameof(Ipv6AddressData))]
    public void TestIpv6Address(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.Ipv6Address, TestLine, shouldBe, input, captures);

    // -----

    public static TheoryData<string, bool, string, Captures?> IpvFutureAddressData = new() {
        { TestLine(), false, "", null },
        { TestLine(), false, " ", null },
        { TestLine(), false, "1.1.1.1", null },
        { TestLine(), false, "2001:0000:1234:0000:0000:C1C0:ABCD:0876", null },
        { TestLine(), false, "vg.1.1.ab.hex", null },
        { TestLine(), false, "a.1.1.ab.hex", null },
        { TestLine(), false, "va.1.1.a b.hex", null },
        { TestLine(), true, "v3.1.1.ab.hex", new() { ["ipvF"] = "v3.1.1.ab.hex" }  },
        { TestLine(), true, "va.1.1.a-b.hex", new() { ["ipvF"] = "va.1.1.a-b.hex" }  },
        { TestLine(), false, "va.1.1.a-#b.hex", null },
    };

    [Theory]
    [MemberData(nameof(IpvFutureAddressData))]
    public void TestIpvFutureAddress(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.IpvFutureAddress, TestLine, shouldBe, input, captures);

    // -----

    public static TheoryData<string, bool, string> DnsLabelData = new() {
        { TestLine(), false, "" },
        { TestLine(), false, " " },
        { TestLine(), true,  "abc" },
        { TestLine(), false, " abc" },
        { TestLine(), false, "abc " },
        { TestLine(), false, " abc " },
        { TestLine(), false, "1abc" },
        { TestLine(), false, "-abc" },
        { TestLine(), false, "@abc" },
        { TestLine(), false, "abc-" },
        { TestLine(), false, "abc@" },
        { TestLine(), true,  "ab12-x" },
        { TestLine(), true,  "ab12-9" },
        { TestLine(), false, "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789z" },
        { TestLine(), true,  "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789" },
    };

    [Theory]
    [MemberData(nameof(DnsLabelData))]
    public void TestDnsLabel(string TestLine, bool shouldBe, string input)
        => base.RegexStringTest($"^{Net.DnsLabelRex}$", TestLine, shouldBe, input);

    // -----

    public static TheoryData<string, bool, string, Captures?> DnsNameData = new() {
        { TestLine("empty"),
                      false, "", null },
        { TestLine("one or more spaces"),
                      false, " ", null },
        { TestLine("good short one"),
                      true,  "abc", new() { ["ipDnsName"] = "abc" } },
        { TestLine("one or more spaces"),
                      false, " abc", null },
        { TestLine("one or more spaces"),
                      false, "abc ", null },
        { TestLine("one or more spaces"),
                      false, " abc ", null },
        { TestLine("cannot start with a digit"),
                      false, "1abc", null },
        { TestLine("cannot start with a dash"),
                      false, "-abc", null },
        { TestLine("not allowed character - @"),
                      false, "@abc", null },
        { TestLine("cannot end with a dash"),
                      false, "abc-", null },
        { TestLine("not allowed character - @"),
                      false, "abc@", null },
        { TestLine("good short single label"),
                      true,  "ab12-x", new() { ["ipDnsName"] = "ab12-x" } },
        { TestLine("good short single label"),
                      true,  "ab12-9", new() { ["ipDnsName"] = "ab12-9" } },
        { TestLine("too long"),
                      false, "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789z", null },
        { TestLine("not allowed character - $"),
                      false, "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJK$LZXCVBNM012345678", null },
        { TestLine("good single label, max label length"),
                      true,  "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789", new() { ["ipDnsName"] = "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789" } },
        { TestLine("good short multiple labels"),
                      true,  "test.vm.com", new() { ["ipDnsName"] = "test.vm.com" } },
        { TestLine("good max number of labels"),
          true,
          "a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a",
          new() { ["ipDnsName"] = "a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a" }
        },
        { TestLine("max number of labels + 1"),
          false,
          "a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a",
          null
        },
        { TestLine(), true,  "dir.bg", new() { ["ipDnsName"] = "dir.bg" } },
        { TestLine("not good - unicode letters"),
                      false, "дир.бг", null },
        { TestLine("not good - percent URL-like coded"),
                      false, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3", null },
    };

    [Theory]
    [MemberData(nameof(DnsNameData))]
    public void TestDnsName(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.DnsName, TestLine, shouldBe, input, captures);

    // -----

    public static TheoryData<string, bool, string, Captures?> PortData = new() {
        { TestLine("empty"),
                      false, "", null },
        { TestLine("space"),
                      false, " ", null },
        { TestLine("spaces"),
                      false, "  ", null },
        { TestLine("has space(s)"),
                      false, "55 ", null },
        { TestLine("has space(s)"),
                      false, " 55", null },
        { TestLine("has space(s)"),
                      false, " 55 ", null },
        { TestLine("has space(s)"),
                      false, " 5 5 ", null },
        { TestLine("has letters"),
                      false, " 5 5 ", null },
        { TestLine("negative"),
                      false, "-1", null },
        { TestLine("zero"),
                      true, "0", new() { ["port"] = "0" }  },
        { TestLine("ftp"),
                      true, "21", new() { ["port"] = "21" }  },
        { TestLine("http"),
                      true, "80", new() { ["port"] = "80" }  },
        { TestLine("https"),
                      true, "443", new() { ["port"] = "443" }  },
        { TestLine("65535"),
                      true, "65535", new() { ["port"] = "65535" }  },
        { TestLine("65536"),
                      false, "65536", null },
        { TestLine("65540"),
                      false, "65540", null },
        { TestLine("65600"),
                      false, "65600", null },
        { TestLine("66600"),
                      false, "66600", null },
        { TestLine("76600"),
                      false, "76600", null },
        { TestLine("99999999"),
                      false, "99999999", null },
    };

    [Theory]
    [MemberData(nameof(PortData))]
    public void TestPort(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Net.Port, TestLine, shouldBe, input, captures);
}
