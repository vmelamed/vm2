﻿namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

[CollectionDefinition("XML")]
public partial class LambdaTests(XmlTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(XmlTestsFixture.TestFilesPath, "Lambdas");

    [Theory]
    [MemberData(nameof(LambdaData))]
    public async Task LambdaToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(LambdaData))]
    public async Task LambdaFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> LambdaData = new ()
    {
        { TestLine(), "i => true",              "Param2BoolConstant" },
        { TestLine(), "i => i",                 "Param1Ret1" },
        { TestLine(), "(i, j) => j",            "Param2Ret2nd" },
        { TestLine(), "(s,d) => true",          "2ParamsToConstant" },
        { TestLine(), "a => a._a",              "MemberField" },
        { TestLine(), "a => a.A",               "MemberProperty" },
        { TestLine(), "(s,d) => true",          "StaticMember" },
        { TestLine(), "a => a.Method3(1,1)",    "InstanceMethod3Params" },
        { TestLine(), "a => a.Method4(42,3.14)","InstanceMethod4Params" },
        { TestLine(), "(i, j) => (a=i)+(b=j)",  "Param2Var1Ret2nd" },
    };

    protected override Expression Substitute(string id) => LambdaTestData.GetExpression(id);
}