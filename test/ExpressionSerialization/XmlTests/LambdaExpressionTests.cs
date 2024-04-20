namespace vm2.ExpressionSerialization.XmlTests;

public partial class LambdaExpressionTests : IClassFixture<TestsFixture>
{
    internal const string TestConstantsFilesPath = TestsFixture.TestFilesPath+"Lambdas/";

    public ITestOutputHelper Out { get; }

    TestsFixture _fixture;

    public LambdaExpressionTests(
        TestsFixture fixture,
        ITestOutputHelper output)
    {
        FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();
        _fixture = fixture;
        Out = output;
    }

    [Theory]
    [MemberData(nameof(LambdaExpressionData))]
    public async Task ParamToBoolConstantAsync(string _, string expressionString, string fileName)
    {
        var expression = Substitute(expressionString);
        var pathName = TestConstantsFilesPath + fileName;
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(pathName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }
}
