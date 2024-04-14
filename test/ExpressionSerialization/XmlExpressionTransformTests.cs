namespace vm2.ExpressionSerialization.ExpressionSerializationTests;

public partial class XmlExpressionTransformTests
{
    public ITestOutputHelper Out { get; }

    XmlSerializationTestsFixture _fixture;

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
    public async Task ConstantTestAsync(string _, object value, string fileName)
    {
        Expression expression = Expression.Constant(value);
        var (expectedDoc, expectedStr) = await GetExpectedAsync($"../../../TestData/Constants/{fileName}", Out);

        Out.WriteLine("EXPECTED:\n{0}\n", expectedStr);

        TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        await TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);
    }

    [Fact]
    public async Task ConstantTestNullableIntAsync()
    {
        int? value = 5;
        Expression expression = Expression.Constant(value, typeof(int?));
        var (expectedDoc, expectedStr) = await GetExpectedAsync($"../../../TestData/Constants/NullableInt.xml", Out);

        Out.WriteLine("EXPECTED:\n{0}\n", expectedStr);

        TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        await TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);
    }
}
