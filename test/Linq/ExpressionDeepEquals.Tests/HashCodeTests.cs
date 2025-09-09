namespace vm2.Linq.ExpressionDeepEquals.Tests;

public class HashCodeTests
{
    [Fact]
    public void GetDeepHashCode_Visit_Null_ThrowsArgumentNullException()
    {
        var visitor = new HashCodeVisitor();
        Action act = () => visitor.Visit((Expression?)null);
        act.Should().Throw<ArgumentNullException>().Where(e => e.ParamName == "node");
    }

    [Fact]
    public void GetDeepHashCode_IdenticalStructure_ProducesSameHash()
    {
        Expression<Func<int,int>> e1 = x => x + 1;
        Expression<Func<int,int>> e2 = x => x + 1;

        var h1 = e1.GetDeepHashCode();
        var h2 = e2.GetDeepHashCode();

        h1.Should().Be(h2);
    }

    [Fact]
    public void GetDeepHashCode_DifferentConstants_ProducesDifferentHash()
    {
        Expression<Func<int,int>> e1 = x => x + 1;
        Expression<Func<int,int>> e2 = x => x + 2;

        var h1 = e1.GetDeepHashCode();
        var h2 = e2.GetDeepHashCode();

        h1.Should().NotBe(h2);
    }
}