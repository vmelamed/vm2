﻿namespace vm2.Linq.ExpressionSerialization.Json.Tests;

[CollectionDefinition("JSON")]
public partial class ChangeByOneTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(_fixture.TestFilesPath, "ChangeByOne");

    [Theory]
    [MemberData(nameof(ChangeByOneTestData.Data), MemberType = typeof(ChangeByOneTestData))]
    public async Task ChangeByOneToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(ChangeByOneTestData.Data), MemberType = typeof(ChangeByOneTestData))]
    public async Task ChangeByOneFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    protected override Expression Substitute(string id) => ChangeByOneTestData.GetExpression(id);
}
