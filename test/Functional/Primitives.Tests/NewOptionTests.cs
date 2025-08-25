namespace vm2.Functional.Primitives.Tests;

public class NewOptionTests(
    PrimitivesTestsFixture fixture,
    ITestOutputHelper output) : IClassFixture<PrimitivesTestsFixture>
{
    protected ITestOutputHelper Out => output;
    protected PrimitivesTestsFixture _fixture = fixture;

    [Fact]
    public void NewDateTime()
    {
        var dt = FromString<DateTime>("2025-1-1T12:35:12+3:00", DateTime.TryParse);

        Assert.Equal(new DateTime(2025, 1, 1, 4, 35, 12, DateTimeKind.Local), dt);
    }

    [Fact]
    public void NewNoneDateTime()
    {
        var dt = FromString<DateTime>("2025-1-1Time12:35:12+3:00", DateTime.TryParse);

        Assert.Equal(None, dt);
    }

    [Fact]
    public void NewByte()
    {
        var n = FromString<byte>("202", byte.TryParse);
        Assert.Equal((byte)202, n);
    }

    [Fact]
    public void NewNoneByte()
    {
        var n = FromString<byte>("202ad", byte.TryParse);
        Assert.Equal(None, n);
    }

    [Fact]
    public void NewSByte()
    {
        var n = FromString<sbyte>("102", sbyte.TryParse);
        Assert.Equal((sbyte)102, n);
    }

    [Fact]
    public void NewNoneSByte()
    {
        var n = FromString<sbyte>("202", sbyte.TryParse);
        Assert.Equal(None, n);
    }

    [Fact]
    public void NewInt()
    {
        var n = FromString<int>("2025", int.TryParse);
        Assert.Equal(2025, n);
    }

    [Fact]
    public void NewNoneInt()
    {
        var n = FromString<int>("2025ad", int.TryParse);
        Assert.Equal(None, n);
    }

    [Fact]
    public void NewDecimal()
    {
        var n = FromString<decimal>("2025", decimal.TryParse);
        Assert.Equal(2025M, n);
    }

    [Fact]
    public void NewNoneDecimal()
    {
        var n = FromString<decimal>("2025ad", decimal.TryParse);
        Assert.Equal(None, n);
    }

    [Fact]
    public void NewOptInt()
    {
        var n = FromString<int>(Some("2025"), int.TryParse);
        Assert.Equal(2025, n);
    }

    [Fact]
    public void NewNoneOptInt()
    {
        var n = FromString<int>(Some("2025ad"), int.TryParse);
        Assert.Equal(None, n);
    }

    [Fact]
    public void NewOptDecimal()
    {
        var n = FromString<decimal>(Some("2025"), decimal.TryParse);
        Assert.Equal(2025M, n);
    }

    [Fact]
    public void NewNoneOptDecimal()
    {
        var n = FromString<decimal>(Some("2025ad"), decimal.TryParse);
        Assert.Equal(None, n);
    }

    [Fact]
    public void NewGuid()
    {
        Guid g = new("E9A61A1A-FF79-4416-AD92-85A4B1B38B5D");

        var n = FromString<Guid>("E9A61A1A-FF79-4416-AD92-85A4B1B38B5D", Guid.TryParse);
        Assert.Equal(g, n);
    }

    [Fact]
    public void NewNoneGuid()
    {
        var n = FromString<Guid>("2025ad", Guid.TryParse);
        Assert.Equal(None, n);
    }

    [Fact]
    public void NewValidDecimal()
    {
        var n = FromValid(
                            2025.73M,
                            m => m switch
                            {
                                > 0M and < 3000M => true,
                                5000M => true,
                                _ => false,
                            });
        Assert.Equal(2025.73M, n);
    }

    [Fact]
    public void NewValidDecimal2()
    {
        var n = FromValid(
                            5000M,
                            m => m switch
                            {
                                > 0M and < 3000M => true,
                                5000M => true,
                                _ => false,
                            });
        Assert.Equal(5000, n);
    }

    [Fact]
    public void NewNotValidDecimal()
    {
        var n = FromValid(
                            -2.71M,
                            m => m switch
                            {
                                > 0M and < 3000M => true,
                                5000M => true,
                                _ => false,
                            });
        Assert.Equal(None, n);
    }
}
