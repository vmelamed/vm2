namespace vm2.ExpressionSerialization.XmlTests;

[CollectionDefinition("XML")]
public partial class AssignmentTests(XmlTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(XmlTestsFixture.TestFilesPath, "Assignments");

    [Theory]
    [MemberData(nameof(AssignmentTestData.Data), MemberType = typeof(AssignmentTestData))]
    public async Task AssignmentToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(AssignmentTestData.Data), MemberType = typeof(AssignmentTestData))]
    public async Task AssignmentFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

    protected override Expression Substitute(string id) => AssignmentTestData.GetExpression(id);
}
