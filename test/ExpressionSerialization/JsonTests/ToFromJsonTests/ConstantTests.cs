namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

[CollectionDefinition("JSON")]
public partial class ConstantTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(JsonTestsFixture.TestFilesPath, "Constants");

    [Theory]
    [MemberData(nameof(ConstantTestData.Data), MemberType = typeof(ConstantTestData))]
    public async Task ConstantToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(ConstantTestData.Data), MemberType = typeof(ConstantTestData))]
    public async Task ConstantFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    protected override Expression Substitute(string id) => ConstantTestData.GetExpression(id);
}
