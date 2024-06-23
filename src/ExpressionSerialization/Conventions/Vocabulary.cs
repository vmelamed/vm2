namespace vm2.ExpressionSerialization.Conventions;

#pragma warning disable CS1591

/// <summary>
/// Class Vocabulary contains declaration of all words used in LINQ serialization, either as XML or JSON tokens.
/// </summary>
public static class Vocabulary
{
    // The following constants represent the names (hence the prefix 'N') of basic types that are common for all text
    // documents that we transform into or from, e.g. XML (local names), JSON, YAML, etc.).
    public const string Void                    = "void";   // used only for method return values!

    public const string Boolean                 = "boolean";
    public const string Byte                    = "byte";
    public const string Char                    = "char";
    public const string Double                  = "double";
    public const string Float                   = "float";
    public const string Int                     = "int";
    public const string IntPtr                  = "intPtr";
    public const string Long                    = "long";
    public const string SignedByte              = "signedByte";
    public const string Short                   = "short";
    public const string UnsignedInt             = "unsignedInt";
    public const string UnsignedIntPtr          = "unsignedIntPtr";
    public const string UnsignedLong            = "unsignedLong";
    public const string UnsignedShort           = "unsignedShort";

    public const string DateTime                = "dateTime";
    public const string DateTimeOffset          = "dateTimeOffset";
    public const string Duration                = "duration";
    public const string DBNull                  = "dbNull";
    public const string Decimal                 = "decimal";
    public const string Guid                    = "guid";
    public const string Half                    = "half";
    public const string String                  = "string";
    public const string Uri                     = "uri";

    // other types and type categories  
    public const string Object                  = "object";
    public const string Nullable                = "nullable";
    public const string Enum                    = "enum";
    public const string Anonymous               = "anonymous";
    public const string ByteSequence            = "byteSequence";
    public const string Sequence                = "sequence";
    public const string Dictionary              = "dictionary";
    public const string KeyValuePair            = "key-value";
    public const string Tuple                   = "tuple";
    public const string TupleItem               = "item";

    // names for other types of elements, attributes, and properties
    public const string Null                    = "nil";
    public const string NaN                     = "NaN";
    public const string PosInfinity             = "INF";
    public const string NegInfinity             = "-INF";
    public const string Type                    = "type";
    public const string DeclaringType           = "declaringType";
    public const string ConcreteType            = "concreteType";   // e.g. derived from ConstantExpression.GetEType
    public const string BaseType                = "baseType";
    public const string Name                    = "name";
    public const string Key                     = "key";
    public const string Value                   = "value";
    public const string Id                      = "id";
    public const string IdRef                   = "idref";
    public const string BaseValue               = "baseValue";
    public const string Length                  = "length";
    public const string Assembly                = "assembly";
    public const string DelegateType            = "delegateType";
    public const string Family                  = "family";
    public const string FamilyAndAssembly       = "familyAndAssembly";
    public const string FamilyOrAssembly        = "familyOrAssembly";
    public const string IsByRef                 = "isByRef";
    public const string IsLifted                = "isLifted";
    public const string IsLiftedToNull          = "isLiftedToNull";
    public const string Kind                    = "kind";
    public const string Private                 = "private";
    public const string Public                  = "public";
    public const string Static                  = "static";
    public const string TailCall                = "tailCall";
    public const string TypeOperand             = "typeOperand";
    public const string Visibility              = "visibility";
    public const string Indexer                 = "indexer";
    public const string ElementType             = "elementType";
    public const string ReadOnly                = "readOnly";

    public const string ParameterSpec           = "parameterSpec";             // used in MemberInfo
    public const string Parameter               = "parameter";
    public const string Parameters              = "parameters";
    public const string ParameterSpecs          = "parameterSpecs";
    public const string Variables               = "variables";
    public const string Expressions             = "expressions";

    // for JSON the comments are added as a property with a name "$comment" - one comment per JsonObject is allowed!
    public const string Comment                 = "$comment";
    public const string Schema                  = "$schema";

    public const string Expression              = "expression";
    public const string Operands                = "operands";

