namespace vm2.ExpressionSerialization.XmlTests;

public partial class AssignmentTests(TestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string TestConstantsFilesPath => TestsFixture.TestFilesPath + "Assignments/";

    static ParameterExpression _paramA = Expression.Parameter(typeof(int), "a");

    [Theory]
    [MemberData(nameof(AssignmentsData))]
    public async Task AssignmentTestAsync(string _, string expressionString, string fileName)
        => await base.TestAsync(expressionString, fileName);

    public static readonly TheoryData<string, string, string> AssignmentsData = new ()
    {
        { TestLine(), "a = 1", "AssignConstant.xml" },
        { TestLine(), "a = b", "AssignVariable.xml" },
        { TestLine(), "a += b", "AddAssign.xml" },
        { TestLine(), "checked(a += b)", "AddAssignChecked.xml" },
        { TestLine(), "a -= b", "SubtractAssign.xml" },
        { TestLine(), "checked(a -= b)", "SubtractAssignChecked.xml" },
        { TestLine(), "a *= b", "MultiplyAssign.xml" },
        { TestLine(), "checked(a *= b)", "MultiplyAssignChecked.xml" },
        { TestLine(), "a /= b", "DivideAssign.xml" },
        { TestLine(), "a %= b", "ModuloAssign.xml" },
        { TestLine(), "a &= b", "AndAssign.xml" },
        { TestLine(), "a |= b", "OrAssign.xml" },
        { TestLine(), "a ^= b", "XorAssign.xml" },
        //{ TestLine(), "a **= b", "PowerAssign.xml" },
        { TestLine(), "a <<= b", "LShiftAssign.xml" },
        { TestLine(), "a >>= b", "RShiftAssign.xml" },
    };

    protected override Expression Substitute(string id) => _substitutes[id];

    static Dictionary<string, Expression> _substitutes = new()
    {
        ["a = 1"]           = Expression.Assign(
                                        _paramA,
                                        Expression.Constant(1)),
        ["a = b"]           = Expression.Assign(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
        ["a += b"]          = Expression.AddAssign(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
        ["checked(a += b)"] = Expression.AddAssignChecked(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
        ["a -= b"]          = Expression.SubtractAssign(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
        ["checked(a -= b)"] = Expression.SubtractAssignChecked(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
        ["a *= b"]          = Expression.MultiplyAssign(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
        ["checked(a *= b)"] = Expression.MultiplyAssignChecked(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
        ["a /= b"]          = Expression.DivideAssign(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
        ["a %= b"]          = Expression.ModuloAssign(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
        ["a &= b"]          = Expression.AndAssign(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
        ["a |= b"]          = Expression.OrAssign(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
        ["a ^= b"]          = Expression.ExclusiveOrAssign(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
        ["a <<= b"]         = Expression.LeftShiftAssign(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
        ["a >>= b"]         = Expression.RightShiftAssign(
                                        _paramA,
                                        Expression.Parameter(typeof(int), "b")),
    };
}
