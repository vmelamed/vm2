using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using vm2.tests.RegexLibTests;

namespace RegexLibTests;

public class FileNameTests : RegexTests
{
    static readonly string boarderLengthName = new string('a', 260);
    static readonly string overBoardLengthName = new string('a', 261);

    public FileNameTests(ITestOutputHelper output) : base(output) { }

    public static TheoryData<string, bool, string> WinFilenameData => new TheoryData<string, bool, string>
    {
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
        => base.RegexTest(WindowsFileNames.DiskFilename, TestLine, shouldBe, fileName);

    public static TheoryData<string, bool, string, string, string, string> WinPathnameData => new TheoryData<string, bool, string, string, string, string>
    {
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
        { TestLine(), true,  "a", "", "", "" },
        { TestLine(), true,  "C", "", "", "" },
        { TestLine(), true,  "ж", "", "", "" },
        { TestLine(), true,  "Ж", "", "", "" },
        { TestLine(), true,  "ab", "", "", "" },
        { TestLine(), true,  "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm", "", "", "" },
        { TestLine(), true,  "QWERTYUIOPASDFGHJKLZXCVBNM qwertyuiopasdfghjklzxcvbnm", "", "", "" },
        { TestLine(), true,  "QWERTYUIOPASDFGHJKLZXCVBNM.qwertyuiopasdfghjklzxcvbnm", "", "", "" },
        { TestLine(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm.", "", "", "" },
        { TestLine(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm ", "", "", "" },
        { TestLine(), true,  boarderLengthName, "", "", "" },
        { TestLine(), false, overBoardLengthName, "", "", "" },
        { TestLine(), true,  "явертъуиопасдфгхйклзьцжбнмшщюч ЯВЕРТЪУИОПАСДФГХЙКЛЗЬЦЖБНМШЩЮЧ", "", "", "" },
                        //   pathname:                   filename:   path:           drive:
        { TestLine(), false,  @"a:com1",                   "",         "",             ""  },
        { TestLine(), false,  @"ъ:a",                      "",         "",             ""  },
        { TestLine(), false,  @"a:"+boarderLengthName,     "a",        "",             "a" },
        { TestLine(), false,  @"a:a.",                     "a",        "",             "a" },
        { TestLine(), false,  @"a:a ",                     "a",        "",             "a" },
        { TestLine(), true,   @"a:a",                      "a",        "",             "a" },
        { TestLine(), true,   @"B:a",                      "a",        "",             "B" },
        { TestLine(), false,  @"a:/com2",                  "",         "",             ""  },
        { TestLine(), false,  @"a:/com2.bin",              "",         "",             ""  },
        { TestLine(), false,  @"a:",                       "",         "",             ""  },
        { TestLine(), true,   @"a:/a",                     "a",        @"/",           "a" },
        { TestLine(), true,   @"a:\a",                     "a",        @"\",           "a" },
        { TestLine(), true,   @"a:b/a",                    "a",        "b",            "a" },
        { TestLine(), true,   @"a:./a",                    "a",        ".",            "a" },
        { TestLine(), true,   @"a:../a",                   "a",        "..",           "a" },
        { TestLine(), false,  @"a:lpt2/a",                 "",         "",             ""  },
        { TestLine(), false,  @"a:lpt2.txt/a",             "",         "",             ""  },
        { TestLine(), false,  @"a:/b/lpt2/a",              "",         "",             ""  },
        { TestLine(), false,  @"a:/b/lpt2.txt/a",          "",         "",             ""  },
        { TestLine(), false,  @"a:/b/c/d/lpt2",            "",         "",             ""  },
        { TestLine(), false,  @"a:/b/c/d/lpt2.txt",        "",         "",             ""  },
        { TestLine(), false,  @"a:/b/c./d/lp",             "",         "",             ""  },
        { TestLine(), false,  @"a:/b/c./d/lp.txt",         "",         "",             ""  },
        { TestLine(), false,  @"a:/b/c /d/lp",             "",         "",             ""  },
        { TestLine(), false,  @"a:/b/c /d/lp.txt",         "",         "",             ""  },
        { TestLine(), true,   @"a:b\a",                    "a",        @"b",           "a" },
        { TestLine(), true,   @"a:/bb/a",                  "a",        @"/bb",         "a" },
        { TestLine(), true,   @"a:/bb/cc/aa.aa",           "aa.aa",    @"/bb/cc",      "a" },
        { TestLine(), true,   @"a:bb/cc/aa.aa",            "aa.aa",    @"bb/cc",       "a" },
        { TestLine(), true,   @"bb/cc/aa.aa",              "aa.aa",    @"bb/cc",       ""  },
        { TestLine(), true,   @"a:/./a",                   "a",        @"/.",          "a" },
        { TestLine(), true,   @"a:../../a",                "a",        @"../..",       "a" },
        { TestLine(), true,   @"a:..\../a",                "a",        @"..\..",       "a" },
        { TestLine(), true,   @"a:\./a",                   "a",        @"\.",          "a" },
        { TestLine(), true,   @"a:\bb\a",                  "a",        @"\bb",         "a" },
        { TestLine(), true,   @"b\a",                      "a",        @"b",           ""  },
        { TestLine(), true,   @"/bb/a",                    "a",        @"/bb",         ""  },
        { TestLine(), true,   @"/bb/cc/aa.aa",             "aa.aa",    @"/bb/cc",      ""  },
        { TestLine(), true,   @"/./a",                     "a",        @"/.",          ""  },
        { TestLine(), true,   @"../../a",                  "a",        @"../..",       ""  },
        { TestLine(), true,   @"..\../a",                  "a",        @"..\..",       ""  },
        { TestLine(), true,   @"\./a",                     "a",        @"\.",          ""  },
        { TestLine(), true,   @"\bb\a",                    "a",        @"\bb",         ""  },
        { TestLine(), false, $@"a:\{boarderLengthName}\a", "",         "",             ""  },
        { TestLine(), false, $@"\{boarderLengthName}",     "",         "",             ""  },

        { TestLine(), false, $@"a:\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\012345678901234567890123456789012345678901.txt",
                                                            "",         "",             ""  },
        
        { TestLine(), true,  $@"a:\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\01234567890123456789012345678901234567890.txt",
                                                            "01234567890123456789012345678901234567890.txt",
                                                                        @"\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm",
                                                                                        "a" },
    };

    [Theory]
    [MemberData(nameof(WinPathnameData))]
    public void TestWinPathname(string TestLine, bool shouldBe, string pathname, string file, string path, string drive)
    {
        var matches = base.RegexTest(WindowsFileNames.Pathname, TestLine, shouldBe, pathname);

        if (matches.Count == 0)
            return;

        matches.Should().HaveCount(1);

        var gDrive = matches.First().Groups[WindowsFileNames.G_DRIVE].Value;
        var gPath = matches.First().Groups[WindowsFileNames.G_PATH].Value;
        var gFile = matches.First().Groups[WindowsFileNames.G_FILE].Value;

        gDrive.Should().Be(drive);
        gPath.Should().Be(path);
        gFile.Should().Be(file.Length > 0 ? file : pathname);

        Out.WriteLine($"""
                         Drive:    →{gDrive}←
                         Path:     →{gPath}←
                         File:     →{gFile}←
                       """);

    }
}
