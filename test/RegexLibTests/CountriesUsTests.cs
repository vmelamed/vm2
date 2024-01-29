namespace vm2.RegexLibTests;

public class CountriesUsTests(ITestOutputHelper output) : RegexTests(output)
{
    public static TheoryData<string, bool, string> TelephoneNumberData => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "0"},
        { TestLine(), false, "+01"},
        { TestLine(), false, "012"},
        { TestLine(), false, "+0123"},
        { TestLine(), false, "1234567890123456"},
        { TestLine(), false, "+1234567890123456"},
        { TestLine(), false,  "1234"},
        { TestLine(), false, "+1234a-BC"},
        { TestLine(), false, "*1234"},
        { TestLine(), false,  "+1234"},
        { TestLine(), false, "+123456"},
        { TestLine(), true,  "+12345678901"},
        { TestLine(), true,  "12345678901"},
        { TestLine(), true,  "+1 (234) 567-8901"},
        { TestLine(), true,  "+1 234-567-8901"},
        { TestLine(), true,  "1(234)567-8901"},
        { TestLine(), false, "1(034)567-8901"},
        { TestLine(), false, "1(911)511-8901"},
        { TestLine(), false, "+1.234.567.8901"},
        { TestLine(), false, "+1+234+567+8901"},
    };

    [Theory]
    [MemberData(nameof(TelephoneNumberData))]
    public void TestTelephoneNumber(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.US.TelephoneNumber, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> TelephoneNumberStrictData => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "0"},
        { TestLine(), false, "+01"},
        { TestLine(), false, "012"},
        { TestLine(), false, "+0123"},
        { TestLine(), false, "1234567890123456"},
        { TestLine(), false, "+1234567890123456"},
        { TestLine(), false, "1234"},
        { TestLine(), false, "+1234a-BC"},
        { TestLine(), false, "*1234"},
        { TestLine(), false, "+1234"},
        { TestLine(), false, "+123456"},
        { TestLine(), false, "+12345678901"},
        { TestLine(), false, "12345678901"},
        { TestLine(), true,  "+1 (234) 567-8901"},
        { TestLine(), false, "+1 234-567-8901"},
        { TestLine(), true,  "1(234)567-8901"},
        { TestLine(), false, "1(034)567-8901"},
        { TestLine(), false, "1(911)511-8901"},
        { TestLine(), false, "+1.234.567.8901"},
        { TestLine(), false, "+1+234+567+8901"},
    };

    [Theory]
    [MemberData(nameof(TelephoneNumberStrictData))]
    public void TestTelephoneNumberStrict(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.US.TelephoneNumberStrict, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> StateCodeData => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "0"},
        { TestLine(), false, "01"},
        { TestLine(), false, "012"},
        { TestLine(), false, "0123"},
        { TestLine(), false, "A"},
        { TestLine(), false, "A1"},
        { TestLine(), false, "0B"},
        { TestLine(), true,  "AB"},
        { TestLine(), false, "A B"},
        { TestLine(), false, " AB "},
        { TestLine(), false, " A B "},
        { TestLine(), false, "a"},
        { TestLine(), false, "a1"},
        { TestLine(), false, "0b"},
        { TestLine(), false, "ab"},
        { TestLine(), false, "a b"},
        { TestLine(), false, " ab "},
        { TestLine(), false, " a b "},
    };

    [Theory]
    [MemberData(nameof(StateCodeData))]
    public void TestStateCode(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.US.StateCode, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> StateCodeIData => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "0"},
        { TestLine(), false, "01"},
        { TestLine(), false, "012"},
        { TestLine(), false, "0123"},
        { TestLine(), false, "A"},
        { TestLine(), false, "A1"},
        { TestLine(), false, "0B"},
        { TestLine(), true,  "AB"},
        { TestLine(), false, "A B"},
        { TestLine(), false, " AB "},
        { TestLine(), false, " A B "},
        { TestLine(), false, "a"},
        { TestLine(), false, "a1"},
        { TestLine(), false, "0b"},
        { TestLine(), true,  "ab"},
        { TestLine(), false, "a b"},
        { TestLine(), false, " ab "},
        { TestLine(), false, " a b "},
    };

    [Theory]
    [MemberData(nameof(StateCodeIData))]
    public void TestStateCodeI(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.US.StateCodeI, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> ZipCode5Data => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "0"},
        { TestLine(), false, "01"},
        { TestLine(), false, "012"},
        { TestLine(), false, "0123"},
        { TestLine(), true,  "01234"},
        { TestLine(), false, "012345"},
        { TestLine(), false, "A"},
        { TestLine(), false, "AA"},
        { TestLine(), false, "AAA"},
        { TestLine(), false, "AAAA"},
        { TestLine(), false, "AAAAA"},
    };

    [Theory]
    [MemberData(nameof(ZipCode5Data))]
    public void TestZipCode5(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.US.ZipCode5, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> ZipCode5x4Data => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "0"},
        { TestLine(), false, "01"},
        { TestLine(), false, "012"},
        { TestLine(), false, "0123"},
        { TestLine(), false,  "01234"},
        { TestLine(), true,  "01234-5678"},
        { TestLine(), false, "A"},
        { TestLine(), false, "AA"},
        { TestLine(), false, "AAA"},
        { TestLine(), false, "AAAA"},
        { TestLine(), false, "AAAAA"},
    };

    [Theory]
    [MemberData(nameof(ZipCode5x4Data))]
    public void TestZipCode5x4(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.US.ZipCode5x4, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> ZipCode5o4Data => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "0"},
        { TestLine(), false, "01"},
        { TestLine(), false, "012"},
        { TestLine(), false, "0123"},
        { TestLine(), true,  "01234"},
        { TestLine(), true,  "01234-5678"},
        { TestLine(), false, "A"},
        { TestLine(), false, "AA"},
        { TestLine(), false, "AAA"},
        { TestLine(), false, "AAAA"},
        { TestLine(), false, "AAAAA"},
    };

    [Theory]
    [MemberData(nameof(ZipCode5o4Data))]
    public void TestZipCode5o4(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.US.ZipCode5o4, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> SocialSecurityNumberData => new() {
        { TestLine(), false, "" },
        { TestLine(), false, " " },
        { TestLine(), false, "a" },
        { TestLine(), false, "ab" },
        { TestLine(), false, "abcdefghi" },
        { TestLine(), false, "1" },
        { TestLine(), false, "12345678" },
        { TestLine(), false, "1234567890" },
        { TestLine(), false, "1234567890123" },
        { TestLine(), false, "123.45.6789" },
        { TestLine(), true,  "123456789" },
        { TestLine(), true,  "123-45-6789" },
        { TestLine(), false, "823-45-6789" },
        { TestLine(), false, "923-45-6789" },
        { TestLine(), false, "783-45-6789" },
        { TestLine(), false, "793-45-6789" },
        { TestLine(), true,  "763-45-6789" },
        { TestLine(), false, "778-45-6789" },
        { TestLine(), true,  "771-45-6789" },
        { TestLine(), false, "000-45-6789" },
        { TestLine(), false, "123-00-6789" },
        { TestLine(), false, "123-45-0000" },
        { TestLine(), false, "666-45-6789" },
        { TestLine(), false, "893-45-6789" },
        { TestLine(), false, "983-45-6789" },
        { TestLine(), false, "783-45-6789" },
        { TestLine(), false, "793-45-6789" },
        { TestLine(), false, "773-45-6789" },
        { TestLine(), false, "779-45-6789" },
        { TestLine(), false, "001-01-0001" },
        { TestLine(), false, "078-05-1120" },
        { TestLine(), false, "433-54-3937" },
    };

    [Theory]
    [MemberData(nameof(SocialSecurityNumberData))]
    public void TestSocialSecurityNumber(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.US.SocialSecurityNumber, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> ItinData => new() {
        { TestLine(), false, "" },
        { TestLine(), false, " " },
        { TestLine(), false, "a" },
        { TestLine(), false, "ab" },
        { TestLine(), false, "abcdefghi" },
        { TestLine(), false, "1" },
        { TestLine(), false, "12345678" },
        { TestLine(), false, "1234567890" },
        { TestLine(), false, "1234567890123" },
        { TestLine(), false, "123.45.6789" },
        { TestLine(), false, "823-45-6789" },
        { TestLine(), true,  "923-75-6789" },
        { TestLine(), true,  "923-85-6789" },
        { TestLine(), true,  "923-95-6789" },
        { TestLine(), false, "783-45-6789" },
        { TestLine(), false, "793-45-6789" },
        { TestLine(), false, "763-45-6789" },
        { TestLine(), false, "778-45-6789" },
        { TestLine(), false, "771-45-6789" },
        { TestLine(), false, "000-45-6789" },
        { TestLine(), false, "123-00-6789" },
        { TestLine(), false, "123-45-0000" },
        { TestLine(), false, "666-45-6789" },
        { TestLine(), false, "893-45-6789" },
        { TestLine(), false, "983-45-6789" },
        { TestLine(), false, "783-45-6789" },
        { TestLine(), false, "793-45-6789" },
        { TestLine(), false, "773-45-6789" },
        { TestLine(), false, "779-45-6789" },
        { TestLine(), false, "001-01-0001" },
        { TestLine(), false, "078-05-1120" },
        { TestLine(), false, "433-54-3937" },
    };

    [Theory]
    [MemberData(nameof(ItinData))]
    public void TestItin(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.US.Itin, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }
}
