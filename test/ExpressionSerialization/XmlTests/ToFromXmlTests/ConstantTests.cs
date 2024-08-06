namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

[CollectionDefinition("XML")]
public partial class ConstantTests(XmlTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(XmlTestsFixture.TestFilesPath, "Constants");

    [Theory]
    [MemberData(nameof(ConstantsData))]
    public async Task ConstantToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(ConstantsData))]
    public async Task ConstantFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

    [Fact]
    public async Task TestConstantToXmlClassNonSerializableAsync()
    {
        var pathName = Path.Combine(XmlTestFilesPath, "ClassSerializable1.xml");
        var expression = Expression.Constant(new ClassNonSerializable(1, "One"));
        var (expectedDoc, expectedStr) = await _fixture.GetXmlDocumentAsync(TestLine(), pathName, "EXPECTED", Out);

        var testCall = () => _fixture.TestExpressionToXml(TestLine(), expression, expectedDoc, expectedStr, pathName, Out);

        testCall.Should().Throw<SerializationException>();

        var testAsyncCall = async () => await _fixture.TestExpressionToXmlAsync(TestLine(), expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);

        await testAsyncCall.Should().ThrowAsync<SerializationException>();
    }

    protected override Expression Substitute(string id) => ConstantTestData.GetExpression(id);
}
