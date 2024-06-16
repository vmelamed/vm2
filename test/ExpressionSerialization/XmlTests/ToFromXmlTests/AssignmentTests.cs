namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

[CollectionDefinition("XML")]
public partial class AssignmentTests(XmlTestsFixture fixture, ITestOutputHelper output) : BaseTests(fixture, output)
{
    protected override string XmlTestFilesPath => Path.Combine(XmlTestsFixture.TestFilesPath, "Assignments");

    static ParameterExpression _paramA = Expression.Parameter(typeof(int), "a");
    static ParameterExpression _paramB = Expression.Parameter(typeof(int), "b");

    [Theory]
    [MemberData(nameof(AssignmentsData))]
    public async Task AssignmentToXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.ToXmlTestAsync(testFileLine, expressionString, fileName);

    [Theory]
    [MemberData(nameof(AssignmentsData))]
    public async Task AssignmentFromXmlTestAsync(string testFileLine, string expressionString, string fileName)
        => await base.FromXmlTestAsync(testFileLine, expressionString, fileName);

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

    protected override Expression Substitute(string id) => _substitutes[id]();

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
        ["a >>= b"]         = () => Expression.RightShiftAssign(_paramA,_paramB),
    };
}
