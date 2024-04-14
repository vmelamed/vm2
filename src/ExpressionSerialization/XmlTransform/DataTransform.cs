namespace vm2.ExpressionSerialization.XmlTransform;

using System.Collections.ObjectModel;

using vm2.ExpressionSerialization.Conventions;
using vm2.ExpressionSerialization.Utilities;

using ConstantTransform = Action<ConstantExpression, XElement>;

/// <summary>
/// Class DataTransform.
/// </summary>
public class DataTransform(Options? options = default)
{
    Options _options = options ?? new Options();

    #region constant serializers
    /// <summary>
    /// The map of base type constants serializers
    /// </summary>
    static Dictionary<Type, ConstantTransform> _constantTransforms = new()
    {
        { typeof(bool),     (n, x) => x.Add(new XElement(XmlElement.Boolean,        XmlConvert.ToString((bool)n.Value!))) },
        { typeof(byte),     (n, x) => x.Add(new XElement(XmlElement.UnsignedByte,   XmlConvert.ToString((byte)n.Value!))) },
        { typeof(sbyte),    (n, x) => x.Add(new XElement(XmlElement.Byte,           XmlConvert.ToString((sbyte)n.Value!))) },
        { typeof(short),    (n, x) => x.Add(new XElement(XmlElement.Short,          XmlConvert.ToString((short)n.Value!))) },
        { typeof(ushort),   (n, x) => x.Add(new XElement(XmlElement.UnsignedShort,  XmlConvert.ToString((ushort)n.Value!))) },
        { typeof(int),      (n, x) => x.Add(new XElement(XmlElement.Int,            XmlConvert.ToString((int)n.Value!))) },
        { typeof(uint),     (n, x) => x.Add(new XElement(XmlElement.UnsignedInt,    XmlConvert.ToString((uint)n.Value!))) },
        { typeof(long),     (n, x) => x.Add(new XElement(XmlElement.Long,           XmlConvert.ToString((long)n.Value!))) },
        { typeof(ulong),    (n, x) => x.Add(new XElement(XmlElement.UnsignedLong,   XmlConvert.ToString((ulong)n.Value!))) },
        { typeof(Half),     (n, x) => x.Add(new XElement(XmlElement.Half,           n.Value!.ToString())) },
        { typeof(float),    (n, x) => x.Add(new XElement(XmlElement.Float,          XmlConvert.ToString((float)n.Value!))) },
        { typeof(double),   (n, x) => x.Add(new XElement(XmlElement.Double,         XmlConvert.ToString((double)n.Value!))) },
        { typeof(decimal),  (n, x) => x.Add(new XElement(XmlElement.Decimal,        XmlConvert.ToString((decimal)n.Value!))) },
        { typeof(string),   (n, x) => x.Add(new XElement(XmlElement.String,         n.Value!))},
        { typeof(char),     (n, x) => x.Add(new XElement(XmlElement.Char,           XmlConvert.ToChar(new string((char)n.Value!, 1)))) },
        { typeof(Guid),     (n, x) => x.Add(new XElement(XmlElement.Guid,           XmlConvert.ToString((Guid)n.Value!))) },
        { typeof(DateTime), (n, x) => x.Add(new XElement(XmlElement.DateTime,       XmlConvert.ToString((DateTime)n.Value!, XmlDateTimeSerializationMode.RoundtripKind))) },
        { typeof(TimeSpan), (n, x) => x.Add(new XElement(XmlElement.Duration,       XmlConvert.ToString((TimeSpan)n.Value!))) },
        { typeof(Uri),      (n, x) => x.Add(new XElement(XmlElement.AnyURI,         ((Uri)n.Value!).ToString())) },
        { typeof(DBNull),   (n, x) => x.Add(new XElement(XmlElement.DBNull))        },
        { typeof(IntPtr),   (n, x) => x.Add(new XElement(XmlElement.IntPtr,         XmlConvert.ToString((IntPtr)n.Value!))) },
        { typeof(UIntPtr),  (n, x) => x.Add(new XElement(XmlElement.UnsignedIntPtr, XmlConvert.ToString((UIntPtr)n.Value!))) },
    };

    static ReadOnlyDictionary<Type, ConstantTransform> _constantTransforms2 = new (_constantTransforms);
    #endregion

