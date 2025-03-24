namespace vm2.Functional.PrimitivesTests;

public class OptionExtTests(
    PrimitivesTestsFixture fixture,
    ITestOutputHelper output) : IClassFixture<PrimitivesTestsFixture>
{
    protected ITestOutputHelper Out => output;
    protected PrimitivesTestsFixture _fixture = fixture;

    [Fact]
    public void NullableInt_ToOptionTest_Null()
    {
        int? v = null;

        var r = v.ToOption();

        Assert.Equal(None, r);
    }

    [Fact]
    public void NullableInt_ToOptionTest_NotNull()
    {
        int? v = 7;

        var r = v.ToOption();

        Assert.Equal(v.Value, r);
    }
}
