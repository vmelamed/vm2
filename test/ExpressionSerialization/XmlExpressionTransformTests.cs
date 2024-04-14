namespace ExpressionSerializationTests;

public partial class XmlExpressionTransformTests
{
    public ITestOutputHelper Out { get; }
#pragma warning disable IDE0052 // Remove unread private members
    XmlSerializationTestsFixture _fixture;
#pragma warning restore IDE0052 // Remove unread private members

    public XmlExpressionTransformTests(
        ITestOutputHelper output,
        XmlSerializationTestsFixture fixture)
    {
        Out = output;
        FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();
        _fixture = fixture;
    }

    [Theory]
    [MemberData(nameof(ExpressionsData))]
    public async Task ConstantTestIntAsync(string _, Expression expression, string fileName)
    {
        var (expectedDoc, expectedStr) = await GetExpectedAsync($"../../../TestData/Constants/{fileName}", Out);

        Out.WriteLine("EXPECTED:\n{0}\n", expectedStr);

        TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        await TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);
    }
}
