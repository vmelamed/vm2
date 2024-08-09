namespace vm2.ExpressionSerialization.JsonTests;

[CollectionDefinition("JSON")]
public partial class LambdaTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(JsonTestsFixture.TestFilesPath, "Lambdas");

    [Theory]
    [MemberData(nameof(LambdaTestData.Data), MemberType = typeof(LambdaTestData))]
    public async Task LambdaToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(LambdaTestData.Data), MemberType = typeof(LambdaTestData))]
    public async Task LambdaFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    protected override Expression Substitute(string id) => LambdaTestData.GetExpression(id);
}