namespace vm2.ExpressionSerialization.Conventions;

static partial class Transform
{
    static IEnumerable<Type> EnumNonPrimitiveBasicTypes()
    {
        yield return typeof(DateTime);
        yield return typeof(DateTimeOffset);
        yield return typeof(DBNull);
        yield return typeof(decimal);
        yield return typeof(TimeSpan);
        yield return typeof(Guid);
        yield return typeof(Half);
        yield return typeof(string);
        yield return typeof(Uri);
    }

    /// <summary>
    /// The non primitive basic types says it all: these are the basic types, which are not .NET primitive (<c>typeof(type).IsPrimitive == false</c>).
    /// </summary>
    public static readonly FrozenSet<Type> NonPrimitiveBasicTypes = EnumNonPrimitiveBasicTypes().ToFrozenSet();

    static IEnumerable<Type> EnumSequenceTypes()
    {
        yield return typeof(ArraySegment<>);
        yield return typeof(BlockingCollection<>);
        yield return typeof(Collection<>);
        yield return typeof(ConcurrentBag<>);
        yield return typeof(ConcurrentQueue<>);
        yield return typeof(ConcurrentStack<>);
        yield return typeof(FrozenSet<>);
        yield return typeof(HashSet<>);
        yield return typeof(ImmutableArray<>);
        yield return typeof(ImmutableHashSet<>);
        yield return typeof(ImmutableList<>);
        yield return typeof(ImmutableQueue<>);
        yield return typeof(ImmutableSortedSet<>);
        yield return typeof(ImmutableStack<>);
        yield return typeof(LinkedList<>);
        yield return typeof(List<>);
        yield return typeof(Memory<>);
        yield return typeof(Queue<>);
        yield return typeof(ReadOnlyCollection<>);
        yield return typeof(ReadOnlyMemory<>);
        yield return typeof(SortedSet<>);
        yield return typeof(Stack<>);

        // Although sequence like, Span-s cannot be members of objects (ConstantExpression) and have other limitations.
        // Therefore we do not support them.
        //  typeof(Span<>),
        //  typeof(ReadOnlySpan<>),
    }

    /// <summary>
    /// Sequence of supported generic types that represent sequences of elements. These are mostly types that
    /// implement <see cref="IEnumerable{T}"/>. Technically <see cref="Memory{T}"/> and <see cref="ReadOnlyMemory{T}"/>
    /// do not qualify but their property <see cref="Memory{T}.Span"/> does and the spirit of these classes is a
    /// sequence-like.
    /// </summary>
    public static readonly FrozenSet<Type> SequenceTypes = EnumSequenceTypes().ToFrozenSet();

    static IEnumerable<Type> EnumByteSequences()
    {
        yield return typeof(ArraySegment<byte>);
        yield return typeof(Memory<byte>);
        yield return typeof(ReadOnlyMemory<byte>);

        // Although byte-sequence like, Span-s cannot be members of objects (ConstantExpression) and have other limitations.
        // Therefore we do not support them.
        //  typeof(Span<byte>),
        //  typeof(ReadOnlySpan<byte>),
    }

    /// <summary>
    /// Sequence of supported generic types that represent contiguous sequences of bytes with similar behavior as <c>byte[]</c>.
    /// Technically <see cref="Memory{T}"/> and <see cref="ReadOnlyMemory{T}"/> do not qualify but their property
    /// <c>Memory&lt;byte&gt;.Span</c> does and the spirit of these classes is a byte sequence-like.
    /// </summary>
    public static readonly FrozenSet<Type> ByteSequences = EnumByteSequences().ToFrozenSet();
}
