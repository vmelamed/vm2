namespace vm2.ExpressionSerialization.XmlTests.ToFromXmlTests;

public partial class ConstantTests
{
    public static readonly TheoryData<string, string, string> ConstantsData = new ()
    {
        // primitive:
        // bool
        { TestLine(), "false",                                                                  "Bool.False.xml" },
        { TestLine(), "true",                                                                   "Bool.True.xml" },
        // byte
        { TestLine(), "(byte)5",                                                                "Byte.xml" },
        // char
        { TestLine(), "'V'",                                                                    "Char.xml" },
        // double
        //{ TestLine(), "double.MinValue",                                                        "Double.MinValue.xml" },
        //{ TestLine(), "double.MaxValue",                                                        "Double.MaxValue.xml" },
        //{ TestLine(), "double.Nan",                                                             "Double.Nan.xml" },
        //{ TestLine(), "double.NegativeInfinity",                                                "Double.NegativeInfinity.xml" },
        //{ TestLine(), "double.PositiveInfinity",                                                "Double.PositiveInfinity.xml" },
        { TestLine(), "double.NegativeZero",                                                    "Double.NegativeZero.xml" },
        { TestLine(), "double.Zero",                                                            "Double.Zero.xml" },
        { TestLine(), "double.Epsilon",                                                         "Double.Epsilon.xml" },
        { TestLine(), "double.PI",                                                              "Double.Pi.xml" },
        { TestLine(), "double.E",                                                               "Double.E.xml" },
        { TestLine(), "-2.234567891233658E-123",                                                "Double.2.2345.xml" },
        { TestLine(), "5.1234567891234567E-123",                                                "Double.5.12345.xml" },
        // float
        //{ TestLine(), "float.MinValue",                                                         "Float.MinValue.xml" },
        //{ TestLine(), "float.MaxValue",                                                         "Float.MaxValue.xml" },
        //{ TestLine(), "float.Nan",                                                              "Float.Nan.xml" },
        //{ TestLine(), "float.NegativeInfinity",                                                 "Float.NegativeInfinity.xml" },
        //{ TestLine(), "float.PositiveInfinity",                                                 "Float.PositiveInfinity.xml" },
        { TestLine(), "float.Epsilon",                                                          "Float.Epsilon.xml" },
        { TestLine(), "float.NegativeZero",                                                     "Float.NegativeZero.xml" },
        { TestLine(), "float.Zero",                                                             "Float.Zero.xml" },
        { TestLine(), "-2.234568E-23F",                                                         "Float.2.2345.xml" },
        { TestLine(), "5.5123453E-34F",                                                         "Float.5.512345.xml" },
        // int
        { TestLine(), "5",                                                                      "Int.5.xml" },
        { TestLine(), "42",                                                                     "Int.42.xml" },
        { TestLine(), "int.Min",                                                                "Int.Min.xml" },
        { TestLine(), "int.Max",                                                                "Int.Max.xml" },
        // IntPtr
        { TestLine(), "IntPtr5",                                                                "IntPtr.5.xml" },
        { TestLine(), "IntPtr23",                                                               "IntPtr.23.xml" },
        // long
        { TestLine(), "long0",                                                                  "Long.0.xml" },
        { TestLine(), "5L",                                                                     "Long.5.xml" },
        { TestLine(), "long.Min",                                                               "Long.Min.xml" },
        { TestLine(), "long.Max",                                                               "Long.Max.xml" },
        { TestLine(), "long.IntMin",                                                            "Long.IntMin.xml" },
        { TestLine(), "long.IntMax",                                                            "Long.IntMax.xml" },
        { TestLine(), "long.IntMin-1",                                                          "Long.IntMin-1.xml" },
        { TestLine(), "long.IntMax+1",                                                          "Long.IntMax+1.xml" },
        { TestLine(), "long.IntMin+1",                                                          "Long.IntMin+1.xml" },
        { TestLine(), "long.IntMax-1",                                                          "Long.IntMax-1.xml" },
        // sbyte
        { TestLine(), "(sbyte)5",                                                               "SignedByte.5.xml"},
        { TestLine(), "(sbyte)-5",                                                              "SignedByte.-5.xml"},
        // short
        { TestLine(), "(short)32000",                                                           "Short.xml"},
        // uint
        { TestLine(), "(uint)5",                                                                "UnsignedInt.5.xml"},
        { TestLine(), "(uint)42",                                                               "UnsignedInt.42.xml"},
        { TestLine(), "uint.Min",                                                               "UnsignedInt.Min.xml"},
        { TestLine(), "uint.Max",                                                               "UnsignedInt.Max.xml"},
        // UIntPtr
        { TestLine(), "(UnsignedIntPtr)5",                                                      "UnsignedIntPtr.5.xml"},
        { TestLine(), "(UnsignedIntPtr)42",                                                     "UnsignedIntPtr.42.xml"},
        // ulong
        { TestLine(), "ulong0",                                                                 "UnsignedLong.0.xml"},
        { TestLine(), "(ulong)5",                                                               "UnsignedLong.5.xml"},
        { TestLine(), "ulong.Min",                                                              "UnsignedLong.Min.xml"},
        { TestLine(), "ulong.Max",                                                              "UnsignedLong.Max.xml"},
        { TestLine(), "ulong.IntMax-1",                                                         "UnsignedLong.IntMax-1.xml"},
        { TestLine(), "ulong.IntMax",                                                           "UnsignedLong.IntMax.xml"},
        { TestLine(), "ulong.IntMax+1",                                                         "UnsignedLong.IntMax+1.xml"},
        // ushort
        { TestLine(), "(ushort)5",                                                              "UnsignedShort.5.xml"},
        { TestLine(), "(ushort)443",                                                            "UnsignedShort.443.xml"},
        
        // basic:
        // DateTime
        { TestLine(), "DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local)",             "DateTime.xml" },
        { TestLine(), "DateTime(2024, 4, 13, 23, 18, 26, 234)",                                 "DateTime.Local.xml" },
        // DateTimeOffset
        { TestLine(), "DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0))", "DateTimeOffset.xml" },
        // TimeSpan
        { TestLine(), "TimeSpan(3, 4, 15, 32, 123)",                                            "TimeSpan.xml" },
        { TestLine(), "TimeSpan(-3, 4, 15, 32, 123)",                                           "TimeSpan-.xml" },
        // DBNull
        { TestLine(), "DBNull.Value",                                                           "DBNull.xml" },
        // decimal
        { TestLine(), "5.5M",                                                                   "Decimal.xml" },
        // GUID
        { TestLine(), "Guid(\"00112233-4455-6677-8899-aabbccddeeff\")",                         "Guid.xml" },
        // Half
        { TestLine(), "(Half)3.14",                                                             "Half.xml" },
        // string
        { TestLine(), "abrah-cadabrah",                                                         "String.xml" },
        // Uri
        { TestLine(), "Uri(\"http://www.delinea.com\")",                                        "Uri.xml" },
        // enum
        { TestLine(), "EnumFlagsTest.One | EnumFlagsTest.Three",                                "EnumFlags.xml" },
        { TestLine(), "EnumTest.Three",                                                         "Enum.xml" },
        // nullable primitive
        { TestLine(), "(int?)5",                                                                "Int0.5.xml" },
        { TestLine(), "(int?)null",                                                             "Int0.Null.xml" },
        { TestLine(), "(long?)5L",                                                              "Long0.5.xml" },
        { TestLine(), "(long?)null",                                                            "Long0.Null.xml" },
        { TestLine(), "(long?)long.Min",                                                        "Long0.Min.xml" },
        { TestLine(), "(long?)long.Max",                                                        "Long0.Max.xml" },
        { TestLine(), "(long?)long.IntMin",                                                     "Long0.IntMin.xml" },
        { TestLine(), "(long?)long.IntMax",                                                     "Long0.IntMax.xml" },
        { TestLine(), "(long?)long.IntMin-1",                                                   "Long0.IntMin-1.xml" },
        { TestLine(), "(long?)long.IntMax+1",                                                   "Long0.IntMax+1.xml" },
        { TestLine(), "(long?)long.IntMin+1",                                                   "Long0.IntMin+1.xml" },
        { TestLine(), "(long?)long.IntMax-1",                                                   "Long0.IntMax-1.xml" },
        // objects
        { TestLine(), "null",                                                                   "NullObject.xml" },
        { TestLine(), "object()",                                                               "Object.xml" },
        { TestLine(), "Object1()",                                                              "Object1.xml" },
        { TestLine(), "(Object1)null",                                                          "Object1Null.xml" },
        { TestLine(), "ClassDataContract1()",                                                   "ClassDataContract1.xml" },
        { TestLine(), "ClassDataContract2()",                                                   "ClassDataContract2.xml" },
        { TestLine(), "ClassSerializable1()",                                                   "ClassSerializable1.xml" },
        // structs
        { TestLine(), "(StructDataContract1?)null",                                             "NullableStructDataContract1.Null.xml" },
        { TestLine(), "StructDataContract1() { IntProperty = 7, StringProperty = \"vm\" }",     "StructDataContract1-2.xml" },
        { TestLine(), "StructDataContract1()",                                                  "StructDataContract1.xml" },
        { TestLine(), "StructDataContract1?() { IntProperty = 7, StringProperty = \"vm\" }",    "NullableStructDataContract1.xml" },
        { TestLine(), "(StructSerializable1?)null",                                             "NullableStructSerializable1.Null.xml" },
        { TestLine(), "StructSerializable1() { IntProperty = 7, StringProperty = \"vm\" }",     "NullableStructSerializable1.xml" },
        { TestLine(), "StructSerializable1()",                                                  "StructSerializable1.xml" },
        // anonymous
        { TestLine(), "anonymous",                                                              "Anonymous.xml" },
        // byte sequences
        { TestLine(), "byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }",                                "Bytes.Array.xml" },
        { TestLine(), "byte[]{ }",                                                              "Bytes.Array.Empty.xml" },
        { TestLine(), "(byte[])null",                                                           "Bytes.Array.Null.xml" },
        { TestLine(), "Memory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])",                         "Bytes.Memory.xml" },
        { TestLine(), "ReadOnlyMemory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])",                 "Bytes.ReadOnlyMemory.xml" },
        { TestLine(), "ArraySegment<byte>",                                                     "Bytes.ArraySegment.xml" },
        // sequences
        { TestLine(), "int[]{ 1, 2, 3, 4 }",                                                    "ArrayOfInt.xml" },
        { TestLine(), "int[]{}",                                                                "ArrayOfInt.Empty.xml" },
        { TestLine(), "(int[])null",                                                            "ArrayOfInt.Null.xml" },
        { TestLine(), "int?[]{ 1, 2, null, null }",                                             "ArrayOfNullableInt.xml" },
        { TestLine(), "(int?[])null",                                                           "ArrayOfNullableInt.Null.xml" },
        { TestLine(), "Memory<int>([ 1, 2, 3, 4 ])",                                            "IntMemory.xml" },
        { TestLine(), "(Memory<int>)null",                                                      "IntMemory.Null.xml" },
        { TestLine(), "EnumTest?[]{ EnumTest.One, EnumTest.Two, null, null }",                  "ArrayOfNullableEnums.xml" },
        { TestLine(), "(EnumTest?[])null",                                                      "ArrayOfNullableEnums.Null.xml" },
        { TestLine(), "ArraySegment<int>([ 1, 2, 3, 4 ], 1, 2)",                                "IntArraySegment.xml" },
        { TestLine(), "List<int>([1, 2, 3, 4])",                                                "IntList.xml" },
        { TestLine(), "List<int?>{ 1, 2, null, null }",                                         "ListOfNullableInt.xml" },
        { TestLine(), "(List<int?>)null",                                                       "ListOfNullableInt.Null.xml" },
        { TestLine(), "LinkedList<int>([1, 2, 3, 4])",                                          "IntLinkedList.xml" },
        { TestLine(), "Collection<int>([1, 2, 3, 4])",                                          "IntCollection.xml" },
        { TestLine(), "ReadOnlyCollection<int>([1, 2, 3, 4])",                                  "IntReadOnlyCollection.xml" },
        { TestLine(), "ReadOnlyMemory<int>([ 1, 2, 3, 4 ])",                                    "IntReadOnlyMemory.xml" },
        { TestLine(), "HashSet<int>([1, 2, 3, 4])",                                             "IntHashSet.xml" },
        { TestLine(), "SortedSet<int>([1, 2, 3, 4])",                                           "IntSortedSet.xml" },
        { TestLine(), "Queue<int>([1, 2, 3, 4])",                                               "IntQueue.xml" },
        { TestLine(), "Stack<int>([1, 2, 3, 4])",                                               "IntStack.xml" },
        { TestLine(), "BlockingCollection<double>()",                                           "BlockingCollection.xml" },
        { TestLine(), "ConcurrentBag<int>([1, 2, 3, 4])",                                       "IntConcurrentBag.xml" },
        { TestLine(), "ConcurrentQueue<int>([1, 2, 3, 4])",                                     "IntConcurrentQueue.xml" },
        { TestLine(), "ConcurrentStack<int>([1, 2, 3, 4])",                                     "IntConcurrentStack.xml" },
        { TestLine(), "ImmutableArray.Create(1, 2, 3, 4 )",                                     "IntImmutableSet.xml" },
        { TestLine(), "ImmutableHashSet.Create(1, 2, 3, 4 )",                                   "IntImmutableHashSet.xml" },
        { TestLine(), "ImmutableList.Create(1, 2, 3, 4 )",                                      "IntImmutableList.xml" },
        { TestLine(), "ImmutableQueue.Create(1, 2, 3, 4 )",                                     "IntImmutableQueue.xml" },
        { TestLine(), "ImmutableSortedSet.Create(1, 2, 3, 4 )",                                 "IntImmutableSortedSet.xml" },
        { TestLine(), "ImmutableStack.Create(1, 2, 3, 4 )",                                     "IntImmutableStack.xml" },
        { TestLine(), "ClassDataContract1[] { new ClassDataContract1()...",                     "ArrayWithClassDataContract1and2.xml" },
        { TestLine(), "ClassDataContract1[] { new(0, \"vm\"), new(1, \"vm2 vm\"), }",           "ArrayOfClassDataContract1.xml" },
        { TestLine(), "Frozen byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }",                         "Bytes.SetFrozen.xml" },
        { TestLine(), "Frozen int[]{ 1, 2, 3, 4 }",                                             "SetOfIntFrozen.xml" },
        { TestLine(), "Frozen int?[]{ 1, 2, null, null }",                                      "SetOfNullableIntFrozen.xml" },
        { TestLine(), "Frozen decimal[]{ 1, 2, 3, 4 }",                                         "DecimalFrozenSet.xml" },
        { TestLine(), "Frozen EnumTest?[]{ EnumTest.One, EnumTest.Two, null, null }",           "SetOfNullableEnumsFrozen.xml" },
        { TestLine(), "Frozen anonymous[]",                                                     "AnonymousSetFrozen.xml" },
        // tuples
        { TestLine(), "(Tuple<int, string>)null",                                               "Tuple.Null.xml" },
        { TestLine(), "Tuple<int, string>",                                                     "Tuple.xml" },
        { TestLine(), "ValueTuple<int, string>",                                                "TupleValue.xml" },
        // dictionaries
        { TestLine(), "Dictionary<int, string?>{ [1] = \"one\", [2] = \"two\"...",              "DictionaryIntNullableString.xml" },
        { TestLine(), "Dictionary<int, string>{ [1] = \"one\", [2]=\"two\" }",                  "DictionaryIntString.xml" },
        { TestLine(), "Frozen Dictionary<int, string?>...",                                     "Frozen.DictionaryIntNullableString.xml" },
        { TestLine(), "Frozen Dictionary<int, string>...",                                      "Frozen.DictionaryIntString.son" },
        { TestLine(), "Hashtable(new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" })",    "Hashtable.xml" },
        { TestLine(), "ImmutableDictionary.Create<int,string>().Add(...)",                      "Immutable.DictionaryIntString.xml" },
        { TestLine(), "ImmutableSortedDictionary.Create<int,string>().Add(...)",                "Immutable.SortedDictionaryIntString.xml" },
        { TestLine(), "ReadOnlyDictionary<int, string>...",                                     "ReadOnly.DictionaryIntString.xml" },
        { TestLine(), "SortedDictionary<int, string>{ [1] =\"one\", [2]=\"two\" }",             "SortedDictionaryIntString.xml" },
        { TestLine(), "ConcurrentDictionary<int, string>{ [1] = \"one\", [2]=\"two\" }",        "Concurrent.DictionaryIntString.xml" },

        { TestLine(), "StructDataContract1[]",                                                  "ArrayOfStructDataContract1.xml" },
        { TestLine(), "StructDataContract1?[]",                                                 "ArrayOfNullableStructDataContract1.xml" },
        { TestLine(), "StructSerializable1[]",                                                  "ArrayOfStructSerializable1.xml" },
        { TestLine(), "StructSerializable1?[]",                                                 "ArrayOfNullableStructSerializable1.xml" },
    };
}
