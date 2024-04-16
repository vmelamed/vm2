namespace vm2.ExpressionSerialization.XmlTests;

public partial class TransformTests : IClassFixture<SerializationTestsFixture>
{
    internal const string TestFilesPath = "../../../TestData/";
    internal const string TestConstantsFilesPath = TestFilesPath+"Constants/";

    public ITestOutputHelper Out { get; }

    SerializationTestsFixture _fixture;

    public TransformTests(
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
        fileName = TestConstantsFilesPath + fileName;
        var expression = Expression.Constant(value);
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, fileName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, fileName, Out, CancellationToken.None);
    }
#pragma warning restore xUnit1045

    [Theory]
    [InlineData(5, TestConstantsFilesPath + "NullableInt.xml")]
    [InlineData(null, TestConstantsFilesPath + "NullNullableInt.xml")]
    public async Task TestConstantNullableIntAsync(int? value, string fileName)
    {
        var expression = Expression.Constant(value, typeof(int?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, fileName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, fileName, Out, CancellationToken.None);
    }

    [Theory]
    [InlineData(5L, TestConstantsFilesPath + "NullableLong.xml")]
    [InlineData(null, TestConstantsFilesPath + "NullNullableLong.xml")]
    public async Task TestConstantNullableLongAsync(long? value, string fileName)
    {
        var expression = Expression.Constant(value, typeof(long?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, fileName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, fileName, Out, CancellationToken.None);
    }

    [Fact]
    public async Task TestConstantObject1NullAsync()
    {
        var fileName = TestConstantsFilesPath+"Object1Null.xml";
        var expression = Expression.Constant(null, typeof(Object1));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, fileName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, fileName, Out, CancellationToken.None);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task TestConstantExpressionClassNonSerializableAsync(bool callAsync)
    {
        var fileName = TestConstantsFilesPath+"ClassSerializable1.xml";
        var expression = Expression.Constant(new ClassNonSerializable());
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);
        var testCall = () => _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, fileName, Out);
        var testAsyncCall = async () => await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, fileName, Out, CancellationToken.None);

        if (!callAsync)
            testCall.Should().Throw<ObjectNotSerializableException>();
        else
            await testAsyncCall.Should().ThrowAsync<ObjectNotSerializableException>();
    }

    [Theory]
    [InlineData(false, TestConstantsFilesPath + "NullableStructDataContract1.xml")]
    [InlineData(true, TestConstantsFilesPath + "NullNullableStructDataContract1.xml")]
    public async Task TestConstantNullableStructDataContractAsync(bool isNull, string fileName)
    {
        StructDataContract1? structDc = isNull ? null : new StructDataContract1()
        {
            IntProperty = 7,
            StringProperty = "vm",
        };
        var expression = Expression.Constant(structDc, typeof(StructDataContract1?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, fileName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, fileName, Out, CancellationToken.None);
    }

    [Theory]
    [InlineData(false, TestConstantsFilesPath + "NullableStructSerializable1.xml")]
    [InlineData(true, TestConstantsFilesPath + "NullNullableStructSerializable1.xml")]
    public async Task TestConstantNullableStructSerializableAsync(bool isNull, string fileName)
    {
        StructSerializable1? structDc = isNull ? null : new StructSerializable1()
        {
            IntProperty = 7,
            StringProperty = "vm",
        };
        var expression = Expression.Constant(structDc, typeof(StructSerializable1?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, fileName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, fileName, Out, CancellationToken.None);
    }
}
