namespace vm2.ExpressionSerialization.XmlTransform;

partial class ToXmlDataTransform
{
    static T Is<T>(object? v) where T : struct
        => v is T tv ? tv : throw new InternalTransformErrorException($"Expected {typeof(T).Name} v but got {(v is null ? "null" : v.GetType().Name)}");

    static T? Is<T>(object? v, bool nullable = true) where T : class
        => v is T || (nullable && v is null) ? (T?)v : throw new InternalTransformErrorException($"Expected {typeof(T).Name} v but got {(v is null ? "null" : v.GetType().Name)}");

    static ReadOnlyDictionary<Type, TransformConstant> _constantTransformsDict = new (new Dictionary<Type, TransformConstant>()
    {
        { typeof(bool),             (v, t) => new XElement(ElementNames.Boolean,        XmlConvert.ToString(Is<bool>(v))) },
        { typeof(byte),             (v, t) => new XElement(ElementNames.Byte,           XmlConvert.ToString(Is<byte>(v))) },
        { typeof(char),             (v, t) => new XElement(ElementNames.Char,           XmlConvert.ToString(Is<char>(v))) },
        { typeof(double),           (v, t) => new XElement(ElementNames.Double,         XmlConvert.ToString(Is<double>(v))) },
        { typeof(float),            (v, t) => new XElement(ElementNames.Float,          XmlConvert.ToString(Is<float>(v))) },
        { typeof(int),              (v, t) => new XElement(ElementNames.Int,            XmlConvert.ToString(Is<int>(v))) },
        { typeof(IntPtr),           (v, t) => new XElement(ElementNames.IntPtr,         PtrToXmlString(Is<IntPtr>(v))) },
        { typeof(long),             (v, t) => new XElement(ElementNames.Long,           XmlConvert.ToString(Is<long>(v))) },
        { typeof(sbyte),            (v, t) => new XElement(ElementNames.SignedByte,     XmlConvert.ToString(Is<sbyte>(v))) },
        { typeof(short),            (v, t) => new XElement(ElementNames.Short,          XmlConvert.ToString(Is<short>(v))) },
        { typeof(uint),             (v, t) => new XElement(ElementNames.UnsignedInt,    XmlConvert.ToString(Is<uint>(v))) },
        { typeof(UIntPtr),          (v, t) => new XElement(ElementNames.UnsignedIntPtr, PtrToXmlString(Is<UIntPtr>(v))) },
        { typeof(ulong),            (v, t) => new XElement(ElementNames.UnsignedLong,   XmlConvert.ToString(Is<ulong>(v))) },
        { typeof(ushort),           (v, t) => new XElement(ElementNames.UnsignedShort,  XmlConvert.ToString(Is<ushort>(v))) },

        { typeof(DateTime),         (v, t) => new XElement(ElementNames.DateTime,       XmlConvert.ToString(Is<DateTime>(v), XmlDateTimeSerializationMode.RoundtripKind)) },
        { typeof(DateTimeOffset),   (v, t) => new XElement(ElementNames.DateTimeOffset, XmlConvert.ToString(Is<DateTimeOffset>(v), "O")) },
        { typeof(TimeSpan),         (v, t) => new XElement(ElementNames.Duration,       XmlConvert.ToString(Is<TimeSpan>(v))) },
        { typeof(DBNull),           (v, t) => new XElement(ElementNames.DBNull)         },
        { typeof(decimal),          (v, t) => new XElement(ElementNames.Decimal,        XmlConvert.ToString(Is<decimal>(v))) },
        { typeof(Guid),             (v, t) => new XElement(ElementNames.Guid,           XmlConvert.ToString(Is<Guid>(v))) },
        { typeof(Half),             (v, t) => new XElement(ElementNames.Half,           XmlConvert.ToString((double)Is<Half>(v))) },
        { typeof(string),           (v, t) => new XElement(ElementNames.String,         (object?)Is<string>(v) ?? new XAttribute(AttributeNames.Nil, true)) },
        { typeof(Uri),              (v, t) => new XElement(ElementNames.Uri,            (object?)Is<Uri>(v)?.ToString() ?? new XAttribute(AttributeNames.Nil, true)) },
    });
    static FrozenDictionary<Type, TransformConstant> _constantTransforms = _constantTransformsDict.ToFrozenDictionary();

#pragma warning disable IDE0049 // Simplify Names
    static string PtrToXmlString(IntPtr v)
        => Environment.Is64BitProcess
                ? XmlConvert.ToString(checked((Int64)v))
                : XmlConvert.ToString(checked((Int32)v));

    static string PtrToXmlString(UIntPtr v)
        => Environment.Is64BitProcess
                ? XmlConvert.ToString(checked((UInt64)v))
                : XmlConvert.ToString(checked((UInt32)v));
#pragma warning restore IDE0049 // Simplify Names
}
