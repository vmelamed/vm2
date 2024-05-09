namespace vm2.ExpressionSerialization.XmlTests.ToDocumentTests;
public partial class LambdaTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(TestsFixture.TestFilesPath, "Lambdas");

    [Theory]
    [MemberData(nameof(LambdaData))]
    public async Task LambdaTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> LambdaData = new ()
    {
        { TestLine(), "i => true",              "Param2BoolConstant.xml" },
        { TestLine(), "(s,d) => true",          "2ParamsToConstant.xml" },
        { TestLine(), "a => a._a",              "MemberField.xml" },
        { TestLine(), "a => a.A",               "MemberProperty.xml" },
        { TestLine(), "(s,d) => true",          "StaticMember.xml" },
        { TestLine(), "a => a.Method3(1,1)",    "InstanceMethod3Params.xml" },
        { TestLine(), "a => a.Method4(42,3.14)","InstanceMethod4Params.xml" },
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
        ["a => a.Method4(42,3.14)"]             = () => (TestMethods a) => a.Method4(42, 3.14),
    };
}