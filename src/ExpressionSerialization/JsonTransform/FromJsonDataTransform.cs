﻿namespace vm2.ExpressionSerialization.JsonTransform;
static partial class FromJsonDataTransform
{
    /// <summary>
    /// Transforms the element to a <see cref="ConstantExpression"/> object.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>ConstantExpression.</returns>
    internal static ConstantExpression ConstantTransform(JElement element)
    {
        var (value, type) = ValueTransform(element.GetOneOf(ConstantTypes));

        return Expression.Constant(value, type);
    }

    delegate object? Transformation(JElement element, ref Type type);

    static (object?, Type) ValueTransform(JElement element)
    {
        var type = element.GetType();

        if (type == typeof(void))
            throw new SerializationException($"Got 'void' type of constant data in the element '{element.Name}'");

        var transform = GetTransformation(element);
        return (transform(element, ref type), type);
    }

    static Transformation GetTransformation(JElement element)
        => _constantTransformations.TryGetValue(element.Name, out var transform)
                ? transform
                : throw new SerializationException($"Error deserializing and converting to a strong type the value of the element '{element.Name}'.");

    static object? TransformEnum(
        JElement element,
        ref Type type)
    {
        try
        {
            type = element.GetTypeFromProperty();

            var value = element.GetChild(Vocabulary.Value).Value;

            if (value is not null)
            {
                if (value.GetValueKind() == JsonValueKind.String)
                    return Enum.Parse(type, value.AsValue().GetValue<string>());

                if (value.GetValueKind() == JsonValueKind.Array &&
                    value.AsArray().All(n => n?.GetValueKind() == JsonValueKind.String))
                    return Enum.Parse(type, string.Join(", ", value.AsArray().Select(n => n?.GetValue<string>())));
            }
        }
        catch (ArgumentException ex)
        {
            throw new SerializationException($"Cannot transform '{element.Value}' to '{type.FullName}' valueElement.", ex);
        }
        catch (OverflowException ex)
        {
            throw new SerializationException($"Cannot transform '{element.Value}' to '{type.FullName}' valueElement.", ex);
        }

        throw new SerializationException($"Could not convert the valueElement of property {element.Name} to '{type.FullName}'");
    }

    static object? TransformNullable(
        JElement element,
        ref Type type)
    {
        object? value = null;
        Type? underlyingType = null;

        if (element.IsNil())
        {
            underlyingType = element.GetTypeFromProperty();

            if (underlyingType == typeof(void))
                throw new SerializationException($"Constant expression of 'Nullable<>' type specified with 'void' type: '{element.Name}'.");

            type = typeof(Nullable<>).MakeGenericType(underlyingType);
            return value;
        }

        var vElement = element.GetOneOf(ConstantTypes);

        (value, underlyingType) = ValueTransform(vElement);

        if (underlyingType == typeof(void))
            throw new SerializationException($"Constant expression of 'Nullable<>' type specified with 'void' valueElement: '{element.Name}'.");

        type = typeof(Nullable<>).MakeGenericType(underlyingType);

        var ctor = type.GetConstructor([underlyingType])
                            ?? throw new InternalTransformErrorException($"Could not get the constructor for Nullable<{underlyingType.Name}>");

        return ctor.Invoke([value]);
    }

    static object? TransformObject(
        JElement element,
        ref Type type)
    {
        // get the expression type
        type = element.GetTypeFromProperty();

        // get the concrete type but do not change the expression type
        if (!element.TryGetTypeFromProperty(out var concreteType, Vocabulary.ConcreteType) || concreteType is null)
            concreteType = type;   // the element type IS the concrete type

        if (concreteType is null)
            throw new SerializationException($"Unknown type at the element {element.Name}.");

        var valueElement = element.GetChild(Vocabulary.Value);

        if (valueElement.IsNil())
            return null;

        if (concreteType == typeof(object))
            return new();

        return JsonSerializer.Deserialize(valueElement.Value, concreteType);
    }

