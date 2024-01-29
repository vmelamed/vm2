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

    public static TheoryData<string, bool, string, string> DnsNameData = new() {
        { TestLine(), false, "", "" },
        { TestLine(), false, " ", "" },
        { TestLine(), false, "ab12-34", "" },
        { TestLine(), false, "ab12-34", "" },
    };

    [Theory]
    [MemberData(nameof(DnsNameData))]
    public void TestDnsName(string TestLine, bool shouldBe, string input, string group)
        => base.RegexTest(Net.DnsName, TestLine, shouldBe, input, group);
}