    public const string Body                    = "body";
    public const string Instance                = "instance";
    public const string Indexes                 = "indexes";
    public const string Delegate                = "delegate";
    public const string Arguments               = "arguments";
    public const string Left                    = "left";
    public const string Method                  = "method";
    public const string Right                   = "right";
    public const string ArrayIndex              = "arrayIndex";
    public const string Add                     = "add";
    public const string AddAssign               = "addAssign";
    public const string AddAssignChecked        = "addAssignChecked";
    public const string AddChecked              = "addChecked";
    public const string And                     = "and";
    public const string AndAlso                 = "andAlso";
    public const string AndAssign               = "andAssign";
    public const string ArrayLength             = "arrayLength";
    public const string Assign                  = "assign";
    public const string Bindings                = "bindings";
    public const string Block                   = "block";
    public const string Bounds                  = "bounds";
    public const string BreakLabel              = "breakLabel";
    public const string Case                    = "case";
    public const string Cases                   = "cases";
    public const string Catch                   = "catch";
    public const string Catches                 = "catches";
    public const string Call                    = "call";
    public const string CaseValues              = "caseValues";
    public const string Coalesce                = "coalesce";
    public const string Comparison              = "comparison";
    public const string Conditional             = "conditional";
    public const string Constant                = "constant";
    public const string Constructor             = "constructor";
    public const string ContinueLabel           = "continueLabel";
    public const string Convert                 = "convert";
    public const string ConvertChecked          = "convertChecked";
    public const string ConvertLambda           = "convertLambda";
    public const string Decrement               = "decrement";
    public const string Default                 = "default";
    public const string DefaultCase             = "defaultCase";
    public const string Divide                  = "divide";
    public const string DivideAssign            = "divideAssign";
    public const string ElementInit             = "elementInit";
    public const string ArrayElements           = "elements";
    public const string Equal                   = "equal";
    public const string Event                   = "event";
    public const string Exception               = "exception";
    public const string ExclusiveOr             = "exclusiveOr";
    public const string ExclusiveOrAssign       = "exclusiveOrAssign";
    public const string Fault                   = "fault";
    public const string Field                   = "field";
    public const string Filter                  = "filter";
    public const string Finally                 = "finally";
    public const string Goto                    = "goto";
    public const string GreaterThan             = "greaterThan";
    public const string GreaterThanOrEqual      = "greaterThanOrEqual";
    public const string Increment               = "increment";
    public const string Index                   = "index";
    public const string Invoke                  = "invoke";
    public const string IsFalse                 = "isFalse";
    public const string IsTrue                  = "isTrue";
    public const string Label                   = "label";
    public const string Lambda                  = "lambda";
    public const string LeftShift               = "leftShift";
    public const string LeftShiftAssign         = "leftShiftAssign";
    public const string LessThan                = "lessThan";
    public const string LessThanOrEqual         = "lessThanOrEqual";
    public const string ListInit                = "listInit";
    public const string Initializers            = "initializers";
    public const string Loop                    = "loop";
    public const string MemberAccess            = "memberAccess";
    public const string MemberInit              = "memberInit";
    public const string Members                 = "members";
    public const string Modulo                  = "modulo";
    public const string ModuloAssign            = "moduloAssign";
    public const string Multiply                = "multiply";
    public const string MultiplyAssign          = "multiplyAssign";
    public const string MultiplyAssignChecked   = "multiplyAssignChecked";
    public const string MultiplyChecked         = "multiplyChecked";
    public const string Negate                  = "negate";
    public const string NegateChecked           = "negateChecked";
    public const string New                     = "new";
    public const string NewArrayBounds          = "newArrayBounds";
    public const string NewArrayInit            = "newArrayInit";
    public const string Not                     = "not";
    public const string NotEqual                = "notEqual";
    public const string OnesComplement          = "onesComplement";
    public const string Or                      = "or";
    public const string OrAssign                = "orAssign";
    public const string OrElse                  = "orElse";
    public const string PostDecrementAssign     = "postDecrementAssign";
    public const string PostIncrementAssign     = "postIncrementAssign";
    public const string Power                   = "power";
    public const string PowerAssign             = "powerAssign";
    public const string PreDecrementAssign      = "preDecrementAssign";
    public const string PreIncrementAssign      = "preIncrementAssign";
    public const string Property                = "property";
    public const string Quote                   = "quote";
    public const string RightShift              = "rightShift";
    public const string RightShiftAssign        = "rightShiftAssign";
    public const string Subtract                = "subtract";
    public const string SubtractAssign          = "subtractAssign";
    public const string SubtractAssignChecked   = "subtractAssignChecked";
    public const string SubtractChecked         = "subtractChecked";
    public const string Switch                  = "switch";
    public const string If                      = "if";
    public const string Then                    = "then";
    public const string Else                    = "else";
    public const string LabelTarget             = "target";
    public const string Throw                   = "throw";
    public const string Try                     = "try";
    public const string TypeAs                  = "typeAs";
    public const string TypeEqual               = "typeEqual";
    public const string TypeIs                  = "typeIs";
    public const string UnaryPlus               = "unaryPlus";
    public const string Unbox                   = "unbox";
    public const string AssignmentBinding       = "assignmentBinding";
    public const string MemberMemberBinding     = "memberMemberBinding";
    public const string MemberListBinding       = "memberListBinding";

#pragma warning restore CS1591

