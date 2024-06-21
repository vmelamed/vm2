namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

using System.Reflection;

[CollectionDefinition("JSON")]
public partial class StatementTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(JsonTestsFixture.TestFilesPath, "Statements");

    [Theory]
    [MemberData(nameof(StatementData))]
    public async Task StatementToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(StatementData))]
    public async Task StatementFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> StatementData = new ()
    {
        //{ TestLine(), "() => new StructDataContract1(42, \"don't panic\")", "New.json" },
        { TestLine(), "(a,b) => { ... }",       "Block.json" },
        //{ TestLine(), "(f,a) => f(a)",          "Invocation.json" },
        //{ TestLine(), "accessMemberMember",     "AccessMemberMember.json" },
        //{ TestLine(), "accessMemberMember1",    "AccessMemberMember1.json" },
        //{ TestLine(), "arrayAccessExpr",        "ArrayAccessExpr.json" },
        //{ TestLine(), "array[index]",           "ArrayIndex.json" },
        //{ TestLine(), "b => b ? 1 : 3",         "Conditional.json" },
        //{ TestLine(), "Console.WriteLine",      "Invocation2.json" },
        //{ TestLine(), "goto1",                  "Goto1.json" },
        //{ TestLine(), "goto2",                  "Goto2.json" },
        //{ TestLine(), "goto3",                  "Goto3.json" },
        //{ TestLine(), "goto4",                  "Goto4.json" },
        //{ TestLine(), "newHashtableInit",       "NewHashtable.json" },
        //{ TestLine(), "indexMember",            "IndexMember.json" },
        //{ TestLine(), "indexObject1",           "IndexObject1.json" },
        //{ TestLine(), "loop",                   "Loop.json" },
        //{ TestLine(), "newArrayBounds",         "NewArrayBounds.json" },
        //{ TestLine(), "newArrayItems",          "NewArrayInit.json" },
        //{ TestLine(), "newDictionaryInit",      "NewDictionaryInit.json" },
        //{ TestLine(), "newListInit",            "NewListInit.json" },
        //{ TestLine(), "newMembersInit",         "NewMembersInit.json" },
        //{ TestLine(), "newMembersInit1",        "NewMembersInit1.json" },
        //{ TestLine(), "newMembersInit2",        "NewMembersInit2.json" },
        //{ TestLine(), "return1",                "Return1.json" },
        //{ TestLine(), "return2",                "Return2.json" },
        //{ TestLine(), "switch(a){ ... }",       "Switch.json" },
        //{ TestLine(), "throw",                  "Throw.json" },
        //{ TestLine(), "try1",                   "TryCatch1.json" },
        //{ TestLine(), "try2",                   "TryCatch2.json" },
        //{ TestLine(), "try3",                   "TryCatch3.json" },
        //{ TestLine(), "try4",                   "TryCatch4.json" },
        //{ TestLine(), "try5",                   "TryCatch5.json" },
        //{ TestLine(), "try6",                   "TryCatch6.json" },
    };

    protected override Expression Substitute(string id) => _substitutes[id]();

    static ParameterExpression _paramA = Expression.Parameter(typeof(int), "a");
    static ParameterExpression _paramB = Expression.Parameter(typeof(int), "b");
    static ParameterExpression _paramC = Expression.Parameter(typeof(int), "c");
    static ParameterExpression _paramD = Expression.Parameter(typeof(int), "d");

    static Expression _block =
        Expression.Lambda(
            Expression.Block(
                [_paramD, ],
                Expression.Assign(_paramD, Expression.Constant(42)),
                Expression.AddAssign(_paramA, _paramD),
                Expression.SubtractAssign(_paramA, _paramB),
                _paramC,
                Expression.Assign(_paramC, Expression.Constant(2)),
                Expression.LeftShiftAssign(_paramA, _paramC)),
            _paramA, _paramB);

    static ParameterExpression _value  = Expression.Parameter(typeof(int), "_value");
    static ParameterExpression _result = Expression.Parameter(typeof(int), "_result");
    static LabelTarget _labelContinue  = Expression.Label("continue");
    static LabelTarget _labelBreak = Expression.Label("break");

    static Expression _lambdaWithLoopContinueBreak =
        Expression.Block(
            new[] { _result },
            Expression.Assign(_value, Expression.Constant(5)),
            Expression.Assign(_result, Expression.Constant(1)),
            Expression.Loop(
                Expression.Block(
                    [],
                    Expression.IfThenElse(
                        Expression.GreaterThan(_value, Expression.Constant(1)),
                        Expression.Block(
                            [],
                            Expression.MultiplyAssign(_result, Expression.PostDecrementAssign(_value)),
                            Expression.Continue(_labelContinue)),
                        Expression.Break(_labelBreak))),
                _labelBreak,
                _labelContinue))
    ;

    static MethodInfo _miWriteLine = typeof(Console).GetMethod("WriteLine", [ typeof(string) ])!;
    static Expression WriteLine1Expression(string s) => Expression.Call(null, _miWriteLine, Expression.Constant(s));
    static Expression ExceptionDefaultCtor() => Expression.New(typeof(Exception).GetConstructor(Type.EmptyTypes)!);
    static Expression ThrowException() => Expression.Throw(ExceptionDefaultCtor());

    static Expression _switch =
        Expression.Switch(
            _paramA,
            WriteLine1Expression("Default"),
            Expression.SwitchCase(
                WriteLine1Expression("FirstChild"),
                Expression.Constant(1)),
            Expression.SwitchCase(
                WriteLine1Expression("Second"),
                Expression.Constant(2),
                Expression.Constant(3),
                Expression.Constant(4))
            );

    static Expression _throw =
        Expression.Block(
            WriteLine1Expression("Before throwing"),
            Expression.Throw(ExceptionDefaultCtor())
        );

    static Expression _try1 =
        Expression.TryFault(
            Expression.Block(
                new Expression[]
                {
                    WriteLine1Expression("TryBody"),
                    ThrowException(),
                }),
            WriteLine1Expression("catch {}"));

    static Expression _try2 =
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

    static Expression _try3 =
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

    static Expression _try4 =
        Expression.TryFinally(
            Expression.Block(
                WriteLine1Expression("TryBody"),
                ThrowException()
            ),
            WriteLine1Expression("finally {}"));

    static ParameterExpression _exception = Expression.Parameter(typeof(ArgumentException), "x");
    static Expression _try5 =
        Expression.TryCatch(
            Expression.Block(
                WriteLine1Expression("TryBody"),
                ThrowException()
            ),
            [
                Expression.MakeCatchBlock(
                    typeof(ArgumentException),
                    _exception,
                    Expression.Call(
                                null,
                                _miWriteLine,
                                Expression.MakeMemberAccess(_exception, typeof(ArgumentException).GetProperty("Message")!)),
                    null),
            ]);

    static Expression _try6 =
        Expression.TryCatch(
            Expression.Block(
                WriteLine1Expression("TryBody"),
                ThrowException()),
            [
                Expression.MakeCatchBlock(
                    typeof(ArgumentException),
                    _exception,
                    Expression.Call(
                                null,
                                _miWriteLine,
                                Expression.MakeMemberAccess(_exception, typeof(ArgumentException).GetProperty("ParamName")!)),
                    Expression.Equal(
                        Expression.MakeMemberAccess(_exception, typeof(ArgumentException).GetProperty("ParamName")!),
                        Expression.Constant("x"))),
            ]);

    static LabelTarget _returnTarget = Expression.Label();
    static ParameterExpression _a = Expression.Parameter(typeof(int), "a");

    static Expression _goto1 =
        Expression.Block(
            WriteLine1Expression("GoTo"),
            Expression.Goto(_returnTarget),
            WriteLine1Expression("Unreachable"),
            Expression.Label(_returnTarget)
        );
    static Expression _goto2 =
        Expression.Block(
            WriteLine1Expression("GoTo"),
            //Expression.Goto(_returnTarget),
            WriteLine1Expression("Reachable"),
            Expression.Label(_returnTarget)
        );

    static Expression _goto3 =
        Expression.Block(
            [ _a ],
            Expression.Assign(_a, Expression.Constant(0)),
            Expression.Increment(_a),
            Expression.Goto(_returnTarget),
            Expression.Increment(_a),
            Expression.Label(_returnTarget)
        );

    static Expression _goto4 =
        Expression.Block(
            [ _a ],
            Expression.Assign(_a, Expression.Constant(0)),
            Expression.Increment(_a),
            Expression.Goto(_returnTarget, type: typeof(void)),
            Expression.Increment(_a),
            Expression.Label(_returnTarget)
        );

    static Expression _return1 =
        Expression.Block(
            [ _a ],
            Expression.Assign(_a, Expression.Constant(0)),
            Expression.Increment(_a),
            Expression.Return(_returnTarget),
            Expression.Increment(_a),
            Expression.Label(_returnTarget)
        );

    static Expression _return2 =
        Expression.Block(
            [ _a ],
            Expression.Assign(_a, Expression.Constant(0)),
            Expression.Increment(_a),
            Expression.Return(_returnTarget, _a),
            Expression.Increment(_a),
            Expression.Label(_returnTarget)
        );

    static ParameterExpression _m = Expression.Parameter(typeof(TestMembersInitialized), "m");
    static LabelTarget _return3 = Expression.Label(typeof(int));
    static Expression _accessMemberMember1 =
        Expression.Block(
            [_m],
            Expression.Assign(
                _m,
                Expression.New(typeof(TestMembersInitialized).GetConstructor(Type.EmptyTypes)!)),
            Expression.Return(
                _return3,
                Expression.Add(
                    Expression.MakeMemberAccess(
                        _m,
                        typeof(TestMembersInitialized).GetProperty(nameof(TestMembersInitialized.TheOuterIntProperty))!
                    ),
                    Expression.MakeMemberAccess(
                        Expression.MakeMemberAccess(_m, typeof(TestMembersInitialized).GetProperty(nameof(TestMembersInitialized.InnerProperty))!),
                        typeof(Inner).GetProperty(nameof(Inner.IntProperty))!)
                ),
                typeof(int)
            ),
            Expression.Label(_return3, Expression.Constant(0))
        );

    static ParameterExpression _arrayExpr = Expression.Parameter(typeof(int[]), "Array");
    static ParameterExpression _indexExpr = Expression.Parameter(typeof(int), "Index");
    static ParameterExpression _valueExpr = Expression.Parameter(typeof(int), "Value");
    static Expression _arrayAccessExpr = Expression.ArrayAccess(_arrayExpr, _indexExpr);

    // Array[Index] = (Array[Index] + Value)
    static Expression _lambdaExpr =
            Expression.Assign(
                _arrayAccessExpr,
                Expression.Add(
                    _arrayAccessExpr,
                    _valueExpr));

    static Dictionary<string, Func<Expression>> _substitutes = new()
    {
        ["() => new StructDataContract1(42, \"don't panic\")"] = () => () => new StructDataContract1(42, "don't panic"),
        ["(a,b) => { ... }"]        = () => _block,
        ["(f,a) => f(a)"]           = () => (Func<int, int> f, int a) => f(a),
        ["accessMemberMember"]      = () => (TestMembersInitialized m) => m.InnerProperty.IntProperty,
        ["accessMemberMember1"]     = () => _accessMemberMember1,
        ["arrayAccessExpr"]         = () => _arrayAccessExpr,
        ["b => b ? 1 : 3"]          = () => (bool b) => b ? 1 : 3,
        ["Console.WriteLine"]       = () => WriteLine1Expression("Default"),
        ["goto1"]                   = () => _goto1,
        ["goto2"]                   = () => _goto2,
        ["goto3"]                   = () => _goto3,
        ["goto4"]                   = () => _goto4,
#pragma warning disable IDE0056 // Use index operator
        ["indexMember"]             = () => (TestMembersInitialized m) => m.ArrayProperty.Length > 0 ? m.ArrayProperty[m.ArrayProperty.Length - 1] : -1,
#pragma warning restore IDE0056 // Use index operator
        ["array[index]"]            = () => _lambdaExpr,
        ["indexObject1"]            = () => (TestMembersInitialized1 m) => m[1],
        ["loop"]                    = () => _lambdaWithLoopContinueBreak,
        ["newArrayBounds"]          = () => () => new string[2, 3, 4],
        ["newArrayItems"]           = () => () => new string[] { "aaa", "bbb", "ccc" },
        ["newDictionaryInit"]       = () => () => new Dictionary<int, string> { { 1, "one" }, { 2, "two" }, { 3, "three" }, },
        ["newHashtableInit"]        = () => () => new Hashtable { { 1, "one" }, { 2, "two" }, { 3, "three" }, },
        ["newListInit"]             = () => () => new List<string> { "aaa", "bbb", "ccc", },
        ["newMembersInit"]          = () => () => new TestMembersInitialized
        {
            TheOuterIntProperty = 42,
            Time = new DateTime(1776, 7, 4),
            InnerProperty = new Inner
            {
                IntProperty = 23,
                StringProperty = "inner string"
            },
            EnumerableProperty = new List<string> { "aaa", "bbb", "ccc", },
        },
#pragma warning disable IDE0300 // Simplify collection initialization
        ["newMembersInit1"]         = () => () => new TestMembersInitialized1()
        {
            TheOuterIntProperty = 42,
            Time = new DateTime(1776, 7, 4),
            InnerProperty = new Inner
            {
                IntProperty = 23,
                StringProperty = "inner string"
            },
            ArrayProperty = new[] { 4, 5, 6 },
            ListProperty = { new Inner() { IntProperty = 23, StringProperty = "inner string" }, new Inner () { IntProperty = 42, StringProperty = "next inner string" } },
        },
        ["newMembersInit2"]         = () => () => new TestMembersInitialized1()
        {
            TheOuterIntProperty = 42,
            Time = new DateTime(1776, 7, 4),
            InnerProperty = { IntProperty = 23, StringProperty = "inner string" },
            ArrayProperty = new int[] { 4, 5, 6 },
            ListProperty = { new Inner() { IntProperty = 23, StringProperty = "inner string" }, new Inner () { IntProperty = 42, StringProperty = "next inner string" } },
        },
#pragma warning restore IDE0300 // Simplify collection initialization
        ["return1"]                 = () => _return1,
        ["return2"]                 = () => _return2,
        ["switch(a){ ... }"]        = () => _switch,
        ["throw"]                   = () => _throw,
        ["try1"]                    = () => _try1,
        ["try2"]                    = () => _try2,
        ["try3"]                    = () => _try3,
        ["try4"]                    = () => _try4,
        ["try5"]                    = () => _try5,
        ["try6"]                    = () => _try6,
    };
}