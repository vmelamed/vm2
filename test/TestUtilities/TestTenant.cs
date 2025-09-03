namespace vm2.TestUtilities;

public static class TestTenant
{
    static readonly char[] _guidChars = "0123456789abcdef-".ToCharArray();
    public static readonly Guid DefaultTenant = Guid.Parse(new string(_guidChars[0], 32));

    static AsyncLocal<int> index = new();
    static AsyncLocal<Guid> _current = new();

    public static Guid Next()
    {
        _current.Value = Guid.Parse(new string(_guidChars[++index.Value], 32));
        return Current();
    }

    public static Guid Current() => _current.Value;

    public static Guid Reset()
    {
        index.Value = 0;
        _current.Value = DefaultTenant;
        return Current();
    }

    public static IEnumerable<Guid> Actors
        => Enumerable
                .Range(0, _guidChars.Length)
                .Select(i => Guid.Parse(new string(_guidChars[i], 32)));
}
