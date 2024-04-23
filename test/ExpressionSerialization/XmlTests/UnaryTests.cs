namespace vm2.ExpressionSerialization.XmlTests;
public partial class UnaryTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string TestConstantsFilesPath => TestsFixture.TestFilesPath + "Unary/";

    [Theory]
    [MemberData(nameof(UnaryData))]
    public async Task AssignmentTestAsync(string _, string expressionString, string fileName)
        => await base.TestAsync(expressionString, fileName);

    public static readonly TheoryData<string, string, string> UnaryData = new ()
    {
        { TestLine(), "(C c) => c as A",                "AsType.xml" },
        { TestLine(), "(int a) => () => a",             "Quote.xml" },
        { TestLine(), "(double a) => checked((int)a)",  "ConvertChecked.xml" },
        { TestLine(), "(double a) => (int)a",           "Convert.xml" },
        { TestLine(), "(int[] a) => !a",                "ArrayLength.xml" },
        { TestLine(), "(bool a) => !a",                 "Not.xml" },
        { TestLine(), "(int a) => checked(-a)",         "NegateChecked.xml" },
        { TestLine(), "(int a) => -a",                  "Negate.xml" },

        { TestLine(), "(A a) => +a",                    "UnaryPlusMethod.xml" },
        { TestLine(), "(A a) => -a",                    "UnaryMinusMethod.xml" },
    };

    protected override Expression Substitute(string id) => _substitutes[id];

    static Dictionary<string, Expression> _substitutes = new()
    {
        ["(C c) => c as A"]                 = (C c) => c as A,
        ["(int a) => () => a"]              = GetQuoteTest(),
        ["(double a) => checked((int)a)"]   = (double a) => checked((int)a),
        ["(double a) => (int)a"]            = (double a) => (int)a,
        ["(int[] a) => !a"]                 = (int[] a) => a.Length,
        ["(bool a) => !a"]                  = (bool a) => !a,
        ["(int a) => checked(-a)"]          = (int a) => checked(-a),
        ["(int a) => -a"]                   = (int a) => -a,

        ["(A a) => +a"]                     = (A a) => +a,
        ["(A a) => -a"]                     = (A a) => -a,
    };

    static LambdaExpression GetQuoteTest()
    {
        var pa = Expression.Parameter(typeof(int), "a");

        return Expression.Lambda(
                    Expression.Quote(
                        Expression.Lambda(pa)),
                    pa);
    }
}