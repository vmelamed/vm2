﻿namespace vm2.Linq.ExpressionSerialization.Json.Tests;

[CollectionDefinition("JSON")]
public partial class ConstantTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(_fixture.TestFilesPath, "Constants");

    [Theory]
    [MemberData(nameof(ConstantTestData.Data), MemberType = typeof(ConstantTestData))]
#if !JSON_SCHEMA
    [MemberData(nameof(ConstantTestDataNs.Data), MemberType = typeof(ConstantTestDataNs))]
#endif
    public async Task ConstantToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(ConstantTestData.Data), MemberType = typeof(ConstantTestData))]
#if !JSON_SCHEMA
    [MemberData(nameof(ConstantTestDataNs.Data), MemberType = typeof(ConstantTestDataNs))]
#endif
    public async Task ConstantFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    protected override Expression Substitute(string id) => ConstantTestData.GetExpression(id) ?? ConstantTestDataNs.GetExpression(id);
}
