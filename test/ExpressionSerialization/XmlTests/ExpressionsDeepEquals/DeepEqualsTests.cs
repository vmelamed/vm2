namespace vm2.ExpressionSerialization.XmlTests.ExpressionsDeepEquals;

using vm2.ExpressionSerialization.ExpressionsDeepEquals;

public partial class DeepEqualsTests
{
    [Theory]
    [InlineData(null, typeof(object), null, typeof(object), true)]

    [InlineData((byte)1, typeof(byte), (byte)1, typeof(byte), true)]
    [InlineData((byte)1, typeof(byte), (byte)2, typeof(byte), false)]
    [InlineData((byte)1, typeof(byte), 2, typeof(int), false)]

    [InlineData((byte)1, typeof(byte?), (byte)1, typeof(byte?), true)]
    [InlineData((byte)1, typeof(byte?), (byte)2, typeof(byte?), false)]
    [InlineData((byte)1, typeof(byte?), null, typeof(byte?), false)]
    [InlineData(null, typeof(byte?), null, typeof(byte?), true)]
    [InlineData((byte)1, typeof(byte?), 2, typeof(int), false)]

    [InlineData(true, typeof(bool), true, typeof(bool), true)]
    [InlineData(true, typeof(bool), false, typeof(bool), false)]
    [InlineData(true, typeof(bool), 1, typeof(int), false)]

    [InlineData(true, typeof(bool?), true, typeof(bool?), true)]
    [InlineData(true, typeof(bool?), false, typeof(bool?), false)]
    [InlineData(true, typeof(bool?), null, typeof(bool?), false)]
    [InlineData(null, typeof(bool?), null, typeof(bool?), true)]
    [InlineData(true, typeof(bool?), 1, typeof(int), false)]

    [InlineData('a', typeof(char), 'a', typeof(char), true)]
    [InlineData('a', typeof(char), 'z', typeof(char), false)]
    [InlineData('a', typeof(char), 1, typeof(int), false)]

    [InlineData('a', typeof(char?), 'a', typeof(char?), true)]
    [InlineData('a', typeof(char?), 'z', typeof(char?), false)]
    [InlineData('a', typeof(char?), null, typeof(char?), false)]
    [InlineData(null, typeof(char?), null, typeof(char?), true)]
    [InlineData('a', typeof(char?), 1, typeof(int), false)]

    [InlineData(1, typeof(int), 1, typeof(int), true)]
    [InlineData(1, typeof(int), 2, typeof(int), false)]
    [InlineData(1, typeof(int), 2.0, typeof(double), false)]

    [InlineData(1, typeof(int?), 1, typeof(int?), true)]
    [InlineData(1, typeof(int?), 2, typeof(int?), false)]
    [InlineData(1, typeof(int?), null, typeof(int?), false)]
    [InlineData(null, typeof(int?), null, typeof(int?), true)]
    [InlineData(1, typeof(int?), 2.0, typeof(double), false)]

    [InlineData(EnumTest.One, typeof(EnumTest), EnumTest.One, typeof(EnumTest), true)]
    [InlineData(EnumTest.One, typeof(EnumTest), EnumTest.Two, typeof(EnumTest), false)]
    [InlineData(EnumTest.One, typeof(EnumTest), 2.0, typeof(double), false)]

    [InlineData(EnumTest.One, typeof(EnumTest?), EnumTest.One, typeof(EnumTest?), true)]
    [InlineData(EnumTest.One, typeof(EnumTest?), EnumTest.Two, typeof(EnumTest?), false)]
    [InlineData(EnumTest.One, typeof(EnumTest?), null, typeof(EnumTest?), false)]
    [InlineData(null, typeof(EnumTest?), null, typeof(EnumTest?), true)]
    [InlineData(EnumTest.One, typeof(EnumTest?), 2.0, typeof(double), false)]

    [InlineData("one", typeof(string), "one", typeof(string), true)]
    [InlineData("one", typeof(string), "two", typeof(string), false)]
    [InlineData("one", typeof(string), null, typeof(string), false)]
    [InlineData(null, typeof(string), null, typeof(string), true)]

    [InlineData("new DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local)", typeof(DateTime), "new DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local)", typeof(DateTime), true)]
    [InlineData("new DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local)", typeof(DateTime), "new DateTime(2024, 4, 13, 23, 18, 26, 345, DateTimeKind.Local)", typeof(DateTime), false)]

    [InlineData("new TimeSpan(3, 4, 15, 32, 123)", typeof(TimeSpan), "new TimeSpan(3, 4, 15, 32, 123)", typeof(TimeSpan), true)]
    [InlineData("new TimeSpan(3, 4, 15, 32, 123)", typeof(TimeSpan), "new TimeSpan(3, 4, 15, 32, 234)", typeof(TimeSpan), false)]

    [InlineData("new DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0))", typeof(DateTimeOffset), "new DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0))", typeof(DateTimeOffset), true)]
    [InlineData("new DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0))", typeof(DateTimeOffset), "new DateTimeOffset(2024, 4, 13, 23, 18, 26, 345, new TimeSpan(0, -300, 0))", typeof(DateTimeOffset), false)]

    [InlineData("ArraySegment<byte>1", typeof(ArraySegment<byte>), "ArraySegment<byte>1", typeof(ArraySegment<byte>), true)]
    [InlineData("ArraySegment<byte>1", typeof(ArraySegment<byte>), "ArraySegment<byte>2", typeof(ArraySegment<byte>), false)]

