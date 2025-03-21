namespace vm2.Functional.PrimitivesTests;

public class ActionExtTests(
    PrimitivesTestsFixture fixture,
    ITestOutputHelper output) : IClassFixture<PrimitivesTestsFixture>
{
    protected ITestOutputHelper Out => output;
    protected PrimitivesTestsFixture _fixture = fixture;

    [Fact]
    public void ToFunc_StateUnderTest_ExpectedBehavior()
    {
        // Arrange
        var actionExt = Substitute.For<Action>();

        // Act
        var result = actionExt.ToFunc()();

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public void ToFunc_StateUnderTest_ExpectedBehavior1()
    {
        // Arrange
        var actionExt = Substitute.For<Action<int>>();

        // Act
        var result = actionExt.ToFunc()(42);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public void ToFunc_StateUnderTest_ExpectedBehavior2()
    {
        // Arrange
        var actionExt = Substitute.For<Action<int, int>>();

        // Act
        var result = actionExt.ToFunc()(42, 1);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public void ToFunc_StateUnderTest_ExpectedBehavior3()
    {
        // Arrange
        var actionExt = Substitute.For<Action<int, int, int>>();

        // Act
        var result = actionExt.ToFunc()(42, 1, 2);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public void ToFunc_StateUnderTest_ExpectedBehavior4()
    {
        // Arrange
        var actionExt = Substitute.For<Action<int, int, int, int>>();

        // Act
        var result = actionExt.ToFunc()(42, 1, 2, 3);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public void ToFunc_StateUnderTest_ExpectedBehavior5()
    {
        // Arrange
        var actionExt = Substitute.For<Action<int, int, int, int, int>>();

        // Act
        var result = actionExt.ToFunc()(42, 1, 2, 3, 4);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public void ToFunc_StateUnderTest_ExpectedBehavior6()
    {
        // Arrange
        var actionExt = Substitute.For<Action<int, int, int, int, int, int>>();

        // Act
        var result = actionExt.ToFunc()(42, 1, 2, 3, 4, 5);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public void ToFunc_StateUnderTest_ExpectedBehavior7()
    {
        // Arrange
        var actionExt = Substitute.For<Action<int, int, int, int, int, int, int>>();

        // Act
        var result = actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public void ToFunc_StateUnderTest_ExpectedBehavior8()
    {
        // Arrange
        var actionExt = Substitute.For<Action<int, int, int, int, int, int, int, int>>();

        // Act
        var result = actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public void ToFunc_StateUnderTest_ExpectedBehavior9()
    {
        // Arrange
        var actionExt = Substitute.For<Action<int, int, int, int, int, int, int, int, int>>();

        // Act
        var result = actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public void ToFunc_StateUnderTest_ExpectedBehavior10()
    {
        // Arrange
        var actionExt = Substitute.For<Action<int, int, int, int, int, int, int, int, int, int>>();

        // Act
        var result = actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public void ToFunc_StateUnderTest_ExpectedBehavior11()
    {
        // Arrange
        var actionExt = Substitute.For<Action<int, int, int, int, int, int, int, int, int, int, int>>();

        // Act
        var result = actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public void ToFunc_StateUnderTest_ExpectedBehavior12()
    {
        // Arrange
        var actionExt = Substitute.For<Action<int, int, int, int, int, int, int, int, int, int, int, int>>();

        // Act
        var result = actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }
}
