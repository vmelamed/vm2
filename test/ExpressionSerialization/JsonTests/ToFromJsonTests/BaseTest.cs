﻿namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;
public abstract class BaseTests(
        TestsFixture fixture,
        ITestOutputHelper output) : IClassFixture<TestsFixture>
{
    public ITestOutputHelper Out { get; } = output;

    protected TestsFixture _fixture = fixture;

    protected abstract string JsonTestFilesPath { get; }

    public virtual async Task ToJsonTestAsync(string testFileLine, string expressionString, string fileName)
    {
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
