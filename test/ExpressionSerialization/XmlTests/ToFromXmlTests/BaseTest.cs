namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

[CollectionDefinition("XML")]
public abstract class BaseTests(
        XmlTestsFixture fixture,
        ITestOutputHelper output) : IClassFixture<XmlTestsFixture>
{
    public ITestOutputHelper Out { get; } = output;

    protected XmlTestsFixture _fixture = fixture;

    protected abstract string XmlTestFilesPath { get; }

    public virtual async Task ToXmlTestAsync(string testFileLine, string expressionString, string fileName)
    {
        var expression = Substitute(expressionString);
        var pathName = Path.Combine(XmlTestFilesPath, fileName);
        var (expectedDoc, expectedStr) = await _fixture.GetXmlDocumentAsync(testFileLine, pathName, "EXPECTED", Out);

        _fixture.TestExpressionToXml(testFileLine, expression, expectedDoc, expectedStr, pathName, Out);
        await _fixture.TestExpressionToXmlAsync(testFileLine, expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

    public virtual async Task FromXmlTestAsync(string testFileLine, string expressionString, string fileName)
    {
        var expectedExpression = Substitute(expressionString);
        var pathName = Path.Combine(XmlTestFilesPath, fileName);
        var (inputDoc, _) = await _fixture.GetXmlDocumentAsync(testFileLine, pathName, "INPUT", Out, true);

        inputDoc.Should().NotBeNull($"the input XDocument from {testFileLine} should not be null");

        _fixture.TestXmlToExpression(testFileLine, inputDoc!, expectedExpression);
    }

    protected abstract Expression Substitute(string id);
}
