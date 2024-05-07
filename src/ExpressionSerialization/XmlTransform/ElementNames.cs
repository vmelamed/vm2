﻿namespace vm2.ExpressionSerialization.XmlTransform;
static class ElementNames
{
    public static readonly XName Expression             = Namespaces.Exs + "expression";

    public static readonly XName Boolean                = Namespaces.Exs + "boolean";
    public static readonly XName UnsignedByte           = Namespaces.Exs + "unsignedByte";
    public static readonly XName Byte                   = Namespaces.Exs + "byte";
    public static readonly XName Short                  = Namespaces.Exs + "short";
    public static readonly XName UnsignedShort          = Namespaces.Exs + "unsignedShort";
    public static readonly XName Int                    = Namespaces.Exs + "int";
    public static readonly XName UnsignedInt            = Namespaces.Exs + "unsignedInt";
    public static readonly XName Long                   = Namespaces.Exs + "long";
    public static readonly XName UnsignedLong           = Namespaces.Exs + "unsignedLong";
    public static readonly XName Half                   = Namespaces.Exs + "half";
    public static readonly XName Float                  = Namespaces.Exs + "float";
    public static readonly XName Double                 = Namespaces.Exs + "double";
    public static readonly XName Decimal                = Namespaces.Exs + "decimal";
    public static readonly XName Guid                   = Namespaces.Exs + "guid";
    public static readonly XName AnyURI                 = Namespaces.Exs + "uri";
    public static readonly XName Duration               = Namespaces.Exs + "duration";
    public static readonly XName String                 = Namespaces.Exs + "string";
    public static readonly XName Char                   = Namespaces.Exs + "char";
    public static readonly XName DateTime               = Namespaces.Exs + "dateTime";
    public static readonly XName DateTimeOffset         = Namespaces.Exs + "dateTimeOffset";
    public static readonly XName DBNull                 = Namespaces.Exs + "dbNull";
    public static readonly XName IntPtr                 = Namespaces.Exs + "intPtr";
    public static readonly XName UnsignedIntPtr         = Namespaces.Exs + "unsignedIntPtr";

    public static readonly XName Object                 = Namespaces.Exs + "object";
    public static readonly XName Nullable               = Namespaces.Exs + "nullable";
    public static readonly XName Enum                   = Namespaces.Exs + "enum";
    public static readonly XName Anonymous              = Namespaces.Exs + "anonymous";
    public static readonly XName ByteSequence           = Namespaces.Exs + "byteSequence";
    public static readonly XName Collection             = Namespaces.Exs + "collection";
    public static readonly XName Dictionary             = Namespaces.Exs + "dictionary";
    public static readonly XName KeyValuePair           = Namespaces.Exs + "key-value";
    public static readonly XName Tuple                  = Namespaces.Exs + "tuple";
    public static readonly XName TupleItem              = Namespaces.Exs + "item";

    public static readonly XName Body                   = Namespaces.Exs + "body";
    public static readonly XName Indexes                = Namespaces.Exs + "indexes";
    public static readonly XName IsLiftedToNull         = Namespaces.Exs + "isLiftedToNull";
    public static readonly XName Left                   = Namespaces.Exs + "left";
    public static readonly XName Method                 = Namespaces.Exs + "method";
    public static readonly XName ParameterSpec          = Namespaces.Exs + "parameterSpec";             // used in MemberInfo
    public static readonly XName ParameterDefinition    = Namespaces.Exs + "parameterDefinition";       // used in Lambdas
    public static readonly XName ParameterReference     = Namespaces.Exs + "parameter";
    public static readonly XName Parameters             = Namespaces.Exs + "parameters";
    public static readonly XName ParameterSpecs         = Namespaces.Exs + "parameterSpecs";
    public static readonly XName Right                  = Namespaces.Exs + "right";
    public static readonly XName VariableDefinition     = Namespaces.Exs + "variableDefinition";
    public static readonly XName VariableReference      = Namespaces.Exs + "variable";                  // used in blocks
    public static readonly XName Variables              = Namespaces.Exs + "variables";

