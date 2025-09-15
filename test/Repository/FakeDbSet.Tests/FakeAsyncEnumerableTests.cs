namespace vm2.Repository.FakeDbSet.Tests;

public class FakeAsyncEnumerableTests(
    FakeDbSetTestsFixture fixture,
    ITestOutputHelper output) : IClassFixture<FakeDbSetTestsFixture>
{
    public ITestOutputHelper Out { get; } = output;

    protected FakeDbSetTestsFixture _fixture = fixture;

    [Fact]
    public void Ctor_NullEnumerable_ThrowsArgumentNullException()
    {
        Action act = () => new FakeAsyncEnumerable<int>((IEnumerable<int>)null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Ctor_NullExpression_ThrowsArgumentNullException()
    {
        Action act = () => new FakeAsyncEnumerable<int>((Expression)null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task Enumerate_EmptySource_YieldsNoResults()
    {
        var enumerable = new FakeAsyncEnumerable<int>([]);
        var results = new List<int>();
        await foreach (var i in enumerable)
            results.Add(i);
        results.Should().BeEmpty();
    }

    [Fact]
    public async Task Ctor_Enumerable_ProviderAsync()
    {
        var enumerable = new FakeAsyncEnumerable<int>(Enumerable.Range(1, 3));
        var k = 1;

        await foreach (var i in enumerable)
            i.Should().Be(k++);
    }

    [Fact]
    public async Task Ctor_Expression_ProviderAsync()
    {
        var f = Expression.Constant(Enumerable.Range(1, 3));

        var enumerable = new FakeAsyncEnumerable<int>(f);
        var k = 1;

        await foreach (var i in enumerable)
            i.Should().Be(k++);
    }

    [Fact]
    public async Task Queryable_EnumeratedAsync()
    {
        var l = new FakeAsyncEnumerable<int>(Enumerable.Range(1, 3));

        var k = 2;
        await foreach (var i in l.AsQueryable()
                                 .Where(i => i >= 2)
                                 .AsAsyncEnumerable())
            i.Should().Be(k++);
    }

    [Fact]
    public async Task Queryable_ExecutedAsync()
    {
        var l = new FakeAsyncEnumerable<int>(Enumerable.Range(1, 3));

        var k = 2;
        var l2 = await l
            .AsQueryable()
            .Where(i => i >= 2)
            .ToListAsync(TestContext.Current.CancellationToken)
            ;

        foreach (var i in l2)
            i.Should().Be(k++);
    }

    [Fact]
    public async Task MultipleEnumeration_YieldsSameResults()
    {
        var enumerable = new FakeAsyncEnumerable<int>(Enumerable.Range(1, 3));
        var first = new List<int>();
        var second = new List<int>();

        await foreach (var i in enumerable)
            first.Add(i);

        await foreach (var i in enumerable)
            second.Add(i);

        first.Should().Equal(second);
    }
}
