namespace vm2.ExpressionSerialization.XmlTransform;

static partial class FromXmlDataTransform
{
    static readonly Dictionary<string, Transformation> _constantTransformations_ = new()
    {
        { Vocabulary.Uri,            (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? new Uri(x.Value)                         : throw new SerializationException("Cannot deserialize URI object from null or empty string.") },
        { Vocabulary.Bool,           (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) && XmlConvert.ToBoolean(x.Value)                     },
        { Vocabulary.Byte,           (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToByte(x.Value)               : default },
        { Vocabulary.Char,           (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? x.Value[0]                               : default },
        { Vocabulary.DateTime,       (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDateTime(x.Value, XmlDateTimeSerializationMode.RoundtripKind) : default },
        { Vocabulary.DateTimeOffset, (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDateTimeOffset(x.Value, "O"): default },
        { Vocabulary.DBNull,         (XElement x, ref Type t) => DBNull.Value                                                                             },
        { Vocabulary.Decimal,        (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDecimal(x.Value)            : default },
        { Vocabulary.Double,         (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDouble(x.Value)             : default },
        { Vocabulary.TimeSpan,       (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToTimeSpan(x.Value)           : default },
        { Vocabulary.Float,          (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToSingle(x.Value)             : default },
        { Vocabulary.Guid,           (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToGuid(x.Value)               : default },
        { Vocabulary.Half,           (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? (Half)XmlConvert.ToDouble(x.Value)       : default },
        { Vocabulary.Int,            (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToInt32(x.Value)              : default },
        { Vocabulary.IntPtr,         (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? (IntPtr)XmlConvert.ToInt32(x.Value)      : default },
        { Vocabulary.Long,           (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToInt64(x.Value)              : default },
        { Vocabulary.Short,          (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToInt16(x.Value)              : default },
        { Vocabulary.String,         (XElement x, ref Type t) => x.IsNil() ? null : x.Value                                                     },
        { Vocabulary.SByte,          (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToSByte(x.Value)              : default },
        { Vocabulary.UInt,           (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToUInt32(x.Value)             : default },
        { Vocabulary.UIntPtr,        (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? (UIntPtr)XmlConvert.ToUInt32(x.Value)    : default },
        { Vocabulary.ULong,          (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToUInt64(x.Value)             : default },
        { Vocabulary.UShort,         (XElement x, ref Type t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToUInt16(x.Value)             : default },
        { Vocabulary.Anonymous,      TransformAnonymous                                                                       },
        { Vocabulary.ByteSequence,   TransformByteSequence                                                                    },
        { Vocabulary.Collection,     TransformCollection                                                                      },
        { Vocabulary.Dictionary,     TransformDictionary                                                                      },
        { Vocabulary.Enum,           TransformEnum                                                                            },
        { Vocabulary.Nullable,       TransformNullable                                                                        },
        { Vocabulary.Object,         TransformObject                                                                          },
        { Vocabulary.Tuple,          TransformTuple                                                                           },
        { Vocabulary.TupleItem,      TransformTuple                                                                           },
    };
    static readonly FrozenDictionary<string, Transformation> _constantTransformations = _constantTransformations_.ToFrozenDictionary();

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

    static readonly Dictionary<Type, Func<Type, Type, int, IEnumerable, object?>> _sequenceBuilders_ = new ()
    {
        [typeof(FrozenSet<>)]           = (gt, et, len, seq) => _toFrozenSet.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et), null]),
        [typeof(ImmutableArray<>)]      = (gt, et, len, seq) => _toImmutableArray.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(ImmutableHashSet<>)]    = (gt, et, len, seq) => _toImmutableHashSet.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(ImmutableList<>)]       = (gt, et, len, seq) => _toImmutableList.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(ImmutableSortedSet<>)]  = (gt, et, len, seq) => _toImmutableSortedSet.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(ImmutableQueue<>)]      = (gt, et, len, seq) => _toImmutableQueue.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(ImmutableStack<>)]      = (gt, et, len, seq) => _toImmutableStack.MakeGenericMethod(et).Invoke(null, [CastSequence(seq.Cast<object?>().Reverse(), et)]),
        [typeof(List<>)]                = (gt, et, len, seq) => _toList.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(HashSet<>)]             = (gt, et, len, seq) => _toHashSet.MakeGenericMethod(et).Invoke(null, [CastSequence(seq, et)]),
        [typeof(ArraySegment<>)]        = (gt, et, len, seq) => BuildCollectionFromArray(gt, et, TransformToArray(et, len, seq)),
        [typeof(Memory<>)]              = (gt, et, len, seq) => BuildCollectionFromArray(gt, et, TransformToArray(et, len, seq)),
        [typeof(ReadOnlyMemory<>)]      = (gt, et, len, seq) => BuildCollectionFromArray(gt, et, TransformToArray(et, len, seq)),
        [typeof(ConcurrentQueue<>)]     = (gt, et, len, seq) => BuildCollectionFromEnumerable(gt, et, seq),
        [typeof(ConcurrentStack<>)]     = (gt, et, len, seq) => BuildCollectionFromEnumerable(gt, et, seq.Cast<object?>().Reverse()),
        [typeof(Stack<>)]               = (gt, et, len, seq) => BuildCollectionFromEnumerable(gt, et, seq.Cast<object?>().Reverse()),
        [typeof(Collection<>)]          = (gt, et, len, seq) => BuildCollectionFromList(gt, et, seq.Cast<object?>().ToList()),
        [typeof(ReadOnlyCollection<>)]  = (gt, et, len, seq) => BuildCollectionFromList(gt, et, seq.Cast<object?>().ToList()),
        [typeof(LinkedList<>)]          = (gt, et, len, seq) => BuildCollectionFromEnumerable(gt, et, seq),
        [typeof(Queue<>)]               = (gt, et, len, seq) => BuildCollectionFromEnumerable(gt, et, seq.Cast<object?>()),
        [typeof(SortedSet<>)]           = (gt, et, len, seq) => BuildCollectionFromEnumerable(gt, et, seq),
        [typeof(BlockingCollection<>)]  = BuildBlockingCollection,
        [typeof(ConcurrentBag<>)]       = BuildConcurrentBag,
    };
    static readonly FrozenDictionary<Type, Func<Type, Type, int, IEnumerable, object?>> _sequenceBuilders = _sequenceBuilders_.ToFrozenDictionary();
}
