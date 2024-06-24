namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

public abstract class BaseTests(
        JsonTestsFixture fixture,
        ITestOutputHelper output) : IClassFixture<JsonTestsFixture>
{
    public ITestOutputHelper Out { get; } = output;

    protected JsonTestsFixture _fixture = fixture;

    protected abstract string JsonTestFilesPath { get; }

    protected bool JsonTestFilesPathExists { get; set; }

    public virtual async Task ToJsonTestAsync(string testFileLine, string expressionString, string fileName)
    {
        if (!JsonTestFilesPathExists)
        {
            if (!Directory.Exists(JsonTestFilesPath))
                Directory.CreateDirectory(JsonTestFilesPath);

            JsonTestFilesPathExists = true;
        }

        var expression = Substitute(expressionString);
        var pathName = Path.Combine(JsonTestFilesPath, fileName);
        var (expectedDoc, expectedStr) = await _fixture.GetJsonDocumentAsync(testFileLine, pathName, "EXPECTED", Out);

        _fixture.TestExpressionToJson(testFileLine, expression, expectedDoc, expectedStr, pathName, Out);
        await _fixture.TestExpressionToJsonAsync(testFileLine, expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

    public virtual async Task FromJsonTestAsync(string testFileLine, string expressionString, string fileName)
    {
        var expectedExpression = Substitute(expressionString);
        var pathName = Path.Combine(JsonTestFilesPath, fileName);
        var (inputDoc, _) = await _fixture.GetJsonDocumentAsync(testFileLine, pathName, "INPUT", Out, true);

        _fixture.TestJsonToExpression(testFileLine, inputDoc, expectedExpression);
    }

    protected abstract Expression Substitute(string id);
}
