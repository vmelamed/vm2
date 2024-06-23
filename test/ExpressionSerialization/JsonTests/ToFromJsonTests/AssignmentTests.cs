namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

using System.Linq.Expressions;

[CollectionDefinition("JSON")]
public partial class AssignmentTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(JsonTestsFixture.TestFilesPath, "Assignments");

    [Theory]
    [MemberData(nameof(AssignmentsData))]
    public async Task AssignmentToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(AssignmentsData))]
    public async Task AssignmentFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

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
