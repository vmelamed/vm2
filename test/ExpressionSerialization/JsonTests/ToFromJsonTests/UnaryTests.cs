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
        { TestLine(), "default(bool)",                  "Default.bool" },
        { TestLine(), "default(char)",                  "Default.char" },
        { TestLine(), "default(double)",                "Default.double" },
        { TestLine(), "default(half)",                  "Default.half" },
        { TestLine(), "default(int)",                   "Default.int" },
        { TestLine(), "default(long)",                  "Default.long" },
        { TestLine(), "default(DateTime)",              "Default.DateTime" },
        { TestLine(), "default(DateTimeOffset)",        "Default.DateTimeOffset" },
        { TestLine(), "default(TimeSpan)",              "Default.TimeSpan" },
        { TestLine(), "default(decimal)",               "Default.decimal" },
        { TestLine(), "default(Guid)",                  "Default.Guid" },
        { TestLine(), "default(string)",                "Default.string" },
        { TestLine(), "default(object)",                "Default.object" },
        { TestLine(), "default(ClassDataContract1)",    "Default.ClassDataContract1" },
        { TestLine(), "default(StructDataContract1)",   "Default.StructDataContract1" },
        { TestLine(), "default(int?)",                  "Default.int0" },
        { TestLine(), "default(StructDataContract1?)",  "Default.StructDataContract10" },

        { TestLine(), "(C c) => c as A",                "AsType" },
        { TestLine(), "(object c) => c as int?",        "ObjectAsNullable" },
        { TestLine(), "(int a) => () => a",             "Quote" },
        { TestLine(), "(double a) => checked((int)a)",  "ConvertChecked" },
        { TestLine(), "(double a) => (int)a",           "Convert" },
        { TestLine(), "(int[] a) => a.GetLength",       "ArrayLength" },
        { TestLine(), "(bool a) => !a",                 "Not" },
        { TestLine(), "(int a) => checked(-a)",         "NegateChecked" },
        { TestLine(), "(int a) => -a",                  "Negate" },
        { TestLine(), "(int a) => ~a",                  "BitwiseNot" },

        { TestLine(), "(A a) => +a",                    "UnaryPlusMethod" },
        { TestLine(), "(A a) => -a",                    "UnaryMinusMethod" },
        { TestLine(), "(B b) => !b",                    "UnaryNotMethod" },
    };

    protected override Expression Substitute(string id) => UnaryTestData.GetExpression(id);
}