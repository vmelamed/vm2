namespace vm2.Functional.PrimitivesTests;

public class OptionTests(
    PrimitivesTestsFixture fixture,
    ITestOutputHelper output) : IClassFixture<PrimitivesTestsFixture>
{
    protected ITestOutputHelper Out => output;
    protected PrimitivesTestsFixture _fixture = fixture;

    [Fact]
    public void MatchSomeTest()
    {
        // ARRANGE:
        Option<int> sut = 42;
        var some = Substitute.For<Func<int, bool>>();
        var none = Substitute.For<Func<bool>>();

        some(Arg.Any<int>()).Returns(false);
        some(42).Returns(true);
        none().Returns(false);

        // ACT:
        var result = sut.Match(
                            Some: some,
                            None: none);

        // ASSERT:
        some.Received(1);
        none.Received(0);
        result
            .Should()
            .BeTrue()
            ;
    }

    [Fact]
    public void MatchNoneTest()
    {
        // ARRANGE:
        Option<int> sut = None;
        var some = Substitute.For<Func<int, bool>>();
        var none = Substitute.For<Func<bool>>();

        some(Arg.Any<int>()).Returns(false);
        none().Returns(true);

        // ACT:
        var result = sut.Match(
                            Some: some,
                            None: none);

        // ASSERT:
        some.Received(0);
        none.Received(1);
        result
            .Should()
            .BeTrue()
            ;
    }

    [Fact]
    public void OptionAsEnumerableTest()
    {
        Option<int> sut = 42;

        var enumerator = sut.AsEnumerable().GetEnumerator();

        enumerator
            .MoveNext()
            .Should()
            .BeTrue()
            ;
        enumerator
            .Current
            .Should()
            .Be(42)
            ;
        enumerator
            .MoveNext()
            .Should()
            .BeFalse()
            ;
    }

    [Fact]
    public void OptionNoneAsEnumerableTest()
    {
        Option<int> sut = None;

        var enumerator = sut.AsEnumerable().GetEnumerator();

        enumerator
            .MoveNext()
            .Should()
            .BeFalse()
            ;
    }

    [Fact]
    public void OptionTrueTest()
    {
        Option<int> sut = 42;

        (sut ? true : false).Should().BeTrue();
    }

    [Fact]
    public void OptionFalseTest()
    {
        Option<int> sut = None;

        (sut ? true : false).Should().BeFalse();
    }

    [Fact]
    public void OptionsCoalesceOperatorTest()
    {
        Option<int> v1 = 42;
        Option<int> v2 = None;
        Option<int> v3 = 357;

        (v1 | v2).Should().Be((Option<int>)42);
        (v2 | v1).Should().Be((Option<int>)42);

        (v1 | v3).Should().Be((Option<int>)42);
        (v3 | v1).Should().Be((Option<int>)357);
    }

    [Fact]
    public void OptionEqualsOptionTest()
    {
        Option<int> v1 = 42;
        Option<int> v2 = 42;
        Option<int> v3 = 42;
        Option<int> v4 = 357;
        Option<int> v5 = None;
        Option<int> v6 = None;
        Option<int> v7 = None;

        // reflexivity
        v1.Equals(v1).Should().BeTrue();
        v5.Equals(v5).Should().BeTrue();

        // symmetricity
        v1.Equals(v2).Should().BeTrue();
        v2.Equals(v1).Should().BeTrue();
        v5.Equals(v6).Should().BeTrue();
        v6.Equals(v5).Should().BeTrue();
        v1.Equals(v4).Should().BeFalse();
        v4.Equals(v1).Should().BeFalse();
        v1.Equals(v5).Should().BeFalse();
        v5.Equals(v1).Should().BeFalse();

        // transitivity
        v1.Equals(v2).Should().BeTrue();
        v2.Equals(v3).Should().BeTrue();
        v3.Equals(v1).Should().BeTrue();

        v5.Equals(v6).Should().BeTrue();
        v6.Equals(v7).Should().BeTrue();
        v7.Equals(v5).Should().BeTrue();
    }

    [Fact]
    public void OptionEqualsOperatorTest()
    {
        Option<int> v1 = 42;
        Option<int> v2 = 42;
        Option<int> v3 = 42;
        Option<int> v4 = 357;
        Option<int> v5 = None;
        Option<int> v6 = None;
        Option<int> v7 = None;

        // reflexivity
#pragma warning disable CS1718 // Comparison made to same variable
        (v1 == v1).Should().BeTrue();
        (v5 == v5).Should().BeTrue();
#pragma warning restore CS1718 // Comparison made to same variable

        // symmetricity
        (v1 == v2).Should().BeTrue();
        (v2 == v1).Should().BeTrue();
        (v5 == v6).Should().BeTrue();
        (v6 == v5).Should().BeTrue();
        (v1 == v4).Should().BeFalse();
        (v4 == v1).Should().BeFalse();
        (v1 == v5).Should().BeFalse();
        (v5 == v1).Should().BeFalse();

        // transitivity
        (v1 == v2).Should().BeTrue();
        (v2 == v3).Should().BeTrue();
        (v3 == v1).Should().BeTrue();

        (v5 == v6).Should().BeTrue();
        (v6 == v7).Should().BeTrue();
        (v7 == v5).Should().BeTrue();
    }

    [Fact]
    public void OptionNotEqualsOperatorTest()
    {
        Option<int> v1 = 42;
        Option<int> v2 = 42;
        Option<int> v3 = 42;
        Option<int> v4 = 357;
        Option<int> v5 = None;
        Option<int> v6 = None;
        Option<int> v7 = None;

        // reflexivity
#pragma warning disable CS1718 // Comparison made to same variable
        (v1 != v1).Should().BeFalse();
        (v5 != v5).Should().BeFalse();
#pragma warning restore CS1718 // Comparison made to same variable

        // symmetricity
        (v1 != v2).Should().BeFalse();
        (v2 != v1).Should().BeFalse();
        (v5 != v6).Should().BeFalse();
        (v6 != v5).Should().BeFalse();
        (v1 != v4).Should().BeTrue();
        (v4 != v1).Should().BeTrue();
        (v1 != v5).Should().BeTrue();
        (v5 != v1).Should().BeTrue();

        // != is not transitive
        //(v1 == v2).Should().BeTrue();
        //(v2 == v3).Should().BeTrue();
        //(v3 == v1).Should().BeTrue();

        //(v5 == v6).Should().BeTrue();
        //(v6 == v7).Should().BeTrue();
        //(v7 == v5).Should().BeTrue();
    }

    [Fact]
    public void OptionEqualsObjectTest()
    {
        Option<int> v1 = 42;
        Option<int> v2 = 42;
        Option<int> v3 = 42;
        Option<int> v4 = 357;
        Option<int> v5 = None;
        Option<int> v6 = None;
        Option<int> v7 = None;

        object o1 = v1;
        object o2 = v2;
        object o3 = v3;
        object o4 = v4;
        object o5 = v5;
        object o6 = v6;
        object o7 = v7;

        // reflexivity
        o1.Equals(o1).Should().BeTrue();
        o5.Equals(o5).Should().BeTrue();

        // symmetricity
        v1.Equals(o2).Should().BeTrue();
        o2.Equals(v1).Should().BeTrue();
        v5.Equals(o6).Should().BeTrue();
        o6.Equals(v5).Should().BeTrue();
        v1.Equals(o4).Should().BeFalse();
        o4.Equals(v1).Should().BeFalse();
        v1.Equals(o5).Should().BeFalse();
        o5.Equals(v1).Should().BeFalse();

        // transitivity
        v1.Equals(v2).Should().BeTrue();
        v2.Equals(o3).Should().BeTrue();
        o3.Equals(v1).Should().BeTrue();

        v5.Equals(v6).Should().BeTrue();
        v6.Equals(o7).Should().BeTrue();
        o7.Equals(v5).Should().BeTrue();
    }

    [Fact]
    public void CastStringToOptionTest()
    {
        // ARRANGE:
        Option<string> sut = "test";
        var some = Substitute.For<Func<string, bool>>();
        var none = Substitute.For<Func<bool>>();

        some(Arg.Any<string>()).Returns(false);
        some("test").Returns(true);
        none().Returns(false);

        // ACT:
        var result = sut.Match(
                            Some: some,
                            None: none);

        // ASSERT:
        some.Received(1);
        none.Received(0);
        result
            .Should()
            .BeTrue()
            ;
    }

    [Fact]
    public void CastNoneToOptionTest()
    {
        // ARRANGE:
        Option<string> sut = None;
        var some = Substitute.For<Func<string, bool>>();
        var none = Substitute.For<Func<bool>>();

        some(Arg.Any<string>()).Returns(false);
        none().Returns(true);

        // ACT:
        var result = sut.Match(
                            Some: some,
                            None: none);

        // ASSERT:
        some.Received(0);
        none.Received(1);
        result
            .Should()
            .BeTrue()
            ;
    }

    [Fact]
    public void OptionHashCodeTest()
    {
        Option<int> sut = 42;

        sut.GetHashCode().Should().Be(42.GetHashCode());

        sut = None;

        sut.GetHashCode().Should().Be(0);
    }

    [Fact]
    public void OptionToStringTest()
    {
        Option<int> sut = 42;

        sut.ToString().Should().Be("Some(42)");

        sut = None;

        sut.ToString().Should().Be("None");
    }
}