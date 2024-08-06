namespace vm2.ExpressionSerialization.TestsCommonData;

public static class AssignmentTestData
{
    /// <summary>
    /// Gets the expression mapped to the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Expression.</returns>
    public static Expression GetExpression(string id) => _substitutes[id];

    static ParameterExpression _paramA = Expression.Parameter(typeof(int), "a");
    static ParameterExpression _paramB = Expression.Parameter(typeof(int), "b");
    static ParameterExpression _paramX = Expression.Parameter(typeof(double), "x");
    static ParameterExpression _paramZ = Expression.Parameter(typeof(double), "z");

    static Dictionary<string, Expression> _substitutes = new()
    {
        ["a = 1"]           = Expression.Assign(_paramA, Expression.Constant(1)),
        ["a = b"]           = Expression.Assign(_paramA, _paramB),
        ["a += b"]          = Expression.AddAssign(_paramA, _paramB),
        ["checked(a += b)"] = Expression.AddAssignChecked(_paramA, _paramB),
        ["a -= b"]          = Expression.SubtractAssign(_paramA, _paramB),
        ["checked(a -= b)"] = Expression.SubtractAssignChecked(_paramA, _paramB),
        ["a *= b"]          = Expression.MultiplyAssign(_paramA, _paramB),
        ["checked(a *= b)"] = Expression.MultiplyAssignChecked(_paramA, _paramB),
        ["a /= b"]          = Expression.DivideAssign(_paramA, _paramB),
        ["a %= b"]          = Expression.ModuloAssign(_paramA, _paramB),
        ["a &= b"]          = Expression.AndAssign(_paramA, _paramB),
        ["a |= b"]          = Expression.OrAssign(_paramA, _paramB),
        ["a ^= b"]          = Expression.ExclusiveOrAssign(_paramA, _paramB),
        ["a <<= b"]         = Expression.LeftShiftAssign(_paramA, _paramB),
        ["a >>= b"]         = Expression.RightShiftAssign(_paramA, _paramB),
        ["x **= z"]         = Expression.PowerAssign(_paramX, _paramZ)
    };
}
