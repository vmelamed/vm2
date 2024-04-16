namespace vm2.ExpressionSerialization.XmlTests;

public partial class TransformTests
{
    public static readonly TheoryData<string, object, string> ConstantExpressionData = new ()
    {
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

        { TestLine(), new List<int>{ 1, 2, 3, 4 }, "ListOfInt.xml" },
        { TestLine(), new List<int?>{ 1, 2, null, null }, "ListOfNullableInt.xml" },

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
    };
}
