namespace vm2.Functional.PrimitivesTests;

public class OptionExtTests(
    BaseTestsFixture fixture,
    ITestOutputHelper output) : IClassFixture<BaseTestsFixture>
{
    protected ITestOutputHelper Out => output;
    protected BaseTestsFixture _fixture = fixture;

    [Fact]
    public void MapNoneTest()
    {
        Option<int> v = None;

        var r = v.Map(t => t.ToString());

        r.Should().BeOfType<Option<string>>();
        Assert.Equal(r, None);
    }

    [Fact]
    public void MapNotNoneTest()
    {
        Option<int> v = 42;

        var r = v.Map(t => t.ToString());

        r.Should().BeOfType<Option<string>>();
        Assert.Equal(r, "42");
    }

    readonly record struct Struct(int I, Option<string> S, decimal D);

    [Fact]
    public void MapNotNone2Test()
    {
        Option<Struct> v = new Struct(42, "gogo", 3.14M);

        var r = v.Map(t => new { I = t.I.ToString(), S = t.S.ToString(), D = t.D * 2 });

        Assert.Equal(r, new { I = "42", S = "Some(gogo)", D = 6.28M });
    }
}
