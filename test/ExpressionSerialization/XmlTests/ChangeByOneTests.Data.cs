namespace vm2.ExpressionSerialization.XmlTests;

public partial class ChangeByOneTests
{
    public static readonly TheoryData<string, string, string> ChangeByOneExpressionData = new ()
    {
        { TestLine(), "a => increment(a)", "Increment.xml" },
        { TestLine(), "a => decrement(a)", "Decrement.xml" },
        { TestLine(), "a => ++a",          "PreIncrementAssign.xml" },
        { TestLine(), "a => a++",          "PostIncrementAssign.xml" },
        { TestLine(), "a => --a",          "PreDecrementAssign.xml" },
        { TestLine(), "a => a--",          "PostDecrementAssign.xml" },
    };

    static ParameterExpression _pa = Expression.Parameter(typeof(int), "a");

    static Dictionary<string, Expression> _substitutes = new()
    {
        ["a => increment(a)"] = Expression.Lambda(Expression.Increment(_pa), _pa),
        ["a => decrement(a)"] = Expression.Lambda(Expression.Decrement(_pa), _pa),
        ["a => ++a"]          = Expression.Lambda(Expression.PreIncrementAssign(_pa), _pa),
        ["a => a++"]          = Expression.Lambda(Expression.PostIncrementAssign(_pa), _pa),
        ["a => --a"]          = Expression.Lambda(Expression.PreDecrementAssign(_pa), _pa),
        ["a => a--"]          = Expression.Lambda(Expression.PostDecrementAssign(_pa), _pa),
    };
}