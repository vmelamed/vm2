namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;
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
        { TestLine(), "(f,a) => f(a)",          "Invocation.json" },
        //{ TestLine(), "accessMemberMember",     "AccessMemberMember.json" },
        //{ TestLine(), "accessMemberMember1",    "AccessMemberMember1.json" },
        //{ TestLine(), "arrayAccessExpr",        "ArrayAccessExpr.json" },
        { TestLine(), "array[index]",           "ArrayIndex.json" },
        { TestLine(), "b => b ? 1 : 3",         "Conditional.json" },
        { TestLine(), "Console.WriteLine",      "MethodCall.json" },
        { TestLine(), "goto1",                  "Goto1.json" },
        { TestLine(), "goto2",                  "Goto2.json" },
        { TestLine(), "goto3",                  "Goto3.json" },
        { TestLine(), "goto4",                  "Goto4.json" },
        //{ TestLine(), "newHashtableInit",       "NewHashtable.json" },
        //{ TestLine(), "indexMember",            "IndexMember.json" },
        //{ TestLine(), "indexObject1",           "IndexObject1.json" },
        //{ TestLine(), "loop",                   "Loop.json" },
        { TestLine(), "newArrayBounds",         "NewArrayBounds.json" },
        { TestLine(), "newArrayItems",          "NewArrayInit.json" },
        { TestLine(), "newDictionaryInit",      "NewDictionaryInit.json" },
        { TestLine(), "newHashtableInit",       "NewHashtableInit.json" },
        { TestLine(), "newListInit",            "NewListInit.json" },
        //{ TestLine(), "newMembersInit",         "NewMembersInit.json" },
        //{ TestLine(), "newMembersInit1",        "NewMembersInit1.json" },
        //{ TestLine(), "newMembersInit2",        "NewMembersInit2.json" },
        { TestLine(), "return1",                "Return1.json" },
        { TestLine(), "return2",                "Return2.json" },
        //{ TestLine(), "switch(a){ ... }",       "Switch.json" },
        //{ TestLine(), "throw",                  "Throw.json" },
        //{ TestLine(), "try1",                   "TryCatch1.json" },
        //{ TestLine(), "try2",                   "TryCatch2.json" },
        //{ TestLine(), "try3",                   "TryCatch3.json" },
        //{ TestLine(), "try4",                   "TryCatch4.json" },
        //{ TestLine(), "try5",                   "TryCatch5.json" },
        //{ TestLine(), "try6",                   "TryCatch6.json" },
    };

    protected override Expression Substitute(string id) => StatementTestData.GetExpression(id);
}