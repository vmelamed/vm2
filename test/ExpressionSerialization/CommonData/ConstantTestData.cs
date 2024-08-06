namespace vm2.ExpressionSerialization.TestsCommonData;

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
        ["Bool.false"]                                                                          = Expression.Constant(false),
        ["Bool.true"]                                                                           = Expression.Constant(true),
        // byte
        ["Byte.5"]                                                                              = Expression.Constant((byte)5),
        // char
        ["Char.'V'"]                                                                            = Expression.Constant('V'),
        // double
        ["Double.MinValue"]                                                                     = Expression.Constant(double.MinValue),
        ["Double.MaxValue"]                                                                     = Expression.Constant(double.MaxValue),
        ["Double.float.MinValue"]                                                               = Expression.Constant((double)float.MinValue),
        ["Double.float.MaxValue"]                                                               = Expression.Constant((double)float.MaxValue),
        ["Double.BigValue"]                                                                     = Expression.Constant(1.7976931348623157e308),
        ["Double.SmallValue"]                                                                   = Expression.Constant(-1.7976931348623157e+308),
        ["Double.Nan"]                                                                          = Expression.Constant(double.NaN),
        ["Double.NegativeInfinity"]                                                             = Expression.Constant(double.NegativeInfinity),
        ["Double.PositiveInfinity"]                                                             = Expression.Constant(double.PositiveInfinity),
        ["Double.NegativeZero"]                                                                 = Expression.Constant(double.NegativeZero),
        ["Double.Zero"]                                                                         = Expression.Constant(0.0),
        ["Double.Epsilon"]                                                                      = Expression.Constant(double.Epsilon),
        ["Double.PI"]                                                                           = Expression.Constant(double.Pi),
        ["Double.E"]                                                                            = Expression.Constant(double.E),
        ["Double.-2.234567891233658E-123"]                                                      = Expression.Constant(-2.234567891233658E-123),
        ["Double.5.1234567891234567E-123"]                                                      = Expression.Constant(5.1234567891234567E-123),
        ["Double.-3.4028234663852886E+38"]                                                      = Expression.Constant(-3.4028234663852886E+38),
        ["Double.3.4028234663852886E+38"]                                                       = Expression.Constant(3.4028234663852886E+38),
        // float
        ["Float.MinValue"]                                                                      = Expression.Constant(float.MinValue),
        ["Float.MaxValue"]                                                                      = Expression.Constant(float.MaxValue),
        ["Float.Nan"]                                                                           = Expression.Constant(float.NaN),
        ["Float.NegativeInfinity"]                                                              = Expression.Constant(float.NegativeInfinity),
        ["Float.PositiveInfinity"]                                                              = Expression.Constant(float.PositiveInfinity),
        ["Float.Epsilon"]                                                                       = Expression.Constant(float.Epsilon),
        ["Float.NegativeZero"]                                                                  = Expression.Constant(float.NegativeZero),
        ["Float.Zero"]                                                                          = Expression.Constant(0.0F),
        ["Float.-2.234568E-23F"]                                                                = Expression.Constant(-2.234568E-23F),
        ["Float.5.5123453E-34F"]                                                                = Expression.Constant(5.5123453E-34F),
        // Half
        ["Half.3.14"]                                                                          = Expression.Constant((Half)3.14),
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
        ["Int.5"]                                                                               = Expression.Constant(5),
        ["Int.42"]                                                                              = Expression.Constant(42),
        ["Int.Min"]                                                                             = Expression.Constant(int.MinValue),
        ["Int.Max"]                                                                             = Expression.Constant(int.MaxValue),
        // IntPtr
        ["IntPtr.5"]                                                                            = Expression.Constant((IntPtr)5),
        ["IntPtr.23"]                                                                           = Expression.Constant((IntPtr)23),
        ["IntPtr.MinValue"]                                                                     = Expression.Constant(IntPtr.MinValue),
        ["IntPtr.MaxValue"]                                                                     = Expression.Constant(IntPtr.MaxValue),
        // long
        ["Long.0"]                                                                              = Expression.Constant(0L),
        ["Long.5L"]                                                                             = Expression.Constant(5L),
        ["Long.Min"]                                                                            = Expression.Constant(long.MinValue),
        ["Long.Max"]                                                                            = Expression.Constant(long.MaxValue),
        ["Long.IntMin"]                                                                         = Expression.Constant(MinJsonInteger),
        ["Long.IntMax"]                                                                         = Expression.Constant(MaxJsonInteger),
        ["Long.IntMin-1"]                                                                       = Expression.Constant(MinJsonInteger-1),
        ["Long.IntMax+1"]                                                                       = Expression.Constant(MaxJsonInteger+1),
        ["Long.IntMin+1"]                                                                       = Expression.Constant(MinJsonInteger+1),
        ["Long.IntMax-1"]                                                                       = Expression.Constant(MaxJsonInteger-1),
        // sbyte
        ["Sbyte.5"]                                                                             = Expression.Constant((sbyte)5),
        ["Sbyte.-5"]                                                                            = Expression.Constant((sbyte)-5),
        ["Sbyte.MinValue"]                                                                      = Expression.Constant(sbyte.MinValue),
        ["Sbyte.MaxValue"]                                                                      = Expression.Constant(sbyte.MaxValue),
        // short
        ["Short.32000"]                                                                         = Expression.Constant((short)32000),
        ["Short.MinValue"]                                                                      = Expression.Constant(short.MinValue),
        ["Short.MaxValue"]                                                                      = Expression.Constant(short.MaxValue),
        // uint
        ["Uint.0"]                                                                              = Expression.Constant((uint)0),
        ["Uint.5"]                                                                              = Expression.Constant((uint)5),
        ["Uint.42"]                                                                             = Expression.Constant((uint)42),
        ["Uint.Min"]                                                                            = Expression.Constant(uint.MinValue),
        ["Uint.Max"]                                                                            = Expression.Constant(uint.MaxValue),
        // UIntPtr
        ["UnsignedIntPtr.5"]                                                                    = Expression.Constant((UIntPtr)5),
        ["UnsignedIntPtr.42"]                                                                   = Expression.Constant((UIntPtr)42),
        ["UnsignedIntPtr.MinValue"]                                                             = Expression.Constant(UIntPtr.MinValue),
        ["UnsignedIntPtr.MaxValue"]                                                             = Expression.Constant(UIntPtr.MaxValue),
        // ulong
        ["Ulong.0"]                                                                             = Expression.Constant(0UL),
        ["Ulong.5"]                                                                             = Expression.Constant((ulong)5),
        ["Ulong.Min"]                                                                           = Expression.Constant(ulong.MinValue),
        ["Ulong.Max"]                                                                           = Expression.Constant(ulong.MaxValue),
        ["Ulong.IntMax-1"]                                                                      = Expression.Constant((ulong)MaxJsonInteger-1),
        ["Ulong.IntMax"]                                                                        = Expression.Constant((ulong)MaxJsonInteger),
        ["Ulong.IntMax+1"]                                                                      = Expression.Constant((ulong)MaxJsonInteger+1),
        // ushort
        ["Ushort.5"]                                                                            = Expression.Constant((ushort)5),
        ["Ushort.443"]                                                                          = Expression.Constant((ushort)443),
        ["Ushort.MinValue"]                                                                     = Expression.Constant(ushort.MinValue),
        ["Ushort.MaxValue"]                                                                     = Expression.Constant(ushort.MaxValue),
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
        ["TimeSpan.MinValue"]                                                                   = Expression.Constant(new TimeSpan(TimeSpan.MinValue.Days, TimeSpan.MinValue.Hours, TimeSpan.MinValue.Minutes, TimeSpan.MinValue.Seconds)),
        ["TimeSpan.MaxValue"]                                                                   = Expression.Constant(new TimeSpan(TimeSpan.MaxValue.Days, TimeSpan.MaxValue.Hours, TimeSpan.MaxValue.Minutes, TimeSpan.MaxValue.Seconds)),
        ["TimeSpan.Zero"]                                                                       = Expression.Constant(TimeSpan.Zero),
        ["TimeSpan(3, 4, 15, 32, 123)"]                                                         = Expression.Constant(new TimeSpan(3, 4, 15, 32)),
        ["TimeSpan(-3, 4, 15, 32, 123)"]                                                        = Expression.Constant(new TimeSpan(3, 4, 15, 32).Negate()),
        // DBNull
        ["DBNull.Value"]                                                                        = Expression.Constant(DBNull.Value),
        // decimal
        ["Decimal.Zero"]                                                                        = Expression.Constant(decimal.Zero),
        ["Decimal.MinusOne"]                                                                    = Expression.Constant(decimal.MinusOne),
        ["Decimal.One"]                                                                         = Expression.Constant(decimal.One),
        ["Decimal.MinValue"]                                                                    = Expression.Constant(decimal.MinValue),
        ["Decimal.MaxValue"]                                                                    = Expression.Constant(decimal.MaxValue),
        ["5.5M"]                                                                                = Expression.Constant(5.5M),
        // GUID
        ["Guid.Empty"]                                                                          = Expression.Constant(Guid.Empty),
        ["Guid(\"00112233-4455-6677-8899-aabbccddeeff\")"]                                      = Expression.Constant(new Guid("00112233-4455-6677-8899-aabbccddeeff")),
        // string
        ["String.Empty"]                                                                        = Expression.Constant(string.Empty),
        ["String.null"]                                                                         = Expression.Constant(null, typeof(string)),
        ["String.abrah-cadabrah"]                                                               = Expression.Constant("abrah-cadabrah"),
        ["String.ала-бала"]                                                                     = Expression.Constant("ала-бала"),
        // Uri
        ["Uri(\"http://www.delinea.com\")"]                                                     = Expression.Constant(new Uri("http://www.delinea.com")),
        // enum
        ["EnumFlagsTest.One | EnumFlagsTest.Three"]                                             = Expression.Constant(EnumFlagsTest.One | EnumFlagsTest.Three),
        ["EnumTest.Three"]                                                                      = Expression.Constant(EnumTest.Three),
        // nullable primitive
        ["Nullable.int.5"]                                                                      = Expression.Constant(5, typeof(int?)),
        ["Nullable.int.null"]                                                                   = Expression.Constant(null, typeof(int?)),
        ["Nullable.long.5L"]                                                                    = Expression.Constant(5L, typeof(long?)),
        ["Nullable.long.null"]                                                                  = Expression.Constant(null, typeof(long?)),
        ["Nullable.long.long.Min"]                                                              = Expression.Constant(long.MinValue, typeof(long?)),
        ["Nullable.long.long.Max"]                                                              = Expression.Constant(long.MaxValue, typeof(long?)),
        ["Nullable.long.long.IntMin"]                                                           = Expression.Constant(MinJsonInteger, typeof(long?)),
        ["Nullable.long.long.IntMax"]                                                           = Expression.Constant(MaxJsonInteger, typeof(long?)),
        ["Nullable.long.long.IntMin-1"]                                                         = Expression.Constant(MinJsonInteger-1, typeof(long?)),
        ["Nullable.long.long.IntMax+1"]                                                         = Expression.Constant(MaxJsonInteger+1, typeof(long?)),
        ["Nullable.long.long.IntMin+1"]                                                         = Expression.Constant(MinJsonInteger+1, typeof(long?)),
        ["Nullable.long.long.IntMax-1"]                                                         = Expression.Constant(MaxJsonInteger-1, typeof(long?)),
        // objects
        ["Object.null"]                                                                         = Expression.Constant(null),
        ["Object()"]                                                                            = Expression.Constant(new object()),
        ["Object1()"]                                                                           = Expression.Constant(new Object1(), typeof(Object1)),
        ["Object1.null"]                                                                        = Expression.Constant(null, typeof(Object1)),
        ["ClassDataContract1()"]                                                                = Expression.Constant(new ClassDataContract1()),
        ["ClassDataContract2()"]                                                                = Expression.Constant(new ClassDataContract2(1, "two", 3M), typeof(ClassDataContract1)),
        ["ClassSerializable1()"]                                                                = Expression.Constant(new ClassSerializable1()),
        // structs
        ["StructDataContract1.null"]                                                            = Expression.Constant(null, typeof(StructDataContract1?)),
        ["StructDataContract1() { IntProperty = 7, StringProperty = \"vm\" }"]                  = Expression.Constant(new StructDataContract1() { IntProperty = 7, StringProperty = "vm" }, typeof(StructDataContract1)),
        ["StructDataContract1()"]                                                               = Expression.Constant(new StructDataContract1()),
        ["StructDataContract1?() { IntProperty = 7, StringProperty = \"vm\" }"]                 = Expression.Constant((StructDataContract1?)new StructDataContract1() { IntProperty = 7, StringProperty = "vm" }, typeof(StructDataContract1?)),
        ["(StructSerializable1?)null"]                                                          = Expression.Constant(null, typeof(StructSerializable1?)),
        ["StructSerializable1() { IntProperty = 7, StringProperty = \"vm\" }"]                  = Expression.Constant(new StructSerializable1() { IntProperty = 7, StringProperty = "vm" }, typeof(StructSerializable1)),
        ["StructSerializable1()"]                                                               = Expression.Constant(new StructSerializable1()),
        // anonymous
        ["Anonymous"]                                                                           = Expression.Constant(
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
                                                                                                                            TimeSpanProperty = new TimeSpan(1, 2, 3, 4),
                                                                                                                        }),
        // byte sequences
        ["Bytes.(byte[])null"]                                                                  = Expression.Constant(null, typeof(byte[])),
        ["Bytes.byte[]{}"]                                                                      = Expression.Constant(Array.Empty<byte>()),
        ["Bytes.byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }"]                                       = Expression.Constant(new byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }),
        ["Bytes.Memory<byte>()"]                                                                = Expression.Constant(new Memory<byte>()),
        ["Bytes.Memory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])"]                                = Expression.Constant(new Memory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])),
        ["Bytes.ReadOnlyMemory<byte>()"]                                                        = Expression.Constant(new ReadOnlyMemory<byte>()),
        ["Bytes.ReadOnlyMemory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])"]                        = Expression.Constant(new ReadOnlyMemory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])),
        ["Bytes.ArraySegment<byte>([])"]                                                        = Expression.Constant(new ArraySegment<byte>([])),
        ["Bytes.ArraySegment<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10], 1, 8)"]                     = Expression.Constant(new ArraySegment<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10], 1, 8)),
        // sequences
        ["Ints.(int[])null"]                                                                    = Expression.Constant(null, typeof(int[])),
        ["Ints.int[]{}"]                                                                        = Expression.Constant(Array.Empty<int>()),
        ["Ints.int[]{ 1, 2, 3, 4 }"]                                                            = Expression.Constant(new int[]{ 1, 2, 3, 4 }),
        ["Ints.(int?[])null"]                                                                   = Expression.Constant(null, typeof(int?[])),
        ["Ints.int?[]{}"]                                                                       = Expression.Constant(Array.Empty<int?>()),
        ["Ints.int?[]{ 1, 2, null, null }"]                                                     = Expression.Constant(new int?[]{ 1, 2, null, null }),
        ["Ints.Memory<int>(null)"]                                                              = Expression.Constant(new Memory<int>(null)),
        ["Ints.Memory<int>()"]                                                                  = Expression.Constant(new Memory<int>()),
        ["Ints.Memory<int>([ 1, 2, 3, 4 ])"]                                                    = Expression.Constant(new Memory<int>([ 1, 2, 3, 4 ])),
        ["Ints.(Memory<int>?)null"]                                                             = Expression.Constant(null, typeof(Memory<int>?)),
        ["Ints.(Memory<int>?)([ 1, 2, 3, 4 ])"]                                                 = Expression.Constant(new Memory<int>([ 1, 2, 3, 4 ]), typeof(Memory<int>?)),
        ["Ints.(Memory<int>?)()"]                                                               = Expression.Constant(new Memory<int>(), typeof(Memory<int>?)),
        ["EnumTest?[]{}"]                                                                       = Expression.Constant(Array.Empty<EnumTest?>()),
        ["EnumTest?[]{ EnumTest.One, EnumTest.Two, null, null }"]                               = Expression.Constant(new EnumTest?[]{ EnumTest.One, EnumTest.Two, null, null }),
        ["EnumTest?[].null"]                                                                    = Expression.Constant(null, typeof(EnumTest?[])),
        ["ArraySegment<int>([ 1, 2, 3, 4 ], 1, 2)"]                                             = Expression.Constant(new ArraySegment<int>([ 1, 2, 3, 4 ], 1, 2)),
        ["ArraySegment<int>.null"]                                                              = Expression.Constant(null, typeof(ArraySegment<int>?)),
        ["List<int?>{ 1, 2, null, null }"]                                                      = Expression.Constant(new List<int?>{ 1, 2, null, null }),
        ["List<int?>.null"]                                                                     = Expression.Constant(null, typeof(List<int?>)),
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
        ["Frozen.byte[]{}"]                                                                     = Expression.Constant(Array.Empty<int>().ToFrozenSet()),
        ["Frozen.byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }"]                                      = Expression.Constant(new byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }.ToFrozenSet()),
        ["Frozen.int[]{ 1, 2, 3, 4 }"]                                                          = Expression.Constant(new int[]{ 1, 2, 3, 4 }.ToFrozenSet()),
        ["Frozen.int?[]{ 1, 2, null, null }"]                                                   = Expression.Constant(new int?[]{ 1, 2, null, null }.ToFrozenSet()),
        ["Frozen.decimal[]{ 1, 2, 3, 4 }"]                                                      = Expression.Constant(new decimal[]{ 1, 2, 3, 4 }.ToFrozenSet()),
        ["Frozen.EnumTest?[]{ EnumTest.One, EnumTest.Two, null, null }"]                        = Expression.Constant(new EnumTest?[]{ EnumTest.One, EnumTest.Two, null, null }.ToFrozenSet()),
        ["Frozen.anonymous[]"]                                                                  = Expression.Constant(new object?[] {
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
                                                                                                                            TimeSpanProperty = new TimeSpan(1, 2, 3, 4),
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
                                                                                                                            TimeSpanProperty = new TimeSpan(1, 2, 3, 4),
                                                                                                                            DateTimeOffsetProperty = new DateTimeOffset(new DateTime(2013, 2, 13)),
                                                                                                                        },
                                                                                                                        null
                                                                                                                    }.ToFrozenSet()),
        // tuples
        ["Tuple.(Tuple<int, string>)null"]                                                      = Expression.Constant(null, typeof(Tuple<int, string>)),
        ["Tuple.Tuple<int, string>"]                                                            = Expression.Constant(new Tuple<int, string>(1, "one")),
        ["Tuple.ValueTuple<int, string>"]                                                       = Expression.Constant((1, "one")),
        ["Tuple.Tuple<int, String, StructDataContract1>"]                                       = Expression.Constant(new Tuple<int, string, StructDataContract1>(1, "one", new StructDataContract1(2, "two"))),
        ["Tuple.ValueTuple<int, String, StructDataContract1>"]                                  = Expression.Constant((1, "one", new StructDataContract1(2, "two"))),
        // dictionaries
        ["Dictionary<int, string?>{ [1] = \"one\", [2] = \"two\"..."]                           = Expression.Constant(new Dictionary<int, string?>{ [1] = "one", [2] = "two", [3] = null, [4] = null }),
        ["Dictionary<int, string>{ [1] = \"one\", [2] = \"two\" }"]                             = Expression.Constant(new Dictionary<int, string>{ [1] ="one", [2]="two" }),
        ["Frozen.Dictionary<int, string?>..."]                                                  = Expression.Constant(new Dictionary<int, string?>{ [1] = "one", [2] = "two", [3] = null, [4] = null }.ToFrozenDictionary()),
        ["Frozen.Dictionary<int, string>..."]                                                   = Expression.Constant(new Dictionary<int, string>{ [1] = "one", [2] = "two", [3] = "three", }.ToFrozenDictionary()),
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
