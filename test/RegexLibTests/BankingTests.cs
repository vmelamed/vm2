namespace vm2.RegexLibTests;

public class BankingTests(ITestOutputHelper output) : RegexTests(output)
{
    public static TheoryData<string, bool, string> AbaRoutingNumberRexData => new() {
        { TestLine(), false, "" },
        { TestLine(), false, " " },
        { TestLine(), false,  "0" },
        { TestLine(), false,  "12" },
        { TestLine(), false,  "012" },
        { TestLine(), false,  "01234567" },
        { TestLine(), false,  "0123456701" },
        { TestLine(), false,  "0123456701234567012345670123456701234567" },
        { TestLine(), false,  "01a34b678" },
        { TestLine(), false,  "-012345678" },
        { TestLine(), false,  "-01234567" },
        { TestLine(), true,   "012345678" },
    };

    [Theory]
    [MemberData(nameof(AbaRoutingNumberRexData))]
    public void TestAbaRoutingNumberRex(string TestLine, bool shouldBe, string input)
        => base.RegexStringTest(Banking.AbaRoutingNumberRegex, TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> SwiftCodeData => new() {
        { TestLine(), false, "" },
        { TestLine(), false, " " },
        { TestLine(), false,  "0" },
        { TestLine(), false,  "12" },
        { TestLine(), false,  "012" },
        { TestLine(), false,  "01234567" },
        { TestLine(), false,  "0123456701" },
        { TestLine(), false,  "0123456701234567012345670123456701234567" },
        { TestLine(), false,  "01a34b678" },
        { TestLine(), false,  "-012345678" },
        { TestLine(), false,  "-01234567" },
        { TestLine(), false,   "012345678" },
        { TestLine(), false,  "BANK" },
        { TestLine(), false,  "BANKUS" },
        { TestLine(), false,  "BANkUSNJ" },
        { TestLine(), false,  "BANKuSNJ" },
        { TestLine(), false,  "BANKUSnJ" },
        { TestLine(), false,  "BANKUSNJaBC" },
        { TestLine(), false,  "bankusnjab3" },
        { TestLine(), false,  "BANKUSNJABCDEF" },
        { TestLine(), true,   "BANKUSNJABC" },
        { TestLine(), true,   "BANKUSNJAB1" },
        { TestLine(), true,   "BANKUSNJ321" },
    };

    [Theory]
    [MemberData(nameof(SwiftCodeData))]
    public void TestSwiftCodeRex(string TestLine, bool shouldBe, string input)
        => base.RegexTest(Banking.SwiftCode, TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> IbanData => new() {
        { TestLine(), false, "" },
        { TestLine(), false, " " },
        { TestLine(), false,  "0" },
        { TestLine(), false,  "12" },
        { TestLine(), false,  "012" },
        { TestLine(), false,  "XY12 134 ABCD WXYZ A4B5 2222 123" },
        { TestLine(), false,  "XY12 12345 ABCD WXYZ A4B5 2222 123" },
        { TestLine(), false,  "XY12 1@%4 ABCD WXYZ A4B5 2222 123" },
        { TestLine(), false,  "12XY 1234 ABCD WXYZ A4B5 2222 123" },
        { TestLine(), false,  "XY12 1234 ABCD WXY" },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ" },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ A4B5 1" },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ A4B5 12" },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ A4B5 2222 123" },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ A4B5 2222123" },
        { TestLine(), true,   "XY121234ABCDWXYZA4B52222123" },
    };

    [Theory]
    [MemberData(nameof(IbanData))]
    public void TestIbanRex(string TestLine, bool shouldBe, string input)
        => base.RegexTest(Banking.Iban, TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> IbanIData => new() {
        { TestLine(), false, "" },
        { TestLine(), false, " " },
        { TestLine(), false,  "0" },
        { TestLine(), false,  "12" },
        { TestLine(), false,  "012" },
        { TestLine(), false,  "12XY 1234 ABCD WXYZ A4B5 2222 123" },
        { TestLine(), false,  "12XY 1234 ABCD wxyz A4B5 2222 123" },
        { TestLine(), false,  "XY12 1234 ABCD WXY" },
        { TestLine(), false,  "xy12 1234 ABCD WXY" },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ" },
        { TestLine(), true,   "xy12 1234 ABCD WXYZ" },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ A4B5 1" },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ A4B5 2222123" },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ a4b5 2222123" },
        { TestLine(), true,   "XY121234ABCDWXYZA4B52222123" },
        { TestLine(), true,   "xy121234abcdwxyza4b52222123" },
    };

    [Theory]
    [MemberData(nameof(IbanIData))]
    public void TestIbanIRex(string TestLine, bool shouldBe, string input)
        => base.RegexTest(Banking.IbanI, TestLine, shouldBe, input);
}
