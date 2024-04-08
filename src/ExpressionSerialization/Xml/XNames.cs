namespace vm2.ExpressionSerialization.Xml;

static class XmlNamespace
{
    /// <summary>
    /// The XML namespace of the W3C schema definition - http://www.w3.org/2001/XMLSchema
    /// </summary>
    public static readonly XNamespace Xsd  = XNamespace.Get("http://www.w3.org/2001/XMLSchema");

    /// <summary>
    /// The XML namespace of the W3C instance schema definition - http://www.w3.org/2001/XMLSchema-instance
    /// </summary>
    public static readonly XNamespace Xsi  = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");

    /// <summary>
    /// The XML namespace of the Microsoft serialization schema definition - http://schemas.microsoft.com/2003/10/Serialization/
    /// </summary>
    public static readonly XNamespace Ser  = XNamespace.Get("http://schemas.microsoft.com/2003/10/Serialization/");

    /// <summary>
    /// The XML namespace object representing the namespace of the data contracts - http://schemas.datacontract.org/2004/07/System
    /// </summary>
    public static readonly XNamespace Dcs = XNamespace.Get("http://schemas.datacontract.org/2004/07/System");

    /// <summary>
    /// The XML namespace object representing the namespace of the Aspects' expression serialization - urn:schemas-vm-com:Aspects.Linq.Expressions.Serialization
    /// </summary>
    public static readonly XNamespace Xxp = XNamespace.Get("urn:schemas-vm-com:Aspects.Linq.Expressions.Serialization");
}

static class XmlElement
{
    public static readonly XName Expression             = XmlNamespace.Xxp + "expression";

    public static readonly XName Boolean                = XmlNamespace.Xxp + "boolean";
    public static readonly XName UnsignedByte           = XmlNamespace.Xxp + "unsignedByte";
    public static readonly XName Byte                   = XmlNamespace.Xxp + "byte";
    public static readonly XName Short                  = XmlNamespace.Xxp + "short";
    public static readonly XName UnsignedShort          = XmlNamespace.Xxp + "unsignedShort";
    public static readonly XName Int                    = XmlNamespace.Xxp + "int";
    public static readonly XName UnsignedInt            = XmlNamespace.Xxp + "unsignedInt";
    public static readonly XName Long                   = XmlNamespace.Xxp + "long";
    public static readonly XName UnsignedLong           = XmlNamespace.Xxp + "unsignedLong";
    public static readonly XName Half                   = XmlNamespace.Xxp + "half";
    public static readonly XName Float                  = XmlNamespace.Xxp + "float";
    public static readonly XName Double                 = XmlNamespace.Xxp + "double";
    public static readonly XName Decimal                = XmlNamespace.Xxp + "decimal";
    public static readonly XName Guid                   = XmlNamespace.Xxp + "guid";
    public static readonly XName AnyURI                 = XmlNamespace.Xxp + "anyURI";
    public static readonly XName Duration               = XmlNamespace.Xxp + "duration";
    public static readonly XName String                 = XmlNamespace.Xxp + "string";
    public static readonly XName Char                   = XmlNamespace.Xxp + "char";
    public static readonly XName DateTime               = XmlNamespace.Xxp + "dateTime";
    public static readonly XName DBNull                 = XmlNamespace.Xxp + "dbNull";

    public static readonly XName Nullable               = XmlNamespace.Xxp + "nullable";
    public static readonly XName Enum                   = XmlNamespace.Xxp + "enum";
    public static readonly XName Custom                 = XmlNamespace.Xxp + "custom";
    public static readonly XName Anonymous              = XmlNamespace.Xxp + "anonymous";

    public static readonly XName Body                   = XmlNamespace.Xxp + "body";
    public static readonly XName Indexes                = XmlNamespace.Xxp + "indexes";
    public static readonly XName IsLiftedToNull         = XmlNamespace.Xxp + "isLiftedToNull";
    public static readonly XName Left                   = XmlNamespace.Xxp + "left";
    public static readonly XName Method                 = XmlNamespace.Xxp + "method";
    public static readonly XName Parameters             = XmlNamespace.Xxp + "parameters";
    public static readonly XName Right                  = XmlNamespace.Xxp + "right";
    public static readonly XName Variables              = XmlNamespace.Xxp + "variables";

