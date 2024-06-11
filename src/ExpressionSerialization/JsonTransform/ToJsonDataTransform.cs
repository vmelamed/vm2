namespace vm2.ExpressionSerialization.JsonTransform;

using System.Text.Json.Serialization.Metadata;

using TransformConstant = Func<object?, Type, JElement>;

/// <summary>
/// Class ToJsonDataTransform transforms data (Expression constants) to JSON.
/// </summary>
public class ToJsonDataTransform(JsonOptions options)
{
    /// <summary>
    /// The maximum long number that can be expressed as &quot;JSON integer&quot; without loosing fidelity.
    /// </summary>
    /// <remarks>
    /// In JavaScript, the maximum safe integer is 2^53 - 1, which is 9007199254740991. This is because JavaScript uses 
    /// double-precision floating-point format numbers as specified in IEEE 754, and can only safely represent integers 
    /// between [-(2^53-1), 2^53 - 1].
    /// Therefore we serialize numbers outside of that range as strings, e.g. <c>&quot;9007199254740992&quot;</c>.
    /// </remarks>
    public const long MaxJsonInteger = 9_007_199_254_740_991L;

    /// <summary>
    /// The minimum long number that can be expressed as &quot;JSON integer&quot; without loosing fidelity.
    /// </summary>
    /// <remarks>
    /// In JavaScript, the maximum safe integer is 2^53 - 1, which is 9007199254740991. This is because JavaScript uses 
    /// double-precision floating-point format numbers as specified in IEEE 754, and can only safely represent integers 
    /// from the range [-(2^53-1), 2^53 - 1].
    /// Therefore we serialize numbers outside of that range as strings, e.g. <c>&quot;9007199254740992&quot;</c>.
    /// </remarks>
    public const long MinJsonInteger = -MaxJsonInteger;

    static T Is<T>(object? v) where T : struct
        => v is T tv ? tv : throw new InternalTransformErrorException($"Expected {typeof(T).Name} v but got {(v is null ? "null" : v.GetType().Name)}");

    static T? Is<T>(object? v, bool nullable = true) where T : class
        => v is T || (nullable && v is null) ? (T?)v : throw new InternalTransformErrorException($"Expected {typeof(T).Name} v but got {(v is null ? "null" : v.GetType().Name)}");

