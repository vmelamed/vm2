namespace vm2.TestUtilities;

public static class TestEntityId
{
    static AsyncLocal<List<Guid>> _ids = new();
    static AsyncLocal<Guid> _current = new();

    public static Guid Next()
    {
        _current.Value = Ulid.NewUlid().ToGuid();
        _ids.Value?.Add(_current.Value);
        return _current.Value;
    }

    public static Guid Current() => _current.Value;

    public static void Reset()
    {
        _ids.Value = [];
        _current.Value = Guid.Empty;
    }

    public static IEnumerable<Guid> Ids
        => _ids.Value!;
}
