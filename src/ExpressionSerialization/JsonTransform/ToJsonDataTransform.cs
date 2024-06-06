namespace vm2.ExpressionSerialization.JsonTransform;

using System.Text.Json.Serialization.Metadata;

using TransformConstant = Func<object?, Type, JElement>;

/// <summary>
/// Class ToJsonDataTransform transforms data (Expression constants) to JSON.
/// </summary>
class ToJsonDataTransform(JsonOptions options)
{
    static T? Typed<T>(object? value, bool allowNull = false) => value is T || (value is null && allowNull)
                                            ? (T?)value
                                            : throw new InternalTransformErrorException($"Expected {typeof(T).Name} value but got {(value is null ? "null" : value.GetType().Name)}");

    #region constant ToXml transforms
    static ReadOnlyDictionary<Type, TransformConstant> _constantTransformsDict = new (new Dictionary<Type, TransformConstant>()
    {
        { typeof(bool),             (v, t) => new JElement(Vocabulary.Bool,           JsonValue.Create(Typed<bool>(v))) },
        { typeof(byte),             (v, t) => new JElement(Vocabulary.Byte,           JsonValue.Create(Typed<byte>(v))) },
        { typeof(char),             (v, t) => new JElement(Vocabulary.Char,           JsonValue.Create(Typed<char>(v))) },
        { typeof(double),           (v, t) => new JElement(Vocabulary.Double,         JsonValue.Create(Typed<double>(v))) },
        { typeof(float),            (v, t) => new JElement(Vocabulary.Float,          JsonValue.Create(Typed<float>(v))) },
        { typeof(int),              (v, t) => new JElement(Vocabulary.Int,            JsonValue.Create(Typed<int>(v))) },
        { typeof(IntPtr),           (v, t) => new JElement(Vocabulary.IntPtr,         JsonValue.Create(Typed<IntPtr>(v))) },
        { typeof(long),             (v, t) => new JElement(Vocabulary.Long,           JsonValue.Create(Typed<long>(v))) },
        { typeof(sbyte),            (v, t) => new JElement(Vocabulary.SByte,          JsonValue.Create(Typed<sbyte>(v))) },
        { typeof(short),            (v, t) => new JElement(Vocabulary.Short,          JsonValue.Create(Typed<short>(v))) },
        { typeof(uint),             (v, t) => new JElement(Vocabulary.UInt,           JsonValue.Create(Typed<uint>(v))) },
        { typeof(UIntPtr),          (v, t) => new JElement(Vocabulary.UIntPtr,        JsonValue.Create(Typed<UIntPtr>(v))) },
        { typeof(ulong),            (v, t) => new JElement(Vocabulary.ULong,          JsonValue.Create(Typed<ulong>(v))) },
        { typeof(ushort),           (v, t) => new JElement(Vocabulary.UShort,         JsonValue.Create(Typed<ushort>(v))) },
        { typeof(DateTime),         (v, t) => new JElement(Vocabulary.DateTime,       JsonValue.Create(Typed<DateTime>(v))) },
        { typeof(DateTimeOffset),   (v, t) => new JElement(Vocabulary.DateTimeOffset, JsonValue.Create(Typed<DateTimeOffset>(v))) },
        { typeof(DBNull),           (v, t) => new JElement(Vocabulary.DBNull)         },
        { typeof(decimal),          (v, t) => new JElement(Vocabulary.Decimal,        JsonValue.Create(Typed<decimal>(v))) },
        { typeof(TimeSpan),         (v, t) => new JElement(Vocabulary.TimeSpan,       JsonValue.Create(Typed<TimeSpan>(v))) },  // TODO: ISO 8601 doesn't look good
        { typeof(Guid),             (v, t) => new JElement(Vocabulary.Guid,           JsonValue.Create(Typed<Guid>(v))) },
        { typeof(Half),             (v, t) => new JElement(Vocabulary.Half,           JsonValue.Create(Typed<Half>(v))) },
        { typeof(string),           (v, t) => new JElement(Vocabulary.String,         JsonValue.Create(Typed<string>(v))) },
        { typeof(Uri),              (v, t) => new JElement(Vocabulary.Uri,            JsonValue.Create(Typed<Uri?>(v))) },
    });
    static FrozenDictionary<Type, TransformConstant> _constantTransforms = _constantTransformsDict.ToFrozenDictionary();
    #endregion

    public JElement TransformNode(ConstantExpression node) => GetTransform(node.Type)(node.Value, node.Type);

