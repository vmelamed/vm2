namespace vm2.RegexLib;

/// <summary>
/// Class DateAndTime. Regular expressions for validating dates, time, intervals, etc.
/// Follows:
/// https://datatracker.ietf.org/doc/rfc3339/
/// </summary>
public static partial class DateAndTime
{
    #region DateTime ISO 8601/RFC3339
    const string dateFullYear = @"\d\d\d\d";

    const string dateMonth = "0[1-9]|1[0-2]";

    const string dateDay = "0[1-9]|[12][0-9]|3[0-1]";

    const string timeHour = "[01][0-9]|2[0-3]";

    const string timeMinute = "[0-5][0-9]";

    const string timeSecond = "[0-5][0-9]";

    const string secondFrac = @"\.\d+";

    const string numOffset = $"[-+](?:{timeHour})(?::?{timeMinute})?";

    /// <summary>
    /// The name of a matching group representing the date's full year.
    /// </summary>
    public const string FullYearGr = "year";
    /// <summary>
    /// The name of a matching group representing the date's month.
    /// </summary>
    public const string MonthGr = "month";
    /// <summary>
    /// The name of a matching group representing the date's day of the month.
    /// </summary>
    public const string DayGr = "day";
    /// <summary>
    /// The name of a matching group representing the date's hour.
    /// </summary>
    public const string HourGr = "hour";
    /// <summary>
    /// The name of a matching group representing the date's minute.
    /// </summary>
    public const string MinuteGr = "minute";
    /// <summary>
    /// The name of a matching group representing the date's second.
    /// </summary>
    public const string SecondGr = "second";
    /// <summary>
    /// The name of a matching group representing the date's fraction of a second.
    /// </summary>
    public const string FractionSecondGr = "fracSecond";
    /// <summary>
    /// The name of a matching group representing the time's offset from UTC.
    /// </summary>
    public const string OffsetGr = "offset";
    /// <summary>
    /// The name of a matching group representing the numeric time's offset from UTC in hours and minutes.
    /// </summary>
    public const string NumOffsetGr = "numOffset";

    /// <summary>
    /// Regular expression pattern which matches an ISO 8601 date and time representation in a string.
    /// </summary>
    public const string DateTimeRex = $@"(?<{FullYearGr}>{dateFullYear})-(?<{MonthGr}>{dateMonth})-(?<{DayGr}>{dateDay})[T ](?<{HourGr}>{timeHour}):(?<{MinuteGr}>{timeMinute}):(?<{SecondGr}>{timeSecond})(?<{FractionSecondGr}>{secondFrac})?(?<{OffsetGr}>Z|(?<{NumOffsetGr}>{numOffset}))";

    /// <summary>
    /// Regular expression pattern which matches a string that represents an ISO 8601 date and time representation.
    /// </summary>
    public const string DateTimeRegex = $@"^{DateTimeRex}$";

    /// <summary>
    /// Gets a Regex object which matches a string representing an ISO 8601 (case sensitive!) date and time representation.
    /// </summary>
    [GeneratedRegex(DateTimeRegex, Common.Options)]
    public static partial Regex DateTime();

    /// <summary>
    /// Gets a Regex object which matches a string representing an RFC 3339 (case insensitive) date and time representation.
    /// </summary>
    [GeneratedRegex(DateTimeRegex, Common.OptionsI)]
    public static partial Regex DateTimeI();
    #endregion

    #region Duration
    /// <summary>
    /// The name of a matching group representing the duration's seconds.
    /// </summary>
    public const string DurSecondGr = "second";
    /// <summary>
    /// The name of a matching group representing the duration's minutes.
    /// </summary>
    public const string DurMinuteGr = "minute";
    /// <summary>
    /// The name of a matching group representing the duration's hours.
    /// </summary>
    public const string DurHourGr   = "hour";
    /// <summary>
    /// The name of a matching group representing the duration time.
    /// </summary>
    public const string DurTimeGr   = "time";
    /// <summary>
    /// The name of a matching group representing the duration's days.
    /// </summary>
    public const string DurDayGr = "day";
    /// <summary>
    /// The name of a matching group representing the duration's week.
    /// </summary>
    public const string DurWeekGr = "week";
    /// <summary>
    /// The name of a matching group representing the duration's month.
    /// </summary>
    public const string DurMonthGr = "month";
    /// <summary>
    /// The name of a matching group representing the duration's years.
    /// </summary>
    public const string DurYearGr = "year";
    /// <summary>
    /// The name of a matching group representing the duration date period.
    /// </summary>
    public const string DurDateGr = "date";

    const string durSecond = @$"(?<{DurSecondGr}>\d+)S";
    const string durMinute = @$"(?<{DurMinuteGr}>\d+)M(?:{durSecond})?";
    const string durHour   = @$"(?<{DurHourGr}>\d+)H(?:{durMinute})?";

    const string durDay    = @$"(?<{DurDayGr}>\d+)D";
    const string durMonth  = @$"(?<{DurMonthGr}>\d+)M(?:{durDay})?";
    const string durYear   = @$"(?<{DurYearGr}>\d+)Y(?:{durMonth})?";

    const string durTime   = @$"T(?<{DurTimeGr}>(?:{durHour}|{durMinute}|{durSecond}))";
    const string durDate   = @$"(?<{DurDateGr}>{durDay}|{durMonth}|{durYear})(?:{durTime})?";
    const string durWeek   = @$"(?<{DurWeekGr}>\d+)W";

    /// <summary>
    /// Regular expression pattern which matches a time duration in a string.
    /// </summary>
    public const string DurationRex = @$"P(?:{durDate}|{durTime}|{durWeek})";

    /// <summary>
    /// Regular expression pattern which matches a string that represents a time duration .
    /// </summary>
    public const string DurationRegex = @$"^{DurationRex}$";

    /// <summary>
    /// Gets a Regex object which matches a string representing a time duration in ISO 8601 (case sensitive!).
    /// </summary>
    [GeneratedRegex(DurationRegex, Common.Options)]
    public static partial Regex Duration();

    /// <summary>
    /// Gets a Regex object which matches a string representing a time duration (case insensitive!).
    /// </summary>
    [GeneratedRegex(DurationRegex, Common.OptionsI)]
    public static partial Regex DurationI();
    #endregion
}
