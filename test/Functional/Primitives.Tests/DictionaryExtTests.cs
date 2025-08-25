namespace vm2.Functional.Primitives.Tests;
public class DictionaryExtTests(
    PrimitivesTestsFixture fixture,
    ITestOutputHelper output) : IClassFixture<PrimitivesTestsFixture>
{
    protected ITestOutputHelper Out => output;
    protected PrimitivesTestsFixture _fixture = fixture;

    [Fact]
    public void Lookup_NotFound_Returns_None()
    {
        // arrange
        IDictionary<int, string> keyValuePairs = Substitute.For<IDictionary<int, string>>();
        keyValuePairs.TryGetValue(default, out var value).Returns(x => { x[1] = null; return false; });

        // act
        var result = keyValuePairs.Lookup(7);

        // assert
        Assert.Equal(None, result);
        result.Should().BeOfType<Option<string>>();
        keyValuePairs.Received(1);
    }

    [Fact]
    public void Lookup_Found_Returns_OptionT()
    {
        // arrange
        IDictionary<int, string> keyValuePairs = Substitute.For<IDictionary<int, string>>();
        keyValuePairs
            .TryGetValue(7, out Arg.Any<string>()!)
            .Returns(
                args =>
                {
                    args[1] = "gogo";
                    return true;
                })
            ;

        // act
        var result = keyValuePairs.Lookup(7);

        // assert
        Assert.Equal(Some("gogo"), result);
        result.Should().BeOfType<Option<string>>();
        keyValuePairs.Received(1);
    }

    [Fact]
    public void LookupRO_NotFound_Returns_None()
    {
        // arrange
        IReadOnlyDictionary<int, string> keyValuePairs = Substitute.For<IReadOnlyDictionary<int, string>>();
        keyValuePairs.TryGetValue(default, out var value).Returns(x => { x[1] = null; return false; });

        // act
        var result = keyValuePairs.Lookup(7);

        // assert
        Assert.Equal(None, result);
        result.Should().BeOfType<Option<string>>();
        keyValuePairs.Received(1);
    }

    [Fact]
    public void LookupRO_Found_Returns_OptionT()
    {
        // arrange
        IReadOnlyDictionary<int, string> keyValuePairs = Substitute.For<IReadOnlyDictionary<int, string>>();
        keyValuePairs
            .TryGetValue(7, out Arg.Any<string>()!)
            .Returns(
                args =>
                {
                    args[1] = "gogo";
                    return true;
                })
            ;

        // act
        var result = keyValuePairs.Lookup(7);

        // assert
        Assert.Equal(Some("gogo"), result);
        result.Should().BeOfType<Option<string>>();
        keyValuePairs.Received(1);
    }
}
