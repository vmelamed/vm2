namespace vm2.ExpressionSerialization.XmlTests.ToDocumentTests;
using System.Runtime.Serialization;

public partial class ConstantTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(TestsFixture.TestFilesPath, "Constants");

    [Theory]
    [MemberData(nameof(ConstantsData))]
    public async Task ConstantTestAsync(string _, string expressionString, string fileName) => await base.TestAsync(expressionString, fileName);

    protected override Expression Substitute(string id) => _substitutes[id];

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task TestConstantExpressionClassNonSerializableAsync(bool callAsync)
    {
        var pathName = Path.Combine(XmlTestFilesPath, "ClassSerializable1.xml");
        var expression = Expression.Constant(new ClassNonSerializable(1, "One"));
        var (expectedDoc, expectedStr) = await TestsFixture.GetExpectedAsync(pathName, Out);
        var testCall = () => TestsFixture.TestSerializeExpression(expression, expectedDoc, expectedStr, pathName, Out);
        var testAsyncCall = async () => await TestsFixture.TestSerializeExpressionAsync(expression, expectedDoc, expectedStr, pathName, Out, CancellationToken.None);

        if (!callAsync)
            testCall.Should().Throw<SerializationException>();
        else
            await testAsyncCall.Should().ThrowAsync<SerializationException>();
    }
}
