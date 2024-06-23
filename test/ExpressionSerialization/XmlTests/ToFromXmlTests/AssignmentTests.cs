namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

using System.Linq.Expressions;

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
        { TestLine(), "a = 1",              "AssignConstant.xml" },
        { TestLine(), "a = b",              "AssignVariable.xml" },
        { TestLine(), "a += b",             "AddAssign.xml" },
        { TestLine(), "checked(a += b)",    "AddAssignChecked.xml" },
        { TestLine(), "a -= b",             "SubtractAssign.xml" },
        { TestLine(), "checked(a -= b)",    "SubtractAssignChecked.xml" },
        { TestLine(), "a *= b",             "MultiplyAssign.xml" },
        { TestLine(), "checked(a *= b)",    "MultiplyAssignChecked.xml" },
        { TestLine(), "a /= b",             "DivideAssign.xml" },
        { TestLine(), "a %= b",             "ModuloAssign.xml" },
        { TestLine(), "a &= b",             "AndAssign.xml" },
        { TestLine(), "a |= b",             "OrAssign.xml" },
        { TestLine(), "a ^= b",             "XorAssign.xml" },
        { TestLine(), "x **= z",            "PowerAssign.json" },
        { TestLine(), "a <<= b",            "LShiftAssign.xml" },
        { TestLine(), "a >>= b",            "RShiftAssign.xml" },
    };

    protected override Expression Substitute(string id) => AssignmentTestData.GetExpression(id);
}
