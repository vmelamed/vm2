﻿namespace vm2.ExpressionSerialization.XmlTransform;

using System.Runtime.CompilerServices;

using TransformConstant = Func<object?, Type, XElement>;

/// <summary>
/// Class DataTransform.
/// </summary>
internal class DataTransform(Options? options = default)
{
    Options _options = options ?? new Options();

    #region constant serializers
    /// <summary>
    /// The map of base type constants serializers
    /// </summary>
    static ReadOnlyDictionary<Type, TransformConstant> _constantTransformsDict = new (new Dictionary<Type, TransformConstant>()
    {
        { typeof(bool),             (v, t) => new XElement(ElementNames.Boolean,        XmlConvert.ToString((bool)v!)) },
        { typeof(byte),             (v, t) => new XElement(ElementNames.UnsignedByte,   XmlConvert.ToString((byte)v!)) },
        { typeof(sbyte),            (v, t) => new XElement(ElementNames.Byte,           XmlConvert.ToString((sbyte)v!)) },
        { typeof(short),            (v, t) => new XElement(ElementNames.Short,          XmlConvert.ToString((short)v!)) },
        { typeof(ushort),           (v, t) => new XElement(ElementNames.UnsignedShort,  XmlConvert.ToString((ushort)v!)) },
        { typeof(int),              (v, t) => new XElement(ElementNames.Int,            XmlConvert.ToString((int)v!)) },
        { typeof(uint),             (v, t) => new XElement(ElementNames.UnsignedInt,    XmlConvert.ToString((uint)v!)) },
        { typeof(long),             (v, t) => new XElement(ElementNames.Long,           XmlConvert.ToString((long)v!)) },
        { typeof(ulong),            (v, t) => new XElement(ElementNames.UnsignedLong,   XmlConvert.ToString((ulong)v!)) },
        { typeof(Half),             (v, t) => new XElement(ElementNames.Half,           v!.ToString()) },
        { typeof(float),            (v, t) => new XElement(ElementNames.Float,          XmlConvert.ToString((float)v!)) },
        { typeof(double),           (v, t) => new XElement(ElementNames.Double,         XmlConvert.ToString((double)v!)) },
        { typeof(decimal),          (v, t) => new XElement(ElementNames.Decimal,        XmlConvert.ToString((decimal)v!)) },
        { typeof(string),           (v, t) => new XElement(ElementNames.String,         v!) },
        { typeof(char),             (v, t) => new XElement(ElementNames.Char,           XmlConvert.ToChar(new string((char)v!, 1))) },
        { typeof(Guid),             (v, t) => new XElement(ElementNames.Guid,           XmlConvert.ToString((Guid)v!)) },
        { typeof(DateTime),         (v, t) => new XElement(ElementNames.DateTime,       XmlConvert.ToString((DateTime)v!, XmlDateTimeSerializationMode.RoundtripKind)) },
        { typeof(DateTimeOffset),   (v, t) => new XElement(ElementNames.DateTimeOffset, XmlConvert.ToString((DateTimeOffset)v!, "O")) },
        { typeof(TimeSpan),         (v, t) => new XElement(ElementNames.Duration,       XmlConvert.ToString((TimeSpan)v!)) },
        { typeof(Uri),              (v, t) => new XElement(ElementNames.AnyURI,         ((Uri)v!).ToString()) },
        { typeof(DBNull),           (v, t) => new XElement(ElementNames.DBNull)         },
        { typeof(IntPtr),           (v, t) => new XElement(ElementNames.IntPtr,         XmlConvert.ToString((IntPtr)v!)) },
        { typeof(UIntPtr),          (v, t) => new XElement(ElementNames.UnsignedIntPtr, XmlConvert.ToString((UIntPtr)v!)) },
    });
    static FrozenDictionary<Type, TransformConstant> _constantTransforms = _constantTransformsDict.ToFrozenDictionary();
    #endregion

