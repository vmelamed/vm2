namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

[CollectionDefinition("JSON")]
public partial class LambdaTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(JsonTestsFixture.TestFilesPath, "Lambdas");

    [Theory]
    [MemberData(nameof(LambdaData))]
    public async Task LambdaToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(LambdaData))]
    public async Task LambdaFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> LambdaData = new ()
    {
        { TestLine(), "i => true",              "Param2BoolConstant.json" },
        { TestLine(), "i => i",                 "Param1Ret1Constant.json" },
        { TestLine(), "(i, j) => j",            "Param2Ret2Constant.json" },
        //{ TestLine(), "(s,d) => true",          "2ParamsToConstant.json" },
        //{ TestLine(), "a => a._a",              "MemberField.json" },
        //{ TestLine(), "a => a.A",               "MemberProperty.json" },
        //{ TestLine(), "(s,d) => true",          "StaticMember.json" },
        //{ TestLine(), "a => a.Method3(1,1)",    "InstanceMethod3Params.json" },
        //{ TestLine(), "a => a.Method4(42,3.14)","InstanceMethod4Params.json" },
        //{ TestLine(), "(i, j) => (a=i)+(b=j)",  "Param2Var1Ret2nd.xml" },
    };

    protected override Expression Substitute(string id) => _substitutes[id]();

    static ParameterExpression _paramI = Expression.Parameter(typeof(int), "i");
    static ParameterExpression _paramJ = Expression.Parameter(typeof(double), "j");
    static ParameterExpression _varA = Expression.Parameter(typeof(int), "a");
    static ParameterExpression _varB = Expression.Parameter(typeof(double), "b");

    static Dictionary<string, Func<Expression>> _substitutes = new()

    {
        ["i => true"]                           = () => (int i) => true,
        ["i => i"]                              = () => (int i) => i,
        ["(i, j) => j"]                         = () => (int i, double j) => j,
        ["(i, j) => (a=i)+(b=j)"]               = () => // (int i, double j) => { int a; double b; a = i; b = j; return a + b },
                                                        Expression.Lambda(
                                                            Expression.Block(
                                                                [_varA, _varB],
                                                                Expression.Assign( _varA, _paramI ),
                                                                Expression.Assign( _varB, _paramJ ),
                                                                Expression.Add(Expression.Convert(_varA, typeof(double)), _varB)),
                                                            _paramI, _paramJ),
        ["(s,d) => true"]                       = () => (string s, DateTime d) => true,
        ["a => a._a"]                           = () => (TestMethods a) => a._a,
        ["a => a.A"]                            = () => (TestMethods a) => a.A,
        ["a => a.Method1()"]                    = () => () => TestMethods.Method1(),
        ["a => a.Method3(1,1)"]                 = () => (TestMethods a) => a.Method3(1, 1.1),
        ["a => a.Method4(42,3.14)"]             = () => (TestMethods a) => a.Method4(42, 3.14),
    };
}