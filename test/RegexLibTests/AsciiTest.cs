using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine.ClientProtocol;

using Xunit.Abstractions;

namespace vm2.tests.RegexLibTests;

public class AsciiTest
{
    public ITestOutputHelper Out { get; }

    public AsciiTest(ITestOutputHelper output) => Out = output;

    [Theory]
    [InlineData(1, "", false)]
    [InlineData(2, "a", true)]
    [InlineData(3, "z", true)]
    [InlineData(4, "1", false)]
    [InlineData(5, "A", false)]
    [InlineData(6, "!", false)]
    [InlineData(7, "ж", false)]
    [InlineData(8, " ", false)]
    public void TestLowAlphaRex(int _, string input, bool shouldBe)
    {
        Out.WriteLine($"`{Ascii.LowAlphaRex}`");
        Regex.IsMatch(input, Ascii.LowAlphaRex).Should().Be(shouldBe);
    }

    [Theory]
    [InlineData(1, "", false)]
    [InlineData(2, "A", true)]
    [InlineData(3, "Z", true)]
    [InlineData(4, "1", false)]
    [InlineData(5, "a", false)]
    [InlineData(6, "!", false)]
    [InlineData(7, "Ж", false)]
    [InlineData(8, " ", false)]
    public void TestHighAlphaRex(int _, string input, bool shouldBe)
    {
        Out.WriteLine($"`{Ascii.HighAlphaRex}`");
        Regex.IsMatch(input, Ascii.HighAlphaRex).Should().Be(shouldBe);
    }

    [Theory]
    [InlineData(01, "", false)]
    [InlineData(02, "A", true)]
    [InlineData(03, "Z", true)]
    [InlineData(04, "a", true)]
    [InlineData(05, "z", true)]
    [InlineData(06, "1", false)]
    [InlineData(07, "!", false)]
    [InlineData(08, "Ж", false)]
    [InlineData(09, "ж", false)]
    [InlineData(10, " ", false)]
    public void TestAlphaRex(int _, string input, bool shouldBe)
    {
        Out.WriteLine($"`{Ascii.AlphaRex}`");
        Regex.IsMatch(input, Ascii.AlphaRex, RegexOptions.IgnorePatternWhitespace).Should().Be(shouldBe);
    }

    [Theory]
    [InlineData(01, "", false)]
    [InlineData(02, "A", true)]
    [InlineData(03, "Z", true)]
    [InlineData(04, "a", true)]
    [InlineData(05, "z", true)]
    [InlineData(06, "0", true)]
    [InlineData(07, "9", true)]
    [InlineData(08, "/", true)]
    [InlineData(09, "+", true)]
    [InlineData(10, "!", false)]
    [InlineData(11, "Ж", false)]
    [InlineData(12, "ж", false)]
    [InlineData(13, " ", false)]
    [InlineData(14, "\r", false)]
    [InlineData(15, "\n", false)]
    public void TestBase64CharRex(int _, string input, bool shouldBe)
    {
        Out.WriteLine($"`{Ascii.Base64CharRex}`");
        Regex.IsMatch(input, Ascii.Base64CharRex, RegexOptions.IgnorePatternWhitespace).Should().Be(shouldBe);
    }

    [Theory]
    [InlineData(01, "", true)]
    [InlineData(02, "TWFueSBoYW5kcyBtYWtlIGxpZ2h0IHdvcmsu", true)]
    [InlineData(03, "!", false)]
    [InlineData(04, """
                    IA==

                    """, true)]
    [InlineData(05, """
                    I!A==

                    """, false)]
    [InlineData(06, """
                    !IA==

                    """, false)]
    [InlineData(07, """
                    !IA==!

                    """, false)]
    [InlineData(08, """
                    IAmg455a
                    12tGb7/+

                    """, true)]
    [InlineData(09, """
                    I!Amg455a
                    12tGb7/+

                    """, false)]
    [InlineData(10, """
                    !IAmg455a
                    12tGb7/+

                    """, false)]
    [InlineData(11, """
                    IAmg455a!
                    12tGb7/+

                    """, false)]
    [InlineData(12, """
                    IAmg455a
                    12tGb7/+
                    IA==

                    """, true)]
    [InlineData(13, """
                    IAmg455a
                    12tGb7/+
                    !IA==

                    """, false)]
    [InlineData(14, """
                    IAmg455a
                    1!2tGb7/+
                    IA==

                    """, false)]
    [InlineData(15, """
                    IAmg455a
                    12tGb7/+!
                    IA==

                    """, false)]
    [InlineData(16, """
                    IAmg455a
                    12tGb7/+
                    !IA==

                    """, false)]
    [InlineData(17, """
                    IAmg455a
                    12tGb7/+
                    IA==!

                    """, false)]
    [InlineData(18, """
                    IAmg455a
                    12tGb7/+
                    IA=!=

                    """, false)]
    [InlineData(19, """
                    IAmg455a
                    12tGb7/+
                    I!A==

                    """, false)]
    public void TestBase64(int _, string input, bool shouldBe)
    {
        Out.WriteLine($"`{Ascii.Base64Regex}`");

        Ascii.Base64.IsMatch(input).Should().Be(shouldBe);
        var mm = Ascii.Base64.Matches(input);

        mm.Should().NotBeNull();

        if (shouldBe)
            mm.Should().HaveCount(1);

        Out.WriteLine($"{mm.Count} matches:");
        foreach (Match m in mm)
            Out.WriteLine(m.Value);
    }
}