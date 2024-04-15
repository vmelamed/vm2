namespace vm2.ExpressionSerialization.ExpressionSerializationTests;

public partial class TransformOptionsTests
{
    public ITestOutputHelper Out { get; }

    public TransformOptionsTests(ITestOutputHelper output)
    {
        Out = output;
        FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();
    }

    [Theory]
    [MemberData(nameof(TransformIdentifiersData))]
    public void TransformIdentifiersTest(string _, string input, string expected, IdentifierConventions convention, bool throws)
    {
        var call = () => Transform.Identifier(input, convention);
        if (throws)
        {
            call.Should().Throw<InternalTransformErrorException>();
            return;
        }

        call().Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(TransformTypeNamesData))]
    public void TransformTypeNamesTest(string _, Type input, string expected, TypeNameConventions convention, bool throws)
    {
        Transform.ResetTypesNames();

        var call = () => Transform.TypeName(input, convention);
        if (throws)
        {
            call.Should().Throw<InternalTransformErrorException>();
            return;
        }

        call().Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(TransformTypeNamesLocalData))]
    public void TransformTypeNamesAnonymousTest(string _, string expected, TypeNameConventions convention, bool throws)
    {
        Transform.ResetTypesNames();

        var test = new
        {
            Abc = 123,
            Xyz = "xyz",
        };
        var input = test.GetType();

        var call = () => Transform.TypeName(input, convention);
        if (throws)
        {
            call.Should().Throw<InternalTransformErrorException>();
            return;
        }

        call().Should().Be(expected);
    }
}
