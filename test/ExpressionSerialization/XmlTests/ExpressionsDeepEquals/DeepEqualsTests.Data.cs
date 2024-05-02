namespace vm2.ExpressionSerialization.XmlTests.ExpressionsDeepEquals;
public partial class DeepEqualsTests
{
    static readonly Dictionary<object, object?> _substituteConstants = new()
    {
        ["new DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local)"]                  = new DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local),
        ["new DateTime(2024, 4, 13, 23, 18, 26, 345, DateTimeKind.Local)"]                  = new DateTime(2024, 4, 13, 23, 18, 26, 345, DateTimeKind.Local),
        ["new TimeSpan(3, 4, 15, 32, 123)"]                                                 = new TimeSpan(3, 4, 15, 32, 123),
        ["new TimeSpan(3, 4, 15, 32, 234)"]                                                 = new TimeSpan(3, 4, 15, 32, 234),
        ["new DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0))"]      = new DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0)),
        ["new DateTimeOffset(2024, 4, 13, 23, 18, 26, 345, new TimeSpan(0, -300, 0))"]      = new DateTimeOffset(2024, 4, 13, 23, 18, 26, 345, new TimeSpan(0, -300, 0)),
        ["new int[]{ 1, 2, 3, 4 }"]                                                         = new int[]{ 1, 2, 3, 4 },
        ["new int[]{ 1, 2, 3, 5 }"]                                                         = new int[]{ 1, 2, 3, 5 },
        ["ArraySegment<byte>1"]                                                             = new ArraySegment<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10], 1, 8),
        ["ArraySegment<byte>2"]                                                             = new ArraySegment<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10], 2, 8),
        ["new object1()"]                                                                   = new object(),
        ["new object2()"]                                                                   = new object(),
        ["(IntPtr)5"]                                                                       = (IntPtr)5,
        ["(IntPtr)6"]                                                                       = (IntPtr)6,
        ["new decimal[]{ 1, 2, 3, 4 }.ToFrozenSet()"]                                       = new decimal[]{ 1, 2, 3, 4 }.ToFrozenSet(),
        ["new decimal[]{ 1, 2, 3, 5 }.ToFrozenSet()"]                                       = new decimal[]{ 1, 2, 3, 5 }.ToFrozenSet(),
        ["new Queue<int>([ 1, 2, 3, 4 ])"]                                                  = new Queue<int>([ 1, 2, 3, 4 ]),
        ["new Queue<int>([ 1, 2, 3, 5 ])"]                                                  = new Queue<int>([ 1, 2, 3, 5 ]),
        ["ImmutableArray.Create(1, 2, 3, 4 )"]                                              = ImmutableArray.Create(1, 2, 3, 4 ),
        ["ImmutableArray.Create(1, 2, 3, 5 )"]                                              = ImmutableArray.Create(1, 2, 3, 4 ),
        ["new ConcurrentStack<int>([1, 2, 3, 4])"]                                          = new ConcurrentStack<int>([1, 2, 3, 4]),
        ["new ConcurrentStack<int>([1, 2, 3, 5])"]                                          = new ConcurrentStack<int>([1, 2, 3, 5]),
        ["new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" }.ToFrozenDictionary()"]   = new Dictionary<int, string>{ [1] ="one", [2]="two" }.ToFrozenDictionary(),
        ["new Dictionary<int, string>{ [1] =\"one\", [3]=\"three\" }.ToFrozenDictionary()"] = new Dictionary<int, string>{ [1] ="one", [3]="three" }.ToFrozenDictionary(),
        ["new ClassDataContract1()"]                                                        = new ClassDataContract1(),
        ["new ClassDataContract2()"]                                                        = new ClassDataContract1(),
        ["new ClassDataContract3()"]                                                        = new ClassDataContract1() { IntProperty = 8 },
        ["new ClassDataContract1[] { new(0, \"vm\"), new(1, \"vm2 vm\"), }"]                = new ClassDataContract1[] { new(0, "vm"), new(1, "vm2 vm"), },
    };

    static ParameterExpression _paramA = Expression.Parameter(typeof(int), "a");
    static ParameterExpression _paramB = Expression.Parameter(typeof(int), "b");
    static ConstantExpression _const1 = Expression.Constant(1, typeof(int));
    static ConstantExpression _const2 = Expression.Constant(2, typeof(int));

    static readonly Dictionary<string, Func<Expression>> _substituteExpressions = new ()
    {
        ["a = 1"]           = () => Expression.Assign(_paramA, _const1),
        ["a = 2"]           = () => Expression.Assign(_paramA, _const2),
        ["a = b"]           = () => Expression.Assign(_paramA, _paramB),
        ["a += b"]          = () => Expression.AddAssign(_paramA, _paramB),
        ["a += 1"]          = () => Expression.AddAssign(_paramA, _const1),
        ["a -= b"]          = () => Expression.SubtractAssign(_paramA, _paramB),
        ["a *= b"]          = () => Expression.MultiplyAssign(_paramA, _paramB),
        ["checked(a *= b)"] = () => Expression.MultiplyAssignChecked(_paramA, _paramB),
        ["checked(a *= 2)"] = () => Expression.MultiplyAssignChecked(_paramA, _const2),
    };
}
