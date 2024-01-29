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

    public static TheoryData<string, bool, string, string, string, string> WinPathnameData => new() {
        { TestLine(), false, "", "", "", "" },
        { TestLine(), false, " ", "", "", "" },
        { TestLine(), false, ".", "", "", "" },
        { TestLine(), false, "..", "", "", "" },
        { TestLine(), false, "con", "", "", "" },
        { TestLine(), false, "con.txt", "", "", "" },
        { TestLine(), false, "lpt1", "", "", "" },
        { TestLine(), false, "lpt1.txt", "", "", "" },
        { TestLine(), false, "a.", "", "", "" },
        { TestLine(), false, "a ", "", "", "" },
        { TestLine(), true, "a", "", "", "a" },
        { TestLine(), true, "C", "", "", "C" },
        { TestLine(), true, "ж", "", "", "ж" },
        { TestLine(), true, "Ж", "", "", "Ж" },
        { TestLine(), true, "ab", "", "", "ab" },
        { TestLine(), true, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm", "", "", "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm" },
        { TestLine(), true, "QWERTYUIOPASDFGHJKLZXCVBNM qwertyuiopasdfghjklzxcvbnm", "", "", "QWERTYUIOPASDFGHJKLZXCVBNM qwertyuiopasdfghjklzxcvbnm" },
        { TestLine(), true, "QWERTYUIOPASDFGHJKLZXCVBNM.qwertyuiopasdfghjklzxcvbnm", "", "", "QWERTYUIOPASDFGHJKLZXCVBNM.qwertyuiopasdfghjklzxcvbnm" },
        { TestLine(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm.", "", "", "" },
        { TestLine(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm ", "", "", "" },
        { TestLine(), true, boarderLengthName, "", "", boarderLengthName },
        { TestLine(), false, overBoardLengthName, "", "", "" },
        { TestLine(), true, "явертъуиопасдфгхйклзьцжбнмшщюч ЯВЕРТЪУИОПАСДФГХЙКЛЗЬЦЖБНМШЩЮЧ", "", "", "явертъуиопасдфгхйклзьцжбнмшщюч ЯВЕРТЪУИОПАСДФГХЙКЛЗЬЦЖБНМШЩЮЧ" },
        { TestLine(), false, @"a:com1", "", "", "" },
        { TestLine(), false, @"ъ:a", "", "", "" },
        { TestLine(), false, @"a:"+boarderLengthName, "a", "", boarderLengthName },
        { TestLine(), false, @"a:a.", "a", "", "a" },
        { TestLine(), false, @"a:a ", "a", "", "a" },
        { TestLine(), true, @"a:a", "a", "", "a" },
        { TestLine(), true, @"B:a", "B", "", "a" },
        { TestLine(), false, @"a:/com2", "", "", "" },
        { TestLine(), false, @"a:/com2.bin", "", "", "" },
        { TestLine(), false, @"a:", "", "", "" },
        { TestLine(), true, @"a:/a", "a", @"/", "a" },
        { TestLine(), true, @"a:\a", "a", @"\", "a" },
        { TestLine(), true, @"a:b/a", "a", "b", "a" },
        { TestLine(), true, @"a:./a", "a", ".", "a" },
        { TestLine(), true, @"a:../a", "a", "..", "a" },
        { TestLine(), false, @"a:lpt2/a", "", "", "" },
        { TestLine(), false, @"a:lpt2.txt/a", "", "", "" },
        { TestLine(), false, @"a:/b/lpt2/a", "", "", "" },
        { TestLine(), false, @"a:/b/lpt2.txt/a", "", "", "" },
        { TestLine(), false, @"a:/b/c/d/lpt2", "", "", "" },
        { TestLine(), false, @"a:/b/c/d/lpt2.txt", "", "", "" },
        { TestLine(), false, @"a:/b/c./d/lp", "", "", "" },
        { TestLine(), false, @"a:/b/c./d/lp.txt", "", "", "" },
        { TestLine(), false, @"a:/b/c /d/lp", "", "", "" },
        { TestLine(), false, @"a:/b/c /d/lp.txt", "", "", "" },
        { TestLine(), true, @"a:b\a", "a", @"b", "a" },
        { TestLine(), true, @"a:/bb/a", "a", @"/bb", "a" },
        { TestLine(), true, @"a:/bb/cc/aa.aa", "a", @"/bb/cc", "aa.aa" },
        { TestLine(), true, @"a:bb/cc/aa.aa", "a", @"bb/cc", "aa.aa" },
        { TestLine(), true, @"bb/cc/aa.aa", "", @"bb/cc", "aa.aa" },
        { TestLine(), true, @"a:/./a", "a", @"/.", "a" },
        { TestLine(), true, @"a:../../a", "a", @"../..", "a" },
        { TestLine(), true, @"a:..\../a", "a", @"..\..", "a" },
        { TestLine(), true, @"a:\./a", "a", @"\.", "a" },
        { TestLine(), true, @"a:\bb\a", "a", @"\bb", "a" },
        { TestLine(), true, @"b\a", "", @"b", "a" },
        { TestLine(), true, @"/bb/a", "", @"/bb", "a" },
        { TestLine(), true, @"/bb/cc/aa.aa", "", @"/bb/cc", "aa.aa" },
        { TestLine(), true, @"/./a", "", @"/.", "a" },
        { TestLine(), true, @"../../a", "", @"../..", "a" },
        { TestLine(), true, @"..\../a", "", @"..\..", "a" },
        { TestLine(), true, @"\./a", "", @"\.", "a" },
        { TestLine(), true, @"\bb\a", "", @"\bb", "a" },
        { TestLine(), false, $@"a:\{boarderLengthName}\a", "", "", "" },
        { TestLine(), false, $@"\{boarderLengthName}", "", "", "" },

        { TestLine(), false, $@"a:\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\012345678901234567890123456789012345678901.txt",
                                "", "", ""  },

        { TestLine(), true,  $@"a:\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\01234567890123456789012345678901234567890.txt",
                                "a", @"\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm",
                                    "01234567890123456789012345678901234567890.txt" },
    };

    [Theory]
    [MemberData(nameof(WinPathnameData))]
    public void TestWinPathname(string TestLine, bool shouldBe, string pathname, string drive, string path, string file)
        => base.RegexTest(WindowsPathname.Pathname, TestLine, shouldBe, pathname, drive, path, file);
}
