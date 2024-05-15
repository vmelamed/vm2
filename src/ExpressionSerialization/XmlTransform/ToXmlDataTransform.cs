namespace vm2.ExpressionSerialization.XmlTransform;

using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

using TransformConstant = Func<object?, Type, XElement>;

/// <summary>
/// Class ToXmlDataTransform.
/// </summary>
class ToXmlDataTransform(Options? options = default)
{
    Options _options = options ?? new Options();

    static T? Typed<T>(object? value, bool allowNull = false) => value is T || (value is null && allowNull)
                                            ? (T?)value
                                            : throw new InternalTransformErrorException($"Expected {typeof(T).Name} value but got {(value is null ? "null" : value.GetType().Name)}");

    #region constant ToXml transforms
    static ReadOnlyDictionary<Type, TransformConstant> _constantTransformsDict = new (new Dictionary<Type, TransformConstant>()
    {
        { typeof(bool),             (v, t) => new XElement(ElementNames.Boolean,        XmlConvert.ToString(Typed<bool>(v))) },
        { typeof(byte),             (v, t) => new XElement(ElementNames.Byte,           XmlConvert.ToString(Typed<byte>(v))) },
        { typeof(char),             (v, t) => new XElement(ElementNames.Char,           XmlConvert.ToString(Typed<char>(v))) },
        { typeof(double),           (v, t) => new XElement(ElementNames.Double,         XmlConvert.ToString(Typed<double>(v))) },
        { typeof(float),            (v, t) => new XElement(ElementNames.Float,          XmlConvert.ToString(Typed<float>(v))) },
        { typeof(int),              (v, t) => new XElement(ElementNames.Int,            XmlConvert.ToString(Typed<int>(v))) },
        { typeof(IntPtr),           (v, t) => new XElement(ElementNames.IntPtr,         XmlConvert.ToString(Typed<IntPtr>(v))) },
        { typeof(long),             (v, t) => new XElement(ElementNames.Long,           XmlConvert.ToString(Typed<long>(v))) },
        { typeof(sbyte),            (v, t) => new XElement(ElementNames.SignedByte,     XmlConvert.ToString(Typed<sbyte>(v))) },
        { typeof(short),            (v, t) => new XElement(ElementNames.Short,          XmlConvert.ToString(Typed<short>(v))) },
        { typeof(uint),             (v, t) => new XElement(ElementNames.UnsignedInt,    XmlConvert.ToString(Typed<uint>(v))) },
        { typeof(UIntPtr),          (v, t) => new XElement(ElementNames.UnsignedIntPtr, XmlConvert.ToString(Typed<UIntPtr>(v))) },
        { typeof(ulong),            (v, t) => new XElement(ElementNames.UnsignedLong,   XmlConvert.ToString(Typed<ulong>(v))) },
        { typeof(ushort),           (v, t) => new XElement(ElementNames.UnsignedShort,  XmlConvert.ToString(Typed<ushort>(v))) },

        { typeof(DateTime),         (v, t) => new XElement(ElementNames.DateTime,       XmlConvert.ToString(Typed<DateTime>(v), XmlDateTimeSerializationMode.RoundtripKind)) },
        { typeof(DateTimeOffset),   (v, t) => new XElement(ElementNames.DateTimeOffset, XmlConvert.ToString(Typed<DateTimeOffset>(v), "O")) },
        { typeof(DBNull),           (v, t) => new XElement(ElementNames.DBNull)         },
        { typeof(decimal),          (v, t) => new XElement(ElementNames.Decimal,        XmlConvert.ToString(Typed<decimal>(v))) },
        { typeof(TimeSpan),         (v, t) => new XElement(ElementNames.Duration,       XmlConvert.ToString(Typed<TimeSpan>(v))) },
        { typeof(Guid),             (v, t) => new XElement(ElementNames.Guid,           XmlConvert.ToString(Typed<Guid>(v))) },
        { typeof(Half),             (v, t) => new XElement(ElementNames.Half,           XmlConvert.ToString((double)Typed<Half>(v))) },
        { typeof(string),           (v, t) => new XElement(ElementNames.String,         (object?)Typed<string?>(v, true) ?? new XAttribute(AttributeNames.Nil, true)) },
        { typeof(Uri),              (v, t) => new XElement(ElementNames.AnyURI,         (object?)Typed<Uri?>(v, true)?.ToString() ?? new XAttribute(AttributeNames.Nil, true)) },
    });
    static FrozenDictionary<Type, TransformConstant> _constantTransforms = _constantTransformsDict.ToFrozenDictionary();
    #endregion

    public XNode TransformNode(ConstantExpression node) => GetTransform(node.Type)(node.Value, node.Type);

    /// <summary>
    /// Gets the best matching transform function for the type encapsulated in the  for the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    /// A delegate that can transform a nullable of the specified <paramref name="type"/> into an XML element (<see cref="XElement"/>).
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
    /// <param name="nodeType">GetType of the node value.</param>
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
    /// <param name="nodeType">GetType of the node value.</param>
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
    /// <param name="nodeType">GetType of the node value.</param>
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
    /// <param name="nodeType">GetType of the node value.</param>
    /// <exception cref="InternalTransformErrorException"></exception>
    XElement ByteSequenceTransform(
        object? nodeValue,
        Type nodeType)
    {
        var sequenceElement = new XElement(
                                    ElementNames.ByteSequence,
                                    new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)),
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

