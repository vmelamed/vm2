namespace vm2.RegexLibTests;

public class LinuxPathnameTests : RegexTests
{
    public LinuxPathnameTests(ITestOutputHelper output) : base(output)
    {
    }

    static readonly string longestName = new('a', 255);

    public static TheoryData<string, bool, string, string, string> LinuxPathnameData => new() {
        { TestLine(), false, "", "", "" },
        { TestLine(), false, "/", "", "" },
        { TestLine(), true,  "a", "", "a" },
        { TestLine(), true,  $"{longestName}", "", $"{longestName}" },
        { TestLine(), false, $"{longestName+'a'}", "", "" },
        { TestLine(), true,  $"/a", "/", "a" },
        { TestLine(), true,  $"/a/b", "/a", "b" },
        { TestLine(), true,  $"/{longestName}", "/", $"{longestName}" },
        { TestLine(), false, $"/{longestName+'a'}", "", "" },
        { TestLine(), true,  $"/{longestName}/{longestName}", $"/{longestName}", $"{longestName}" },
        { TestLine(), true,  "./some/file", "./some", "file" },
        { TestLine(), true,  "../another/file", "../another", "file" },
        { TestLine(), true,  "/yet/../another/file", "/yet/../another", "file" },
        { TestLine(), true,  "/yet/../another/.file", "/yet/../another", ".file" },
        { TestLine(), true,  "/някакъв/../друг/.файл", "/някакъв/../друг", ".файл" },
    };

    [Theory]
    [MemberData(nameof(LinuxPathnameData))]
    public void TestLinuxPathname(string TestLine, bool shouldBe, string pathname, string path, string file)
        => base.RegexTest(LinuxPathname.Pathname, TestLine, shouldBe, pathname, path, file);
}
