namespace vm2.ExpressionSerialization.TestsCommonData;

public static class UnaryTestData
{
    /// <summary>
    /// Gets the expression mapped to the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Expression.</returns>
    public static Expression GetExpression(string id) => _substitutes[id];

    static ParameterExpression _pa = Expression.Parameter(typeof(int), "a");

    static Dictionary<string, Expression> _substitutes = new()
    {
        ["default(bool)"]                   = Expression.Default(typeof(bool)),
        ["default(char)"]                   = Expression.Default(typeof(char)),
        ["default(double)"]                 = Expression.Default(typeof(double)),
        ["default(half)"]                   = Expression.Default(typeof(Half)),
        ["default(int)"]                    = Expression.Default(typeof(int)),
        ["default(long)"]                   = Expression.Default(typeof(long)),
        ["default(DateTime)"]               = Expression.Default(typeof(DateTime)),
        ["default(DateTimeOffset)"]         = Expression.Default(typeof(DateTimeOffset)),
        ["default(TimeSpan)"]               = Expression.Default(typeof(TimeSpan)),
        ["default(decimal)"]                = Expression.Default(typeof(decimal)),
        ["default(Guid)"]                   = Expression.Default(typeof(Guid)),
        ["default(string)"]                 = Expression.Default(typeof(string)),
        ["default(object)"]                 = Expression.Default(typeof(object)),
        ["default(ClassDataContract1)"]     = Expression.Default(typeof(ClassDataContract1)),
        ["default(StructDataContract1)"]    = Expression.Default(typeof(StructDataContract1)),
        ["default(int?)"]                   = Expression.Default(typeof(int?)),
        ["default(StructDataContract1?)"]   = Expression.Default(typeof(StructDataContract1?)),

        ["(C c) => c as A"]                 = (C c) => c as A,
        ["(object c) => c as int?"]         = (object c) => c as int?,
        ["(int a) => () => a"]              = Expression.Quote(Expression.Lambda(_pa)),
        ["(double a) => checked((int)a)"]   = (double a) => checked((int)a),
        ["(double a) => (int)a"]            = (double a) => (int)a,
        ["(int[] a) => a.GetLength"]        = (int[] a) => a.Length,
        ["(bool a) => !a"]                  = (bool a) => !a,
        ["(int a) => checked(-a)"]          = (int a) => checked(-a),
        ["(int a) => -a"]                   = (int a) => -a,
        ["(int a) => ~a"]                   = (int a) => ~a,

        ["(A a) => +a"]                     = (A a) => +a,
        ["(A a) => -a"]                     = (A a) => -a,
        ["(B b) => !b"]                     = (B b) => !b,
    };
}
