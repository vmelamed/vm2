﻿namespace vm2.ExpressionSerialization.CommonData;

public static class ConstantTestData
{
    /// <summary>
    /// The maximum long number that can be expressed as &quot;JSON integer&quot; without loosing fidelity.
    /// </summary>
    /// <remarks>
    /// In JavaScript, the maximum safe integer is 2^53 - 1, which is 9007199254740991. This is because JavaScript uses 
    /// double-precision floating-point format numbers as specified in IEEE 754, and can only safely represent integers 
    /// between [-(2^53-1), 2^53 - 1].
    /// Therefore we serialize numbers outside of that range as strings, e.g. <c>&quot;9007199254740992&quot;</c>.
    /// </remarks>
    public static readonly long MaxJsonInteger = (long)Math.Pow(2, 53);

    /// <summary>
    /// The minimum long number that can be expressed as &quot;JSON integer&quot; without loosing fidelity.
    /// </summary>
    /// <remarks>
    /// In JavaScript, the maximum safe integer is 2^53 - 1, which is 9007199254740991. This is because JavaScript uses 
    /// double-precision floating-point format numbers as specified in IEEE 754, and can only safely represent integers 
    /// from the range [-(2^53-1), 2^53-1].
    /// Therefore we serialize numbers outside of that range as strings, e.g. <c>&quot;-9007199254740992&quot;</c>.
    /// </remarks>
    public static readonly long MinJsonInteger = -MaxJsonInteger;

    /// <summary>
    /// Gets the expression mapped to the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns>Expression.</returns>
    public static Expression GetExpression(string id) => _substitutes[id];

