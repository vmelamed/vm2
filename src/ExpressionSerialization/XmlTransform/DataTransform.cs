﻿namespace vm2.XmlExpressionSerialization.XmlTransform;

using System.Collections.ObjectModel;
using System.Xml.Linq;

using vm2.XmlExpressionSerialization.Conventions;
using vm2.XmlExpressionSerialization.Utilities;

using ConstantTransform = Action<ConstantExpression, XContainer>;

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
    static ReadOnlyDictionary<Type, ConstantTransform> _constantTransforms = new (new Dictionary<Type, ConstantTransform>()
    {
        { typeof(bool),             (n, x) => x.Add(new XElement(ElementNames.Boolean,        XmlConvert.ToString((bool)n.Value!))) },
        { typeof(byte),             (n, x) => x.Add(new XElement(ElementNames.UnsignedByte,   XmlConvert.ToString((byte)n.Value!))) },
        { typeof(sbyte),            (n, x) => x.Add(new XElement(ElementNames.Byte,           XmlConvert.ToString((sbyte)n.Value!))) },
        { typeof(short),            (n, x) => x.Add(new XElement(ElementNames.Short,          XmlConvert.ToString((short)n.Value!))) },
        { typeof(ushort),           (n, x) => x.Add(new XElement(ElementNames.UnsignedShort,  XmlConvert.ToString((ushort)n.Value!))) },
        { typeof(int),              (n, x) => x.Add(new XElement(ElementNames.Int,            XmlConvert.ToString((int)n.Value!))) },
        { typeof(uint),             (n, x) => x.Add(new XElement(ElementNames.UnsignedInt,    XmlConvert.ToString((uint)n.Value!))) },
        { typeof(long),             (n, x) => x.Add(new XElement(ElementNames.Long,           XmlConvert.ToString((long)n.Value!))) },
        { typeof(ulong),            (n, x) => x.Add(new XElement(ElementNames.UnsignedLong,   XmlConvert.ToString((ulong)n.Value!))) },
        { typeof(Half),             (n, x) => x.Add(new XElement(ElementNames.Half,           n.Value!.ToString())) },
        { typeof(float),            (n, x) => x.Add(new XElement(ElementNames.Float,          XmlConvert.ToString((float)n.Value!))) },
        { typeof(double),           (n, x) => x.Add(new XElement(ElementNames.Double,         XmlConvert.ToString((double)n.Value!))) },
        { typeof(decimal),          (n, x) => x.Add(new XElement(ElementNames.Decimal,        XmlConvert.ToString((decimal)n.Value!))) },
        { typeof(string),           (n, x) => x.Add(new XElement(ElementNames.String,         n.Value!))},
        { typeof(char),             (n, x) => x.Add(new XElement(ElementNames.Char,           XmlConvert.ToChar(new string((char)n.Value!, 1)))) },
        { typeof(Guid),             (n, x) => x.Add(new XElement(ElementNames.Guid,           XmlConvert.ToString((Guid)n.Value!))) },
        { typeof(DateTime),         (n, x) => x.Add(new XElement(ElementNames.DateTime,       XmlConvert.ToString((DateTime)n.Value!, XmlDateTimeSerializationMode.RoundtripKind))) },
        { typeof(DateTimeOffset),   (n, x) => x.Add(new XElement(ElementNames.DateTimeOffset, XmlConvert.ToString((DateTimeOffset)n.Value!, "O"))) },
        { typeof(TimeSpan),         (n, x) => x.Add(new XElement(ElementNames.Duration,       XmlConvert.ToString((TimeSpan)n.Value!))) },
        { typeof(Uri),              (n, x) => x.Add(new XElement(ElementNames.AnyURI,         ((Uri)n.Value!).ToString())) },
        { typeof(DBNull),           (n, x) => x.Add(new XElement(ElementNames.DBNull))        },
        { typeof(IntPtr),           (n, x) => x.Add(new XElement(ElementNames.IntPtr,         XmlConvert.ToString((IntPtr)n.Value!))) },
        { typeof(UIntPtr),          (n, x) => x.Add(new XElement(ElementNames.UnsignedIntPtr, XmlConvert.ToString((UIntPtr)n.Value!))) },
    });
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
        if (type.IsNullable())
            return NullableTransform;

        // if it is an anonymous - get anonymous transformer or
        if (type.IsAnonymous())
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
        XContainer parent)
    {
        var value = Convert.ChangeType(node.Value, Enum.GetUnderlyingType(node.Type));
        var baseType = node.Type.GetEnumUnderlyingType();

        parent.Add(
            _options.TypeComment(node.Type),
            new XElement(
                    ElementNames.Enum,
                    new XAttribute(AttributeNames.Type, Transform.TypeName(node.Type)),
                    baseType != typeof(int) ? new XAttribute(AttributeNames.BaseType, Transform.TypeName(baseType)) : null,
                    new XAttribute(AttributeNames.BaseValue, value!.ToString()!),
                    node.Value!.ToString()));
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
        XContainer parent)
    {
        Debug.Assert(node.Type.IsGenericType);

        var type = node.Type;

        Debug.Assert(type.GetGenericTypeDefinition() == typeof(Nullable<>));

        var nullableType    = node.Type;
        var underlyingType  = nullableType.GetGenericArguments()[0];
        var nullable        = node.Value;
        var isNull          = nullable is null || nullableType.GetProperty("HasValue")?.GetValue(nullable) is false;

        var nullableElement = new XElement(
                                    ElementNames.Nullable,
                                    isNull ? new XAttribute(AttributeNames.Type, Transform.TypeName(underlyingType)) : null,
                                    isNull ? new XAttribute(AttributeNames.Nil, isNull) : null);

        parent.Add(
            _options.TypeComment(node.Type),
            nullableElement);

        if (isNull)
            return;

        var value = nullableType.GetProperty("Value")?.GetValue(nullable);

        Debug.Assert(value is not null);

        Get(underlyingType)(Expression.Constant(value, underlyingType), nullableElement);
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
        XContainer parent)
    {
        var anonymousElement = new XElement(
                                    ElementNames.Anonymous,
                                    new XAttribute(AttributeNames.Type, Transform.TypeName(node.Type)));

        parent.Add(
            _options.TypeComment(node.Type),
            anonymousElement);

        var props = node.Type.GetProperties();

        for (var i = 0; i < props.Length; i++)
        {
            var curElement = new XElement(
                                    ElementNames.Property,
                                    new XAttribute(AttributeNames.Name, props[i].Name));

            var propValue = props[i].GetValue(node.Value, null);

            Get(props[i].PropertyType)(
                Expression.Constant(propValue, props[i].PropertyType),
                curElement);

            anonymousElement.Add(curElement);
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
        XContainer parent)
    {
        var custom = new XElement(
                                ElementNames.Custom,
                                new XAttribute(AttributeNames.Type, Transform.TypeName(node.Type)));

        parent.Add(
            _options.TypeComment(node.Type),
            custom);

        if (node.Value is null)
            return;

        // create a data contract serializer (works with [Serializable] types too)
        var dcSerializer = new DataContractSerializer(node.Value.GetType(), Type.EmptyTypes);

        using var writer = custom.CreateWriter();
        dcSerializer.WriteObject(writer, node.Value);
    }
    #endregion
}
