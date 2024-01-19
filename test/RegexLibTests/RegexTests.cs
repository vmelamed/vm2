using System.Runtime.CompilerServices;

namespace vm2.RegexLibTests;

public abstract class RegexTests
{
    public ITestOutputHelper Out { get; }

    public RegexTests(ITestOutputHelper output) => Out = output;

    protected static string TestLine([CallerLineNumber] int line = 0) => $"{line:d3}";

    /// <summary>
    /// Data driven (theory) test for the regular expression <see cref="Regex" />
    /// </summary>
    /// <param name="regex">The regex object.</param>
    /// <param name="testLine">The line from which the function was called. Usually, where the test is in the test file.</param>
    /// <param name="shouldMatch">The should match.</param>
    /// <param name="input">The input.</param>
    protected virtual MatchCollection RegexTest(
        Regex regex,
        string testLine,
        bool shouldMatch,
        string input,
        params string[] expectedCaptures)
    {
        var matches = regex.Matches(input);
        var isMatch = matches.Count > 0;

        matches.Should().NotBeNull();

        Out.WriteLine($"""
                       Test ID:  {testLine}
                         Regex:    →{regex}←
                         Input:    →{input}←
                         Is match: {isMatch}
                         Matches:  {matches.Count}:
                       """);

        isMatch.Should().Be(shouldMatch);
        if (shouldMatch)
            matches.Should().HaveCountGreaterThanOrEqualTo(1);

        foreach (var m in matches)
        {
            m.Should().BeOfType<Match>();
            var match = ((Match)m);
            Out.WriteLine($"    →{match.Value}←");

            if (!expectedCaptures.Any())
            {
                foreach (var gr in match.Groups)
                    if (gr is Group group && group.Name != "0")
                        Out.WriteLine($"      {group.Name}: \t→{group.Value}←");
            }
            else
            {
                match.Groups.Count.Should().Be(expectedCaptures.Length + 1);

                var i = 0;
                foreach (var gr in match.Groups)
                    if (gr is Group group && group.Name != "0")
                    {
                        Out.WriteLine($"      {group.Name}: \t→{group.Value}←");
                        group.Value.Should().Be(expectedCaptures[i]);
                        i++;
                    }
            }
        }

        return matches;
    }

    /// <summary>
    /// Data driven (theory) test for the regular expression string <see cref="RegexString" />
    /// </summary>
    /// <param name="regex">The regex string.</param>
    /// <param name="testId">The test identifier, e.g. "03".</param>
    /// <param name="shouldMatch">The should match.</param>
    /// <param name="input">The input.</param>
    protected virtual void RegexStringTest(
        string regex,
        string testId,
        bool shouldMatch,
        string input,
        RegexOptions ro = RegexOptions.CultureInvariant |
                          RegexOptions.IgnorePatternWhitespace) => RegexTest(
                                                                        new Regex(regex, ro),
                                                                        testId,
                                                                        shouldMatch,
                                                                        input);
}
