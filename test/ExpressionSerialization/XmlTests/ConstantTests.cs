namespace vm2.ExpressionSerialization.XmlTests;
public partial class ConstantTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string TestConstantsFilesPath => TestsFixture.TestFilesPath + "Constants/";

    [Theory]
    [MemberData(nameof(ConstantsData))]
    public async Task AssignmentTestAsync(string _, string expressionString, string fileName) => await base.TestAsync(expressionString, fileName);

    protected override Expression Substitute(string id) => _substitutes[id] as ConstantExpression ?? Expression.Constant(_substitutes[id]);

    [Theory]
    [InlineData(5, "NullableInt.xml")]
    [InlineData(null, "NullNullableInt.xml")]
    public async Task TestConstantNullableIntAsync(int? value, string fileName)
    {
        var pathName = TestConstantsFilesPath + fileName;
        var expression = Expression.Constant(value, typeof(int?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(pathName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

    [Theory]
    [InlineData(5L, "NullableLong.xml")]
    [InlineData(null, "NullNullableLong.xml")]
    public async Task TestConstantNullableLongAsync(long? value, string fileName)
    {
        var pathName = TestConstantsFilesPath + fileName;
        var expression = Expression.Constant(value, typeof(long?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(pathName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

    [Fact]
    public async Task TestConstantObject1NullAsync()
    {
        var pathName = TestConstantsFilesPath+"Object1Null.xml";
        var expression = Expression.Constant(null, typeof(Object1));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(pathName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task TestConstantExpressionClassNonSerializableAsync(bool callAsync)
    {
        var pathName = TestConstantsFilesPath+"ClassSerializable1.xml";
        var expression = Expression.Constant(new ClassNonSerializable(1, "One"));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(pathName, Out);
        var testCall = () => _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        var testAsyncCall = async () => await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);

        if (!callAsync)
            testCall.Should().Throw<NonSerializableObjectException>();
        else
            await testAsyncCall.Should().ThrowAsync<NonSerializableObjectException>();
    }

    [Theory]
    [InlineData(false, "NullableStructDataContract1.xml")]
    [InlineData(true, "NullNullableStructDataContract1.xml")]
    public async Task TestConstantNullableStructDataContractAsync(bool isNull, string fileName)
    {
        var pathName = TestConstantsFilesPath + fileName;
        StructDataContract1? structDc = isNull ? null : new StructDataContract1()
        {
            IntProperty = 7,
            StringProperty = "vm",
        };
        var expression = Expression.Constant(structDc, typeof(StructDataContract1?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(pathName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

    [Theory]
    [InlineData(false, "NullableStructSerializable1.xml")]
    [InlineData(true, "NullNullableStructSerializable1.xml")]
    public async Task TestConstantNullableStructSerializableAsync(bool isNull, string fileName)
    {
        var pathName = TestConstantsFilesPath + fileName;
        StructSerializable1? structDc = isNull ? null : new StructSerializable1()
        {
            IntProperty = 7,
            StringProperty = "vm",
        };
        var expression = Expression.Constant(structDc, typeof(StructSerializable1?));
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(pathName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }
}
