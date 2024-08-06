namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

[CollectionDefinition("JSON")]
public partial class LambdaTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(JsonTestsFixture.TestFilesPath, "Lambdas");

    [Theory]
    [MemberData(nameof(LambdaData))]
    public async Task LambdaToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(LambdaData))]
    public async Task LambdaFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> LambdaData = new ()
    {
        { TestLine(), "i => true",                  "Param2BoolConstant" },
        { TestLine(), "i => i",                     "Param1Ret1Constant" },
        { TestLine(), "(i, j) => j",                "Param2Ret2Constant" },
        { TestLine(), "(s,d) => true",              "2ParamsToConstant" },
        { TestLine(), "a => a._a",                  "MemberField" },
        { TestLine(), "a => a.A",                   "MemberProperty" },
        { TestLine(), "(s,d) => true",              "StaticMember" },
        { TestLine(), "a => a.Method3(1,1)",        "InstanceMethod3Params" },
        { TestLine(), "a => a.Method4(42,3.14)",    "InstanceMethod4Params" },
        { TestLine(), "(i, j) => (a=i)+(b=j)",      "Param2Var1Ret2nd" },
    };

    protected override Expression Substitute(string id) => LambdaTestData.GetExpression(id);
}