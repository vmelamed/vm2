namespace vm2.RegexLibTests;

public class LinuxPathnameTests : RegexTests
{
    public LinuxPathnameTests(ITestOutputHelper output) : base(output)
    {
    }

    static readonly string longestName = new string('a', 255);

    public static TheoryData<string, bool, string, string, string> LinuxPathnameData => new TheoryData<string, bool, string, string, string>
    {
        { TestLine(), false, "", "", "" },
        { TestLine(), false, "/", "", "" },
        { TestLine(), true,  "a", "a", "" },
        { TestLine(), true,  $"{longestName}", $"{longestName}", "" },
        { TestLine(), false, $"{longestName+'a'}", "", "" },
        { TestLine(), true,  $"/a", "a", "/" },
        { TestLine(), true,  $"/a/b", "b", "/a" },
        { TestLine(), true,  $"/{longestName}", $"{longestName}", "/" },
        { TestLine(), false, $"/{longestName+'a'}", "", "" },
        { TestLine(), true,  $"/{longestName}/{longestName}", $"{longestName}", $"/{longestName}" },
        { TestLine(), true,  "./some/file", "file", "./some" },
        { TestLine(), true,  "../another/file", "file", "../another" },
        { TestLine(), true,  "/yet/../another/file", "file", "/yet/../another" },
        { TestLine(), true,  "/yet/../another/.file", ".file", "/yet/../another" },
        { TestLine(), true,  "/някакъв/../друг/.файл", ".файл", "/някакъв/../друг" },
    };

    [Theory]
    [MemberData(nameof(LinuxPathnameData))]
    public void TestLinuxPathname(string TestLine, bool shouldBe, string pathname, string file, string path)
    {
        var matches = base.RegexTest(LinuxPathname.Pathname, TestLine, shouldBe, pathname);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);

        var gPath = matches.First().Groups [LinuxPathname.PathGr].Value;
        var gFile = matches.First().Groups [LinuxPathname.FileGr].Value;

        gPath.Should().Be(path);
        gFile.Should().Be(file.Length > 0 ? file : pathname);

        Out.WriteLine($"""
                         Path:     →{gPath}←
                         File:     →{gFile}←
                       """);
    }
}
