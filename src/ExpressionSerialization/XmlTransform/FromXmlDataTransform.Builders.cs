namespace vm2.ExpressionSerialization.XmlTransform;

using vm2.ExpressionSerialization.Utilities;

partial class FromXmlDataTransform
{
    static object BuildWithConstructor(
        Type genericType,
        Type elementType,
        IEnumerable elements)
    {
        var ctor = genericType
                        .MakeGenericType(elementType)
                        .GetConstructors()
                        .Where(ci => ci.ConstructorHas1EnumerableParameter())
                        .Single()
                        ;

        var collection = CastSequence(elements, elementType);

        return ctor!.Invoke([collection]);
    }

    static object BuildWithConstructor1ListParameter(
        Type genericType,
        Type elementType,
        IEnumerable elements)
    {
        var ctor = genericType
                        .MakeGenericType(elementType)
                        .GetConstructors()
                        .Where(ci => ci.ConstructorHas1EnumerableParameter())
                        .Single()
                        ;

        var collection = _toList.MakeGenericMethod(elementType).Invoke(null, [CastSequence(elements, elementType)]);

        return ctor!.Invoke([collection]);
    }

    static object BuildConcurrentBag(
        Type genericType,
        Type elementType,
        IEnumerable elements)
    {
        var ctor = genericType
                        .MakeGenericType(elementType)
                        .GetConstructors()
                        .Where(ci => ci.ConstructorHas1EnumerableParameter())
                        .Single()
                        ;

        var collection = CastSequence(elements, elementType);

        collection = _reverse.MakeGenericMethod(elementType).Invoke(null, [collection]);

        return ctor!.Invoke([collection]);
    }
}
