namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

[CollectionDefinition("JSON")]
public partial class ChangeByOneTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(JsonTestsFixture.TestFilesPath, "ChangeByOne");

    [Theory]
    [MemberData(nameof(ChangeByOneExpressionData))]
    public async Task ChangeByOneToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(ChangeByOneExpressionData))]
    public async Task ChangeByOneFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> ChangeByOneExpressionData = new ()
    {
        { TestLine(), "a => increment(a)", "Increment" },
        { TestLine(), "a => decrement(a)", "Decrement" },
        { TestLine(), "a => ++a",          "PreIncrementAssign" },
        { TestLine(), "a => a++",          "PostIncrementAssign" },
        { TestLine(), "a => --a",          "PreDecrementAssign" },
        { TestLine(), "a => a--",          "PostDecrementAssign" },
    };

    protected override Expression Substitute(string id) => ChangeByOneTestData.GetExpression(id);
}
