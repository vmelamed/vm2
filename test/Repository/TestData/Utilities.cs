namespace vm2.Repository.TestData;

public static class Utilities
{
    public const string Caller  = "TestCaller";
    public const string Creator = "TestDataCreator";
    public const string Updater = "TestDataUpdater";
    public const string Deleter = "TestDataDeleter";

    /// <summary>
    /// Starting time window for the test data.
    /// </summary>
    public static readonly DateTimeOffset Dt0 = new (2025, 7, 22, 0, 0, 0, TimeSpan.FromHours(0));

    /// <summary>
    /// Represents a time increment for every time request.
    /// </summary>
    public static readonly TimeSpan TimeIncrement = TimeSpan.FromSeconds(1);

    static DateTimeOffset _nextDt = Dt0;

    public static Lock _syncNextDt = new();

    public static DateTimeOffset CurrentDt
    {
        get { lock (_syncNextDt) return _nextDt; }
    }

    public static DateTimeOffset NextDt
    {
        get { lock (_syncNextDt) return _nextDt += TimeIncrement; }
    }

    public static DateTimeOffset Now => DateTimeOffset.UtcNow;

    public static string SeededString(string value) => value + DateTime.Now.Ticks;

    public static void ResetData()
    {
        NewLabels();
        NewInstruments();
    }
}
