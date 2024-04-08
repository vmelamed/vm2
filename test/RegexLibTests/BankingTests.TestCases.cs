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
        { TestFileLine(), false,  "XY12 134 ABCD WXYZ A4B5 2222 123", null },
        { TestFileLine(), false,  "XY12 12345 ABCD WXYZ A4B5 2222 123", null },
        { TestFileLine(), false,  "XY12 1@%4 ABCD WXYZ A4B5 2222 123", null },
        { TestFileLine(), false,  "12XY 1234 ABCD WXYZ A4B5 2222 123", null },
        { TestFileLine(), false,  "XY12 1234 ABCD WXY", null },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ", new()
                                                                    {
                                                                        ["bank"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234 ABCD WXYZ"
                                                                    }
        },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ A4B5 1", new()
                                                                    {
                                                                        ["bank"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"] = "1234 ABCD WXYZ A4B5 1",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ A4B5 12", new()
                                                                    {
                                                                        ["bank"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"] = "1234 ABCD WXYZ A4B5 12",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ A4B5 2222 123", new()
                                                                    {
                                                                        ["bank"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"] = "1234 ABCD WXYZ A4B5 2222 123",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ A4B5 2222123", new()
                                                                    {
                                                                        ["bank"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"] = "1234 ABCD WXYZ A4B5 2222123",
                                                                    } },
        { TestFileLine(), true,   "XY121234ABCDWXYZA4B52222123", new()
                                                                    {
                                                                        ["bank"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"] = "1234ABCDWXYZA4B52222123",
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
        { TestFileLine(), false,  "XY12 1234 ABCD WXY", null },
        { TestFileLine(), false,  "xy12 1234 ABCD WXY", null },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ", new()
                                                                    {
                                                                        ["bank"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234 ABCD WXYZ",
                                                                    } },
        { TestFileLine(), true,   "xy12 1234 ABCD WXYZ", new()
                                                                    {
                                                                        ["bank"]  = "xy",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234 ABCD WXYZ",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ A4B5 1", new()
                                                                    {
                                                                        ["bank"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234 ABCD WXYZ A4B5 1",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ A4B5 2222123", new()
                                                                    {
                                                                        ["bank"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234 ABCD WXYZ A4B5 2222123",
                                                                    } },
        { TestFileLine(), true,   "XY12 1234 ABCD WXYZ a4b5 2222123", new()
                                                                    {
                                                                        ["bank"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234 ABCD WXYZ a4b5 2222123",
                                                                    } },
        { TestFileLine(), true,   "XY121234ABCDWXYZA4B52222123", new()
                                                                    {
                                                                        ["bank"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234ABCDWXYZA4B52222123",
                                                                    } },
        { TestFileLine(), true,   "xy121234abcdwxyza4b52222123", new()
                                                                    {
                                                                        ["bank"]  = "xy",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234abcdwxyza4b52222123",
                                                                    } },
    };
}