    /// <summary>
    /// Gets the best matching transform function for the type encapsulated in the  for the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    /// A delegate that can transform a nullable of the specified <paramref name="type"/> into an XML element (<see cref="JElement"/>).
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
    /// <param name="nodeType">GetEType of the node value.</param>
    JElement EnumTransform(
        object? nodeValue,
        Type nodeType)
    {
        var value = Convert.ChangeType(nodeValue, Enum.GetUnderlyingType(nodeType));
        var baseType = nodeType.GetEnumUnderlyingType();

        return new JElement(
                        Vocabulary.Enum,
                        new JElement(Vocabulary.Type, Transform.TypeName(nodeType)),
                        value is not null ? new JElement(Vocabulary.BaseValue, value.ToString()) : null,
                        baseType != typeof(int) ? new JElement(Vocabulary.BaseType, Transform.TypeName(baseType)) : null,
                        nodeValue is not null ? new JElement(Vocabulary.Value, nodeValue.ToString()) : null
                    );
    }

    /// <summary>
    /// Transforms a nullable nullable.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">GetEType of the node value.</param>
    JElement NullableTransform(
        object? nodeValue,
        Type nodeType)
    {
        var nullableType    = nodeType;
        var underlyingType  = nullableType.GetGenericArguments()[0];
        var nullable        = nodeValue;
        var isNull          = nullable is null || nullableType.GetProperty("HasValue")?.GetValue(nullable) is false;

        var nullableElement = new JElement(
                                    Vocabulary.Nullable,
                                    isNull ? new JElement(Vocabulary.Type, Transform.TypeName(underlyingType)) : null,
                                    isNull ? new JElement(Vocabulary.Null, isNull) : null);

        if (isNull)
            return nullableElement;

        var value = nullableType.GetProperty("Value")?.GetValue(nullable)
                        ?? throw new InternalTransformErrorException("'Nullable<T>.HasValue' is true but the value is null.");

        nullableElement.Add(
            options.TypeComment(underlyingType),
            GetTransform(underlyingType)(value, underlyingType));

        return nullableElement;
    }

    /// <summary>
    /// Transforms an anonymous object.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">GetEType of the node value.</param>
    JElement AnonymousTransform(
        object? nodeValue,
        Type nodeType)
    {
        var anonymousElement = new JElement(
                                    Vocabulary.Anonymous,
                                    new JElement(Vocabulary.Type, Transform.TypeName(nodeType)));
        var pis = nodeType.GetProperties();

        for (var i = 0; i < pis.Length; i++)
        {
            var pi = pis[i];

            anonymousElement.Add(
                new JElement(
                        Vocabulary.Property,
                        new JElement(Vocabulary.Name, pi.Name),
                        options.TypeComment(pi.PropertyType),
                        GetTransform(pi.PropertyType)(pi.GetValue(nodeValue, null), pi.PropertyType)));
        }

        return anonymousElement;
    }

    /// <summary>
    /// Transforms sequences of bytes.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">GetEType of the node value.</param>
    /// <exception cref="InternalTransformErrorException"></exception>
    JElement ByteSequenceTransform(
        object? nodeValue,
        Type nodeType)
    {
        var sequenceElement = new JElement(
                                    Vocabulary.ByteSequence,
                                    new JElement(Vocabulary.Type, Transform.TypeName(nodeType)),
                                    nodeValue is null ? new JElement(Vocabulary.Null, true) : null
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
            new JElement(Vocabulary.Length, bytes.Length),
            new JElement(Vocabulary.Value, Convert.ToBase64String(bytes)));

        return sequenceElement;
    }

