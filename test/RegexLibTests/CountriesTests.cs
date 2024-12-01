namespace vm2.RegexLibTests;

public class CountriesTests(ITestOutputHelper output) : RegexTests(output)
{
    public static TheoryData<string, bool, string> CountryCode2Data => new() {
        { TestFileLine(), false, ""},
        { TestFileLine(), false, " "},
        { TestFileLine(), false, "  "},
        { TestFileLine(), false, "a"},
        { TestFileLine(), false, "ab"},
        { TestFileLine(), false, "abc"},
        { TestFileLine(), false, "a1"},
        { TestFileLine(), false, "1a"},
        { TestFileLine(), false, "12"},
        { TestFileLine(), false, "A"},
        { TestFileLine(), false, "A1"},
        { TestFileLine(), false, "A="},
        { TestFileLine(), false, "1A"},
        { TestFileLine(), false, "@A"},
        { TestFileLine(), false, "ABC"},
        { TestFileLine(), true,  "AB"},
    };

    [Theory]
    [MemberData(nameof(CountryCode2Data))]
    public void TestCountryCode2(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.CountryCode2(), TestLine, shouldBe, code);

    public static TheoryData<string, bool, string> CountryCode2IData => new() {
        { TestFileLine(), false, ""},
        { TestFileLine(), false, " "},
        { TestFileLine(), false, "  "},
        { TestFileLine(), false, "a"},
        { TestFileLine(), true,  "ab"},
        { TestFileLine(), false, "abc"},
        { TestFileLine(), false, "a1"},
        { TestFileLine(), false, "a/"},
        { TestFileLine(), false, "1a"},
        { TestFileLine(), false, "-a"},
        { TestFileLine(), false, "12"},
        { TestFileLine(), false, "A"},
        { TestFileLine(), false, "-1"},
        { TestFileLine(), false, "A1"},
        { TestFileLine(), false, "A$"},
        { TestFileLine(), false, "1A"},
        { TestFileLine(), false, "|A"},
        { TestFileLine(), false, "ABC"},
        { TestFileLine(), true,  "AB"},
    };

    [Theory]
    [MemberData(nameof(CountryCode2IData))]
    public void TestCountryCode2I(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.CountryCode2I(), TestLine, shouldBe, code);

    public static TheoryData<string, bool, string> CountryCode3Data => new() {
        { TestFileLine(), false, ""},
        { TestFileLine(), false, " "},
        { TestFileLine(), false, "  "},
        { TestFileLine(), false, "a"},
        { TestFileLine(), false, "ab"},
        { TestFileLine(), false, "abc"},
        { TestFileLine(), false, "a1"},
        { TestFileLine(), false, "1a"},
        { TestFileLine(), false, "13"},
        { TestFileLine(), false, "A"},
        { TestFileLine(), false, "A1"},
        { TestFileLine(), false, "A="},
        { TestFileLine(), false, "1A"},
        { TestFileLine(), false, "@A"},
        { TestFileLine(), true,  "ABC"},
        { TestFileLine(), false, "AB"},
    };

    [Theory]
    [MemberData(nameof(CountryCode3Data))]
    public void TestCountryCode3(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.CountryCode3(), TestLine, shouldBe, code);

    public static TheoryData<string, bool, string> CountryCode3IData => new() {
        { TestFileLine(), false, ""},
        { TestFileLine(), false, " "},
        { TestFileLine(), false, "  "},
        { TestFileLine(), false, "a"},
        { TestFileLine(), false, "ab"},
        { TestFileLine(), true,  "abc"},
        { TestFileLine(), false, "a1"},
        { TestFileLine(), false, "a/"},
        { TestFileLine(), false, "1a"},
        { TestFileLine(), false, "-a"},
        { TestFileLine(), false, "13"},
        { TestFileLine(), false, "A"},
        { TestFileLine(), false, "-1"},
        { TestFileLine(), false, "A1"},
        { TestFileLine(), false, "A$"},
        { TestFileLine(), false, "1A"},
        { TestFileLine(), false, "|A"},
        { TestFileLine(), true,  "ABC"},
        { TestFileLine(), false, "AB"},
    };

    [Theory]
    [MemberData(nameof(CountryCode3IData))]
    public void TestCountryCode3I(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.CountryCode3I(), TestLine, shouldBe, code);

    public static TheoryData<string, bool, string> CurrencyCodeData => new() {
        { TestFileLine(), false, ""},
        { TestFileLine(), false, " "},
        { TestFileLine(), false, "  "},
        { TestFileLine(), false, "a"},
        { TestFileLine(), false, "ab"},
        { TestFileLine(), false, "abc"},
        { TestFileLine(), false, "a1"},
        { TestFileLine(), false, "1a"},
        { TestFileLine(), false, "13"},
        { TestFileLine(), false, "A"},
        { TestFileLine(), false, "A1"},
        { TestFileLine(), false, "A="},
        { TestFileLine(), false, "1A"},
        { TestFileLine(), false, "@A"},
        { TestFileLine(), true,  "ABC"},
        { TestFileLine(), false, "AB"},
    };

    [Theory]
    [MemberData(nameof(CurrencyCodeData))]
    public void TestCurrencyCode(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.CurrencyCode(), TestLine, shouldBe, code);

