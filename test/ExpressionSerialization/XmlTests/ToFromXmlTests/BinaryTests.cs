namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

[CollectionDefinition("XML")]
public partial class BinaryTests(XmlTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(XmlTestsFixture.TestFilesPath, "Binary");

    [Theory]
    [MemberData(nameof(BinaryData))]
    public async Task BinaryToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(BinaryData))]
    public async Task BinaryFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> BinaryData = new ()
    {
        { TestLine(), "(a, b) => checked(a - b)",     "SubtractChecked" },
        { TestLine(), "(a, b) => a - b",              "Subtract" },
        { TestLine(), "(a, b) => a >> b",             "RightShift" },
        { TestLine(), "(a, b) => a ^ b",              "Xor" },
        { TestLine(), "(a, b) => a || b",             "OrElse" },
        { TestLine(), "(a, b) => a | b",              "Or" },
        { TestLine(), "(a, b) => a != b",             "NotEqual" },
        { TestLine(), "(a, b) => checked(a * b)",     "MultiplyChecked" },
        { TestLine(), "(a, b) => a * b",              "Multiply" },
        { TestLine(), "(a, b) => a % b",              "Modulo" },
        { TestLine(), "(a, b) => a <= b",             "LessThanOrEqual" },
        { TestLine(), "(a, b) => a < b",              "LessThan" },
        { TestLine(), "(a, b) => a << b",             "LeftShift" },
        { TestLine(), "(a, b) => a >= b",             "GreaterThanOrEqual" },
        { TestLine(), "(a, b) => a > b",              "GreaterThan" },
        { TestLine(), "(a, b) => a ^ b",              "ExclusiveOr" },
        { TestLine(), "(a, b) => a == b",             "Equal" },
        { TestLine(), "(a, b) => a / b",              "Divide" },
        { TestLine(), "(a, b) => a ?? b",             "Coalesce" },
        { TestLine(), "(a, i) => a[i]",               "ArrayIndex" },
        { TestLine(), "(a, b) => a && b",             "AndAlso" },
        { TestLine(), "(a, b) => a & b",              "And" },
        { TestLine(), "(a, b) => (a + b) * 42",       "MultiplyAdd" },
        { TestLine(), "(a, b) => a + b * 42",         "AddMultiply" },
        { TestLine(), "(a, b) => checked(a + b)",     "AddChecked" },
        { TestLine(), "(a, b) => a + (b + c)",        "AddParenAdd" },
        { TestLine(), "(a, b) => a + b + c",          "AddAdd" },
        { TestLine(), "(a, b) => a + b",              "Add" },
        { TestLine(), "a => a as b",                  "ExprAsType" },
        { TestLine(), "a => a is b",                  "ExprIsType" },
        { TestLine(), "a => a equals int",            "ExprTypeEqual" },
        { TestLine(), "(a, b) => a ** b",             "Power" },
    };

    protected override Expression Substitute(string id) => BinaryTestData.GetExpression(id);
}
