namespace vm2.ExpressionSerialization.XmlTests;

public partial class LambdaTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string TestConstantsFilesPath => TestsFixture.TestFilesPath + "Lambdas/";

    [Theory]
    [MemberData(nameof(LambdaData))]
    public async Task AssignmentTestAsync(string _, string expressionString, string fileName)
        => await base.TestAsync(expressionString, fileName);

    public static readonly TheoryData<string, string, string> LambdaData = new ()
    {
        { TestLine(), "i => true",              "Param2BoolConstant.xml" },
        { TestLine(), "(s,d) => true",          "2ParamsToConstant.xml" },
        { TestLine(), "a => a._a",              "MemberField.xml" },
        { TestLine(), "a => a.A",               "MemberProperty.xml" },
        { TestLine(), "(s,d) => true",          "StaticMember.xml" },
        { TestLine(), "a => a.Method3(1,1)",    "InstanceMethod2Params.xml" },
        { TestLine(), "(f,a) => f(a)",          "Invocation.xml" },
        { TestLine(), "(a,b) => { int d = 42; a += d; a -= b; int c = 2; a <<= c; }", "Block.xml" },
    };

    protected override Expression Substitute(string id) => _substitutes[id]();

    static Dictionary<string, Func<Expression>> _substitutes = new()
    {
        ["(s,d) => true"]                       = () => (string s, DateTime d) => true,
        ["i => true"]                           = () => (int i) => true,
        ["a => a._a"]                           = () => (TestMethods a) => a._a,
        ["a => a.A"]                            = () => (TestMethods a) => a.A,
        ["a => a.Method1()"]                    = () => () => TestMethods.Method1(),
        ["a => a.Method3(1,1)"]                 = () => (TestMethods a) => a.Method3(1, 1.1),
        ["(f,a) => f(a)"]                       = () => (Func<int, int> f, int a) => f(a),
        ["(a,b) => { int d = 42; a += d; a -= b; int c = 2; a <<= c; }"] = () => Expression.Lambda(
                                                    Expression.Block(
                                                        [Expression.Parameter(typeof(int), "d"),],
                                                        Expression.Assign(
                                                            Expression.Parameter(typeof(int), "d"),
                                                            Expression.Constant(42)),
                                                        Expression.AddAssign(
                                                            Expression.Parameter(typeof(int), "a"),
                                                            Expression.Parameter(typeof(int), "d")),
                                                        Expression.SubtractAssign(
                                                            Expression.Parameter(typeof(int), "a"),
                                                            Expression.Parameter(typeof(int), "b")),
                                                        Expression.Parameter(typeof(int), "c"),
                                                        Expression.Assign(
                                                            Expression.Parameter(typeof(int), "c"),
                                                            Expression.Constant(2)),
                                                        Expression.LeftShiftAssign(
                                                            Expression.Parameter(typeof(int), "a"),
                                                            Expression.Parameter(typeof(int), "c"))
                                                        ),
                                                    Expression.Parameter(typeof(int), "a"),
                                                    Expression.Parameter(typeof(int), "b")
                                                ),
    };
}