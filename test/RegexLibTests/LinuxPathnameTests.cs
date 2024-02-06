namespace vm2.RegexLibTests;

public class LinuxPathnameTests(ITestOutputHelper output) : RegexTests(output)
{
    static readonly string longestName = new('a', 255);

    public static TheoryData<string, bool, string, Captures?> LinuxPathnameData => new() {
        { TestLine(), false, "", null },
        { TestLine(), false, "/", null },
        { TestLine(), true,  "a", new() { ["path"] = "", ["file"] = "a" } },
        { TestLine(), true,  $"{longestName}", new() { ["path"] = "", ["file"] = $"{longestName}" } },
        { TestLine(), false, $"{longestName+'a'}", null },
        { TestLine(), true,  $"/a", new() { ["path"] = "/", ["file"] = "a" } },
        { TestLine(), true,  $"/a/b", new() { ["path"] = "/a", ["file"] = "b" } },
        { TestLine(), true,  $"/{longestName}", new() { ["path"] = "/", ["file"] = $"{longestName}" } },
        { TestLine(), false, $"/{longestName+'a'}", null },
        { TestLine(), true,  $"/{longestName}/{longestName}", new() { ["path"] = $"/{longestName}", ["file"] = $"{longestName}" } },
        { TestLine(), true,  "./some/file", new() { ["path"] = "./some", ["file"] = "file" } },
        { TestLine(), true,  "../another/file", new() { ["path"] = "../another", ["file"] = "file" } },
        { TestLine(), true,  "/yet/../another/file", new() { ["path"] = "/yet/../another", ["file"] = "file" } },
        { TestLine(), true,  "/yet/../another/.file", new() { ["path"] = "/yet/../another", ["file"] = ".file" } },
        { TestLine(), true,  "/някакъв/../друг/.файл", new() { ["path"] = "/някакъв/../друг", ["file"] = ".файл" } },
    };

    [Theory]
    [MemberData(nameof(LinuxPathnameData))]
    public void TestLinuxPathname(string TestLine, bool shouldBe, string pathname, Captures? captures)
        => base.RegexTest(LinuxPathname.Pathname, TestLine, shouldBe, pathname, captures);
}
