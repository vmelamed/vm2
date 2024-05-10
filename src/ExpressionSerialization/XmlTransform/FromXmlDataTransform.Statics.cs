namespace vm2.ExpressionSerialization.XmlTransform;

using Transform = Func<XElement, Type, object?>;

partial class FromXmlDataTransform
{
    static readonly Dictionary<XName, Transform> _constantTransforms_ = new()
    {
        { ElementNames.AnyURI,         (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? new Uri(x.Value)                         : throw new SerializationException("Cannot deserialize URI object from null or empty string.") },
        { ElementNames.Boolean,        (x, t) => !string.IsNullOrWhiteSpace(x.Value) && XmlConvert.ToBoolean(x.Value)                     },
        { ElementNames.Byte,           (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToByte(x.Value)               : default },
        { ElementNames.Char,           (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? x.Value[0]                               : default },
        { ElementNames.DateTime,       (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDateTime(x.Value, XmlDateTimeSerializationMode.RoundtripKind) : default },
        { ElementNames.DateTimeOffset, (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDateTimeOffset(x.Value, "O"): default },
        { ElementNames.DBNull,         (x, t) => DBNull.Value                                                                             },
        { ElementNames.Decimal,        (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDecimal(x.Value)            : default },
        { ElementNames.Double,         (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToDouble(x.Value)             : default },
        { ElementNames.Duration,       (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToTimeSpan(x.Value)           : default },
        { ElementNames.Float,          (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToSingle(x.Value)             : default },
        { ElementNames.Guid,           (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToGuid(x.Value)               : default },
        { ElementNames.Half,           (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? (Half)XmlConvert.ToDouble(x.Value)       : default },
        { ElementNames.Int,            (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToInt32(x.Value)              : default },
        { ElementNames.IntPtr,         (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? (IntPtr)XmlConvert.ToInt32(x.Value)      : default },
        { ElementNames.Long,           (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToInt64(x.Value)              : default },
        { ElementNames.Short,          (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToInt16(x.Value)              : default },
        { ElementNames.String,         (x, t) => x.IsNil() ? null : x.Value                                                     },
        { ElementNames.SignedByte,     (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToSByte(x.Value)              : default },
        { ElementNames.UnsignedInt,    (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToUInt32(x.Value)             : default },
        { ElementNames.UnsignedIntPtr, (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? (UIntPtr)XmlConvert.ToUInt32(x.Value)    : default },
        { ElementNames.UnsignedLong,   (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToUInt64(x.Value)             : default },
        { ElementNames.UnsignedShort,  (x, t) => !string.IsNullOrWhiteSpace(x.Value) ? XmlConvert.ToUInt16(x.Value)             : default },
        { ElementNames.Anonymous,      TransformAnonymous                                                                       },
        { ElementNames.ByteSequence,   TransformByteSequence                                                                    },
        { ElementNames.Collection,     TransformCollection                                                                      },
        { ElementNames.Dictionary,     TransformDictionary                                                                      },
        { ElementNames.Enum,           TransformEnum                                                                            },
        { ElementNames.Nullable,       TransformNullable                                                                        },
        { ElementNames.Object,         TransformObject                                                                          },
        { ElementNames.Tuple,          TransformTuple                                                                           },
        { ElementNames.TupleItem,      TransformTuple                                                                           },
    };
    static readonly FrozenDictionary<XName, Transform> _constantTransforms = _constantTransforms_.ToFrozenDictionary();

    static Dictionary<string, Type> _namesToTypes_ = new()
    {
        { ElementNames.AnyURI.LocalName,            typeof(Uri)            },
        { ElementNames.Boolean.LocalName,           typeof(bool)           },
        { ElementNames.Byte.LocalName,              typeof(byte)           },
        { ElementNames.Char.LocalName,              typeof(char)           },
        { ElementNames.DateTime.LocalName,          typeof(DateTime)       },
        { ElementNames.DateTimeOffset.LocalName,    typeof(DateTimeOffset) },
        { ElementNames.DBNull.LocalName,            typeof(DBNull)         },
        { ElementNames.Decimal.LocalName,           typeof(decimal)        },
        { ElementNames.Double.LocalName,            typeof(double)         },
        { ElementNames.Duration.LocalName,          typeof(TimeSpan)       },
        { ElementNames.Float.LocalName,             typeof(float)          },
        { ElementNames.Guid.LocalName,              typeof(Guid)           },
        { ElementNames.Half.LocalName,              typeof(Half)           },
        { ElementNames.Int.LocalName,               typeof(int)            },
        { ElementNames.IntPtr.LocalName,            typeof(IntPtr)         },
        { ElementNames.Long.LocalName,              typeof(long)           },
        { ElementNames.Short.LocalName,             typeof(short)          },
        { ElementNames.String.LocalName,            typeof(string)         },
        { ElementNames.SignedByte.LocalName,        typeof(sbyte)          },
        { ElementNames.UnsignedInt.LocalName,       typeof(uint)           },
        { ElementNames.UnsignedIntPtr.LocalName,    typeof(UIntPtr)        },
        { ElementNames.UnsignedLong.LocalName,      typeof(ulong)          },
        { ElementNames.UnsignedShort.LocalName,     typeof(ushort)         },
    };
    static FrozenDictionary<string, Type> _namesToTypes = _namesToTypes_.ToFrozenDictionary();

    #region cache some method info-s used in deserialization
    static MethodInfo _toFrozenSet                  = typeof(FrozenSet).GetMethod("ToFrozenSet") ?? throw new InternalTransformErrorException($"Could not get reflection of the method FrozenSet.ToFrozenSet");
    static MethodInfo _toFrozenDictionary           = typeof(FrozenDictionary).GetMethods().Where(mi => mi.Name == nameof(FrozenDictionary.ToFrozenDictionary) && mi.GetParameters().Length == 2).Single();
    static MethodInfo _toImmutableArray             = typeof(ImmutableArray).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableArray.ToImmutableArray))).Single();
    static MethodInfo _toImmutableHashSet           = typeof(ImmutableHashSet).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableHashSet.ToImmutableHashSet))).Single();
    static MethodInfo _toImmutableSortedSet         = typeof(ImmutableSortedSet).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableSortedSet.ToImmutableSortedSet))).Single();
    static MethodInfo _toImmutableList              = typeof(ImmutableList).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableList.ToImmutableList))).Single();
    static MethodInfo _toImmutableQueue             = typeof(ImmutableQueue).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableQueue.CreateRange))).Single();
    static MethodInfo _toImmutableStack             = typeof(ImmutableStack).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableStack.CreateRange))).Single();
    static MethodInfo _toImmutableDictionary        = typeof(ImmutableDictionary).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableDictionary.CreateRange))).Single();
    static MethodInfo _toImmutableSortedDictionary  = typeof(ImmutableSortedDictionary).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(ImmutableDictionary.CreateRange))).Single();
    static MethodInfo _toList                       = typeof(Enumerable).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(Enumerable.ToList))).Single();
    static MethodInfo _toHashSet                    = typeof(Enumerable).GetMethods().Where(mi => mi.MethodHas1EnumerableParameter(nameof(Enumerable.ToHashSet))).Single();
    static MethodInfo _cast                         = typeof(Enumerable).GetMethod("Cast")!;
    static MethodInfo _reverse                      = typeof(Enumerable).GetMethod(nameof(Enumerable.Reverse)) ?? throw new InternalTransformErrorException($"Could not get reflection of the method Enumerable.Reverse");
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
