namespace vm2.RegexLibTests;

public class WindowsPathnameTests(ITestOutputHelper output) : RegexTests(output)
{
    static readonly string boarderLengthName = new('a', 260);
    static readonly string overBoardLengthName = new('a', 261);

    public static TheoryData<string, bool, string> WinFilenameData => new() {
        { TestFileLine(), false, "" },
        { TestFileLine(), false, " " },
        { TestFileLine(), false, "." },
        { TestFileLine(), false, ".." },
        { TestFileLine(), false, "con" },
        { TestFileLine(), false, "con.txt" },
        { TestFileLine(), false, "lpt1" },
        { TestFileLine(), false, "lpt1.txt" },
        { TestFileLine(), true,  "a" },
        { TestFileLine(), true,  "C" },
        { TestFileLine(), true,  "ж" },
        { TestFileLine(), true,  "Ж" },
        { TestFileLine(), true,  "ab" },
        { TestFileLine(), true,  "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm" },
        { TestFileLine(), true,  "QWERTYUIOPASDFGHJKLZXCVBNM qwertyuiopasdfghjklzxcvbnm" },
        { TestFileLine(), true,  "QWERTYUIOPASDFGHJKLZXCVBNM.qwertyuiopasdfghjklzxcvbnm" },
        { TestFileLine(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm." },
        { TestFileLine(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm " },
        { TestFileLine(), true,  boarderLengthName },
        { TestFileLine(), false, overBoardLengthName },
        { TestFileLine(), true,  "явертъуиопасдфгхйклзьцжбнмшщюч ЯВЕРТЪУИОПАСДФГХЙКЛЗЬЦЖБНМШЩЮЧ" },
    };

    [Theory]
    [MemberData(nameof(WinFilenameData))]
    public void TestWinFilename(string TestLine, bool shouldBe, string fileName)
        => base.RegexTest(WindowsPathname.DiskFilename, TestLine, shouldBe, fileName);

    public static TheoryData<string, bool, string, Captures?> WinPathnameData => new() {
        { TestFileLine(), false, "", null },
        { TestFileLine(), false, " ", null },
        { TestFileLine(), false, ".", null },
        { TestFileLine(), false, "..", null },
        { TestFileLine(), false, "con", null },
        { TestFileLine(), false, "con.txt", null },
        { TestFileLine(), false, "lpt1", null },
        { TestFileLine(), false, "lpt1.txt", null },
        { TestFileLine(), false, "a.", null },
        { TestFileLine(), false, "a ", null },
        { TestFileLine(), true, "a", new() { ["drive"] = "", ["path"] = "", ["file"] = "a" } },
        { TestFileLine(), true, "C", new() { ["drive"] = "", ["path"] = "", ["file"] = "C" } },
        { TestFileLine(), true, "ж", new() { ["drive"] = "", ["path"] = "", ["file"] = "ж" } },
        { TestFileLine(), true, "Ж", new() { ["drive"] = "", ["path"] = "", ["file"] = "Ж" } },
        { TestFileLine(), true, "ab", new() { ["drive"] = "", ["path"] = "", ["file"] = "ab" } },
        { TestFileLine(), true, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm", new() { ["drive"] = "", ["path"] = "", ["file"] = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm" } },
        { TestFileLine(), true, "QWERTYUIOPASDFGHJKLZXCVBNM qwertyuiopasdfghjklzxcvbnm", new() { ["drive"] = "", ["path"] = "", ["file"] = "QWERTYUIOPASDFGHJKLZXCVBNM qwertyuiopasdfghjklzxcvbnm" } },
        { TestFileLine(), true, "QWERTYUIOPASDFGHJKLZXCVBNM.qwertyuiopasdfghjklzxcvbnm", new() { ["drive"] = "", ["path"] = "", ["file"] = "QWERTYUIOPASDFGHJKLZXCVBNM.qwertyuiopasdfghjklzxcvbnm" } },
        { TestFileLine(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm.", null },
        { TestFileLine(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm ", null },
        { TestFileLine(), true, boarderLengthName, new() { ["drive"] = "", ["path"] = "", ["file"] = boarderLengthName } },
        { TestFileLine(), false, overBoardLengthName, null },
        { TestFileLine(), true, "явертъуиопасдфгхйклзьцжбнмшщюч ЯВЕРТЪУИОПАСДФГХЙКЛЗЬЦЖБНМШЩЮЧ", new() { ["drive"] = "", ["path"] = "", ["file"] = "явертъуиопасдфгхйклзьцжбнмшщюч ЯВЕРТЪУИОПАСДФГХЙКЛЗЬЦЖБНМШЩЮЧ" } },
        { TestFileLine(), false, @"a:com1", null },
        { TestFileLine(), false, @"ъ:a", null },
        { TestFileLine(), false, @"a:"+boarderLengthName, new() { ["drive"] = "a", ["path"] = "", ["file"] = boarderLengthName } },
        { TestFileLine(), false, @"a:a.", null },
        { TestFileLine(), false, @"a:a ", null },
        { TestFileLine(), true, @"a:a", new() { ["drive"] = "a", ["path"] = "", ["file"] = "a" } },
        { TestFileLine(), true, @"B:a", new() { ["drive"] = "B", ["path"] = "", ["file"] = "a" } },
        { TestFileLine(), false, @"a:/com2", null },
        { TestFileLine(), false, @"a:/com2.bin", null },
        { TestFileLine(), false, @"a:", null },
        { TestFileLine(), true, @"a:/a", new() { ["drive"] = "a", ["path"] = @"/", ["file"] = "a" } },
        { TestFileLine(), true, @"a:\a", new() { ["drive"] = "a", ["path"] = @"\", ["file"] = "a" } },
        { TestFileLine(), true, @"a:b/a", new() { ["drive"] = "a", ["path"] = "b", ["file"] = "a" } },
        { TestFileLine(), true, @"a:./a", new() { ["drive"] = "a", ["path"] = ".", ["file"] = "a" } },
        { TestFileLine(), true, @"a:../a", new() { ["drive"] = "a", ["path"] = "..", ["file"] = "a" } },
        { TestFileLine(), false, @"a:lpt2/a", null },
        { TestFileLine(), false, @"a:lpt2.txt/a", null },
        { TestFileLine(), false, @"a:/b/lpt2/a", null },
        { TestFileLine(), false, @"a:/b/lpt2.txt/a", null },
        { TestFileLine(), false, @"a:/b/c/d/lpt2", null },
        { TestFileLine(), false, @"a:/b/c/d/lpt2.txt", null },
        { TestFileLine(), false, @"a:/b/c./d/lp", null },
        { TestFileLine(), false, @"a:/b/c./d/lp.txt", null },
        { TestFileLine(), false, @"a:/b/c /d/lp", null },
        { TestFileLine(), false, @"a:/b/c /d/lp.txt", null },
        { TestFileLine(), true, @"a:b\a", new() { ["drive"] = "a", ["path"] = @"b", ["file"] = "a" } },
        { TestFileLine(), true, @"a:/bb/a", new() { ["drive"] = "a", ["path"] = @"/bb", ["file"] = "a" } },
        { TestFileLine(), true, @"a:/bb/cc/aa.aa", new() { ["drive"] = "a", ["path"] = @"/bb/cc", ["file"] = "aa.aa" } },
        { TestFileLine(), true, @"a:bb/cc/aa.aa", new() { ["drive"] = "a", ["path"] = @"bb/cc", ["file"] = "aa.aa" } },
        { TestFileLine(), true, @"bb/cc/aa.aa", new() { ["drive"] = "", ["path"] = @"bb/cc", ["file"] = "aa.aa" } },
        { TestFileLine(), true, @"a:/./a", new() { ["drive"] = "a", ["path"] = @"/.", ["file"] = "a" } },
        { TestFileLine(), true, @"a:../../a", new() { ["drive"] = "a", ["path"] = @"../..", ["file"] = "a" } },
        { TestFileLine(), true, @"a:..\../a", new() { ["drive"] = "a", ["path"] = @"..\..", ["file"] = "a" } },
        { TestFileLine(), true, @"a:\./a", new() { ["drive"] = "a", ["path"] = @"\.", ["file"] = "a" } },
        { TestFileLine(), true, @"a:\bb\a", new() { ["drive"] = "a", ["path"] = @"\bb", ["file"] = "a" } },
        { TestFileLine(), true, @"b\a", new() { ["drive"] = "", ["path"] = @"b", ["file"] = "a" } },
        { TestFileLine(), true, @"/bb/a", new() { ["drive"] = "", ["path"] = @"/bb", ["file"] = "a" } },
        { TestFileLine(), true, @"/bb/cc/aa.aa", new() { ["drive"] = "", ["path"] = @"/bb/cc", ["file"] = "aa.aa" } },
        { TestFileLine(), true, @"/./a", new() { ["drive"] = "", ["path"] = @"/.", ["file"] = "a" } },
        { TestFileLine(), true, @"../../a", new() { ["drive"] = "", ["path"] = @"../..", ["file"] = "a" } },
        { TestFileLine(), true, @"..\../a", new() { ["drive"] = "", ["path"] = @"..\..", ["file"] = "a" } },
        { TestFileLine(), true, @"\./a", new() { ["drive"] = "", ["path"] = @"\.", ["file"] = "a" } },
        { TestFileLine(), true, @"\bb\a", new() { ["drive"] = "", ["path"] = @"\bb", ["file"] = "a" } },
        { TestFileLine(), false, $@"a:\{boarderLengthName}\a", null },
        { TestFileLine(), false, $@"\{boarderLengthName}", null },
        { TestFileLine(), false, $@"a:\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\012345678901234567890123456789012345678901.txt",
                                null  },

        { TestFileLine(), true,  $@"a:\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\01234567890123456789012345678901234567890.txt",
                             new()
                             {
                                ["drive"] = "a",
                                ["path"] = @"\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm",
                                ["file"] = "01234567890123456789012345678901234567890.txt" }
                             },
    };

    [Theory]
    [MemberData(nameof(WinPathnameData))]
    public void TestWinPathname(string TestLine, bool shouldBe, string pathname, Captures? captures)
        => base.RegexTest(WindowsPathname.Pathname, TestLine, shouldBe, pathname, captures);
}
