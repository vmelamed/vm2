﻿namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

[CollectionDefinition("JSON")]
public partial class AssignmentTests(JsonTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string JsonTestFilesPath => Path.Combine(JsonTestsFixture.TestFilesPath, "Assignments");

    [Theory]
    [MemberData(nameof(AssignmentsData))]
    public async Task AssignmentToJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToJsonTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(AssignmentsData))]
    public async Task AssignmentFromJsonTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromJsonTestAsync(testFileLine, expressionString, fileName);

    public static readonly TheoryData<string, string, string> AssignmentsData = new ()
    {
        { TestLine(), "a = 1",              "AssignConstant.json" },
        { TestLine(), "a = b",              "AssignVariable.json" },
        { TestLine(), "a += b",             "AddAssign.json" },
        { TestLine(), "checked(a += b)",    "AddAssignChecked.json" },
        { TestLine(), "a -= b",             "SubtractAssign.json" },
        { TestLine(), "checked(a -= b)",    "SubtractAssignChecked.json" },
        { TestLine(), "a *= b",             "MultiplyAssign.json" },
        { TestLine(), "checked(a *= b)",    "MultiplyAssignChecked.json" },
        { TestLine(), "a /= b",             "DivideAssign.json" },
        { TestLine(), "a %= b",             "ModuloAssign.json" },
        { TestLine(), "a &= b",             "AndAssign.json" },
        { TestLine(), "a |= b",             "OrAssign.json" },
        { TestLine(), "a ^= b",             "XorAssign.json" },
        { TestLine(), "x **= z",            "PowerAssign.json" },
        { TestLine(), "a <<= b",            "LShiftAssign.json" },
        { TestLine(), "a >>= b",            "RShiftAssign.json" },
    };

    protected override Expression Substitute(string id) => _substitutes[id]();

    static ParameterExpression _paramA = Expression.Parameter(typeof(int), "a");
    static ParameterExpression _paramB = Expression.Parameter(typeof(int), "b");
    static ParameterExpression _paramX = Expression.Parameter(typeof(double), "x");
    static ParameterExpression _paramZ = Expression.Parameter(typeof(double), "z");

    static Dictionary<string, Func<Expression>> _substitutes = new()
    {
        ["a = 1"]           = () => Expression.Assign(_paramA, Expression.Constant(1)),
        ["a = b"]           = () => Expression.Assign(_paramA,_paramB),
        ["a += b"]          = () => Expression.AddAssign(_paramA,_paramB),
        ["checked(a += b)"] = () => Expression.AddAssignChecked(_paramA,_paramB),
        ["a -= b"]          = () => Expression.SubtractAssign(_paramA,_paramB),
        ["checked(a -= b)"] = () => Expression.SubtractAssignChecked(_paramA,_paramB),
        ["a *= b"]          = () => Expression.MultiplyAssign(_paramA,_paramB),
        ["checked(a *= b)"] = () => Expression.MultiplyAssignChecked(_paramA,_paramB),
        ["a /= b"]          = () => Expression.DivideAssign(_paramA,_paramB),
        ["a %= b"]          = () => Expression.ModuloAssign(_paramA,_paramB),
        ["a &= b"]          = () => Expression.AndAssign(_paramA,_paramB),
        ["a |= b"]          = () => Expression.OrAssign(_paramA,_paramB),
        ["a ^= b"]          = () => Expression.ExclusiveOrAssign(_paramA,_paramB),
        ["a <<= b"]         = () => Expression.LeftShiftAssign(_paramA,_paramB),
        ["a >>= b"]         = () => Expression.RightShiftAssign(_paramA, _paramB),
        ["x **= z"]         = () => Expression.PowerAssign(_paramX,_paramZ)
    };
}