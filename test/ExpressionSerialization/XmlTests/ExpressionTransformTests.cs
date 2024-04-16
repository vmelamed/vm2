namespace vm2.ExpressionSerialization.XmlTests;

public partial class ExpressionTransformTests : IClassFixture<SerializationTestsFixture>
{
    public ITestOutputHelper Out { get; }

    SerializationTestsFixture _fixture;

    public ExpressionTransformTests(
        SerializationTestsFixture fixture,
        ITestOutputHelper output)
    {
        FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();
        _fixture = fixture;
        Out = output;
    }

#pragma warning disable xUnit1045
    [Theory]
    [MemberData(nameof(ConstantExpressionData))]
    public async Task TestConstantAsync(string _, object value, string fileName)
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
    public async Task TestConstantNullableIntAsync(int? value, string fileName)
    {
        var expression = Expression.Constant(value, typeof(int?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);
    }

    [Theory]
    [InlineData(5L, "../../../TestData/Constants/NullableLong.xml")]
    [InlineData(null, "../../../TestData/Constants/NullNullableLong.xml")]
    public async Task TestConstantNullableLongAsync(long? value, string fileName)
    {
        var expression = Expression.Constant(value, typeof(long?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);
    }

    [Fact]
    public async Task TestConstantObject1NullAsync()
    {
        var fileName = "../../../TestData/Constants/Object1Null.xml";
        var expression = Expression.Constant(null, typeof(Object1));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task TestConstantExpressionClassNonSerializableAsync(bool callAsync)
    {
        var fileName = "../../../TestData/Constants/ClassSerializable1.xml";
        var expression = Expression.Constant(new ClassNonSerializable());
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);
        var testCall = () => _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        var testAsyncCall = async () => await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);

        if (!callAsync)
            testCall.Should().Throw<ObjectNotSerializableException>();
        else
            await testAsyncCall.Should().ThrowAsync<ObjectNotSerializableException>();
    }

    [Theory]
    [InlineData(false, "../../../TestData/Constants/NullableStructDataContract1.xml")]
    [InlineData(true, "../../../TestData/Constants/NullNullableStructDataContract1.xml")]
    public async Task TestConstantNullableStructDataContractAsync(bool isNull, string fileName)
    {
        StructDataContract1? structDc = isNull ? null : new StructDataContract1()
        {
            IntProperty = 7,
            StringProperty = "vm",
        };
        var expression = Expression.Constant(structDc, typeof(StructDataContract1?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, Out, CancellationToken.None);
    }
}
