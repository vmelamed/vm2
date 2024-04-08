using TestUtilities;

using Xunit.Abstractions;

namespace ExpressionSerialization;

public class ExpressionTransformVisitorTests
{
    public ITestOutputHelper Out { get; }

    public ExpressionTransformVisitorTests(ITestOutputHelper output)
    {
        Out = output;
        FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();
    }

    public static TheoryData<string, string, string> ExpressionTransformVisitorData => new() {
        { TestLine(), "",          "" },
        { TestLine(), " ",         " " },
        { TestLine(), " Abc",      " Abc" },
        { TestLine(), "a",         "a" },
        { TestLine(), "A",         "a" },
        { TestLine(), "ABC",       "aBC" },
        { TestLine(), "1BC",       "1BC" },
        { TestLine(), "CamelCase", "camelCase" },
    };

    [Theory]
    [MemberData(nameof(ExpressionTransformVisitorData))]
    public void ExpressionTransformVisitorTest(string _, string input, string expected)
        => ExpressionTransformVisitor<object>.CamelCase(input).Should().Be(expected);
}