    static object? TransformAnonymous(
        JElement element,
        ref Type type)
    {
        if (element.IsNil())
            return null;

        type = element.GetTypeFromProperty();

        var constructor = type.GetConstructors()[0];
        var constructorParameters = constructor.GetParameters();
        var valueElement = element.GetChild(Vocabulary.Value);

        if (valueElement.Value is null || valueElement.Value.GetValueKind() != JsonValueKind.Object)
            throw new SerializationException($"Invalid value of element {element.Name}.");

        var value = valueElement.Value.AsObject();

        if (value.Count != constructorParameters.Length)
            throw new SerializationException("The number of properties and the number of initialization parameters do not match for anonymous type.");

        var parameters = new object?[constructorParameters.Length];

        for (var i = 0; i < constructorParameters.Length; i++)
        {
            var paramName = constructorParameters[i].Name ?? "";
            var propElement = valueElement.GetChild(paramName).GetOneOf(ConstantTypes);
            parameters[i] = ValueTransform(propElement).Item1;
        }

        return constructor.Invoke(parameters);
    }

    static object? TransformTuple(
        JElement element,
        ref Type type)
        => Activator.CreateInstance(
                    type,
                    element
                        .GetChild(Vocabulary.Value)
                        .Value!
                        .AsObject()
                        .Where(kvp => kvp.Value is JsonObject)
                        .Select(kvp => ValueTransform(new JElement(kvp).GetOneOf(ConstantTypes)).Item1)
                        .ToArray());

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

    static object? TransformByteSequence(
        JElement element,
        ref Type type)
    {
        if (element.IsNil())
            return null;

        var bytes = Convert.FromBase64String(element.Value?.GetValue<string>() ?? throw new SerializationException($"Could not find the Base64 string representation of the value for property {element.Name}"));

        if (element.TryGetLength(out var length) && length != bytes.Length)
            throw new SerializationException($"The actual length of byte sequence is different from the one specified in the element '{element.Name}'.");

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

        throw new SerializationException($"Unexpected type of element '{element.Name}'.");
    }

    static object? TransformCollection(
        JElement element,
        ref Type type)
    {
        if (element.IsNil())
            return null;

        var collectionObj = element.GetChild(Vocabulary.Value);
        var jArray = collectionObj.Value?.AsArray() ?? throw new SerializationException($"Could not get the array object at the property '{element.Name}'.");
        int length = jArray.Count;

        if (element.TryGetLength(out var len) && len != length)
            throw new SerializationException($"The actual length of a collection is different from the one specified in the element '{element.Name}'.");

        Type elementType = type.IsArray
                                ? type.GetElementType() ?? throw new SerializationException($"Could not get the type of the array elements in the property '{element.Name}'.")
                                : type.IsGenericType
                                    ? type.GetGenericArguments()[0]
                                    : throw new SerializationException($"Could not get the type of the array elements in the property '{element.Name}'.");

        if (elementType == typeof(void))
            throw new SerializationException($"Constant expression's type specified as type 'void' '{element.Name}'.");

        var elements = jArray
                        .Select(
                            (e, i) =>
                            {
                                var (elem, t) = ValueTransform(new JElement($"Item{i}", e));

                                if (!elementType.IsAssignableFrom(t))
                                    throw new SerializationException($"The actual type of the element at {element.Name}/{Vocabulary.Value}/[{i}] is not compatible with the array element type {elementType.FullName}");

                                return elem;
                            });

        // TODO: this is pretty wonky but I don't know how to detect the internal "SmallValueTypeComparableFrozenSet'1" or "SmallFrozenSet'1"
        var genericType = type.Name.EndsWith("FrozenSet'1")
                                ? typeof(FrozenSet<>)
                                : type.GetGenericTypeDefinition();

        if (_sequenceBuilders.TryGetValue(genericType, out var seqTransform))
            return seqTransform(genericType, elementType, length, elements);

        throw new SerializationException($"Don't know how to deserialize '{type.FullName}'.");
    }

