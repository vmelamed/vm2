namespace vm2.ExpressionSerialization.XmlTests;

using System.Collections.Frozen;

public partial class ConstantTests : IClassFixture<TestsFixture>
{
    internal const string TestFilesPath = "../../../TestData/";
    internal const string TestConstantsFilesPath = TestFilesPath+"Constants/";

    public ITestOutputHelper Out { get; }

    TestsFixture _fixture;

    public ConstantTests(
        TestsFixture fixture,
        ITestOutputHelper output)
    {
        FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();
        _fixture = fixture;
        Out = output;
    }

#pragma warning disable xUnit1045
    [Theory]
    [MemberData(nameof(ConstantExpressionData))]
    public async Task TestConstantAsync(string _, object? value, string fileNm)
    {
        var fileName = TestConstantsFilesPath + fileNm;
        var expression = Expression.Constant(value);
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, fileName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, fileName, Out, CancellationToken.None);
    }

    [Theory]
    [MemberData(nameof(ConstantExpressionNsData))]
    public async Task TestConstantNsAsync(string _, object? value, string fileNm)
    {
        var fileName = TestConstantsFilesPath + fileNm;
        var expression = Expression.Constant(value);
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, fileName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, fileName, Out, CancellationToken.None);
    }
#pragma warning restore xUnit1045

    [Theory]
    [InlineData(5, "NullableInt.xml")]
    [InlineData(null, "NullNullableInt.xml")]
    public async Task TestConstantNullableIntAsync(int? value, string fileNm)
    {
        var fileName = TestConstantsFilesPath + fileNm;
        var expression = Expression.Constant(value, typeof(int?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, fileName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, fileName, Out, CancellationToken.None);
    }

    [Theory]
    [InlineData(5L, "NullableLong.xml")]
    [InlineData(null, "NullNullableLong.xml")]
    public async Task TestConstantNullableLongAsync(long? value, string fileNm)
    {
        var fileName = TestConstantsFilesPath + fileNm;
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

    [Fact]
    public async Task TestIntFrozenSetAsync()
    {
        var fileName = TestConstantsFilesPath+"IntFrozenSet.xml";
        var expression = Expression.Constant((new int[]{ 1, 2, 3, 4 }).ToFrozenSet(), typeof(FrozenSet<int>));
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
        var expression = Expression.Constant(new ClassNonSerializable(1, "One"));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);
        var testCall = () => _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, fileName, Out);
        var testAsyncCall = async () => await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, fileName, Out, CancellationToken.None);

        if (!callAsync)
            testCall.Should().Throw<NonSerializableObjectException>();
        else
            await testAsyncCall.Should().ThrowAsync<NonSerializableObjectException>();
    }

    [Theory]
    [InlineData(false, "NullableStructDataContract1.xml")]
    [InlineData(true, "NullNullableStructDataContract1.xml")]
    public async Task TestConstantNullableStructDataContractAsync(bool isNull, string fileNm)
    {
        var fileName = TestConstantsFilesPath + fileNm;
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
    [InlineData(false, "NullableStructSerializable1.xml")]
    [InlineData(true, "NullNullableStructSerializable1.xml")]
    public async Task TestConstantNullableStructSerializableAsync(bool isNull, string fileNm)
    {
        var fileName = TestConstantsFilesPath + fileNm;
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

    [Theory]
    [InlineData(typeof(int), "DefaultInt.xml")]
    [InlineData(typeof(int?), "DefaultNullableInt.xml")]
    public async Task TestDefaultIntAsync(Type type, string fileNm)
    {
        var fileName = TestConstantsFilesPath + fileNm;
        var expression = Expression.Default(type);
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(fileName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, fileName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, fileName, Out, CancellationToken.None);
    }
}