    public XNode TransformNode(ConstantExpression node) => GetTransform(node.Type)(node.Value, node.Type);

    /// <summary>
    /// Gets the best matching transform function for the type encapsulated in the  for the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    /// A delegate that can serialize a nullable of the specified <paramref name="type"/> into an XML element (<see cref="XElement"/>).
    /// </returns>
    /// <exception cref="SerializationException"></exception>
    public TransformConstant GetTransform(Type type)
    {
        // get the transform from the table, or
        if (_constantTransforms.TryGetValue(type, out var transform))
            return transform;

        // if it is an enum - return the EnumTransform
        if (type.IsEnum)
            return EnumTransform;

        // if it is a nullable - get nullable transform or
        if (type.IsNullable())
            return NullableTransform;

        // if it is an anonymous - get anonymous transform or
        if (type.IsAnonymous())
            return AnonymousTransform;

        if (type.IsByteSequence())
            return ByteSequenceTransform;

        if (type.IsDictionary())
            return DictionaryTransform;

        if (type.IsSequence())
            return SequenceTransform;

        if (type.IsTupleValue())
            return ValueTupleTransform;

        if (type.IsTupleClass())
            return ClassTupleTransform;

        // get general object transform
        return ObjectTransform;
    }

    #region Transforms
    /// <summary>
    /// Transforms enum values.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">Type of the node value.</param>
    XElement EnumTransform(
        object? nodeValue,
        Type nodeType)
    {
        var value = Convert.ChangeType(nodeValue, Enum.GetUnderlyingType(nodeType));
        var baseType = nodeType.GetEnumUnderlyingType();

        return new XElement(
                        ElementNames.Enum,
                        new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)),
                        baseType != typeof(int) ? new XAttribute(AttributeNames.BaseType, Transform.TypeName(baseType)) : null,
                        new XAttribute(AttributeNames.BaseValue, value!.ToString()!),
                        nodeValue!.ToString()
                    );
    }

    /// <summary>
    /// Transforms a nullable nullable.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">Type of the node value.</param>
    XElement NullableTransform(
        object? nodeValue,
        Type nodeType)
    {
        var nullableType    = nodeType;
        var underlyingType  = nullableType.GetGenericArguments()[0];
        var nullable        = nodeValue;
        var isNull          = nullable is null || nullableType.GetProperty("HasValue")?.GetValue(nullable) is false;

        var nullableElement = new XElement(
                                    ElementNames.Nullable,
                                    isNull ? new XAttribute(AttributeNames.Type, Transform.TypeName(underlyingType)) : null,
                                    isNull ? new XAttribute(AttributeNames.Nil, isNull) : null);

        if (isNull)
            return nullableElement;

        var value = nullableType.GetProperty("Value")?.GetValue(nullable)
                        ?? throw new InternalTransformErrorException("'Nullable<T>.HasValue' is true but the value is null.");

        nullableElement.Add(
            _options.TypeComment(underlyingType),
            GetTransform(underlyingType)(value, underlyingType));

        return nullableElement;
    }

    /// <summary>
    /// Transforms an anonymous object.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">Type of the node value.</param>
    XElement AnonymousTransform(
        object? nodeValue,
        Type nodeType)
    {
        var anonymousElement = new XElement(
                                    ElementNames.Anonymous,
                                    new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)));
        var pis = nodeType.GetProperties();

        for (var i = 0; i < pis.Length; i++)
        {
            var pi = pis[i];

            anonymousElement.Add(
                new XElement(
                        ElementNames.Property,
                        new XAttribute(AttributeNames.Name, pi.Name),
                        _options.TypeComment(pi.PropertyType),
                        GetTransform(pi.PropertyType)(pi.GetValue(nodeValue, null), pi.PropertyType)));
        }

        return anonymousElement;
    }

    /// <summary>
    /// Transforms sequences of bytes.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">Type of the node value.</param>
    /// <exception cref="InternalTransformErrorException"></exception>
    XElement ByteSequenceTransform(
        object? nodeValue,
        Type nodeType)
    {
        var sequenceElement = new XElement(
                                    ElementNames.ByteSequence,
                                    new XAttribute(AttributeNames.Type, _options.TransformTypeName(nodeType)),
                                    nodeValue is null ? new XAttribute(AttributeNames.Nil, true) : null
                                );
        ReadOnlySpan<byte> bytes;

        if (nodeType == typeof(byte[]))
        {
            if (nodeValue is null)
                return sequenceElement;

            bytes = ((byte[])nodeValue).AsSpan();
        }
        else
        {
            if (nodeValue is null)
                throw new InternalTransformErrorException("Unexpected non-array byte sequenceElement with null value of type '{nodeType.FullName}'.");

            if (nodeValue is Memory<byte> mb)
                bytes = mb.Span;
            else
            if (nodeValue is ReadOnlyMemory<byte> rmb)
                bytes = rmb.Span;
            else
            if (nodeValue is ArraySegment<byte> asb)
                bytes = asb.AsSpan();
            else
                throw new InternalTransformErrorException($"Unknown byte sequenceElement '{nodeType.FullName}'.");
        }

        sequenceElement.Add(
            new XAttribute(AttributeNames.Length, bytes.Length),
            Convert.ToBase64String(bytes));

        return sequenceElement;
    }

    /// <summary>
    /// Transforms sequences of objects.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">Type of the node value.</param>
    XElement SequenceTransform(
        object? nodeValue,
        Type nodeType)
    {
        var piCount = nodeType.GetProperty("Count") ?? nodeType.GetProperty("Length");
        var length = (int?)piCount?.GetValue(nodeValue);
        var elementType = (nodeType.IsGenericType
                                ? nodeType.GetGenericArguments()[0]
                                : nodeType.GetElementType()) ?? throw new InternalTransformErrorException("Could not find the type of a sequenceElement elements.");

        var collectionElement = new XElement(
                                        ElementNames.Collection,
                                        new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)),
                                        new XAttribute(AttributeNames.ElementType, Transform.TypeName(elementType)),
                                        length.HasValue ? new XAttribute(AttributeNames.Length, length.Value) : null,
                                        nodeValue is null ? new XAttribute(AttributeNames.Nil, true) : null,
                                        _options.TypeComment(elementType)
                                    );

        if (nodeValue is null)
            return collectionElement;

        if (nodeValue is IEnumerable enumerable1)
        {
            foreach (var element in enumerable1)
                collectionElement.Add(
                    GetTransform(elementType)(element, elementType));

            return collectionElement;
        }

        // TODO: figure out how to enumerate Memory<> with reflection, instead of copying the array:
        var array = nodeType.GetMethod("ToArray")?.Invoke(nodeValue, null);

        if (array is IEnumerable enumerable2)
        {
            foreach (var element in enumerable2)
                collectionElement.Add(
                    GetTransform(elementType)(element, elementType));

            return collectionElement;
        }

        throw new InternalTransformErrorException($"Could not find the enumerable for {nodeType.FullName}.");
    }

    /// <summary>
    /// Transforms value tuples.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">Type of the node value.</param>
    XElement ValueTupleTransform(
        object? nodeValue,
        Type nodeType)
    {
        if (nodeValue is null)
            throw new InternalTransformErrorException("Null value for a constant expression node of type 'ValueTuple'.");

        var tupleElement = new XElement(
                                    ElementNames.Tuple,
                                    new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)));
        var types = nodeType.GetGenericArguments();

        if (nodeValue is not ITuple tuple)
            throw new InternalTransformErrorException("The value of type 'ValueTuple' doesn't implement ITuple.");

        for (var i = 0; i < tuple.Length; i++)
        {
            var item = tuple[i];
            var type = types[i];

            tupleElement.Add(
                new XElement(
                        ElementNames.TupleItem,
                        new XAttribute(AttributeNames.Name, $"Item{i + 1}"),
                        item is null ? new XAttribute(AttributeNames.Nil, true) : null,
                        _options.TypeComment(type),
                        GetTransform(type)(item, type)));
        }
        return tupleElement;
    }

    /// <summary>
    /// Transforms class tuples.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">Type of the node value.</param>
    XElement ClassTupleTransform(
        object? nodeValue,
        Type nodeType)
    {
        var tupleElement = new XElement(
                                ElementNames.Tuple,
                                new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)),
                                nodeValue is null ? new XAttribute(AttributeNames.Nil, true) : null);

        if (nodeValue is null)
            return tupleElement;

        var types = nodeType.GetGenericArguments();

        if (nodeValue is not ITuple tuple)
            throw new InternalTransformErrorException("The value of type 'ValueTuple' doesn't implement ITuple.");

        for (var i = 0; i < tuple.Length; i++)
        {
            var type = types[i];
            var item = tuple[i];

            tupleElement.Add(
                new XElement(
                        ElementNames.TupleItem,
                        new XAttribute(AttributeNames.Name, $"Item{i + 1}"),
                        item is null ? new XAttribute(AttributeNames.Nil, true) : null,
                        _options.TypeComment(type),
                        GetTransform(type)(item, type)));
        }

        return tupleElement;
    }

    /// <summary>
    /// Transforms dictionaries.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">Type of the node value.</param>
    XElement DictionaryTransform(
        object? nodeValue,
        Type nodeType)
    {
        var dict = nodeValue as IDictionary;
        var length = dict?.Count;
        var dictElement = new XElement(
                                ElementNames.Dictionary,
                                new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)),
                                length.HasValue ? new XAttribute(AttributeNames.Length, length.Value) : null,
                                nodeValue is null ? new XAttribute(AttributeNames.Nil, true) : null);

        if (nodeValue is null)
            return dictElement;
        if (dict is null)
            throw new InternalTransformErrorException("The value of type 'Dictionary' doesn't implement IDictionary.");

        var kvTypes   = nodeType.GetGenericArguments();
        var keyType   = kvTypes?.Length is 2 ? kvTypes?[0] : null;
        var valueType = kvTypes?.Length is 2 ? kvTypes?[1] : null;

        foreach (DictionaryEntry kv in dict)
        {
            var kType = keyType ?? kv.Key.GetType();
            var vType = valueType ?? kv.Value?.GetType() ?? typeof(object);
            dictElement.Add(
                new XElement(
                    ElementNames.KeyValuePair,
                    GetTransform(kType)(kv.Key, kType),
                    GetTransform(vType)(kv.Value, vType)));
        }

        return dictElement;
    }

    /// <summary>
    /// Transforms objects using <see cref="DataContractSerializer" /> which also works with classes marked with 
    /// <see cref="SerializableAttribute"/> types too.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">Type of the node value.</param>
    XElement ObjectTransform(
        object? nodeValue,
        Type nodeType)
    {
        if (nodeValue is null)
            return new XElement(
                            ElementNames.Object,
                            new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)),
                            new XAttribute(AttributeNames.Nil, nodeValue is null));

        var actualTransform = GetTransform(nodeValue.GetType());

        if (actualTransform != ObjectTransform)
            return actualTransform(nodeValue, nodeValue.GetType());

        var concreteType = nodeValue.GetType();
        var objectElement = new XElement(
                                    ElementNames.Object,
                                    new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)),
                                    nodeType != concreteType ? new XAttribute(AttributeNames.ConcreteType, Transform.TypeName(concreteType)) : null,
                                    nodeValue is null ? new XAttribute(AttributeNames.Nil, true) : null
                                );

        if (nodeValue is null)
            return objectElement;

        var dcSerializer = new DataContractSerializer(nodeValue.GetType(), Type.EmptyTypes);

        using var writer = objectElement.CreateWriter();

        // XML serialize into the element
        dcSerializer.WriteObject(writer, nodeValue);

        return objectElement;
    }
    #endregion
}
