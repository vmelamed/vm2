namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

[CollectionDefinition("XML")]
public partial class LambdaTests(XmlTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(XmlTestsFixture.TestFilesPath, "Lambdas");

    [Theory]
    [MemberData(nameof(LambdaData))]
    public async Task LambdaToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(LambdaData))]
    public async Task LambdaFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> LambdaData = new ()
    {
        { TestLine(), "i => true",              "Param2BoolConstant.xml" },
        { TestLine(), "i => i",                 "Param1Ret1.xml" },
        { TestLine(), "(i, j) => j",            "Param2Ret2nd.xml" },
        { TestLine(), "(s,d) => true",          "2ParamsToConstant.xml" },
        { TestLine(), "a => a._a",              "MemberField.xml" },
        { TestLine(), "a => a.A",               "MemberProperty.xml" },
        { TestLine(), "(s,d) => true",          "StaticMember.xml" },
        { TestLine(), "a => a.Method3(1,1)",    "InstanceMethod3Params.xml" },
        { TestLine(), "a => a.Method4(42,3.14)","InstanceMethod4Params.xml" },
        { TestLine(), "(i, j) => (a=i)+(b=j)",  "Param2Var1Ret2nd.xml" },
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
        ["(s,d) => true"]                       = () => (string s, DateTime d) => true,
        ["a => a._a"]                           = () => (TestMethods a) => a._a,
        ["a => a.A"]                            = () => (TestMethods a) => a.A,
        ["a => a.Method1()"]                    = () => () => TestMethods.Method1(),
        ["a => a.Method3(1,1)"]                 = () => (TestMethods a) => a.Method3(1, 1.1),
        ["a => a.Method4(42,3.14)"]             = () => (TestMethods a) => a.Method4(42, 3.14),
        ["(i, j) => (a=i)+(b=j)"]               = () => // (int i, double j) => { int a; double b; a = i; b = j; return a + b },
                                                        Expression.Lambda(
                                                            Expression.Block(
                                                                [_varA, _varB],
                                                                Expression.Assign( _varA, _paramI ),
                                                                Expression.Assign( _varB, _paramJ ),
                                                                Expression.Add(Expression.Convert(_varA, typeof(double)), _varB)),
                                                            _paramI, _paramJ),
    };
}