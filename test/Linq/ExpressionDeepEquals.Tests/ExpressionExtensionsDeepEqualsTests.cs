namespace vm2.Linq.ExpressionDeepEquals.Tests;

public class ExpressionExtensionsDeepEqualsTests
{
    [Fact]
    public void DeepEquals_ReferenceEqualsSameInstance_ReturnsTrueAndEmptyDifference()
    {
        var expr = Expression.Constant(42);

        var result = expr.DeepEquals(expr, out var difference);

        result.Should().BeTrue();
        difference.Should().BeEmpty();
    }

    [Fact]
    public void DeepEquals_DifferentTypes_ReturnsFalseAndDescribesTypes()
    {
        Expression left = Expression.Constant(1);
        Expression right = Expression.Constant("1");

        var result = left.DeepEquals(right, out var difference);

        result.Should().BeFalse();
        difference.Should().Contain("different types");
        difference.Should().Contain(typeof(int).FullName);
        difference.Should().Contain(typeof(string).FullName);
    }

    [Fact]
    public void DeepEquals_IdenticalStructureDifferentInstances_ReturnsTrue()
    {
        Expression<Func<int, int>> e1 = x => x + 1;
        Expression<Func<int, int>> e2 = x => x + 1;

        e1.DeepEquals(e2).Should().BeTrue();
    }

    [Fact]
    public void DeepEquals_StructuralDifference_ReportsNonEmptyDifference()
    {
        Expression<Func<int, int>> left = x => x + 1;
        Expression<Func<int, int>> right = x => x + 2;

        var result = left.DeepEquals(right, out var difference);

        result.Should().BeFalse();
        difference.Should().NotBeNullOrWhiteSpace();
        difference.Should().Contain("Difference");
    }

    [Fact]
    public void DeepEquals_ParameterNameMismatch_ReturnsFalseAndReportsNames()
    {
        // two lambdas with same shape but different parameter names
        Expression<Func<int, int>> e1 = (int a) => a + 1;
        Expression<Func<int, int>> e2 = (int b) => b + 1;

        var eq = e1.DeepEquals(e2, out var difference);

        eq.Should().BeFalse();
        difference.Should().NotBeNullOrWhiteSpace();
        // expect parameter names to appear in the diagnostic
        difference.Should().Contain("a");
        difference.Should().Contain("b");
    }

    [Fact]
    public void DeepEquals_LabelTargetNameMismatch_ReturnsFalse()
    {
        var ret1 = Expression.Label("ret1");
        var block1 = Expression.Block(
            Expression.Goto(ret1),
            Expression.Label(ret1)
        );

        var ret2 = Expression.Label("ret2");
        var block2 = Expression.Block(
            Expression.Goto(ret2),
            Expression.Label(ret2)
        );

        var eq = block1.DeepEquals(block2, out var difference);

        eq.Should().BeFalse();
        difference.Should().NotBeNullOrWhiteSpace();
        difference.Should().Contain("ret1");
        difference.Should().Contain("ret2");
    }

    [Fact]
    public void DeepEquals_Constants_ObjectDifferentInstances_TreatedAsEqual()
    {
        // constants typed as System.Object - implementation treats System.Object constants specially
        var c1 = Expression.Constant(new object(), typeof(object));
        var c2 = Expression.Constant(new object(), typeof(object));

        c1.DeepEquals(c2).Should().BeTrue();
    }

    [Fact]
    public void DeepEquals_ConstantArrays_DifferentElements_ReturnsFalse()
    {
        var a1 = Expression.Constant(new int[] { 1, 2, 3 }, typeof(int[]));
        var a2 = Expression.Constant(new int[] { 1, 2, 4 }, typeof(int[]));

        var eq = a1.DeepEquals(a2, out var difference);

        eq.Should().BeFalse();
        difference.Should().NotBeNullOrWhiteSpace();
    }
}