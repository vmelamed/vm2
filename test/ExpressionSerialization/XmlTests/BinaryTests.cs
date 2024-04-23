namespace vm2.ExpressionSerialization.XmlTests;

public partial class BinaryTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string TestConstantsFilesPath => TestsFixture.TestFilesPath + "Binary/";

    [Theory]
    [MemberData(nameof(BinaryData))]
    public async Task BinaryTestAsync(string _, string expressionString, string fileName) => await base.TestAsync(expressionString, fileName);

    protected override Expression Substitute(string id) => _substitutes[id];
}
