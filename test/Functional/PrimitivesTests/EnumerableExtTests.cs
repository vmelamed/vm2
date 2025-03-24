namespace vm2.Functional.PrimitivesTests;

public class EnumerableExtTests(
    PrimitivesTestsFixture fixture,
    ITestOutputHelper output) : IClassFixture<PrimitivesTestsFixture>
{
    protected ITestOutputHelper Out => output;
    protected PrimitivesTestsFixture _fixture = fixture;

    [Fact]
    public void MapTest()
    {
        IEnumerable<int> ns = [1, 2, 3 ];

        Enumerable.SequenceEqual(ns, ns.Map(x => x)).Should().BeTrue();
    }

    [Fact]
    public void EmptyMapTest()
    {
        IEnumerable<int> ns = [];

        Enumerable.SequenceEqual(ns, ns.Map(x => x)).Should().BeTrue();
    }

    [Fact]
    public void ForEachTest()
    {
        IEnumerable<int> ns = [1, 2, 3 ];

        var us = ns.ForEach(n => Out.WriteLine(n.ToString()));

        us.Should()
            .NotBeNull().And
            .NotBeEmpty().And
            .HaveCount(3);
    }

    [Fact]
    public void EmptyForEachTest()
    {
        IEnumerable<int> ns = [];

        var us = ns.ForEach(n => Out.WriteLine(n.ToString()));

        us.Should()
            .NotBeNull().And
            .BeEmpty().And
            .HaveCount(0);
    }

    [Fact]
    public void ForEach2Test()
    {
        IEnumerable<int> ns = [1, 2, 3 ];

        var us = ns.ForEach2(n => Out.WriteLine(n.ToString()));

        us.Should()
            .NotBeNull().And
            .BeOfType<Unit>();
    }

    [Fact]
    public void EmptyForEach2Test()
    {
        IEnumerable<int> ns = [];

        var us = ns.ForEach2(n => Out.WriteLine(n.ToString()));

        us.Should()
            .NotBeNull().And
            .BeOfType<Unit>();
    }
}
