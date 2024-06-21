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

    protected override Expression Substitute(string id) => _substitutes[id];

    static Dictionary<string, Expression> _substitutes = new()
    {
        ["default(bool)"]                   = Expression.Default(typeof(bool)),
        ["default(char)"]                   = Expression.Default(typeof(char)),
        ["default(double)"]                 = Expression.Default(typeof(double)),
        ["default(half)"]                   = Expression.Default(typeof(Half)),
        ["default(int)"]                    = Expression.Default(typeof(int)),
        ["default(long)"]                   = Expression.Default(typeof(long)),
        ["default(DateTime)"]               = Expression.Default(typeof(DateTime)),
        ["default(DateTimeOffset)"]         = Expression.Default(typeof(DateTimeOffset)),
        ["default(TimeSpan)"]               = Expression.Default(typeof(TimeSpan)),
        ["default(decimal)"]                = Expression.Default(typeof(decimal)),
        ["default(Guid)"]                   = Expression.Default(typeof(Guid)),
        ["default(string)"]                 = Expression.Default(typeof(string)),
        ["default(object)"]                 = Expression.Default(typeof(object)),
        ["default(ClassDataContract1)"]     = Expression.Default(typeof(ClassDataContract1)),
        ["default(StructDataContract1)"]    = Expression.Default(typeof(StructDataContract1)),
        ["default(int?)"]                   = Expression.Default(typeof(int?)),
        ["default(StructDataContract1?)"]   = Expression.Default(typeof(StructDataContract1?)),

        ["(C c) => c as A"]                 = (C c) => c as A,
        ["(object c) => c as int?"]         = (object c) => c as int?,
        ["(int a) => () => a"]              = GetQuoteTest(),
        ["(double a) => checked((int)a)"]   = (double a) => checked((int)a),
        ["(double a) => (int)a"]            = (double a) => (int)a,
        ["(int[] a) => a.GetLength"]        = (int[] a) => a.Length,
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