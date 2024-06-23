﻿namespace vm2.ExpressionSerialization.CommonData;

public static class LambdaTestData
{
    /// <summary>
    /// Gets the expression mapped to the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Expression.</returns>
    public static Expression GetExpression(string id) => _substitutes[id];

    static ParameterExpression _paramI = Expression.Parameter(typeof(int), "i");
    static ParameterExpression _paramJ = Expression.Parameter(typeof(double), "j");
    static ParameterExpression _varA = Expression.Parameter(typeof(int), "a");
    static ParameterExpression _varB = Expression.Parameter(typeof(double), "b");

    static Dictionary<string, Expression> _substitutes = new()

    {
        ["i => true"]                           = (int i) => true,
        ["i => i"]                              = (int i) => i,
        ["(i, j) => j"]                         = (int i, double j) => j,
        ["(s,d) => true"]                       = (string s, DateTime d) => true,
        ["a => a._a"]                           = (TestMethods a) => a._a,
        ["a => a.A"]                            = (TestMethods a) => a.A,
        ["a => a.Method1()"]                    = () => TestMethods.Method1(),
        ["a => a.Method3(1,1)"]                 = (TestMethods a) => a.Method3(1, 1.1),
        ["a => a.Method4(42,3.14)"]             = (TestMethods a) => a.Method4(42, 3.14),
        ["(i, j) => (a=i)+(b=j)"]               = Expression.Lambda(
                                                            Expression.Block(
                                                                [_varA, _varB],
                                                                Expression.Assign( _varA, _paramI ),
                                                                Expression.Assign( _varB, _paramJ ),
                                                                Expression.Add(Expression.Convert(_varA, typeof(double)), _varB)),
                                                            _paramI, _paramJ),  // (int i, double j) => { int a; double b; a = i; b = j; return a + b },                                                  
    };
}