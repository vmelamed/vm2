namespace vm2.ExpressionSerialization.Utilities;

using System.Runtime.CompilerServices;

/// <summary>
/// Class TypeExtensions contains extension methods of <see cref="Type"/>.
/// </summary>
internal static partial class TypeExtensions
{
    /// <summary>
    /// Class TypeExtensions contains extension methods of <see cref="Type"/>.
    /// </summary>
    static readonly Type[] _nonPrimitiveBasicTypesCollection =
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
    static readonly FrozenSet<Type> _nonPrimitiveBasicTypes = _nonPrimitiveBasicTypesCollection.ToFrozenSet();

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

    public static bool ImplementsInterface(this Type type, Type interfaceType)
        => type.GetInterface(interfaceType.Name) is not null;

    public static bool ImplementsInterface(this Type type, string interfaceName)
        => type.GetInterface(interfaceName) is not null;

    static readonly Type[] _byteArrayLikeCollection =
    [
        typeof(ArraySegment<byte>),
        typeof(Memory<byte>),
        typeof(ReadOnlyMemory<byte>),
        // Span-s cannot be members of objects (ConstantExpression)
        //typeof(Span<byte>),
        //typeof(ReadOnlySpan<byte>),
    ];
    static readonly FrozenSet<Type> _byteArrayLikeSequences = _byteArrayLikeCollection.ToFrozenSet();

    /// <summary>
    /// Determines whether the specified type is a byte sequence: <c>byte[], Span&lt;byte&gt;, ReadOnlySpan&lt;byte&gt;,
    /// Memory&lt;byte&gt;, ReadOnlyMemory&lt;byte&gt;, ArraySegment&lt;byte&gt;</c>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>bool.</returns>
    public static bool IsByteSequence(this Type type)
        => type.IsArray && type.GetElementType() == typeof(byte) ||
           _byteArrayLikeSequences.Contains(type);
    static readonly Type[] _sequencesCollection =
    [
        typeof(ArraySegment<>),
        typeof(Memory<>),
        typeof(ReadOnlyMemory<>),
        typeof(FrozenSet<>),
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
        // Span-s cannot be members of objects (ConstantExpression)
        //typeof(Span<>),
        //typeof(ReadOnlySpan<>),
    ];
    static readonly FrozenSet<Type> _sequences = _sequencesCollection.ToFrozenSet();

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
                genType.Name.EndsWith("FrozenSet`1")) // TODO: this is pretty wonky but I don't know how to fix it for the internal "SmallValueTypeComparableFrozenSet`1" or "SmallFrozenSet`1" 
                return true;
        }

        // should we even support these...?
        return type == typeof(Queue) || type == typeof(Stack) || type == typeof(Hashtable);
    }

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
