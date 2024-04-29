﻿namespace vm2.ExpressionSerialization.XmlTests;

using System.Reflection;

public partial class StatementTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string TestConstantsFilesPath => TestsFixture.TestFilesPath + "Statements/";

    [Theory]
    [InlineData(typeof(int), "DefaultInt.xml")]
    [InlineData(typeof(int?), "DefaultNullableInt.xml")]
    public async Task TestDefaultIntAsync(Type type, string fileName)
    {
        var pathName = TestConstantsFilesPath + fileName;
        var expression = Expression.Default(type);
        var (expectedDoc, expectedStr) = await _fixture.GetExpectedAsync(pathName, Out);

        _fixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        await _fixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

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
        { TestLine(), "throw",              "Throw.xml" },
        { TestLine(), "try1",               "TryCatch1.xml" },
        { TestLine(), "try2",               "TryCatch2.xml" },
        { TestLine(), "try3",               "TryCatch3.xml" },
        { TestLine(), "try4",               "TryCatch4.xml" },
        { TestLine(), "try5",               "TryCatch5.xml" },
        { TestLine(), "try6",               "TryCatch6.xml" },
        { TestLine(), "newListInit",        "NewListInit.xml" },
        { TestLine(), "newArrayItems",      "NewArrayInit.xml" },
        { TestLine(), "newArrayBounds",     "NewArrayBounds.xml" },
        { TestLine(), "newDictionaryInit",  "NewDictionaryInit.xml" },
        { TestLine(), "newMembersInit",     "NewMembersInit.xml" },
        { TestLine(), "goto1",              "Goto1.xml" },
        { TestLine(), "goto2",              "Goto2.xml" },
        { TestLine(), "goto3",              "Goto3.xml" },
        { TestLine(), "goto4",              "Goto4.xml" },
        { TestLine(), "return1",            "Return1.xml" },
        { TestLine(), "return2",            "Return2.xml" },
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

    static Func<Expression> _throw = () =>
        Expression.Block(
            WriteLine1Expression("Before throwing"),
            Expression.Throw(ExceptionDefaultCtor())
        );

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
                WriteLine1Expression("TryBody"),
                ThrowException()
            ),
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
                WriteLine1Expression("TryBody"),
                ThrowException()
            ),
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
                WriteLine1Expression("TryBody"),
                ThrowException()
            ),
            WriteLine1Expression("finally {}"));

    static Func<Expression> _try5 = () =>
    {
        var exception = Expression.Parameter(typeof(ArgumentException), "x");
        return Expression.TryCatch(
                Expression.Block(
                    WriteLine1Expression("TryBody"),
                    ThrowException()
                ),
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

    static LabelTarget _returnTarget = Expression.Label();
    static ParameterExpression _a = Expression.Parameter(typeof(int), "a");

    static Func<Expression> _goto1 = () =>
        Expression.Block(
                WriteLine1Expression("GoTo"),
                Expression.Goto(_returnTarget),
                WriteLine1Expression("Unreachable"),
                Expression.Label(_returnTarget)
            );
    static Func<Expression> _goto2 = () =>
        Expression.Block(
                WriteLine1Expression("GoTo"),
                //Expression.Goto(_returnTarget),
                WriteLine1Expression("Reachable"),
                Expression.Label(_returnTarget)
            );

    static Func<Expression> _goto3 = () =>
        Expression.Block(
                [ _a ],
                Expression.Assign(_a, Expression.Constant(0)),
                Expression.Increment(_a),
                Expression.Goto(_returnTarget),
                Expression.Increment(_a),
                Expression.Label(_returnTarget)
            );

    static Func<Expression> _goto4 = () =>
        Expression.Block(
                [ _a ],
                Expression.Assign(_a, Expression.Constant(0)),
                Expression.Increment(_a),
                Expression.Goto(_returnTarget, type: typeof(void)),
                Expression.Increment(_a),
                Expression.Label(_returnTarget)
            );

    static Func<Expression> _return1 = () =>
        Expression.Block(
                [ _a ],
                Expression.Assign(_a, Expression.Constant(0)),
                Expression.Increment(_a),
                Expression.Return(_returnTarget),
                Expression.Increment(_a),
                Expression.Label(_returnTarget)
            );

    static Func<Expression> _return2 = () =>
        Expression.Block(
                [ _a ],
                Expression.Assign(_a, Expression.Constant(0)),
                Expression.Increment(_a),
                Expression.Return(_returnTarget, _a),
                Expression.Increment(_a),
                Expression.Label(_returnTarget)
            );

    static Expression<Func<TestMembersInitialized>> _newMembersInit =
                () => new TestMembersInitialized
                {
                    TheOuterIntProperty = 42,
                    Time = new DateTime(1776, 7, 4),
                    InnerProperty = new Inner
                    {
                        IntProperty = 23,
                        StringProperty = "inner string"
                    },
                    MyProperty = new List<string>
                    {
                        "aaa",
                        "bbb",
                        "ccc",
                    },
                };

    static Dictionary<string, Func<Expression>> _substitutes = new()
    {
        ["() => new StructDataContract1(42, \"don't panic\")"]
                                    = () => () => new StructDataContract1(42, "don't panic"),
        ["b => b ? 1 : 3"]          = () => (bool b) => b ? 1 : 3,
        ["(f,a) => f(a)"]           = () => (Func<int, int> f, int a) => f(a),
        ["(a,b) => { ... }"]        = _lambdaWithBlock,
        ["loop"]                    = _lambdaWithLoopContinueBreak,
        ["switch(a){ ... }"]        = _switch,
        ["Console.WriteLine"]       = () => WriteLine1Expression("Default"),
        ["throw"]                   = _throw,
        ["try1"]                    = _try1,
        ["try2"]                    = _try2,
        ["try3"]                    = _try3,
        ["try4"]                    = _try4,
        ["try5"]                    = _try5,
        ["try6"]                    = _try6,
        ["newListInit"]             = () => () => new List<string> { "aaa", "bbb", "ccc", },
        ["newArrayItems"]           = () => () => new string[] { "aaa", "bbb", "ccc" },
        ["newArrayBounds"]          = () => () => new string[2, 3, 4],
        ["newDictionaryInit"]       = () => () => new Dictionary<int, string> { { 1, "one" }, { 2, "two" }, { 3, "three" }, },
        ["newMembersInit"]          = () => _newMembersInit,
        ["goto1"]                   = _goto1,
        ["goto2"]                   = _goto2,
        ["goto3"]                   = _goto3,
        ["goto4"]                   = _goto4,
        ["return1"]                 = _return1,
        ["return2"]                 = _return2,
    };
}