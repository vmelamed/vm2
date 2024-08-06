namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

[CollectionDefinition("XML")]
public partial class AssignmentTests(XmlTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(XmlTestsFixture.TestFilesPath, "Assignments");

    [Theory]
    [MemberData(nameof(AssignmentsData))]
    public async Task AssignmentToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(AssignmentsData))]
    public async Task AssignmentFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> AssignmentsData = new ()
    {
        { TestLine(), "a = 1",              "AssignConstant.json" },
        { TestLine(), "a = b",              "AssignVariable.json" },
        { TestLine(), "a += b",             "AddAssign.json" },
        { TestLine(), "checked(a += b)",    "AddAssignChecked.json" },
        { TestLine(), "a -= b",             "SubtractAssign.json" },
        { TestLine(), "checked(a -= b)",    "SubtractAssignChecked.json" },
        { TestLine(), "a *= b",             "MultiplyAssign.json" },
        { TestLine(), "checked(a *= b)",    "MultiplyAssignChecked.json" },
        { TestLine(), "a /= b",             "DivideAssign.json" },
        { TestLine(), "a %= b",             "ModuloAssign.json" },
        { TestLine(), "a &= b",             "AndAssign.json" },
        { TestLine(), "a |= b",             "OrAssign.json" },
        { TestLine(), "a ^= b",             "XorAssign.json" },
        { TestLine(), "x **= z",            "PowerAssign.json" },
        { TestLine(), "a <<= b",            "LShiftAssign.json" },
        { TestLine(), "a >>= b",            "RShiftAssign.json" },
    };

    protected override Expression Substitute(string id) => AssignmentTestData.GetExpression(id);
}
