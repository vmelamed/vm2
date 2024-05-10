namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

public partial class ConstantTests
{
    public static readonly TheoryData<string, string, string> ConstantsData = new ()
    {
        { TestLine(), "null",  "NullObject.xml" },
        { TestLine(), "true",  "Bool.xml" },
        { TestLine(), "false", "False.xml" },
        { TestLine(), "'V'",  "Char.xml" },

        { TestLine(), "(byte)5",  "Byte.xml" },
        { TestLine(), "(sbyte)5",  "SByte.xml" },
        { TestLine(), "(short)5",  "Short.xml" },
        { TestLine(), "(ushort)5",  "UShort.xml" },
        { TestLine(), "5",  "Int.xml" },
        { TestLine(), "(uint)5",  "UInt.xml" },
        { TestLine(), "5L",  "Long.xml" },
        { TestLine(), "(ulong)5",  "ULong.xml" },

        { TestLine(), "5.5123453E-34F",  "Float.xml" },
        { TestLine(), "5.1234567891234567E-123",  "Double.xml" },
        { TestLine(), "5.5M",  "Decimal.xml" },

        { TestLine(), "EnumTest.Three",  "Enum.xml" },
        { TestLine(), "EnumFlagsTest.One | EnumFlagsTest.Three",  "EnumFlags.xml" },

        { TestLine(), "abrah-cadabrah",  "String.xml" },
        { TestLine(), "new DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local)", "DateTime.xml" },
        { TestLine(), "new TimeSpan(3, 4, 15, 32, 123)", "TimeSpan.xml" },
        { TestLine(), "new DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0))", "DateTimeOffset.xml" },
        { TestLine(), "new int[]{ 1, 2, 3, 4 }", "ArrayOfInt.xml" },
        { TestLine(), "new int?[]{ 1, 2, null, null }", "ArrayOfNullableInt.xml" },
        { TestLine(), "new byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }", "ByteArray.xml" },

        { TestLine(), "(int?)5", "NullableInt.xml" },
        { TestLine(), "(int?)null", "NullNullableInt.xml" },
        { TestLine(), "(long?)5L", "NullableLong.xml" },
        { TestLine(), "(long?)null", "NullNullableLong.xml" },
        { TestLine(), "(Object1)null", "Object1Null.xml" },
        { TestLine(), "new StructDataContract1()", "StructDataContract1.xml" },
        { TestLine(), "new StructDataContract1() { IntProperty = 7, StringProperty = \"vm\" }", "StructDataContract1-2.xml" },
        { TestLine(), "new StructDataContract1() { IntProperty = 7, StringProperty = \"vm\" } (nullable)", "NullableStructDataContract1.xml" },
        { TestLine(), "(StructDataContract1)null", "NullNullableStructDataContract1.xml" },
        { TestLine(), "new StructSerializable1() { IntProperty = 7, StringProperty = \"vm\" }", "NullableStructSerializable1.xml" },
        { TestLine(), "(StructSerializable1)null", "NullNullableStructSerializable1.xml" },

        { TestLine(), "new object()", "Object.xml" },
        { TestLine(), "(Half)3.14", "Half.xml" },
        { TestLine(), "(IntPtr)5", "IntPtr.xml" },
        { TestLine(), "(UIntPtr)5", "UIntPtr.xml" },
        { TestLine(), "new Guid(\"00112233-4455-6677-8899-aabbccddeeff\")", "Guid.xml" },
        { TestLine(), "new Uri(\"http://www.delinea.com\")", "Uri.xml" },
        { TestLine(), "DBNull.Value", "DBNull.xml" },
        { TestLine(), "new anonymous", "Anonymous.xml" },
        { TestLine(), "new Object1()", "Object1.xml" },

        { TestLine(), "new ArraySegment<int>([ 1, 2, 3, 4 ], 1, 2)", "IntArraySegment.xml" },
        { TestLine(), "new decimal[]{ 1, 2, 3, 4 }.ToFrozenSet()", "DecimalFrozenSet.xml" },
        { TestLine(), "new Queue<int>([ 1, 2, 3, 4 ])", "IntQueue.xml" },
        { TestLine(), "new Stack<int>([ 1, 2, 3, 4 ])", "IntStack.xml" },
        { TestLine(), "ImmutableArray.Create(1, 2, 3, 4 )", "IntImmutableArray.xml" },
        { TestLine(), "ImmutableHashSet.Create(1, 2, 3, 4 )", "IntImmutableHashSet.xml" },
        { TestLine(), "ImmutableList.Create(1, 2, 3, 4 )", "IntImmutableList.xml" },
        { TestLine(), "ImmutableQueue.Create(1, 2, 3, 4 )", "IntImmutableQueue.xml" },
        { TestLine(), "ImmutableSortedSet.Create(1, 2, 3, 4 )", "IntImmutableSortedSet.xml" },
        { TestLine(), "ImmutableStack.Create(1, 2, 3, 4 )", "IntImmutableStack.xml" },
        { TestLine(), "new ConcurrentBag<int>([1, 2, 3, 4])", "IntConcurrentBag.xml" },
        { TestLine(), "new ConcurrentQueue<int>([1, 2, 3, 4])", "IntConcurrentQueue.xml" },
        { TestLine(), "new ConcurrentStack<int>([1, 2, 3, 4])", "IntConcurrentStack.xml" },
        { TestLine(), "new Collection<int>([1, 2, 3, 4])", "IntCollection.xml" },
        { TestLine(), "new ReadOnlyCollection<int>([1, 2, 3, 4])", "IntReadOnlyCollection.xml" },
        { TestLine(), "new HashSet<int>([1, 2, 3, 4])", "IntHashSet.xml" },
        { TestLine(), "new LinkedList<int>([1, 2, 3, 4])", "IntLinkedList.xml" },
        { TestLine(), "new List<int>([1, 2, 3, 4])", "IntList.xml" },
        { TestLine(), "new List<int?>{ 1, 2, null, null }", "ListOfNullableInt.xml" },
        { TestLine(), "new Queue<int>([1, 2, 3, 4])", "IntQueue.xml" },
        { TestLine(), "new SortedSet<int>([1, 2, 3, 4])", "IntSortedSet.xml" },
        { TestLine(), "new Stack<int>([1, 2, 3, 4])", "IntStack.xml" },
        { TestLine(), "new Memory<int>([ 1, 2, 3, 4 ])", "IntMemory.xml" },
        { TestLine(), "new ReadOnlyMemory<int>([ 1, 2, 3, 4 ])", "IntReadOnlyMemory.xml" },
        { TestLine(), "new BlockingCollection<double>()", "BlockingCollection.xml" },

        { TestLine(), "(IntField: 1, StringField: \"one\")", "ValueTupleIntString.xml" },
        { TestLine(), "new Tuple<int, string>(1, \"one\")", "ClassTupleIntString.xml" },

        { TestLine(), "new int[]{ 1, 2, 3, 4 }.ToFrozenSet()", "IntFrozenSet.xml" },
        { TestLine(), "new Hashtable(new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" })", "Dictionary.xml" },
        { TestLine(), "new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" }", "DictionaryIntString.xml" },
        { TestLine(), "new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" }.ToFrozenDictionary()", "FrozenDictionaryIntString.xml" },
        { TestLine(), "new ReadOnlyDictionary<int, string>(new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" })", "ReadOnlyDictionaryIntString.xml" },
        { TestLine(), "new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" }", "DictionaryIntString.xml" },
        { TestLine(), "new SortedDictionary<int, string>{ [1] =\"one\", [2]=\"two\" }", "SortedDictionaryIntString.xml" },
        { TestLine(), "ImmutableDictionary.Create<int,string>().Add(1, \"one\").Add(2, \"two\")", "ImmutableDictionaryIntString.xml" },
        { TestLine(), "ImmutableSortedDictionary.Create<int,string>().Add(1, \"one\").Add(2, \"two\")", "ImmutableSortedDictionaryIntString.xml" },
        { TestLine(), "new ConcurrentDictionary<int, string>{ [1] = \"one\", [2]=\"two\" }", "ConcurrentDictionaryIntString.xml" },
        { TestLine(), "new Dictionary<int, string>{ [1] = \"one\", [2] = \"two\", [3] = \"three\", }", "IntStringDictionary.xml" },
        { TestLine(), "new Dictionary<int, string?>{ [1] = \"one\", [2] = \"two\", [3] = null, [4] = null }", "IntNullableStringDictionary.xml" },

        { TestLine(), "new ClassDataContract1()", "ClassDataContract1.xml" },
        { TestLine(), "new ClassDataContract1[] { new(0, \"vm\"), new(1, \"vm2 vm\"), }", "ArrayOfClassDataContract1.xml" },

        { TestLine(), "new ClassSerializable1()", "ClassSerializable1.xml" },

        { TestLine(), "new StructDataContract1[]", "ArrayOfStructDataContract1.xml" },
        { TestLine(), "new StructDataContract1?[]", "ArrayOfNullableStructDataContract1.xml" },

        { TestLine(), "new StructSerializable1()", "StructSerializable1.xml" },
        { TestLine(), "new StructSerializable1[]", "ArrayOfStructSerializable1.xml" },
        { TestLine(), "new StructSerializable1?[]", "ArrayOfNullableStructSerializable1.xml" },

        { TestLine(), "new Memory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])", "ByteMemory.xml" },
        { TestLine(), "new ReadOnlyMemory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])", "ByteReadOnlyMemory.xml" },
        { TestLine(), "new object[anonymous].ToFrozenSet()", "AnonymousFrozenArray.xml" },
        { TestLine(), "new ClassDataContract2()", "ClassDataContract2.xml" },
        { TestLine(), "new ClassDataContract1[] { new ClassDataContract1(), new ClassDataContract2(), null }", "ArrayWithClassDataContract1and2.xml" }
    };

    static Dictionary<string, ConstantExpression> _substitutes = new()
    {
        ["true"]                                                                                            = Expression.Constant(true),
        ["false"]                                                                                           = Expression.Constant(false),
        ["'V'"]                                                                                             = Expression.Constant('V'),

        ["(byte)5"]                                                                                         = Expression.Constant((byte)5),
        ["(sbyte)5"]                                                                                        = Expression.Constant((sbyte)5),
        ["(short)5"]                                                                                        = Expression.Constant((short)5),
        ["(ushort)5"]                                                                                       = Expression.Constant((ushort)5),
        ["5"]                                                                                               = Expression.Constant(5),
        ["(uint)5"]                                                                                         = Expression.Constant((uint)5),
        ["5L"]                                                                                              = Expression.Constant(5L),
        ["(ulong)5"]                                                                                        = Expression.Constant((ulong)5),

        ["5.5123453E-34F"]                                                                                  = Expression.Constant(5.5123453E-34F),
        ["5.1234567891234567E-123"]                                                                         = Expression.Constant(5.1234567891234567E-123),
        ["5.5M"]                                                                                            = Expression.Constant(5.5M),

        ["EnumTest.Three"]                                                                                  = Expression.Constant(EnumTest.Three),
        ["EnumFlagsTest.One | EnumFlagsTest.Three"]                                                         = Expression.Constant(EnumFlagsTest.One | EnumFlagsTest.Three),

        ["abrah-cadabrah"]                                                                                  = Expression.Constant("abrah-cadabrah"),

        ["new DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local)"]                                  = Expression.Constant(new DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local)),
        ["new TimeSpan(3, 4, 15, 32, 123)"]                                                                 = Expression.Constant(new TimeSpan(3, 4, 15, 32, 123)),
        ["new DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0))"]                      = Expression.Constant(new DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0))),
        ["new int[]{ 1, 2, 3, 4 }"]                                                                         = Expression.Constant(new int[]{ 1, 2, 3, 4 }),
        ["new int?[]{ 1, 2, null, null }"]                                                                  = Expression.Constant(new int?[]{ 1, 2, null, null }),
        ["new byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }"]                                                     = Expression.Constant(new byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }),

        ["(int?)5"]                                                                                         = Expression.Constant(5, typeof(int?)),
        ["(int?)null"]                                                                                      = Expression.Constant(null, typeof(int?)),
        ["(long?)5L"]                                                                                       = Expression.Constant(5L, typeof(long?)),
        ["(long?)null"]                                                                                     = Expression.Constant(null, typeof(long?)),

        ["ArraySegment<byte>"]                                                                              = Expression.Constant(new ArraySegment<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10], 1, 8)),
        ["(Half)3.14"]                                                                                      = Expression.Constant((Half)3.14),
        ["(IntPtr)5"]                                                                                       = Expression.Constant((IntPtr)5),
        ["(UIntPtr)5"]                                                                                      = Expression.Constant((UIntPtr)5),
        ["new Guid(\"00112233-4455-6677-8899-aabbccddeeff\")"]                                              = Expression.Constant(new Guid("00112233-4455-6677-8899-aabbccddeeff")),
        ["new Uri(\"http://www.delinea.com\")"]                                                             = Expression.Constant(new Uri("http://www.delinea.com")),
        ["DBNull.Value"]                                                                                    = Expression.Constant(DBNull.Value),

        ["new ArraySegment<int>([ 1, 2, 3, 4 ], 1, 2)"]                                                     = Expression.Constant(new ArraySegment<int>([ 1, 2, 3, 4 ], 1, 2)),
        ["new decimal[]{ 1, 2, 3, 4 }.ToFrozenSet()"]                                                       = Expression.Constant(new decimal[]{ 1, 2, 3, 4 }.ToFrozenSet()),
        ["new Queue<int>([ 1, 2, 3, 4 ])"]                                                                  = Expression.Constant(new Queue<int>([ 1, 2, 3, 4 ])),
        ["new Stack<int>([ 1, 2, 3, 4 ])"]                                                                  = Expression.Constant(new Stack<int>([ 1, 2, 3, 4 ])),
        ["ImmutableArray.Create(1, 2, 3, 4 )"]                                                              = Expression.Constant(ImmutableArray.Create(1, 2, 3, 4 )),
        ["ImmutableHashSet.Create(1, 2, 3, 4 )"]                                                            = Expression.Constant(ImmutableHashSet.Create(1, 2, 3, 4 )),
        ["ImmutableList.Create(1, 2, 3, 4 )"]                                                               = Expression.Constant(ImmutableList.Create(1, 2, 3, 4 )),
        ["ImmutableQueue.Create(1, 2, 3, 4 )"]                                                              = Expression.Constant(ImmutableQueue.Create(1, 2, 3, 4 )),
        ["ImmutableSortedSet.Create(1, 2, 3, 4 )"]                                                          = Expression.Constant(ImmutableSortedSet.Create(1, 2, 3, 4 )),
        ["ImmutableStack.Create(1, 2, 3, 4 )"]                                                              = Expression.Constant(ImmutableStack.Create(1, 2, 3, 4 )),
        ["new ConcurrentBag<int>([1, 2, 3, 4])"]                                                            = Expression.Constant(new ConcurrentBag<int>([1, 2, 3, 4])),
        ["new ConcurrentQueue<int>([1, 2, 3, 4])"]                                                          = Expression.Constant(new ConcurrentQueue<int>([1, 2, 3, 4])),
        ["new ConcurrentStack<int>([1, 2, 3, 4])"]                                                          = Expression.Constant(new ConcurrentStack<int>([1, 2, 3, 4])),
        ["new Collection<int>([1, 2, 3, 4])"]                                                               = Expression.Constant(new Collection<int>([1, 2, 3, 4])),
        ["new ReadOnlyCollection<int>([1, 2, 3, 4])"]                                                       = Expression.Constant(new ReadOnlyCollection<int>([1, 2, 3, 4])),
        ["new HashSet<int>([1, 2, 3, 4])"]                                                                  = Expression.Constant(new HashSet<int>([1, 2, 3, 4])),
        ["new LinkedList<int>([1, 2, 3, 4])"]                                                               = Expression.Constant(new LinkedList<int>([1, 2, 3, 4])),
        ["new List<int>([1, 2, 3, 4])"]                                                                     = Expression.Constant(new List<int>([1, 2, 3, 4])),
        ["new List<int?>{ 1, 2, null, null }"]                                                              = Expression.Constant(new List<int?>{ 1, 2, null, null }),
        ["new Queue<int>([1, 2, 3, 4])"]                                                                    = Expression.Constant(new Queue<int>([1, 2, 3, 4])),
        ["new SortedSet<int>([1, 2, 3, 4])"]                                                                = Expression.Constant(new SortedSet<int>([1, 2, 3, 4])),
        ["new Stack<int>([1, 2, 3, 4])"]                                                                    = Expression.Constant(new Stack<int>([1, 2, 3, 4])),
        ["new Memory<int>([ 1, 2, 3, 4 ])"]                                                                 = Expression.Constant(new Memory<int>([ 1, 2, 3, 4 ])),
        ["new ReadOnlyMemory<int>([ 1, 2, 3, 4 ])"]                                                         = Expression.Constant(new ReadOnlyMemory<int>([ 1, 2, 3, 4 ])),
        ["new BlockingCollection<double>()"]                                                                = Expression.Constant(new BlockingCollection<double>() { Math.PI, Math.Tau, Math.E }),
        ["(IntField: 1, StringField: \"one\")"]                                                             = Expression.Constant((IntField: 1, StringField: "one")),
        ["new Tuple<int, string>(1, \"one\")"]                                                              = Expression.Constant(new Tuple<int, string>(1, "one")),

        ["new int[]{ 1, 2, 3, 4 }.ToFrozenSet()"]                                                           = Expression.Constant(new int[]{ 1, 2, 3, 4 }.ToFrozenSet()),
        ["new Hashtable(new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" })"]                         = Expression.Constant(new Hashtable(new Dictionary<int, string>{ [1] ="one", [2]="two" })),
        ["new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" }.ToFrozenDictionary()"]                   = Expression.Constant(new Dictionary<int, string>{ [1] ="one", [2]="two" }.ToFrozenDictionary()),
        ["new ReadOnlyDictionary<int, string>(new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" })"]   = Expression.Constant(new ReadOnlyDictionary<int, string>(new Dictionary<int, string>{ [1] ="one", [2]="two" })),
        ["new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" }"]                                        = Expression.Constant(new Dictionary<int, string>{ [1] ="one", [2]="two" }),
        ["new SortedDictionary<int, string>{ [1] =\"one\", [2]=\"two\" }"]                                  = Expression.Constant(new SortedDictionary<int, string>{ [1] ="one", [2]="two" }),
        ["ImmutableDictionary.Create<int,string>().Add(1, \"one\").Add(2, \"two\")"]                        = Expression.Constant(ImmutableDictionary.Create<int,string>().Add(1, "one").Add(2, "two")),
        ["ImmutableSortedDictionary.Create<int,string>().Add(1, \"one\").Add(2, \"two\")"]                  = Expression.Constant(ImmutableSortedDictionary.Create<int,string>().Add(1, "one").Add(2, "two")),
        ["new ConcurrentDictionary<int, string>{ [1] = \"one\", [2]=\"two\" }"]                             = Expression.Constant(new ConcurrentDictionary<int, string>{ [1] ="one", [2]="two" }),
        ["new Dictionary<int, string>{ [1] = \"one\", [2] = \"two\", [3] = \"three\", }"]                   = Expression.Constant(new Dictionary<int, string>{ [1] = "one", [2] = "two", [3] = "three", }),
        ["new Dictionary<int, string?>{ [1] = \"one\", [2] = \"two\", [3] = null, [4] = null }"]            = Expression.Constant(new Dictionary<int, string?>{ [1] = "one", [2] = "two", [3] = null, [4] = null }),

        ["null"]                                                                                            = Expression.Constant(null),
        ["new object()"]                                                                                    = Expression.Constant(new object()),
        ["new anonymous"]                                                                                   = Expression.Constant(
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
                                                                                                                                    }),

        ["new ClassDataContract1()"]                                                                        = Expression.Constant(new ClassDataContract1()),
        ["new ClassDataContract1[] { new(0, \"vm\"), new(1, \"vm2 vm\"), }"]                                = Expression.Constant(new ClassDataContract1[] { new(0, "vm"), new(1, "vm2 vm"), }),

        ["new ClassSerializable1()"]                                                                        = Expression.Constant(new ClassSerializable1()),

        ["new Object1()"]                                                                                   = Expression.Constant(new Object1(), typeof(Object1)),
        ["(Object1)null"]                                                                                   = Expression.Constant(null, typeof(Object1)),

        ["new StructDataContract1() { IntProperty = 7, StringProperty = \"vm\" }"]                          = Expression.Constant(new StructDataContract1() { IntProperty = 7, StringProperty = "vm" }, typeof(StructDataContract1)),
        ["new StructDataContract1() { IntProperty = 7, StringProperty = \"vm\" } (nullable)"]               = Expression.Constant(new StructDataContract1() { IntProperty = 7, StringProperty = "vm" }, typeof(StructDataContract1?)),
        ["(StructDataContract1)null"]                                                                       = Expression.Constant(null, typeof(StructDataContract1?)),
        ["new StructDataContract1()"]                                                                       = Expression.Constant(new StructDataContract1()),
        ["new StructDataContract1[]"]                                                                       = Expression.Constant(new StructDataContract1[]
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
        ["new StructDataContract1?[]"]                                                                      = Expression.Constant(new StructDataContract1?[]
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

        ["new StructSerializable1()"]                                                                       = Expression.Constant(new StructSerializable1()),
        ["new StructSerializable1() { IntProperty = 7, StringProperty = \"vm\" }"]                          = Expression.Constant(new StructSerializable1() { IntProperty = 7, StringProperty = "vm" }, typeof(StructSerializable1)),
        ["(StructSerializable1)null"]                                                                       = Expression.Constant(null, typeof(StructSerializable1?)),
        ["new StructSerializable1[]"]                                                                       = Expression.Constant(new StructSerializable1[]
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
        ["new StructSerializable1?[]"]                                                                      = Expression.Constant(new StructSerializable1?[]
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

        ["new Memory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])"]                                              = Expression.Constant(new Memory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])),
        ["new ReadOnlyMemory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])"]                                      = Expression.Constant(new ReadOnlyMemory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])),
        ["new object[anonymous].ToFrozenSet()"]                                                             = Expression.Constant(new object?[] {
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
        ["new ClassDataContract2()"]                                                                        = Expression.Constant(new ClassDataContract2(1, "two", 3M), typeof(ClassDataContract1)),
        ["new ClassDataContract1[] { new ClassDataContract1(), new ClassDataContract2(), null }"]           = Expression.Constant(new ClassDataContract1?[] { new(), new ClassDataContract2(), null }),
    };
}
