namespace vm2.ExpressionSerialization.XmlTransform;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

using Transform = Func<XElement, Type, object?>;

/// <summary>
/// Delegate TransformFunc
/// </summary>
/// <typeparam name="TElement">The type of the t element.</typeparam>
/// <param name="arg">The argument.</param>
/// <returns>IEnumerable&lt;TElement&gt;.</returns>
public delegate IEnumerable<TElement> TransformFunc<TElement>(IEnumerable<TElement> arg);

partial class FromXmlDataTransform(Options? options = default)
{
    Options _options = options ?? new Options();

    #region constant de-serializers
    /// <summary>
    /// The map of base type constants transforms
    /// </summary>
    static readonly Dictionary<XName, Transform> _constantDataTransforms = new()
    {
        { ElementNames.Boolean,        (x, t) => !string.IsNullOrWhiteSpace(x.Value) && XmlConvert.ToBoolean(x.Value)                     },
        { ElementNames.UnsignedByte,   (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToByte(x.Value)               : default },
        { ElementNames.Byte,           (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToSByte(x.Value)              : default },
        { ElementNames.Short,          (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToInt16(x.Value)              : default },
        { ElementNames.UnsignedShort,  (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToUInt16(x.Value)             : default },
        { ElementNames.Int,            (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToInt32(x.Value)              : default },
        { ElementNames.UnsignedInt,    (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToUInt32(x.Value)             : default },
        { ElementNames.Long,           (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToInt64(x.Value)              : default },
        { ElementNames.UnsignedLong,   (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToUInt64(x.Value)             : default },
        { ElementNames.Half,           (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? (Half)XmlConvert.ToSingle(x.Value)       : default },
        { ElementNames.Float,          (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToSingle(x.Value)             : default },
        { ElementNames.Double,         (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDouble(x.Value)             : default },
        { ElementNames.Decimal,        (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDecimal(x.Value)            : default },
        { ElementNames.Char,           (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? x.Value[0]                               : default },
        { ElementNames.Guid,           (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToGuid(x.Value)               : default },
        { ElementNames.DateTime,       (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDateTime(x.Value, XmlDateTimeSerializationMode.RoundtripKind) : default },
        { ElementNames.DateTimeOffset, (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDateTimeOffset(x.Value, "O"): default },
        { ElementNames.Duration,       (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToTimeSpan(x.Value)           : default },
        { ElementNames.IntPtr,         (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? (IntPtr)XmlConvert.ToInt32(x.Value)      : default },
        { ElementNames.UnsignedIntPtr, (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? (UIntPtr)XmlConvert.ToUInt32(x.Value)    : default },
        { ElementNames.AnyURI,         (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? new Uri(x.Value)                         : throw new SerializationException("Cannot deserialize URI object from null or empty string.") },
        { ElementNames.DBNull,         (x, t) => DBNull.Value                                                                   },
        { ElementNames.String,         (x, t) => x.Value                                                                        },
        { ElementNames.Nullable,       TransformNullable                                                                   },
        { ElementNames.Enum,           TransformEnum                                                                       },
        { ElementNames.Object,         TransformObject                                                                     },
        { ElementNames.Anonymous,      TransformAnonymous                                                                  },
        { ElementNames.ByteSequence,   TransformByteSequence                                                               },
        { ElementNames.Collection,     TransformCollection                                                                 },
        { ElementNames.Dictionary,     TransformDictionary                                                                 },
        { ElementNames.Tuple,          TransformTupleElement                                                               },
        { ElementNames.TupleItem,      TransformTupleElement                                                               },
    };
    static readonly FrozenDictionary<XName, Transform> _constantTransforms = _constantDataTransforms.ToFrozenDictionary();
    #endregion

    static Dictionary<string, Type> _elementNamesToTypes = new()
    {
        { ElementNames.Char.LocalName,              typeof(char)           },
        { ElementNames.Boolean.LocalName,           typeof(bool)           },
        { ElementNames.UnsignedByte.LocalName,      typeof(byte)           },
        { ElementNames.Byte.LocalName,              typeof(sbyte)          },
        { ElementNames.Short.LocalName,             typeof(short)          },
        { ElementNames.UnsignedShort.LocalName,     typeof(ushort)         },
        { ElementNames.Int.LocalName,               typeof(int)            },
        { ElementNames.UnsignedInt.LocalName,       typeof(uint)           },
        { ElementNames.Long.LocalName,              typeof(long)           },
        { ElementNames.UnsignedLong.LocalName,      typeof(ulong)          },
        { ElementNames.Half.LocalName,              typeof(Half)           },
        { ElementNames.Float.LocalName,             typeof(float)          },
        { ElementNames.Double.LocalName,            typeof(double)         },
        { ElementNames.Decimal.LocalName,           typeof(decimal)        },
        { ElementNames.Guid.LocalName,              typeof(Guid)           },
        { ElementNames.AnyURI.LocalName,            typeof(Uri)            },
        { ElementNames.String.LocalName,            typeof(string)         },
        { ElementNames.Duration.LocalName,          typeof(TimeSpan)       },
        { ElementNames.DateTime.LocalName,          typeof(DateTime)       },
        { ElementNames.DateTimeOffset.LocalName,    typeof(DateTimeOffset) },
        { ElementNames.DBNull.LocalName,            typeof(DBNull)         },
        { ElementNames.IntPtr.LocalName,            typeof(IntPtr)         },
        { ElementNames.UnsignedIntPtr.LocalName,    typeof(UIntPtr)        },
    };
    static FrozenDictionary<string, Type> _namesToTypes = _elementNamesToTypes.ToFrozenDictionary();

    /// <summary>
    /// Gets the constant value XML to .NET transform delegate corresponding to the XML <paramref name="element"/>.
    /// </summary>
    /// <param name="element">The element which holds transformed constant value.</param>
    /// <returns>The transforming delegate corresponding to the <paramref name="element"/>.</returns>
    internal static Transform GetTransform(XElement element) => _constantTransforms[element.Name];

    internal static ConstantExpression ConstantTransform(XElement element)
    {
        var type = GetType(element);
        var transform = GetTransform(element);
        var value = transform(element, type);

        return Expression.Constant(value, type);
    }

    internal static Type GetType(XElement element)
    {
        if (_namesToTypes.TryGetValue(element.Name.LocalName, out var type))
            return type;

        var typeName = element.Attribute(AttributeNames.Type)?.Value;

        if (element.Name == ElementNames.Nullable)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                type = GetType(element.FirstChild());
            else
            if (!_namesToTypes.TryGetValue(typeName, out type))
                type = Type.GetType(typeName) ?? throw new InternalTransformErrorException($"Non-null nullable value of unknown type in {element.Name}.");

            return typeof(Nullable<>).MakeGenericType([type]);
        }

        if (string.IsNullOrWhiteSpace(typeName))
            if (element.Name == ElementNames.Object)
                return typeof(object);
            else
                throw new SerializationException($"An XML element {element.Name} is missing the type attribute.");

        return Type.GetType(typeName)
                        ?? throw new SerializationException($"Could not resolve the type name {typeName} specified in element {element.Name}.");
    }

    static object? TransformNullable(
        XElement element,
        Type type)
    {
        if (element.IsNil())
            return null;

        // we do not need to return Nullable<T> here. Since the return type is object? the CLR will either return null or the boxed value of the Nullable<T>
        var valueElement = element.FirstChild();
        var typeElement = GetType(valueElement);
        var value = _constantTransforms[valueElement.Name](valueElement, typeElement);

        return (typeof(Nullable<>)
                    .MakeGenericType(typeElement)
                    .GetConstructor([typeElement]) ?? throw new InternalTransformErrorException($"Could not get the constructor for Nullable<{typeElement.Name}>"))
                    .Invoke([value]);
    }

    static object? TransformEnum(
        XElement element,
        Type type)
    {
        try
        {
            return Enum.Parse(type, element.Value);
        }
        catch (ArgumentException ex)
        {
            throw new SerializationException(
                        $"Cannot transform {element.Value} to {type.FullName} value.", ex);
        }
        catch (OverflowException ex)
        {
            throw new SerializationException(
                        $"Cannot transform {element.Value} to {type.FullName} value.", ex);
        }
    }

    static object? TransformObject(
        XElement element,
        Type type)
    {
        if (element.IsNil() || element.Elements().FirstOrDefault() is null)
            return null;

        if (type == typeof(object))
            return new();

        var concreteTypeName = element.Attribute(AttributeNames.ConcreteType)?.Value;
        var concreteType = !string.IsNullOrEmpty(concreteTypeName) ? Type.GetType(concreteTypeName) : null;

        var serializer = new DataContractSerializer(concreteType ?? type);
        using var reader = element.FirstChild().CreateReader();

        return serializer.ReadObject(reader);
    }

    static object? TransformAnonymous(
        XElement element,
        Type type)
    {
        if (!element.Elements(ElementNames.Property).Any())
            return null;

        var constructor = type.GetConstructors()[0];
        var constructorParameters = constructor.GetParameters();
        var parameters = new object?[constructorParameters.Length];

        for (var i = 0; i < constructorParameters.Length; i++)
        {
            var paramName = constructorParameters[i].Name;
            var propElement = element
                                .Elements(ElementNames.Property)
                                .Where(e => paramName == (e.Attribute(AttributeNames.Name)?.Value
                                                                ?? throw new SerializationException($"Expected attribute with name {paramName}.")))
                                .First()
                                .FirstChild()
                                ;

            if (propElement is not null)
                parameters[i] = GetTransform(propElement)(propElement, GetType(propElement));
        }

        return constructor.Invoke(parameters);
    }

    static object? TransformByteSequence(
        XElement element,
        Type type)
    {
        var length = element.Length();
        var bytes = Convert.FromBase64String(element.Value);

        if (length.HasValue && length.Value != bytes.Length)
            throw new SerializationException($"Unexpected value of element {element.Name}.");

        if (type == typeof(byte[]))
            return bytes;
        if (type == typeof(ImmutableArray<byte>))
            return bytes.ToImmutableArray();
        if (type == typeof(ArraySegment<byte>))
            return new ArraySegment<byte>(bytes);
        if (type == typeof(Memory<byte>) || type == typeof(Span<byte>))
            return new Memory<byte>(bytes);
        if (type == typeof(ReadOnlyMemory<byte>) || type == typeof(ReadOnlySpan<byte>))
            return new ReadOnlyMemory<byte>(bytes);

        throw new SerializationException($"Unexpected type of element {element.Name}.");
    }

    static MethodInfo _toFrozenSet = typeof(FrozenSet).GetMethod("ToFrozenSet")!;
    static MethodInfo _toImmutableArray = typeof(ImmutableArray).GetMethod("ToImmutableArray", [typeof(IEnumerable<>)])!;  // 
    static MethodInfo _toImmutableHashSet = typeof(ImmutableHashSet).GetMethod("ToImmutableHashSet", [typeof(IEnumerable<>)])!;
    static MethodInfo _toImmutableList = typeof(ImmutableList).GetMethod("ToImmutableList", [typeof(IEnumerable<>)])!;
    static MethodInfo _toImmutableSortedSet = typeof(ImmutableSortedSet).GetMethod("ToImmutableSortedSet", [typeof(IEnumerable<>)])!;
    static MethodInfo _toImmutableQueue = typeof(ImmutableQueue).GetMethod("Create", [typeof(IEnumerable<>)])!;
    static MethodInfo _toImmutableStack = typeof(ImmutableStack).GetMethod("Create", [typeof(IEnumerable<>)])!;
    static MethodInfo _toList = typeof(Enumerable).GetMethod("ToList", [typeof(IEnumerable<>)])!;
    static MethodInfo _toHashSet = typeof(Enumerable).GetMethod("ToHashSet", [typeof(IEnumerable<>)])!;
    static MethodInfo _cast = typeof(Enumerable).GetMethod("Cast")!;

    static object? CastSequence(IEnumerable sequence, Type elementType)
        => _cast.MakeGenericMethod(elementType).Invoke(null, [sequence]);

    static readonly Dictionary<Type, Func<Type, Type, int, IEnumerable, object?>> _sequenceTypesToTransforms = new ()
    {
        [typeof(FrozenSet<>)]           = (gt, et, len, seq) => _toFrozenSet.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et), null]),
        [typeof(ImmutableArray<>)]      = (gt, et, len, seq) => _toImmutableArray.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(ImmutableHashSet<>)]    = (gt, et, len, seq) => _toImmutableHashSet.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(ImmutableList<>)]       = (gt, et, len, seq) => _toImmutableList.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(ImmutableSortedSet<>)]  = (gt, et, len, seq) => _toImmutableSortedSet.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(ImmutableQueue<>)]      = (gt, et, len, seq) => _toImmutableQueue.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(ImmutableStack<>)]      = (gt, et, len, seq) => _toImmutableStack.MakeGenericMethod(et).Invoke(null, [CastSequence(seq.Cast<object?>().Reverse(), et)]),
        [typeof(List<>)]                = (gt, et, len, seq) => _toList.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(HashSet<>)]             = (gt, et, len, seq) => _toHashSet.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(ArraySegment<>)]        = (gt, et, len, seq) => TransformWithConstructor(gt, et, TransformToArray(et, len, seq)),
        [typeof(Memory<>)]              = (gt, et, len, seq) => TransformWithConstructor(gt, et, TransformToArray(et, len, seq)),
        [typeof(ReadOnlyMemory<>)]      = (gt, et, len, seq) => TransformWithConstructor(gt, et, TransformToArray(et, len, seq)),
        [typeof(ConcurrentBag<>)]       = (gt, et, len, seq) => TransformWithConstructor(gt, et, seq),
        [typeof(ConcurrentQueue<>)]     = (gt, et, len, seq) => TransformWithConstructor(gt, et, seq),
        [typeof(ConcurrentStack<>)]     = (gt, et, len, seq) => TransformWithConstructor(gt, et, seq.Cast<object?>().Reverse()),
        [typeof(Collection<>)]          = (gt, et, len, seq) => TransformWithConstructor(gt, et, seq.Cast<object>().ToList()),
        [typeof(ReadOnlyCollection<>)]  = (gt, et, len, seq) => TransformWithConstructor(gt, et, seq.Cast<object>().ToList()),
        [typeof(LinkedList<>)]          = (gt, et, len, seq) => TransformWithConstructor(gt, et, seq),
        [typeof(Queue<>)]               = (gt, et, len, seq) => TransformWithConstructor(gt, et, seq),
        [typeof(SortedSet<>)]           = (gt, et, len, seq) => TransformWithConstructor(gt, et, seq),
        [typeof(Stack<>)]               = (gt, et, len, seq) => TransformWithConstructor(gt, et, seq.Cast<object?>().Reverse()),
        [typeof(BlockingCollection<>)]  = TransformToBlockingCollection,
    };
    static readonly FrozenDictionary<Type, Func<Type, Type, int, IEnumerable, object?>> _sequenceTransforms = _sequenceTypesToTransforms.ToFrozenDictionary();

    static IEnumerable TransformToArray(
        Type elementType,
        int length,
        IEnumerable elements)
    {
        var array = Array.CreateInstance(elementType, length);
        var itr = elements.GetEnumerator();

        itr.MoveNext();
        for (int i = 0; i < length; i++, itr.MoveNext())
            array.SetValue(itr.Current, i);

        return array;
    }

    static object TransformToBlockingCollection(
        Type genericType,
        Type elementType,
        int length,
        IEnumerable elements)
    {
        var bcCtor = genericType
                        .MakeGenericType(elementType)
                        .GetConstructors()
                        .Where(ci => ci.GetParameters().Length == 0)
                        .FirstOrDefault()
                            ?? throw new InternalTransformErrorException("Could not get constructor for BlockingCollection<T>(array).")
                        ;
        var bc = bcCtor.Invoke([]);

        var addMi = genericType.MakeGenericType(elementType)
                        .GetMethods()
                        .Where(ci => ci.Name == "Add" && ci.GetParameters().Length == 1)
                        .FirstOrDefault()
                            ?? throw new InternalTransformErrorException("Could not get method info for BlockingCollection<T>.Add(element).")
                        ;
        var added = elements.Cast<object?>().Select(e => { addMi.Invoke(bc, [e]); return 1; }).Count();

        if (added != length)
            throw new InternalTransformErrorException("Could not add some or all members of the input sequence to BlockingCollection<T>.");

        return bc;
    }

    static object TransformWithConstructor(
        Type genericType,
        Type elementType,
        IEnumerable elements)
    {
        var ctor = genericType
                        .MakeGenericType(elementType)
                        .GetConstructors()
                        .Where(ci => ci.GetParameters().Length == 1)
                        .FirstOrDefault()
                            ?? throw new InternalTransformErrorException("Could not get constructor for ArraySegment<T>(array).");

        return ctor!.Invoke([CastSequence(elements, elementType)]);
    }

    static object? TransformCollection(
        XElement element,
        Type type)
    {
        int length = element.Elements().Count();
        var len = element.Length();

        if (len.HasValue && len.Value != length)
            throw new SerializationException($"The specified length af an array in the XML element {element.Name} is different from the actual length.");

        Type elementType = type.IsArray
                                ? type.GetElementType()
                                    ?? throw new SerializationException($"Could not get the type of the array elements in the XML element {element.Name}.")
                                : type.IsGenericType
                                    ? type.GetGenericArguments()[0]
                                    : throw new SerializationException($"Could not get the type of the array elements in the XML element {element.Name}.");

        var elements = element
                        .Elements()
                        .Select(e => GetTransform(e)(e, GetType(e)));


        if (type.IsArray)
            return TransformToArray(elementType, length, elements);

        if (!type.IsGenericType)
            throw new SerializationException($"The collection in {element.Name} must be either array or a generic collection.");

        var genericType = type.Name.EndsWith("FrozenSet`1") ? typeof(FrozenSet<>) : type.GetGenericTypeDefinition();

        if (_sequenceTransforms.TryGetValue(genericType, out var fn))
            return fn(genericType, elementType, length, elements);

        throw new SerializationException($"Don't know how to deserialize {type.FullName}.");
    }

    static object? TransformDictionary(
        XElement element,
        Type type)
    {
        if (Activator.CreateInstance(type) is not IDictionary dict)
            throw new InternalTransformErrorException($"The type {type.FullName} does not implement IDictionary.");

        var kvTypes   = type.GetGenericArguments();

        if (kvTypes.Length is not 2)
            throw new InternalTransformErrorException("The elements of 'Dictionary' do not have key-type and element-type.");

        foreach (var kvElement in element.Elements(ElementNames.KeyValuePair))
        {
            var keyElement = kvElement.FirstChild();
            var valElement = kvElement.Elements().LastOrDefault();

            if (keyElement is null || valElement is null)
                throw new SerializationException($"Could not find a key-value pair in {element.Name}.");

            var key = TransformObject(keyElement, GetType(keyElement)) ?? throw new SerializationException($"Could transform a value of a key in {element.Name}.");
            var value = TransformObject(valElement, GetType(valElement));

            dict[key] = value;
        }

        return dict;
    }

    static object? TransformTupleElement(
        XElement element,
        Type type)
    {
        var parameters = element
                            .Elements(ElementNames.TupleItem)
                            .Select(
                                e =>
                                {
                                    var i = e.FirstChild();
                                    return TransformObject(i, GetType(i));
                                })
                            .ToArray()
                            ;
        return Activator.CreateInstance(type, parameters);
    }
}
