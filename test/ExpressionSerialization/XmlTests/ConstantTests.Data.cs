namespace vm2.ExpressionSerialization.XmlTests;

public partial class ConstantTests
{
    public static readonly TheoryData<string, object?, string> ConstantExpressionData = new ()
    {
        { TestLine(), new object(), "Object.xml" },
        { TestLine(), null, "NullObject.xml" },
        { TestLine(), true, "Bool.xml" },
        { TestLine(), 'V', "Char.xml" },

        { TestLine(), (byte)5, "Byte.xml" },
        { TestLine(), (sbyte)5, "SByte.xml" },
        { TestLine(), (short)5, "Short.xml" },
        { TestLine(), (ushort)5, "UShort.xml" },
        { TestLine(), 5, "Int.xml" },
        { TestLine(), (uint)5, "UInt.xml" },
        { TestLine(), 5L, "Long.xml" },
        { TestLine(), (ulong)5, "ULong.xml" },

        { TestLine(), (Half)3.14, "Half.xml" },
        { TestLine(), 5.5123453E-34F, "Float.xml" },
        { TestLine(), 5.1234567891234567E-123, "Double.xml" },
        { TestLine(), 5.5M, "Decimal.xml" },

        { TestLine(), (IntPtr)5, "IntPtr.xml" },
        { TestLine(), (UIntPtr)5, "UIntPtr.xml" },

        { TestLine(), EnumTest.Three, "Enum.xml" },
        { TestLine(), EnumFlagsTest.One | EnumFlagsTest.Three, "EnumFlags.xml" },

        { TestLine(), "abrah-cadabrah", "String.xml" },
        { TestLine(), Guid.Empty, "Guid.xml" },
        { TestLine(), new Uri("http://www.delinea.com"), "Uri.xml" },
        { TestLine(), new DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local), "DateTime.xml" },
        { TestLine(), new TimeSpan(3, 4, 15, 32, 123), "TimeSpan.xml" },
        { TestLine(), new DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0)), "DateTimeOffset.xml" },
        { TestLine(), DBNull.Value, "DBNull.xml" },

        { TestLine(), new
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
                        }, "Anonymous.xml" },
        { TestLine(), new Object1(), "Object1.xml" },

        { TestLine(), new int[]{ 1, 2, 3, 4 }, "ArrayOfInt.xml" },
        { TestLine(), new int?[]{ 1, 2, null, null }, "ArrayOfNullableInt.xml" },

        { TestLine(), new ArraySegment<int>([ 1, 2, 3, 4 ], 1, 2), "IntArraySegment.xml" },
        { TestLine(), new Queue<int>([ 1, 2, 3, 4 ]), "IntQueue.xml" },
        { TestLine(), (new decimal[]{ 1, 2, 3, 4 }).ToFrozenSet(), "DecimalFrozenSet.xml" },
        { TestLine(), new Queue<int>([ 1, 2, 3, 4 ]), "IntQueue.xml" },
        { TestLine(), new Stack<int>([ 1, 2, 3, 4 ]), "IntStack.xml" },
        { TestLine(), ImmutableArray.Create(1, 2, 3, 4 ), "IntImmutableArray.xml" },
        { TestLine(), ImmutableHashSet.Create(1, 2, 3, 4 ), "IntImmutableHashSet.xml" },
        { TestLine(), ImmutableList.Create(1, 2, 3, 4 ), "IntImmutableList.xml" },
        { TestLine(), ImmutableQueue.Create(1, 2, 3, 4 ), "IntImmutableQueue.xml" },
        { TestLine(), ImmutableSortedSet.Create(1, 2, 3, 4 ), "IntImmutableSortedSet.xml" },
        { TestLine(), ImmutableStack.Create(1, 2, 3, 4 ), "IntImmutableStack.xml" },
        { TestLine(), new ConcurrentBag<int>([1, 2, 3, 4]), "IntConcurrentBag.xml" },
        { TestLine(), new ConcurrentQueue<int>([1, 2, 3, 4]), "IntConcurrentQueue.xml" },
        { TestLine(), new ConcurrentStack<int>([1, 2, 3, 4]), "IntConcurrentStack.xml" },
        { TestLine(), new Collection<int>([1, 2, 3, 4]), "IntCollection.xml" },
        { TestLine(), new ReadOnlyCollection<int>([1, 2, 3, 4]), "IntReadOnlyCollection.xml" },
        { TestLine(), new HashSet<int>([1, 2, 3, 4]), "IntHashSet.xml" },
        { TestLine(), new LinkedList<int>([1, 2, 3, 4]), "IntLinkedList.xml" },
        { TestLine(), new List<int>([1, 2, 3, 4]), "IntList.xml" },
        { TestLine(), new List<int?>{ 1, 2, null, null }, "ListOfNullableInt.xml" },
        { TestLine(), new Queue<int>([1, 2, 3, 4]), "IntQueue.xml" },
        { TestLine(), new SortedSet<int>([1, 2, 3, 4]), "IntSortedSet.xml" },
        { TestLine(), new Stack<int>([1, 2, 3, 4]), "IntStack.xml" },
        { TestLine(), new Memory<int>([ 1, 2, 3, 4 ]), "IntMemory.xml" },
        { TestLine(), new ReadOnlyMemory<int>([ 1, 2, 3, 4 ]), "IntReadOnlyMemory.xml" },

        { TestLine(), (IntField: 1, StringField: "one"), "ValueTupleIntString.xml" },
        { TestLine(), new Tuple<int, string>(1, "one"), "ClassTupleIntString.xml" },

        { TestLine(), new Hashtable(new Dictionary<int, string>{ [1] ="one", [2]="two" }), "Dictionary.xml" },
        { TestLine(), new Dictionary<int, string>{ [1] ="one", [2]="two" }, "DictionaryIntString.xml" },
        { TestLine(), new Dictionary<int, string>{ [1] ="one", [2]="two" }.ToFrozenDictionary(), "FrozenDictionaryIntString.xml" },
        { TestLine(), new ReadOnlyDictionary<int, string>(new Dictionary<int, string>{ [1] ="one", [2]="two" }), "ReadOnlyDictionaryIntString.xml" },
        { TestLine(), new Dictionary<int, string>{ [1] ="one", [2]="two" }, "DictionaryIntString.xml" },
        { TestLine(), new SortedDictionary<int, string>{ [1] ="one", [2]="two" }, "SortedDictionaryIntString.xml" },
        { TestLine(), ImmutableDictionary.Create<int,string>().Add(1, "one").Add(2, "two"), "ImmutableDictionaryIntString.xml" },
        { TestLine(), ImmutableSortedDictionary.Create<int,string>().Add(1, "one").Add(2, "two"), "ImmutableSortedDictionaryIntString.xml" },
        { TestLine(), new ConcurrentDictionary<int, string>{ [1] ="one", [2]="two" }, "ConcurrentDictionaryIntString.xml" },
        { TestLine(), new Dictionary<int, string>{ [1] = "one", [2] = "two", [3] = "three", }, "IntStringDictionary.xml" },
        { TestLine(), new Dictionary<int, string?>{ [1] = "one", [2] = "two", [3] = null, [4] = null }, "IntNullableStringDictionary.xml" },

        { TestLine(), new ClassDataContract1(), "ClassDataContract1.xml" },
        { TestLine(), new ClassDataContract1[] { new(0, "vm"), new(1, "vm2 vm"), }, "ArrayOfClassDataContract1.xml" },

        { TestLine(), new ClassSerializable1(), "ClassSerializable1.xml" },

        { TestLine(), new StructDataContract1(), "StructDataContract1.xml" },
        { TestLine(), new StructDataContract1[]
                        {
                            new() {
                                IntProperty = 0,
                                StringProperty = "vm",
                            },
                            new() {
                                IntProperty = 1,
                                StringProperty = "vm vm",
                            },
                        }, "ArrayOfStructDataContract1.xml" },
        { TestLine(), new StructDataContract1?[]
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
                        }, "ArrayOfNullableStructDataContract1.xml" },

        { TestLine(), new StructSerializable1(), "StructSerializable1.xml" },
        { TestLine(), new StructSerializable1[]
                        {
                            new () {
                                IntProperty = 0,
                                StringProperty = "vm",
                            },
                            new() {
                                IntProperty = 1,
                                StringProperty = "vm vm",
                            },
                        }, "ArrayOfStructSerializable1.xml" },
        { TestLine(), new StructSerializable1?[]
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
                        }, "ArrayOfNullableStructSerializable1.xml" },

        { TestLine(), new byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }, "ByteArray.xml" },
        { TestLine(), new ArraySegment<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10], 1, 8), "ByteArraySegment.xml" },
        { TestLine(), new Memory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ]), "ByteMemory.xml" },
        { TestLine(), new ReadOnlyMemory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ]), "ByteReadOnlyMemory.xml" },
    };
}