    public static TheoryData<string, bool, string> CurrencyCodeIData => new() {
        { TestFileLine(), false, ""},
        { TestFileLine(), false, " "},
        { TestFileLine(), false, "  "},
        { TestFileLine(), false, "a"},
        { TestFileLine(), false, "ab"},
        { TestFileLine(), true,  "abc"},
        { TestFileLine(), false, "a1"},
        { TestFileLine(), false, "a/"},
        { TestFileLine(), false, "1a"},
        { TestFileLine(), false, "-a"},
        { TestFileLine(), false, "13"},
        { TestFileLine(), false, "A"},
        { TestFileLine(), false, "-1"},
        { TestFileLine(), false, "A1"},
        { TestFileLine(), false, "A$"},
        { TestFileLine(), false, "1A"},
        { TestFileLine(), false, "|A"},
        { TestFileLine(), true,  "ABC"},
        { TestFileLine(), false, "AB"},
    };

    [Theory]
    [MemberData(nameof(CurrencyCodeIData))]
    public void TestCurrencyCodeI(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.CurrencyCodeI(), TestLine, shouldBe, code);

    public static TheoryData<string, bool, string> TelephoneCodeData => new() {
        { TestFileLine(), false, ""},
        { TestFileLine(), false, " "},
        { TestFileLine(), false, "  "},
        { TestFileLine(), false, "0"},
        { TestFileLine(), false, "01"},
        { TestFileLine(), false, "012"},
        { TestFileLine(), true,  "1"},
        { TestFileLine(), true,  "12"},
        { TestFileLine(), true,  "103"},
        { TestFileLine(), true,  "120"},
        { TestFileLine(), false, "1234"},
        { TestFileLine(), false, "a"},
        { TestFileLine(), false, "A"},
        { TestFileLine(), false, "AB"},
        { TestFileLine(), false, "ABC"},
    };

    [Theory]
    [MemberData(nameof(TelephoneCodeData))]
    public void TestTelephoneCode(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.TelephoneCode(), TestLine, shouldBe, code);

    public static TheoryData<string, bool, string> TelephoneNumberData => new() {
        { TestFileLine(), false, ""},
        { TestFileLine(), false, " "},
        { TestFileLine(), false, "  "},
        { TestFileLine(), false, "0"},
        { TestFileLine(), false, "01"},
        { TestFileLine(), false, "012"},
        { TestFileLine(), false, "1234567890123456"},
        { TestFileLine(), true,  "1234"},
        { TestFileLine(), true,  "12345"},
        { TestFileLine(), true,  "123456"},
        { TestFileLine(), true,  "123456789012345"},
    };

    [Theory]
    [MemberData(nameof(TelephoneNumberData))]
    public void TestTelephoneNumber(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.TelephoneNumber(), TestLine, shouldBe, code);

    public static TheoryData<string, bool, string> TelephoneNumberE164Data => new() {
        { TestFileLine(), false, ""},
        { TestFileLine(), false, " "},
        { TestFileLine(), false, "  "},
        { TestFileLine(), false, "0"},
        { TestFileLine(), false, "+01"},
        { TestFileLine(), false, "012"},
        { TestFileLine(), false, "+0123"},
        { TestFileLine(), false, "1234567890123456"},
        { TestFileLine(), false, "+1234567890123456"},
        { TestFileLine(), false, "1234"},
        { TestFileLine(), false, "+1234a-BC"},
        { TestFileLine(), false, "*1234"},
        { TestFileLine(), true,  "+1234"},
        { TestFileLine(), true,  "+123456"},
        { TestFileLine(), true,  "+123456789012345"},
        { TestFileLine(), false,  "+1234567890123456"},
    };

    [Theory]
    [MemberData(nameof(TelephoneNumberE164Data))]
    public void TestTelephoneNumberE164(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.TelephoneNumberE164(), TestLine, shouldBe, code);

    public static TheoryData<string, bool, string> TelephoneNumberExtraData => new() {
        { TestFileLine(), false, ""},
        { TestFileLine(), false, " "},
        { TestFileLine(), false, "  "},
        { TestFileLine(), false, "0"},
        { TestFileLine(), false, "+01"},
        { TestFileLine(), false, "012"},
        { TestFileLine(), false, "+0123"},
        { TestFileLine(), false, "1234567890123456"},
        { TestFileLine(), false, "+1234567890123456"},
        { TestFileLine(), true,  "1234"},
        { TestFileLine(), false, "+1234a-BC"},
        { TestFileLine(), false, "*1234"},
        { TestFileLine(), true,  "+1234"},
        { TestFileLine(), true,  "+123456"},
        { TestFileLine(), true,  "+123456789012345"},
        { TestFileLine(), true,  "123456789012345"},
        { TestFileLine(), true,  "1(234)567-89-01-23-45"},
        { TestFileLine(), true,  "+1(234)567-89-01-23-45"},
        { TestFileLine(), true,  "+1 (134) 567-890 23.45"},
        { TestFileLine(), true,  "+1 (034) 567-890 23.45"},
        { TestFileLine(), true,  "+1 (234) 567.89  01 (23 45)"},
        { TestFileLine(), true,  "----+1 (234) 567.89  01 ((23 45))   "},
        { TestFileLine(), false, "+1234567890123456"},
    };

    [Theory]
    [MemberData(nameof(TelephoneNumberExtraData))]
    public void TestTelephoneNumberExtra(string TestLine, bool shouldBe, string code)
        => base.RegexTest(Countries.TelephoneNumberExtra(), TestLine, shouldBe, code);
}