    /// <summary>
    /// Gets a transformer for a nullable of the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    /// A delegate that can serialize a nullable of the specified <paramref name="type"/> into an XML element (<see cref="XElement"/>).
    /// </returns>
    /// <exception cref="System.Runtime.Serialization.SerializationException"></exception>
    internal ConstantTransform Get(Type type)
    {
        if (!type.CanXmlTransform())
            throw new SerializationException($"Don't know how to transform type \"{type.AssemblyQualifiedName}\".");

        // get the transformer from the table, or
        if (_constantTransforms.TryGetValue(type, out var transformer))
            return transformer;

        // if it is an enum - return the EnumTransform
        if (type.IsEnum)
            return EnumTransform;

        // if it is a nullable - get nullable transformer or
        if (type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(Nullable<>))
            return NullableTransform;

        // if it is an anonymous - get anonymous transformer or
        if (type.IsGenericType &&
            type.IsAnonymous())
            return AnonymousTransformer;

        // get general object transformer
        return CustomTransform;
    }

    #region Enum transformation
    /// <summary>
    /// Transform enum values.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="parent">The parent.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    void EnumTransform(
        ConstantExpression node,
        XElement parent)
    {
        var value = Convert.ChangeType(node.Value, Enum.GetUnderlyingType(node.Type));

        parent.Add(
                new XElement(
                        XmlElement.Enum,
                        new XAttribute(XmlAttribute.Type, Transform.TypeName(node.Type, _options.TypeNames)),
                        new XAttribute(XmlAttribute.Value, value?.ToString() ?? ""),
                        node.Value?.ToString()));
    }
    #endregion

    #region Nullables transformation
    /// <summary>
    /// Serializes a nullable nullable.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="parent">The parent element where to add the serialized.</param>
    void NullableTransform(
        ConstantExpression node,
        XElement parent)
    {
        Debug.Assert(node.Type.IsGenericType);

        var type = node.Type;

        Debug.Assert(type.GetGenericTypeDefinition() == typeof(Nullable<>));

        var underlyingType  = node.Type.GetGenericArguments()[0];
        var nullable        = node.Value;
        var isNull          = nullable is null;
        var nullableElement = new XElement(
                                    XmlElement.Nullable,
                                    new XAttribute(XmlAttribute.IsNull, isNull),
                                    isNull ? null : new XAttribute(XmlAttribute.Type, Transform.TypeName(underlyingType, _options.TypeNames)));

        parent.Add(nullableElement);

        if (isNull)
            return;

        // get the transformer for the type argument from the table or
        if (_constantTransforms.TryGetValue(underlyingType, out var transform))
        {
            transform(System.Linq.Expressions.Expression.Constant(nullable, underlyingType), nullableElement);
            return;
        }

        // construct custom type element
        nullableElement.Add(new XElement(
                                    XmlElement.Custom,
                                    new XAttribute(XmlAttribute.Type, Transform.TypeName(underlyingType, _options.TypeNames)),
                                    nullable));
    }
    #endregion

    #region Anonymous types transformation
    /// <summary>
    /// Serializes an anonymous object.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="parent">The parent element where to serialize the anonymous object to.</param>
    void AnonymousTransformer(
        ConstantExpression node,
        XElement parent)
    {

        var anonymousElement = new XElement(
                                    XmlElement.Anonymous,
                                    new XAttribute(XmlAttribute.Type, node.Type.AssemblyQualifiedName ?? ""));
        parent.Add(anonymousElement);

        var props = node.Type.GetProperties();

        for (var i = 0; i < props.Length; i++)
        {
            var curElement = new XElement(
                                    XmlElement.Property,
                                    new XAttribute(XmlAttribute.Name, props[i].Name));

            var propValue = props[i].GetValue(anonymousElement, null);

            if (propValue is not null)
            {
                var transform = Get(props[i].PropertyType);

                transform(
                    System.Linq.Expressions.Expression.Constant(propValue, props[i].PropertyType),
                    curElement);

                anonymousElement.Add(curElement);
            }
        }
    }
    #endregion

    #region Custom types (classes and structs) transformation
    /// <summary>
    /// Serializes an object using <see cref="DataContractSerializer" />.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="parent">The parent element where to serialize the object to.</param>
    void CustomTransform(
        ConstantExpression node,
        XElement parent)
    {
        var custom = new XElement(
                                XmlElement.Custom,
                                new XAttribute(XmlAttribute.Type, Transform.TypeName(node.Type, _options.TypeNames)));
        parent.Add(custom);

        if (node.Value is null)
            return;

        // create a data contract serializer (works with [Serializable] types too)
        var dcSerializer = new DataContractSerializer(node.Value.GetType(), Type.EmptyTypes);

        using var writer = custom.CreateWriter();
        dcSerializer.WriteObject(writer, node.Value);
    }
    #endregion
}
