namespace vm2.ExpressionSerialization.XmlTransform;

using System.Linq;

partial class FromXmlDataTransform(Options? options = default)
{
    Options _options = options ?? new Options();

#pragma warning disable IDE0075 // Simplify conditional expression
    #region constant de-serializers
    /// <summary>
    /// The map of base type constants transforms
    /// </summary>
    static readonly Dictionary<XName, Func<XElement, object?>> _constantTransforms = new()
    {
        { ElementNames.Boolean,        x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToBoolean(x.Value)            : default },
        { ElementNames.UnsignedByte,   x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToByte(x.Value)               : default },
        { ElementNames.Byte,           x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToSByte(x.Value)              : default },
        { ElementNames.Short,          x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToInt16(x.Value)              : default },
        { ElementNames.UnsignedShort,  x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToUInt16(x.Value)             : default },
        { ElementNames.Int,            x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToInt32(x.Value)              : default },
        { ElementNames.UnsignedInt,    x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToUInt32(x.Value)             : default },
        { ElementNames.Long,           x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToInt64(x.Value)              : default },
        { ElementNames.UnsignedLong,   x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToUInt64(x.Value)             : default },
        { ElementNames.Half,           x => !string.IsNullOrWhiteSpace(x.Value) ? (Half)XmlConvert.ToSingle(x.Value)       : default },
        { ElementNames.Float,          x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToSingle(x.Value)             : default },
        { ElementNames.Double,         x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDouble(x.Value)             : default },
        { ElementNames.Decimal,        x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDecimal(x.Value)            : default },
        { ElementNames.Char,           x => !string.IsNullOrWhiteSpace(x.Value) ? Convert.ToChar(Convert.ToInt32(x.Value, CultureInfo.InvariantCulture)) : default },
        { ElementNames.Guid,           x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToGuid(x.Value)               : default },
        { ElementNames.DateTime,       x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDateTime(x.Value, XmlDateTimeSerializationMode.RoundtripKind) : default },
        { ElementNames.DateTimeOffset, x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDateTimeOffset(x.Value)     : default },
        { ElementNames.Duration,       x => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToTimeSpan(x.Value)           : default },
        { ElementNames.IntPtr,         x => !string.IsNullOrWhiteSpace(x.Value) ? (IntPtr)XmlConvert.ToInt32(x.Value)      : default },
        { ElementNames.UnsignedIntPtr, x => !string.IsNullOrWhiteSpace(x.Value) ? (UIntPtr)XmlConvert.ToUInt32(x.Value)    : default },
        { ElementNames.AnyURI,         x => !string.IsNullOrWhiteSpace(x.Value) ? new Uri(x.Value)                         : throw new SerializationException("Cannot deserialize URI object from null or empty string.") },
        { ElementNames.DBNull,         x => DBNull.Value                                                                   },
        { ElementNames.String,         x => x.Value                                                                        },
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
    #endregion
#pragma warning restore IDE0075 // Simplify conditional expression

    static ReaderWriterLockSlim _namesToTypesLock = new(LockRecursionPolicy.SupportsRecursion);
    static Dictionary<string, Type> _namesToTypes = new()
        {
            { "void",           typeof(void)        },
            { "char",           typeof(char)        },
            { "boolean",        typeof(bool)        },
            { "unsignedByte",   typeof(byte)        },
            { "byte",           typeof(sbyte)       },
            { "short",          typeof(short)       },
            { "unsignedShort",  typeof(ushort)      },
            { "int",            typeof(int)         },
            { "unsignedInt",    typeof(uint)        },
            { "long",           typeof(long)        },
            { "unsignedLong",   typeof(ulong)       },
            { "float",          typeof(float)       },
            { "double",         typeof(double)      },
            { "decimal",        typeof(decimal)     },
            { "guid",           typeof(Guid)        },
            { "anyURI",         typeof(Uri)         },
            { "string",         typeof(string)      },
            { "duration",       typeof(TimeSpan)    },
            { "dateTime",       typeof(DateTime)    },
            { "dbNull",         typeof(DBNull)      },
            { "nullable",       typeof(Nullable<>)  },
            { "custom",         typeof(object)      },
            { "enum",           typeof(Enum)        },
        };

    /// <summary>
    /// Gets the constant value XML to .NET transform delegate corresponding to the XML <paramref name="element"/>.
    /// </summary>
    /// <param name="element">The element which holds transformed constant value.</param>
    /// <returns>The transforming delegate corresponding to the <paramref name="element"/>.</returns>
    internal static Func<XElement, object?> GetTransform(XElement element) => _constantTransforms[element.Name];

    internal static Type GetType(XElement element)
        => GetType(element.Attribute(AttributeNames.Type)
                        ?? throw new SerializationException($"An XML element {element.Name} is missing the type attribute."));

    internal static ConstantExpression ConstantTransform(XElement element)
    {
        var type = FromXmlDataTransform.GetType(element);
        var value = FromXmlDataTransform.GetTransform(element)(element);

        return Expression.Constant(value, type);
    }

    /// <summary>
    /// Gets the type corresponding to a type name written in an XML attribute.
    /// </summary>
    /// <param name="typeAttribute">The type attribute.</param>
    /// <returns>The specified type.</returns>
    internal static Type GetType(XAttribute typeAttribute)
        => GetType(typeAttribute.Value);

    /// <summary>
    /// Gets the type corresponding to a type name.
    /// </summary>
    /// <param name="typeName">The type name.</param>
    /// <returns>The specified type.</returns>
    internal static Type GetType(string typeName)
    {
        using (_namesToTypesLock.ReaderLock())
        {
            if (_namesToTypes.TryGetValue(typeName, out var type))
                return type;

            type = Type.GetType(typeName) ?? throw new SerializationException($"Unknown type {typeName}.");
            using (_namesToTypesLock.WriterLock())
                _namesToTypes[typeName] = type;
            return type;
        }
    }

    static object? TransformNullable(XElement element)
    {
        if (XmlConvert.ToBoolean(
                element.Attribute(AttributeNames.Nil)?.Value
                    ?? throw new SerializationException($"Deserialization error in element {element.Name}.")))
            return null;

        // we do not need to return Nullable<T> here. Since the return type is object? the CLR will either return null or the boxed value of the Nullable<T>
        var valueElement = element.Elements().FirstOrDefault()
                                ?? throw new SerializationException($"Deserialization error in element {element.Name}.");
        return _constantTransforms[valueElement.Name](valueElement);
    }

    static object? TransformEnum(XElement element)
    {
        var enumType = GetType(element);

        try
        {
            return Enum.Parse(enumType, element.Value);
        }
        catch (ArgumentException ex)
        {
            throw new SerializationException(
                        $"Cannot transform {element.Value} to {enumType.FullName} value.", ex);
        }
        catch (OverflowException ex)
        {
            throw new SerializationException(
                        $"Cannot transform {element.Value} to {enumType.FullName} value.", ex);
        }
    }

    static object? TransformObject(XElement element)
    {
        if (element.Elements().FirstOrDefault() is null)
            return null;

        string typeString = element.Attribute(AttributeNames.Type)?.Value
                                ?? throw new SerializationException($"Expected type attribute in element {element.Name}.");

        var serializer = new DataContractSerializer(GetType(typeString));
        using var reader = element.Elements().First().CreateReader();

        return serializer.ReadObject(reader);
    }

    static object? TransformAnonymous(XElement element)
    {
        if (!element.Elements(ElementNames.Property).Any())
            return null;

        var type = GetType(element);
        var constructor = type.GetConstructors()[0];
        var constructorParameters = constructor.GetParameters();
        var parameters = new object[constructorParameters.Length];

        for (var i = 0; i < constructorParameters.Length; i++)
        {
            var paramName = constructorParameters[i].Name;
            var propElement = element
                                .Elements(ElementNames.Property)
                                .Where(e => paramName == (e.Attribute(AttributeNames.Name)?.Value ?? throw new SerializationException($"Expected attribute with name {paramName}.")))
                                .First()
                                .Elements()
                                .FirstOrDefault()
                                ;

            if (propElement is not null)
                parameters[i] = GetTransform(propElement)(propElement) ?? throw new SerializationException($"Don't know how to transform {propElement.Name}.");
        }

        return constructor.Invoke(parameters);
    }

    static object? TransformByteSequence(XElement element)
    {
        var length = int.Parse(element.Attribute(AttributeNames.Length)?.Value ?? "0");
        var bytes = Convert.FromBase64String(element.Value);

        if (bytes.Length != length)
            throw new SerializationException($"Unexpected value of element {element.Name}.");

        var type = GetType(element);

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

    static readonly Dictionary<Type, Func<IEnumerable<ConstantExpression>, IEnumerable<ConstantExpression>>> _sequenceTransforms = new ()
    {
        [typeof(ArraySegment<>)]       = s => new ArraySegment<ConstantExpression>(s.ToArray()),
        [typeof(FrozenSet<>)]          = s => s.ToFrozenSet(),
        [typeof(ImmutableArray<>)]     = s => s.ToImmutableArray(),
        [typeof(ImmutableHashSet<>)]   = s => s.ToImmutableHashSet(),
        [typeof(ImmutableList<>)]      = s => s.ToImmutableList(),
        [typeof(ImmutableQueue<>)]     = s => ImmutableQueue.Create(s.ToArray()),
        [typeof(ImmutableSortedSet<>)] = s => s.ToImmutableSortedSet(),
        [typeof(ImmutableStack<>)]     = s => ImmutableStack.Create(s.Reverse().ToArray()),
        [typeof(ConcurrentBag<>)]      = s => new ConcurrentBag<ConstantExpression>(s),
        [typeof(ConcurrentQueue<>)]    = s => new ConcurrentQueue<ConstantExpression>(s),
        [typeof(ConcurrentStack<>)]    = s => new ConcurrentStack<ConstantExpression>(s.Reverse()),
        [typeof(Collection<>)]         = s => new Collection<ConstantExpression>(s.ToList()),
        [typeof(ReadOnlyCollection<>)] = s => new ReadOnlyCollection<ConstantExpression>(s.ToList()),
        [typeof(List<>)]               = s => s.ToList(),
        [typeof(LinkedList<>)]         = s => new LinkedList<ConstantExpression>(s),
        [typeof(HashSet<>)]            = s => s.ToHashSet(),
        [typeof(Queue<>)]              = s => new Queue<ConstantExpression>(s),
        [typeof(SortedSet<>)]          = s => new SortedSet<ConstantExpression>(s),
        [typeof(Stack<>)]              = s => new Stack<ConstantExpression>(s.Reverse()),
        [typeof(BlockingCollection<>)] = s =>
        {
            var bc = new BlockingCollection<ConstantExpression>();
            s.Select(e => { bc.Add(e); return 1; }).Count();
            return bc!;
        },
    };

    static object? TransformCollection(XElement element)
    {
        var type = Type.GetType(element.Attribute(AttributeNames.Type)?.Value
                                    ?? throw new SerializationException($"Could not get the required attribute 'type' in the element {element.Name}"))
                            ?? throw new SerializationException($"Unknown type {element.Attribute(AttributeNames.Type)?.Value} in the element {element.Name}");
        var elements = element.Elements().Select(ConstantTransform);
        var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;

        if (type.IsArray)
            return elements.ToArray();

        if (genericType == typeof(Memory<>))
            return new Memory<Expression>(elements.ToArray());
        if (genericType == typeof(ReadOnlyMemory<>))
            return new ReadOnlyMemory<Expression>(elements.ToArray());

        if (genericType is not null && _sequenceTransforms.TryGetValue(genericType!, out var transformSequence))
            return transformSequence(elements);

        throw new SerializationException($"Don't know how to deserialize {type.FullName}.");
    }

    static readonly Dictionary<Type, Func<IDictionary>> _dictionaryTransforms = new ()
    {
        [typeof(Hashtable)]             = () => new Hashtable(),
        [typeof(Dictionary<,>)]         = () => new Hashtable(),
    };

    static object? TransformDictionary(XElement element)
    {
        var dictType = Type.GetType(element.Attribute(AttributeNames.Type)?.Value
                                    ?? throw new SerializationException($"Could not get the required attribute 'type' in the element {element.Name}"))
                            ?? throw new SerializationException($"Unknown type {element.Attribute(AttributeNames.Type)?.Value} in the element {element.Name}");
        if (Activator.CreateInstance(dictType) is not IDictionary dict)
            throw new InternalTransformErrorException($"The type {dictType.FullName} does not implement IDictionary.");

        var kvTypes   = dictType.GetGenericArguments();

        if (kvTypes.Length is not 2)
            throw new InternalTransformErrorException("The elements of 'Dictionary' do not have key-type and element-type.");

        foreach (var kvElement in element.Elements(ElementNames.KeyValuePair))
        {
            var keyElement = kvElement.Elements().FirstOrDefault();
            var valElement = kvElement.Elements().LastOrDefault();

            if (keyElement is null || valElement is null)
                throw new SerializationException($"Could not find a key-value pair in {element.Name}.");

            var key = TransformObject(keyElement) ?? throw new SerializationException($"Could transform a value of a key in {element.Name}.");
            var value = TransformObject(valElement);

            dict[key] = value;
        }

        return dict;
    }

    const string indexCG = "index";

    [GeneratedRegex($@"Item(?<{indexCG}>\d+)", RegexOptions.Compiled | RegexOptions.IgnoreCase, 500)]
    private static partial Regex ItemName();

    static object? TransformTupleElement(XElement element)
    {
        var tupleType = Type.GetType(element.Attribute(AttributeNames.Type)?.Value
                                    ?? throw new SerializationException($"Could not get the required attribute 'type' in the element {element.Name}"))
                            ?? throw new SerializationException($"Unknown type {element.Attribute(AttributeNames.Type)?.Value} in the element {element.Name}");

        var ctorInfo = tupleType.GetConstructors()[0];
        var ctorParams = element.Elements(ElementNames.TupleItem).Select(e => TransformObject(e.Elements().First())).ToArray();

        return ctorInfo.Invoke(null, ctorParams);
    }
}
