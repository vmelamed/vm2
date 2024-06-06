﻿namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;
using System.Runtime.Serialization;

public partial class ConstantTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(TestsFixture.TestFilesPath, "Constants");

    [Theory]
    [MemberData(nameof(ConstantsData))]
    public async Task ConstantToXmlTestAsync(string testFileLine, string expressionString, string fileName) => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(ConstantsData))]
    public async Task ConstantFromXmlTestAsync(string testFileLine, string expressionString, string fileName) => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

    protected override Expression Substitute(string id) => _substitutes[id];

    [Fact]
    public async Task TestConstantToXmlClassNonSerializableAsync()
    {
        var pathName = Path.Combine(XmlTestFilesPath, "ClassSerializable1.xml");
        var expression = Expression.Constant(new ClassNonSerializable(1, "One"));
        var (expectedDoc, expectedStr) = await TestsFixture.GetXmlDocumentAsync(TestLine(), pathName, "EXPECTED", Out);

        var testCall = () => TestsFixture.TestExpressionToXml(TestLine(), expression, expectedDoc, expectedStr, pathName, Out);

        testCall.Should().Throw<SerializationException>();

        var testAsyncCall = async () => await TestsFixture.TestExpressionToXmlAsync(TestLine(), expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);

        await testAsyncCall.Should().ThrowAsync<SerializationException>();
    }
}