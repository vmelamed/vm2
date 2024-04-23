namespace vm2.ExpressionSerialization.XmlTests;

public partial class ChangeByOneTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string TestConstantsFilesPath => TestsFixture.TestFilesPath + "ChangeByOne/";

    [Theory]
    [MemberData(nameof(ChangeByOneExpressionData))]
    public async Task ChangeByOneTestAsync(string _, string expressionString, string fileName) => await base.TestAsync(expressionString, fileName);

    protected override Expression Substitute(string id) => _substitutes[id];
}
