﻿namespace vm2.ExpressionSerialization.CommonData;

public static class BinaryTestData
{
    /// <summary>
    /// Gets the expression mapped to the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Expression.</returns>
    public static Expression GetExpression(string id) => _substitutes[id];

    static ParameterExpression _paramA = Expression.Parameter(typeof(int), "a");

    static Dictionary<string, Expression> _substitutes = new()
    {
        ["(a, b) => checked(a - b)"]            = (int a, int b) => checked(a - b),
        ["(a, b) => a - b"]                     = (int a, int b) => a - b,
        ["(a, b) => a >> b"]                    = (int a, int b) => a >> b,
        ["(a, b) => a ^ b"]                     = (int a, int b) => a ^ b,
        ["(a, b) => a || b"]                    = (bool a, bool b) => a || b,
        ["(a, b) => a | b"]                     = (int a, int b) => a | b,
        ["(a, b) => a != b"]                    = (int a, int b) => a != b,
        ["(a, b) => checked(a * b)"]            = (int a, int b) => checked(a * b),
        ["(a, b) => a * b"]                     = (int a, int b) => a * b,
        ["(a, b) => a % b"]                     = (int a, int b) => a % b,
        ["(a, b) => a <= b"]                    = (int a, int b) => a <= b,
        ["(a, b) => a < b"]                     = (int a, int b) => a < b,
        ["(a, b) => a << b"]                    = (int a, int b) => a << b,
        ["(a, b) => a >= b"]                    = (int a, int b) => a >= b,
        ["(a, b) => a > b"]                     = (int a, int b) => a > b,
        ["(a, b) => a == b"]                    = (int a, int b) => a == b,
        ["(a, b) => a / b"]                     = (int a, int b) => a / b,
        ["(a, b) => a ?? b"]                    = (int? a, int b) => a ?? b,
        ["(a, i) => a[i]"]                      = (int[] a, int i) => a[i],
        ["(a, b) => a && b"]                    = (bool a, bool b) => a && b,
        ["(a, b) => a & b"]                     = (int a, int b) => a & b,
        ["(a, b) => (a + b) * 42"]              = (int a, int b) => (a + b) * 42,
        ["(a, b) => a + b * 42"]                = (int a, int b) => a + b * 42,
        ["(a, b) => checked(a + b)"]            = (int a, int b) => checked(a + b),
        ["(a, b) => a + (b + c)"]               = (int a, int b, int c) => a + (b + c),
        ["(a, b) => a + b + c"]                 = (int a, int b, int c) => a + b + c,
        ["(a, b) => a + b"]                     = (int a, int b) => a + b,
        ["a => a as b"]                         = (ClassDataContract2 a) => a as ClassDataContract1,
        ["a => a is b"]                         = (object a) => a is ClassDataContract1,
        ["a => a equals int"]                   = Expression.Lambda(Expression.TypeEqual(_paramA, typeof(int)), _paramA),
        ["(a, b) => a ** b"]                    = Expression.Lambda(Expression.Power(Expression.Constant(2.0), Expression.Constant(3.0))),
    };
}