    readonly struct DictBuildData(IDictionary dictionary, Type[] keyValueTypes, Func<IDictionary, object?> convert)
    {
        /// <summary>
        /// Gets or sets the initial dictionary to build.
        /// </summary>
        public IDictionary Dictionary { get; } = dictionary;

        /// <summary>
        /// Gets or sets the types of the key and the value in the dictionary.
        /// </summary>
        public Type[] KeyValueTypes { get; } = keyValueTypes;

        /// <summary>
        /// Converts the <see cref="Dictionary"/> to its final dictionary type, incl. <see cref="Hashtable"/>
        /// </summary>
        public Func<IDictionary, object?> ConvertToTargetType { get; } = convert;

        public void Deconstruct(out IDictionary dictionary, out Type[] keyValueTypes, out Func<IDictionary, object?> convert)
        {
            dictionary = Dictionary;
            keyValueTypes = KeyValueTypes;
            convert = ConvertToTargetType;
        }
    }

    delegate DictBuildData PrepForDict(Type[] kvTypes);

    static DictBuildData PrepForDictionary(Type[] kvTypes)
    {
        var dict = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type 'Dictionary<{kvTypes[0].Name},{kvTypes[1].Name}>'.");
        return new(dict, kvTypes, d => d);
    }

    static DictBuildData PrepForReadOnlyDictionary(Type[] kvTypes)
    {
        var dict = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type 'Dictionary<{kvTypes[0].Name},{kvTypes[1].Name}>'.");
        var ctor = typeof(ReadOnlyDictionary<,>)
                        .MakeGenericType(kvTypes[0], kvTypes[1])
                        .GetConstructors()
                        .Where(ci => ci.GetParameters().Length == 1)
                        .Single()
                    ;
        object? convert(IDictionary d) => (ctor!.Invoke([d]) as IDictionary)
                                                    ?? throw new InternalTransformErrorException($"Could not create object of type 'ReadOnlyDictionary<{kvTypes[0].Name},{kvTypes[1].Name}>'.");

        return new(dict, kvTypes, convert);
    }

