﻿namespace vm2.Linq.ExpressionSerialization.Json.Tests;

[CollectionDefinition("JSON")]
public partial class AssignmentTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(_fixture.TestFilesPath, "Assignments");

    [Theory]
    [MemberData(nameof(AssignmentTestData.Data), MemberType = typeof(AssignmentTestData))]
    public async Task AssignmentToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(AssignmentTestData.Data), MemberType = typeof(AssignmentTestData))]
    public async Task AssignmentFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    protected override Expression Substitute(string id) => AssignmentTestData.GetExpression(id);
}