    /// <summary>
    /// Transforms sequences of objects.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">GetEType of the node value.</param>
    JElement SequenceTransform(
        object? nodeValue,
        Type nodeType)
    {
        try
        {
            var elementType = (nodeType.IsGenericType
                                ? nodeType.GetGenericArguments()[0]
                                : nodeType.GetElementType()) ?? throw new InternalTransformErrorException("Could not find the type of a sequenceElement elements.");

            if (nodeValue is null)
                return new JElement(
                                Vocabulary.Collection,
                                new JElement(Vocabulary.Type, Transform.TypeName(nodeType)),
                                options.TypeComment(elementType),
                                new JElement(Vocabulary.ElementType, Transform.TypeName(elementType)),
                                new JElement(Vocabulary.Null, true)
                            );

            var piCount = nodeType.GetProperty("Count") ?? nodeType.GetProperty("GetLength");
            var length = (int?)piCount?.GetValue(nodeValue);
            var collectionElement = new JElement(
                                        Vocabulary.Collection,
                                        new JElement(Vocabulary.Type, Transform.TypeName(nodeType)),
                                        new JElement(Vocabulary.ElementType, Transform.TypeName(elementType)),
                                        length.HasValue ? new JElement(Vocabulary.Length, length.Value) : null,
                                        options.TypeComment(elementType)
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
    /// <param name="nodeType">GetEType of the node value.</param>
    JElement ValueTupleTransform(
        object? nodeValue,
        Type nodeType)
    {
        if (nodeValue is null)
            throw new InternalTransformErrorException("Null value for a constant expression node of type 'ValueTuple'.");

        var tupleElement = new JElement(
                                    Vocabulary.Tuple,
                                    new JElement(Vocabulary.Type, Transform.TypeName(nodeType)));
        var types = nodeType.GetGenericArguments();

        if (nodeValue is not ITuple tuple)
            throw new InternalTransformErrorException("The value of type 'ValueTuple' doesn't implement ITuple.");

        for (var i = 0; i < tuple.Length; i++)
        {
            var item = tuple[i];
            var type = types[i];

            tupleElement.Add(
                new JElement(
                        Vocabulary.TupleItem,
                        new JElement(Vocabulary.Name, $"Item{i + 1}"),
                        item is null ? new JElement(Vocabulary.Null, true) : null,
                        options.TypeComment(type),
                        GetTransform(type)(item, type)));
        }
        return tupleElement;
    }

    /// <summary>
    /// Transforms class tuples.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">GetEType of the node value.</param>
    JElement ClassTupleTransform(
        object? nodeValue,
        Type nodeType)
    {
        var tupleElement = new JElement(
                                Vocabulary.Tuple,
                                new JElement(Vocabulary.Type, Transform.TypeName(nodeType)),
                                nodeValue is null ? new JElement(Vocabulary.Null, true) : null);

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
                new JElement(
                        Vocabulary.TupleItem,
                        new JElement(Vocabulary.Name, $"Item{i + 1}"),
                        item is null ? new JElement(Vocabulary.Null, true) : null,
                        options.TypeComment(type),
                        GetTransform(type)(item, type)));
        }

        return tupleElement;
    }

    /// <summary>
    /// Transforms dictionaries.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">GetEType of the node value.</param>
    JElement DictionaryTransform(
        object? nodeValue,
        Type nodeType)
    {
        if (nodeValue is null)
            return new JElement(
                                Vocabulary.Dictionary,
                                new JElement(Vocabulary.Type, Transform.TypeName(nodeType)),
                                nodeValue is null ? new JElement(Vocabulary.Null, true) : null);

        if (nodeValue is not IDictionary dict)
            throw new InternalTransformErrorException("The value of type 'Dictionary' doesn't implement IDictionary.");

        var length = dict.Count;
        var dictElement = new JElement(
                                Vocabulary.Dictionary,
                                new JElement(Vocabulary.Type, Transform.TypeName(nodeType)),
                                new JElement(Vocabulary.Length, length),
                                nodeValue is null ? new JElement(Vocabulary.Null, true) : null);

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
                new JElement(
                    Vocabulary.KeyValuePair,
                    GetTransform(kType)(kv.Key, kType),
                    GetTransform(vType)(kv.Value, vType)));

        return dictElement;
    }

    /// <summary>
    /// Transforms objects using <see cref="DataContractSerializer" /> which also works with classes marked with 
    /// <see cref="SerializableAttribute"/> types too.
    /// </summary>
    /// <param name="nodeValue">The node value.</param>
    /// <param name="nodeType">GetEType of the node value.</param>
    JElement ObjectTransform(
        object? nodeValue,
        Type nodeType)
    {
        var element = new JElement(Vocabulary.Object);

        if (nodeValue is null)
        {
            element.Add(new JElement(Vocabulary.Null, true));
            if (nodeType != typeof(object))
                element.Add(new JElement(Vocabulary.Type, Transform.TypeName(nodeType)));
            return element;
        }

        var concreteType = nodeValue.GetType();

        if (concreteType == typeof(object))
            return element;

        var actualTransform = GetTransform(concreteType);

        if (actualTransform != ObjectTransform)
            return actualTransform(nodeValue, concreteType);

        element.Add(
            new JElement(Vocabulary.ConcreteType, Transform.TypeName(nodeType)),
            nodeType != concreteType ? new JElement(Vocabulary.ConcreteType, Transform.TypeName(concreteType)) : null
        );

        //var dcSerializer = new DataContractSerializer(concreteType);
        //using var writer = element.CreateWriter();

        // XML serialize into the element
        //dcSerializer.WriteObject(writer, nodeValue);

        var jsonElement = JsonSerializer.SerializeToElement(nodeValue, JsonTypeInfo.CreateJsonTypeInfo(concreteType, options.GetJsonSerializerOptions()));
        return element.Add(Vocabulary.Value, JsonObject.Create(jsonElement));
    }
    #endregion
}
