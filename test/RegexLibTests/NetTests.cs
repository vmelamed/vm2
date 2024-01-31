namespace vm2.RegexLibTests;

public partial class NetTests(ITestOutputHelper output) : RegexTests(output)
{
    [Theory]
    [MemberData(nameof(Ipv4AddressData))]
    public void TestIpv4Address(string TestLine, bool shouldBe, string input, string group)
        => base.RegexTest(Net.Ipv4Address, TestLine, shouldBe, input, group);

    // -----

    [Theory]
    [MemberData(nameof(Ipv6AddressData))]
    public void TestIpv6Address(string TestLine, bool shouldBe, string input, string group)
        => base.RegexTest(Net.Ipv6Address, TestLine, shouldBe, input, group);

    // -----

    public static TheoryData<string, bool, string, string> IpvFutureAddressData = new() {
        { TestLine(), false, "", "" },
        { TestLine(), false, " ", "" },
        { TestLine(), false, "1.1.1.1", "" },
        { TestLine(), false, "2001:0000:1234:0000:0000:C1C0:ABCD:0876", "" },
        { TestLine(), false, "vg.1.1.ab.hex", "" },
        { TestLine(), false, "a.1.1.ab.hex", "" },
        { TestLine(), false, "va.1.1.a b.hex", "" },
        { TestLine(), true,  "v3.1.1.ab.hex", "v3.1.1.ab.hex" },
        { TestLine(), true,  "va.1.1.a-b.hex", "va.1.1.a-b.hex" },
        { TestLine(), false, "va.1.1.a-#b.hex", "" },
    };

    [Theory]
    [MemberData(nameof(IpvFutureAddressData))]
    public void TestIpvFutureAddress(string TestLine, bool shouldBe, string input, string group)
        => base.RegexTest(Net.IpvFutureAddress, TestLine, shouldBe, input, group);

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

    public static TheoryData<string, bool, string, string> DnsNameData = new() {
        { TestLine("empty"),
                      false, "", "" },
        { TestLine("one or more spaces"),
                      false, " ", "" },
        { TestLine("good short one"),
                      true,  "abc", "abc" },
        { TestLine("one or more spaces"),
                      false, " abc", "" },
        { TestLine("one or more spaces"),
                      false, "abc ", "" },
        { TestLine("one or more spaces"),
                      false, " abc ", "" },
        { TestLine("cannot start with a digit"),
                      false, "1abc", "" },
        { TestLine("cannot start with a dash"),
                      false, "-abc", "" },
        { TestLine("not allowed character - @"),
                      false, "@abc", "" },
        { TestLine("cannot end with a dash"),
                      false, "abc-", "" },
        { TestLine("not allowed character - @"),
                      false, "abc@", "" },
        { TestLine("good short single label"),
                      true,  "ab12-x", "ab12-x" },
        { TestLine("good short single label"),
                      true,  "ab12-9", "ab12-9" },
        { TestLine("too long"),
                      false, "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789z", "" },
        { TestLine("not allowed character - $"),
                      false, "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJK$LZXCVBNM012345678", "" },
        { TestLine("good single label, max label length"),
                      true,  "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789", "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789" },
        { TestLine("good short multiple labels"),
                      true,  "test.vm.com", "test.vm.com"},
        { TestLine("good max number of labels"),
                      true,  "a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a", "a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a"},
        { TestLine("max number of labels + 1"),
                      false, "a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a", ""},
        { TestLine(), true,  "dir.bg", "dir.bg"},
        { TestLine("not good - unicode letters"),
                      false,  "дир.бг", ""},
        { TestLine("not good - percent URL-like coded"),
                      false, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3%", ""},
    };

    [Theory]
    [MemberData(nameof(DnsNameData))]
    public void TestDnsName(string TestLine, bool shouldBe, string input, string name)
        => base.RegexTest(Net.DnsName, TestLine, shouldBe, input, name);

    // -----

    public static TheoryData<string, bool, string, string> PortData = new() {
        { TestLine("empty"),
                      false, "", "" },
        { TestLine("space"),
                      false, " ", "" },
        { TestLine("spaces"),
                      false, "  ", "" },
        { TestLine("has space(s)"),
                      false, "55 ", "" },
        { TestLine("has space(s)"),
                      false, " 55", "" },
        { TestLine("has space(s)"),
                      false, " 55 ", "" },
        { TestLine("has space(s)"),
                      false, " 5 5 ", "" },
        { TestLine("has letters"),
                      false, " 5 5 ", "" },
        { TestLine("zero"),
                      true, "0", "0" },
        { TestLine("negative"),
                      false, "-1", "" },
        { TestLine("65535"),
                      true, "65535", "65535" },
        { TestLine("65536"),
                      false, "65536", "" },
        { TestLine("65540"),
                      false, "65540", "" },
        { TestLine("65600"),
                      false, "65600", "" },
        { TestLine("66600"),
                      false, "66600", "" },
        { TestLine("76600"),
                      false, "76600", "" },
        { TestLine("99999999"),
                      false, "99999999", "" },
    };

    [Theory]
    [MemberData(nameof(PortData))]
    public void TestPort(string TestLine, bool shouldBe, string input, string port)
        => base.RegexTest(Net.Port, TestLine, shouldBe, input, port);
}
