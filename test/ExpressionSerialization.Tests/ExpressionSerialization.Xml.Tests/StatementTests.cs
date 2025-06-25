﻿namespace vm2.ExpressionSerialization.Xml.Tests;

[CollectionDefinition("XML")]
public partial class StatementTests(XmlTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(_fixture.TestFilesPath, "Statements");

    [Theory]
    [MemberData(nameof(StatementTestData.Data), MemberType = typeof(StatementTestData))]
    public async Task StatementToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(StatementTestData.Data), MemberType = typeof(StatementTestData))]
    public async Task StatementFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

    protected override Expression Substitute(string id) => StatementTestData.GetExpression(id);
}
