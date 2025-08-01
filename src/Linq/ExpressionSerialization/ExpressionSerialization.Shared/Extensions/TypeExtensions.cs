namespace vm2.Linq.ExpressionSerialization.Shared.Extensions;

/// <summary>
/// Class TypeExtensions contains extension methods of <see cref="Type"/>.
/// </summary>
public static partial class TypeExtensions
{
    /// <summary>
    /// Determines whether the specified type is a basic type: primitive, enum, decimal, string, Guid, Uri, DateTime,
    /// TimeSpan, DateTimeOffset, IntPtr, UnsignedIntPtr.
    /// </summary>
    /// <param name="type">The type to be tested.</param>
    /// <returns>
    ///   <see langword="true"/> if the specified type is one of the basic types; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsBasicType(this Type type)
    => type.IsPrimitive ||
       type.IsEnum ||
       Transform.NonPrimitiveBasicTypes.Contains(type);

    const string anonymousTypePrefix = "<>f__AnonymousType";

    /// <summary>
    /// Determines whether the specified type is anonymous.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsAnonymous(this Type type)
        => type.IsGenericType && type.Name.StartsWith(anonymousTypePrefix, StringComparison.Ordinal);

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
    /// Determines if the type implements the interface.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="interfaceType">Type of the interface.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public static bool ImplementsInterface(this Type type, Type interfaceType)
        => type.GetInterface(interfaceType.Name) is not null;

    /// <summary>
    /// Determines if the type implements the interface.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="interfaceName">Name of the interface.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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
           Transform.ByteSequences.Contains(type);

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

            if (Transform.SequenceTypes.Contains(genType) ||
                genType.Name.EndsWith("FrozenSet`1")) // TODO: this is pretty wonky but I don't know how to detect the internal "SmallValueTypeComparableFrozenSet`1" or "SmallFrozenSet`1"
                return true;
        }

        // should we even support these...?
        return type == typeof(Queue) || type == typeof(Stack) || type == typeof(Hashtable);
    }

    /// <summary>
    /// Determines whether the specified type is dictionary.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if the specified type is dictionary; otherwise, <c>false</c>.</returns>
    public static bool IsDictionary(this Type type)
        => type.ImplementsInterface(typeof(IDictionary));

    /// <summary>
    /// This predicate determines whether the passed method info is for a method that has the specified name and has 1 parameter of type <c>"IEnumerable&lt;&gt;"</c>.
    /// </summary>
    /// <param name="mi">The method info.</param>
    /// <param name="name">The name.</param>
    /// <returns>bool.</returns>
    public static bool MethodHas1EnumerableParameter(this MethodInfo mi, string name)
        => mi.Name == name &&
           mi.GetParameters().Length == 1 &&
           mi.GetParameters()[0].ParameterType.Name == typeof(IEnumerable<>).Name;

    /// <summary>
    /// This predicate determines whether the passed constructor info is for a constructor that has 1 parameter of type <c>"IEnumerable&lt;&gt;"</c>.
    /// </summary>
    /// <param name="ci">The constructor info.</param>
    /// <returns>bool.</returns>
    public static bool ConstructorHas1ArrayParameter(this ConstructorInfo ci)
        => ci.GetParameters().Length == 1 &&
           ci.GetParameters()[0].ParameterType.IsArray;

    /// <summary>
    /// This predicate determines whether the passed constructor info is for a constructor that has 1 parameter of type <c>"IEnumerable&lt;&gt;"</c>.
    /// </summary>
    /// <param name="ci">The constructor info.</param>
    /// <returns>bool.</returns>
    public static bool ConstructorHas1EnumerableParameter(this ConstructorInfo ci)
        => ci.GetParameters().Length == 1 &&
           ci.GetParameters()[0].ParameterType.Name == typeof(IEnumerable<>).Name;

    /// <summary>
    /// This predicate determines whether the passed constructor info is for a constructor that has 1 parameter of type <c>"IEnumerable&lt;&gt;"</c>.
    /// </summary>
    /// <param name="ci">The constructor info.</param>
    /// <returns>bool.</returns>
    public static bool ConstructorHas1ListParameter(this ConstructorInfo ci)
        => ci.GetParameters().Length == 1 &&
           ci.GetParameters()[0].ParameterType.Name == typeof(IList<>).Name;
}
