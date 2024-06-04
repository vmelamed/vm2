namespace vm2.ExpressionSerialization.Conventions;

static partial class Transform
{
    static readonly Type[] _nonPrimitiveBasicTypes_ =
    [
        typeof(DateTime),
        typeof(DateTimeOffset),
        typeof(DBNull),
        typeof(decimal),
        typeof(TimeSpan),
        typeof(Guid),
        typeof(Half),
        typeof(string),
        typeof(Uri),
    ];
    /// <summary>
    /// The non primitive basic types says it all: these are the basic types, which are not .NET primitive (<c>typeof(type).IsPrimitive == false</c>).
    /// </summary>
    public static readonly FrozenSet<Type> NonPrimitiveBasicTypes = _nonPrimitiveBasicTypes_.ToFrozenSet();

    static readonly Type[] _sequences_ =
    [
        typeof(ArraySegment<>),
        typeof(BlockingCollection<>),
        typeof(Collection<>),
        typeof(ConcurrentBag<>),
        typeof(ConcurrentQueue<>),
        typeof(ConcurrentStack<>),
        typeof(FrozenSet<>),
        typeof(HashSet<>),
        typeof(ImmutableArray<>),
        typeof(ImmutableHashSet<>),
        typeof(ImmutableList<>),
        typeof(ImmutableQueue<>),
        typeof(ImmutableSortedSet<>),
        typeof(ImmutableStack<>),
        typeof(LinkedList<>),
        typeof(List<>),
        typeof(Memory<>),
        typeof(Queue<>),
        typeof(ReadOnlyCollection<>),
        typeof(ReadOnlyMemory<>),
        typeof(SortedSet<>),
        typeof(Stack<>),

        // Although sequence like, Span-s cannot be members of objects (ConstantExpression) and have other limitations.
        // Therefore we do not support them.
        //  typeof(Span<>),
        //  typeof(ReadOnlySpan<>),
    ];
    /// <summary>
    /// Collection of supported generic types that represent sequences of elements. These are mostly types that 
    /// implement <see cref="IEnumerable{T}"/>. Technically <see cref="Memory{T}"/> and <see cref="ReadOnlyMemory{T}"/> 
    /// do not qualify but their property <see cref="Memory{T}.Span"/> does and the spirit of these classes is a
    /// sequence-like.
    /// </summary>
    public static readonly FrozenSet<Type> SequenceTypes = _sequences_.ToFrozenSet();

    static readonly Type[] _byteSequences_ =
    [
        typeof(ArraySegment<byte>),
        typeof(Memory<byte>),
        typeof(ReadOnlyMemory<byte>),

        // Although byte-sequence like, Span-s cannot be members of objects (ConstantExpression) and have other limitations.
        // Therefore we do not support them.
        //  typeof(Span<byte>),
        //  typeof(ReadOnlySpan<byte>),
    ];
    /// <summary>
    /// Collection of supported generic types that represent contiguous sequences of bytes with similar behavior as <c>byte[]</c>. 
    /// Technically <see cref="Memory{T}"/> and <see cref="ReadOnlyMemory{T}"/> do not qualify but their property 
    /// <c>Memory&lt;byte&gt;.Span</c> does and the spirit of these classes is a byte sequence-like.
    /// </summary>
    public static readonly FrozenSet<Type> ByteSequences = _byteSequences_.ToFrozenSet();
}
