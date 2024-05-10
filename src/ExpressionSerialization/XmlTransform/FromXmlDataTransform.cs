namespace vm2.ExpressionSerialization.XmlTransform;

using Transform = Func<XElement, Type, object?>;

partial class FromXmlDataTransform
{
    /// <summary>
    /// Gets the constant value XML to .NET transform delegate corresponding to the XML <paramref name="element"/>.
    /// </summary>
    /// <param name="element">The element which holds transformed constant value.</param>
    /// <returns>The transforming delegate corresponding to the <paramref name="element"/>.</returns>
    internal static Transform GetTransform(XElement element) => _constantTransforms[element.Name];

    internal static ConstantExpression ConstantTransform(XElement element)
    {
        var (value, type) = ValueTransform(element);

        return Expression.Constant(value, type);
    }

    internal static (object?, Type) ValueTransform(XElement element)
    {
        var type = GetType(element);
        var transform = GetTransform(element);
        return (transform(element, type), type);
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
        var vElement = element.FirstChild();
        var typeElement = GetType(vElement);
        var value = ValueTransform(vElement);

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
            throw new SerializationException($"Cannot transform {element.Value} to {type.FullName} value.", ex);
        }
        catch (OverflowException ex)
        {
            throw new SerializationException($"Cannot transform {element.Value} to {type.FullName} value.", ex);
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
                                .Where(e => paramName == (e.Attribute(AttributeNames.Name)?.Value ?? throw new SerializationException($"Expected attribute with name {paramName}.")))
                                .First()
                                .FirstChild()
                                ;

            if (propElement is not null)
            {
                var (v, _) = ValueTransform(propElement);
                parameters[i] = v;
            }
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
                        .Select(e => GetTransform(e)(e, GetType(e)))
                        ;

        if (type.IsArray)
            return TransformToArray(elementType, length, elements);

        if (!type.IsGenericType)
            throw new SerializationException($"The collection in {element.Name} must be either array or a generic collection.");

        var genericType = type.Name.EndsWith("FrozenSet`1") ? typeof(FrozenSet<>) : type.GetGenericTypeDefinition();

        if (_sequenceBuilders.TryGetValue(genericType, out var seqTransform))
            return seqTransform(genericType, elementType, length, elements);

        throw new SerializationException($"Don't know how to deserialize {type.FullName}.");
    }

