namespace vm2.Repository.FakeDbSet.Tests;

public class FakeAsyncEnumeratorTests(
    FakeDbSetTestsFixture fixture,
    ITestOutputHelper output) : IClassFixture<FakeDbSetTestsFixture>
{
    public ITestOutputHelper Out { get; } = output;

    protected FakeDbSetTestsFixture _fixture = fixture;

    [Fact]
    public async Task MoveNextAsync_ReturnsFalseAtEnd()
    {
        var enr = new FakeAsyncEnumerator<int>(Enumerable.Range(1, 1).GetEnumerator());

        (await enr.MoveNextAsync()).Should().BeTrue();
        (await enr.MoveNextAsync()).Should().BeFalse();

        await enr.DisposeAsync();
    }

    [Fact]
    public async Task Current_BeforeMoveNextAsync_Throws()
    {
        var enr = new FakeAsyncEnumerator<int>(Enumerable.Range(1, 1).GetEnumerator());

        Action act = () => { var _ = enr.Current; };

        act.Should().Throw<InvalidOperationException>();

        await enr.DisposeAsync();
    }

    [Fact]
    public async Task Current_AfterEnumeration_Throws()
    {
        var enr = new FakeAsyncEnumerator<int>(Enumerable.Range(1, 1).GetEnumerator());
        await enr.MoveNextAsync();
        await enr.MoveNextAsync(); // End reached

        Action act = () => { var _ = enr.Current; };

        act.Should().Throw<InvalidOperationException>();

        await enr.DisposeAsync();
    }

    [Fact]
    public async Task DisposeAsync_CanBeCalledMultipleTimes()
    {
        var enr = new FakeAsyncEnumerator<int>(Enumerable.Range(1, 1).GetEnumerator());

        var act = async () => await enr.DisposeAsync();

        await act.Should().NotThrowAsync();
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task TestMoveNext_And_CurrentAsync()
    {
        var enr = new FakeAsyncEnumerator<int>(Enumerable.Range(1, 3).GetEnumerator());

        (await enr.MoveNextAsync()).Should().BeTrue();
        enr.Current.Should().Be(1);
        (await enr.MoveNextAsync()).Should().BeTrue();
        enr.Current.Should().Be(2);
        (await enr.MoveNextAsync()).Should().BeTrue();
        enr.Current.Should().Be(3);
        (await enr.MoveNextAsync()).Should().BeFalse();

        await enr.DisposeAsync();
    }
}
