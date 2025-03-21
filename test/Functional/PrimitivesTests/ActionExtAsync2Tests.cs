namespace vm2.Functional.PrimitivesTests;

public partial class ActionExtTests
{
    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<Task>>();

        // Act
        var result = await actionExt.ToFunc()();

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior1_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior2_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior3_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior4_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior5_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior6_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior7_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior8_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior9_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior10_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior11_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior12_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior13_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior14_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior15_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }

    [Fact]
    public async Task ToFunc_StateUnderTest_ExpectedBehavior16_2Async()
    {
        // Arrange
        var actionExt = Substitute.For<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task>>();

        // Act
        var result = await actionExt.ToFunc()(42, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15);

        // Assert
        result.Should().BeOfType<Unit>();
        actionExt.Received(1);
    }
}
