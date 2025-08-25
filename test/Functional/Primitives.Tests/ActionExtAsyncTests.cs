namespace vm2.Functional.Primitives.Tests;

public partial class ActionExtTests
{
    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehaviorAsync()
    {
        // Arrange
        var actionExt = Substitute.For<Func<ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()();

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior1Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior3Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior4Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior5Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior6Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior7Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior8Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior9Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior10Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior11Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior12Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior13Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior14Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior15Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior16Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, ValueTask>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }
}
