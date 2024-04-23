namespace vm2.ExpressionSerialization.XmlTests;

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

    protected abstract string TestConstantsFilesPath { get; }

    public virtual async Task TestAsync(string expressionString, string fileName)
    {
        var expression = Substitute(expressionString);
        var pathName = TestConstantsFilesPath + fileName;
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(pathName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

    protected abstract Expression Substitute(string id);
}
