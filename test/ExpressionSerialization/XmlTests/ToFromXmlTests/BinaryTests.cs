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
        { TestLine(), "(a, b) => checked(a - b)",     "SubtractChecked.xml" },
        { TestLine(), "(a, b) => a - b",              "Subtract.xml" },
        { TestLine(), "(a, b) => a >> b",             "RightShift.xml" },
        { TestLine(), "(a, b) => a ^ b",              "Xor.xml" },
        { TestLine(), "(a, b) => a || b",             "OrElse.xml" },
        { TestLine(), "(a, b) => a | b",              "Or.xml" },
        { TestLine(), "(a, b) => a != b",             "NotEqual.xml" },
        { TestLine(), "(a, b) => checked(a * b)",     "MultiplyChecked.xml" },
        { TestLine(), "(a, b) => a * b",              "Multiply.xml" },
        { TestLine(), "(a, b) => a % b",              "Modulo.xml" },
        { TestLine(), "(a, b) => a <= b",             "LessThanOrEqual.xml" },
        { TestLine(), "(a, b) => a < b",              "LessThan.xml" },
        { TestLine(), "(a, b) => a << b",             "LeftShift.xml" },
        { TestLine(), "(a, b) => a >= b",             "GreaterThanOrEqual.xml" },
        { TestLine(), "(a, b) => a > b",              "GreaterThan.xml" },
        { TestLine(), "(a, b) => a ^ b",              "ExclusiveOr.xml" },
        { TestLine(), "(a, b) => a == b",             "Equal.xml" },
        { TestLine(), "(a, b) => a / b",              "Divide.xml" },
        { TestLine(), "(a, b) => a ?? b",             "Coalesce.xml" },
        { TestLine(), "(a, i) => a[i]",               "ArrayIndex.xml" },
        { TestLine(), "(a, b) => a && b",             "AndAlso.xml" },
        { TestLine(), "(a, b) => a & b",              "And.xml" },
        { TestLine(), "(a, b) => (a + b) * 42",       "MultiplyAdd.xml" },
        { TestLine(), "(a, b) => a + b * 42",         "AddMultiply.xml" },
        { TestLine(), "(a, b) => checked(a + b)",     "AddChecked.xml" },
        { TestLine(), "(a, b) => a + (b + c)",        "AddParenAdd.xml" },
        { TestLine(), "(a, b) => a + b + c",          "AddAdd.xml" },
        { TestLine(), "(a, b) => a + b",              "Add.xml" },
        { TestLine(), "a => a as b",                  "ExprAsType.xml" },
        { TestLine(), "a => a is b",                  "ExprIsType.xml" },
        { TestLine(), "a => a equals int",            "ExprTypeEqual.xml" },
        { TestLine(), "(a, b) => a ** b",             "Power.xml" },
    };

    protected override Expression Substitute(string id) => BinaryTestData.GetExpression(id);
}
