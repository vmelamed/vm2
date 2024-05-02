namespace vm2.ExpressionSerialization.XmlTests.ToDocumentTests;
public partial class ConstantTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(TestsFixture.TestFilesPath, "Constants");

    [Theory]
    [MemberData(nameof(ConstantsData))]
    public async Task AssignmentTestAsync(string _, string expressionString, string fileName) => await base.TestAsync(expressionString, fileName);

    protected override Expression Substitute(string id) => _substitutes[id] as ConstantExpression ?? Expression.Constant(_substitutes[id]);

    [Theory]
    [InlineData(5, "NullableInt.xml")]
    [InlineData(null, "NullNullableInt.xml")]
    public async Task TestConstantNullableIntAsync(int? value, string fileName)
    {
        var pathName = Path.Combine(XmlTestFilesPath, fileName);
        var expression = Expression.Constant(value, typeof(int?));
        var (expectedDoc, expectedStr) = await TestsFixture.GetExpectedAsync(pathName, Out);

        TestsFixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await TestsFixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

    [Theory]
    [InlineData(5L, "NullableLong.xml")]
    [InlineData(null, "NullNullableLong.xml")]
    public async Task TestConstantNullableLongAsync(long? value, string fileName)
    {
        var pathName = Path.Combine(XmlTestFilesPath, fileName);
        var expression = Expression.Constant(value, typeof(long?));
        var (expectedDoc, expectedStr) = await TestsFixture.GetExpectedAsync(pathName, Out);

        TestsFixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await TestsFixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

    [Fact]
    public async Task TestConstantObject1NullAsync()
    {
        var pathName = Path.Combine(XmlTestFilesPath, "Object1Null.xml");
        var expression = Expression.Constant(null, typeof(Object1));
        var (expectedDoc, expectedStr) = await TestsFixture.GetExpectedAsync(pathName, Out);

        TestsFixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await TestsFixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task TestConstantExpressionClassNonSerializableAsync(bool callAsync)
    {
        var pathName = Path.Combine(XmlTestFilesPath, "ClassSerializable1.xml");
        var expression = Expression.Constant(new ClassNonSerializable(1, "One"));
        var (expectedDoc, expectedStr) = await TestsFixture.GetExpectedAsync(pathName, Out);
        var testCall = () => TestsFixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        var testAsyncCall = async () => await TestsFixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);

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
        var pathName = Path.Combine(XmlTestFilesPath, fileName);
        StructDataContract1? structDc = isNull ? null : new StructDataContract1()
        {
            IntProperty = 7,
            StringProperty = "vm",
        };
        var expression = Expression.Constant(structDc, typeof(StructDataContract1?));
        var (expectedDoc, expectedStr) = await TestsFixture.GetExpectedAsync(pathName, Out);

        TestsFixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await TestsFixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

    [Theory]
    [InlineData(false, "NullableStructSerializable1.xml")]
    [InlineData(true, "NullNullableStructSerializable1.xml")]
    public async Task TestConstantNullableStructSerializableAsync(bool isNull, string fileName)
    {
        var pathName = Path.Combine(XmlTestFilesPath, fileName);
        StructSerializable1? structDc = isNull ? null : new StructSerializable1()
        {
            IntProperty = 7,
            StringProperty = "vm",
        };
        var expression = Expression.Constant(structDc, typeof(StructSerializable1?));
        var (expectedDoc, expectedStr) = await TestsFixture.GetExpectedAsync(pathName, Out);

        TestsFixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await TestsFixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }
}