    static Dictionary<string, ConstantExpression> _substitutes = new()
    {
        // bool
        ["false"]                                                                               = Expression.Constant(false),
        ["true"]                                                                                = Expression.Constant(true),
        // byte
        ["(byte)5"]                                                                             = Expression.Constant((byte)5),
        // char
        ["'V'"]                                                                                 = Expression.Constant('V'),
        // double
        ["double.MinValue"]                                                                     = Expression.Constant(double.MinValue),
        ["double.MaxValue"]                                                                     = Expression.Constant(double.MaxValue),
        ["double.float.MinValue"]                                                               = Expression.Constant((double)float.MinValue),
        ["double.float.MaxValue"]                                                               = Expression.Constant((double)float.MaxValue),
        ["double.BigValue"]                                                                     = Expression.Constant(1.7976931348623157e308),
        ["double.SmallValue"]                                                                   = Expression.Constant(-1.7976931348623157e+308),
        ["double.Nan"]                                                                          = Expression.Constant(double.NaN),
        ["double.NegativeInfinity"]                                                             = Expression.Constant(double.NegativeInfinity),
        ["double.PositiveInfinity"]                                                             = Expression.Constant(double.PositiveInfinity),
        ["double.NegativeZero"]                                                                 = Expression.Constant(double.NegativeZero),
        ["double.Zero"]                                                                         = Expression.Constant(0.0),
        ["double.Epsilon"]                                                                      = Expression.Constant(double.Epsilon),
        ["double.PI"]                                                                           = Expression.Constant(double.Pi),
        ["double.E"]                                                                            = Expression.Constant(double.E),
        ["-2.234567891233658E-123"]                                                             = Expression.Constant(-2.234567891233658E-123),
        ["5.1234567891234567E-123"]                                                             = Expression.Constant(5.1234567891234567E-123),
        ["-3.4028234663852886E+38"]                                                             = Expression.Constant(-3.4028234663852886E+38),
        [ "3.4028234663852886E+38"]                                                             = Expression.Constant(3.4028234663852886E+38),
        // float
        ["float.MinValue"]                                                                      = Expression.Constant(float.MinValue),
        ["float.MaxValue"]                                                                      = Expression.Constant(float.MaxValue),
        ["float.Nan"]                                                                           = Expression.Constant(float.NaN),
        ["float.NegativeInfinity"]                                                              = Expression.Constant(float.NegativeInfinity),
        ["float.PositiveInfinity"]                                                              = Expression.Constant(float.PositiveInfinity),
        ["float.Epsilon"]                                                                       = Expression.Constant(float.Epsilon),
        ["float.NegativeZero"]                                                                  = Expression.Constant(float.NegativeZero),
        ["float.Zero"]                                                                          = Expression.Constant(0.0F),
        ["-2.234568E-23F"]                                                                      = Expression.Constant(-2.234568E-23F),
        ["5.5123453E-34F"]                                                                      = Expression.Constant(5.5123453E-34F),
        // Half
        ["(Half)3.14"]                                                                          = Expression.Constant((Half)3.14),
        ["Half.E"]                                                                              = Expression.Constant(Half.E),
        ["Half.MinValue"]                                                                       = Expression.Constant(Half.MinValue),
        ["Half.MaxValue"]                                                                       = Expression.Constant(Half.MaxValue),
        ["Half.Zero"]                                                                           = Expression.Constant(Half.Zero),
        ["Half.One"]                                                                            = Expression.Constant(Half.One),
        ["Half.NaN"]                                                                            = Expression.Constant(Half.NaN),
        ["Half.NegativeInfinity"]                                                               = Expression.Constant(Half.NegativeInfinity),
        ["Half.PositiveInfinity"]                                                               = Expression.Constant(Half.PositiveInfinity),
        ["Half.Pi"]                                                                             = Expression.Constant(Half.Pi),
        ["Half.Epsilon"]                                                                        = Expression.Constant(Half.Epsilon),
        ["Half.NegativeOne"]                                                                    = Expression.Constant(Half.NegativeOne),
        ["Half.NegativeZero"]                                                                   = Expression.Constant(Half.NegativeZero),
        // int
        ["5"]                                                                                   = Expression.Constant(5),
        ["42"]                                                                                  = Expression.Constant(42),
        ["int.Min"]                                                                             = Expression.Constant(int.MinValue),
        ["int.Max"]                                                                             = Expression.Constant(int.MaxValue),
        // IntPtr
        ["IntPtr5"]                                                                             = Expression.Constant((IntPtr)5),
        ["IntPtr23"]                                                                            = Expression.Constant((IntPtr)23),
        ["IntPtr.MinValue"]                                                                     = Expression.Constant(IntPtr.MinValue),
        ["IntPtr.MaxValue"]                                                                     = Expression.Constant(IntPtr.MaxValue),
        // long
        ["long0"]                                                                               = Expression.Constant(0L),
        ["5L"]                                                                                  = Expression.Constant(5L),
        ["long.Min"]                                                                            = Expression.Constant(long.MinValue),
        ["long.Max"]                                                                            = Expression.Constant(long.MaxValue),
        ["long.IntMin"]                                                                         = Expression.Constant(MinJsonInteger),
        ["long.IntMax"]                                                                         = Expression.Constant(MaxJsonInteger),
        ["long.IntMin-1"]                                                                       = Expression.Constant(MinJsonInteger-1),
        ["long.IntMax+1"]                                                                       = Expression.Constant(MaxJsonInteger+1),
        ["long.IntMin+1"]                                                                       = Expression.Constant(MinJsonInteger+1),
        ["long.IntMax-1"]                                                                       = Expression.Constant(MaxJsonInteger-1),
        // sbyte
        ["(sbyte)5"]                                                                            = Expression.Constant((sbyte)5),
        ["(sbyte)-5"]                                                                           = Expression.Constant((sbyte)-5),
        ["sbyte.MinValue"]                                                                      = Expression.Constant(sbyte.MinValue),
        ["sbyte.MaxValue"]                                                                      = Expression.Constant(sbyte.MaxValue),
        // short
        ["(short)32000"]                                                                        = Expression.Constant((short)32000),
        ["short.MinValue"]                                                                      = Expression.Constant(short.MinValue),
        ["short.MaxValue"]                                                                      = Expression.Constant(short.MaxValue),
        // uint
        ["(uint)0"]                                                                             = Expression.Constant((uint)0),
        ["(uint)5"]                                                                             = Expression.Constant((uint)5),
        ["(uint)42"]                                                                            = Expression.Constant((uint)42),
        ["uint.Min"]                                                                            = Expression.Constant(uint.MinValue),
        ["uint.Max"]                                                                            = Expression.Constant(uint.MaxValue),
        // UIntPtr
        ["(UnsignedIntPtr)5"]                                                                   = Expression.Constant((UIntPtr)5),
        ["(UnsignedIntPtr)42"]                                                                  = Expression.Constant((UIntPtr)42),
        ["UnsignedIntPtr.MinValue"]                                                             = Expression.Constant(UIntPtr.MinValue),
        ["UnsignedIntPtr.MaxValue"]                                                             = Expression.Constant(UIntPtr.MaxValue),
        // ulong
        ["ulong0"]                                                                              = Expression.Constant(0UL),
        ["(ulong)5"]                                                                            = Expression.Constant((ulong)5),
        ["ulong.Min"]                                                                           = Expression.Constant(ulong.MinValue),
        ["ulong.Max"]                                                                           = Expression.Constant(ulong.MaxValue),
        ["ulong.IntMax-1"]                                                                      = Expression.Constant((ulong)MaxJsonInteger-1),
        ["ulong.IntMax"]                                                                        = Expression.Constant((ulong)MaxJsonInteger),
        ["ulong.IntMax+1"]                                                                      = Expression.Constant((ulong)MaxJsonInteger+1),
        // ushort
        ["(ushort)5"]                                                                           = Expression.Constant((ushort)5),
        ["(ushort)443"]                                                                         = Expression.Constant((ushort)443),
        ["ushort.MinValue"]                                                                     = Expression.Constant(ushort.MinValue),
        ["ushort.MaxValue"]                                                                     = Expression.Constant(ushort.MaxValue),
        // DateTime
        ["DateTime.MinValue"]                                                                   = Expression.Constant(DateTime.MinValue),
        ["DateTime.MaxValue"]                                                                   = Expression.Constant(DateTime.MaxValue),
        ["DateTime(2024, 4, 13, 23, 18, 26, 234)"]                                              = Expression.Constant(new DateTime(2024, 4, 13, 23, 18, 26, 234)),
        ["DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local)"]                          = Expression.Constant(new DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local)),
        // DateTimeOffset
        ["DateTimeOffset.MinValue"]                                                             = Expression.Constant(DateTimeOffset.MinValue),
        ["DateTimeOffset.MaxValue"]                                                             = Expression.Constant(DateTimeOffset.MaxValue),
        ["DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0))"]              = Expression.Constant(new DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0))),
        // TimeSpan
        ["TimeSpan.MinValue"]                                                                   = Expression.Constant(TimeSpan.MinValue),
        ["TimeSpan.MaxValue"]                                                                   = Expression.Constant(TimeSpan.MaxValue),
        ["TimeSpan.Zero"]                                                                       = Expression.Constant(TimeSpan.Zero),
        ["TimeSpan(3, 4, 15, 32, 123)"]                                                         = Expression.Constant(new TimeSpan(3, 4, 15, 32, 123)),
        ["TimeSpan(-3, 4, 15, 32, 123)"]                                                        = Expression.Constant(new TimeSpan(3, 4, 15, 32, 123).Negate()),
        // DBNull
        ["DBNull.Value"]                                                                        = Expression.Constant(DBNull.Value),
        // decimal
        ["decimal.Zero"]                                                                        = Expression.Constant(decimal.Zero),
        ["decimal.MinusOne"]                                                                    = Expression.Constant(decimal.MinusOne),
        ["decimal.One"]                                                                         = Expression.Constant(decimal.One),
        ["decimal.MinValue"]                                                                    = Expression.Constant(decimal.MinValue),
        ["decimal.MaxValue"]                                                                    = Expression.Constant(decimal.MaxValue),
        ["5.5M"]                                                                                = Expression.Constant(5.5M),
        // GUID
        ["Guid.Empty"]                                                                          = Expression.Constant(Guid.Empty),
        ["Guid(\"00112233-4455-6677-8899-aabbccddeeff\")"]                                      = Expression.Constant(new Guid("00112233-4455-6677-8899-aabbccddeeff")),
        // string
        ["string.Empty"]                                                                        = Expression.Constant(string.Empty),
        ["(string?)null"]                                                                       = Expression.Constant(null, typeof(string)),
        ["abrah-cadabrah"]                                                                      = Expression.Constant("abrah-cadabrah"),
        ["ала-бала"]                                                                            = Expression.Constant("ала-бала"),
        // Uri
        ["Uri(\"http://www.delinea.com\")"]                                                     = Expression.Constant(new Uri("http://www.delinea.com")),
        // enum
        ["EnumFlagsTest.One | EnumFlagsTest.Three"]                                             = Expression.Constant(EnumFlagsTest.One | EnumFlagsTest.Three),
        ["EnumTest.Three"]                                                                      = Expression.Constant(EnumTest.Three),
        // nullable primitive
        ["(int?)5"]                                                                             = Expression.Constant(5, typeof(int?)),
        ["(int?)null"]                                                                          = Expression.Constant(null, typeof(int?)),
        ["(long?)5L"]                                                                           = Expression.Constant(5L, typeof(long?)),
        ["(long?)null"]                                                                         = Expression.Constant(null, typeof(long?)),
        ["(long?)long.Min"]                                                                     = Expression.Constant(long.MinValue, typeof(long?)),
        ["(long?)long.Max"]                                                                     = Expression.Constant(long.MaxValue, typeof(long?)),
        ["(long?)long.IntMin"]                                                                  = Expression.Constant(MinJsonInteger, typeof(long?)),
        ["(long?)long.IntMax"]                                                                  = Expression.Constant(MaxJsonInteger, typeof(long?)),
        ["(long?)long.IntMin-1"]                                                                = Expression.Constant(MinJsonInteger-1, typeof(long?)),
        ["(long?)long.IntMax+1"]                                                                = Expression.Constant(MaxJsonInteger+1, typeof(long?)),
        ["(long?)long.IntMin+1"]                                                                = Expression.Constant(MinJsonInteger+1, typeof(long?)),
        ["(long?)long.IntMax-1"]                                                                = Expression.Constant(MaxJsonInteger-1, typeof(long?)),
        // objects
        ["null"]                                                                                = Expression.Constant(null),
        ["object()"]                                                                            = Expression.Constant(new object()),
        ["Object1()"]                                                                           = Expression.Constant(new Object1(), typeof(Object1)),
        ["(Object1)null"]                                                                       = Expression.Constant(null, typeof(Object1)),
        ["ClassDataContract1()"]                                                                = Expression.Constant(new ClassDataContract1()),
        ["ClassDataContract2()"]                                                                = Expression.Constant(new ClassDataContract2(1, "two", 3M), typeof(ClassDataContract1)),
        ["ClassSerializable1()"]                                                                = Expression.Constant(new ClassSerializable1()),
        // structs
        ["(StructDataContract1?)null"]                                                          = Expression.Constant(null, typeof(StructDataContract1?)),
        ["StructDataContract1() { IntProperty = 7, StringProperty = \"vm\" }"]                  = Expression.Constant(new StructDataContract1() { IntProperty = 7, StringProperty = "vm" }, typeof(StructDataContract1)),
        ["StructDataContract1()"]                                                               = Expression.Constant(new StructDataContract1()),
        ["StructDataContract1?() { IntProperty = 7, StringProperty = \"vm\" }"]                 = Expression.Constant((StructDataContract1?)new StructDataContract1() { IntProperty = 7, StringProperty = "vm" }, typeof(StructDataContract1?)),
        ["(StructSerializable1?)null"]                                                          = Expression.Constant(null, typeof(StructSerializable1?)),
        ["StructSerializable1() { IntProperty = 7, StringProperty = \"vm\" }"]                  = Expression.Constant(new StructSerializable1() { IntProperty = 7, StringProperty = "vm" }, typeof(StructSerializable1)),
        ["StructSerializable1()"]                                                               = Expression.Constant(new StructSerializable1()),
        // anonymous
        ["anonymous"]                                                                           = Expression.Constant(
                                                                                                                        new
                                                                                                                        {
                                                                                                                            ObjectProperty = (object?)null,
                                                                                                                            NullIntProperty = (int?)null,
                                                                                                                            NullLongProperty = (long?)1L,
                                                                                                                            BoolProperty = true,
                                                                                                                            CharProperty = 'A',
                                                                                                                            ByteProperty = (byte)1,
                                                                                                                            SByteProperty = (sbyte)1,
                                                                                                                            ShortProperty = (short)1,
                                                                                                                            IntProperty = 1,
                                                                                                                            LongProperty = (long)1,
                                                                                                                            UShortProperty = (ushort)1,
                                                                                                                            UIntProperty = (uint)1,
                                                                                                                            ULongProperty = (ulong)1,
                                                                                                                            DoubleProperty = 1.0,
                                                                                                                            FloatProperty = (float)1.0,
                                                                                                                            DecimalProperty = 1M,
                                                                                                                            GuidProperty = Guid.Empty,
                                                                                                                            UriProperty = new Uri("http://localhost"),
                                                                                                                            DateTimeProperty = new DateTime(2013, 1, 13),
                                                                                                                            DateTimeOffsetProperty = new DateTimeOffset(new DateTime(2013, 1, 13)),
                                                                                                                            TimeSpanProperty = new TimeSpan(1, 2, 3, 4, 5, 6),
                                                                                                                        }),
        // byte sequences
        ["(byte[])null"]                                                                        = Expression.Constant(null, typeof(byte[])),
        ["byte[]{}"]                                                                            = Expression.Constant(Array.Empty<byte>()),
        ["byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }"]                                             = Expression.Constant(new byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }),
        ["Memory<byte>()"]                                                                      = Expression.Constant(new Memory<byte>()),
        ["Memory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])"]                                      = Expression.Constant(new Memory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])),
        ["ReadOnlyMemory<byte>()"]                                                              = Expression.Constant(new ReadOnlyMemory<byte>()),
        ["ReadOnlyMemory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])"]                              = Expression.Constant(new ReadOnlyMemory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])),
        ["ArraySegment<byte>([])"]                                                              = Expression.Constant(new ArraySegment<byte>([])),
        ["ArraySegment<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10], 1, 8)"]                           = Expression.Constant(new ArraySegment<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10], 1, 8)),
        // sequences
        ["(int[])null"]                                                                         = Expression.Constant(null, typeof(int[])),
        ["int[]{}"]                                                                             = Expression.Constant(Array.Empty<int>()),
        ["int[]{ 1, 2, 3, 4 }"]                                                                 = Expression.Constant(new int[]{ 1, 2, 3, 4 }),
        ["(int?[])null"]                                                                        = Expression.Constant(null, typeof(int?[])),
        ["int?[]{}"]                                                                            = Expression.Constant(Array.Empty<int?>()),
        ["int?[]{ 1, 2, null, null }"]                                                          = Expression.Constant(new int?[]{ 1, 2, null, null }),
        ["Memory<int>(null)"]                                                                   = Expression.Constant(new Memory<int>(null)),
        ["Memory<int>()"]                                                                       = Expression.Constant(new Memory<int>()),
        ["Memory<int>([ 1, 2, 3, 4 ])"]                                                         = Expression.Constant(new Memory<int>([ 1, 2, 3, 4 ])),
        ["(Memory<int>?)null"]                                                                  = Expression.Constant(null, typeof(Memory<int>?)),
        ["(Memory<int>?)([ 1, 2, 3, 4 ])"]                                                      = Expression.Constant(new Memory<int>([ 1, 2, 3, 4 ]), typeof(Memory<int>?)),
        ["(Memory<int>?)()"]                                                                    = Expression.Constant(new Memory<int>(), typeof(Memory<int>?)),
        ["EnumTest?[]{}"]                                                                       = Expression.Constant(Array.Empty<EnumTest?>()),
        ["EnumTest?[]{ EnumTest.One, EnumTest.Two, null, null }"]                               = Expression.Constant(new EnumTest?[]{ EnumTest.One, EnumTest.Two, null, null }),
        ["(EnumTest?[])null"]                                                                   = Expression.Constant(null, typeof(EnumTest?[])),
        ["ArraySegment<int>([ 1, 2, 3, 4 ], 1, 2)"]                                             = Expression.Constant(new ArraySegment<int>([ 1, 2, 3, 4 ], 1, 2)),
        ["(ArraySegment<int>)null"]                                                             = Expression.Constant(null, typeof(ArraySegment<int>?)),
        ["List<int?>{ 1, 2, null, null }"]                                                      = Expression.Constant(new List<int?>{ 1, 2, null, null }),
        ["(List<int?>)null"]                                                                    = Expression.Constant(null, typeof(List<int?>)),
        ["List<int>([1, 2, 3, 4])"]                                                             = Expression.Constant(new List<int>([1, 2, 3, 4])),
        ["LinkedList<int>([1, 2, 3, 4])"]                                                       = Expression.Constant(new LinkedList<int>([1, 2, 3, 4])),
        ["Sequence<int>([1, 2, 3, 4])"]                                                         = Expression.Constant(new Collection<int>([1, 2, 3, 4])),
        ["ReadOnlyCollection<int>([1, 2, 3, 4])"]                                               = Expression.Constant(new ReadOnlyCollection<int>([1, 2, 3, 4])),
        ["ReadOnlyMemory<int>([ 1, 2, 3, 4 ])"]                                                 = Expression.Constant(new ReadOnlyMemory<int>([ 1, 2, 3, 4 ])),
        ["HashSet<int>([1, 2, 3, 4])"]                                                          = Expression.Constant(new HashSet<int>([1, 2, 3, 4])),
        ["SortedSet<int>([1, 2, 3, 4])"]                                                        = Expression.Constant(new SortedSet<int>([1, 2, 3, 4])),
        ["Queue<int>([1, 2, 3, 4])"]                                                            = Expression.Constant(new Queue<int>([1, 2, 3, 4])),
        ["Stack<int>([1, 2, 3, 4])"]                                                            = Expression.Constant(new Stack<int>([1, 2, 3, 4])),
        ["BlockingCollection<double>()"]                                                        = Expression.Constant(new BlockingCollection<double>() { Math.PI, Math.Tau, Math.E }),
        ["ConcurrentBag<int>([1, 2, 3, 4])"]                                                    = Expression.Constant(new ConcurrentBag<int>([1, 2, 3, 4])),
        ["ConcurrentQueue<int>([1, 2, 3, 4])"]                                                  = Expression.Constant(new ConcurrentQueue<int>([1, 2, 3, 4])),
        ["ConcurrentStack<int>([1, 2, 3, 4])"]                                                  = Expression.Constant(new ConcurrentStack<int>([1, 2, 3, 4])),
        ["ImmutableArray.Create()"]                                                             = Expression.Constant(ImmutableArray.Create<int>()),
        ["ImmutableArray.Create(1, 2, 3, 4 )"]                                                  = Expression.Constant(ImmutableArray.Create(1, 2, 3, 4 )),
        ["ImmutableHashSet.Create(1, 2, 3, 4 )"]                                                = Expression.Constant(ImmutableHashSet.Create(1, 2, 3, 4 )),
        ["ImmutableList.Create(1, 2, 3, 4 )"]                                                   = Expression.Constant(ImmutableList.Create(1, 2, 3, 4 )),
        ["ImmutableQueue.Create(1, 2, 3, 4 )"]                                                  = Expression.Constant(ImmutableQueue.Create(1, 2, 3, 4 )),
        ["ImmutableSortedSet.Create(1, 2, 3, 4 )"]                                              = Expression.Constant(ImmutableSortedSet.Create(1, 2, 3, 4 )),
        ["ImmutableStack.Create(1, 2, 3, 4 )"]                                                  = Expression.Constant(ImmutableStack.Create(1, 2, 3, 4 )),
        ["ClassDataContract1[] { new ClassDataContract1()..."]                                  = Expression.Constant(new ClassDataContract1?[] { new(), new ClassDataContract2(), null }),
        ["ClassDataContract1[] { new(0, \"vm\"), new(1, \"vm2 vm\"), }"]                        = Expression.Constant(new ClassDataContract1[] { new(0, "vm"), new(1, "vm2 vm"), }),
        ["Frozen byte[]{}"]                                                                     = Expression.Constant(Array.Empty<int>().ToFrozenSet()),
        ["Frozen byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }"]                                      = Expression.Constant(new byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }.ToFrozenSet()),
        ["Frozen int[]{ 1, 2, 3, 4 }"]                                                          = Expression.Constant(new int[]{ 1, 2, 3, 4 }.ToFrozenSet()),
        ["Frozen int?[]{ 1, 2, null, null }"]                                                   = Expression.Constant(new int?[]{ 1, 2, null, null }.ToFrozenSet()),
        ["Frozen decimal[]{ 1, 2, 3, 4 }"]                                                      = Expression.Constant(new decimal[]{ 1, 2, 3, 4 }.ToFrozenSet()),
        ["Frozen EnumTest?[]{ EnumTest.One, EnumTest.Two, null, null }"]                        = Expression.Constant(new EnumTest?[]{ EnumTest.One, EnumTest.Two, null, null }.ToFrozenSet()),
        ["Frozen anonymous[]"]                                                                  = Expression.Constant(new object?[] {
                                                                                                                        new
                                                                                                                        {
                                                                                                                            ObjectProperty = (object?)null,
                                                                                                                            NullIntProperty = (int?)null,
                                                                                                                            NullLongProperty = (long?)1L,
                                                                                                                            BoolProperty = true,
                                                                                                                            CharProperty = 'A',
                                                                                                                            ByteProperty = (byte)1,
                                                                                                                            SByteProperty = (sbyte)1,
                                                                                                                            ShortProperty = (short)1,
                                                                                                                            IntProperty = 1,
                                                                                                                            LongProperty = (long)1,
                                                                                                                            UShortProperty = (ushort)1,
                                                                                                                            UIntProperty = (uint)1,
                                                                                                                            ULongProperty = (ulong)1,
                                                                                                                            DoubleProperty = 1.0,
                                                                                                                            FloatProperty = (float)1.0,
                                                                                                                            DecimalProperty = 1M,
                                                                                                                            GuidProperty = Guid.Empty,
                                                                                                                            UriProperty = new Uri("http://localhost"),
                                                                                                                            DateTimeProperty = new DateTime(2013, 1, 13),
                                                                                                                            TimeSpanProperty = new TimeSpan(123L),
                                                                                                                            DateTimeOffsetProperty = new DateTimeOffset(new DateTime(2013, 1, 13)),
                                                                                                                        },
                                                                                                                        new
                                                                                                                        {
                                                                                                                            ObjectProperty = (object?)null,
                                                                                                                            NullIntProperty = (int?)null,
                                                                                                                            NullLongProperty = (long?)2L,
                                                                                                                            BoolProperty = true,
                                                                                                                            CharProperty = 'A',
                                                                                                                            ByteProperty = (byte)2,
                                                                                                                            SByteProperty = (sbyte)2,
                                                                                                                            ShortProperty = (short)2,
                                                                                                                            IntProperty = 2,
                                                                                                                            LongProperty = (long)2,
                                                                                                                            UShortProperty = (ushort)2,
                                                                                                                            UIntProperty = (uint)2,
                                                                                                                            ULongProperty = (ulong)2,
                                                                                                                            DoubleProperty = 2.0,
                                                                                                                            FloatProperty = (float)2.0,
                                                                                                                            DecimalProperty = 2M,
                                                                                                                            GuidProperty = Guid.Empty,
                                                                                                                            UriProperty = new Uri("http://localhost"),
                                                                                                                            DateTimeProperty = new DateTime(2013, 2, 13),
                                                                                                                            TimeSpanProperty = new TimeSpan(123L),
                                                                                                                            DateTimeOffsetProperty = new DateTimeOffset(new DateTime(2013, 2, 13)),
                                                                                                                        },
                                                                                                                        null
                                                                                                                    }.ToFrozenSet()),
        // tuples
        ["(Tuple<int, string>)null"]                                                            = Expression.Constant(null, typeof(Tuple<int, string>)),
        ["Tuple<int, string>"]                                                                  = Expression.Constant(new Tuple<int, string>(1, "one")),
        ["ValueTuple<int, string>"]                                                             = Expression.Constant((1, "one")),
        // dictionaries
        ["Dictionary<int, string?>{ [1] = \"one\", [2] = \"two\"..."]                           = Expression.Constant(new Dictionary<int, string?>{ [1] = "one", [2] = "two", [3] = null, [4] = null }),
        ["Dictionary<int, string>{ [1] = \"one\", [2] = \"two\" }"]                             = Expression.Constant(new Dictionary<int, string>{ [1] ="one", [2]="two" }),
        ["Frozen Dictionary<int, string?>..."]                                                  = Expression.Constant(new Dictionary<int, string?>{ [1] = "one", [2] = "two", [3] = null, [4] = null }.ToFrozenDictionary()),
        ["Frozen Dictionary<int, string>..."]                                                   = Expression.Constant(new Dictionary<int, string>{ [1] = "one", [2] = "two", [3] = "three", }.ToFrozenDictionary()),
        ["Hashtable(new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" })"]                 = Expression.Constant(new Hashtable(new Dictionary<int, string>{ [1] ="one", [2]="two" })),
        ["ImmutableDictionary.Create<int,string>().Add(...)"]                                   = Expression.Constant(ImmutableDictionary.Create<int,string>().Add(1, "one").Add(2, "two")),
        ["ImmutableSortedDictionary.Create<int,string>().Add(...)"]                             = Expression.Constant(ImmutableSortedDictionary.Create<int,string>().Add(1, "one").Add(2, "two")),
        ["ReadOnlyDictionary<int, string>..."]                                                  = Expression.Constant(new ReadOnlyDictionary<int, string>(new Dictionary<int, string>{ [1] ="one", [2]="two" })),
        ["SortedDictionary<int, string>{ [1] =\"one\", [2]=\"two\" }"]                          = Expression.Constant(new SortedDictionary<int, string>{ [1] ="one", [2]="two" }),
        ["ConcurrentDictionary<int, string>{ [1] = \"one\", [2]=\"two\" }"]                     = Expression.Constant(new ConcurrentDictionary<int, string>{ [1] ="one", [2]="two" }),

        ["StructDataContract1[]"]                                                               = Expression.Constant(new StructDataContract1[]
                                                                                                                        {
                                                                                                                            new() {
                                                                                                                                IntProperty = 0,
                                                                                                                                StringProperty = "vm",
                                                                                                                            },
                                                                                                                            new() {
                                                                                                                                IntProperty = 1,
                                                                                                                                StringProperty = "vm vm",
                                                                                                                            },
                                                                                                                        }),
        ["StructDataContract1?[]"]                                                              = Expression.Constant(new StructDataContract1?[]
                                                                                                                        {
                                                                                                                            new() {
                                                                                                                                IntProperty = 0,
                                                                                                                                StringProperty = "vm",
                                                                                                                            },
                                                                                                                            null,
                                                                                                                            new() {
                                                                                                                                IntProperty = 1,
                                                                                                                                StringProperty = "vm vm",
                                                                                                                            },
                                                                                                                            null
                                                                                                                        }),
        ["StructSerializable1[]"]                                                               = Expression.Constant(new StructSerializable1[]
                                                                                                                        {
                                                                                                                            new () {
                                                                                                                                IntProperty = 0,
                                                                                                                                StringProperty = "vm",
                                                                                                                            },
                                                                                                                            new() {
                                                                                                                                IntProperty = 1,
                                                                                                                                StringProperty = "vm vm",
                                                                                                                            },
                                                                                                                        }),
        ["StructSerializable1?[]"]                                                              = Expression.Constant(new StructSerializable1?[]
                                                                                                                        {
                                                                                                                            new() {
                                                                                                                                IntProperty = 0,
                                                                                                                                StringProperty = "vm",
                                                                                                                            },
                                                                                                                            null,
                                                                                                                            new() {
                                                                                                                                IntProperty = 1,
                                                                                                                                StringProperty = "vm vm",
                                                                                                                            },
                                                                                                                            null
                                                                                                                        }),
    };
}