    [InlineData("(IntPtr)5", typeof(IntPtr), "(IntPtr)5", typeof(IntPtr), true)]
    [InlineData("(IntPtr)5", typeof(IntPtr), "(IntPtr)6", typeof(IntPtr), false)]

    [InlineData("new object1()", typeof(object), "new object1()", typeof(object), true)]
    [InlineData("new object1()", typeof(object), "new object2()", typeof(object), false)]
    [InlineData("new object1()", typeof(object), null, typeof(object), false)]

    [InlineData("new decimal[]{ 1, 2, 3, 4 }.ToFrozenSet()", typeof(FrozenSet<decimal>), "new decimal[]{ 1, 2, 3, 4 }.ToFrozenSet()", typeof(FrozenSet<decimal>), true)]
    [InlineData("new decimal[]{ 1, 2, 3, 4 }.ToFrozenSet()", typeof(FrozenSet<decimal>), "new decimal[]{ 1, 2, 3, 5 }.ToFrozenSet()", typeof(FrozenSet<decimal>), false)]

    [InlineData("new Queue<int>([ 1, 2, 3, 4 ])", typeof(Queue<int>), "new Queue<int>([ 1, 2, 3, 4 ])", typeof(Queue<int>), true)]
    [InlineData("new Queue<int>([ 1, 2, 3, 4 ])", typeof(Queue<int>), "new Queue<int>([ 1, 2, 3, 5 ])", typeof(Queue<int>), false)]

    [InlineData("ImmutableArray.Create(1, 2, 3, 5 )", typeof(ImmutableArray<int>), "ImmutableArray.Create(1, 2, 3, 5 )", typeof(ImmutableArray<int>), true)]
    [InlineData("ImmutableArray.Create(1, 2, 3, 5 )", typeof(ImmutableArray<int>), "ImmutableArray.Create(1, 2, 3, 4 )", typeof(ImmutableArray<int>), false)]

    [InlineData("new ConcurrentStack<int>([1, 2, 3, 4])", typeof(ConcurrentStack<int>), "new ConcurrentStack<int>([1, 2, 3, 4])", typeof(ConcurrentStack<int>), true)]
    [InlineData("new ConcurrentStack<int>([1, 2, 3, 4])", typeof(ConcurrentStack<int>), "new ConcurrentStack<int>([1, 2, 3, 5])", typeof(ConcurrentStack<int>), false)]

    [InlineData("new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" }.ToFrozenDictionary()", typeof(FrozenDictionary<int, string>), "new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" }.ToFrozenDictionary()", typeof(FrozenDictionary<int, string>), true)]
    [InlineData("new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" }.ToFrozenDictionary()", typeof(FrozenDictionary<int, string>), "new Dictionary<int, string>{ [1] =\"one\", [3]=\"three\" }.ToFrozenDictionary()", typeof(FrozenDictionary<int, string>), false)]

    [InlineData("new ClassDataContract1()", typeof(ClassDataContract1), "new ClassDataContract1()", typeof(ClassDataContract1), true)]
    [InlineData("new ClassDataContract1()", typeof(ClassDataContract1), "new ClassDataContract2()", typeof(ClassDataContract1), true)]
    [InlineData("new ClassDataContract1()", typeof(ClassDataContract1), "new ClassDataContract3()", typeof(ClassDataContract1), false)]

    //[InlineData("(IntPtr)5", typeof(IntPtr), "(IntPtr)5", typeof(IntPtr), true)]
    //[InlineData("(IntPtr)5", typeof(IntPtr), "(IntPtr)6", typeof(IntPtr), false)]

    public void ConstantExpressionTest(object? v1, Type type1, object? v2, Type type2, bool areEqual)
    {
        ConstantExpression? x1 = null;
        ConstantExpression? x2 = null;

        if (v1 is not string or "one" or "two")
        {
            x1 = Expression.Constant(v1, type1);
            x2 = Expression.Constant(v2, type2);
        }
        else
        {
            x1 = v1 is not null ? Expression.Constant(_substituteConstants[v1], type1) : Expression.Constant(null, type1);
            x2 = v2 is not null ? Expression.Constant(_substituteConstants[v2], type2) : Expression.Constant(null, type2);
        }

        if (areEqual)
            x1.DeepEquals(x2).Should().BeTrue();
        else
            x1.DeepEquals(x2).Should().BeFalse();
    }

    [Theory]
    [InlineData("a = 1", "a = 1", true)]
    [InlineData("a = 1", "a = 2", false)]
    [InlineData("a = b", "a = b", true)]
    [InlineData("a = b", "a = 2", false)]
    [InlineData("a = b", "a += 1", false)]

    [InlineData("a += b", "a += b", true)]
    [InlineData("a += b", "a += 1", false)]
    [InlineData("a += b", "a -= b", false)]
    [InlineData("checked(a *= b)", "checked(a *= b)", true)]
    [InlineData("checked(a *= b)", "a *= b", false)]
    [InlineData("checked(a *= b)", "checked(a *= 2)", false)]

    public void ExpressionTest(string expr1, string expr2, bool areEqual)
    {
        Expression x1 = _substituteExpressions[expr1]();
        Expression x2 = _substituteExpressions[expr2]();
        if (areEqual)
            x1.DeepEquals(x2).Should().BeTrue();
        else
            x1.DeepEquals(x2).Should().BeFalse();
    }
}
