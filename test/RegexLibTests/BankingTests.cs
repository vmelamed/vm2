namespace vm2.RegexLibTests;

public partial class BankingTests(ITestOutputHelper output) : RegexTests(output)
{
    [Theory]
    [MemberData(nameof(AbaRoutingNumberRexData))]
    public void TestAbaRoutingNumberRex(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexStringTest(Banking.AbaRoutingNumberRegex, TestLine, shouldBe, input, captures);

    // ------

    [Theory]
    [MemberData(nameof(SwiftCodeData))]
    public void TestSwiftCodeRex(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Banking.SwiftCode, TestLine, shouldBe, input, captures);

    // ------

    [Theory]
    [MemberData(nameof(IbanData))]
    public void TestIbanRex(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Banking.Iban, TestLine, shouldBe, input, captures);

    // ------

    [Theory]
    [MemberData(nameof(IbanIData))]
    public void TestIbanIRex(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Banking.IbanI, TestLine, shouldBe, input, captures);
}
