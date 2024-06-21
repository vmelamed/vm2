﻿namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

[CollectionDefinition("JSON")]
public partial class ChangeByOneTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(JsonTestsFixture.TestFilesPath, "ChangeByOne");

    [Theory]
    [MemberData(nameof(ChangeByOneExpressionData))]
    public async Task ChangeByOneToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(ChangeByOneExpressionData))]
    public async Task ChangeByOneFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    protected override Expression Substitute(string id) => _substitutes[id];

    public static readonly TheoryData<string, string, string> ChangeByOneExpressionData = new ()
    {
        { TestLine(), "a => increment(a)", "Increment.json" },
        { TestLine(), "a => decrement(a)", "Decrement.json" },
        { TestLine(), "a => ++a",          "PreIncrementAssign.json" },
        { TestLine(), "a => a++",          "PostIncrementAssign.json" },
        { TestLine(), "a => --a",          "PreDecrementAssign.json" },
        { TestLine(), "a => a--",          "PostDecrementAssign.json" },
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