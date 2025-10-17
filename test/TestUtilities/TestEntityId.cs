namespace vm2.TestUtilities;

using vm2;

public static class TestEntityId
{
    static UlidFactory _ulidFactory = new();
    static AsyncLocal<Guid> _current = new();
    static AsyncLocal<List<Guid>> _ids = new();

    public static Guid Next()
    {
        _current.Value = new Guid(_ulidFactory.NewUlid().Bytes);
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
