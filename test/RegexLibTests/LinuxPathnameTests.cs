namespace vm2.RegexLibTests;

public class LinuxPathnameTests(ITestOutputHelper output) : RegexTests(output)
{
    static readonly string longestName = new('a', 255);

    public static TheoryData<string, bool, string, Captures?> LinuxPathnameData => new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, "/", null },
        { TestFileLine(), true,  "a", new() { ["path"] = "", ["file"] = "a" } },
        { TestFileLine(), true,  $"{longestName}", new() { ["path"] = "", ["file"] = $"{longestName}" } },
        { TestFileLine(), false, $"{longestName+'a'}", null },
        { TestFileLine(), true,  $"/a", new() { ["path"] = "/", ["file"] = "a" } },
        { TestFileLine(), true,  $"/a/b", new() { ["path"] = "/a", ["file"] = "b" } },
        { TestFileLine(), true,  $"/{longestName}", new() { ["path"] = "/", ["file"] = $"{longestName}" } },
        { TestFileLine(), false, $"/{longestName+'a'}", null },
        { TestFileLine(), true,  $"/{longestName}/{longestName}", new() { ["path"] = $"/{longestName}", ["file"] = $"{longestName}" } },
        { TestFileLine(), true,  "./some/file", new() { ["path"] = "./some", ["file"] = "file" } },
        { TestFileLine(), true,  "../another/file", new() { ["path"] = "../another", ["file"] = "file" } },
        { TestFileLine(), true,  "/yet/../another/file", new() { ["path"] = "/yet/../another", ["file"] = "file" } },
        { TestFileLine(), true,  "/yet/../another/.file", new() { ["path"] = "/yet/../another", ["file"] = ".file" } },
        { TestFileLine(), true,  "/някакъв/../друг/.файл", new() { ["path"] = "/някакъв/../друг", ["file"] = ".файл" } },
    };

    [Theory]
    [MemberData(nameof(LinuxPathnameData))]
    public void TestLinuxPathname(string TestLine, bool shouldBe, string pathname, Captures? captures)
        => base.RegexTest(LinuxPathname.Pathname(), TestLine, shouldBe, pathname, captures);
}
