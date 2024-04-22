﻿namespace vm2.ExpressionSerialization.XmlTests;

public partial class BinaryExpressionTests : IClassFixture<TestsFixture>
{
    internal const string TestConstantsFilesPath = TestsFixture.TestFilesPath+"Binary/";

    public ITestOutputHelper Out { get; }

    TestsFixture _fixture;

    public BinaryExpressionTests(
        TestsFixture fixture,
        ITestOutputHelper output)
    {
        FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();
        _fixture = fixture;
        Out = output;
    }

    [Theory]
    [MemberData(nameof(BinaryExpressionData))]
    public async Task BinaryAsync(string _, string expressionString, string fileName)
    {
        var expression = Substitute(expressionString);
        var pathName = TestConstantsFilesPath + fileName;
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(pathName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }
}
