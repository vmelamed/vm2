using System.Text.RegularExpressions;

using vm2.tests.RegexLibTests;

namespace RegexLibTests;

public class FileNameTests : RegexTests
{
    static readonly string boarderLengthName = new string('a', 260);
    static readonly string overBoardLengthName = new string('a', 261);

    public FileNameTests(ITestOutputHelper output) : base(output) { }

    public static TheoryData<string, bool, string> WinFilenameData
    {
        get
        {
            var i = 0;
            string testId() => i++.ToString("d2");

            return new TheoryData<string, bool, string>
            {
                { testId(), false, "" },
                { testId(), false, " " },
                { testId(), false, "." },
                { testId(), false, ".." },
                { testId(), false, "con" },
                { testId(), false, "con.txt" },
                { testId(), false, "lpt1" },
                { testId(), false, "lpt1.txt" },
                { testId(), true,  "a" },
                { testId(), true,  "C" },
                { testId(), true,  "ж" },
                { testId(), true,  "Ж" },
                { testId(), true,  "ab" },
                { testId(), true,  "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm" },
                { testId(), true,  "QWERTYUIOPASDFGHJKLZXCVBNM qwertyuiopasdfghjklzxcvbnm" },
                { testId(), true,  "QWERTYUIOPASDFGHJKLZXCVBNM.qwertyuiopasdfghjklzxcvbnm" },
                { testId(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm." },
                { testId(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm " },
                { testId(), true,  boarderLengthName },
                { testId(), false, overBoardLengthName },
                { testId(), true,  "явертъуиопасдфгхйклзьцжбнмшщюч ЯВЕРТЪУИОПАСДФГХЙКЛЗЬЦЖБНМШЩЮЧ" },
            };
        }
    }

    [Theory]
    [MemberData(nameof(WinFilenameData))]
    public void TestWinFilename(string testId, bool shouldBe, string fileName)
        => base.RegexTest(WindowsFileNames.DiskFilename, testId, shouldBe, fileName);

    public static TheoryData<string, bool, string, string, string, string> WinPathnameData
    {
        get
        {
            var i = 0;
            string testId() => i++.ToString("d2");

            return new TheoryData<string, bool, string, string, string, string>
            {
                { testId(), false, "", "", "", "" },
                { testId(), false, " ", "", "", "" },
                { testId(), false, ".", "", "", "" },
                { testId(), false, "..", "", "", "" },
                { testId(), false, "con", "", "", "" },
                { testId(), false, "con.txt", "", "", "" },
                { testId(), false, "lpt1", "", "", "" },
                { testId(), false, "lpt1.txt", "", "", "" },
                { testId(), false, "a.", "", "", "" },
                { testId(), false, "a ", "", "", "" },
                { testId(), true,  "a", "", "", "" },
                { testId(), true,  "C", "", "", "" },
                { testId(), true,  "ж", "", "", "" },
                { testId(), true,  "Ж", "", "", "" },
                { testId(), true,  "ab", "", "", "" },
                { testId(), true,  "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm", "", "", "" },
                { testId(), true,  "QWERTYUIOPASDFGHJKLZXCVBNM qwertyuiopasdfghjklzxcvbnm", "", "", "" },
                { testId(), true,  "QWERTYUIOPASDFGHJKLZXCVBNM.qwertyuiopasdfghjklzxcvbnm", "", "", "" },
                { testId(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm.", "", "", "" },
                { testId(), false, "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm ", "", "", "" },
                { testId(), true,  boarderLengthName, "", "", "" },
                { testId(), false, overBoardLengthName, "", "", "" },
                { testId(), true,  "явертъуиопасдфгхйклзьцжбнмшщюч ЯВЕРТЪУИОПАСДФГХЙКЛЗЬЦЖБНМШЩЮЧ", "", "", "" },
                                //   pathname:                   filename:   path:           drive:
                { testId(), false,  @"a:com1",                   "",         "",             ""  },
                { testId(), false,  @"ъ:a",                      "",         "",             ""  },
                { testId(), false,  @"a:"+boarderLengthName,     "a",        "",             "a" },
                { testId(), false,  @"a:a.",                     "a",        "",             "a" },
                { testId(), false,  @"a:a ",                     "a",        "",             "a" },
                { testId(), true,   @"a:a",                      "a",        "",             "a" },
                { testId(), false,  @"a:/com2",                  "",         "",             ""  },
                { testId(), false,  @"a:/com2.bin",              "",         "",             ""  },
                { testId(), false,  @"a:",                       "",         "",             ""  },
                { testId(), true,   @"a:/a",                     "a",        @"/",           "a" },
                { testId(), true,   @"a:\a",                     "a",        @"\",           "a" },
                { testId(), true,   @"a:b/a",                    "a",        "b",            "a" },
                { testId(), true,   @"a:./a",                    "a",        ".",            "a" },
                { testId(), true,   @"a:../a",                   "a",        "..",           "a" },
                { testId(), false,  @"a:lpt2/a",                 "",         "",             ""  },
                { testId(), false,  @"a:lpt2.txt/a",             "",         "",             ""  },
                { testId(), false,  @"a:/b/lpt2/a",              "",         "",             ""  },
                { testId(), false,  @"a:/b/lpt2.txt/a",          "",         "",             ""  },
                { testId(), false,  @"a:/b/c/d/lpt2",            "",         "",             ""  },
                { testId(), false,  @"a:/b/c/d/lpt2.txt",        "",         "",             ""  },
                { testId(), false,  @"a:/b/c./d/lp",             "",         "",             ""  },
                { testId(), false,  @"a:/b/c./d/lp.txt",         "",         "",             ""  },
                { testId(), false,  @"a:/b/c /d/lp",             "",         "",             ""  },
                { testId(), false,  @"a:/b/c /d/lp.txt",         "",         "",             ""  },
                { testId(), true,   @"a:b\a",                    "a",        @"b",           "a" },
                { testId(), true,   @"a:/bb/a",                  "a",        @"/bb",         "a" },
                { testId(), true,   @"a:/bb/cc/aa.aa",           "aa.aa",    @"/bb/cc",      "a" },
                { testId(), true,   @"a:bb/cc/aa.aa",            "aa.aa",    @"bb/cc",       "a" },
                { testId(), true,   @"bb/cc/aa.aa",              "aa.aa",    @"bb/cc",       ""  },
                { testId(), true,   @"a:/./a",                   "a",        @"/.",          "a" },
                { testId(), true,   @"a:../../a",                "a",        @"../..",       "a" },
                { testId(), true,   @"a:..\../a",                "a",        @"..\..",       "a" },
                { testId(), true,   @"a:\./a",                   "a",        @"\.",          "a" },
                { testId(), true,   @"a:\bb\a",                  "a",        @"\bb",         "a" },
                { testId(), true,   @"b\a",                      "a",        @"b",           ""  },
                { testId(), true,   @"/bb/a",                    "a",        @"/bb",         ""  },
                { testId(), true,   @"/bb/cc/aa.aa",             "aa.aa",    @"/bb/cc",      ""  },
                { testId(), true,   @"/./a",                     "a",        @"/.",          ""  },
                { testId(), true,   @"../../a",                  "a",        @"../..",       ""  },
                { testId(), true,   @"..\../a",                  "a",        @"..\..",       ""  },
                { testId(), true,   @"\./a",                     "a",        @"\.",          ""  },
                { testId(), true,   @"\bb\a",                    "a",        @"\bb",         ""  },
                { testId(), false, $@"a:\{boarderLengthName}\a", "",         "",             ""  },
                { testId(), false, $@"\{boarderLengthName}",     "",         "",             ""  },
                { testId(), false, $@"a:\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\012345678901234567890123456789012345678901.txt",
                                                                 "",         "",             ""  },
                { testId(), true,  $@"a:\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\01234567890123456789012345678901234567890.txt",
                                                                 "01234567890123456789012345678901234567890.txt",
                                                                             @"\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm\QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm",
                                                                                             "a" },
            };
        }
    }

    [Theory]
    [MemberData(nameof(WinPathnameData))]
    public void TestWinPathname(string testId, bool shouldBe, string pathname, string file, string path, string drive)
    {
        var matches = base.RegexTest(WindowsFileNames.Pathname, testId, shouldBe, pathname);

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
