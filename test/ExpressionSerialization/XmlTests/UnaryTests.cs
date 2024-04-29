namespace vm2.ExpressionSerialization.XmlTests;
public partial class UnaryTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string TestConstantsFilesPath => TestsFixture.TestFilesPath + "Unary/";

    [Theory]
    [MemberData(nameof(UnaryData))]
    public async Task UnaryTestAsync(string _, string expressionString, string fileName)
        => await base.TestAsync(expressionString, fileName);

    public static readonly TheoryData<string, string, string> UnaryData = new ()
    {
        { TestLine(), "(C c) => c as A",                "AsType.xml" },
        { TestLine(), "(object c) => c as int?",        "ObjectAsNullable.xml" },
        { TestLine(), "(int a) => () => a",             "Quote.xml" },
        { TestLine(), "(double a) => checked((int)a)",  "ConvertChecked.xml" },
        { TestLine(), "(double a) => (int)a",           "Convert.xml" },
        { TestLine(), "(int[] a) => a.Length",          "ArrayLength.xml" },
        { TestLine(), "(bool a) => !a",                 "Not.xml" },
        { TestLine(), "(int a) => checked(-a)",         "NegateChecked.xml" },
        { TestLine(), "(int a) => -a",                  "Negate.xml" },
        { TestLine(), "(int a) => ~a",                  "BitwiseNot.xml" },

        { TestLine(), "(A a) => +a",                    "UnaryPlusMethod.xml" },
        { TestLine(), "(A a) => -a",                    "UnaryMinusMethod.xml" },
        { TestLine(), "(B b) => !b",                    "UnaryNotMethod.xml" },
    };

    protected override Expression Substitute(string id) => _substitutes[id];

    static Dictionary<string, Expression> _substitutes = new()
    {
        ["(C c) => c as A"]                 = (C c) => c as A,
        ["(object c) => c as int?"]         = (object c) => c as int?,
        ["(int a) => () => a"]              = GetQuoteTest(),
        ["(double a) => checked((int)a)"]   = (double a) => checked((int)a),
        ["(double a) => (int)a"]            = (double a) => (int)a,
        ["(int[] a) => a.Length"]                 = (int[] a) => a.Length,
        ["(bool a) => !a"]                  = (bool a) => !a,
        ["(int a) => checked(-a)"]          = (int a) => checked(-a),
        ["(int a) => -a"]                   = (int a) => -a,
        ["(int a) => ~a"]                   = (int a) => ~a,

        ["(A a) => +a"]                     = (A a) => +a,
        ["(A a) => -a"]                     = (A a) => -a,
        ["(B b) => !b"]                     = (B b) => !b,
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