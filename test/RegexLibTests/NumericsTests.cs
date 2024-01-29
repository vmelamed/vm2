namespace vm2.RegexLibTests;

public class NumericsTests(ITestOutputHelper output) : RegexTests(output)
{
    public static TheoryData<string, bool, string> OctalNumberRexData => new() {
        { TestLine(), false, "" },
        { TestLine(), false, " " },
        { TestLine(), true,  "0" },
        { TestLine(), true,  "12" },
        { TestLine(), true,  "012" },
        { TestLine(), true,  "01234567" },
        { TestLine(), true,  "0123456701234567012345670123456701234567" },
        { TestLine(), false, "012345678" },
        { TestLine(), false, "01234567.12" },
        { TestLine(), false, "09876" },
        { TestLine(), false, "-12" },
        { TestLine(), false, "12ab" },
    };

    [Theory]
    [MemberData(nameof(OctalNumberRexData))]
    public void TestOctalNumberRex(string TestLine, bool shouldBe, string input)
        => base.RegexStringTest(Numerics.OctalNumberRegex, TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> HexadecimalNumberRexData => new() {
        { TestLine(), false, "" },
        { TestLine(), false, " " },
        { TestLine(), true,  "0" },
        { TestLine(), true,  "12" },
        { TestLine(), true,  "012" },
        { TestLine(), true,  "01234567" },
        { TestLine(), true,  "012345678" },
        { TestLine(), true,  "0123456789" },
        { TestLine(), true,  "0123456789abcdef" },
        { TestLine(), true,  "0123456789ABCDEF" },
        { TestLine(), true,  "0123456789aBcDeF0123456789aBcDeF0123456789aBcDeF0123456789aBcDeF" },
        { TestLine(), true,  "12345679890" },
        { TestLine(), false, "123456790.123" },
        { TestLine(), false, "-12" },
        { TestLine(), true,  "12ab" },
        { TestLine(), true,  "12abcdef" },
        { TestLine(), false, "12abcdefg" },
    };

    [Theory]
    [MemberData(nameof(HexadecimalNumberRexData))]
    public void TestHexadecimalNumberRex(string TestLine, bool shouldBe, string input)
        => base.RegexStringTest(Numerics.HexadecimalNumberRegex, TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> NaturalNumberRexData => new() {
        { TestLine(), false, "" },
        { TestLine(), false, " " },
        { TestLine(), false,  "0" },
        { TestLine(), true,  "12" },
        { TestLine(), true,  "012" },
        { TestLine(), true,  "01234567" },
        { TestLine(), true,  "012345678" },
        { TestLine(), true,  "0123456789" },
        { TestLine(), true,  "1234567989012345679890123456798901234567989012345679890" },
        { TestLine(), false, "123456790.123" },
        { TestLine(), false, "-12" },
        { TestLine(), false, "12ab" },
    };

    [Theory]
    [MemberData(nameof(NaturalNumberRexData))]
    public void TestNaturalNumberRex(string TestLine, bool shouldBe, string input)
        => base.RegexStringTest(Numerics.NaturalNumberRegex, TestLine, shouldBe, input);

    public static TheoryData<string, bool, string, string, string> IntegerNumberRexData => new() {
        { TestLine(), false, "", "", "" },
        { TestLine(), false, " ", "", "" },
        { TestLine(), true,  "0", "", "0" },
        { TestLine(), true,  "12", "", "12" },
        { TestLine(), true,  "012", "", "012" },
        { TestLine(), true,  "01234567", "", "01234567" },
        { TestLine(), true,  "012345678", "", "012345678" },
        { TestLine(), true,  "0123456789", "", "0123456789" },
        { TestLine(), true,  "12345679890", "", "12345679890" },
        { TestLine(), false, "123456790.123", "", "" },
        { TestLine(), true,  "-12", "-", "12" },
        { TestLine(), true,  "-123456789012345678901234567890", "-", "123456789012345678901234567890" },
        { TestLine(), true,  "+12", "+", "12" },
        { TestLine(), true,  "+123456789012345678901234567890", "+", "123456789012345678901234567890" },
        { TestLine(), false, "12ab", "", "" },
    };

    [Theory]
    [MemberData(nameof(IntegerNumberRexData))]
    public void TestIntegerNumberRex(string TestLine, bool shouldBe, string input, string sign, string number)
        => base.RegexTest(Numerics.IntegerNumber, TestLine, shouldBe, input, sign, number);

    public static TheoryData<string, bool, string, string, string, string> FractionalNumberRexData => new() {
        { TestLine(), false, "", "", "", "" },
        { TestLine(), false, " ", "", "", "" },
        { TestLine(), false, ".", "", "", "" },
        { TestLine(), false, "+.", "", "", "" },
        { TestLine(), false, "-.", "", "", "" },
        { TestLine(), false, ".-", "", "", "" },
        { TestLine(), true,  "0", "", "0", "" },
        { TestLine(), true,  "12", "", "12", "" },
        { TestLine(), true,  "012", "", "012", "" },
        { TestLine(), true,  "01234567", "", "01234567", "" },
        { TestLine(), true,  "012345678", "", "012345678", "" },
        { TestLine(), true,  "0123456789", "", "0123456789", "" },
        { TestLine(), true,  "12345679890", "", "12345679890", "" },
        { TestLine(), true,  "12345679890.", "", "12345679890", "" },
        { TestLine(), true,  "123456790.123", "", "123456790", "123" },
        { TestLine(), true,  "-123456790.123", "-", "123456790", "123" },
        { TestLine(), true,  "-123456790.", "-", "123456790", "" },
        { TestLine(), true,  "+123456790.123", "+", "123456790", "123" },
        { TestLine(), true,  "+123456790.", "+", "123456790", "" },
        { TestLine(), true,  "-.123", "-", "", "123" },
        { TestLine(), true,  "-123456790.", "-", "123456790", "" },
        { TestLine(), true,  "+.123", "+", "", "123" },
        { TestLine(), true,  "+123456790.", "+", "123456790", "" },
        { TestLine(), true,  "-.123", "-", "", "123" },
        { TestLine(), true,  "-12", "-", "12", "" },
        { TestLine(), true,  "+.123", "+", "", "123" },
        { TestLine(), true,  "+12", "+", "12", "" },
        { TestLine(), false, "-.12.", "-", "", "12" },
        { TestLine(), true, "-123456789012345678901234567890", "-", "123456789012345678901234567890", "" },
        { TestLine(), true, "+123456789012345678901234567890", "+", "123456789012345678901234567890", "" },
        { TestLine(), true, "-123456789012345678901234567890.123456789012345678901234567890", "-", "123456789012345678901234567890", "123456789012345678901234567890" },
        { TestLine(), true, "+123456789012345678901234567890.123456789012345678901234567890", "+", "123456789012345678901234567890", "123456789012345678901234567890" },
        { TestLine(), false, "12ab", "", "", "" },
    };

    [Theory]
    [MemberData(nameof(FractionalNumberRexData))]
    public void TestFractionalNumberRex(string TestLine, bool shouldBe, string input, string sign, string whole, string fraction)
        => base.RegexTest(Numerics.FractionalNumber, TestLine, shouldBe, input, sign, whole, fraction);

    public static TheoryData<string, bool, string, string[]> ScientificNumberRexData => new() {
        { TestLine(), false, "",        ["", "", "", "", "", "", ""] },
        { TestLine(), false, " ",       ["", "", "", "", "", "", ""] },
        { TestLine(), false, ".",       ["", "", "", "", "", "", ""] },
        { TestLine(), false, "+.",      ["", "", "", "", "", "", ""] },
        { TestLine(), false, "-.",      ["", "", "", "", "", "", ""] },
        { TestLine(), false, ".-",      ["", "", "", "", "", "", ""] },
        { TestLine(), true,  "0e0",     ["0", "", "0", "", "0", "", "0"] },
        { TestLine(), true,  "0e123",   ["0", "", "0", "", "123", "", "123"] },
        { TestLine(), true,  "0e-123",  ["0", "", "0", "", "-123", "-", "123"] },
        { TestLine(), true,  "0e+123",  ["0", "", "0", "", "+123", "+", "123"] },
        { TestLine(), true,  "0E0",     ["0", "", "0", "", "0", "", "0"] },
        { TestLine(), true,  "0E123",   ["0", "", "0", "", "123", "", "123"] },
        { TestLine(), true,  "0E-123",  ["0", "", "0", "", "-123", "-", "123"] },
        { TestLine(), true,  "0E+123",  ["0", "", "0", "", "+123", "+", "123"] },
        { TestLine(), true,  "-0E123",  ["-0", "-", "0", "", "123", "", "123"] },
        { TestLine(), true,  "-0E-123", ["-0", "-", "0", "", "-123", "-", "123"] },
        { TestLine(), true,  "-0E+123", ["-0", "-", "0", "", "+123", "+", "123"] },
        { TestLine(), true,  "+0E123",  ["+0", "+", "0", "", "123", "", "123"] },
        { TestLine(), true,  "+0E-123", ["+0", "+", "0", "", "-123", "-", "123"] },
        { TestLine(), true,  "+0E+123", ["+0", "+", "0", "", "+123", "+", "123"] },
        { TestLine(), true,  "0.12E123",   ["0.12", "", "0", "12", "123", "", "123"] },
        { TestLine(), true,  "1E123",   ["1", "", "1", "", "123", "", "123"] },
        { TestLine(), true,  "+1E123",   ["+1", "+", "1", "", "123", "", "123"] },
        { TestLine(), true,  "-1E123",   ["-1", "-", "1", "", "123", "", "123"] },
        { TestLine(), true,  "0.12E-123",  ["0.12", "", "0", "12", "-123", "-", "123"] },
        { TestLine(), true,  "0.12E+123",  ["0.12", "", "0", "12", "+123", "+", "123"] },
        { TestLine(), true,  "-0.23E123",  ["-0.23", "-", "0", "23", "123", "", "123"] },
        { TestLine(), true,  "-0.23E-123", ["-0.23", "-", "0", "23", "-123", "-", "123"] },
        { TestLine(), true,  "-0.23E+123", ["-0.23", "-", "0", "23", "+123", "+", "123"] },
        { TestLine(), true,  "+0.345E123",  ["+0.345", "+", "0", "345", "123", "", "123"] },
        { TestLine(), true,  "+0.345E-123", ["+0.345", "+", "0", "345", "-123", "-", "123"] },
        { TestLine(), true,  "+0.345E+123", ["+0.345", "+", "0", "345", "+123", "+", "123"] },
        { TestLine(), true,  "+0.345E0", ["+0.345", "+", "0", "345", "0", "", "0"] },
        { TestLine(), true,  "+0.345E-0", ["+0.345", "+", "0", "345", "-0", "-", "0"] },
        { TestLine(), true,  "+0.345E+0", ["+0.345", "+", "0", "345", "+0", "+", "0"] },
        { TestLine(), true,  "9.345E", ["9.345", "", "9", "345", "", "", ""] },
        { TestLine(), true,  "+10.345E", ["+10.345", "+", "10", "345", "", "", ""] },
        { TestLine(), true,  "-01.345E", ["-01.345", "-", "01", "345", "", "", ""] },
        { TestLine(), true,  ".345E", [".345", "", "", "345", "", "", ""] },
        { TestLine(), true,  "+10.E", ["+10.", "+", "10", "", "", "", ""] },
        { TestLine(), true,  "-01.E12", ["-01.", "-", "01", "", "12", "", "12"] },
        { TestLine(), false, "+10.345E-", ["", "", "", "", "", "", ""] },
        { TestLine(), false, "+10.345E-12q", ["", "", "", "", "", "", ""] },
        { TestLine(), false, "+10.345E.12", ["", "", "", "", "", "", ""] },
        { TestLine(), false, "+10.345d12", ["", "", "", "", "", "", ""] },
        { TestLine(), false, "+10.345", ["", "", "", "", "", "", ""] },
        { TestLine(), false, "+.e12", ["", "", "", "", "", "", ""] },
        { TestLine(), false, "+e12", ["", "", "", "", "", "", ""] },
        { TestLine(), false, "0", ["", "", "", "", "", "", ""] },
        { TestLine(), false, "e12", ["", "", "", "", "", "", ""] },
    };

    [Theory]
    [MemberData(nameof(ScientificNumberRexData))]
    public void TestScientificNumberRex(string TestLine, bool shouldBe, string input, string[] captures)
        => base.RegexTest(Numerics.ScientificNumber, TestLine, shouldBe, input, captures);

    public static TheoryData<string, bool, string> UuidRexData => new() {
        { TestLine(), false, "" },
        { TestLine(), false, " " },
        { TestLine(), false, "0" },
        { TestLine(), false, "0123abc" },
        { TestLine(), false, "0123456789abcdef0123456789abcdef0" },
        { TestLine(), false, "0123456789abcdef0123456789abcde" },
        { TestLine(), true,  "0123456789abcdef0123456789abcdef" },
        { TestLine(), true,  "01234567-89ab-cdef-0123-456789abcdef" },
        { TestLine(), false, "{01234567-89ab-cdef-0123-456789abcdef0}" },
        { TestLine(), false, "{01234567-89ab-cdef-0123-456789abcde}" },
        { TestLine(), false, "(01234567-89ab-cdef-0123-456789abcdef0)" },
        { TestLine(), false, "(01234567-89ab-cdef-0123-456789abcde)" },
        { TestLine(), true,  "{01234567-89ab-cdef-0123-456789abcdef}" },
        { TestLine(), true,  "(01234567-89ab-cdef-0123-456789abcdef)" },
        { TestLine(), false, "0123ABC" },
        { TestLine(), false, "0123456789ABCDEF0123456789ABCDEF0" },
        { TestLine(), false, "0123456789ABCDEF0123456789ABCDE" },
        { TestLine(), true,  "0123456789ABCDEF0123456789ABCDEF" },
        { TestLine(), true,  "01234567-89AB-CDEF-0123-456789ABCDEF" },
        { TestLine(), false, "{01234567-89AB-CDEF-0123-456789ABCDEF0}" },
        { TestLine(), false, "{01234567-89AB-CDEF-0123-456789ABCDE}" },
        { TestLine(), false, "(01234567-89AB-CDEF-0123-456789ABCDEF0)" },
        { TestLine(), false, "(01234567-89AB-CDEF-0123-456789ABCDE)" },
        { TestLine(), true,  "{01234567-89AB-CDEF-0123-456789ABCDEF}" },
        { TestLine(), true,  "(01234567-89AB-CDEF-0123-456789ABCDEF)" },
        { TestLine(), true,  "{01234567-89ab-CDEF-0123-456789abcdef}" },
        { TestLine(), true,  "(01234567-89ab-CDEF-0123-456789abcdef)" },
    };

    [Theory]
    [MemberData(nameof(UuidRexData))]
    public void TestUuidRex(string TestLine, bool shouldBe, string input)
        => base.RegexStringTest(Numerics.UuidRegex, TestLine, shouldBe, input);
}
