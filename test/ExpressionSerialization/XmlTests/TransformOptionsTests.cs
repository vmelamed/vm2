namespace vm2.ExpressionSerialization.XmlTests;

[CollectionDefinition("XML")]
public partial class TransformOptionsTests(ITestOutputHelper output)
{
    public ITestOutputHelper Out { get; } = output;

    [Theory]
    [MemberData(nameof(TransformIdentifiersData))]
    public void TransformIdentifiersTest(string _, string input, string expected, IdentifierConventions convention, bool throws)
    {
        var call = () => Conventions.Transform.Identifier(input, convention);
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
        var call = () => Conventions.Transform.TypeName(input, convention);
        if (throws)
        {
            call.Should().Throw<InternalTransformErrorException>();
            return;
        }

        call().Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(TransformAnonymousTypeNamesLocalData))]
    public void TransformTypeNamesAnonymousTest(string _, string expected, TypeNameConventions convention, bool throws)
    {
        var test = new
        {
            Abc = 123,
            Xyz = "xyz",
        };
        var input = test.GetType();

        var call = () => Conventions.Transform.TypeName(input, convention);
        if (throws)
        {
            call.Should().Throw<InternalTransformErrorException>();
            return;
        }

        call().Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(TransformGenericTypeNamesLocalData))]
    public void TransformTypeNamesDictionaryTest(string _, string expected, TypeNameConventions convention, bool throws)
    {
        var input = typeof(Dictionary<int, string>);

        var call = () => Conventions.Transform.TypeName(input, convention);
        if (throws)
        {
            call.Should().Throw<InternalTransformErrorException>();
            return;
        }

        call().Should().Be(expected);
    }
}
