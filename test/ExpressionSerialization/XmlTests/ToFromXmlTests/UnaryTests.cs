namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

[CollectionDefinition("XML")]
public partial class UnaryTests(XmlTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(XmlTestsFixture.TestFilesPath, "Unary");

    [Theory]
    [MemberData(nameof(UnaryData))]
    public async Task UnaryToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(UnaryData))]
    public async Task UnaryFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> UnaryData = new ()
    {
        { TestLine(), "default(bool)",                  "Default.bool.xml" },
        { TestLine(), "default(char)",                  "Default.char.xml" },
        { TestLine(), "default(double)",                "Default.double.xml" },
        { TestLine(), "default(half)",                  "Default.half.xml" },
        { TestLine(), "default(int)",                   "Default.int.xml" },
        { TestLine(), "default(long)",                  "Default.long.xml" },
        { TestLine(), "default(DateTime)",              "Default.DateTime.xml" },
        { TestLine(), "default(DateTimeOffset)",        "Default.DateTimeOffset.xml" },
        { TestLine(), "default(TimeSpan)",              "Default.TimeSpan.xml" },
        { TestLine(), "default(decimal)",               "Default.decimal.xml" },
        { TestLine(), "default(Guid)",                  "Default.Guid.xml" },
        { TestLine(), "default(string)",                "Default.string.xml" },
        { TestLine(), "default(object)",                "Default.object.xml" },
        { TestLine(), "default(ClassDataContract1)",    "Default.ClassDataContract1.xml" },
        { TestLine(), "default(StructDataContract1)",   "Default.StructDataContract1.xml" },
        { TestLine(), "default(bool)",                  "Default.bool.xml" },
        { TestLine(), "default(int?)",                  "Default.int0.json" },
        { TestLine(), "default(StructDataContract1?)",  "Default.StructDataContract10.json" },

        { TestLine(), "(C c) => c as A",                "AsType.xml" },
        { TestLine(), "(object c) => c as int?",        "ObjectAsNullable.xml" },
        { TestLine(), "(int a) => () => a",             "Quote.xml" },
        { TestLine(), "(double a) => checked((int)a)",  "ConvertChecked.xml" },
        { TestLine(), "(double a) => (int)a",           "Convert.xml" },
        { TestLine(), "(int[] a) => a.GetLength",       "ArrayLength.xml" },
        { TestLine(), "(bool a) => !a",                 "Not.xml" },
        { TestLine(), "(int a) => checked(-a)",         "NegateChecked.xml" },
        { TestLine(), "(int a) => -a",                  "Negate.xml" },
        { TestLine(), "(int a) => ~a",                  "BitwiseNot.xml" },

        { TestLine(), "(A a) => +a",                    "UnaryPlusMethod.xml" },
        { TestLine(), "(A a) => -a",                    "UnaryMinusMethod.xml" },
        { TestLine(), "(B b) => !b",                    "UnaryNotMethod.xml" },
    };

    protected override Expression Substitute(string id) => UnaryTestData.GetExpression(id);
}