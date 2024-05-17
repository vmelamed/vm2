namespace vm2.ExpressionSerialization.XmlTransform;

using System;
using System.Collections.Generic;

static partial class FromXmlDataTransform
{
    delegate object? Transformation(XElement element, ref Type type);

    /// <summary>
    /// Gets the constant value XML to .NET transform delegate corresponding to the XML <paramref name="element"/>.
    /// </summary>
    /// <param name="element">The element which holds transformed constant value.</param>
    /// <returns>The transforming delegate corresponding to the <paramref name="element"/>.</returns>
    static Transformation GetTransformation(XElement element) => _constantTransformations[element.Name.LocalName];

    internal static ConstantExpression ConstantTransform(XElement element)
    {
        var (value, type) = ValueTransform(element);

        return Expression.Constant(value, type);
    }

    static (object?, Type) ValueTransform(XElement element)
    {
        var type = element.GetEType();

        if (type == typeof(void))
            throw new SerializationException($"Got 'void' type of constant data in the element `{element.Name}`");

        return (GetTransformation(element)(element, ref type), type);
    }

    static object? TransformNullable(
        XElement element,
        ref Type type)
    {
        if (element.IsNil())
        {
            var valTypeName = element.Attribute(AttributeNames.Type)?.Value;

            if (!Transform.NamesToTypes.TryGetValue(valTypeName!, out var valType) && valType != typeof(Enum))
                valType = valTypeName is not null
                                    ? (Type.GetType(valTypeName) ?? throw new SerializationException($"Could not resolve the type name `{valTypeName}` specified in element `{element.Name}`."))
                                    : throw new SerializationException($"If a nullable type value is null, the attribute 'type' of the nullable element is mandatory.");

            if (type == typeof(void))
                throw new SerializationException($"Constant expression of 'Nullable<>' type specified with 'void' value: `{element.Name}`.");

            type = typeof(Nullable<>).MakeGenericType(valType);
            return null;
        }

        // we do not need to return Nullable<T> here. Since the return type is object? the CLR will either return null or the boxed value of the Nullable<T>
        var vElement = element.GetChild();
        var (value, valueType) = ValueTransform(vElement);

        if (valueType == typeof(void))
            throw new SerializationException($"Constant expression of 'Nullable<>' type specified with 'void' value: `{element.Name}`.");

        type = typeof(Nullable<>).MakeGenericType(valueType);

        var ctor = type.GetConstructor([valueType])
                            ?? throw new InternalTransformErrorException($"Could not get the constructor for Nullable<{valueType.Name}>");

        return ctor.Invoke([value]);
    }

    static object? TransformEnum(
        XElement element,
        ref Type type)
    {
        try
        {
            var typeName = element.Attribute(AttributeNames.Type)?.Value
                                        ?? throw new ArgumentNullException($"Could not get the full name of the enum type at `{element.Name}`");
            type = Type.GetType(typeName) ?? throw new ArgumentNullException($"Could not get the enum type at `{element.Name}`");

            return Enum.Parse(type, element.Value);
        }
        catch (ArgumentException ex)
        {
            throw new SerializationException($"Cannot transform `{element.Value}` to `{type.FullName}` value.", ex);
        }
        catch (OverflowException ex)
        {
            throw new SerializationException($"Cannot transform `{element.Value}` to `{type.FullName}` value.", ex);
        }
    }

    static object? TransformObject(
        XElement element,
        ref Type type)
    {
        if (element.TryGetETypeFromAttribute(out var t))
            type = t!;

        var concreteTypeName = element.Attribute(AttributeNames.ConcreteType)?.Value;
        var concreteType = string.IsNullOrEmpty(concreteTypeName) ? type : Type.GetType(concreteTypeName);

        if (element.IsNil())
            return null;

        if (concreteType == typeof(object))
            return new();

        if (concreteType is null)
            throw new SerializationException($"Unknown type `{concreteType}`");

        var serializer = new DataContractSerializer(concreteType);
        using var reader = element.GetChild().CreateReader();
        return serializer.ReadObject(reader);
    }

    static object? TransformAnonymous(
        XElement element,
        ref Type type)
    {
        var constructor = type.GetConstructors()[0];
        var constructorParameters = constructor.GetParameters();

        if (element.Elements(ElementNames.Property).Count() != constructorParameters.Length)
            throw new SerializationException("The number of properties and the number of initialization parameters do not match for anonymous type.");

        var parameters = new object?[constructorParameters.Length];

        for (var i = 0; i < constructorParameters.Length; i++)
        {
            var paramName = constructorParameters[i].Name;
            var propElement = element
                                .Elements(ElementNames.Property)
                                .Where(e => paramName == (e.Attribute(AttributeNames.Name)?.Value ?? throw new SerializationException($"Expected attribute with name `{paramName}`.")))
                                .First()
                                .GetChild()
                                ;

            if (propElement is not null)
            {
                var (v, _) = ValueTransform(propElement);
                parameters[i] = v;
            }
        }

        return constructor.Invoke(parameters);
    }

