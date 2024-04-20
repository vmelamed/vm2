namespace vm2.ExpressionSerialization.Utilities;

using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;

/// <summary>
/// Class TypeExtensions contains extension methods of <see cref="Type"/>.
/// </summary>
internal static partial class TypeExtensions
{
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

    static readonly Type[] _arrayLikeCollection =
    [
        typeof(ArraySegment<>),
        typeof(Memory<>),
        typeof(ReadOnlyMemory<>),
        typeof(Span<>),
        typeof(ReadOnlySpan<>),
    ];
    static readonly FrozenSet<Type> _arrayLikes = _arrayLikeCollection.ToFrozenSet();

    static readonly Type[] _byteArrayLikeCollection =
    [
        typeof(ArraySegment<byte>),
        typeof(Memory<byte>),
        typeof(ReadOnlyMemory<byte>),
        typeof(Span<byte>),
        typeof(ReadOnlySpan<byte>),
    ];
    static readonly FrozenSet<Type> _byteArrayLikeSequences = _byteArrayLikeCollection.ToFrozenSet();

    static readonly Type[] _sequencesCollection =
    [
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
    ];
    static readonly FrozenSet<Type> _sequences = _sequencesCollection.ToFrozenSet();
}
