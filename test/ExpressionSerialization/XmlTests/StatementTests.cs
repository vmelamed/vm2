namespace vm2.ExpressionSerialization.XmlTests;

public partial class StatementTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string TestConstantsFilesPath => TestsFixture.TestFilesPath + "Statements/";

    [Theory]
    [MemberData(nameof(StatementData))]
    public async Task StatementTestAsync(string _, string expressionString, string fileName)
        => await base.TestAsync(expressionString, fileName);

    public static readonly TheoryData<string, string, string> StatementData = new ()
    {
        { TestLine(), "() => new StructDataContract1(42, \"don't panic\")", "New.xml" },
        { TestLine(), "b => b ? 1 : 3",         "Conditional.xml" },
        { TestLine(), "(f,a) => f(a)",          "Invocation.xml" },
        { TestLine(), "(a,b) => { int d = 42; a += d; a -= b; int c = 2; a <<= c; }", "Block.xml" },
        { TestLine(), "loop",                   "Loop.xml" },
    };

    protected override Expression Substitute(string id) => _substitutes[id]();

    static Dictionary<string, Func<Expression>> _substitutes = new()
    {
        ["() => new StructDataContract1(42, \"don't panic\")"]
                                                = () => () => new StructDataContract1(42, "don't panic"),
        ["b => b ? 1 : 3"]                      = () => (bool b) => b ? 1 : 3,
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
                                                    Expression.Parameter(typeof(int), "b")),
        ["loop"]                                = () =>
                                                    {
                                                        var value = Expression.Parameter(typeof(int), "value");
                                                        var result = Expression.Parameter(typeof(int), "result");
                                                        var label = Expression.Label(typeof(int));

                                                        return Expression.Lambda(
                                                            Expression.Block(
                                                                new[] { result },
                                                                Expression.Assign(value, Expression.Constant(5)),
                                                                Expression.Assign(result, Expression.Constant(1)),
                                                                Expression.Loop(
                                                                    Expression.IfThenElse(
                                                                        Expression.GreaterThan(value, Expression.Constant(1)),
                                                                        Expression.MultiplyAssign(result, Expression.PostDecrementAssign(value)),
                                                                        Expression.Break(label, result)),
                                                                    label)),
                                                            value
                                                        );
                                                    },
    };
}