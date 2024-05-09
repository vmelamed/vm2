namespace vm2.ExpressionSerialization.XmlTests.ToDocumentTests;
public abstract class BaseTests : IClassFixture<TestsFixture>
{
    public ITestOutputHelper Out { get; }

    protected TestsFixture _fixture;

    public BaseTests(
        TestsFixture fixture,
        ITestOutputHelper output)
    {
        FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();
        _fixture = fixture;
        Out = output;
    }

    protected abstract string XmlTestFilesPath { get; }

    public virtual async Task ToXmlTestAsync(string testFileLine, string expressionString, string fileName)
    {
        var expression = Substitute(expressionString);
        var pathName = Path.Combine(XmlTestFilesPath, fileName);
        var (expectedDoc, expectedStr) = await TestsFixture.GetXmlDocumentAsync(testFileLine, pathName, "EXPECTED", Out);

        TestsFixture.TestExpressionToXml(testFileLine, expression, expectedDoc, expectedStr, pathName, Out);
        await TestsFixture.TestExpressionToXmlAsync(testFileLine, expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);
    }

    public virtual async Task FromXmlTestAsync(string testFileLine, string expressionString, string fileName)
    {
        var expectedExpression = Substitute(expressionString);
        var pathName = Path.Combine(XmlTestFilesPath, fileName);
        var (inputDoc, _) = await TestsFixture.GetXmlDocumentAsync(testFileLine, pathName, "INPUT", Out, true);

        inputDoc.Should().NotBeNull($"the input XDocument from {testFileLine} should not be null");

        TestsFixture.TestXmlToExpression(testFileLine, inputDoc!, expectedExpression);
    }

    protected abstract Expression Substitute(string id);
}
