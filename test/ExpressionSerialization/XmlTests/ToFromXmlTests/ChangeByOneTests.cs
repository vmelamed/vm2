namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;
public partial class ChangeByOneTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(TestsFixture.TestFilesPath, "ChangeByOne");

    [Theory]
    [MemberData(nameof(ChangeByOneExpressionData))]
    public async Task ChangeByOneToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(ChangeByOneExpressionData))]
    public async Task ChangeByOneFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

    protected override Expression Substitute(string id) => _substitutes[id];

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
