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
        { TestLine(), false, "", "" },
        { TestLine(), false, " ", "" },
        { TestLine(), true,  "abc", "abc" },
        { TestLine(), false, " abc", "" },
        { TestLine(), false, "abc ", "" },
        { TestLine(), false, " abc ", "" },
        { TestLine(), false, "1abc", "" },
        { TestLine(), false, "-abc", "" },
        { TestLine(), false, "@abc", "" },
        { TestLine(), false, "abc-", "" },
        { TestLine(), false, "abc@", "" },
        { TestLine(), true,  "ab12-x", "ab12-x" },
        { TestLine(), true,  "ab12-9", "ab12-9" },
        { TestLine(), false, "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789z", "" },
        { TestLine(), true,  "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789", "qwertyuiopasdfghjklzxcvbnm-QWERTYUIOPASDFGHJKLZXCVBNM0123456789" },
        { TestLine(), true,  "test.vm.com", "test.vm.com"},
        { TestLine(), true,  "dir.bg", "dir.bg"},
        { TestLine(), true,  "a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a", "a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a"},
        { TestLine(), false, "a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a.a", ""},
        { TestLine(), false, "%D0%B4%D0%B8%D1%80.%D0%B1%D0%B3%", ""},
    };

    [Theory]
    [MemberData(nameof(DnsNameData))]
    public void TestDnsName(string TestLine, bool shouldBe, string input, string name)
        => base.RegexTest(Net.DnsName, TestLine, shouldBe, input, name);
}