    static object? TransformTuple(
        XElement element,
        ref Type type)
    {
        var parameters = element
                            .Elements(ElementNames.TupleItem)
                            .Select(
                                e =>
                                {
                                    var (i, _) = ValueTransform(e.GetChild());
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

    static object? TransformByteSequence(
        XElement element,
        ref Type type)
    {
        var bytes = Convert.FromBase64String(element.Value);

        if (element.TryGetLength(out var length) && length != bytes.Length)
            throw new SerializationException($"The actual length of byte sequence is different from the one specified in the element `{element.Name}`.");

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

        throw new SerializationException($"Unexpected type of element `{element.Name}`.");
    }

    static object? TransformCollection(
        XElement element,
        ref Type type)
    {
        int length = element.Elements().Count();

        if (element.TryGetLength(out var len) && len != length)
            throw new SerializationException($"The actual length of a collection is different from the one specified in the element `{element.Name}`.");

        Type elementType = type.IsArray
                                ? type.GetElementType()
                                    ?? throw new SerializationException($"Could not get the type of the array elements in the XML element `{element.Name}`.")
                                : type.IsGenericType
                                    ? type.GetGenericArguments()[0]
                                    : throw new SerializationException($"Could not get the type of the array elements in the XML element `{element.Name}`.");

        if (elementType == typeof(void))
            throw new SerializationException($"Constant expression's type specified as type 'void' `{element.Name}`.");

        var elements = element
                        .Elements()
                        .Select(e =>
                        {
                            var t = e.GetEType();
                            return GetTransformation(e)(e, ref t);
                        })
                        ;

        if (type.IsArray)
            return TransformToArray(elementType, length, elements);

        if (!type.IsGenericType)
            throw new SerializationException($"The collection in `{element.Name}` must be either array or a generic collection.");

        var genericType = type.Name.EndsWith("FrozenSet`1")     // TODO: this is pretty wonky but I don't know how to detect the internal "SmallValueTypeComparableFrozenSet`1" or "SmallFrozenSet`1"
                                ? typeof(FrozenSet<>)
                                : type.GetGenericTypeDefinition();

        if (_sequenceBuilders.TryGetValue(genericType, out var seqTransform))
            return seqTransform(genericType, elementType, length, elements);

        throw new SerializationException($"Don't know how to deserialize `{type.FullName}`.");
    }

    delegate (IDictionary, Type[], Func<IDictionary, object?>) PrepForDict(Type[] kvTypes);

    static (IDictionary, Type[], Func<IDictionary, object?>) PrepForDictionary(Type[] kvTypes)
    {
        var dict = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type `Dictionary<{kvTypes[0].Name},{kvTypes[1].Name}>`.");
        return (dict, kvTypes, d => d);
    }

    static (IDictionary, Type[], Func<IDictionary, object?>) PrepForReadOnlyDictionary(Type[] kvTypes)
    {
        var dict = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type `Dictionary<{kvTypes[0].Name},{kvTypes[1].Name}>`.");
        var ctor = typeof(ReadOnlyDictionary<,>)
                        .MakeGenericType(kvTypes[0], kvTypes[1])
                        .GetConstructors()
                        .Where(ci => ci.GetParameters().Length == 1)
                        .Single()
                    ;
        object? convert(IDictionary d) => (ctor!.Invoke([d]) as IDictionary)
                                                    ?? throw new InternalTransformErrorException($"Could not create object of type `ReadOnlyDictionary<{kvTypes[0].Name},{kvTypes[1].Name}>`.");

        return (dict, kvTypes, convert);
    }

    static (IDictionary, Type[], Func<IDictionary, object?>) PrepForSortedDictionary(Type[] kvTypes)
    {
        var dict = Activator.CreateInstance(typeof(SortedDictionary<,>).MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type `SortedDictionary<{kvTypes[0].Name},{kvTypes[1].Name}>`.");

        return (dict, kvTypes, d => d);
    }

    static (IDictionary, Type[], Func<IDictionary, object?>) PrepForImmutableDictionary(Type[] kvTypes)
    {
        var dict = Activator.CreateInstance(typeof(Dictionary<,>)
                            .MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type `Dictionary<{kvTypes[0].Name},{kvTypes[1].Name}>`.");
        return (dict, kvTypes, d => _toImmutableDictionary.MakeGenericMethod(kvTypes).Invoke(null, [d]));
    }

    static (IDictionary, Type[], Func<IDictionary, object?>) PrepForImmutableSortedDictionary(Type[] kvTypes)
    {
        var dict = Activator.CreateInstance(typeof(SortedDictionary<,>)
                            .MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type `SortedDictionary<{kvTypes[0].Name},{kvTypes[1].Name}>`.");
        return (dict, kvTypes, d => _toImmutableSortedDictionary.MakeGenericMethod(kvTypes).Invoke(null, [d]));
    }

    static (IDictionary, Type[], Func<IDictionary, object?>) PrepForFrozenDictionary(Type[] kvTypes)
    {
        var dict = Activator.CreateInstance(typeof(Dictionary<,>)
                            .MakeGenericType(kvTypes[0], kvTypes[1])) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type `Dictionary<{kvTypes[0].Name},{kvTypes[1].Name}>`.");
        return (dict, kvTypes, d => _toFrozenDictionary.MakeGenericMethod(kvTypes).Invoke(null, [d, null]));
    }

    static (IDictionary, Type[], Func<IDictionary, object?>) PrepForConcurrentDictionary(Type[] kvTypes)
    {
        var ctor = typeof(ConcurrentDictionary<,>)
                        .MakeGenericType(kvTypes[0], kvTypes[1])
                        .GetConstructors()
                        .Where(ci => ci.GetParameters().Length == 0)
                        .Single()
                    ;
        var dict = ctor!.Invoke([]) as IDictionary
                                    ?? throw new InternalTransformErrorException($"Could not create object of type `ConcurrentDictionary<{kvTypes[0].Name},{kvTypes[1].Name}>`.");
        return (dict, kvTypes, d => d);
    }

    static Dictionary<Type, PrepForDict> _typeToPrep_ = new()
    {
        [typeof(Hashtable)]                    = kvTypes => (new Hashtable(), kvTypes, d => d),
        [typeof(Dictionary<,>)]                = PrepForDictionary,
        [typeof(ReadOnlyDictionary<,>)]        = PrepForReadOnlyDictionary,
        [typeof(SortedDictionary<,>)]          = PrepForSortedDictionary,
        [typeof(ImmutableDictionary<,>)]       = PrepForImmutableDictionary,
        [typeof(ImmutableSortedDictionary<,>)] = PrepForImmutableSortedDictionary,
        [typeof(FrozenDictionary<,>)]          = PrepForFrozenDictionary,
        [typeof(ConcurrentDictionary<,>)]      = PrepForConcurrentDictionary,
    };
    static FrozenDictionary<Type, PrepForDict> _typeToPrep = _typeToPrep_.ToFrozenDictionary();

    static (IDictionary, Type[], Func<IDictionary, object?>) PrepForDictionary(Type dictType)
    {
        if (!dictType.IsAssignableTo(typeof(IDictionary)))
            throw new InternalTransformErrorException($"The type of the element is not `IDictionary`.");

        Type[] kvTypes;
        Type genericType = typeof(Hashtable);

        if (dictType.IsGenericType)
        {
            kvTypes = dictType.GetGenericArguments();
            genericType = dictType.GetGenericTypeDefinition() ?? throw new InternalTransformErrorException($"Could not get the generic type definition of a generic type `{dictType.FullName}`.");
        }
        else
        if (dictType == typeof(Hashtable))
            kvTypes = [typeof(object), typeof(object)];
        else
            throw new InternalTransformErrorException($"Don't know how to deserialize `{dictType}`.");

        if (kvTypes.Length is not 2 and not 0)
            throw new InternalTransformErrorException("The elements of 'Dictionary' do not have key-type and element-type.");

        if (_typeToPrep.TryGetValue(genericType, out var prep))
            return prep(kvTypes);
        else
        if (dictType.Name.EndsWith("FrozenDictionary`2"))   // TODO: this is pretty wonky but I don't know how to detect the internal "SmallValueTypeComparableFrozenDictionary`1" or "SmallFrozenDictionary`1"
            return PrepForFrozenDictionary(kvTypes);

        throw new InternalTransformErrorException($"Don't know how to deserialize `{dictType}`.");
    }

    static object? TransformDictionary(
        XElement element,
        ref Type type)
    {
        var (dict, kvTypes, convertToFinal) = PrepForDictionary(type);

        foreach (var kvElement in element.Elements(ElementNames.KeyValuePair))
        {
            var keyElement = kvElement.GetChild();
            var valElement = kvElement.Elements().LastOrDefault();

            if (keyElement is null || valElement is null)
                throw new SerializationException($"Could not find a key-value pair in `{element.Name}`.");

            var (key, kt) = ValueTransform(keyElement);

            if (key is null)
                throw new SerializationException($"Could not transform a value of a key in `{element.Name}`.");
            if (!kt.IsAssignableTo(kvTypes[0]))
                throw new SerializationException($"Invalid type of a key in `{element.Name}`.");

            var (value, vt) = ValueTransform(valElement);

            if (!vt.IsAssignableTo(kvTypes[1]))
                throw new SerializationException($"Invalid type of a value in `{element.Name}`.");

            dict[key] = value;
        }

        return convertToFinal(dict);
    }
}
