namespace RegexLibTests;

public class FileNameTests
{
    public ITestOutputHelper Out { get; }

    public FileNameTests(ITestOutputHelper output) => Out = output;

    public static TheoryData<string, bool, string> WinFilenameData 
    { get
        {
            var i = 0;
            string testNo() => i++.ToString("d2");

            return new TheoryData<string, bool, string>
            {
                { testNo(), false, "" },
                { testNo(), false, "." },
                { testNo(), false, ".." },
                { testNo(), true,  "con" },
                { testNo(), true,  "con.txt" },
            };
        }
    }

    [Theory]
    [MemberData(nameof(WinFilenameData))]
    public void TestWinFilename(string _, bool shouldBe, string fileName)
    {
        Out.WriteLine($"`{FileNames.WindowsDiskFilenameRegex}`");
        FileNames.WindowsDiskFilename.IsMatch(fileName).Should().Be(shouldBe);
    }
}
