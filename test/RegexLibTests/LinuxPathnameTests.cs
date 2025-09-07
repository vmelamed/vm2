namespace vm2.RegexLib.Tests;

public class LinuxPathnameTests(
    RegexLibTestsFixture fixture,
    ITestOutputHelper output) : RegexTests(fixture, output)
{
    static readonly string longestName = new('a', 255);

    public static TheoryData<string, bool, string, Captures?> LinuxPathnameData => new() {
        { TestFileLine("Empty string - should not match"), false, "", null },
        { TestFileLine("Only root slash - should not match"), false, "/", null },
        { TestFileLine("Single file name"), true,  "a", new() { ["path"] = "", ["file"] = "a" } },
        { TestFileLine("Single file name"), true, "a", new() { ["path"] = "", ["file"] = "a" } },
        { TestFileLine("Longest valid file name (255 chars)"), true,  $"{longestName}", new() { ["path"] = "", ["file"] = $"{longestName}" } },
        { TestFileLine("Too long file name (256 chars)"), false, $"{longestName+'a'}", null },
        { TestFileLine("Rooted single file"), true,  $"/a", new() { ["path"] = "/", ["file"] = "a" } },
        { TestFileLine("Rooted path with two segments"), true,  $"/a/b", new() { ["path"] = "/a", ["file"] = "b" } },
        { TestFileLine("Rooted path with longest valid file name"), true,  $"/{longestName}", new() { ["path"] = "/", ["file"] = $"{longestName}" } },
        { TestFileLine("Rooted path with too long file name"), false, $"/{longestName+'a'}", null },
        { TestFileLine("Rooted path with two longest valid segments"), true,  $"/{longestName}/{longestName}", new() { ["path"] = $"/{longestName}", ["file"] = $"{longestName}" } },
        { TestFileLine("Relative path with dot"), true,  "./some/file", new() { ["path"] = "./some", ["file"] = "file" } },
        { TestFileLine("Relative path with double dot"), true,  "../another/file", new() { ["path"] = "../another", ["file"] = "file" } },
        { TestFileLine("Path with parent directory reference"), true,  "/yet/../another/file", new() { ["path"] = "/yet/../another", ["file"] = "file" } },
        { TestFileLine("Path with hidden file"), true,  "/yet/../another/.file", new() { ["path"] = "/yet/../another", ["file"] = ".file" } },
        { TestFileLine("Unicode path and file"), true,  "/някакъв/../друг/.файл", new() { ["path"] = "/някакъв/../друг", ["file"] = ".файл" } },
        { TestFileLine("Path with invalid character (slash in file name)"), false, "abc/def/ghi/jkl/", null },
        { TestFileLine("Path with null character (should not match)"), false, "abc\0def", null },
        { TestFileLine("Path with only dots (should match as file)"), true, ".", new() { ["path"] = "", ["file"] = "." } },
        { TestFileLine("Path with only double dots (should match as file)"), true, "..", new() { ["path"] = "", ["file"] = ".." } },
        { TestFileLine("Path with trailing slash (should not match)"), false, "/abc/", null },
        { TestFileLine("Path with multiple slashes (should not match)"), false, "a//b", null },
        { TestFileLine("Path with space in file name"), true, "a b", new() { ["path"] = "", ["file"] = "a b" } },
        { TestFileLine("Path with dash and underscore"), true, "foo-bar_baz", new() { ["path"] = "", ["file"] = "foo-bar_baz" } },
        { TestFileLine("Path with leading slash and dot file"), true, "/.hidden", new() { ["path"] = "/", ["file"] = ".hidden" } },
        { TestFileLine("Path with only slashes (should not match)"), false, "////", null },
    };

    [Theory]
    [MemberData(nameof(LinuxPathnameData))]
    public void TestLinuxPathname(string TestLine, bool shouldBe, string pathname, Captures? captures)
        => base.RegexTest(LinuxPathname.Pathname(), TestLine, shouldBe, pathname, captures);
}
