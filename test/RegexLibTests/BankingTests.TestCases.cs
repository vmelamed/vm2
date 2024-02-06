namespace vm2.RegexLibTests;

public partial class BankingTests
{
    public static TheoryData<string, bool, string, Captures?> AbaRoutingNumberRexData => new() {
        { TestLine(), false, "", null },
        { TestLine(), false, " ", null },
        { TestLine(), false,  "0", null },
        { TestLine(), false,  "12", null },
        { TestLine(), false,  "012", null },
        { TestLine(), false,  "01234567", null },
        { TestLine(), false,  "0123456701", null },
        { TestLine(), false,  "0123456701234567012345670123456701234567", null },
        { TestLine(), false,  "01a34b678", null },
        { TestLine(), false,  "-012345678", null },
        { TestLine(), false,  "-01234567", null },
        { TestLine(), true,   "012345678", null },
    };

    public static TheoryData<string, bool, string, Captures?> SwiftCodeData => new() {
        { TestLine(), false, "", null },
        { TestLine(), false, " ", null },
        { TestLine(), false,  "0", null },
        { TestLine(), false,  "12", null },
        { TestLine(), false,  "012", null },
        { TestLine(), false,  "01234567", null },
        { TestLine(), false,  "0123456701", null },
        { TestLine(), false,  "0123456701234567012345670123456701234567", null },
        { TestLine(), false,  "01a34b678", null },
        { TestLine(), false,  "-012345678", null },
        { TestLine(), false,  "-01234567", null },
        { TestLine(), false,   "012345678", null },
        { TestLine(), false,  "BANK", null },
        { TestLine(), false,  "BANKUS", null },
        { TestLine(), false,  "BANkUSNJ", null },
        { TestLine(), false,  "BANKuSNJ", null },
        { TestLine(), false,  "BANKUSnJ", null },
        { TestLine(), false,  "BANKUSNJaBC", null },
        { TestLine(), false,  "bankusnjab3", null },
        { TestLine(), false,  "BANKUSNJABCDEF", null },
        { TestLine(), true,   "BANKUSNJABC", new()
                                                {
                                                    ["bank"] = "BANK",
                                                    ["country"] = "US",
                                                    ["location"] = "NJ",
                                                    ["branch"] = "ABC",
                                                } },
        { TestLine(), true,   "BANKUSNJAB1", new()
                                                {
                                                    ["bank"] = "BANK",
                                                    ["country"] = "US",
                                                    ["location"] = "NJ",
                                                    ["branch"] = "AB1",
                                                } },
        { TestLine(), true,   "BANKUSNJ321", new()
                                                {
                                                    ["bank"] = "BANK",
                                                    ["country"] = "US",
                                                    ["location"] = "NJ",
                                                    ["branch"] = "321",
                                                } },
    };

    public static TheoryData<string, bool, string, Captures?> IbanData => new() {
        { TestLine(), false, "", null },
        { TestLine(), false, " ", null },
        { TestLine(), false,  "0", null },
        { TestLine(), false,  "12", null },
        { TestLine(), false,  "012", null },
        { TestLine(), false,  "XY12 134 ABCD WXYZ A4B5 2222 123", null },
        { TestLine(), false,  "XY12 12345 ABCD WXYZ A4B5 2222 123", null },
        { TestLine(), false,  "XY12 1@%4 ABCD WXYZ A4B5 2222 123", null },
        { TestLine(), false,  "12XY 1234 ABCD WXYZ A4B5 2222 123", null },
        { TestLine(), false,  "XY12 1234 ABCD WXY", null },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ", new()
                                                                    {
                                                                        ["bank"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234 ABCD WXYZ"
                                                                    }
        },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ A4B5 1", new()
                                                                    {
                                                                        ["bank"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"] = "1234 ABCD WXYZ A4B5 1",
                                                                    } },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ A4B5 12", new()
                                                                    {
                                                                        ["bank"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"] = "1234 ABCD WXYZ A4B5 12",
                                                                    } },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ A4B5 2222 123", new()
                                                                    {
                                                                        ["bank"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"] = "1234 ABCD WXYZ A4B5 2222 123",
                                                                    } },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ A4B5 2222123", new()
                                                                    {
                                                                        ["bank"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"] = "1234 ABCD WXYZ A4B5 2222123",
                                                                    } },
        { TestLine(), true,   "XY121234ABCDWXYZA4B52222123", new()
                                                                    {
                                                                        ["bank"] = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"] = "1234ABCDWXYZA4B52222123",
                                                                    } },
    };

    public static TheoryData<string, bool, string, Captures?> IbanIData => new() {
        { TestLine(), false, "", null },
        { TestLine(), false, " ", null },
        { TestLine(), false,  "0", null },
        { TestLine(), false,  "12", null },
        { TestLine(), false,  "012", null },
        { TestLine(), false,  "12XY 1234 ABCD WXYZ A4B5 2222 123", null },
        { TestLine(), false,  "12XY 1234 ABCD wxyz A4B5 2222 123", null },
        { TestLine(), false,  "XY12 1234 ABCD WXY", null },
        { TestLine(), false,  "xy12 1234 ABCD WXY", null },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ", new()
                                                                    {
                                                                        ["bank"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234 ABCD WXYZ",
                                                                    } },
        { TestLine(), true,   "xy12 1234 ABCD WXYZ", new()
                                                                    {
                                                                        ["bank"]  = "xy",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234 ABCD WXYZ",
                                                                    } },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ A4B5 1", new()
                                                                    {
                                                                        ["bank"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234 ABCD WXYZ A4B5 1",
                                                                    } },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ A4B5 2222123", new()
                                                                    {
                                                                        ["bank"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234 ABCD WXYZ A4B5 2222123",
                                                                    } },
        { TestLine(), true,   "XY12 1234 ABCD WXYZ a4b5 2222123", new()
                                                                    {
                                                                        ["bank"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234 ABCD WXYZ a4b5 2222123",
                                                                    } },
        { TestLine(), true,   "XY121234ABCDWXYZA4B52222123", new()
                                                                    {
                                                                        ["bank"]  = "XY",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234ABCDWXYZA4B52222123",
                                                                    } },
        { TestLine(), true,   "xy121234abcdwxyza4b52222123", new()
                                                                    {
                                                                        ["bank"]  = "xy",
                                                                        ["check"] = "12",
                                                                        ["accn"]  = "1234abcdwxyza4b52222123",
                                                                    } },
    };
}
