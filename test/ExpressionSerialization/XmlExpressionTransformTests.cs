namespace ExpressionSerializationTests;

using System.Text;

public class XmlExpressionTransformTests
{
    public ITestOutputHelper Out { get; }

    public XmlExpressionTransformTests(ITestOutputHelper output)
    {
        Out = output;
        FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();
    }

    string Serialize(Expression expression)
    {
        var xformOptions = new TransformOptions()
        {
            Pretty = true,
            AddComments = true,
        };
        var sot = new XmlExpressionTransform(xformOptions);

        using var stream = new MemoryStream();

        sot.Serialize(expression, stream);

        var result = Encoding.UTF8.GetString(stream.ToArray());

        Out.WriteLine(result);
        return result;
    }

    async Task<string> SerializeAsync(Expression expression, CancellationToken cancellationToken = default)
    {
        var xformOptions = new TransformOptions()
        {
            Pretty = true,
            AddComments = true,
        };
        var sot = new XmlExpressionTransform(xformOptions);

        using var stream = new MemoryStream();
        await sot.SerializeAsync(expression, stream, cancellationToken: cancellationToken);

        var result = Encoding.UTF8.GetString(stream.ToArray());

        Out.WriteLine(result);
        return result;
    }

    [Fact]
    public void ConstantTest()
    {
        var expression = Expression.Constant(1);
        var result = Serialize(expression);
    }

    [Fact]
    public async Task ConstantTestAsync()
    {
        var expression = Expression.Constant(2);
        var result = await SerializeAsync(expression);
    }

    [Fact]
    public async Task ConstantTest1Async()
    {
        var expression = Expression.Constant((Half)3.14);
        var result = await SerializeAsync(expression);
    }
}
