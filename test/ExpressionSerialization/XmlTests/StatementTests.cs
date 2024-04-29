namespace vm2.ExpressionSerialization.XmlTests;

using System.Reflection;

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
        { TestLine(), "b => b ? 1 : 3",     "Conditional.xml" },
        { TestLine(), "(f,a) => f(a)",      "Invocation.xml" },
        { TestLine(), "(a,b) => { ... }",   "Block.xml" },
        { TestLine(), "loop",               "Loop.xml" },
        { TestLine(), "switch(a){ ... }",   "Switch.xml" },
        { TestLine(), "Console.WriteLine",  "Invocation2.xml" },
        { TestLine(), "try1",               "try1.xml" },
        { TestLine(), "try2",               "try2.xml" },
        { TestLine(), "try3",               "try3.xml" },
        { TestLine(), "try4",               "try4.xml" },
        { TestLine(), "try5",               "try5.xml" },
        { TestLine(), "try6",               "try6.xml" },
    };

    protected override Expression Substitute(string id) => _substitutes[id]();

    static Func<Expression> _lambdaWithBlock = () =>
    {
        var a = Expression.Parameter(typeof(int), "a");
        var b = Expression.Parameter(typeof(int), "b");
        var c = Expression.Parameter(typeof(int), "c");
        var d = Expression.Parameter(typeof(int), "d");

        return Expression.Lambda(
                    Expression.Block(
                        [ d, ],
                        Expression.Assign(d, Expression.Constant(42)),
                        Expression.AddAssign(a, d),
                        Expression.SubtractAssign(a, b),
                        c,
                        Expression.Assign(c, Expression.Constant(2)),
                        Expression.LeftShiftAssign(a, c)),
                    a, b);
    };

    static Func<Expression> _lambdaWithLoopContinueBreak = () =>
    {
        var value  = Expression.Parameter(typeof(int), "value");
        var result = Expression.Parameter(typeof(int), "result");
        var labelCont  = Expression.Label("continue");
        var labelBreak = Expression.Label("break");

        return Expression.Lambda(
                Expression.Block(
                    new[] { result },
                    Expression.Assign(value, Expression.Constant(5)),
                    Expression.Assign(result, Expression.Constant(1)),
                    Expression.Loop(
                        Expression.Block(
                            [],
                            Expression.IfThenElse(
                                Expression.GreaterThan(value, Expression.Constant(1)),
                                Expression.Block(
                                    [],
                                    Expression.MultiplyAssign(result, Expression.PostDecrementAssign(value)),
                                    Expression.Continue(labelCont)),
                                Expression.Break(labelBreak))),
                        labelBreak,
                        labelCont)),
                value
            );
    };

    static MethodInfo _miWriteLine = typeof(Console).GetMethod("WriteLine", [ typeof(string) ])!;
    static Expression WriteLine1Expression(string s) => Expression.Call(null, _miWriteLine, Expression.Constant(s));
    static Expression ExceptionDefaultCtor() => Expression.New(typeof(Exception).GetConstructor(Type.EmptyTypes)!);
    static Expression ThrowException() => Expression.Throw(ExceptionDefaultCtor());

    static Func<Expression> _switch = () =>
    {
        var a = Expression.Parameter(typeof(int), "a");

        return Expression.Switch(
                    a,
                    WriteLine1Expression("Default"),
                    Expression.SwitchCase(
                        WriteLine1Expression("First"),
                        Expression.Constant(1)),
                    Expression.SwitchCase(
                        WriteLine1Expression("Second"),
                        Expression.Constant(2),
                        Expression.Constant(3),
                        Expression.Constant(4))
                    );
    };

    static Func<Expression> _try1 = () =>
        Expression.TryFault(
            Expression.Block(
                new Expression[]
                {
                    WriteLine1Expression("TryBody"),
                    ThrowException(),
                }),
            WriteLine1Expression("catch {}"));

    static Func<Expression> _try2 = () =>
        Expression.TryCatch(
            Expression.Block(
                new Expression[]
                {
                    WriteLine1Expression("TryBody"),
                    ThrowException(),
                }),
                [
                    Expression.MakeCatchBlock(
                        typeof(ArgumentException),
                        null,
                        WriteLine1Expression("catch (ArgumentException) {}"),
                        null),
                ]);
    static Func<Expression> _try3 = () =>
        Expression.TryCatchFinally(
            Expression.Block(
                new Expression[]
                {
                    WriteLine1Expression("TryBody"),
                    ThrowException(),
                }),
            WriteLine1Expression("finally {}"),
            [
                Expression.MakeCatchBlock(
                    typeof(ArgumentException),
                    null,
                    WriteLine1Expression("catch (ArgumentException) {}"),
                    null),
            ]);
    static Func<Expression> _try4 = () =>
        Expression.TryFinally(
            Expression.Block(
                new Expression[]
                {
                    WriteLine1Expression("TryBody"),
                    ThrowException(),
                }),
            WriteLine1Expression("finally {}"));

    static Func<Expression> _try5 = () =>
    {
        var exception = Expression.Parameter(typeof(ArgumentException), "x");
        return Expression.TryCatch(
                    Expression.Block(
                        new Expression[]
                        {
                            WriteLine1Expression("TryBody"),
                            ThrowException(),
                        }),
                    [
                        Expression.MakeCatchBlock(
                            typeof(ArgumentException),
                            exception,
                            Expression.Call(
                                        null,
                                        _miWriteLine,
                                        Expression.MakeMemberAccess(exception, typeof(ArgumentException).GetProperty("Message")!)),
                            null),
                    ]);
    };
    static Func<Expression> _try6 = () =>
    {
        var exception = Expression.Parameter(typeof(ArgumentException), "x");
        return Expression.TryCatch(
                    Expression.Block(
                        WriteLine1Expression("TryBody"),
                        ThrowException()),
                    [
                        Expression.MakeCatchBlock(
                            typeof(ArgumentException),
                            exception,
                            Expression.Call(
                                        null,
                                        _miWriteLine,
                                        Expression.MakeMemberAccess(exception, typeof(ArgumentException).GetProperty("ParamName")!)),
                            Expression.Equal(
                                Expression.MakeMemberAccess(exception, typeof(ArgumentException).GetProperty("ParamName")!),
                                Expression.Constant("x"))),
                    ]);
    };

    static Dictionary<string, Func<Expression>> _substitutes = new()
    {
        ["() => new StructDataContract1(42, \"don't panic\")"]
                              = () => () => new StructDataContract1(42, "don't panic"),
        ["b => b ? 1 : 3"]    = () => (bool b) => b ? 1 : 3,
        ["(f,a) => f(a)"]     = () => (Func<int, int> f, int a) => f(a),
        ["(a,b) => { ... }"]  = _lambdaWithBlock,
        ["loop"]              = _lambdaWithLoopContinueBreak,
        ["switch(a){ ... }"]  = _switch,
        ["Console.WriteLine"] = () => WriteLine1Expression("Default"),
        ["try1"]              = _try1,
        ["try2"]              = _try2,
        ["try3"]              = _try3,
        ["try4"]              = _try4,
        ["try5"]              = _try5,
        ["try6"]              = _try6,
    };
}