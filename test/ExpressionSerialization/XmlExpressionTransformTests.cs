namespace vm2.ExpressionSerialization.ExpressionSerializationTests;

//[Collection("XmlSerialization")]
public partial class XmlExpressionTransformTests : IClassFixture<XmlSerializationTestsFixture>
{
    public ITestOutputHelper Out { get; }

    XmlSerializationTestsFixture _fixture;

    public XmlExpressionTransformTests(
        XmlSerializationTestsFixture fixture,
        ITestOutputHelper output)
    {
        FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();
        _fixture = fixture;
        Out = output;
    }

#pragma warning disable xUnit1045
    [Theory]
    [MemberData(nameof(ConstantExpressionData))]
    public async Task ConstantTestAsync(string _, object value, string fileName)
    {
        var expression = Expression.Constant(value);
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync($"../../../TestData/Constants/{fileName}", Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);
    }
#pragma warning restore xUnit1045

    [Theory]
    [InlineData(5, "../../../TestData/Constants/NullableInt.xml")]
    [InlineData(null, "../../../TestData/Constants/NullNullableInt.xml")]
    public async Task ConstantTestNullableIntAsync(int? value, string fileName)
    {
        Expression expression = Expression.Constant(value, typeof(int?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);
    }

    [Theory]
    [InlineData(5L, "../../../TestData/Constants/NullableLong.xml")]
    [InlineData(null, "../../../TestData/Constants/NullNullableLong.xml")]
    public async Task ConstantTestNullableLongAsync(long? value, string fileName)
    {
        Expression expression = Expression.Constant(value, typeof(long?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);
    }

    [Fact]
    public async Task ConstantTestAnonymousAsync()
    {
        var fileName = "../../../TestData/Constants/Anonymous.xml";
        Expression expression = DataUtils.GetAnonymous();
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);
    }

    [Fact]
    public async Task ConstantTestObject1Async()
    {
        var fileName = "../../../TestData/Constants/Object1.xml";
        Expression expression = Expression.Constant(new Object1());
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);
    }

    [Fact]
    public async Task ConstantTestObject1NullAsync()
    {
        var fileName = "../../../TestData/Constants/Object1Null.xml";
        Expression expression = Expression.Constant(null, typeof(Object1));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);
    }
}
