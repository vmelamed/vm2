namespace vm2.RegexLibTests;

public partial class BankingTests(
    RegexLibTestsFixture fixture,
    ITestOutputHelper output) : RegexTests(fixture, output)
{
    [Theory]
    [MemberData(nameof(AbaRoutingNumberRexData))]
    public void TestAbaRoutingNumberRex(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Banking.AbaRoutingNumber(), TestLine, shouldBe, input, captures);

    [Theory]
    [MemberData(nameof(AbaRoutingNumberEdgeData))]
    public void TestAbaRoutingNumber_Edge(string TestLine, bool shouldBe, string input)
        => base.RegexTest(Banking.AbaRoutingNumber(), TestLine, shouldBe, input);

    // ------

    [Theory]
    [MemberData(nameof(SwiftCodeData))]
    [MemberData(nameof(SwiftCodeEdgeData))]
    public void TestSwiftCodeRex(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Banking.SwiftCode(), TestLine, shouldBe, input, captures);

    // ------

    [Theory]
    [MemberData(nameof(IbanData))]
    public void TestIbanRex(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Banking.Iban(), TestLine, shouldBe, input, captures);

    // ------

    [Theory]
    [MemberData(nameof(IbanIData))]
    public void TestIbanIRex(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Banking.IbanI(), TestLine, shouldBe, input, captures);
}
