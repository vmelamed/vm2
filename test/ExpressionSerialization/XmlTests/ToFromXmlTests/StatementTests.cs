namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

[CollectionDefinition("XML")]
public partial class StatementTests(XmlTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(XmlTestsFixture.TestFilesPath, "Statements");

    [Theory]
    [MemberData(nameof(StatementData))]
    public async Task StatementToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(StatementData))]
    public async Task StatementFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> StatementData = new ()
    {
        { TestLine(), "() => new",              "New" },
        { TestLine(), "(a,b) => { ... }",       "Block" },
        { TestLine(), "(f,a) => f(a)",          "Invocation" },
        { TestLine(), "accessMemberMember",     "AccessMemberMember" },
        { TestLine(), "accessMemberMember1",    "AccessMemberMember1" },
        { TestLine(), "arrayAccessExpr",        "ArrayAccessExpr" },
        { TestLine(), "array[index]",           "ArrayIndex" },
        { TestLine(), "b => b ? 1 : 3",         "Conditional" },
        { TestLine(), "Console.WriteLine",      "Invocation2" },
        { TestLine(), "goto1",                  "Goto1" },
        { TestLine(), "goto2",                  "Goto2" },
        { TestLine(), "goto3",                  "Goto3" },
        { TestLine(), "goto4",                  "Goto4" },
        { TestLine(), "newHashtableInit",       "NewHashtable" },
        { TestLine(), "indexMember",            "IndexMember" },
        { TestLine(), "indexObject1",           "IndexObject1" },
        { TestLine(), "loop",                   "Loop" },
        { TestLine(), "newArrayBounds",         "NewArrayBounds" },
        { TestLine(), "newArrayItems",          "NewArrayInit" },
        { TestLine(), "newDictionaryInit",      "NewDictionaryInit" },
        { TestLine(), "newHashtableInit",       "NewHashtableInit" },
        { TestLine(), "newListInit",            "NewListInit" },
        { TestLine(), "newMembersInit",         "NewMembersInit" },
        { TestLine(), "newMembersInit1",        "NewMembersInit1" },
        { TestLine(), "newMembersInit2",        "NewMembersInit2" },
        { TestLine(), "return1",                "Return1" },
        { TestLine(), "return2",                "Return2" },
        { TestLine(), "switch(a){ ... }",       "Switch" },
        { TestLine(), "throw",                  "Throw" },
        { TestLine(), "try1",                   "TryCatch1" },
        { TestLine(), "try2",                   "TryCatch2" },
        { TestLine(), "try3",                   "TryCatch3" },
        { TestLine(), "try4",                   "TryCatch4" },
        { TestLine(), "try5",                   "TryCatch5" },
        { TestLine(), "try6",                   "TryCatch6" },
    };

    protected override Expression Substitute(string id) => StatementTestData.GetExpression(id);
}