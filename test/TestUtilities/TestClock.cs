namespace vm2.TestUtilities;

/// <summary>
/// A simple clock that starts at a fixed point in time and increments by a fixed step on each call to <see cref="Now"/>.
/// </summary>
public static class TestClock
{
    public static DateTime DefaultInitial = new(2025, 01, 02, 03, 04, 05, DateTimeKind.Utc);
    public static readonly TimeSpan DefaultStep = new(0, 0, 1);

    static AsyncLocal<int> _ticks = new(changed);

    private static void changed(AsyncLocalValueChangedArgs<int> args)
    {
        if (args.ThreadContextChanged && args.PreviousValue != 0)
        {
            // If we changed threads and the previous value was not the initial value, we need to reset
            _ticks.Value = args.PreviousValue;
        }
    }

    public static DateTime Reset()
    {
        _ticks.Value = 0;
        return Current;
    }

    public static DateTime Current => DefaultInitial + _ticks.Value * DefaultStep;

    public static DateTime Now()
    {
        var dt = Current;
        _ticks.Value++;
        return dt;
    }

    public static IEnumerable<DateTime> Times
        => Enumerable
                .Range(0, _ticks.Value)
                .Select(i => DefaultInitial + i*DefaultStep);
}
