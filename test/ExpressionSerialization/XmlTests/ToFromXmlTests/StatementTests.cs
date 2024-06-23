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
        { TestLine(), "() => new StructDataContract1(42, \"don't panic\")", "New.xml" },
        { TestLine(), "(a,b) => { ... }",       "Block.xml" },
        { TestLine(), "(f,a) => f(a)",          "Invocation.xml" },
        { TestLine(), "accessMemberMember",     "AccessMemberMember.xml" },
        { TestLine(), "accessMemberMember1",    "AccessMemberMember1.xml" },
        { TestLine(), "arrayAccessExpr",        "ArrayAccessExpr.xml" },
        { TestLine(), "array[index]",           "ArrayIndex.xml" },
        { TestLine(), "b => b ? 1 : 3",         "Conditional.xml" },
        { TestLine(), "Console.WriteLine",      "Invocation2.xml" },
        { TestLine(), "goto1",                  "Goto1.xml" },
        { TestLine(), "goto2",                  "Goto2.xml" },
        { TestLine(), "goto3",                  "Goto3.xml" },
        { TestLine(), "goto4",                  "Goto4.xml" },
        { TestLine(), "newHashtableInit",       "NewHashtable.xml" },
        { TestLine(), "indexMember",            "IndexMember.xml" },
        { TestLine(), "indexObject1",           "IndexObject1.xml" },
        { TestLine(), "loop",                   "Loop.xml" },
        { TestLine(), "newArrayBounds",         "NewArrayBounds.xml" },
        { TestLine(), "newArrayItems",          "NewArrayInit.xml" },
        { TestLine(), "newDictionaryInit",      "NewDictionaryInit.xml" },
        { TestLine(), "newListInit",            "NewListInit.xml" },
        { TestLine(), "newMembersInit",         "NewMembersInit.xml" },
        { TestLine(), "newMembersInit1",        "NewMembersInit1.xml" },
        { TestLine(), "newMembersInit2",        "NewMembersInit2.xml" },
        { TestLine(), "return1",                "Return1.xml" },
        { TestLine(), "return2",                "Return2.xml" },
        { TestLine(), "switch(a){ ... }",       "Switch.xml" },
        { TestLine(), "throw",                  "Throw.xml" },
        { TestLine(), "try1",                   "TryCatch1.xml" },
        { TestLine(), "try2",                   "TryCatch2.xml" },
        { TestLine(), "try3",                   "TryCatch3.xml" },
        { TestLine(), "try4",                   "TryCatch4.xml" },
        { TestLine(), "try5",                   "TryCatch5.xml" },
        { TestLine(), "try6",                   "TryCatch6.xml" },
    };

    protected override Expression Substitute(string id) => StatementTestData.GetExpression(id);
}