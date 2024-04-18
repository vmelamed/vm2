﻿namespace vm2.ExpressionSerialization.Utilities;

using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

/// <summary>
/// Class TypeExtensions contains extension methods of <see cref="Type"/>.
/// </summary>
internal static class TypeExtensions
{
    static Type[] _nonPrimitiveBasicTypesCollection =
    [
        typeof(string),
        typeof(Guid),
        typeof(decimal),
        typeof(Uri),
        typeof(DateTime),
        typeof(DateTimeOffset),
        typeof(TimeSpan),
        typeof(DBNull),
        typeof(Half),
    ];
    static FrozenSet<Type> _nonPrimitiveBasicTypes = _nonPrimitiveBasicTypesCollection.ToFrozenSet();

    /// <summary>
    /// Determines whether the specified type is a basic type: primitive, enum, decimal, string, Guid, Uri, DateTime, 
    /// TimeSpan, DateTimeOffset, IntPtr, UIntPtr.
    /// </summary>
    /// <param name="type">The type to be tested.</param>
    /// <returns>
    ///   <see langword="true"/> if the specified type is one of the basic types; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsBasicType(this Type type)
        => type.IsPrimitive ||
           type.IsEnum ||
           _nonPrimitiveBasicTypes.Contains(type);

    const string anonymousTypePrefix = "<>f__AnonymousType";

    /// <summary>
    /// Determines whether the specified type is anonymous.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsAnonymous(this Type type)
        => type.IsGenericType &&
           type.Name.StartsWith(anonymousTypePrefix, StringComparison.Ordinal);

    const string collectionNamespacePrefix = "System.Collections.Generic";

    /// <summary>
    /// Determines whether the specified type is a generic collection.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsSystemCollectionsGeneric(this Type type)
        => type.IsGenericType &&
           type.Namespace?.StartsWith(collectionNamespacePrefix, StringComparison.Ordinal) is true;

    /// <summary>
    /// Determines whether the specified type is a generic nullable.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsNullable(this Type type)
        => type.IsGenericType &&
           type.GetGenericTypeDefinition() == typeof(Nullable<>);

    /// <summary>
    /// Determines whether the specified type is a generic tuple class.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsTupleClass(this Type type)
        => type.ImplementsInterface(typeof(ITuple)) && type.IsClass;

    /// <summary>
    /// Determines whether the specified type is a generic tuple struct.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsTupleValue(this Type type)
        => type.ImplementsInterface(typeof(ITuple)) && type.IsValueType;

    /// <summary>
    /// Determines whether the specified type is a tuple.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsTuple(this Type type)
        => type.IsTupleValue() || type.IsTupleClass();

    /// <summary>
    /// Determines whether the specified type is a span.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsSpan(this Type type)
        => type.IsGenericType &&
           type.GetGenericTypeDefinition() == typeof(Span<>) ||
           type.GetGenericTypeDefinition() == typeof(ReadOnlySpan<>);

    /// <summary>
    /// Determines whether the specified type is a memory{}.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsMemory(this Type type)
        => type.IsGenericType &&
           type.GetGenericTypeDefinition() == typeof(Memory<>) ||
           type.GetGenericTypeDefinition() == typeof(ReadOnlyMemory<>);

    /// <summary>
    /// Determines whether the specified type is a memory{}.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsSlice(this Type type)
        => type.IsGenericType &&
           type.GetGenericTypeDefinition() == typeof(ArraySegment<>);

    public static bool ImplementsInterface(this Type type, Type interfaceType)
        => type.GetInterface(interfaceType.Name) is not null;

    public static bool ImplementsInterface(this Type type, string interfaceName)
        => type.GetInterface(interfaceName) is not null;

    static Type[] _byteSequencesCollection =
    [
        typeof(Memory<byte>),
        typeof(ReadOnlyMemory<byte>),
        typeof(ArraySegment<byte>),
    ];
    static FrozenSet<Type> _byteSequences = _byteSequencesCollection.ToFrozenSet();

    /// <summary>
    /// Determines whether the specified type is a byte sequence: <c>byte[], Span&lt;byte&gt;, ReadOnlySpan&lt;byte&gt;,
    /// Memory&lt;byte&gt;, ReadOnlyMemory&lt;byte&gt;, ArraySegment&lt;byte&gt;</c>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsByteSequence(this Type type)
        => type.IsArray && type.GetElementType() == typeof(byte) || _byteSequences.Contains(type);

    static Type[] _sequencesCollection =
    [
        typeof(ArraySegment<>),
        typeof(Memory<>),
        typeof(ReadOnlyMemory<>),
        typeof(FrozenSet<>),
        typeof(Queue),
        typeof(Stack),
        typeof(ImmutableArray<>),
        typeof(ImmutableHashSet<>),
        typeof(ImmutableList<>),
        typeof(ImmutableQueue<>),
        typeof(ImmutableSortedSet<>),
        typeof(ImmutableStack<>),
        typeof(BlockingCollection<>),
        typeof(ConcurrentBag<>),
        typeof(ConcurrentQueue<>),
        typeof(ConcurrentStack<>),
        typeof(Collection<>),
        typeof(ReadOnlyCollection<>),
        typeof(HashSet<>),
        typeof(LinkedList<>),
        typeof(List<>),
        typeof(Queue<>),
        typeof(SortedSet<>),
        typeof(Stack<>),
    ];
    static FrozenSet<Type> _sequences = _sequencesCollection.ToFrozenSet();

