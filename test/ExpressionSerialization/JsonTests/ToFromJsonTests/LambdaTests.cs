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
        { TestLine(), "i => true",              "Param2BoolConstant.json" },
        { TestLine(), "i => i",                 "Param1Ret1Constant.json" },
        { TestLine(), "(i, j) => j",            "Param2Ret2Constant.json" },
        //{ TestLine(), "(s,d) => true",          "2ParamsToConstant.json" },
        //{ TestLine(), "a => a._var",              "MemberField.json" },
        //{ TestLine(), "a => a.A",               "MemberProperty.json" },
        //{ TestLine(), "(s,d) => true",          "StaticMember.json" },
        //{ TestLine(), "a => a.Method3(1,1)",    "InstanceMethod3Params.json" },
        //{ TestLine(), "a => a.Method4(42,3.14)","InstanceMethod4Params.json" },
        //{ TestLine(), "(i, j) => (a=i)+(b=j)",  "Param2Var1Ret2nd.xml" },
    };

    protected override Expression Substitute(string id) => LambdaTestData.GetExpression(id);
}