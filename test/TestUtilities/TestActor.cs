namespace vm2.TestUtilities;

public static class TestActor
{
    public const string DefaultActor = "defaultActor";

    static AsyncLocal<List<string>> actors { get; set; } = new();

    public static string Next(string? next)
    {
        var actor = string.IsNullOrWhiteSpace(next) ? $"{DefaultActor}{actors.Value!.Count + 1}" : next;
        actors.Value!.Add(actor);
        return actor;
    }

    public static string Current() => actors.Value!.LastOrDefault(DefaultActor);

    public static string Reset(string? first = null)
    {
        actors.Value = string.IsNullOrWhiteSpace(first) ? [] : [first];
        return Current();
    }

    public static IEnumerable<string> Actors => actors.Value!.AsEnumerable();
}