    static DictBuildData PrepForSortedDictionary(Type[] kvTypes)
    {
        var dict = Activator.CreateInstance(typeof(SortedDictionary<,>).MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type 'SortedDictionary<{kvTypes[0].Name},{kvTypes[1].Name}>'.");

        return new(dict, kvTypes, d => d);
    }

    static DictBuildData PrepForImmutableDictionary(Type[] kvTypes)
    {
        var dict = Activator.CreateInstance(typeof(Dictionary<,>)
                            .MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type 'Dictionary<{kvTypes[0].Name},{kvTypes[1].Name}>'.");
        return new(dict, kvTypes, d => _toImmutableDictionary.MakeGenericMethod(kvTypes).Invoke(null, [d]));
    }

    static DictBuildData PrepForImmutableSortedDictionary(Type[] kvTypes)
    {
        var dict = Activator.CreateInstance(typeof(SortedDictionary<,>)
                            .MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type 'SortedDictionary<{kvTypes[0].Name},{kvTypes[1].Name}>'.");
        return new(dict, kvTypes, d => _toImmutableSortedDictionary.MakeGenericMethod(kvTypes).Invoke(null, [d]));
    }

    static DictBuildData PrepForFrozenDictionary(Type[] kvTypes)
    {
        var dict = Activator.CreateInstance(typeof(Dictionary<,>)
                            .MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type 'Dictionary<{kvTypes[0].Name},{kvTypes[1].Name}>'.");
        return new(dict, kvTypes, d => _toFrozenDictionary.MakeGenericMethod(kvTypes).Invoke(null, [d, null]));
    }

    static DictBuildData PrepForConcurrentDictionary(Type[] kvTypes)
    {
        var ctor = typeof(ConcurrentDictionary<,>)
                        .MakeGenericType(kvTypes[0], kvTypes[1])
                        .GetConstructors()
                        .Where(ci => ci.GetParameters().Length == 0)
                        .Single()
                    ;
        var dict = ctor!.Invoke([]) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type 'ConcurrentDictionary<{kvTypes[0].Name},{kvTypes[1].Name}>'.");
        return new(dict, kvTypes, d => d);
    }

    static IEnumerable<KeyValuePair<Type, PrepForDict>> TypeToPrep()
    {
        yield return new(typeof(Hashtable), kvTypes => new(new Hashtable(), kvTypes, d => d));
        yield return new(typeof(Dictionary<,>), PrepForDictionary);
        yield return new(typeof(ReadOnlyDictionary<,>), PrepForReadOnlyDictionary);
        yield return new(typeof(SortedDictionary<,>), PrepForSortedDictionary);
        yield return new(typeof(ImmutableDictionary<,>), PrepForImmutableDictionary);
        yield return new(typeof(ImmutableSortedDictionary<,>), PrepForImmutableSortedDictionary);
        yield return new(typeof(FrozenDictionary<,>), PrepForFrozenDictionary);
        yield return new(typeof(ConcurrentDictionary<,>), PrepForConcurrentDictionary);
    }

    static FrozenDictionary<Type, PrepForDict> _typeToPrep = TypeToPrep().ToFrozenDictionary();

    static DictBuildData PrepForDictionary(Type dictType)
    {
        if (!dictType.IsAssignableTo(typeof(IDictionary)))
            throw new InternalTransformErrorException($"The type of the element is not 'IDictionary'.");

        Type[] kvTypes;
        Type genericType = typeof(Hashtable);

        if (dictType.IsGenericType)
        {
            kvTypes = dictType.GetGenericArguments();
            genericType = dictType.GetGenericTypeDefinition()
                            ?? throw new InternalTransformErrorException($"Could not get the generic type definition of a generic type '{dictType.FullName}'.");
        }
        else
        if (dictType == typeof(Hashtable))
            kvTypes = [typeof(object), typeof(object)];
        else
            throw new InternalTransformErrorException($"Don't know how to deserialize '{dictType}'.");

        if (kvTypes.Length is not 2)
            throw new InternalTransformErrorException("The elements of 'Dictionary' do not have key-type and element-type.");

        if (_typeToPrep.TryGetValue(genericType, out var prep))
            return prep(kvTypes);
        else
        // TODO: this is pretty wonky but I don't know how to detect the internal "SmallValueTypeComparableFrozenDictionary'1" or "SmallFrozenDictionary'1"
        if (dictType.Name.EndsWith("FrozenDictionary'2"))
            return PrepForFrozenDictionary(kvTypes);

        throw new InternalTransformErrorException($"Don't know how to deserialize '{dictType}'.");
    }

    static object? TransformDictionary(
        JElement element,
        ref Type type)
    {
        if (element.IsNil())
            return null;

        var dictValue = element.GetValue()?.AsArray()
                            ?? throw new SerializationException($"Could not get the dictionary at property '{element.Name}/{Vocabulary.Value}'");

        var (dict, kvTypes, convertToFinal) = PrepForDictionary(type);
        var i = -1;

        foreach (JsonNode? kvElement in dictValue)
        {
            i++;

            var (key, kt) = ValueTransform(
                                new JElement(
                                    Vocabulary.Key,
                                    kvElement?.AsObject().GetChildObject(Vocabulary.Key, $"at '{element.Name}/{Vocabulary.Value}/[{i}]/{Vocabulary.Key}'.")));

            if (key is null)
                throw new SerializationException($"Could not transform a value of a key at '{element.Name}/{Vocabulary.Value}/[{i}]/{Vocabulary.Key}'.");

            if (!kt.IsAssignableTo(kvTypes[0]))
                throw new SerializationException($"Invalid type of a key in '{element.Name}'.");

            var (value, vt) = ValueTransform(
                                new JElement(
                                    Vocabulary.Value,
                                    kvElement?.AsObject().GetChildObject(Vocabulary.Value, $"at '{element.Name}/{Vocabulary.Value}/[{i}]/{Vocabulary.Value}'.")));

            if (!vt.IsAssignableTo(kvTypes[1]))
                throw new SerializationException($"Invalid type of a value in '{element.Name}'.");

            dict[key] = value;
        }

        return convertToFinal(dict);
    }
}