    #region constant ToJson transforms
    static ReadOnlyDictionary<Type, TransformConstant> _constantTransformsDict = new (new Dictionary<Type, TransformConstant>()
    {
        { typeof(bool),             (v, _) => new JElement(Vocabulary.Boolean,        JsonValue.Create(Is<bool>(v))) },
        { typeof(byte),             (v, _) => new JElement(Vocabulary.Byte,           JsonValue.Create(Is<byte>(v))) },
        { typeof(char),             (v, _) => new JElement(Vocabulary.Char,           JsonValue.Create(Is<char>(v).ToString())) },
        { typeof(double),           (v, _) => new JElement(Vocabulary.Double,         JsonValue.Create(Is<double>(v))) },
        { typeof(float),            (v, _) => new JElement(Vocabulary.Float,          JsonValue.Create(Is<float>(v))) },
        { typeof(int),              (v, _) => new JElement(Vocabulary.Int,            JsonValue.Create(Is<int>(v))) },
        { typeof(IntPtr),           (v, _) => new JElement(Vocabulary.IntPtr,         JsonValue.Create((int)Is<IntPtr>(v))) },
        { typeof(long),             (v, _) => new JElement(Vocabulary.Long,           Is<long>(v) is <= MaxJsonInteger and >= MinJsonInteger ? JsonValue.Create((long)v!) : JsonValue.Create(v!.ToString()) ) },
        { typeof(sbyte),            (v, _) => new JElement(Vocabulary.SignedByte,     JsonValue.Create(Is<sbyte>(v))) },
        { typeof(short),            (v, _) => new JElement(Vocabulary.Short,          JsonValue.Create(Is<short>(v))) },
        { typeof(uint),             (v, _) => new JElement(Vocabulary.UnsignedInt,    JsonValue.Create(Is<uint>(v))) },
        { typeof(UIntPtr),          (v, _) => new JElement(Vocabulary.UnsignedIntPtr, JsonValue.Create((uint)Is<UIntPtr>(v))) },
        { typeof(ulong),            (v, _) => new JElement(Vocabulary.UnsignedLong,   Is<ulong>(v) is <= MaxJsonInteger ? JsonValue.Create((long)(ulong)v!) : JsonValue.Create(v!.ToString()) ) },
        { typeof(ushort),           (v, _) => new JElement(Vocabulary.UnsignedShort,  JsonValue.Create(Is<ushort>(v))) },

        { typeof(DateTime),         (v, _) => new JElement(Vocabulary.DateTime,       JsonValue.Create(Is<DateTime>(v).ToString("o"))) },
        { typeof(DateTimeOffset),   (v, _) => new JElement(Vocabulary.DateTimeOffset, JsonValue.Create(Is<DateTimeOffset>(v).ToString("o"))) },
        { typeof(TimeSpan),         (v, _) => new JElement(Vocabulary.Duration,       JsonValue.Create(Duration(Is<TimeSpan>(v)))) },
        { typeof(DBNull),           (v, _) => new JElement(Vocabulary.DBNull)         },
        { typeof(decimal),          (v, _) => new JElement(Vocabulary.Decimal,        JsonValue.Create(Is<decimal>(v).ToString())) },
        { typeof(Guid),             (v, _) => new JElement(Vocabulary.Guid,           JsonValue.Create(Is<Guid>(v).ToString())) },
        { typeof(Half),             (v, _) => new JElement(Vocabulary.Half,           JsonValue.Create((float)Is<Half>(v))) },
        { typeof(string),           (v, _) => new JElement(Vocabulary.String,         JsonValue.Create(Is<string>(v))) },
        { typeof(Uri),              (v, _) => new JElement(Vocabulary.Uri,            JsonValue.Create(Is<Uri>(v)?.ToString())) },
    });
    static FrozenDictionary<Type, TransformConstant> _constantTransforms = _constantTransformsDict.ToFrozenDictionary();
    #endregion

    static string Duration(TimeSpan ts)
    {
        var sbFormat = new StringBuilder();
        if (ts < TimeSpan.Zero)
        {
            sbFormat.Append(@"\-");
            ts = ts.Negate();
        }
        sbFormat.Append(@"\P");
        if (ts.Days != 0)
            sbFormat.Append(@"d\D");
        sbFormat.Append(@"\Th\Hm\Ms\S");

        return ts.ToString(sbFormat.ToString());
    }

    /// <summary>Transforms the node.</summary>
    /// <param name="node">The node.</param>
    /// <returns>JElement.</returns>
    public JElement TransformNode(ConstantExpression node) => GetTransform(node.Type)(node.Value, node.Type);

    /// <summary>
    /// Gets the best matching transform function for the type encapsulated in the  for the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>
    /// A delegate that can transform a nodeValue of the specified <paramref name="type"/> into an JSON element (<see cref="JElement"/>).
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

        // if it is a nodeValue - get nodeValue transform or
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
    /// <param name="nodeValue">The node v.</param>
    /// <param name="nodeType">GetEType of the node v.</param>
    JElement EnumTransform(
        object? nodeValue,
        Type nodeType)
    {
        Debug.Assert(nodeValue is not null);

        var strValue = nodeValue.ToString();
        var valueType = new JElement(Vocabulary.Type, Transform.TypeName(nodeType));
        var valueElement = new JElement(Vocabulary.Value);

        if (strValue is not null && nodeType.IsDefined(typeof(FlagsAttribute)))
            valueElement.Value = new JsonArray(
                                        strValue
                                            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                            .Select(v => JsonValue.Create(v))
                                            .ToArray());
        else
            valueElement.Value = JsonValue.Create(strValue);

        var underlyingType = nodeType.GetEnumUnderlyingType();

        return new JElement(
                        Vocabulary.Enum,
                        valueType,
                        valueElement,
                        underlyingType != typeof(int) ? new JElement(Vocabulary.BaseType, Transform.TypeName(underlyingType)) : null,
                        new JElement(Vocabulary.BaseValue, (int)nodeValue)
                    );
    }

