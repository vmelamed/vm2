namespace vm2.ExpressionSerialization.Conventions;
static partial class Transform
{
    // The following constants represent the names (hence the prefix 'N') of basic types that are common for all text
    // documents that we transform into or from, e.g. XML (local names), JSON, YAML, etc.).
    public const string NVoid                   = "void";   // used only for method return values!
    public const string NUri                    = "uri";
    public const string NBool                   = "boolean";
    public const string NByte                   = "byte";
    public const string NChar                   = "char";
    public const string NDateTime               = "dateTime";
    public const string NDateTimeOffset         = "dateTimeOffset";
    public const string NDBNull                 = "dbNull";
    public const string NDecimal                = "decimal";
    public const string NDouble                 = "double";
    public const string NFloat                  = "float";
    public const string NGuid                   = "guid";
    public const string NHalf                   = "half";
    public const string NInt                    = "int";
    public const string NIntPtr                 = "intPtr";
    public const string NLong                   = "long";
    public const string NSByte                  = "signedByte";
    public const string NShort                  = "short";
    public const string NString                 = "string";
    public const string NTimeSpan               = "duration";
    public const string NUInt                   = "unsignedInt";
    public const string NUIntPtr                = "unsignedIntPtr";
    public const string NULong                  = "unsignedLong";
    public const string NUShort                 = "unsignedShort";

    // other types and type categories
    public const string NObject                 = "object";
    public const string NNullable               = "nullable";
    public const string NEnum                   = "enum";
    public const string NAnonymous              = "anonymous";
    public const string NByteSequence           = "byteSequence";
    public const string NCollection             = "collection";
    public const string NDictionary             = "dictionary";
    public const string NKeyValuePair           = "key-value";
    public const string NTuple                  = "tuple";
    public const string NTupleItem              = "item";

    // names for other types of elements and attributes
    public const string NNil                    = "nil";
    public const string NAssembly               = "assembly";
    public const string NDelegateType           = "delegateType";
    public const string NFamily                 = "family";
    public const string NFamilyAndAssembly      = "familyAndAssembly";
    public const string NFamilyOrAssembly       = "familyOrAssembly";
    public const string NIsByRef                = "isByRef";
    public const string NIsLifted               = "isLifted";
    public const string NIsLiftedToNull         = "isLiftedToNull";
    public const string NKind                   = "kind";
    public const string NName                   = "name";
    public const string NPrivate                = "private";
    public const string NPublic                 = "public";
    public const string NStatic                 = "static";
    public const string NTailCall               = "tailCall";
    public const string NType                   = "type";
    public const string NDeclaringType          = "declaringType";
    public const string NTypeOperand            = "typeOperand";
    public const string NConcreteType           = "concreteType";   // e.g. derived from ConstantExpression.GetType
    public const string NBaseType               = "baseType";
    public const string NId                     = "id";
    public const string NIdRef                  = "idref";
    public const string NValue                  = "value";
    public const string NBaseValue              = "baseValue";
    public const string NVisibility             = "visibility";
    public const string NLength                 = "length";
    public const string NElementType            = "elementType";
    public const string NReadOnly               = "readOnly";

    public const string NArguments              = "arguments";
    public const string NParameterSpec          = "parameterSpec";             // used in MemberInfo
    public const string NParameterDefinition    = "parameterDefinition";       // used in Lambdas
    public const string NParameterReference     = "parameter";
    public const string NParameters             = "parameters";
    public const string NParameterSpecs         = "parameterSpecs";
    public const string NVariableDefinition     = "variableDefinition";
    public const string NVariableReference      = "variable";                  // used in blocks
    public const string NVariables              = "variables";

    public const string NExpression             = "expression";