    public static readonly XName Arguments              = Namespaces.Exs + "arguments";
    public static readonly XName ArrayIndex             = Namespaces.Exs + "arrayIndex";
    public static readonly XName Add                    = Namespaces.Exs + "add";
    public static readonly XName AddAssign              = Namespaces.Exs + "addAssign";
    public static readonly XName AddAssignChecked       = Namespaces.Exs + "addAssignChecked";
    public static readonly XName AddChecked             = Namespaces.Exs + "addChecked";
    public static readonly XName And                    = Namespaces.Exs + "and";
    public static readonly XName AndAlso                = Namespaces.Exs + "andAlso";
    public static readonly XName AndAssign              = Namespaces.Exs + "andAssign";
    public static readonly XName ArrayLength            = Namespaces.Exs + "arrayLength";
    public static readonly XName Assign                 = Namespaces.Exs + "assign";
    public static readonly XName Bindings               = Namespaces.Exs + "bindings";
    public static readonly XName Block                  = Namespaces.Exs + "block";
    public static readonly XName Bounds                 = Namespaces.Exs + "bounds";
    public static readonly XName BreakLabel             = Namespaces.Exs + "breakLabel";
    public static readonly XName Case                   = Namespaces.Exs + "case";
    public static readonly XName Catch                  = Namespaces.Exs + "catch";
    public static readonly XName Call                   = Namespaces.Exs + "call";
    public static readonly XName CaseValues             = Namespaces.Exs + "caseValues";
    public static readonly XName Coalesce               = Namespaces.Exs + "coalesce";
    public static readonly XName Conditional            = Namespaces.Exs + "conditional";
    public static readonly XName Constant               = Namespaces.Exs + "constant";
    public static readonly XName Constructor            = Namespaces.Exs + "constructor";
    public static readonly XName ContinueLabel          = Namespaces.Exs + "continueLabel";
    public static readonly XName Convert                = Namespaces.Exs + "convert";
    public static readonly XName ConvertChecked         = Namespaces.Exs + "convertChecked";
    public static readonly XName DebugInfo              = Namespaces.Exs + "debugInfo";
    public static readonly XName Decrement              = Namespaces.Exs + "decrement";
    public static readonly XName Default                = Namespaces.Exs + "default";
    public static readonly XName DefaultCase            = Namespaces.Exs + "defaultCase";
    public static readonly XName Divide                 = Namespaces.Exs + "divide";
    public static readonly XName DivideAssign           = Namespaces.Exs + "divideAssign";
    public static readonly XName Dynamic                = Namespaces.Exs + "dynamic";
    public static readonly XName ElementInit            = Namespaces.Exs + "elementInit";
    public static readonly XName ArrayElements          = Namespaces.Exs + "elements";
    public static readonly XName Equal                  = Namespaces.Exs + "equal";
    public static readonly XName Event                  = Namespaces.Exs + "event";
    public static readonly XName Exception              = Namespaces.Exs + "exception";
    public static readonly XName ExclusiveOr            = Namespaces.Exs + "exclusiveOr";
    public static readonly XName ExclusiveOrAssign      = Namespaces.Exs + "exclusiveOrAssign";
    public static readonly XName Extension              = Namespaces.Exs + "extension";
    public static readonly XName Fault                  = Namespaces.Exs + "fault";
    public static readonly XName Field                  = Namespaces.Exs + "field";
    public static readonly XName Filter                 = Namespaces.Exs + "filter";
    public static readonly XName Finally                = Namespaces.Exs + "finally";
    public static readonly XName Goto                   = Namespaces.Exs + "goto";
    public static readonly XName GreaterThan            = Namespaces.Exs + "greaterThan";
    public static readonly XName GreaterThanOrEqual     = Namespaces.Exs + "greaterThanOrEqual";
    public static readonly XName Increment              = Namespaces.Exs + "increment";
    public static readonly XName Index                  = Namespaces.Exs + "index";
    public static readonly XName Invoke                 = Namespaces.Exs + "invoke";
    public static readonly XName IsFalse                = Namespaces.Exs + "isFalse";
    public static readonly XName IsTrue                 = Namespaces.Exs + "isTrue";
    public static readonly XName Label                  = Namespaces.Exs + "label";
    public static readonly XName Lambda                 = Namespaces.Exs + "lambda";
    public static readonly XName LeftShift              = Namespaces.Exs + "leftShift";
    public static readonly XName LeftShiftAssign        = Namespaces.Exs + "leftShiftAssign";
    public static readonly XName LessThan               = Namespaces.Exs + "lessThan";
    public static readonly XName LessThanOrEqual        = Namespaces.Exs + "lessThanOrEqual";
    public static readonly XName ListInit               = Namespaces.Exs + "listInit";
    public static readonly XName Loop                   = Namespaces.Exs + "loop";
    public static readonly XName MemberAccess           = Namespaces.Exs + "memberAccess";
    public static readonly XName MemberInit             = Namespaces.Exs + "memberInit";
    public static readonly XName Members                = Namespaces.Exs + "members";
    public static readonly XName Modulo                 = Namespaces.Exs + "modulo";
    public static readonly XName ModuloAssign           = Namespaces.Exs + "moduloAssign";
    public static readonly XName Multiply               = Namespaces.Exs + "multiply";
    public static readonly XName MultiplyAssign         = Namespaces.Exs + "multiplyAssign";
    public static readonly XName MultiplyAssignChecked  = Namespaces.Exs + "multiplyAssignChecked";
    public static readonly XName MultiplyChecked        = Namespaces.Exs + "multiplyChecked";
    public static readonly XName Negate                 = Namespaces.Exs + "negate";
    public static readonly XName NegateChecked          = Namespaces.Exs + "negateChecked";
    public static readonly XName New                    = Namespaces.Exs + "new";
    public static readonly XName NewArrayBounds         = Namespaces.Exs + "newArrayBounds";
    public static readonly XName NewArrayInit           = Namespaces.Exs + "newArrayInit";
    public static readonly XName Not                    = Namespaces.Exs + "not";
    public static readonly XName NotEqual               = Namespaces.Exs + "notEqual";
    public static readonly XName OnesComplement         = Namespaces.Exs + "onesComplement";
    public static readonly XName Or                     = Namespaces.Exs + "or";
    public static readonly XName OrAssign               = Namespaces.Exs + "orAssign";
    public static readonly XName OrElse                 = Namespaces.Exs + "orElse";
    public static readonly XName PostDecrementAssign    = Namespaces.Exs + "postDecrementAssign";
    public static readonly XName PostIncrementAssign    = Namespaces.Exs + "postIncrementAssign";
    public static readonly XName Power                  = Namespaces.Exs + "power";
    public static readonly XName PowerAssign            = Namespaces.Exs + "powerAssign";
    public static readonly XName PreDecrementAssign     = Namespaces.Exs + "preDecrementAssign";
    public static readonly XName PreIncrementAssign     = Namespaces.Exs + "preIncrementAssign";
    public static readonly XName Property               = Namespaces.Exs + "property";
    public static readonly XName Quote                  = Namespaces.Exs + "quote";
    public static readonly XName RightShift             = Namespaces.Exs + "rightShift";
    public static readonly XName RightShiftAssign       = Namespaces.Exs + "rightShiftAssign";
    public static readonly XName RuntimeVariables       = Namespaces.Exs + "runtimeVariables";
    public static readonly XName Subtract               = Namespaces.Exs + "subtract";
    public static readonly XName SubtractAssign         = Namespaces.Exs + "subtractAssign";
    public static readonly XName SubtractAssignChecked  = Namespaces.Exs + "subtractAssignChecked";
    public static readonly XName SubtractChecked        = Namespaces.Exs + "subtractChecked";
    public static readonly XName Switch                 = Namespaces.Exs + "switch";
    public static readonly XName Target                 = Namespaces.Exs + "target";
    public static readonly XName Throw                  = Namespaces.Exs + "throw";
    public static readonly XName Try                    = Namespaces.Exs + "try";
    public static readonly XName TypeAs                 = Namespaces.Exs + "typeAs";
    public static readonly XName TypeEqual              = Namespaces.Exs + "typeEqual";
    public static readonly XName TypeIs                 = Namespaces.Exs + "typeIs";
    public static readonly XName UnaryPlus              = Namespaces.Exs + "unaryPlus";
    public static readonly XName Unbox                  = Namespaces.Exs + "unbox";

    public static readonly XName AssignmentBinding      = Namespaces.Exs + "assignmentBinding";
    public static readonly XName MemberMemberBinding    = Namespaces.Exs + "memberMemberBinding";
    public static readonly XName MemberListBinding      = Namespaces.Exs + "memberListBinding";
}
