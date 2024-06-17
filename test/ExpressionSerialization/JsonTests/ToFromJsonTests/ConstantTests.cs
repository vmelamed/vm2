namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

public partial class ConstantTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(TestsFixture.TestFilesPath, "Constants");

    [Theory]
    [MemberData(nameof(ConstantsData))]
    public async Task ConstantToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(ConstantsData))]
    public async Task ConstantFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    protected override Expression Substitute(string id) => _substitutes[id];
}