    public const string NBody                   = "body";
    public const string NIndexes                = "indexes";
    public const string NLeft                   = "left";
    public const string NMethod                 = "method";
    public const string NRight                  = "right";
    public const string NArrayIndex             = "arrayIndex";
    public const string NAdd                    = "add";
    public const string NAddAssign              = "addAssign";
    public const string NAddAssignChecked       = "addAssignChecked";
    public const string NAddChecked             = "addChecked";
    public const string NAnd                    = "and";
    public const string NAndAlso                = "andAlso";
    public const string NAndAssign              = "andAssign";
    public const string NArrayLength            = "arrayLength";
    public const string NAssign                 = "assign";
    public const string NBindings               = "bindings";
    public const string NBlock                  = "block";
    public const string NBounds                 = "bounds";
    public const string NBreakLabel             = "breakLabel";
    public const string NCase                   = "case";
    public const string NCatch                  = "catch";
    public const string NCall                   = "call";
    public const string NCaseValues             = "caseValues";
    public const string NCoalesce               = "coalesce";
    public const string NConditional            = "conditional";
    public const string NConstant               = "constant";
    public const string NConstructor            = "constructor";
    public const string NContinueLabel          = "continueLabel";
    public const string NConvert                = "convert";
    public const string NConvertChecked         = "convertChecked";
    public const string NDecrement              = "decrement";
    public const string NDefault                = "default";
    public const string NDefaultCase            = "defaultCase";
    public const string NDivide                 = "divide";
    public const string NDivideAssign           = "divideAssign";
    public const string NElementInit            = "elementInit";
    public const string NArrayElements          = "elements";
    public const string NEqual                  = "equal";
    public const string NEvent                  = "event";
    public const string NException              = "exception";
    public const string NExclusiveOr            = "exclusiveOr";
    public const string NExclusiveOrAssign      = "exclusiveOrAssign";
    public const string NFault                  = "fault";
    public const string NField                  = "field";
    public const string NFilter                 = "filter";
    public const string NFinally                = "finally";
    public const string NGoto                   = "goto";
    public const string NGreaterThan            = "greaterThan";
    public const string NGreaterThanOrEqual     = "greaterThanOrEqual";
    public const string NIncrement              = "increment";
    public const string NIndex                  = "index";
    public const string NInvoke                 = "invoke";
    public const string NIsFalse                = "isFalse";
    public const string NIsTrue                 = "isTrue";
    public const string NLabel                  = "label";
    public const string NLambda                 = "lambda";
    public const string NLeftShift              = "leftShift";
    public const string NLeftShiftAssign        = "leftShiftAssign";
    public const string NLessThan               = "lessThan";
    public const string NLessThanOrEqual        = "lessThanOrEqual";
    public const string NListInit               = "listInit";
    public const string NLoop                   = "loop";
    public const string NMemberAccess           = "memberAccess";
    public const string NMemberInit             = "memberInit";
    public const string NMembers                = "members";
    public const string NModulo                 = "modulo";
    public const string NModuloAssign           = "moduloAssign";
    public const string NMultiply               = "multiply";
    public const string NMultiplyAssign         = "multiplyAssign";
    public const string NMultiplyAssignChecked  = "multiplyAssignChecked";
    public const string NMultiplyChecked        = "multiplyChecked";
    public const string NNegate                 = "negate";
    public const string NNegateChecked          = "negateChecked";
    public const string NNew                    = "new";
    public const string NNewArrayBounds         = "newArrayBounds";
    public const string NNewArrayInit           = "newArrayInit";
    public const string NNot                    = "not";
    public const string NNotEqual               = "notEqual";
    public const string NOnesComplement         = "onesComplement";
    public const string NOr                     = "or";
    public const string NOrAssign               = "orAssign";
    public const string NOrElse                 = "orElse";
    public const string NPostDecrementAssign    = "postDecrementAssign";
    public const string NPostIncrementAssign    = "postIncrementAssign";
    public const string NPower                  = "power";
    public const string NPowerAssign            = "powerAssign";
    public const string NPreDecrementAssign     = "preDecrementAssign";
    public const string NPreIncrementAssign     = "preIncrementAssign";
    public const string NProperty               = "property";
    public const string NQuote                  = "quote";
    public const string NRightShift             = "rightShift";
    public const string NRightShiftAssign       = "rightShiftAssign";
    public const string NSubtract               = "subtract";
    public const string NSubtractAssign         = "subtractAssign";
    public const string NSubtractAssignChecked  = "subtractAssignChecked";
    public const string NSubtractChecked        = "subtractChecked";
    public const string NSwitch                 = "switch";
    public const string NTarget                 = "target";
    public const string NThrow                  = "throw";
    public const string NTry                    = "try";
    public const string NTypeAs                 = "typeAs";
    public const string NTypeEqual              = "typeEqual";
    public const string NTypeIs                 = "typeIs";
    public const string NUnaryPlus              = "unaryPlus";
    public const string NUnbox                  = "unbox";
    public const string NAssignmentBinding      = "assignmentBinding";
    public const string NMemberMemberBinding    = "memberMemberBinding";
    public const string NMemberListBinding      = "memberListBinding";

    static readonly Dictionary<Type, string> _typesToNames_ = new()
    {
        [typeof(void)]                  = NVoid, // used only for method return values!
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
        [typeof(uint)]                  = NUInt,
        [typeof(UIntPtr)]               = NUIntPtr,
        [typeof(ulong)]                 = NULong,
        [typeof(Uri)]                   = NUri,
        [typeof(ushort)]                = NUShort,
        [typeof(Enum)]                  = NEnum,
        [typeof(Nullable<>)]            = NNullable,
        [typeof(object)]                = NObject,
    };
    /// <summary>
    /// Maps basic types to names used in text documents.
    /// </summary>
    public static readonly FrozenDictionary<Type, string> TypesToNames = _typesToNames_.ToFrozenDictionary();

    static readonly Dictionary<string, Type> _namesToTypes_ = new ()
    {
        [NVoid]                         = typeof(void), // used only for method return values!
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
        [NUInt]                         = typeof(uint),
        [NUIntPtr]                      = typeof(UIntPtr),
        [NULong]                        = typeof(ulong),
        [NUri]                          = typeof(Uri),
        [NUShort]                       = typeof(ushort),
        [NEnum]                         = typeof(Enum),
        [NNullable]                     = typeof(Nullable<>),
        [NObject]                       = typeof(object),
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
