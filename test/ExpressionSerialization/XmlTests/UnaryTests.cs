namespace vm2.ExpressionSerialization.XmlTests;

public partial class UnaryTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string TestConstantsFilesPath => TestsFixture.TestFilesPath + "Unary/";

    [Theory]
    [MemberData(nameof(UnaryData))]
    public async Task AssignmentTestAsync(string _, string expressionString, string fileName) => await base.TestAsync(expressionString, fileName);

    protected override Expression Substitute(string id) => _substitutes[id];
}