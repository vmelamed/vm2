namespace vm2.RegexLibTests;
using System.Web;

using TestUtilities;

public class Captures : Dictionary<string, string>, IXunitSerializable
{
    const string countId = "countId";

    public void Deserialize(IXunitSerializationInfo info)
    {
        Clear();

        var count = info.GetValue<int>(countId);
        for (var i = 0; i < count; i++)
        {
            var name = info.GetValue<string>($"name{i}");
            var value = info.GetValue<string>($"value{i}");
            Add(name, value);
        }
    }

    public void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(countId, Count, typeof(int));
        int i = 0;
        foreach (var (name, value) in this)
        {
            info.AddValue($"name{i}", name, typeof(string));
            info.AddValue($"value{i}", value, typeof(string));
            i++;
        }
    }
}

public abstract partial class RegexTests
{

    public ITestOutputHelper Out { get; }

    public RegexTests(ITestOutputHelper output)
    {
        Out = output;
        FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();
    }

    public const RegexOptions RegexOpt = RegexOptions.IgnorePatternWhitespace;

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
        Dictionary<string, string>? expectedCaptures = null,
        RegexOptions ro = RegexOpt,
        bool failIfMissingExpected = true)
            => RegexTest(new Regex(regex, ro), testAt, shouldMatch, input, expectedCaptures, failIfMissingExpected);

    /// <summary>
    /// Data driven (theory) test for the regular expression <see cref="Regex" />
    /// </summary>
    /// <param name="regex">The regex object.</param>
    /// <param name="testAt">
    /// Information about the where the function was invoked from, e.g. file, line, and description provided by the 
    /// method <see cref="TestLine(string, string, int)"/>.
    /// </param>
    /// <param name="shouldMatch">The should match.</param>
    /// <param name="input">The input.</param>
    /// <param name="expectedCaptures">The expected capturing groups.</param>
    protected virtual void RegexTest(
        Regex regex,
        string testAt,
        bool shouldMatch,
        string input,
        Dictionary<string, string>? expectedCaptures = null,
        bool failIfMissingExpected = true)
    {
        var matches = regex.Matches(input);
        var isMatch = matches?.Count is > 0;

        Out.WriteLine($"""
                       Test ID:  {testAt}
                         Input:   
                           →{input}←
                         Matches:  {matches?.Count}:
                       """);

        if (!isMatch)
        {
            if (isMatch != shouldMatch)
                Out.WriteLine($"  Regex:\n    →{regex}←\n");
            isMatch.Should().Be(shouldMatch);
            return;
        }

        foreach (var match in matches!.OfType<Match>())
        {
            Out.WriteLine($"    →{match.Value}←");
            foreach (var group in match.Groups.AsReadOnly().Where(gr => !string.IsNullOrEmpty(gr.Value)).Skip(1))
                Out.WriteLine($"      {group.Name}: →{group.Value}←");
        }

        var actualGroups = matches!
                            .SelectMany(m => m.Groups.AsReadOnly())
                            .Where(gr => !string.IsNullOrEmpty(gr.Value))
                            .Skip(1)    // by definition the first group is always ["0"] = match.Value
                            .ToDictionary(gr => gr.Name, gr => gr.Value)
                            ;

        var (failed, messages) = CompareGroups(
                                    expectedCaptures,
                                    actualGroups,
                                    failIfMissingExpected);

        Out.WriteLine($"  Regex:\n    →{regex}←\n");

        foreach (var message in messages)
            Out.WriteLine(message);

        failed.Should().BeFalse($":\n{string.Join("\n", messages)}");
        isMatch.Should().Be(shouldMatch);
    }

    /// <summary>
    /// Data driven (theory) test for the regular expression <see cref="Regex" />
    /// </summary>
    /// <param name="regex">The regex object.</param>
    /// <param name="testAt">
    /// Information about the where the function was invoked from, e.g. file, line, and description provided by the 
    /// method <see cref="TestLine(string, string, int)"/>.
    /// </param>
    /// <param name="shouldMatch">The should match.</param>
    /// <param name="input">The input.</param>
    /// <param name="expectedCaptures">The expected capturing groups.</param>
    protected virtual void RegexTestUri(
        Regex regex,
        string testAt,
        bool shouldMatch,
        string input,
        Dictionary<string, string>? expectedCaptures = null,
        bool failIfMissingExpected = true)
    {
        var succeeded = Uri.TryCreate(input,UriKind.RelativeOrAbsolute, out var uri);

        if (succeeded != shouldMatch)
            Out.WriteLine("Uri.TryCreate IS DIFFERENT FROM THE EXPECTED: ", succeeded ? $"Uri.TryCreate succeeded: →{uri}←" : "Uri.TryCreate failed →{uri}←");

        RegexTest(regex, testAt, shouldMatch, input, expectedCaptures, failIfMissingExpected);
    }

    static (bool failed, IEnumerable<string> messages) CompareGroups(
        Dictionary<string, string>? expectedCaptures,
        Dictionary<string, string> actualCaptures,
        bool failIfMissingExpected)
    {
        var failed = false;
        var messages = new List<string>();

        if (expectedCaptures is null)
        {
            if (actualCaptures.Count > 0)
            {
                OutputActual(actualCaptures, messages);
                failed = failIfMissingExpected;
            }
            return (failed, messages);
        }

        foreach (var (expectedKey, expectedValue) in expectedCaptures)
            if (!string.IsNullOrEmpty(expectedValue))
            {
                if (!actualCaptures.TryGetValue(expectedKey, out var actualValue))
                {
                    messages.Add($"The expected capturing group '{expectedKey}' was not found in the actual capturing groups.");
                    failed = true;
                }
                else
                if (actualValue != expectedValue)
                {
                    messages.Add($"The value of the capturing group '{expectedKey}' is '{actualValue}' but expected '{expectedValue}'");
                    failed = true;
                }
            }

        foreach (var (actualKey, actualValue) in actualCaptures)
        {
            if (!expectedCaptures.TryGetValue(actualKey, out var expectedValue))
            {
                messages.Add($"The capturing group '{actualKey}' with value '{actualValue}' is missing from in the expected capturing groups.");
                failed = failIfMissingExpected;
            }
            else
            if (expectedValue != actualValue)
            {
                messages.Add($"The value of the capturing group '{actualKey}' is '{actualValue}' but expected '{expectedValue}'");
                failed = true;
            }
        }

        if (failed)
            OutputActual(actualCaptures, messages);

        return (failed, messages);
    }

    static void OutputActual(
        Dictionary<string, string> actualCaptures,
        List<string> messages)
    {
        var wr = new StringWriter();

        wr.WriteLine($$"""
                         you can pass expected captures:
                         ```
                         new() {
                         """);
        foreach (var (name, value) in actualCaptures)
        {
            if (name.Contains('%') || value.Contains('%'))
                wr.WriteLine($"            [\"{name}\"] = \"{value}\", // →{HttpUtility.UrlDecode(name)}← = →{HttpUtility.UrlDecode(value)}←");
            else
                wr.WriteLine($"            [\"{name}\"] = \"{value}\",");
        }
        wr.WriteLine("        }\n```");

        messages.Add(wr.ToString());
    }
}