    public static readonly XName Arguments              = XmlNamespace.Xxp + "arguments";
    public static readonly XName ArrayIndex             = XmlNamespace.Xxp + "arrayIndex";
    public static readonly XName Add                    = XmlNamespace.Xxp + "add";
    public static readonly XName AddAssign              = XmlNamespace.Xxp + "addAssign";
    public static readonly XName AddAssignChecked       = XmlNamespace.Xxp + "addAssignChecked";
    public static readonly XName AddChecked             = XmlNamespace.Xxp + "addChecked";
    public static readonly XName And                    = XmlNamespace.Xxp + "and";
    public static readonly XName AndAlso                = XmlNamespace.Xxp + "andAlso";
    public static readonly XName AndAssign              = XmlNamespace.Xxp + "andAssign";
    public static readonly XName ArrayLength            = XmlNamespace.Xxp + "arrayLength";
    public static readonly XName Assign                 = XmlNamespace.Xxp + "assign";
    public static readonly XName Bindings               = XmlNamespace.Xxp + "bindings";
    public static readonly XName Block                  = XmlNamespace.Xxp + "block";
    public static readonly XName Bounds                 = XmlNamespace.Xxp + "bounds";
    public static readonly XName BreakLabel             = XmlNamespace.Xxp + "breakLabel";
    public static readonly XName Case                   = XmlNamespace.Xxp + "case";
    public static readonly XName Catch                  = XmlNamespace.Xxp + "catch";
    public static readonly XName Call                   = XmlNamespace.Xxp + "call";
    public static readonly XName Coalesce               = XmlNamespace.Xxp + "coalesce";
    public static readonly XName Conditional            = XmlNamespace.Xxp + "conditional";
    public static readonly XName Constant               = XmlNamespace.Xxp + "constant";
    public static readonly XName Constructor            = XmlNamespace.Xxp + "constructor";
    public static readonly XName ContinueLabel          = XmlNamespace.Xxp + "continueLabel";
    public static readonly XName Convert                = XmlNamespace.Xxp + "convert";
    public static readonly XName ConvertChecked         = XmlNamespace.Xxp + "convertChecked";
    public static readonly XName DebugInfo              = XmlNamespace.Xxp + "debugInfo";
    public static readonly XName Decrement              = XmlNamespace.Xxp + "decrement";
    public static readonly XName Default                = XmlNamespace.Xxp + "default";
    public static readonly XName DefaultCase            = XmlNamespace.Xxp + "defaultCase";
    public static readonly XName Divide                 = XmlNamespace.Xxp + "divide";
    public static readonly XName DivideAssign           = XmlNamespace.Xxp + "divideAssign";
    public static readonly XName Dynamic                = XmlNamespace.Xxp + "dynamic";
    public static readonly XName ElementInit            = XmlNamespace.Xxp + "elementInit";
    public static readonly XName ArrayElements          = XmlNamespace.Xxp + "elements";
    public static readonly XName Equal                  = XmlNamespace.Xxp + "equal";
    public static readonly XName Event                  = XmlNamespace.Xxp + "event";
    public static readonly XName Exception              = XmlNamespace.Xxp + "exception";
    public static readonly XName ExclusiveOr            = XmlNamespace.Xxp + "exclusiveOr";
    public static readonly XName ExclusiveOrAssign      = XmlNamespace.Xxp + "exclusiveOrAssign";
    public static readonly XName Extension              = XmlNamespace.Xxp + "extension";
    public static readonly XName Fault                  = XmlNamespace.Xxp + "fault";
    public static readonly XName Field                  = XmlNamespace.Xxp + "field";
    public static readonly XName Filter                 = XmlNamespace.Xxp + "filter";
    public static readonly XName Finally                = XmlNamespace.Xxp + "finally";
    public static readonly XName Goto                   = XmlNamespace.Xxp + "goto";
    public static readonly XName GreaterThan            = XmlNamespace.Xxp + "greaterThan";
    public static readonly XName GreaterThanOrEqual     = XmlNamespace.Xxp + "greaterThanOrEqual";
    public static readonly XName Increment              = XmlNamespace.Xxp + "increment";
    public static readonly XName Index                  = XmlNamespace.Xxp + "index";
    public static readonly XName Invoke                 = XmlNamespace.Xxp + "invoke";
    public static readonly XName IsFalse                = XmlNamespace.Xxp + "isFalse";
    public static readonly XName IsTrue                 = XmlNamespace.Xxp + "isTrue";
    public static readonly XName Label                  = XmlNamespace.Xxp + "label";
    public static readonly XName Lambda                 = XmlNamespace.Xxp + "lambda";
    public static readonly XName LeftShift              = XmlNamespace.Xxp + "leftShift";
    public static readonly XName LeftShiftAssign        = XmlNamespace.Xxp + "leftShiftAssign";
    public static readonly XName LessThan               = XmlNamespace.Xxp + "lessThan";
    public static readonly XName LessThanOrEqual        = XmlNamespace.Xxp + "lessThanOrEqual";
    public static readonly XName ListInit               = XmlNamespace.Xxp + "listInit";
    public static readonly XName Loop                   = XmlNamespace.Xxp + "loop";
    public static readonly XName LabelTarget            = XmlNamespace.Xxp + "labelTarget";
    public static readonly XName MemberAccess           = XmlNamespace.Xxp + "memberAccess";
    public static readonly XName MemberInit             = XmlNamespace.Xxp + "memberInit";
    public static readonly XName Members                = XmlNamespace.Xxp + "members";
    public static readonly XName Modulo                 = XmlNamespace.Xxp + "modulo";
    public static readonly XName ModuloAssign           = XmlNamespace.Xxp + "moduloAssign";
    public static readonly XName Multiply               = XmlNamespace.Xxp + "multiply";
    public static readonly XName MultiplyAssign         = XmlNamespace.Xxp + "multiplyAssign";
    public static readonly XName MultiplyAssignChecked  = XmlNamespace.Xxp + "multiplyAssignChecked";
    public static readonly XName MultiplyChecked        = XmlNamespace.Xxp + "multiplyChecked";
    public static readonly XName Negate                 = XmlNamespace.Xxp + "negate";
    public static readonly XName NegateChecked          = XmlNamespace.Xxp + "negateChecked";
    public static readonly XName New                    = XmlNamespace.Xxp + "new";
    public static readonly XName NewArrayBounds         = XmlNamespace.Xxp + "newArrayBounds";
    public static readonly XName NewArrayInit           = XmlNamespace.Xxp + "newArrayInit";
    public static readonly XName Not                    = XmlNamespace.Xxp + "not";
    public static readonly XName NotEqual               = XmlNamespace.Xxp + "notEqual";
    public static readonly XName OnesComplement         = XmlNamespace.Xxp + "onesComplement";
    public static readonly XName Or                     = XmlNamespace.Xxp + "or";
    public static readonly XName OrAssign               = XmlNamespace.Xxp + "orAssign";
    public static readonly XName OrElse                 = XmlNamespace.Xxp + "orElse";
    public static readonly XName Parameter              = XmlNamespace.Xxp + "parameter";
    public static readonly XName PostDecrementAssign    = XmlNamespace.Xxp + "postDecrementAssign";
    public static readonly XName PostIncrementAssign    = XmlNamespace.Xxp + "postIncrementAssign";
    public static readonly XName Power                  = XmlNamespace.Xxp + "power";
    public static readonly XName PowerAssign            = XmlNamespace.Xxp + "powerAssign";
    public static readonly XName PreDecrementAssign     = XmlNamespace.Xxp + "preDecrementAssign";
    public static readonly XName PreIncrementAssign     = XmlNamespace.Xxp + "preIncrementAssign";
    public static readonly XName Property               = XmlNamespace.Xxp + "property";
    public static readonly XName Quote                  = XmlNamespace.Xxp + "quote";
    public static readonly XName RightShift             = XmlNamespace.Xxp + "rightShift";
    public static readonly XName RightShiftAssign       = XmlNamespace.Xxp + "rightShiftAssign";
    public static readonly XName RuntimeVariables       = XmlNamespace.Xxp + "runtimeVariables";
    public static readonly XName Subtract               = XmlNamespace.Xxp + "subtract";
    public static readonly XName SubtractAssign         = XmlNamespace.Xxp + "subtractAssign";
    public static readonly XName SubtractAssignChecked  = XmlNamespace.Xxp + "subtractAssignChecked";
    public static readonly XName SubtractChecked        = XmlNamespace.Xxp + "subtractChecked";
    public static readonly XName Switch                 = XmlNamespace.Xxp + "switch";
    public static readonly XName Throw                  = XmlNamespace.Xxp + "throw";
    public static readonly XName Try                    = XmlNamespace.Xxp + "try";
    public static readonly XName TypeAs                 = XmlNamespace.Xxp + "typeAs";
    public static readonly XName TypeEqual              = XmlNamespace.Xxp + "typeEqual";
    public static readonly XName TypeIs                 = XmlNamespace.Xxp + "typeIs";
    public static readonly XName UnaryPlus              = XmlNamespace.Xxp + "unaryPlus";
    public static readonly XName Unbox                  = XmlNamespace.Xxp + "unbox";
    public static readonly XName Value                  = XmlNamespace.Xxp + "value";

