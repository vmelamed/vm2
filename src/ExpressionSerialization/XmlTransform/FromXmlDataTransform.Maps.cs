namespace vm2.ExpressionSerialization.XmlTransform;

#if JSON_SCHEMA
using Vocabulary = Conventions.Vocabulary;
#endif

static partial class FromXmlDataTransform
{
    static IEnumerable<KeyValuePair<string, Transformation>> ConstantTransformations()
    {
        yield return new(Vocabulary.Boolean, (XElement x, ref Type t) => XmlConvert.ToBoolean(x.Value));
        yield return new(Vocabulary.Byte, (XElement x, ref Type t) => XmlConvert.ToByte(x.Value));
        yield return new(Vocabulary.Char, (XElement x, ref Type t) => x.Value[0]);
        yield return new(Vocabulary.Double, (XElement x, ref Type t) => XmlConvert.ToDouble(x.Value));
        yield return new(Vocabulary.Float, (XElement x, ref Type t) => XmlConvert.ToSingle(x.Value));
        yield return new(Vocabulary.Int, (XElement x, ref Type t) => XmlConvert.ToInt32(x.Value));
        yield return new(Vocabulary.IntPtr, (XElement x, ref Type t) => XmlStringToPtr(x.Value));
        yield return new(Vocabulary.Long, (XElement x, ref Type t) => XmlConvert.ToInt64(x.Value));
        yield return new(Vocabulary.SignedByte, (XElement x, ref Type t) => XmlConvert.ToSByte(x.Value));
        yield return new(Vocabulary.Short, (XElement x, ref Type t) => XmlConvert.ToInt16(x.Value));
        yield return new(Vocabulary.UnsignedInt, (XElement x, ref Type t) => XmlConvert.ToUInt32(x.Value));
        yield return new(Vocabulary.UnsignedIntPtr, (XElement x, ref Type t) => XmlStringToUPtr(x.Value));
        yield return new(Vocabulary.UnsignedLong, (XElement x, ref Type t) => XmlConvert.ToUInt64(x.Value));
        yield return new(Vocabulary.UnsignedShort, (XElement x, ref Type t) => XmlConvert.ToUInt16(x.Value));

        yield return new(Vocabulary.DateTime, (XElement x, ref Type t) => XmlConvert.ToDateTime(x.Value, XmlDateTimeSerializationMode.RoundtripKind));
        yield return new(Vocabulary.DateTimeOffset, (XElement x, ref Type t) => XmlConvert.ToDateTimeOffset(x.Value, "O"));
        yield return new(Vocabulary.Duration, (XElement x, ref Type t) => XmlConvert.ToTimeSpan(x.Value));
        yield return new(Vocabulary.DBNull, (XElement x, ref Type t) => DBNull.Value);
        yield return new(Vocabulary.Decimal, (XElement x, ref Type t) => XmlConvert.ToDecimal(x.Value));
        yield return new(Vocabulary.Guid, (XElement x, ref Type t) => XmlConvert.ToGuid(x.Value));
        yield return new(Vocabulary.Half, (XElement x, ref Type t) => (Half)XmlConvert.ToDouble(x.Value));
        yield return new(Vocabulary.String, (XElement x, ref Type t) => x.IsNil() ? null : x.Value);
        yield return new(Vocabulary.Uri, (XElement x, ref Type t) => new Uri(x.Value));

        yield return new(Vocabulary.Anonymous, TransformAnonymous);
        yield return new(Vocabulary.ByteSequence, TransformByteSequence);
        yield return new(Vocabulary.Sequence, TransformCollection);
        yield return new(Vocabulary.Dictionary, TransformDictionary);
        yield return new(Vocabulary.Enum, TransformEnum);
        yield return new(Vocabulary.Nullable, TransformNullable);
        yield return new(Vocabulary.Object, TransformObject);
        yield return new(Vocabulary.Tuple, TransformTuple);
        yield return new(Vocabulary.TupleItem, TransformTuple);
    }

    static readonly FrozenDictionary<string, Transformation> _constantTransformations = ConstantTransformations().ToFrozenDictionary();

    static IntPtr XmlStringToPtr(string v)
        => (Environment.Is64BitProcess
                ? checked((IntPtr)XmlConvert.ToInt64(v))
                : checked((IntPtr)XmlConvert.ToInt32(v)));

    static UIntPtr XmlStringToUPtr(string v)
        => (Environment.Is64BitProcess
                ? checked((UIntPtr)XmlConvert.ToUInt64(v))
                : checked((UIntPtr)XmlConvert.ToUInt32(v)));

