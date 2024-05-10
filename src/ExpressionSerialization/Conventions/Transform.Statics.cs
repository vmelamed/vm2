namespace vm2.ExpressionSerialization.Conventions;

static partial class Transform
{
    // The following constants represent the names (hence the prefix 'N') of basic types that are common for all text
    // documents that we transform into or from, e.g. XML (local names), JSON, YAML, etc.).
    public const string NVoid           = "void";
    public const string NBool           = "boolean";
    public const string NByte           = "byte";
    public const string NChar           = "char";
    public const string NDouble         = "double";
    public const string NFloat          = "float";
    public const string NInt            = "int";
    public const string NIntPtr         = "intPtr";
    public const string NLong           = "long";
    public const string NShort          = "short";
    public const string NSByte          = "signedByte";
    public const string NUint           = "unsignedInt";
    public const string NUIntPtr        = "unsignedIntPtr";
    public const string NULong          = "unsignedLong";
    public const string NUShort         = "unsignedShort";
    // non-primitive, basic types
    public const string NDateTime       = "dateTime";
    public const string NDateTimeOffset = "dateTimeOffset";
    public const string NDBNull         = "dbNull";
    public const string NDecimal        = "decimal";
    public const string NTimeSpan       = "duration";
    public const string NGuid           = "guid";
    public const string NHalf           = "half";
    public const string NString         = "string";
    public const string NUri            = "uri";

    // other types and type categories
    public const string NObject         = "object";
    public const string NNullable       = "nullable";
    public const string NEnum           = "enum";
    public const string NAnonymous      = "anonymous";
    public const string NByteSequence   = "byteSequence";
    public const string NCollection     = "collection";
    public const string NDictionary     = "dictionary";
    public const string NKeyValuePair   = "key-value";
    public const string NTuple          = "tuple";
    public const string NTupleItem      = "item";

    static readonly Dictionary<Type, string> _typesToNames_ = new()
    {
        [typeof(bool)]                  = NBool,
        [typeof(byte)]                  = NByte,
        [typeof(char)]                  = NChar,
        [typeof(DateTime)]              = NDateTime,
        [typeof(DateTimeOffset)]        = NDateTimeOffset,
        [typeof(DBNull)]                = NDBNull,
        [typeof(decimal)]               = NDecimal,
        [typeof(double)]                = NDouble,
        [typeof(float)]                 = NFloat,
        [typeof(Guid)]                  = NGuid,
        [typeof(Half)]                  = NHalf,
        [typeof(int)]                   = NInt,
        [typeof(IntPtr)]                = NIntPtr,
        [typeof(long)]                  = NLong,
        [typeof(sbyte)]                 = NSByte,
        [typeof(short)]                 = NShort,
        [typeof(string)]                = NString,
        [typeof(TimeSpan)]              = NTimeSpan,
        [typeof(uint)]                  = NUint,
        [typeof(UIntPtr)]               = NUIntPtr,
        [typeof(ulong)]                 = NULong,
        [typeof(Uri)]                   = NUri,
        [typeof(ushort)]                = NUShort,
        [typeof(void)]                  = NVoid,
        [typeof(Enum)]                  = NEnum,
        [typeof(Nullable<>)]            = NNullable,
    };
    /// <summary>
    /// Maps basic types to names used in text documents.
    /// </summary>
    public static readonly FrozenDictionary<Type, string> TypesToNames = _typesToNames_.ToFrozenDictionary();

    static readonly Dictionary<string, Type> _namesToTypes_ = new ()
    {
        [NBool]                         = typeof(bool),
        [NByte]                         = typeof(byte),
        [NChar]                         = typeof(char),
        [NDateTime]                     = typeof(DateTime),
        [NDateTimeOffset]               = typeof(DateTimeOffset),
        [NDBNull]                       = typeof(DBNull),
        [NDecimal]                      = typeof(decimal),
        [NDouble]                       = typeof(double),
        [NFloat]                        = typeof(float),
        [NGuid]                         = typeof(Guid),
        [NHalf]                         = typeof(Half),
        [NInt]                          = typeof(int),
        [NIntPtr]                       = typeof(IntPtr),
        [NLong]                         = typeof(long),
        [NSByte]                        = typeof(sbyte),
        [NShort]                        = typeof(short),
        [NString]                       = typeof(string),
        [NTimeSpan]                     = typeof(TimeSpan),
        [NUint]                         = typeof(uint),
        [NUIntPtr]                      = typeof(UIntPtr),
        [NULong]                        = typeof(ulong),
        [NUri]                          = typeof(Uri),
        [NUShort]                       = typeof(ushort),
        [NVoid]                         = typeof(void),
        [NEnum]                         = typeof(Enum),
        [NNullable]                     = typeof(Nullable<>),
    };
    /// <summary>
    /// Maps names used in text documents to basic types.
    /// </summary>
    public static readonly FrozenDictionary<string, Type> NamesToTypes = _namesToTypes_.ToFrozenDictionary();

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
