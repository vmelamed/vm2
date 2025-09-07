namespace vm2.RegexLib.Tests;

public class NumericalTests(
    RegexLibTestsFixture fixture,
    ITestOutputHelper output) : RegexTests(fixture, output)
{
    public static TheoryData<string, bool, string> OctalNumberRexData => new() {
        { TestFileLine(), false, "" },
        { TestFileLine(), false, " " },
        { TestFileLine(), true,  "0" },
        { TestFileLine(), true,  "12" },
        { TestFileLine(), true,  "012" },
        { TestFileLine(), true,  "01234567" },
        { TestFileLine(), true,  "0123456701234567012345670123456701234567" },
        { TestFileLine(), false, "012345678" },
        { TestFileLine(), false, "01234567.12" },
        { TestFileLine(), false, "09876" },
        { TestFileLine(), false, "-12" },
        { TestFileLine(), false, "12ab" },
    };

    [Theory]
    [MemberData(nameof(OctalNumberRexData))]
    public void TestOctalNumberRex(string TestLine, bool shouldBe, string input)
        => base.RegexTest(Numerical.OctalNumber(), TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> HexadecimalNumberRexData => new() {
        { TestFileLine(), false, "" },
        { TestFileLine(), false, " " },
        { TestFileLine(), true,  "0" },
        { TestFileLine(), true,  "12" },
        { TestFileLine(), true,  "012" },
        { TestFileLine(), true,  "01234567" },
        { TestFileLine(), true,  "012345678" },
        { TestFileLine(), true,  "0123456789" },
        { TestFileLine(), true,  "0123456789abcdef" },
        { TestFileLine(), true,  "0123456789ABCDEF" },
        { TestFileLine(), true,  "0123456789aBcDeF0123456789aBcDeF0123456789aBcDeF0123456789aBcDeF" },
        { TestFileLine(), true,  "12345679890" },
        { TestFileLine(), false, "123456790.123" },
        { TestFileLine(), false, "-12" },
        { TestFileLine(), true,  "12ab" },
        { TestFileLine(), true,  "12abcdef" },
        { TestFileLine(), false, "12abcdefg" },
    };

    [Theory]
    [MemberData(nameof(HexadecimalNumberRexData))]
    public void TestHexadecimalNumberRex(string TestLine, bool shouldBe, string input)
        => base.RegexTest(Numerical.HexadecimalNumber(), TestLine, shouldBe, input);

    public static TheoryData<string, bool, string> NaturalNumberRexData => new() {
        { TestFileLine(), false, "" },
        { TestFileLine(), false, " " },
        { TestFileLine(), false,  "0" },
        { TestFileLine(), true,  "12" },
        { TestFileLine(), true,  "012" },
        { TestFileLine(), true,  "01234567" },
        { TestFileLine(), true,  "012345678" },
        { TestFileLine(), true,  "0123456789" },
        { TestFileLine(), true,  "1234567989012345679890123456798901234567989012345679890" },
        { TestFileLine(), false, "123456790.123" },
        { TestFileLine(), false, "-12" },
        { TestFileLine(), false, "12ab" },
    };

    [Theory]
    [MemberData(nameof(NaturalNumberRexData))]
    public void TestNaturalNumberRex(string TestLine, bool shouldBe, string input)
        => base.RegexTest(Numerical.NaturalNumber(), TestLine, shouldBe, input);

    public static TheoryData<string, bool, string, Captures?> IntegerNumberRexData => new() {
        { TestFileLine(), false, "",                                null },
        { TestFileLine(), false, " ",                               null },
        { TestFileLine(), true,  "0",                               new() { ["isign"] = "", ["iabs"] = "0" } },
        { TestFileLine(), true,  "12",                              new() { ["isign"] = "", ["iabs"] = "12" } },
        { TestFileLine(), true,  "012",                             new() { ["isign"] = "", ["iabs"] = "012" } },
        { TestFileLine(), true,  "01234567",                        new() { ["isign"] = "", ["iabs"] = "01234567" } },
        { TestFileLine(), true,  "012345678",                       new() { ["isign"] = "", ["iabs"] = "012345678" } },
        { TestFileLine(), true,  "0123456789",                      new() { ["isign"] = "", ["iabs"] = "0123456789" } },
        { TestFileLine(), true,  "12345679890",                     new() { ["isign"] = "", ["iabs"] = "12345679890" } },
        { TestFileLine(), false, "123456790.123",                   null },
        { TestFileLine(), true,  "-12",                             new() { ["isign"] = "-", ["iabs"] = "12" } },
        { TestFileLine(), true,  "-123456789012345678901234567890", new() { ["isign"] = "-", ["iabs"] = "123456789012345678901234567890" } },
        { TestFileLine(), true,  "+12",                             new() { ["isign"] = "+", ["iabs"] = "12" } },
        { TestFileLine(), true,  "+123456789012345678901234567890", new() { ["isign"] = "+", ["iabs"] = "123456789012345678901234567890" } },
        { TestFileLine(), false, "12ab", null },
    };

    [Theory]
    [MemberData(nameof(IntegerNumberRexData))]
    public void TestIntegerNumberRex(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Numerical.IntegerNumber(), TestLine, shouldBe, input, captures);

    public static TheoryData<string, bool, string, Captures?> FractionalNumberRexData => new() {
        { TestFileLine(), false, "",               null },
        { TestFileLine(), false, " ",              null },
        { TestFileLine(), false, ".",              null },
        { TestFileLine(), false, "+.",             null },
        { TestFileLine(), false, "-.",             null },
        { TestFileLine(), false, ".-",             null },
        { TestFileLine(), true,  "0",              new() { ["fsign"] = "",  ["whole"] = "0" ,           ["fraction"] = ""    } },
        { TestFileLine(), true,  "12",             new() { ["fsign"] = "",  ["whole"] = "12" ,          ["fraction"] = ""    } },
        { TestFileLine(), true,  "012",            new() { ["fsign"] = "",  ["whole"] = "012" ,         ["fraction"] = ""    } },
        { TestFileLine(), true,  "01234567",       new() { ["fsign"] = "",  ["whole"] = "01234567" ,    ["fraction"] = ""    } },
        { TestFileLine(), true,  "012345678",      new() { ["fsign"] = "",  ["whole"] = "012345678" ,   ["fraction"] = ""    } },
        { TestFileLine(), true,  "0123456789",     new() { ["fsign"] = "",  ["whole"] = "0123456789" ,  ["fraction"] = ""    } },
        { TestFileLine(), true,  "12345679890",    new() { ["fsign"] = "",  ["whole"] = "12345679890" , ["fraction"] = ""    } },
        { TestFileLine(), true,  "12345679890.",   new() { ["fsign"] = "",  ["whole"] = "12345679890" , ["fraction"] = ""    } },
        { TestFileLine(), true,  "123456790.123",  new() { ["fsign"] = "",  ["whole"] = "123456790" ,   ["fraction"] = "123" } },
        { TestFileLine(), true,  "-123456790.123", new() { ["fsign"] = "-", ["whole"] = "123456790" ,   ["fraction"] = "123" } },
        { TestFileLine(), true,  "-123456790.",    new() { ["fsign"] = "-", ["whole"] = "123456790" ,   ["fraction"] = ""    } },
        { TestFileLine(), true,  "+123456790.123", new() { ["fsign"] = "+", ["whole"] = "123456790" ,   ["fraction"] = "123" } },
        { TestFileLine(), true,  "+123456790.",    new() { ["fsign"] = "+", ["whole"] = "123456790" ,   ["fraction"] = ""    } },
        { TestFileLine(), true,  "-.123",          new() { ["fsign"] = "-", ["whole"] = "" ,            ["fraction"] = "123" } },
        { TestFileLine(), true,  "-123456790.",    new() { ["fsign"] = "-", ["whole"] = "123456790" ,   ["fraction"] = ""    } },
        { TestFileLine(), true,  "+.123",          new() { ["fsign"] = "+", ["whole"] = "" ,            ["fraction"] = "123" } },
        { TestFileLine(), true,  "+123456790.",    new() { ["fsign"] = "+", ["whole"] = "123456790" ,   ["fraction"] = ""    } },
        { TestFileLine(), true,  "-.123",          new() { ["fsign"] = "-", ["whole"] = "" ,            ["fraction"] = "123" } },
        { TestFileLine(), true,  "-12",            new() { ["fsign"] = "-", ["whole"] = "12" ,          ["fraction"] = ""    } },
        { TestFileLine(), true,  "+.123",          new() { ["fsign"] = "+", ["whole"] = "" ,            ["fraction"] = "123" } },
        { TestFileLine(), true,  "+12",            new() { ["fsign"] = "+", ["whole"] = "12" ,          ["fraction"] = ""    } },
        { TestFileLine(), false, "-.12.",          null },
        { TestFileLine(), true, "-123456789012345678901234567890", new() { ["fsign"] = "-", ["whole"] = "123456789012345678901234567890" , ["fraction"] = "" } },
        { TestFileLine(), true, "+123456789012345678901234567890", new() { ["fsign"] = "+", ["whole"] = "123456789012345678901234567890" , ["fraction"] = "" } },
        { TestFileLine(), true, "-123456789012345678901234567890.123456789012345678901234567890", new() { ["fsign"] = "-", ["whole"] = "123456789012345678901234567890" , ["fraction"] = "123456789012345678901234567890" } },
        { TestFileLine(), true, "+123456789012345678901234567890.123456789012345678901234567890", new() { ["fsign"] = "+", ["whole"] = "123456789012345678901234567890" , ["fraction"] = "123456789012345678901234567890" } },
        { TestFileLine(), false, "12ab", null },
    };

    [Theory]
    [MemberData(nameof(FractionalNumberRexData))]
    public void TestFractionalNumberRex(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Numerical.FractionalNumber(), TestLine, shouldBe, input, captures);

    public static TheoryData<string, bool, string, Captures?> ScientificNumberRexData => new() {
        { TestFileLine(), false, "",             null },
        { TestFileLine(), false, " ",            null },
        { TestFileLine(), false, ".",            null },
        { TestFileLine(), false, "+.",           null },
        { TestFileLine(), false, "-.",           null },
        { TestFileLine(), false, ".-",           null },
        { TestFileLine(), true,  "0e0",          new() { ["mantissa"] = "0",       ["fsign"] = "",  ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "0",    ["isign"] = "",  ["iabs"] = "0"   } },
        { TestFileLine(), true,  "0e123",        new() { ["mantissa"] = "0",       ["fsign"] = "",  ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "123",  ["isign"] = "",  ["iabs"] = "123" } },
        { TestFileLine(), true,  "0e-123",       new() { ["mantissa"] = "0",       ["fsign"] = "",  ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "-123", ["isign"] = "-", ["iabs"] = "123" } },
        { TestFileLine(), true,  "0e+123",       new() { ["mantissa"] = "0",       ["fsign"] = "",  ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "+123", ["isign"] = "+", ["iabs"] = "123" } },
        { TestFileLine(), true,  "0E0",          new() { ["mantissa"] = "0",       ["fsign"] = "",  ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "0",    ["isign"] = "",  ["iabs"] = "0"   } },
        { TestFileLine(), true,  "0E123",        new() { ["mantissa"] = "0",       ["fsign"] = "",  ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "123",  ["isign"] = "",  ["iabs"] = "123" } },
        { TestFileLine(), true,  "0E-123",       new() { ["mantissa"] = "0",       ["fsign"] = "",  ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "-123", ["isign"] = "-", ["iabs"] = "123" } },
        { TestFileLine(), true,  "0E+123",       new() { ["mantissa"] = "0",       ["fsign"] = "",  ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "+123", ["isign"] = "+", ["iabs"] = "123" } },
        { TestFileLine(), true,  "-0E123",       new() { ["mantissa"] = "-0",      ["fsign"] = "-", ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "123",  ["isign"] = "",  ["iabs"] = "123" } },
        { TestFileLine(), true,  "-0E-123",      new() { ["mantissa"] = "-0",      ["fsign"] = "-", ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "-123", ["isign"] = "-", ["iabs"] = "123" } },
        { TestFileLine(), true,  "-0E+123",      new() { ["mantissa"] = "-0",      ["fsign"] = "-", ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "+123", ["isign"] = "+", ["iabs"] = "123" } },
        { TestFileLine(), true,  "+0E123",       new() { ["mantissa"] = "+0",      ["fsign"] = "+", ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "123",  ["isign"] = "",  ["iabs"] = "123" } },
        { TestFileLine(), true,  "+0E-123",      new() { ["mantissa"] = "+0",      ["fsign"] = "+", ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "-123", ["isign"] = "-", ["iabs"] = "123" } },
        { TestFileLine(), true,  "+0E+123",      new() { ["mantissa"] = "+0",      ["fsign"] = "+", ["whole"] = "0" ,  ["fraction"] = "",    ["exponent"] = "+123", ["isign"] = "+", ["iabs"] = "123" } },
        { TestFileLine(), true,  "0.12E123",     new() { ["mantissa"] = "0.12",    ["fsign"] = "",  ["whole"] = "0" ,  ["fraction"] = "12",  ["exponent"] = "123",  ["isign"] = "",  ["iabs"] = "123" } },
        { TestFileLine(), true,  "1E123",        new() { ["mantissa"] = "1",       ["fsign"] = "",  ["whole"] = "1" ,  ["fraction"] = "",    ["exponent"] = "123",  ["isign"] = "",  ["iabs"] = "123" } },
        { TestFileLine(), true,  "+1E123",       new() { ["mantissa"] = "+1",      ["fsign"] = "+", ["whole"] = "1" ,  ["fraction"] = "",    ["exponent"] = "123",  ["isign"] = "",  ["iabs"] = "123" } },
        { TestFileLine(), true,  "-1E123",       new() { ["mantissa"] = "-1",      ["fsign"] = "-", ["whole"] = "1" ,  ["fraction"] = "",    ["exponent"] = "123",  ["isign"] = "",  ["iabs"] = "123" } },
        { TestFileLine(), true,  "0.12E-123",    new() { ["mantissa"] = "0.12",    ["fsign"] = "",  ["whole"] = "0" ,  ["fraction"] = "12",  ["exponent"] = "-123", ["isign"] = "-", ["iabs"] = "123" } },
        { TestFileLine(), true,  "0.12E+123",    new() { ["mantissa"] = "0.12",    ["fsign"] = "",  ["whole"] = "0" ,  ["fraction"] = "12",  ["exponent"] = "+123", ["isign"] = "+", ["iabs"] = "123" } },
        { TestFileLine(), true,  "-0.23E123",    new() { ["mantissa"] = "-0.23",   ["fsign"] = "-", ["whole"] = "0" ,  ["fraction"] = "23",  ["exponent"] = "123",  ["isign"] = "",  ["iabs"] = "123" } },
        { TestFileLine(), true,  "-0.23E-123",   new() { ["mantissa"] = "-0.23",   ["fsign"] = "-", ["whole"] = "0" ,  ["fraction"] = "23",  ["exponent"] = "-123", ["isign"] = "-", ["iabs"] = "123" } },
        { TestFileLine(), true,  "-0.23E+123",   new() { ["mantissa"] = "-0.23",   ["fsign"] = "-", ["whole"] = "0" ,  ["fraction"] = "23",  ["exponent"] = "+123", ["isign"] = "+", ["iabs"] = "123" } },
        { TestFileLine(), true,  "+0.345E123",   new() { ["mantissa"] = "+0.345",  ["fsign"] = "+", ["whole"] = "0" ,  ["fraction"] = "345", ["exponent"] = "123",  ["isign"] = "",  ["iabs"] = "123" } },
        { TestFileLine(), true,  "+0.345E-123",  new() { ["mantissa"] = "+0.345",  ["fsign"] = "+", ["whole"] = "0" ,  ["fraction"] = "345", ["exponent"] = "-123", ["isign"] = "-", ["iabs"] = "123" } },
        { TestFileLine(), true,  "+0.345E+123",  new() { ["mantissa"] = "+0.345",  ["fsign"] = "+", ["whole"] = "0" ,  ["fraction"] = "345", ["exponent"] = "+123", ["isign"] = "+", ["iabs"] = "123" } },
        { TestFileLine(), true,  "+0.345E0",     new() { ["mantissa"] = "+0.345",  ["fsign"] = "+", ["whole"] = "0" ,  ["fraction"] = "345", ["exponent"] = "0",    ["isign"] = "",  ["iabs"] = "0"   } },
        { TestFileLine(), true,  "+0.345E-0",    new() { ["mantissa"] = "+0.345",  ["fsign"] = "+", ["whole"] = "0" ,  ["fraction"] = "345", ["exponent"] = "-0",   ["isign"] = "-", ["iabs"] = "0"   } },
        { TestFileLine(), true,  "+0.345E+0",    new() { ["mantissa"] = "+0.345",  ["fsign"] = "+", ["whole"] = "0" ,  ["fraction"] = "345", ["exponent"] = "+0",   ["isign"] = "+", ["iabs"] = "0"   } },
        { TestFileLine(), true,  "9.345E",       new() { ["mantissa"] = "9.345",   ["fsign"] = "",  ["whole"] = "9" ,  ["fraction"] = "345", ["exponent"] = "",     ["isign"] = "",  ["iabs"] = ""    } },
        { TestFileLine(), true,  "+10.345E",     new() { ["mantissa"] = "+10.345", ["fsign"] = "+", ["whole"] = "10" , ["fraction"] = "345", ["exponent"] = "",     ["isign"] = "",  ["iabs"] = ""    } },
        { TestFileLine(), true,  "-01.345E",     new() { ["mantissa"] = "-01.345", ["fsign"] = "-", ["whole"] = "01" , ["fraction"] = "345", ["exponent"] = "",     ["isign"] = "",  ["iabs"] = ""    } },
        { TestFileLine(), true,  ".345E",        new() { ["mantissa"] = ".345",    ["fsign"] = "",  ["whole"] = "" ,   ["fraction"] = "345", ["exponent"] = "",     ["isign"] = "",  ["iabs"] = ""    } },
        { TestFileLine(), true,  "+10.E",        new() { ["mantissa"] = "+10.",    ["fsign"] = "+", ["whole"] = "10" , ["fraction"] = "",    ["exponent"] = "",     ["isign"] = "",  ["iabs"] = ""    } },
        { TestFileLine(), true,  "-01.E12",      new() { ["mantissa"] = "-01.",    ["fsign"] = "-", ["whole"] = "01" , ["fraction"] = "",    ["exponent"] = "12",   ["isign"] = "",  ["iabs"] = "12"  } },
        { TestFileLine(), false, "+10.345E-",    null },
        { TestFileLine(), false, "+10.345E-12q", null },
        { TestFileLine(), false, "+10.345E.12",  null },
        { TestFileLine(), false, "+10.345d12",   null },
        { TestFileLine(), false, "+10.345",      null },
        { TestFileLine(), false, "+.e12",        null },
        { TestFileLine(), false, "+e12",         null },
        { TestFileLine(), false, "0",            null },
        { TestFileLine(), false, "e12",          null },
    };

    [Theory]
    [MemberData(nameof(ScientificNumberRexData))]
    public void TestScientificNumberRex(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(Numerical.ScientificNumber(), TestLine, shouldBe, input, captures);

    public static TheoryData<string, bool, string> UuidRexData => new() {
        { TestFileLine(), false, "" },
        { TestFileLine(), false, " " },
        { TestFileLine(), false, "0" },
        { TestFileLine(), false, "0123abc" },
        { TestFileLine(), false, "0123456789abcdef0123456789abcdef0" },
        { TestFileLine(), false, "0123456789abcdef0123456789abcde" },
        { TestFileLine(), true,  "0123456789abcdef0123456789abcdef" },
        { TestFileLine(), true,  "01234567-89ab-cdef-0123-456789abcdef" },
        { TestFileLine(), false, "{01234567-89ab-cdef-0123-456789abcdef0}" },
        { TestFileLine(), false, "{01234567-89ab-cdef-0123-456789abcde}" },
        { TestFileLine(), false, "(01234567-89ab-cdef-0123-456789abcdef0)" },
        { TestFileLine(), false, "(01234567-89ab-cdef-0123-456789abcde)" },
        { TestFileLine(), true,  "{01234567-89ab-cdef-0123-456789abcdef}" },
        { TestFileLine(), true,  "(01234567-89ab-cdef-0123-456789abcdef)" },
        { TestFileLine(), false, "0123ABC" },
        { TestFileLine(), false, "0123456789ABCDEF0123456789ABCDEF0" },
        { TestFileLine(), false, "0123456789ABCDEF0123456789ABCDE" },
        { TestFileLine(), true,  "0123456789ABCDEF0123456789ABCDEF" },
        { TestFileLine(), true,  "01234567-89AB-CDEF-0123-456789ABCDEF" },
        { TestFileLine(), false, "{01234567-89AB-CDEF-0123-456789ABCDEF0}" },
        { TestFileLine(), false, "{01234567-89AB-CDEF-0123-456789ABCDE}" },
        { TestFileLine(), false, "(01234567-89AB-CDEF-0123-456789ABCDEF0)" },
        { TestFileLine(), false, "(01234567-89AB-CDEF-0123-456789ABCDE)" },
        { TestFileLine(), true,  "{01234567-89AB-CDEF-0123-456789ABCDEF}" },
        { TestFileLine(), true,  "(01234567-89AB-CDEF-0123-456789ABCDEF)" },
        { TestFileLine(), true,  "{01234567-89ab-CDEF-0123-456789abcdef}" },
        { TestFileLine(), true,  "(01234567-89ab-CDEF-0123-456789abcdef)" },
    };

    [Theory]
    [MemberData(nameof(UuidRexData))]
    public void TestUuidRex(string TestLine, bool shouldBe, string input)
        => base.RegexTest(Numerical.Uuid(), TestLine, shouldBe, input, null, failIfMissingExpected: false);
}
