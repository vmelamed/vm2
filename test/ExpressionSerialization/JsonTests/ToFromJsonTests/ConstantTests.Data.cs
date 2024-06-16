namespace vm2.ExpressionSerialization.JsonTests.ToFromJsonTests;

public partial class ConstantTests
{
    public static readonly TheoryData<string, string, string> ConstantsData = new ()
    {
        // primitive:
        // bool
        { TestLine(), "false",                                                                  "Bool.False.json" },
        { TestLine(), "true",                                                                   "Bool.True.json" },
        // byte
        { TestLine(), "(byte)5",                                                                "Byte.json" },
        // char
        { TestLine(), "'V'",                                                                    "Char.json" },
        // double
        //{ TestLine(), "double.MinValue",                                                        "Double.MinValue.json" },
        //{ TestLine(), "double.MaxValue",                                                        "Double.MaxValue.json" },
        //{ TestLine(), "double.Nan",                                                             "Double.Nan.json" },
        //{ TestLine(), "double.NegativeInfinity",                                                "Double.NegativeInfinity.json" },
        //{ TestLine(), "double.PositiveInfinity",                                                "Double.PositiveInfinity.json" },
        { TestLine(), "double.NegativeZero",                                                    "Double.NegativeZero.json" },
        { TestLine(), "double.Zero",                                                            "Double.Zero.json" },
        { TestLine(), "double.Epsilon",                                                         "Double.Epsilon.json" },
        { TestLine(), "double.PI",                                                              "Double.Pi.json" },
        { TestLine(), "double.E",                                                               "Double.E.json" },
        { TestLine(), "-2.234567891233658E-123",                                                "Double.2.2345.json" },
        { TestLine(), "5.1234567891234567E-123",                                                "Double.5.12345.json" },
        // float
        //{ TestLine(), "float.MinValue",                                                         "Float.MinValue.json" },
        //{ TestLine(), "float.MaxValue",                                                         "Float.MaxValue.json" },
        //{ TestLine(), "float.Nan",                                                              "Float.Nan.json" },
        //{ TestLine(), "float.NegativeInfinity",                                                 "Float.NegativeInfinity.json" },
        //{ TestLine(), "float.PositiveInfinity",                                                 "Float.PositiveInfinity.json" },
        { TestLine(), "float.Epsilon",                                                          "Float.Epsilon.json" },
        { TestLine(), "float.NegativeZero",                                                     "Float.NegativeZero.json" },
        { TestLine(), "float.Zero",                                                             "Float.Zero.json" },
        { TestLine(), "-2.234568E-23F",                                                         "Float.2.2345.json" },
        { TestLine(), "5.5123453E-34F",                                                         "Float.5.512345.json" },
        // int
        { TestLine(), "5",                                                                      "Int.5.json" },
        { TestLine(), "42",                                                                     "Int.42.json" },
        { TestLine(), "int.Min",                                                                "Int.Min.json" },
        { TestLine(), "int.Max",                                                                "Int.Max.json" },
        // IntPtr
        { TestLine(), "IntPtr5",                                                                "IntPtr.5.json" },
        { TestLine(), "IntPtr23",                                                               "IntPtr.23.json" },
        // long
        { TestLine(), "long0",                                                                  "Long.0.json" },
        { TestLine(), "5L",                                                                     "Long.5.json" },
        { TestLine(), "long.Min",                                                               "Long.Min.json" },
        { TestLine(), "long.Max",                                                               "Long.Max.json" },
        { TestLine(), "long.IntMin",                                                            "Long.IntMin.json" },
        { TestLine(), "long.IntMax",                                                            "Long.IntMax.json" },
        { TestLine(), "long.IntMin-1",                                                          "Long.IntMin-1.json" },
        { TestLine(), "long.IntMax+1",                                                          "Long.IntMax+1.json" },
        { TestLine(), "long.IntMin+1",                                                          "Long.IntMin+1.json" },
        { TestLine(), "long.IntMax-1",                                                          "Long.IntMax-1.json" },
        // sbyte
        { TestLine(), "(sbyte)5",                                                               "SignedByte.5.json"},
        { TestLine(), "(sbyte)-5",                                                              "SignedByte.-5.json"},
        // short
        { TestLine(), "(short)32000",                                                           "Short.json"},
        // uint
        { TestLine(), "(uint)5",                                                                "UnsignedInt.5.json"},
        { TestLine(), "(uint)42",                                                               "UnsignedInt.42.json"},
        { TestLine(), "uint.Min",                                                               "UnsignedInt.Min.json"},
        { TestLine(), "uint.Max",                                                               "UnsignedInt.Max.json"},
        // UIntPtr
        { TestLine(), "(UnsignedIntPtr)5",                                                      "UnsignedIntPtr.5.json"},
        { TestLine(), "(UnsignedIntPtr)42",                                                     "UnsignedIntPtr.42.json"},
        // ulong
        { TestLine(), "ulong0",                                                                 "UnsignedLong.0.json"},
        { TestLine(), "(ulong)5",                                                               "UnsignedLong.5.json"},
        { TestLine(), "ulong.Min",                                                              "UnsignedLong.Min.json"},
        { TestLine(), "ulong.Max",                                                              "UnsignedLong.Max.json"},
        { TestLine(), "ulong.IntMax-1",                                                         "UnsignedLong.IntMax-1.json"},
        { TestLine(), "ulong.IntMax",                                                           "UnsignedLong.IntMax.json"},
        { TestLine(), "ulong.IntMax+1",                                                         "UnsignedLong.IntMax+1.json"},
        // ushort
        { TestLine(), "(ushort)5",                                                              "UnsignedShort.5.json"},
        { TestLine(), "(ushort)443",                                                            "UnsignedShort.443.json"},
        
        // basic:
        // DateTime
        { TestLine(), "DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local)",             "DateTime.json" },
        { TestLine(), "DateTime(2024, 4, 13, 23, 18, 26, 234)",                                 "DateTime.Local.json" },
        // DateTimeOffset
        { TestLine(), "DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0))", "DateTimeOffset.json" },
        // TimeSpan
        { TestLine(), "TimeSpan(3, 4, 15, 32, 123)",                                            "TimeSpan.json" },
        { TestLine(), "TimeSpan(-3, 4, 15, 32, 123)",                                           "TimeSpan-.json" },
        // DBNull
        { TestLine(), "DBNull.Value",                                                           "DBNull.json" },
        // decimal
        { TestLine(), "5.5M",                                                                   "Decimal.json" },
        // GUID
        { TestLine(), "Guid(\"00112233-4455-6677-8899-aabbccddeeff\")",                         "Guid.json" },
        // Half
        { TestLine(), "(Half)3.14",                                                             "Half.json" },
        // string
        { TestLine(), "abrah-cadabrah",                                                         "String.json" },
        // Uri
        { TestLine(), "Uri(\"http://www.delinea.com\")",                                        "Uri.json" },
        // enum
        { TestLine(), "EnumFlagsTest.One | EnumFlagsTest.Three",                                "EnumFlags.json" },
        { TestLine(), "EnumTest.Three",                                                         "Enum.json" },
        // nullable primitive
        { TestLine(), "(int?)5",                                                                "Int0.5.json" },
        { TestLine(), "(int?)null",                                                             "Int0.Null.json" },
        { TestLine(), "(long?)5L",                                                              "Long0.5.json" },
        { TestLine(), "(long?)null",                                                            "Long0.Null.json" },
        { TestLine(), "(long?)long.Min",                                                        "Long0.Min.json" },
        { TestLine(), "(long?)long.Max",                                                        "Long0.Max.json" },
        { TestLine(), "(long?)long.IntMin",                                                     "Long0.IntMin.json" },
        { TestLine(), "(long?)long.IntMax",                                                     "Long0.IntMax.json" },
        { TestLine(), "(long?)long.IntMin-1",                                                   "Long0.IntMin-1.json" },
        { TestLine(), "(long?)long.IntMax+1",                                                   "Long0.IntMax+1.json" },
        { TestLine(), "(long?)long.IntMin+1",                                                   "Long0.IntMin+1.json" },
        { TestLine(), "(long?)long.IntMax-1",                                                   "Long0.IntMax-1.json" },
        // objects
        { TestLine(), "null",                                                                   "NullObject.json" },
        { TestLine(), "object()",                                                               "Object.json" },
        { TestLine(), "Object1()",                                                              "Object1.json" },
        { TestLine(), "(Object1)null",                                                          "Object1Null.json" },
        { TestLine(), "ClassDataContract1()",                                                   "ClassDataContract1.json" },
        { TestLine(), "ClassDataContract2()",                                                   "ClassDataContract2.json" },
        { TestLine(), "ClassSerializable1()",                                                   "ClassSerializable1.json" },
        // structs
        { TestLine(), "(StructDataContract1?)null",                                             "NullableStructDataContract1.Null.json" },
        { TestLine(), "StructDataContract1() { IntProperty = 7, StringProperty = \"vm\" }",     "StructDataContract1-2.json" },
        { TestLine(), "StructDataContract1()",                                                  "StructDataContract1.json" },
        { TestLine(), "StructDataContract1?() { IntProperty = 7, StringProperty = \"vm\" }",    "NullableStructDataContract1.json" },
        { TestLine(), "(StructSerializable1?)null",                                             "NullableStructSerializable1.Null.json" },
        { TestLine(), "StructSerializable1() { IntProperty = 7, StringProperty = \"vm\" }",     "NullableStructSerializable1.json" },
        { TestLine(), "StructSerializable1()",                                                  "StructSerializable1.json" },
        // anonymous
        { TestLine(), "anonymous",                                                              "Anonymous.json" },
        // byte sequences
        { TestLine(), "byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }",                                "Bytes.Array.json" },
        { TestLine(), "byte[]{ }",                                                              "Bytes.Array.Empty.json" },
        { TestLine(), "(byte[])null",                                                           "Bytes.Array.Null.json" },
        { TestLine(), "Memory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])",                         "Bytes.Memory.json" },
        { TestLine(), "ReadOnlyMemory<byte>([1, 2, 3, 1, 2, 3, 1, 2, 3, 10 ])",                 "Bytes.ReadOnlyMemory.json" },
        { TestLine(), "ArraySegment<byte>",                                                     "Bytes.ArraySegment.json" },
        // sequences
        { TestLine(), "int[]{ 1, 2, 3, 4 }",                                                    "ArrayOfInt.json" },
        { TestLine(), "int[]{}",                                                                "ArrayOfInt.Empty.json" },
        { TestLine(), "(int[])null",                                                            "ArrayOfInt.Null.json" },
        { TestLine(), "int?[]{ 1, 2, null, null }",                                             "ArrayOfNullableInt.json" },
        { TestLine(), "(int?[])null",                                                           "ArrayOfNullableInt.Null.json" },
        { TestLine(), "Memory<int>([ 1, 2, 3, 4 ])",                                            "IntMemory.json" },
        { TestLine(), "(Memory<int>)null",                                                      "IntMemory.Null.json" },
        { TestLine(), "EnumTest?[]{ EnumTest.One, EnumTest.Two, null, null }",                  "ArrayOfNullableEnums.json" },
        { TestLine(), "(EnumTest?[])null",                                                      "ArrayOfNullableEnums.Null.json" },
        { TestLine(), "ArraySegment<int>([ 1, 2, 3, 4 ], 1, 2)",                                "IntArraySegment.json" },
        { TestLine(), "List<int>([1, 2, 3, 4])",                                                "IntList.json" },
        { TestLine(), "List<int?>{ 1, 2, null, null }",                                         "ListOfNullableInt.json" },
        { TestLine(), "(List<int?>)null",                                                       "ListOfNullableInt.Null.json" },
        { TestLine(), "LinkedList<int>([1, 2, 3, 4])",                                          "IntLinkedList.json" },
        { TestLine(), "Collection<int>([1, 2, 3, 4])",                                          "IntCollection.json" },
        { TestLine(), "ReadOnlyCollection<int>([1, 2, 3, 4])",                                  "IntReadOnlyCollection.json" },
        { TestLine(), "ReadOnlyMemory<int>([ 1, 2, 3, 4 ])",                                    "IntReadOnlyMemory.json" },
        { TestLine(), "HashSet<int>([1, 2, 3, 4])",                                             "IntHashSet.json" },
        { TestLine(), "SortedSet<int>([1, 2, 3, 4])",                                           "IntSortedSet.json" },
        { TestLine(), "Queue<int>([1, 2, 3, 4])",                                               "IntQueue.json" },
        { TestLine(), "Stack<int>([1, 2, 3, 4])",                                               "IntStack.json" },
        { TestLine(), "BlockingCollection<double>()",                                           "BlockingCollection.json" },
        { TestLine(), "ConcurrentBag<int>([1, 2, 3, 4])",                                       "IntConcurrentBag.json" },
        { TestLine(), "ConcurrentQueue<int>([1, 2, 3, 4])",                                     "IntConcurrentQueue.json" },
        { TestLine(), "ConcurrentStack<int>([1, 2, 3, 4])",                                     "IntConcurrentStack.json" },
        { TestLine(), "ImmutableArray.Create(1, 2, 3, 4 )",                                     "IntImmutableSet.json" },
        { TestLine(), "ImmutableHashSet.Create(1, 2, 3, 4 )",                                   "IntImmutableHashSet.json" },
        { TestLine(), "ImmutableList.Create(1, 2, 3, 4 )",                                      "IntImmutableList.json" },
        { TestLine(), "ImmutableQueue.Create(1, 2, 3, 4 )",                                     "IntImmutableQueue.json" },
        { TestLine(), "ImmutableSortedSet.Create(1, 2, 3, 4 )",                                 "IntImmutableSortedSet.json" },
        { TestLine(), "ImmutableStack.Create(1, 2, 3, 4 )",                                     "IntImmutableStack.json" },
        { TestLine(), "ClassDataContract1[] { new ClassDataContract1()...",                     "ArrayWithClassDataContract1and2.json" },
        { TestLine(), "ClassDataContract1[] { new(0, \"vm\"), new(1, \"vm2 vm\"), }",           "ArrayOfClassDataContract1.json" },
        { TestLine(), "Frozen byte[]{ 1, 2, 3, 1, 2, 3, 1, 2, 3, 10 }",                         "Bytes.SetFrozen.json" },
        { TestLine(), "Frozen int[]{ 1, 2, 3, 4 }",                                             "SetOfIntFrozen.json" },
        { TestLine(), "Frozen int?[]{ 1, 2, null, null }",                                      "SetOfNullableIntFrozen.json" },
        { TestLine(), "Frozen decimal[]{ 1, 2, 3, 4 }",                                         "DecimalFrozenSet.json" },
        { TestLine(), "Frozen EnumTest?[]{ EnumTest.One, EnumTest.Two, null, null }",           "SetOfNullableEnumsFrozen.json" },
        { TestLine(), "Frozen anonymous[]",                                                     "AnonymousSetFrozen.json" },
        // tuples
        { TestLine(), "(Tuple<int, string>)null",                                               "Tuple.Null.json" },
        { TestLine(), "Tuple<int, string>",                                                     "Tuple.json" },
        { TestLine(), "ValueTuple<int, string>",                                                "TupleValue.json" },
        // dictionaries
        { TestLine(), "Dictionary<int, string?>{ [1] = \"one\", [2] = \"two\"...",              "DictionaryIntNullableString.json" },
        { TestLine(), "Dictionary<int, string>{ [1] = \"one\", [2] = \"two\" }",                "DictionaryIntString.json" },
        { TestLine(), "Frozen Dictionary<int, string?>...",                                     "Frozen.DictionaryIntNullableString.json" },
        { TestLine(), "Frozen Dictionary<int, string>...",                                      "Frozen.DictionaryIntString.son" },
        { TestLine(), "Hashtable(new Dictionary<int, string>{ [1] =\"one\", [2]=\"two\" })",    "Hashtable.json" },
        { TestLine(), "ImmutableDictionary.Create<int,string>().Add(...)",                      "Immutable.DictionaryIntString.json" },
        { TestLine(), "ImmutableSortedDictionary.Create<int,string>().Add(...)",                "Immutable.SortedDictionaryIntString.json" },
        { TestLine(), "ReadOnlyDictionary<int, string>...",                                     "ReadOnly.DictionaryIntString.json" },
        { TestLine(), "SortedDictionary<int, string>{ [1] =\"one\", [2]=\"two\" }",             "SortedDictionaryIntString.json" },
        { TestLine(), "ConcurrentDictionary<int, string>{ [1] = \"one\", [2]=\"two\" }",        "Concurrent.DictionaryIntString.json" },

        { TestLine(), "StructDataContract1[]",                                                  "ArrayOfStructDataContract1.json" },
        { TestLine(), "StructDataContract1?[]",                                                 "ArrayOfNullableStructDataContract1.json" },
        { TestLine(), "StructSerializable1[]",                                                  "ArrayOfStructSerializable1.json" },
        { TestLine(), "StructSerializable1?[]",                                                 "ArrayOfNullableStructSerializable1.json" },
    };
}
