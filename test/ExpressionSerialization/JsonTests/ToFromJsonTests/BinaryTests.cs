namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

[CollectionDefinition("JSON")]
public partial class BinaryTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(JsonTestsFixture.TestFilesPath, "Binary");

    [Theory]
    [MemberData(nameof(BinaryData))]
    public async Task BinaryToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(BinaryData))]
    public async Task BinaryFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> BinaryData = new ()
    {
        { TestLine(), "(a, b) => checked(a - b)",     "SubtractChecked.json" },
        { TestLine(), "(a, b) => a - b",              "Subtract.json" },
        { TestLine(), "(a, b) => a >> b",             "RightShift.json" },
        { TestLine(), "(a, b) => a ^ b",              "Xor.json" },
        { TestLine(), "(a, b) => a || b",             "OrElse.json" },
        { TestLine(), "(a, b) => a | b",              "Or.json" },
        { TestLine(), "(a, b) => a != b",             "NotEqual.json" },
        { TestLine(), "(a, b) => checked(a * b)",     "MultiplyChecked.json" },
        { TestLine(), "(a, b) => a * b",              "Multiply.json" },
        { TestLine(), "(a, b) => a % b",              "Modulo.json" },
        { TestLine(), "(a, b) => a <= b",             "LessThanOrEqual.json" },
        { TestLine(), "(a, b) => a < b",              "LessThan.json" },
        { TestLine(), "(a, b) => a << b",             "LeftShift.json" },
        { TestLine(), "(a, b) => a >= b",             "GreaterThanOrEqual.json" },
        { TestLine(), "(a, b) => a > b",              "GreaterThan.json" },
        { TestLine(), "(a, b) => a ^ b",              "ExclusiveOr.json" },
        { TestLine(), "(a, b) => a == b",             "Equal.json" },
        { TestLine(), "(a, b) => a / b",              "Divide.json" },
        { TestLine(), "(a, b) => a ?? b",             "Coalesce.json" },
        { TestLine(), "(a, i) => a[i]",               "ArrayIndex.json" },
        { TestLine(), "(a, b) => a && b",             "AndAlso.json" },
        { TestLine(), "(a, b) => a & b",              "And.json" },
        { TestLine(), "(a, b) => (a + b) * 42",       "MultiplyAdd.json" },
        { TestLine(), "(a, b) => a + b * 42",         "AddMultiply.json" },
        { TestLine(), "(a, b) => checked(a + b)",     "AddChecked.json" },
        { TestLine(), "(a, b) => a + (b + c)",        "AddParenAdd.json" },
        { TestLine(), "(a, b) => a + b + c",          "AddAdd.json" },
        { TestLine(), "(a, b) => a + b",              "Add.json" },
        { TestLine(), "a => a as b",                  "ExprAsType.json" },
        { TestLine(), "a => a is b",                  "ExprIsType.json" },
        { TestLine(), "a => a equals int",            "ExprTypeEqual.json" },
        { TestLine(), "(a, b) => a ** b",             "Power.json" },
    };

    protected override Expression Substitute(string id) => BinaryTestData.GetExpression(id);

}
