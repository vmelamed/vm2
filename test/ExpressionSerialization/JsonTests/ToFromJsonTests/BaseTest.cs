namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

using TestUtilities;

public abstract class BaseTests : IClassFixture<TestsFixture>
{
    public ITestOutputHelper Out { get; }

    protected TestsFixture _fixture;

    public BaseTests(
        TestsFixture fixture,
        ITestOutputHelper output)
    {
        FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();
        _fixture = fixture;
        Out = output;
    }

    protected abstract string JsonTestFilesPath { get; }

    public virtual async Task ToJsonTestAsync(string testFileLine, string expressionString, string fileName)
    {
        var expression = Substitute(expressionString);
        var pathName = Path.Combine(JsonTestFilesPath, fileName);
        var (expectedDoc, expectedStr) = await TestsFixture.GetJsonDocumentAsync(testFileLine, pathName, "EXPECTED", Out);

        TestsFixture.TestExpressionToJson(testFileLine, expression, expectedDoc, expectedStr, pathName, Out);
        await TestsFixture.TestExpressionToJsonAsync(testFileLine, expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

    public virtual async Task FromJsonTestAsync(string testFileLine, string expressionString, string fileName)
    {
        var expectedExpression = Substitute(expressionString);
        var pathName = Path.Combine(JsonTestFilesPath, fileName);
        var (inputDoc, _) = await TestsFixture.GetJsonDocumentAsync(testFileLine, pathName, "INPUT", Out, true);

        TestsFixture.TestJsonToExpression(testFileLine, inputDoc, expectedExpression);
    }

    protected abstract Expression Substitute(string id);
}
