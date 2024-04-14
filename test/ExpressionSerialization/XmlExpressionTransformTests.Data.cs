namespace vm2.ExpressionSerialization.ExpressionSerializationTests;

public enum EnumTest
{
    One,
    Two,
    Three,
};

[Flags]
public enum EnumFlagsTest
{
    One = 1,
    Two = 2,
    Three = 4,
}

public partial class XmlExpressionTransformTests
{
    public static TheoryData<string, object, string> ExpressionsData = new ()
    {
        //{ TestLine(), true, "Bool.xml" },
        //{ TestLine(), (byte)5, "Byte.xml" },
        //{ TestLine(), (char)86, "Char.xml" },
        //{ TestLine(), new DateTime(2024, 4, 13, 23, 18, 26, 234, DateTimeKind.Local), "DateTime.xml" },
        //{ TestLine(), new DateTimeOffset(2024, 4, 13, 23, 18, 26, 234, new TimeSpan(0, -300, 0)), "DateTimeOffset.xml" },
        //{ TestLine(), DBNull.Value, "DBNull.xml" },
        //{ TestLine(), 5.5M, "Decimal.xml" },
        //{ TestLine(), 5.1234567891234567E-123, "Double.xml" },
        //{ TestLine(), EnumTest.Three, "Enum.xml" },
        //{ TestLine(), EnumFlagsTest.One | EnumFlagsTest.Three, "EnumFlags.xml" },
        //{ TestLine(), 5.5123453E-34F, "Float.xml" },
        //{ TestLine(), Guid.Empty, "Guid.xml" },
        //{ TestLine(), (Half)3.14, "Half.xml" },
        //{ TestLine(), 5, "Int.xml" },
        //{ TestLine(), 5L, "Long.xml" },
        //{ TestLine(), (sbyte)5, "SByte.xml" },
        //{ TestLine(), (short)5, "Short.xml" },
        //{ TestLine(), "abrah-cadabrah", "String.xml" },
        //{ TestLine(), new TimeSpan(3, 4, 15, 32, 123), "TimeSpan.xml" },
        //{ TestLine(), (uint)5, "UInt.xml" },
        //{ TestLine(), (ulong)5, "ULong.xml" },
        //{ TestLine(), new Uri("http://www.delinea.com"), "Uri.xml" },
        //{ TestLine(), (ushort)5, "UShort.xml" },
        //{ TestLine(), (UIntPtr)5, "UIntPtr.xml" },
        //{ TestLine(), (IntPtr)5, "IntPtr.xml" },

        { TestLine(), (int?)5, "NullableInt.xml" },
        //{ TestLine(), (long?)5, "NullableLong.xml" },

        //{ TestLine(), Expression.Default(typeof(int)), "DefaultInt.xml" },
        //{ TestLine(), Expression.Default(typeof(int?)), "DefaultNullableInt.xml" },
    };
}
