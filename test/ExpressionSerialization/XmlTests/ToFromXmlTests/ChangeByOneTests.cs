namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

[CollectionDefinition("XML")]
public partial class ChangeByOneTests(XmlTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(XmlTestsFixture.TestFilesPath, "ChangeByOne");

    [Theory]
    [MemberData(nameof(ChangeByOneExpressionData))]
    public async Task ChangeByOneToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(ChangeByOneExpressionData))]
    public async Task ChangeByOneFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

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