    /// <summary>
    /// Transforms a nodeValue nodeValue.
    /// </summary>
    /// <param name="nodeValue">The node v.</param>
    /// <param name="nodeType">GetEType of the node v.</param>
    JElement NullableTransform(
        object? nodeValue,
        Type nodeType)
    {
        var underlyingType  = nodeType.GetGenericArguments()[0];

        if (nodeValue is null || nodeType.GetProperty("HasValue")?.GetValue(nodeValue) is false)
            return new JElement(
                            Vocabulary.Nullable,
                            new JElement(Vocabulary.Type, Transform.TypeName(underlyingType)),
                            new JElement(Vocabulary.Null, true));

        var value = nodeType.GetProperty("Value")?.GetValue(nodeValue)
                        ?? throw new InternalTransformErrorException("'Nullable<T>.HasValue' is true but 'Nullable<T>.Value' is null.");

        return new JElement(
            Vocabulary.Nullable,
            options.TypeComment(underlyingType),
            GetTransform(underlyingType)(value, underlyingType));
    }

    /// <summary>
    /// Transforms an anonymous object.
    /// </summary>
    /// <param name="nodeValue">The node v.</param>
    /// <param name="nodeType">GetEType of the node v.</param>
    JElement AnonymousTransform(
        object? nodeValue,
        Type nodeType)
        => new(
                    Vocabulary.Anonymous,
                    new JElement(Vocabulary.Type, Transform.TypeName(nodeType)),
                    new JElement(Vocabulary.Value, nodeType
                                                    .GetProperties()
                                                    .Select(pi => new JElement(
                                                                        pi.Name,
                                                                        GetTransform(pi.PropertyType)(
                                                                            pi.GetValue(nodeValue, null),
                                                                            pi.PropertyType)))));

    /// <summary>
    /// Transforms sequences of bytes.
    /// </summary>
    /// <param name="nodeValue">The node v.</param>
    /// <param name="nodeType">GetEType of the node v.</param>
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
                throw new InternalTransformErrorException("Unexpected non-array byte sequenceElement with null v of type '{nodeType.FullName}'.");

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
            new JElement(Vocabulary.Value, Convert.ToBase64String(bytes)),
            new JElement(Vocabulary.Length, bytes.Length));

        return sequenceElement;
    }

    /// <summary>
    /// Transforms sequences of objects.
    /// </summary>
    /// <param name="nodeValue">The node v.</param>
    /// <param name="nodeType">GetEType of the node v.</param>
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
    /// Transforms v tuples.
    /// </summary>
    /// <param name="nodeValue">The node v.</param>
    /// <param name="nodeType">GetEType of the node v.</param>
    JElement ValueTupleTransform(
        object? nodeValue,
        Type nodeType)
    {
        if (nodeValue is null)
            throw new InternalTransformErrorException("Null v for a constant expression node of type 'ValueTuple'.");

        var tupleElement = new JElement(
                                    Vocabulary.Tuple,
                                    new JElement(Vocabulary.Type, Transform.TypeName(nodeType)));
        var types = nodeType.GetGenericArguments();

        if (nodeValue is not ITuple tuple)
            throw new InternalTransformErrorException("The v of type 'ValueTuple' doesn't implement ITuple.");

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
    /// <param name="nodeValue">The node v.</param>
    /// <param name="nodeType">GetEType of the node v.</param>
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
            throw new InternalTransformErrorException("The v of type 'ValueTuple' doesn't implement ITuple.");

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
    /// <param name="nodeValue">The node v.</param>
    /// <param name="nodeType">GetEType of the node v.</param>
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
            throw new InternalTransformErrorException("The v of type 'Dictionary' doesn't implement IDictionary.");

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
    /// <param name="nodeValue">The node v.</param>
    /// <param name="nodeType">GetEType of the node v.</param>
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

        // USE: Utf8JsonWriter.WriteRawValue?

        //var dcSerializer = new DataContractSerializer(concreteType);
        //using var writer = element.CreateWriter();

        // XML serialize into the element
        //dcSerializer.WriteObject(writer, nodeValue);

        var jsonElement = JsonSerializer.SerializeToElement(nodeValue, JsonTypeInfo.CreateJsonTypeInfo(concreteType, options.JsonSerializerOptions));
        return element.Add(Vocabulary.Value, JsonObject.Create(jsonElement));
    }
    #endregion
}
