using System.Runtime.CompilerServices;

namespace vm2.RegexLibTests;

public abstract class RegexTests(ITestOutputHelper output)
{
    public ITestOutputHelper Out { get; } = output;

    static readonly Regex TestDir = new(@"\\test\\", RegexOptions.Compiled | RegexOptions.IgnoreCase);


    protected static string TestLine(
        string testDescription = "",
        [CallerFilePath] string fileName = "",
        [CallerLineNumber] int line = 0)
        => $"{fileName[(TestDir.Match(fileName).Index + 1)..]}:{line}{(testDescription.Length > 0 ? " : " + testDescription : "")}";

    /// <summary>
    /// Data driven (theory) test for the regular expression <see cref="Regex" />
    /// </summary>
    /// <param name="regex">The regex object.</param>
    /// <param name="testAt">
    /// The relative file path and line from which the function was called. Usually, where the test is in the test file,
    /// or the <see cref="TheoryData"/> is defined.</param>
    /// <param name="shouldMatch">The should match.</param>
    /// <param name="input">The input.</param>
    protected virtual MatchCollection RegexTest(
        Regex regex,
        string testAt,
        bool shouldMatch,
        string input,
        params string[] expectedCaptures)
    {
        var matches = regex.Matches(input);
        var isMatch = (matches?.Count ?? 0) > 0;

        Out.WriteLine($"""
                       Test ID:  {testAt}
                         Input:   
                           →{input}←
                         Is match: {isMatch}
                         Matches:  {matches?.Count}:
                       """);

        try
        {
            if (isMatch)
                foreach (var m in matches!)
                {
                    m.Should().BeOfType<Match>();
                    var match = ((Match)m);
                    Out.WriteLine($"    →{match.Value}←");

                    if (!expectedCaptures.Any())
                    {
                        foreach (var gr in match.Groups)
                            if (gr is Group group && group.Name != "0")
                                Out.WriteLine($"      {group.Name}: →{group.Value}←");
                    }
                    else
                    {
                        match.Groups.Count.Should().Be(expectedCaptures.Length + 1);

                        var i = 0;
                        foreach (var gr in match.Groups)
                            if (gr is Group group && group.Name != "0")
                            {
                                var notInExpected = "";

                                if (i < expectedCaptures.Length)
                                    group.Value.Should().Be(expectedCaptures[i]);
                                else
                                    notInExpected = " - not in the expectedCaptures";

                                Out.WriteLine($"      {group.Name}: →{group.Value}← {notInExpected}");
                                i++;
                            }
                    }
                }
        }
        finally
        {
            Out.WriteLine($"  Regex:\n    →{regex}←");
        }

        matches.Should().NotBeNull();
        isMatch.Should().Be(shouldMatch);
        if (shouldMatch)
            matches.Should().HaveCountGreaterThanOrEqualTo(1);

        return matches!;
    }

    public const RegexOptions RegexOpt = RegexOptions.CultureInvariant | RegexOptions.IgnorePatternWhitespace;

    /// <summary>
    /// Data driven (theory) test for the regular expression string <see cref="RegexString" />
    /// </summary>
    /// <param name="regex">The regex string.</param>
    /// <param name="testAt">The test identifier, e.g. "03".</param>
    /// <param name="shouldMatch">The should match.</param>
    /// <param name="input">The input.</param>
    protected virtual void RegexStringTest(
        string regex,
        string testAt,
        bool shouldMatch,
        string input,
        RegexOptions ro = RegexOpt,
        params string[] expectedCaptures)
            => RegexTest(new Regex(regex, ro), testAt, shouldMatch, input, expectedCaptures);
}