    /// <summary>
    /// Determines whether the specified type is a sequence of objects: array, list, set, etc..
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsSequence(this Type type)
        => type.IsArray ||
           type.IsGenericType && _sequences.Contains(type.GetGenericTypeDefinition()) ||
           _sequences.Contains(type);

    /// <summary>
    /// Determines whether the specified type is a span or memory{}.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsSliceSpanOrMemory(this Type type)
        => type.IsGenericType &&
           type.GetGenericTypeDefinition() == typeof(ArraySegment<>) ||
           type.GetGenericTypeDefinition() == typeof(Memory<>) ||
           type.GetGenericTypeDefinition() == typeof(ReadOnlyMemory<>) ||
           type.GetGenericTypeDefinition() == typeof(Span<>) ||
           type.GetGenericTypeDefinition() == typeof(ReadOnlySpan<>);

    static Type[] _xmlSerializableAttributesCollection = [
            typeof(XmlRootAttribute),
            typeof(XmlTypeAttribute),
            typeof(XmlElementAttribute),
            typeof(XmlArrayAttribute),
            typeof(XmlArrayItemAttribute),
            typeof(SerializableAttribute),
            typeof(DataContractAttribute),
            typeof(MessageContractAttribute),
            typeof(CollectionDataContractAttribute),
        ];
    static FrozenSet<Type> _xmlSerializableAttributes = _xmlSerializableAttributesCollection.ToFrozenSet();

    /// <summary>
    /// Determines whether a value of type <paramref name="type"/> can be serialized.
    /// </summary>
    /// <remarks>
    /// A type is serializable if it is:
    /// <list type="bullet">
    ///     <item>primitive (<see cref="char"/>, <see cref="byte"/>, <see cref="int"/>, <see cref="long"/>, etc.)</item>
    ///     <item><see cref="Enum"/></item>
    ///     <item><see cref="DBNull"/></item>
    ///     <item><see cref="decimal"/></item>
    ///     <item><see cref="string"/></item>
    ///     <item><see cref="Guid"/></item>
    ///     <item><see cref="Uri"/></item>
    ///     <item><see cref="DateTime"/></item>
    ///     <item><see cref="TimeSpan"/></item>
    ///     <item><see cref="DateTimeOffset"/></item>
    ///     <item><see cref="IntPtr"/></item>
    ///     <item><see cref="UIntPtr"/></item>
    ///     <item>Anonymous type with transformable properties</item>
    ///     <item><see cref="Nullable{T}"/> of transformable type</item>
    ///     <item><see cref="Tuple{T}"/> of transformable type</item>
    ///     <item><see cref="ValueTuple{T}"/> of transformable type</item>
    ///     <item><see cref="Span{T}"/> of transformable type</item>
    ///     <item><see cref="ReadOnlySpan{T}"/> of transformable type</item>
    ///     <item><see cref="Memory{T}"/> of transformable type</item>
    ///     <item><see cref="ReadOnlyMemory{T}"/> of transformable type</item>
    ///     <item><see cref="IEnumerable{T}"/> from the System.Collections namespace of transformable type</item>
    ///     <item>The type implements <see cref="ISerializable"/>.</item>
    ///     <item>The type is marked with one of the following attributes:
    ///     <see cref="SerializableAttribute"/>, <see cref="XmlRootAttribute"/>, <see cref="XmlElementAttribute"/>,
    ///     <see cref="DataContractAttribute"/>, <see cref="MessageContractAttribute"/>,
    ///     <see cref="CollectionDataContractAttribute"/></item>
    /// </list>
    /// </remarks>
    /// <param name="type">The type.</param>
    /// <returns>
    ///   <see langword="true"/> if <paramref name="type"/> can be serialized; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool CanXmlTransform(this Type type)
    {
        if (type.IsBasicType() ||
            type.GetInterfaces().Contains(typeof(ISerializable)) ||
            type.GetInterfaces().Contains(typeof(IXmlSerializable)) ||
            type.GetCustomAttributes(true).Any(a => _xmlSerializableAttributes.Contains(a.GetType())))
            return true;

        // if the type is array - continue the test for the element type
        if (type.IsArray)
            return type.GetElementType()!.CanXmlTransform();

        // if the type is collection - continue the test for the element type
        if (type.IsSystemCollectionsGeneric() &&
            type.GetInterfaces()
                .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IEnumerable<>))?
                .GetGenericArguments()
                .All(CanXmlTransform) is true)
            return true;

        // if the type is a generic nullable (e.g. `Nullable<int>`) - continue the test for the underlying type
        if (type.IsTuple())
            return type.GetGenericArguments().All(CanXmlTransform);

        // if it is an anonymous type and recursively all of its properties are serializable - yes
        if (type.IsAnonymous())
            return type.GetProperties().All(p => p.PropertyType.CanXmlTransform());

        // if the type is a generic nullable (e.g. `Nullable<int>`) - continue the test for the underlying type
        if (type.IsNullable() ||
            type.IsSliceSpanOrMemory())
            return type.GetGenericArguments()[0].CanXmlTransform();

        // if this is all we can do - return optimistic true. In the worst case it will throw serialization exception later.
        return true;
    }
}
