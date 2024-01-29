namespace vm2.RegexLibTests;

public class CountriesTests(ITestOutputHelper output) : RegexTests(output)
{
    public static TheoryData<string, bool, string> CountryCode2Data => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "a"},
        { TestLine(), false, "ab"},
        { TestLine(), false, "abc"},
        { TestLine(), false, "a1"},
        { TestLine(), false, "1a"},
        { TestLine(), false, "12"},
        { TestLine(), false, "A"},
        { TestLine(), false, "A1"},
        { TestLine(), false, "A="},
        { TestLine(), false, "1A"},
        { TestLine(), false, "@A"},
        { TestLine(), false, "ABC"},
        { TestLine(), true,  "AB"},
    };

    [Theory]
    [MemberData(nameof(CountryCode2Data))]
    public void TestCountryCode2(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.CountryCode2, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> CountryCode2IData => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "a"},
        { TestLine(), true,  "ab"},
        { TestLine(), false, "abc"},
        { TestLine(), false, "a1"},
        { TestLine(), false, "a/"},
        { TestLine(), false, "1a"},
        { TestLine(), false, "-a"},
        { TestLine(), false, "12"},
        { TestLine(), false, "A"},
        { TestLine(), false, "-1"},
        { TestLine(), false, "A1"},
        { TestLine(), false, "A$"},
        { TestLine(), false, "1A"},
        { TestLine(), false, "|A"},
        { TestLine(), false, "ABC"},
        { TestLine(), true,  "AB"},
    };

    [Theory]
    [MemberData(nameof(CountryCode2IData))]
    public void TestCountryCode2I(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.CountryCode2I, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> CountryCode3Data => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "a"},
        { TestLine(), false, "ab"},
        { TestLine(), false, "abc"},
        { TestLine(), false, "a1"},
        { TestLine(), false, "1a"},
        { TestLine(), false, "13"},
        { TestLine(), false, "A"},
        { TestLine(), false, "A1"},
        { TestLine(), false, "A="},
        { TestLine(), false, "1A"},
        { TestLine(), false, "@A"},
        { TestLine(), true,  "ABC"},
        { TestLine(), false, "AB"},
    };

    [Theory]
    [MemberData(nameof(CountryCode3Data))]
    public void TestCountryCode3(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.CountryCode3, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> CountryCode3IData => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "a"},
        { TestLine(), false, "ab"},
        { TestLine(), true,  "abc"},
        { TestLine(), false, "a1"},
        { TestLine(), false, "a/"},
        { TestLine(), false, "1a"},
        { TestLine(), false, "-a"},
        { TestLine(), false, "13"},
        { TestLine(), false, "A"},
        { TestLine(), false, "-1"},
        { TestLine(), false, "A1"},
        { TestLine(), false, "A$"},
        { TestLine(), false, "1A"},
        { TestLine(), false, "|A"},
        { TestLine(), true,  "ABC"},
        { TestLine(), false, "AB"},
    };

    [Theory]
    [MemberData(nameof(CountryCode3IData))]
    public void TestCountryCode3I(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.CountryCode3I, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }


    public static TheoryData<string, bool, string> CurrencyCodeData => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "a"},
        { TestLine(), false, "ab"},
        { TestLine(), false, "abc"},
        { TestLine(), false, "a1"},
        { TestLine(), false, "1a"},
        { TestLine(), false, "13"},
        { TestLine(), false, "A"},
        { TestLine(), false, "A1"},
        { TestLine(), false, "A="},
        { TestLine(), false, "1A"},
        { TestLine(), false, "@A"},
        { TestLine(), true,  "ABC"},
        { TestLine(), false, "AB"},
    };

    [Theory]
    [MemberData(nameof(CurrencyCodeData))]
    public void TestCurrencyCode(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.CurrencyCode, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> CurrencyCodeIData => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "a"},
        { TestLine(), false, "ab"},
        { TestLine(), true,  "abc"},
        { TestLine(), false, "a1"},
        { TestLine(), false, "a/"},
        { TestLine(), false, "1a"},
        { TestLine(), false, "-a"},
        { TestLine(), false, "13"},
        { TestLine(), false, "A"},
        { TestLine(), false, "-1"},
        { TestLine(), false, "A1"},
        { TestLine(), false, "A$"},
        { TestLine(), false, "1A"},
        { TestLine(), false, "|A"},
        { TestLine(), true,  "ABC"},
        { TestLine(), false, "AB"},
    };

    [Theory]
    [MemberData(nameof(CurrencyCodeIData))]
    public void TestCurrencyCodeI(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.CurrencyCodeI, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> TelephoneCodeData => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "0"},
        { TestLine(), false, "01"},
        { TestLine(), false, "012"},
        { TestLine(), true,  "1"},
        { TestLine(), true,  "12"},
        { TestLine(), true,  "103"},
        { TestLine(), true,  "120"},
        { TestLine(), false, "1234"},
        { TestLine(), false, "a"},
        { TestLine(), false, "A"},
        { TestLine(), false, "AB"},
        { TestLine(), false, "ABC"},
    };

    [Theory]
    [MemberData(nameof(TelephoneCodeData))]
    public void TestTelephoneCode(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.TelephoneCode, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> TelephoneNumberData => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "0"},
        { TestLine(), false, "01"},
        { TestLine(), false, "012"},
        { TestLine(), false, "1234567890123456"},
        { TestLine(), true,  "1234"},
        { TestLine(), true,  "12345"},
        { TestLine(), true,  "123456"},
        { TestLine(), true,  "123456789012345"},
    };

    [Theory]
    [MemberData(nameof(TelephoneNumberData))]
    public void TestTelephoneNumber(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.TelephoneNumber, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> TelephoneNumberE164Data => new() {
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
        { TestLine(), true,  "+1234"},
        { TestLine(), true,  "+123456"},
        { TestLine(), true,  "+123456789012345"},
        { TestLine(), false,  "+1234567890123456"},
    };

    [Theory]
    [MemberData(nameof(TelephoneNumberE164Data))]
    public void TestTelephoneNumberE164(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.TelephoneNumberE164, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }

    public static TheoryData<string, bool, string> TelephoneNumberExtraData => new() {
        { TestLine(), false, ""},
        { TestLine(), false, " "},
        { TestLine(), false, "  "},
        { TestLine(), false, "0"},
        { TestLine(), false, "+01"},
        { TestLine(), false, "012"},
        { TestLine(), false, "+0123"},
        { TestLine(), false, "1234567890123456"},
        { TestLine(), false, "+1234567890123456"},
        { TestLine(), true,  "1234"},
        { TestLine(), false, "+1234a-BC"},
        { TestLine(), false, "*1234"},
        { TestLine(), true,  "+1234"},
        { TestLine(), true,  "+123456"},
        { TestLine(), true,  "+123456789012345"},
        { TestLine(), true,  "123456789012345"},
        { TestLine(), true,  "1(234)567-89-01-23-45"},
        { TestLine(), true,  "+1(234)567-89-01-23-45"},
        { TestLine(), true,  "+1 (134) 567-890 23.45"},
        { TestLine(), true,  "+1 (034) 567-890 23.45"},
        { TestLine(), true,  "+1 (234) 567.89  01 (23 45)"},
        { TestLine(), true,  "----+1 (234) 567.89  01 ((23 45))   "},
        { TestLine(), false, "+1234567890123456"},
    };

    [Theory]
    [MemberData(nameof(TelephoneNumberExtraData))]
    public void TestTelephoneNumberExtra(string TestLine, bool shouldBe, string code)
    {
        var matches = base.RegexTest(Countries.TelephoneNumberExtra, TestLine, shouldBe, code);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);
    }
}