            if (nodeValue is ImmutableArray<byte> iab)
                bytes = iab.AsSpan();
            else
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
    /// <param name="nodeType">GetType of the node value.</param>
    XElement SequenceTransform(
        object? nodeValue,
        Type nodeType)
    {
        try
        {
            var elementType = (nodeType.IsGenericType
                                ? nodeType.GetGenericArguments()[0]
                                : nodeType.GetElementType()) ?? throw new InternalTransformErrorException("Could not find the type of a sequenceElement elements.");

            if (nodeValue is null)
                return new XElement(
                                ElementNames.Collection,
                                new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)),
                                _options.TypeComment(elementType),
                                new XAttribute(AttributeNames.ElementType, Transform.TypeName(elementType)),
                                new XAttribute(AttributeNames.Nil, true)
                            );

            var piCount = nodeType.GetProperty("Count") ?? nodeType.GetProperty("GetLength");
            var length = (int?)piCount?.GetValue(nodeValue);
            var collectionElement = new XElement(
                                        ElementNames.Collection,
                                        new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)),
                                        new XAttribute(AttributeNames.ElementType, Transform.TypeName(elementType)),
                                        length.HasValue ? new XAttribute(AttributeNames.Length, length.Value) : null,
                                        _options.TypeComment(elementType)
                                    );

            if (nodeValue is IEnumerable enumerable1)
            {
                foreach (var element in enumerable1)
                    collectionElement.Add(
                        GetTransform(elementType)(element, elementType));

                return collectionElement;
            }

            // Of all sequences only Memory<> does not implement IEnumerable.
            // TODO: figure out how to enumerate Memory<> with reflection, instead of copying to array:
            Debug.Assert(nodeType.IsMemory());

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
        catch (Exception ex)
        {
            throw new SerializationException($"Could not transform {nodeValue}", ex);
        }
    }

    /// <summary>
    /// Transforms value tuples.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">GetType of the node value.</param>
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
    /// <param name="nodeType">GetType of the node value.</param>
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
    /// <param name="nodeType">GetType of the node value.</param>
    XElement DictionaryTransform(
        object? nodeValue,
        Type nodeType)
    {
        if (nodeValue is null)
            return new XElement(
                                ElementNames.Dictionary,
                                new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)),
                                nodeValue is null ? new XAttribute(AttributeNames.Nil, true) : null);

        if (nodeValue is not IDictionary dict)
            throw new InternalTransformErrorException("The value of type 'Dictionary' doesn't implement IDictionary.");

        var length = dict.Count;
        var dictElement = new XElement(
                                ElementNames.Dictionary,
                                new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)),
                                new XAttribute(AttributeNames.Length, length),
                                nodeValue is null ? new XAttribute(AttributeNames.Nil, true) : null);

        Type kType, vType;

        if (nodeType.IsGenericType)
        {
            var kvTypes   = nodeType.GetGenericArguments();

            if (kvTypes.Length is not 2)
                throw new InternalTransformErrorException("The elements of 'Dictionary' do not have key-type and element-type.");

            kType = kvTypes[0];
            vType = kvTypes[1];
        }
        else
        {
            kType = typeof(object);
            vType = typeof(object);
        }

        foreach (DictionaryEntry kv in dict)
            dictElement.Add(
                new XElement(
                    ElementNames.KeyValuePair,
                    GetTransform(kType)(kv.Key, kType),
                    GetTransform(vType)(kv.Value, vType)));

        return dictElement;
    }

    /// <summary>
    /// Transforms objects using <see cref="DataContractSerializer" /> which also works with classes marked with 
    /// <see cref="SerializableAttribute"/> types too.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">GetType of the node value.</param>
    XElement ObjectTransform(
        object? nodeValue,
        Type nodeType)
    {
        var element = new XElement(ElementNames.Object);

        if (nodeValue is null)
        {
            element.Add(new XAttribute(AttributeNames.Nil, true));
            if (nodeType != typeof(object))
                element.Add(new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)));
            return element;
        }

        var actualType = nodeValue.GetType();

        if (actualType == typeof(object))
            return element;

        var actualTransform = GetTransform(actualType);

        if (actualTransform != ObjectTransform)
            return actualTransform(nodeValue, actualType);

        element.Add(
            new XAttribute(AttributeNames.Type, Transform.TypeName(nodeType)),
            nodeType != actualType ? new XAttribute(AttributeNames.ConcreteType, Transform.TypeName(actualType)) : null
        );

        var dcSerializer = new DataContractSerializer(actualType);
        using var writer = element.CreateWriter();

        // XML serialize into the element
        dcSerializer.WriteObject(writer, nodeValue);

        return element;
    }
    #endregion
}
