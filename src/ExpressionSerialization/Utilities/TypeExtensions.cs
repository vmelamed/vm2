namespace vm2.ExpressionSerialization.Utilities;

using System.Runtime.CompilerServices;

/// <summary>
/// Class TypeExtensions contains extension methods of <see cref="Type"/>.
/// </summary>
internal static partial class TypeExtensions
{
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
        => type.IsGenericType && type.Name.StartsWith(anonymousTypePrefix, StringComparison.Ordinal);

    const string collectionNamespacePrefix = "System.Collections.Generic";

    /// <summary>
    /// Determines whether the specified type is a generic nullable.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsNullable(this Type type)
        => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    /// <summary>
    /// Determines whether the specified type is a generic tuple class.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsTupleClass(this Type type)
        => type.IsClass && type.ImplementsInterface(typeof(ITuple));

    /// <summary>
    /// Determines whether the specified type is a generic tuple struct.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsTupleValue(this Type type)
        => type.IsValueType && type.ImplementsInterface(typeof(ITuple));

    /// <summary>
    /// Determines whether the specified type is a tuple.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsTuple(this Type type)
        => type.ImplementsInterface(typeof(ITuple));

    /// <summary>
    /// Determines whether the specified type is a span.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsSpan(this Type type)
    {
        if (!type.IsGenericType)
            return false;

        var genType = type.GetGenericTypeDefinition();

        return genType == typeof(Span<>) ||
               genType == typeof(ReadOnlySpan<>);
    }

    /// <summary>
    /// Determines whether the specified type is a memory{}.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsMemory(this Type type)
    {
        if (!type.IsGenericType)
            return false;

        var genType = type.GetGenericTypeDefinition();

        return genType == typeof(Memory<>) ||
               genType == typeof(ReadOnlyMemory<>);
    }

    /// <summary>
    /// Determines whether the specified type is an array like sequence of objects (ArraySpan, Span, Memory, etc.).
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsArrayLike(this Type type)
        => type.IsGenericType && _arrayLikes.Contains(type.GetGenericTypeDefinition());

    public static bool ImplementsInterface(this Type type, Type interfaceType)
        => type.GetInterface(interfaceType.Name) is not null;

    public static bool ImplementsInterface(this Type type, string interfaceName)
        => type.GetInterface(interfaceName) is not null;

    /// <summary>
    /// Determines whether the specified type is a byte sequence: <c>byte[], Span&lt;byte&gt;, ReadOnlySpan&lt;byte&gt;,
    /// Memory&lt;byte&gt;, ReadOnlyMemory&lt;byte&gt;, ArraySegment&lt;byte&gt;</c>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsByteSequence(this Type type)
        => type.IsArray && type.GetElementType() == typeof(byte) ||
           _byteArrayLikeSequences.Contains(type);

    /// <summary>
    /// Determines whether the specified type is a sequence of objects: array, list, set, etc..
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsSequence(this Type type)
    {
        if (type.IsArray)
            return true;

        var isGeneric = type.IsGenericType;

        if (isGeneric)
        {
            var genType = type.GetGenericTypeDefinition();

            if (_sequences.Contains(genType) ||
                _arrayLikes.Contains(genType) ||
                genType.Name.EndsWith("FrozenSet`1"))
                return true;
        }

        // should we even support these...?
        return type == typeof(Queue) || type == typeof(Stack) || type == typeof(Hashtable);
    }

    public static bool IsDictionary(this Type type)
        => type.ImplementsInterface(typeof(IDictionary));
}
