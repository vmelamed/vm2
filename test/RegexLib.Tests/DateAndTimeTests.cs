namespace vm2.RegexLibTests;

public class DateAndTimeTests(
    RegexLibTestsFixture fixture,
    ITestOutputHelper output) : RegexTests(fixture, output)
{
    public static TheoryData<string, bool, string, Captures?> DateTimeData => new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), true, "2023-02-30T12:00:00Z", new()
                                                                {
                                                                    ["year"] = "2023",
                                                                    ["month"] = "02",
                                                                    ["day"] = "30",
                                                                    ["hour"] = "12",
                                                                    ["minute"] = "00",
                                                                    ["second"] = "00",
                                                                    ["offset"] = "Z",
                                                                } }, // invalid day
        { TestFileLine(), true , "2023-02-28T12:00:00Z", new()
                                                                {
                                                                    ["year"] = "2023",
                                                                    ["month"] = "02",
                                                                    ["day"] = "28",
                                                                    ["hour"] = "12",
                                                                    ["minute"] = "00",
                                                                    ["second"] = "00",
                                                                    ["offset"] = "Z",
                                                                } }, // valid leap year
        { TestFileLine(), true , "2020-02-29T23:59:59Z", new()
                                                                {
                                                                    ["year"] = "2020",
                                                                    ["month"] = "02",
                                                                    ["day"] = "29",
                                                                    ["hour"] = "23",
                                                                    ["minute"] = "59",
                                                                    ["second"] = "59",
                                                                    ["offset"] = "Z",
                                                                } }, // valid leap day
        { TestFileLine(), true,  "2021-02-29T23:59:59Z", new()
                                                                {
                                                                    ["year"] = "2021",
                                                                    ["month"] = "02",
                                                                    ["day"] = "29",
                                                                    ["hour"] = "23",
                                                                    ["minute"] = "59",
                                                                    ["second"] = "59",
                                                                    ["offset"] = "Z",
                                                                } }, // invalid leap day
        { TestFileLine(), true , "1999-12-31T23:59:59+00:00", new()
                                                                {
                                                                    ["year"] = "1999",
                                                                    ["month"] = "12",
                                                                    ["day"] = "31",
                                                                    ["hour"] = "23",
                                                                    ["minute"] = "59",
                                                                    ["second"] = "59",
                                                                    ["offset"] = "+00:00",
                                                                    ["numOffset"] = "+00:00",
                                                                } }, // valid offset
        { TestFileLine(), true , "1999-12-31T23:59:59-01:30", new()
                                                                {
                                                                    ["year"] = "1999",
                                                                    ["month"] = "12",
                                                                    ["day"] = "31",
                                                                    ["hour"] = "23",
                                                                    ["minute"] = "59",
                                                                    ["second"] = "59",
                                                                    ["offset"] = "-01:30",
                                                                    ["numOffset"] = "-01:30",
                                                                } }, // valid negative offset
        { TestFileLine(), true , "1999-12-31T23:59:59+0130", new()
                                                                {
                                                                    ["year"] = "1999",
                                                                    ["month"] = "12",
                                                                    ["day"] = "31",
                                                                    ["hour"] = "23",
                                                                    ["minute"] = "59",
                                                                    ["second"] = "59",
                                                                    ["offset"] = "+0130",
                                                                    ["numOffset"] = "+0130",
                                                                } }, // valid offset without colon
        { TestFileLine(), true , "1999-12-31T23:59:59.123Z", new()
                                                                {
                                                                    ["year"] = "1999",
                                                                    ["month"] = "12",
                                                                    ["day"] = "31",
                                                                    ["hour"] = "23",
                                                                    ["minute"] = "59",
                                                                    ["second"] = "59",
                                                                    ["fracSecond"] = ".123",
                                                                    ["offset"] = "Z",
                                                                } }, // valid with fraction
        { TestFileLine(), true , "1999-12-31 23:59:59Z", new()
                                                                {
                                                                    ["year"] = "1999",
                                                                    ["month"] = "12",
                                                                    ["day"] = "31",
                                                                    ["hour"] = "23",
                                                                    ["minute"] = "59",
                                                                    ["second"] = "59",
                                                                    ["offset"] = "Z",
                                                                } }, // valid with space
        { TestFileLine(), false, "1999-12-31T24:00:00Z", null }, // invalid hour
        { TestFileLine(), false, "1999-12-31T23:60:00Z", null }, // invalid minute
        { TestFileLine(), false, "1999-12-31T23:59:60Z", null }, // invalid second
        { TestFileLine(), false, "1999-12-31T23:59:59+25:00", null }, // invalid offset hour
        { TestFileLine(), false, "1999-12-31T23:59:59+01:60", null }, // invalid offset minute
        { TestFileLine(), false, "1999-12-31T23:59:59", null }, // missing offset
        { TestFileLine(), false, "1999-12-31T23:59Z", null }, // missing seconds
        { TestFileLine(), false, "1999-12-31T23:59:59.123", null }, // missing offset
        { TestFileLine(), false, "1999-13-31T23:59:59Z", null }, // invalid month
        { TestFileLine(), false, "1999-00-31T23:59:59Z", null }, // invalid month
        { TestFileLine(), false, "1999-12-00T23:59:59Z", null }, // invalid day
        { TestFileLine(), false, "abcd-12-31T23:59:59Z", null }, // invalid year
    };

    [Theory]
    [MemberData(nameof(DateTimeData))]
    public void TestDateTime(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(DateAndTime.DateTime(), TestLine, shouldBe, input, captures);

    public static TheoryData<string, bool, string, Captures?> DurationData => new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), true , "P1Y2M3DT4H5M6S", new()
                                                                {
                                                                    ["date"] = "1Y2M3D",
                                                                    ["day"] = "3",
                                                                    ["month"] = "2",
                                                                    ["year"] = "1",
                                                                    ["time"] = "4H5M6S",
                                                                    ["hour"] = "4",
                                                                    ["minute"] = "5",
                                                                    ["second"] = "6",
                                                                } }, // full duration
        { TestFileLine(), true , "P1Y", new()
                                                                {
                                                                    ["date"] = "1Y",
                                                                    ["year"] = "1",
                                                                } }, // only years
        { TestFileLine(), true , "P2M", new()
                                                                {
                                                                    ["date"] = "2M",
                                                                    ["month"] = "2",
                                                                } }, // only months
        { TestFileLine(), true , "P3D", new()
                                                                {
                                                                    ["date"] = "3D",
                                                                    ["day"] = "3",
                                                                } }, // only days
        { TestFileLine(), true , "PT4H", new()
                                                                {
                                                                    ["time"] = "4H",
                                                                    ["hour"] = "4",
                                                                } }, // only hours
        { TestFileLine(), true , "PT5M", new()
                                                                {
                                                                    ["time"] = "5M",
                                                                    ["minute"] = "5",
                                                                } }, // only minutes
        { TestFileLine(), true , "PT6S", new()
                                                                {
                                                                    ["time"] = "6S",
                                                                    ["second"] = "6",
                                                                } }, // only seconds
        { TestFileLine(), true , "P1Y2M", new()
                                                                {
                                                                    ["date"] = "1Y2M",
                                                                    ["month"] = "2",
                                                                    ["year"] = "1",
                                                                } }, // year and month
        { TestFileLine(), true , "P1DT2H", new()
                                                                {
                                                                    ["date"] = "1D",
                                                                    ["day"] = "1",
                                                                    ["time"] = "2H",
                                                                    ["hour"] = "2",
                                                                } }, // day and hour
        { TestFileLine(), true , "P1W", new()
                                                                {
                                                                    ["week"] = "1",
                                                                } }, // week
        { TestFileLine(), false, "P", null }, // missing value
        { TestFileLine(), false, "PT", null }, // missing value
        { TestFileLine(), false, "P1Y2Q", null }, // invalid designator
        { TestFileLine(), false, "P-1Y", null }, // negative value not allowed
        { TestFileLine(), false, "P1Y2M3DT4H5M6", null }, // missing S
        { TestFileLine(), false, "P1Y2M3DT4H5M6S7S", null }, // duplicate S
        { TestFileLine(), false, "P1Y2M3DT4H5M6S7M", null }, // duplicate M
        { TestFileLine(), false, "P1Y2M3DT4H5M6S7H", null }, // duplicate H
        { TestFileLine(), false, "P1Y2M3DT4H5M6S7D", null }, // duplicate D
        { TestFileLine(), false, "P1Y2M3DT4H5M6S7W", null }, // week with date
        { TestFileLine(), false, "P1Y2M3DT4H5M6S7Y", null }, // duplicate Y
        { TestFileLine(), false, "P1Y2M3DT4H5M6S7T", null }, // invalid T
    };

    [Theory]
    [MemberData(nameof(DurationData))]
    public void TestDuration(string TestLine, bool shouldBe, string input, Captures? captures)
        => base.RegexTest(DateAndTime.Duration(), TestLine, shouldBe, input, captures);
}