    static (IDictionary, Type[], Func<IDictionary, object?>) PrepForDictionary(Type dictType)
    {
        if (!dictType.IsAssignableTo(typeof(IDictionary)))
            throw new InternalTransformErrorException($"The type of the element is not 'IDictionary'.");

        Type[] kvTypes;
        Type? genericType = null;

        if (dictType.IsGenericType)
        {
            kvTypes = dictType.GetGenericArguments();
            genericType = dictType.GetGenericTypeDefinition() ?? throw new InternalTransformErrorException();
        }
        else
        if (dictType == typeof(Hashtable))
            kvTypes = [typeof(object), typeof(object)];
        else
            throw new InternalTransformErrorException($"Don't know how to deserialize {dictType}.");

        if (kvTypes.Length is not 2 and not 0)
            throw new InternalTransformErrorException("The elements of 'Dictionary' do not have key-type and element-type.");

        IDictionary dict;
        Func<IDictionary, object?> convert;

        if (dictType == typeof(Hashtable))
        {
            dict = new Hashtable();
            convert = d => d;

            return (dict, kvTypes, convert);
        }

        if (genericType == typeof(Dictionary<,>))
        {
            dict = Activator.CreateInstance(typeof(Dictionary<,>)
                                .MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary ?? throw new InternalTransformErrorException($"Could not create object of type Dictionary<{kvTypes[0].Name},{kvTypes[1].Name}>.");
            convert = d => d;
            return (dict, kvTypes, convert);
        }

        if (genericType == typeof(ReadOnlyDictionary<,>))
        {
            dict = Activator.CreateInstance(typeof(Dictionary<,>)
                                .MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary ?? throw new InternalTransformErrorException($"Could not create object of type Dictionary<{kvTypes[0].Name},{kvTypes[1].Name}>.");

            var ctor = typeof(ReadOnlyDictionary<,>)
                        .MakeGenericType(kvTypes[0], kvTypes[1])
                        .GetConstructors()
                        .Where(ci => ci.GetParameters().Length == 1)
                        .Single()
                        ;
            convert = d => ctor!.Invoke([d]) as IDictionary
                        ?? throw new InternalTransformErrorException($"Could not create object of type ConcurrentDictionary<{kvTypes[0].Name},{kvTypes[1].Name}>.");

            return (dict, kvTypes, convert);
        }

        if (genericType == typeof(SortedDictionary<,>))
        {
            dict = Activator.CreateInstance(typeof(SortedDictionary<,>).MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary ?? throw new InternalTransformErrorException($"Could not create object of type SortedDictionary<{kvTypes[0].Name},{kvTypes[1].Name}>.");
            convert = d => d;
            return (dict, kvTypes, convert);
        }

        if (genericType == typeof(ImmutableDictionary<,>))
        {
            dict = Activator.CreateInstance(typeof(Dictionary<,>)
                                .MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary ?? throw new InternalTransformErrorException($"Could not create object of type Dictionary<{kvTypes[0].Name},{kvTypes[1].Name}>.");
            convert = d => _toImmutableDictionary.MakeGenericMethod(kvTypes).Invoke(null, [d]);
            return (dict, kvTypes, convert);
        }

        if (genericType == typeof(ImmutableSortedDictionary<,>))
        {
            dict = Activator.CreateInstance(typeof(SortedDictionary<,>)
                                .MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary ?? throw new InternalTransformErrorException($"Could not create object of type SortedDictionary<{kvTypes[0].Name},{kvTypes[1].Name}>.");
            convert = d => _toImmutableSortedDictionary.MakeGenericMethod(kvTypes).Invoke(null, [d]);
            return (dict, kvTypes, convert);
        }

        if (dictType.Name == typeof(ConcurrentDictionary<,>).Name)
        {
            Debug.Assert(genericType is not null);

            var ctor = genericType
                        .MakeGenericType(kvTypes[0], kvTypes[1])
                        .GetConstructors()
                        .Where(ci => ci.GetParameters().Length == 0)
                        .Single()
                        ;
            dict = ctor!.Invoke([]) as IDictionary ?? throw new InternalTransformErrorException($"Could not create object of type ConcurrentDictionary<{kvTypes[0].Name},{kvTypes[1].Name}>.");
            convert = d => d;
            return (dict, kvTypes, convert);
        }

        if (dictType.Name.EndsWith("FrozenDictionary`2"))
        {
            dict = Activator.CreateInstance(typeof(Dictionary<,>)
                                .MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary ?? throw new InternalTransformErrorException($"Could not create object of type Dictionary<{kvTypes[0].Name},{kvTypes[1].Name}>.");
            convert = d => _toFrozenDictionary.MakeGenericMethod(kvTypes).Invoke(null, [d, null]);
            return (dict, kvTypes, convert);
        }

        throw new InternalTransformErrorException($"Don't know how to deserialize {dictType}.");
    }

    static object? TransformDictionary(
        XElement element,
        Type type)
    {
        var (dict, kvTypes, conversionMethod) = PrepForDictionary(type);
        var keyType = kvTypes[0];
        var valType = kvTypes[1];

        foreach (var kvElement in element.Elements(ElementNames.KeyValuePair))
        {
            var keyElement = kvElement.FirstChild();
            var valElement = kvElement.Elements().LastOrDefault();

            if (keyElement is null || valElement is null)
                throw new SerializationException($"Could not find a key-value pair in {element.Name}.");

            var (key, kt) = ValueTransform(keyElement);

            if (key is null)
                throw new SerializationException($"Could not transform a value of a key in {element.Name}.");
            if (!kt.IsAssignableTo(keyType))
                throw new SerializationException($"Invalid type of a key in {element.Name}.");

            var (value, vt) = ValueTransform(valElement);

            if (!vt.IsAssignableTo(valType))
                throw new SerializationException($"Invalid type of a value in {element.Name}.");

            dict[key] = value;
        }

        return conversionMethod(dict);
    }

    static object? TransformTuple(
        XElement element,
        Type type)
    {
        var parameters = element
                            .Elements(ElementNames.TupleItem)
                            .Select(
                                e =>
                                {
                                    var (i, _) = ValueTransform(e.FirstChild());
                                    return i;
                                })
                            .ToArray()
                            ;
        return Activator.CreateInstance(type, parameters);
    }

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
}
