﻿namespace vm2.ExpressionSerialization.XmlTests;

[CollectionDefinition("XML")]
public partial class BinaryTests(XmlTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(XmlTestsFixture.TestFilesPath, "Binary");

    [Theory]
    [MemberData(nameof(BinaryTestData.Data), MemberType = typeof(BinaryTestData))]
    public async Task BinaryToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(BinaryTestData.Data), MemberType = typeof(BinaryTestData))]
    public async Task BinaryFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

    protected override Expression Substitute(string id) => BinaryTestData.GetExpression(id);
}
