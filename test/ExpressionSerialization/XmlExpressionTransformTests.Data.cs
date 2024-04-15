namespace vm2.ExpressionSerialization.ExpressionSerializationTests;

public partial class XmlExpressionTransformTests
{
    public static readonly TheoryData<string, object, string> ConstantExpressionData = new ()
    {
        { TestLine(), true, "Bool.xml" },
        { TestLine(), (byte)5, "Byte.xml" },
        { TestLine(), 'V', "Char.xml" },
        { TestLine(), new DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local), "DateTime.xml" },
        { TestLine(), new DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0)), "DateTimeOffset.xml" },
        { TestLine(), DBNull.Value, "DBNull.xml" },
        { TestLine(), 5.5M, "Decimal.xml" },
        { TestLine(), 5.1234567891234567E-123, "Double.xml" },
        { TestLine(), EnumTest.Three, "Enum.xml" },
        { TestLine(), EnumFlagsTest.One | EnumFlagsTest.Three, "EnumFlags.xml" },
        { TestLine(), 5.5123453E-34F, "Float.xml" },
        { TestLine(), Guid.Empty, "Guid.xml" },
        { TestLine(), (Half)3.14, "Half.xml" },
        { TestLine(), 5, "Int.xml" },
        { TestLine(), 5L, "Long.xml" },
        { TestLine(), (sbyte)5, "SByte.xml" },
        { TestLine(), (short)5, "Short.xml" },
        { TestLine(), "abrah-cadabrah", "String.xml" },
        { TestLine(), new TimeSpan(3, 4, 15, 32, 123), "TimeSpan.xml" },
        { TestLine(), (uint)5, "UInt.xml" },
        { TestLine(), (ulong)5, "ULong.xml" },
        { TestLine(), new Uri("http://www.delinea.com"), "Uri.xml" },
        { TestLine(), (ushort)5, "UShort.xml" },
        { TestLine(), (UIntPtr)5, "UIntPtr.xml" },
        { TestLine(), (IntPtr)5, "IntPtr.xml" },
        { TestLine(), new int[]{ 1, 2, 3, 4 }, "ArrayOfInt.xml" },
        { TestLine(), new int?[]{ 1, 2, null, null }, "ArrayOfNullableInt.xml" },
        { TestLine(), new List<int>{ 1, 2, 3, 4 }, "ListOfInt.xml" },
        { TestLine(), new List<int?>{ 1, 2, null, null }, "ListOfNullableInt.xml" },
        { TestLine(), new ClassDataContract1(), "ClassDataContract1.xml" },
        { TestLine(), new ClassDataContract1[] { new(0, "vm"), new(1, "vm2 vm"), }, "ArrayOfClassDataContract1.xml" },
        { TestLine(), new ClassSerializable1(), "ClassSerializable1.xml" },
    };
}
