namespace vm2.ExpressionSerialization.XmlTests;

public partial class AssignmentTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string TestConstantsFilesPath => TestsFixture.TestFilesPath + "Assignments/";

    [Theory]
    [MemberData(nameof(AssignmentsData))]
    public async Task AssignmentTestAsync(string _, string expressionString, string fileName) => await base.TestAsync(expressionString, fileName);

    protected override Expression Substitute(string id) => _substitutes[id];
}
