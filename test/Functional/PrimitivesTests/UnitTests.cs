namespace vm2.Functional.PrimitivesTests;

using FluentAssertions;

public class UnitTests(
        PrimitivesTestsFixture fixture,
        ITestOutputHelper output) : IClassFixture<PrimitivesTestsFixture>
{
    protected ITestOutputHelper Out => output;
    protected PrimitivesTestsFixture _fixture = fixture;
    int _sideEffect;

    [Fact]
    public void Test0()
    {
        int state = 0, newState = 0;

        var a = () => { state = _sideEffect; newState = Interlocked.Increment(ref _sideEffect); };

        a.ToFunc()().Should().Be(default(Unit));
        state.Should().BeLessThan(newState);
    }

    [Fact]
    public void Test1()
    {
        int state = 0, newState = 0;

        var a = (int _) => { state = _sideEffect; newState = Interlocked.Increment(ref _sideEffect); };

        a.ToFunc()(0).Should().Be(default(Unit));
        state.Should().BeLessThan(newState);
    }

    [Fact]
    public void Test2()
    {
        int state = 0, newState = 0;

        var a = (int _, int _) => { state = _sideEffect; newState = Interlocked.Increment(ref _sideEffect); };

        a.ToFunc()(0, 0).Should().Be(default(Unit));
        state.Should().BeLessThan(newState);
    }

    [Fact]
    public void Test3()
    {
        int state = 0, newState = 0;

        var a = (int _, int _, int _) => { state = _sideEffect; newState = Interlocked.Increment(ref _sideEffect); };

        a.ToFunc()(0, 0, 0).Should().Be(default(Unit));
        state.Should().BeLessThan(newState);
    }

    [Fact]
    public void Test4()
    {
        int state = 0, newState = 0;

        var a = (int _, int _, int _, int _) => { state = _sideEffect; newState = Interlocked.Increment(ref _sideEffect); };

        a.ToFunc()(0, 0, 0, 0).Should().Be(default(Unit));
        state.Should().BeLessThan(newState);
    }

    [Fact]
    public void Test5()
    {
        int state = 0, newState = 0;

        var a = (int _, int _, int _, int _, int _) => { state = _sideEffect; newState = Interlocked.Increment(ref _sideEffect); };

        a.ToFunc()(0, 0, 0, 0, 0).Should().Be(default(Unit));
        state.Should().BeLessThan(newState);
    }

    [Fact]
    public void Test6()
    {
        int state = 0, newState = 0;

        var a = (int _, int _, int _, int _, int _, int _) => { state = _sideEffect; newState = Interlocked.Increment(ref _sideEffect); };

        a.ToFunc()(0, 0, 0, 0, 0, 0).Should().Be(default(Unit));
        state.Should().BeLessThan(newState);
    }

    [Fact]
    public void Test7()
    {
        int state = 0, newState = 0;

        var a = (int _, int _, int _, int _, int _, int _, int _) => { state = _sideEffect; newState = Interlocked.Increment(ref _sideEffect); };

        a.ToFunc()(0, 0, 0, 0, 0, 0, 0).Should().Be(default(Unit));
        state.Should().BeLessThan(newState);
    }

    [Fact]
    public void Test8()
    {
        int state = 0, newState = 0;

        var a = (int _, int _, int _, int _, int _, int _, int _, int _) => { state = _sideEffect; newState = Interlocked.Increment(ref _sideEffect); };

        a.ToFunc()(0, 0, 0, 0, 0, 0, 0, 0).Should().Be(default(Unit));
        state.Should().BeLessThan(newState);
    }

    [Fact]
    public void Test9()
    {
        int state = 0, newState = 0;

        var a = (int _, int _, int _, int _, int _, int _, int _, int _, int _) => { state = _sideEffect; newState = Interlocked.Increment(ref _sideEffect); };

        a.ToFunc()(0, 0, 0, 0, 0, 0, 0, 0, 0).Should().Be(default(Unit));
        state.Should().BeLessThan(newState);
    }

    [Fact]
    public void Test10()
    {
        int state = 0, newState = 0;

        var a = (int _, int _, int _, int _, int _, int _, int _, int _, int _, int _) => { state = _sideEffect; newState = Interlocked.Increment(ref _sideEffect); };

        a.ToFunc()(0, 0, 0, 0, 0, 0, 0, 0, 0, 0).Should().Be(default(Unit));
        state.Should().BeLessThan(newState);
    }

    [Fact]
    public void Test11()
    {
        int state = 0, newState = 0;

        var a = (int _, int _, int _, int _, int _, int _, int _, int _, int _, int _, int _) => { state = _sideEffect; newState = Interlocked.Increment(ref _sideEffect); };

        a.ToFunc()(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0).Should().Be(default(Unit));
        state.Should().BeLessThan(newState);
    }

    [Fact]
    public void Test12()
    {
        int state = 0, newState = 0;

        var a = (int _, int _, int _, int _, int _, int _, int _, int _, int _, int _, int _, int _) => { state = _sideEffect; newState = Interlocked.Increment(ref _sideEffect); };

        a.ToFunc()(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0).Should().Be(default(Unit));
        state.Should().BeLessThan(newState);
    }
}
