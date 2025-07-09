namespace vm2.RegexLibTests;

public partial class BankingTests
{
    public static TheoryData<string, bool, string, Captures?> AbaRoutingNumberRexData => new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), false,  "0", null },
        { TestFileLine(), false,  "12", null },
        { TestFileLine(), false,  "012", null },
        { TestFileLine(), false,  "01234567", null },
        { TestFileLine(), false,  "0123456701", null },
        { TestFileLine(), false,  "0123456701234567012345670123456701234567", null },
        { TestFileLine(), false,  "01a34b678", null },
        { TestFileLine(), false,  "-012345678", null },
        { TestFileLine(), false,  "-01234567", null },
        { TestFileLine(), true,   "012345678", null },
    };

    public static TheoryData<string, bool, string, Captures?> SwiftCodeData => new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), false,  "0", null },
        { TestFileLine(), false,  "12", null },
        { TestFileLine(), false,  "012", null },
        { TestFileLine(), false,  "01234567", null },
        { TestFileLine(), false,  "0123456701", null },
        { TestFileLine(), false,  "0123456701234567012345670123456701234567", null },
        { TestFileLine(), false,  "01a34b678", null },
        { TestFileLine(), false,  "-012345678", null },
        { TestFileLine(), false,  "-01234567", null },
        { TestFileLine(), false,   "012345678", null },
        { TestFileLine(), false,  "BANK", null },
        { TestFileLine(), false,  "BANKUS", null },
        { TestFileLine(), false,  "BANkUSNJ", null },
        { TestFileLine(), false,  "BANKuSNJ", null },
        { TestFileLine(), false,  "BANKUSnJ", null },
        { TestFileLine(), false,  "BANKUSNJaBC", null },
        { TestFileLine(), false,  "bankusnjab3", null },
        { TestFileLine(), false,  "BANKUSNJABCDEF", null },
        { TestFileLine(), true,   "BANKUSNJABC", new()
                                                {
                                                    ["bank"] = "BANK",
                                                    ["country"] = "US",
                                                    ["location"] = "NJ",
                                                    ["branch"] = "ABC",
                                                } },
        { TestFileLine(), true,   "BANKUSNJAB1", new()
                                                {
                                                    ["bank"] = "BANK",
                                                    ["country"] = "US",
                                                    ["location"] = "NJ",
                                                    ["branch"] = "AB1",
                                                } },
        { TestFileLine(), true,   "BANKUSNJ321", new()
                                                {
                                                    ["bank"] = "BANK",
                                                    ["country"] = "US",
                                                    ["location"] = "NJ",
                                                    ["branch"] = "321",
                                                } },
    };

    public static TheoryData<string, bool, string, Captures?> IbanData => new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), false,  "0", null },
        { TestFileLine(), false,  "12", null },
        { TestFileLine(), false,  "012", null },
        { TestFileLine(), true,   "XY12 134 ABCD WXYZ A4B5 2222 123", new()
                                                                    {
                                                                        ["country"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"] = "134 ABCD WXYZ A4B5 2222 123",
                                                                    } },
        { TestFileLine(), true,   "XY12 12345 ABCD WXYZ A4B5 2222 123", new()
                                                                    {
                                                                        ["country"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"] = "12345 ABCD WXYZ A4B5 2222 123",
                                                                    } },
        { TestFileLine(), false,  "XY12 1@%4 ABCD WXYZ A4B5 2222 123", null },
        { TestFileLine(), false,  "12XY 1234 ABCD WXYZ A4B5 2222 123", null },
        { TestFileLine(), true,   "XY12 1234 ABCD WXY", new()
                                                                    {
                                                                        ["country"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"] = "1234 ABCD WXY",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ", new()
                                                                    {
                                                                        ["country"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"]  = "1234 ABCD WXYZ"
                                                                    }
        },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ A4B5 1", new()
                                                                    {
                                                                        ["country"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"] = "1234 ABCD WXYZ A4B5 1",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ A4B5 12", new()
                                                                    {
                                                                        ["country"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"] = "1234 ABCD WXYZ A4B5 12",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ A4B5 2222 123", new()
                                                                    {
                                                                        ["country"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"] = "1234 ABCD WXYZ A4B5 2222 123",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ A4B5 2222123", new()
                                                                    {
                                                                        ["country"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"] = "1234 ABCD WXYZ A4B5 2222123",
                                                                    } },
        { TestFileLine(), true,   "XY121234ABCDWXYZA4B52222123", new()
                                                                    {
                                                                        ["country"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"] = "1234ABCDWXYZA4B52222123",
                                                                    } },
    };

    public static TheoryData<string, bool, string, Captures?> IbanIData => new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), false,  "0", null },
        { TestFileLine(), false,  "12", null },
        { TestFileLine(), false,  "012", null },
        { TestFileLine(), false,  "12XY 1234 ABCD WXYZ A4B5 2222 123", null },
        { TestFileLine(), false,  "12XY 1234 ABCD wxyz A4B5 2222 123", null },
        { TestFileLine(), true,   "XY12 1234 ABCD WXY", new()
                                                                    {
                                                                        ["country"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"] = "1234 ABCD WXY",
                                                                    } },
        { TestFileLine(), true,   "xy12 1234 ABCD WXY", new()
                                                                    {
                                                                        ["country"] = "xy",
                                                                        ["check"] = "12",
                                                                        ["account"] = "1234 ABCD WXY",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ", new()
                                                                    {
                                                                        ["country"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"]  = "1234 ABCD WXYZ",
                                                                    } },
        { TestFileLine(), true,   "xy12 1234 ABCD WXYZ", new()
                                                                    {
                                                                        ["country"]  = "xy",
                                                                        ["check"] = "12",
                                                                        ["account"]  = "1234 ABCD WXYZ",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ A4B5 1", new()
                                                                    {
                                                                        ["country"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"]  = "1234 ABCD WXYZ A4B5 1",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ A4B5 2222123", new()
                                                                    {
                                                                        ["country"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"]  = "1234 ABCD WXYZ A4B5 2222123",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ a4b5 2222123", new()
                                                                    {
                                                                        ["country"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"]  = "1234 ABCD WXYZ a4b5 2222123",
                                                                    } },
        { TestFileLine(), true,   "XY121234ABCDWXYZA4B52222123", new()
                                                                    {
                                                                        ["country"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["account"]  = "1234ABCDWXYZA4B52222123",
                                                                    } },
        { TestFileLine(), true,   "xy121234abcdwxyza4b52222123", new()
                                                                    {
                                                                        ["country"]  = "xy",
                                                                        ["check"] = "12",
                                                                        ["account"]  = "1234abcdwxyza4b52222123",
                                                                    } },
        { TestFileLine(), false, "", null },
        { TestFileLine(), true,  "DE8937040044053201300",  new()
                                                                    {
                                                                        ["country"] = "DE",
                                                                        ["check"] = "89",
                                                                        ["account"] = "37040044053201300",
                                                                    } },   // not too short
        { TestFileLine(), true , "DE89370400440532013000", new()
                                                                    {
                                                                        ["country"] = "DE",
                                                                        ["check"] = "89",
                                                                        ["account"] = "370400440532013000",
                                                                    } },  // valid German IBAN
        { TestFileLine(), true, "DE893704004405320130000" , new()
                                                                    {
                                                                        ["country"] = "DE",
                                                                        ["check"] = "89",
                                                                        ["account"] = "3704004405320130000",
                                                                    } }, // not too long
        { TestFileLine(), false, "DE8937040044053201300003704004405320X" , null }, // too long
        { TestFileLine(), true, "DE89 3704 0044 0532 0130 00" , new()
                                                                    {
                                                                        ["country"] = "DE",
                                                                        ["check"] = "89",
                                                                        ["account"] = "3704 0044 0532 0130 00",
                                                                    } }, // spaces
        { TestFileLine(), false, "DE89-3704-0044-0532-0130-00" , null }, // dashes
        { TestFileLine(), true,  "DE89370400440532013O00" , new()
                                                                    {
                                                                        ["country"] = "DE",
                                                                        ["check"] = "89",
                                                                        ["account"] = "370400440532013O00",
                                                                    } },  // letter O instead of zero
        { TestFileLine(), false, "DE8937040044053201300!" , null },  // invalid char
    };

    public static TheoryData<string, bool, string> AbaRoutingNumberEdgeData => new() {
        { TestFileLine(), false, "" },
        { TestFileLine(), false, "12345678" },      // too short
        { TestFileLine(), true , "123456789" },     // valid
        { TestFileLine(), false, "1234567890" },    // too long
        { TestFileLine(), false, "12345678A" },     // non-digit
        { TestFileLine(), false, "A23456789" },     // non-digit
        { TestFileLine(), false, " 23456789" },     // leading space
        { TestFileLine(), false, "12345678 " },     // trailing space
    };

    public static TheoryData<string, bool, string, Captures?> SwiftCodeEdgeData => new() {
        { TestFileLine(), false, "" , null },
        { TestFileLine(), true, "DEUTDEFF5O0" , new()
                                                                    {
                                                                        ["bank"] = "DEUT",
                                                                        ["country"] = "DE",
                                                                        ["location"] = "FF",
                                                                        ["branch"] = "5O0",
                                                                    } },      // contains letter 'O' instead of zero
        { TestFileLine(), false, "DEUTDEFF5!" , null },       // invalid char
        { TestFileLine(), false, "DEUTDEFF5 0" , null },      // space
        { TestFileLine(), false, "DEUTDEFF5000" , null },     // too long
        { TestFileLine(), false, "DEUTDEFF5" , null },        // too short
        { TestFileLine(), true , "DEUTDEFF" , new()
                                                                    {
                                                                        ["bank"] = "DEUT",
                                                                        ["country"] = "DE",
                                                                        ["location"] = "FF",
                                                                    } },         // valid 8-char (no branch)
        { TestFileLine(), true , "DEUTDEFF500", new()
                                                                    {
                                                                        ["bank"] = "DEUT",
                                                                        ["country"] = "DE",
                                                                        ["location"] = "FF",
                                                                        ["branch"] = "500",
                                                                    } },      // valid 11-char
    };
}
