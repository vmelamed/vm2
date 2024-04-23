namespace vm2.ExpressionSerialization.XmlTests;

public partial class AssignmentTests
{
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

    static ParameterExpression _pa = Expression.Parameter(typeof(int), "a");

    static Dictionary<string, Expression> _substitutes = new()
    {
        ["a = 1"]           = Expression.Assign(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Constant(1)),
        ["a = b"]           = Expression.Assign(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
        ["a += b"]          = Expression.AddAssign(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
        ["checked(a += b)"] = Expression.AddAssignChecked(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
        ["a -= b"]          = Expression.SubtractAssign(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
        ["checked(a -= b)"] = Expression.SubtractAssignChecked(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
        ["a *= b"]          = Expression.MultiplyAssign(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
        ["checked(a *= b)"] = Expression.MultiplyAssignChecked(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
        ["a /= b"]          = Expression.DivideAssign(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
        ["a %= b"]          = Expression.ModuloAssign(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
        ["a &= b"]          = Expression.AndAssign(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
        ["a |= b"]          = Expression.OrAssign(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
        ["a ^= b"]          = Expression.ExclusiveOrAssign(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
        //["a **= b"]         = Expression.PowerAssign(
        //                                Expression.Parameter(typeof(int), "a"),
        //                                Expression.Parameter(typeof(int), "b")),
        ["a <<= b"]         = Expression.LeftShiftAssign(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
        ["a >>= b"]         = Expression.RightShiftAssign(
                                        Expression.Parameter(typeof(int), "a"),
                                        Expression.Parameter(typeof(int), "b")),
    };
}
