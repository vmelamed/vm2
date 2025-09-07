namespace vm2.RegexLib.Tests;

public class CountriesUsTests(
    RegexLibTestsFixture fixture,
    ITestOutputHelper output) : RegexTests(fixture, output)
{
    public static TheoryData<string, bool, string, Captures?> TelephoneNumberData => new() {
        { TestFileLine(), false, "", null},
        { TestFileLine(), false, " ", null},
        { TestFileLine(), false, "  ", null},
        { TestFileLine(), false, "0", null},
        { TestFileLine(), false, "+01", null},
        { TestFileLine(), false, "012", null},
        { TestFileLine(), false, "+0123", null},
        { TestFileLine(), false, "1234567890123456", null},
        { TestFileLine(), false, "+1234567890123456", null},
        { TestFileLine(), false,  "1234", null},
        { TestFileLine(), false, "+1234a-BC", null},
        { TestFileLine(), false, "*1234", null},
        { TestFileLine(), false,  "+1234", null},
        { TestFileLine(), false, "+123456", null},
        { TestFileLine(), true,  "+12345678901", new() { ["area"] = "234", ["number"] = "5678901" } },
        { TestFileLine(), true,  "12345678901", new() { ["area"] = "234", ["number"] = "5678901" } },
        { TestFileLine(), true,  "+1 (234) 567-8901", new() { ["area"] = "234", ["number"] = "567-8901" } },
        { TestFileLine(), true,  "+1 234-567-8901", new() { ["area"] = "234", ["number"] = "567-8901" } },
        { TestFileLine(), true,  "1(234)567-8901", new() { ["area"] = "234", ["number"] = "567-8901" } },
        { TestFileLine(), false, "1(034)567-8901", null},
        { TestFileLine(), false, "1(911)511-8901", null},
        { TestFileLine(), false, "+1.234.567.8901", null},
        { TestFileLine(), false, "+1+234+567+8901", null},
    };

    [Theory]
    [MemberData(nameof(TelephoneNumberData))]
    public void TestTelephoneNumber(string TestLine, bool shouldBe, string code, Captures? captures = null)
        => base.RegexTest(Countries.US.TelephoneNumber(), TestLine, shouldBe, code, captures);

    public static TheoryData<string, bool, string, Captures?> TelephoneNumberStrictData => new() {
        { TestFileLine(), false, "", null},
        { TestFileLine(), false, " ", null},
        { TestFileLine(), false, "  ", null},
        { TestFileLine(), false, "0", null},
        { TestFileLine(), false, "+01", null},
        { TestFileLine(), false, "012", null},
        { TestFileLine(), false, "+0123", null },
        { TestFileLine(), false, "1234567890123456", null },
        { TestFileLine(), false, "+1234567890123456", null },
        { TestFileLine(), false, "1234", null },
        { TestFileLine(), false, "+1234a-BC", null },
        { TestFileLine(), false, "*1234", null },
        { TestFileLine(), false, "+1234", null },
        { TestFileLine(), false, "+123456", null },
        { TestFileLine(), false, "+12345678901", null },
        { TestFileLine(), false, "12345678901", null },
        { TestFileLine(), true,  "+1 (234) 567-8901", new() { ["area"] = "234", ["number"] = "567-8901"} },
        { TestFileLine(), false, "+1 234-567-8901", null },
        { TestFileLine(), true,  "1(234)567-8901", new() { ["area"] = "234", ["number"] = "567-8901"} },
        { TestFileLine(), false, "1(034)567-8901", null },
        { TestFileLine(), false, "1(911)511-8901", null },
        { TestFileLine(), false, "+1.234.567.8901", null },
        { TestFileLine(), false, "+1+234+567+8901", null },
    };

    [Theory]
    [MemberData(nameof(TelephoneNumberStrictData))]
    public void TestTelephoneNumberStrict(string TestLine, bool shouldBe, string code, Captures? captures)
        => base.RegexTest(Countries.US.TelephoneNumberStrict(), TestLine, shouldBe, code, captures);

    public static TheoryData<string, bool, string> StateCodeData => new() {
        { TestFileLine(), false, ""},
        { TestFileLine(), false, " "},
        { TestFileLine(), false, "  "},
        { TestFileLine(), false, "0"},
        { TestFileLine(), false, "01"},
        { TestFileLine(), false, "012"},
        { TestFileLine(), false, "0123"},
        { TestFileLine(), false, "A"},
        { TestFileLine(), false, "A1"},
        { TestFileLine(), false, "0B"},
        { TestFileLine(), true,  "AB"},
        { TestFileLine(), false, "A B"},
        { TestFileLine(), false, " AB "},
        { TestFileLine(), false, " A B "},
        { TestFileLine(), false, "a"},
        { TestFileLine(), false, "a1"},
        { TestFileLine(), false, "0b"},
        { TestFileLine(), false, "ab"},
        { TestFileLine(), false, "a b"},
        { TestFileLine(), false, " ab "},
        { TestFileLine(), false, " a b "},
    };

    [Theory]
    [MemberData(nameof(StateCodeData))]
    public void TestStateCode(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.US.StateCode(), TestLine, shouldBe, code);

    public static TheoryData<string, bool, string> StateCodeIData => new() {
        { TestFileLine(), false, ""},
        { TestFileLine(), false, " "},
        { TestFileLine(), false, "  "},
        { TestFileLine(), false, "0"},
        { TestFileLine(), false, "01"},
        { TestFileLine(), false, "012"},
        { TestFileLine(), false, "0123"},
        { TestFileLine(), false, "A"},
        { TestFileLine(), false, "A1"},
        { TestFileLine(), false, "0B"},
        { TestFileLine(), true,  "AB"},
        { TestFileLine(), false, "A B"},
        { TestFileLine(), false, " AB "},
        { TestFileLine(), false, " A B "},
        { TestFileLine(), false, "a"},
        { TestFileLine(), false, "a1"},
        { TestFileLine(), false, "0b"},
        { TestFileLine(), true,  "ab"},
        { TestFileLine(), false, "a b"},
        { TestFileLine(), false, " ab "},
        { TestFileLine(), false, " a b "},
    };

    [Theory]
    [MemberData(nameof(StateCodeIData))]
    public void TestStateCodeI(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.US.StateCodeI(), TestLine, shouldBe, code);

    public static TheoryData<string, bool, string, Captures?> ZipCode5Data => new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), false, "  ", null },
        { TestFileLine(), false, "0", null },
        { TestFileLine(), false, "01", null },
        { TestFileLine(), false, "012", null },
        { TestFileLine(), false, "0123", null },
        { TestFileLine(), true,  "01234", new() { ["zip"] = "01234" } },
        { TestFileLine(), false, "012345", null },
        { TestFileLine(), false, "A", null },
        { TestFileLine(), false, "AA", null },
        { TestFileLine(), false, "AAA", null },
        { TestFileLine(), false, "AAAA", null },
        { TestFileLine(), false, "AAAAA", null },
    };

    [Theory]
    [MemberData(nameof(ZipCode5Data))]
    public void TestZipCode5(string TestLine, bool shouldBe, string code, Captures? captures)
        => base.RegexTest(Countries.US.ZipCode5(), TestLine, shouldBe, code, captures);

    public static TheoryData<string, bool, string, Captures?> ZipCode5x4Data => new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), false, "  ", null },
        { TestFileLine(), false, "0", null },
        { TestFileLine(), false, "01", null },
        { TestFileLine(), false, "012", null },
        { TestFileLine(), false, "0123", null },
        { TestFileLine(), false,  "01234", null },
        { TestFileLine(), true,  "01234-5678", new() { ["zip"] = "01234", ["ext"] = "5678" } },
        { TestFileLine(), false, "A", null },
        { TestFileLine(), false, "AA", null },
        { TestFileLine(), false, "AAA", null },
        { TestFileLine(), false, "AAAA", null },
        { TestFileLine(), false, "AAAAA", null },
    };

    [Theory]
    [MemberData(nameof(ZipCode5x4Data))]
    public void TestZipCode5x4(string TestLine, bool shouldBe, string code, Captures? captures)
        => base.RegexTest(Countries.US.ZipCode5x4(), TestLine, shouldBe, code, captures);

    public static TheoryData<string, bool, string, Captures?> ZipCode5o4Data => new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), false, "  ", null },
        { TestFileLine(), false, "0", null },
        { TestFileLine(), false, "01", null },
        { TestFileLine(), false, "012", null },
        { TestFileLine(), false, "0123", null },
        { TestFileLine(), true,  "01234", new() { ["zip"] = "01234", ["ext"] = "" } },
        { TestFileLine(), true,  "01234-5678", new() { ["zip"] = "01234", ["ext"] = "5678" } },
        { TestFileLine(), false, "A", null },
        { TestFileLine(), false, "AA", null },
        { TestFileLine(), false, "AAA", null },
        { TestFileLine(), false, "AAAA", null },
        { TestFileLine(), false, "AAAAA", null },
    };

    [Theory]
    [MemberData(nameof(ZipCode5o4Data))]
    public void TestZipCode5o4(string TestLine, bool shouldBe, string code, Captures? captures)
        => base.RegexTest(Countries.US.ZipCode5o4(), TestLine, shouldBe, code, captures);

    public static TheoryData<string, bool, string> SocialSecurityNumberData => new() {
        { TestFileLine(), false, "" },
        { TestFileLine(), false, " " },
        { TestFileLine(), false, "a" },
        { TestFileLine(), false, "ab" },
        { TestFileLine(), false, "abcdefghi" },
        { TestFileLine(), false, "1" },
        { TestFileLine(), false, "12345678" },
        { TestFileLine(), false, "1234567890" },
        { TestFileLine(), false, "1234567890123" },
        { TestFileLine(), false, "123.45.6789" },
        { TestFileLine(), true,  "123456789" },
        { TestFileLine(), true,  "123-45-6789" },
        { TestFileLine(), false, "823-45-6789" },
        { TestFileLine(), false, "923-45-6789" },
        { TestFileLine(), false, "783-45-6789" },
        { TestFileLine(), false, "793-45-6789" },
        { TestFileLine(), true,  "763-45-6789" },
        { TestFileLine(), false, "778-45-6789" },
        { TestFileLine(), true,  "771-45-6789" },
        { TestFileLine(), false, "000-45-6789" },
        { TestFileLine(), false, "123-00-6789" },
        { TestFileLine(), false, "123-45-0000" },
        { TestFileLine(), false, "666-45-6789" },
        { TestFileLine(), false, "893-45-6789" },
        { TestFileLine(), false, "983-45-6789" },
        { TestFileLine(), false, "783-45-6789" },
        { TestFileLine(), false, "793-45-6789" },
        { TestFileLine(), false, "773-45-6789" },
        { TestFileLine(), false, "779-45-6789" },
        { TestFileLine(), false, "001-01-0001" },
        { TestFileLine(), false, "078-05-1120" },
        { TestFileLine(), false, "433-54-3937" },
    };

    [Theory]
    [MemberData(nameof(SocialSecurityNumberData))]
    public void TestSocialSecurityNumber(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.US.SocialSecurityNumber(), TestLine, shouldBe, code);

    public static TheoryData<string, bool, string> ItinData => new() {
        { TestFileLine(), false, "" },
        { TestFileLine(), false, " " },
        { TestFileLine(), false, "a" },
        { TestFileLine(), false, "ab" },
        { TestFileLine(), false, "abcdefghi" },
        { TestFileLine(), false, "1" },
        { TestFileLine(), false, "12345678" },
        { TestFileLine(), false, "1234567890" },
        { TestFileLine(), false, "1234567890123" },
        { TestFileLine(), false, "123.45.6789" },
        { TestFileLine(), false, "823-45-6789" },
        { TestFileLine(), true,  "923-75-6789" },
        { TestFileLine(), true,  "923-85-6789" },
        { TestFileLine(), true,  "923-95-6789" },
        { TestFileLine(), false, "783-45-6789" },
        { TestFileLine(), false, "793-45-6789" },
        { TestFileLine(), false, "763-45-6789" },
        { TestFileLine(), false, "778-45-6789" },
        { TestFileLine(), false, "771-45-6789" },
        { TestFileLine(), false, "000-45-6789" },
        { TestFileLine(), false, "123-00-6789" },
        { TestFileLine(), false, "123-45-0000" },
        { TestFileLine(), false, "666-45-6789" },
        { TestFileLine(), false, "893-45-6789" },
        { TestFileLine(), false, "983-45-6789" },
        { TestFileLine(), false, "783-45-6789" },
        { TestFileLine(), false, "793-45-6789" },
        { TestFileLine(), false, "773-45-6789" },
        { TestFileLine(), false, "779-45-6789" },
        { TestFileLine(), false, "001-01-0001" },
        { TestFileLine(), false, "078-05-1120" },
        { TestFileLine(), false, "433-54-3937" },
    };

    [Theory]
    [MemberData(nameof(ItinData))]
    public void TestItin(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.US.Itin(), TestLine, shouldBe, code);
}
