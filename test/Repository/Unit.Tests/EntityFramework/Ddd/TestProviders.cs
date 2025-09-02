namespace vm2.Repository.UnitTests.EntityFramework.Ddd;

static class Clock
{
    public static DateTime Initial = new(2025, 01, 02, 03, 04, 05, DateTimeKind.Utc);
    public static readonly TimeSpan Step = new(0, 0, 1);

    public static DateTime Current { get; private set; } = Initial;

    public static DateTime Now() => Current += Step;

    public static DateTime Reset(DateTime? initial = null) => Current = initial ?? Clock.Initial;
}

static class Actor
{
    public static List<string> Actors { get; } = [];

    static string _current = "actor";

    public static string Current
    {
        get => _current;
        set
        {
            Actors.Add(_current);
            _current = value;
        }
    }

    public static string Reset(string actor)
    {
        Actors.Clear();
        _current = actor;
        return _current;
    }

    public static string Log() => Current;
}

static class TestTenant
{
    static char _init = '1';

    public static Guid Current { get; set; } = Guid.Parse(new string(_init, 32));

    public static Guid Tenant() => Current;

    public static Guid Next()
    {
        _init++;
        Current = Guid.Parse(new string(_init, 32));
        return Current;
    }

    public static Guid Reset()
    {
        _init = '1';
        Current = Guid.Parse(new string(_init, 32));
        return Current;
    }
}

