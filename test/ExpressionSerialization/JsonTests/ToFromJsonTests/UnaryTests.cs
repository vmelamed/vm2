namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

[CollectionDefinition("JSON")]
public partial class UnaryTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(JsonTestsFixture.TestFilesPath, "Unary");

    [Theory]
    [MemberData(nameof(UnaryData))]
    public async Task UnaryToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(UnaryData))]
    public async Task UnaryFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> UnaryData = new ()
    {
        { TestLine(), "default(bool)",                  "Default.bool.json" },
        { TestLine(), "default(char)",                  "Default.char.json" },
        { TestLine(), "default(double)",                "Default.double.json" },
        { TestLine(), "default(half)",                  "Default.half.json" },
        { TestLine(), "default(int)",                   "Default.int.json" },
        { TestLine(), "default(long)",                  "Default.long.json" },
        { TestLine(), "default(DateTime)",              "Default.DateTime.json" },
        { TestLine(), "default(DateTimeOffset)",        "Default.DateTimeOffset.json" },
        { TestLine(), "default(TimeSpan)",              "Default.TimeSpan.json" },
        { TestLine(), "default(decimal)",               "Default.decimal.json" },
        { TestLine(), "default(Guid)",                  "Default.Guid.json" },
        { TestLine(), "default(string)",                "Default.string.json" },
        { TestLine(), "default(object)",                "Default.object.json" },
        { TestLine(), "default(ClassDataContract1)",    "Default.ClassDataContract1.json" },
        { TestLine(), "default(StructDataContract1)",   "Default.StructDataContract1.json" },
        { TestLine(), "default(int?)",                  "Default.int0.json" },
        { TestLine(), "default(StructDataContract1?)",  "Default.StructDataContract10.json" },

        { TestLine(), "(C c) => c as A",                "AsType.json" },
        { TestLine(), "(object c) => c as int?",        "ObjectAsNullable.json" },
        { TestLine(), "(int a) => () => a",             "Quote.json" },
        { TestLine(), "(double a) => checked((int)a)",  "ConvertChecked.json" },
        { TestLine(), "(double a) => (int)a",           "Convert.json" },
        { TestLine(), "(int[] a) => a.GetLength",       "ArrayLength.json" },
        { TestLine(), "(bool a) => !a",                 "Not.json" },
        { TestLine(), "(int a) => checked(-a)",         "NegateChecked.json" },
        { TestLine(), "(int a) => -a",                  "Negate.json" },
        { TestLine(), "(int a) => ~a",                  "BitwiseNot.json" },

        { TestLine(), "(A a) => +a",                    "UnaryPlusMethod.json" },
        { TestLine(), "(A a) => -a",                    "UnaryMinusMethod.json" },
        { TestLine(), "(B b) => !b",                    "UnaryNotMethod.json" },
    };

    protected override Expression Substitute(string id) => UnaryTestData.GetExpression(id);
}