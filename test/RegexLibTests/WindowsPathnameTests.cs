namespace vm2.RegexLibTests;

public class WindowsPathnameTests(ITestOutputHelper output) : RegexTests(output)
{
    static readonly string boarderLengthName = new('a', 260);
    static readonly string overBoardLengthName = new('a', 261);

    public static TheoryData<string, bool, string> WinFilenameData => new() {
        { TestLine(), false, "" },
        { TestLine(), false, " " },
        { TestLine(), false, "." },
        { TestLine(), false, ".." },
        { TestLine(), false, "con" },
        { TestLine(), false, "con.txt" },
        { TestLine(), false, "lpt1" },
        { TestLine(), false, "lpt1.txt" },
        { TestLine(), true,  "a" },
        { TestLine(), true,  "C" },
        { TestLine(), true,  "ж" },
        { TestLine(), true,  "Ж" },
        { TestLine(), true,  "ab" },
        { TestLine(), true,  "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm" },
        { TestLine(), true,  "QWERTYUIOPASDFGHJKLZXCVBNM qwertyuiopasdfghjklzxcvbnm" },
        { TestLine(), true,  "QWERTYUIOPASDFGHJKLZXCVBNM.qwertyuiopasdfghjklzxcvbnm" },
        { TestLine(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm." },
        { TestLine(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm " },
        { TestLine(), true,  boarderLengthName },
        { TestLine(), false, overBoardLengthName },
        { TestLine(), true,  "явертъуиопасдфгхйклзьцжбнмшщюч ЯВЕРТЪУИОПАСДФГХЙКЛЗЬЦЖБНМШЩЮЧ" },
    };

    [Theory]
    [MemberData(nameof(WinFilenameData))]
    public void TestWinFilename(string TestLine, bool shouldBe, string fileName)
        => base.RegexTest(WindowsPathname.DiskFilename, TestLine, shouldBe, fileName);

    public static TheoryData<string, bool, string, Captures?> WinPathnameData => new() {
        { TestLine(), false, "", null },
        { TestLine(), false, " ", null },
        { TestLine(), false, ".", null },
        { TestLine(), false, "..", null },
        { TestLine(), false, "con", null },
        { TestLine(), false, "con.txt", null },
        { TestLine(), false, "lpt1", null },
        { TestLine(), false, "lpt1.txt", null },
        { TestLine(), false, "a.", null },
        { TestLine(), false, "a ", null },
        { TestLine(), true, "a", new() { ["drive"] = "", ["path"] = "", ["file"] = "a" } },
        { TestLine(), true, "C", new() { ["drive"] = "", ["path"] = "", ["file"] = "C" } },
        { TestLine(), true, "ж", new() { ["drive"] = "", ["path"] = "", ["file"] = "ж" } },
        { TestLine(), true, "Ж", new() { ["drive"] = "", ["path"] = "", ["file"] = "Ж" } },
        { TestLine(), true, "ab", new() { ["drive"] = "", ["path"] = "", ["file"] = "ab" } },
        { TestLine(), true, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm", new() { ["drive"] = "", ["path"] = "", ["file"] = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm" } },
        { TestLine(), true, "QWERTYUIOPASDFGHJKLZXCVBNM qwertyuiopasdfghjklzxcvbnm", new() { ["drive"] = "", ["path"] = "", ["file"] = "QWERTYUIOPASDFGHJKLZXCVBNM qwertyuiopasdfghjklzxcvbnm" } },
        { TestLine(), true, "QWERTYUIOPASDFGHJKLZXCVBNM.qwertyuiopasdfghjklzxcvbnm", new() { ["drive"] = "", ["path"] = "", ["file"] = "QWERTYUIOPASDFGHJKLZXCVBNM.qwertyuiopasdfghjklzxcvbnm" } },
        { TestLine(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm.", null },
        { TestLine(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm ", null },
        { TestLine(), true, boarderLengthName, new() { ["drive"] = "", ["path"] = "", ["file"] = boarderLengthName } },
        { TestLine(), false, overBoardLengthName, null },
        { TestLine(), true, "явертъуиопасдфгхйклзьцжбнмшщюч ЯВЕРТЪУИОПАСДФГХЙКЛЗЬЦЖБНМШЩЮЧ", new() { ["drive"] = "", ["path"] = "", ["file"] = "явертъуиопасдфгхйклзьцжбнмшщюч ЯВЕРТЪУИОПАСДФГХЙКЛЗЬЦЖБНМШЩЮЧ" } },
        { TestLine(), false, @"a:com1", null },
        { TestLine(), false, @"ъ:a", null },
        { TestLine(), false, @"a:"+boarderLengthName, new() { ["drive"] = "a", ["path"] = "", ["file"] = boarderLengthName } },
        { TestLine(), false, @"a:a.", null },
        { TestLine(), false, @"a:a ", null },
        { TestLine(), true, @"a:a", new() { ["drive"] = "a", ["path"] = "", ["file"] = "a" } },
        { TestLine(), true, @"B:a", new() { ["drive"] = "B", ["path"] = "", ["file"] = "a" } },
        { TestLine(), false, @"a:/com2", null },
        { TestLine(), false, @"a:/com2.bin", null },
        { TestLine(), false, @"a:", null },
        { TestLine(), true, @"a:/a", new() { ["drive"] = "a", ["path"] = @"/", ["file"] = "a" } },
        { TestLine(), true, @"a:\a", new() { ["drive"] = "a", ["path"] = @"\", ["file"] = "a" } },
        { TestLine(), true, @"a:b/a", new() { ["drive"] = "a", ["path"] = "b", ["file"] = "a" } },
        { TestLine(), true, @"a:./a", new() { ["drive"] = "a", ["path"] = ".", ["file"] = "a" } },
        { TestLine(), true, @"a:../a", new() { ["drive"] = "a", ["path"] = "..", ["file"] = "a" } },
        { TestLine(), false, @"a:lpt2/a", null },
        { TestLine(), false, @"a:lpt2.txt/a", null },
        { TestLine(), false, @"a:/b/lpt2/a", null },
        { TestLine(), false, @"a:/b/lpt2.txt/a", null },
        { TestLine(), false, @"a:/b/c/d/lpt2", null },
        { TestLine(), false, @"a:/b/c/d/lpt2.txt", null },
        { TestLine(), false, @"a:/b/c./d/lp", null },
        { TestLine(), false, @"a:/b/c./d/lp.txt", null },
        { TestLine(), false, @"a:/b/c /d/lp", null },
        { TestLine(), false, @"a:/b/c /d/lp.txt", null },
        { TestLine(), true, @"a:b\a", new() { ["drive"] = "a", ["path"] = @"b", ["file"] = "a" } },
        { TestLine(), true, @"a:/bb/a", new() { ["drive"] = "a", ["path"] = @"/bb", ["file"] = "a" } },
        { TestLine(), true, @"a:/bb/cc/aa.aa", new() { ["drive"] = "a", ["path"] = @"/bb/cc", ["file"] = "aa.aa" } },
        { TestLine(), true, @"a:bb/cc/aa.aa", new() { ["drive"] = "a", ["path"] = @"bb/cc", ["file"] = "aa.aa" } },
        { TestLine(), true, @"bb/cc/aa.aa", new() { ["drive"] = "", ["path"] = @"bb/cc", ["file"] = "aa.aa" } },
        { TestLine(), true, @"a:/./a", new() { ["drive"] = "a", ["path"] = @"/.", ["file"] = "a" } },
        { TestLine(), true, @"a:../../a", new() { ["drive"] = "a", ["path"] = @"../..", ["file"] = "a" } },
        { TestLine(), true, @"a:..\../a", new() { ["drive"] = "a", ["path"] = @"..\..", ["file"] = "a" } },
        { TestLine(), true, @"a:\./a", new() { ["drive"] = "a", ["path"] = @"\.", ["file"] = "a" } },
        { TestLine(), true, @"a:\bb\a", new() { ["drive"] = "a", ["path"] = @"\bb", ["file"] = "a" } },
        { TestLine(), true, @"b\a", new() { ["drive"] = "", ["path"] = @"b", ["file"] = "a" } },
        { TestLine(), true, @"/bb/a", new() { ["drive"] = "", ["path"] = @"/bb", ["file"] = "a" } },
        { TestLine(), true, @"/bb/cc/aa.aa", new() { ["drive"] = "", ["path"] = @"/bb/cc", ["file"] = "aa.aa" } },
        { TestLine(), true, @"/./a", new() { ["drive"] = "", ["path"] = @"/.", ["file"] = "a" } },
        { TestLine(), true, @"../../a", new() { ["drive"] = "", ["path"] = @"../..", ["file"] = "a" } },
        { TestLine(), true, @"..\../a", new() { ["drive"] = "", ["path"] = @"..\..", ["file"] = "a" } },
        { TestLine(), true, @"\./a", new() { ["drive"] = "", ["path"] = @"\.", ["file"] = "a" } },
        { TestLine(), true, @"\bb\a", new() { ["drive"] = "", ["path"] = @"\bb", ["file"] = "a" } },
        { TestLine(), false, $@"a:\{boarderLengthName}\a", null },
        { TestLine(), false, $@"\{boarderLengthName}", null },
        { TestLine(), false, $@"a:\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\012345678901234567890123456789012345678901.txt",
                                null  },

        { TestLine(), true,  $@"a:\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\01234567890123456789012345678901234567890.txt",
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