    #region cache some method info-s used in deserialization
    static MethodInfo _toFrozenSet                  = typeof(FrozenSet).GetMethod("ToFrozenSet") ?? throw new InternalTransformErrorException($"Could not get reflection of the method FrozenSet.ToFrozenSet");
    static MethodInfo _toImmutableArray             = typeof(ImmutableArray).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableArray.ToImmutableArray))).Single();
    static MethodInfo _toImmutableHashSet           = typeof(ImmutableHashSet).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableHashSet.ToImmutableHashSet))).Single();
    static MethodInfo _toImmutableSortedSet         = typeof(ImmutableSortedSet).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableSortedSet.ToImmutableSortedSet))).Single();
    static MethodInfo _toImmutableList              = typeof(ImmutableList).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableList.ToImmutableList))).Single();
    static MethodInfo _toImmutableQueue             = typeof(ImmutableQueue).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableQueue.CreateRange))).Single();
    static MethodInfo _toImmutableStack             = typeof(ImmutableStack).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableStack.CreateRange))).Single();
    static MethodInfo _toList                       = typeof(Enumerable).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(Enumerable.ToList))).Single();
    static MethodInfo _toHashSet                    = typeof(Enumerable).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(Enumerable.ToHashSet))).Single();
    static MethodInfo _cast                         = typeof(Enumerable).GetMethod("Cast")!;
    static MethodInfo _reverse                      = typeof(Enumerable).GetMethod(nameof(Enumerable.Reverse)) ?? throw new InternalTransformErrorException($"Could not get reflection of the method Enumerable.Reverse");
    static MethodInfo _toImmutableDictionary        = typeof(ImmutableDictionary).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableDictionary.CreateRange))).Single();
    static MethodInfo _toImmutableSortedDictionary  = typeof(ImmutableSortedDictionary).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableDictionary.CreateRange))).Single();
    static MethodInfo _toFrozenDictionary           = typeof(FrozenDictionary).GetMethods().Where(mi => mi.Name == nameof(FrozenDictionary.ToFrozenDictionary) && mi.GetParameters().Length == 2).Single();
    #endregion

    static object? CastSequence(IEnumerable sequence, Type elementType) => _cast.MakeGenericMethod(elementType).Invoke(null, [sequence]);

    static IEnumerable<KeyValuePair<Type, Func<Type, Type, int, IEnumerable, object?>>> SequenceBuilders()
    {
        yield return new(typeof(FrozenSet<>), (gt, et, len, seq) => _toFrozenSet.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et), null]));
        yield return new(typeof(ImmutableArray<>), (gt, et, len, seq) => _toImmutableArray.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]));
        yield return new(typeof(ImmutableHashSet<>), (gt, et, len, seq) => _toImmutableHashSet.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]));
        yield return new(typeof(ImmutableList<>), (gt, et, len, seq) => _toImmutableList.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]));
        yield return new(typeof(ImmutableSortedSet<>), (gt, et, len, seq) => _toImmutableSortedSet.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]));
        yield return new(typeof(ImmutableQueue<>), (gt, et, len, seq) => _toImmutableQueue.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]));
        yield return new(typeof(ImmutableStack<>), (gt, et, len, seq) => _toImmutableStack.MakeGenericMethod(et).Invoke(null, [CastSequence(seq.Cast<object?>().Reverse(), et)]));
        yield return new(typeof(List<>), (gt, et, len, seq) => _toList.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]));
        yield return new(typeof(HashSet<>), (gt, et, len, seq) => _toHashSet.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]));
        yield return new(typeof(ArraySegment<>), (gt, et, len, seq) => BuildCollectionFromArray(gt, et, TransformToArray(et, len, seq)));
        yield return new(typeof(Memory<>), (gt, et, len, seq) => BuildCollectionFromArray(gt, et, TransformToArray(et, len, seq)));
        yield return new(typeof(ReadOnlyMemory<>), (gt, et, len, seq) => BuildCollectionFromArray(gt, et, TransformToArray(et, len, seq)));
        yield return new(typeof(ConcurrentQueue<>), (gt, et, len, seq) => BuildCollectionFromEnumerable(gt, et, seq));
        yield return new(typeof(ConcurrentStack<>), (gt, et, len, seq) => BuildCollectionFromEnumerable(gt, et, seq.Cast<object?>().Reverse()));
        yield return new(typeof(Stack<>), (gt, et, len, seq) => BuildCollectionFromEnumerable(gt, et, seq.Cast<object?>().Reverse()));
        yield return new(typeof(Collection<>), (gt, et, len, seq) => BuildCollectionFromList(gt, et, seq.Cast<object?>().ToList()));
        yield return new(typeof(ReadOnlyCollection<>), (gt, et, len, seq) => BuildCollectionFromList(gt, et, seq.Cast<object?>().ToList()));
        yield return new(typeof(LinkedList<>), (gt, et, len, seq) => BuildCollectionFromEnumerable(gt, et, seq));
        yield return new(typeof(Queue<>), (gt, et, len, seq) => BuildCollectionFromEnumerable(gt, et, seq.Cast<object?>()));
        yield return new(typeof(SortedSet<>), (gt, et, len, seq) => BuildCollectionFromEnumerable(gt, et, seq));
        yield return new(typeof(BlockingCollection<>), BuildBlockingCollection);
        yield return new(typeof(ConcurrentBag<>), BuildConcurrentBag);
    }

    static readonly FrozenDictionary<Type, Func<Type, Type, int, IEnumerable, object?>> _sequenceBuilders = SequenceBuilders().ToFrozenDictionary();
}