    public static readonly XName AssignmentBinding      = XmlNamespace.Xxp + "assignmentBinding";
    public static readonly XName MemberMemberBinding    = XmlNamespace.Xxp + "memberMemberBinding";
    public static readonly XName ListBinding            = XmlNamespace.Xxp + "listBinding";
}

static class XmlAttribute
{
    public static readonly XName Assembly               = "assembly";
    public static readonly XName DelegateType           = "delegateType";
    public static readonly XName Family                 = "family";
    public static readonly XName FamilyAndAssembly      = "familyAndAssembly";
    public static readonly XName FamilyOrAssembly       = "familyOrAssembly";
    public static readonly XName IsByRef                = "isByRef";
    public static readonly XName IsLiftedToNull         = "isLiftedToNull";
    public static readonly XName IsNull                 = XmlNamespace.Xsi + "nil";
    public static readonly XName IsOut                  = "isOut";
    public static readonly XName Kind                   = "kind";
    public static readonly XName Name                   = "name";
    public static readonly XName Private                = "private";
    public static readonly XName Property               = "property";
    public static readonly XName Static                 = "static";
    public static readonly XName TailCall               = "tailCall";
    public static readonly XName Type                   = "type";
    public static readonly XName Uid                    = "uid";
    public static readonly XName UidRef                 = "uidRef";
    public static readonly XName Value                  = "value";
    public static readonly XName Visibility             = "visibility";
};
