namespace vm2.Repository.TestData;

public static class Utilities
{
    public const string Caller  = "TestCaller";
    public const string Creator = "TestCreator";
    public const string Updater = "TestUpdater";
    public const string Deleter = "TestDeleter";

    /// <summary>
    /// Starting time window for the test data.
    /// </summary>
    public static readonly DateTime Dt0 = new (2025, 7, 22, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Represents a time increment for every time request.
    /// </summary>
    public static readonly TimeSpan TimeIncrement = TimeSpan.FromSeconds(1);

    static DateTime nextDt = Dt0;

    public static Lock SyncNextDt = new();

    public static DateTime CurrentDt
    {
        get { lock (SyncNextDt) return nextDt; }
    }

    public static DateTime NextDt
    {
        get { lock (SyncNextDt) return nextDt += TimeIncrement; }
    }

    public static DateTime Now => DateTime.UtcNow;

    public static string SeededString(string value) => value + DateTime.Now.Ticks;

    public static void ResetData()
    {
        NewLabels();
        NewInstruments();
    }
}