    static readonly Dictionary<Type, string> _typesToNames_ = new()
    {
        [typeof(void)]                          = Void, // used only for method return values!
        [typeof(bool)]                          = Boolean,
        [typeof(byte)]                          = Byte,
        [typeof(char)]                          = Char,
        [typeof(DateTime)]                      = DateTime,
        [typeof(DateTimeOffset)]                = DateTimeOffset,
        [typeof(DBNull)]                        = DBNull,
        [typeof(decimal)]                       = Decimal,
        [typeof(double)]                        = Double,
        [typeof(float)]                         = Float,
        [typeof(Guid)]                          = Guid,
        [typeof(Half)]                          = Half,
        [typeof(int)]                           = Int,
        [typeof(IntPtr)]                        = IntPtr,
        [typeof(long)]                          = Long,
        [typeof(sbyte)]                         = SignedByte,
        [typeof(short)]                         = Short,
        [typeof(string)]                        = String,
        [typeof(TimeSpan)]                      = Duration,
        [typeof(uint)]                          = UnsignedInt,
        [typeof(UIntPtr)]                       = UnsignedIntPtr,
        [typeof(ulong)]                         = UnsignedLong,
        [typeof(Uri)]                           = Uri,
        [typeof(ushort)]                        = UnsignedShort,
        [typeof(Enum)]                          = Enum,
        [typeof(Nullable<>)]                    = Nullable,
        [typeof(object)]                        = Object,
    };

    /// <summary>
    /// Maps basic types to names used in text documents.
    /// </summary>
    public static readonly FrozenDictionary<Type, string> TypesToNames = _typesToNames_.ToFrozenDictionary();

    static readonly Dictionary<string, Type> _namesToTypes_ = new ()
    {
        [Void]                                  = typeof(void), // used only for method return values!
        [Boolean]                               = typeof(bool),
        [Byte]                                  = typeof(byte),
        [Char]                                  = typeof(char),
        [DateTime]                              = typeof(DateTime),
        [DateTimeOffset]                        = typeof(DateTimeOffset),
        [DBNull]                                = typeof(DBNull),
        [Decimal]                               = typeof(decimal),
        [Double]                                = typeof(double),
        [Float]                                 = typeof(float),
        [Guid]                                  = typeof(Guid),
        [Half]                                  = typeof(Half),
        [Int]                                   = typeof(int),
        [IntPtr]                                = typeof(IntPtr),
        [Long]                                  = typeof(long),
        [SignedByte]                            = typeof(sbyte),
        [Short]                                 = typeof(short),
        [String]                                = typeof(string),
        [Duration]                              = typeof(TimeSpan),
        [UnsignedInt]                           = typeof(uint),
        [UnsignedIntPtr]                        = typeof(UIntPtr),
        [UnsignedLong]                          = typeof(ulong),
        [Uri]                                   = typeof(Uri),
        [UnsignedShort]                         = typeof(ushort),
        [Enum]                                  = typeof(Enum),
        [Nullable]                              = typeof(Nullable<>),
        [Object]                                = typeof(object),
    };
    /// <summary>
    /// Maps names used in text documents to basic types.
    /// </summary>
    public static readonly FrozenDictionary<string, Type> NamesToTypes = _namesToTypes_.ToFrozenDictionary();
}
