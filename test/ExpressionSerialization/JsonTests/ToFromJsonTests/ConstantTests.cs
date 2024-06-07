namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

public partial class ConstantTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(TestsFixture.TestFilesPath, "Constants");

    [Theory]
    [MemberData(nameof(ConstantsData))]
    public async Task ConstantToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(ConstantsData))]
    public async Task ConstantFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    protected override Expression Substitute(string id) => _substitutes[id];

    [Fact]
    public async Task TestConstantToXmlClassNonSerializableAsync()
    {
        var pathName = Path.Combine(JsonTestFilesPath, "ClassSerializable1.json");
        var expression = Expression.Constant(new ClassNonSerializable(1, "One"));
        var (expectedDoc, expectedStr) = await TestsFixture.GetJsonDocumentAsync(TestLine(), pathName, "EXPECTED", Out);

        var testCall = () => TestsFixture.TestExpressionToJson(TestLine(), expression, expectedDoc, expectedStr, pathName, Out);

        testCall.Should().Throw<SerializationException>();

        var testAsyncCall = async () => await TestsFixture.TestExpressionToJsonAsync(TestLine(), expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);

        await testAsyncCall.Should().ThrowAsync<SerializationException>();
    }
}
