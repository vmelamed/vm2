﻿namespace vm2.ExpressionSerialization.XmlTransform;
static class ElementNames
{
    public static XName Expression => Namespaces.Exs + Transform.NExpression;
    public static XName AnyURI => Namespaces.Exs + Transform.NUri;
    public static XName Boolean => Namespaces.Exs + Transform.NBool;
    public static XName Byte => Namespaces.Exs + Transform.NByte;
    public static XName Char => Namespaces.Exs + Transform.NChar;
    public static XName DateTime => Namespaces.Exs + Transform.NDateTime;
    public static XName DateTimeOffset => Namespaces.Exs + Transform.NDateTimeOffset;
    public static XName DBNull => Namespaces.Exs + Transform.NDBNull;
    public static XName Decimal => Namespaces.Exs + Transform.NDecimal;
    public static XName Double => Namespaces.Exs + Transform.NDouble;
    public static XName Duration => Namespaces.Exs + Transform.NTimeSpan;
    public static XName Float => Namespaces.Exs + Transform.NFloat;
    public static XName Guid => Namespaces.Exs + Transform.NGuid;
    public static XName Half => Namespaces.Exs + Transform.NHalf;
    public static XName Int => Namespaces.Exs + Transform.NInt;
    public static XName IntPtr => Namespaces.Exs + Transform.NIntPtr;
    public static XName Long => Namespaces.Exs + Transform.NLong;
    public static XName Short => Namespaces.Exs + Transform.NShort;
    public static XName SignedByte => Namespaces.Exs + Transform.NSByte;
    public static XName String => Namespaces.Exs + Transform.NString;
    public static XName UnsignedInt => Namespaces.Exs + Transform.NUInt;
    public static XName UnsignedIntPtr => Namespaces.Exs + Transform.NUIntPtr;
    public static XName UnsignedLong => Namespaces.Exs + Transform.NULong;
    public static XName UnsignedShort => Namespaces.Exs + Transform.NUShort;
    public static XName Enum => Namespaces.Exs + Transform.NEnum;
    public static XName Nullable => Namespaces.Exs + Transform.NNullable;
    public static XName Object => Namespaces.Exs + Transform.NObject;
    public static XName Anonymous => Namespaces.Exs + Transform.NAnonymous;
    public static XName ByteSequence => Namespaces.Exs + Transform.NByteSequence;
    public static XName Collection => Namespaces.Exs + Transform.NCollection;
    public static XName Dictionary => Namespaces.Exs + Transform.NDictionary;
    public static XName KeyValuePair => Namespaces.Exs + Transform.NKeyValuePair;
    public static XName Tuple => Namespaces.Exs + Transform.NTuple;
    public static XName TupleItem => Namespaces.Exs + Transform.NTupleItem;
    public static XName Arguments => Namespaces.Exs + Transform.NArguments;
    public static XName ParameterSpec => Namespaces.Exs + Transform.NParameterSpec;
    public static XName ParameterDefinition => Namespaces.Exs + Transform.NParameterDefinition;
    public static XName ParameterReference => Namespaces.Exs + Transform.NParameterReference;
    public static XName Parameters => Namespaces.Exs + Transform.NParameters;
    public static XName ParameterSpecs => Namespaces.Exs + Transform.NParameterSpecs;
    public static XName VariableDefinition => Namespaces.Exs + Transform.NVariableDefinition;
    public static XName VariableReference => Namespaces.Exs + Transform.NVariableReference;
    public static XName Variables => Namespaces.Exs + Transform.NVariables;
    public static XName Body => Namespaces.Exs + Transform.NBody;
    public static XName Indexes => Namespaces.Exs + Transform.NIndexes;
    public static XName IsLiftedToNull => Namespaces.Exs + Transform.NIsLiftedToNull;
    public static XName Left => Namespaces.Exs + Transform.NLeft;
    public static XName Method => Namespaces.Exs + Transform.NMethod;
    public static XName Right => Namespaces.Exs + Transform.NRight;
    public static XName ArrayIndex => Namespaces.Exs + Transform.NArrayIndex;
    public static XName Add => Namespaces.Exs + Transform.NAdd;
    public static XName AddAssign => Namespaces.Exs + Transform.NAddAssign;
    public static XName AddAssignChecked => Namespaces.Exs + Transform.NAddAssignChecked;
    public static XName AddChecked => Namespaces.Exs + Transform.NAddChecked;
    public static XName And => Namespaces.Exs + Transform.NAnd;
    public static XName AndAlso => Namespaces.Exs + Transform.NAndAlso;
    public static XName AndAssign => Namespaces.Exs + Transform.NAndAssign;
    public static XName ArrayLength => Namespaces.Exs + Transform.NArrayLength;
    public static XName Assign => Namespaces.Exs + Transform.NAssign;
    public static XName Bindings => Namespaces.Exs + Transform.NBindings;
    public static XName Block => Namespaces.Exs + Transform.NBlock;
    public static XName Bounds => Namespaces.Exs + Transform.NBounds;
    public static XName BreakLabel => Namespaces.Exs + Transform.NBreakLabel;
    public static XName Case => Namespaces.Exs + Transform.NCase;
    public static XName Catch => Namespaces.Exs + Transform.NCatch;
    public static XName Call => Namespaces.Exs + Transform.NCall;
    public static XName CaseValues => Namespaces.Exs + Transform.NCaseValues;
    public static XName Coalesce => Namespaces.Exs + Transform.NCoalesce;
    public static XName Conditional => Namespaces.Exs + Transform.NConditional;
    public static XName Constant => Namespaces.Exs + Transform.NConstant;
    public static XName Constructor => Namespaces.Exs + Transform.NConstructor;
    public static XName ContinueLabel => Namespaces.Exs + Transform.NContinueLabel;
    public static XName Convert => Namespaces.Exs + Transform.NConvert;
    public static XName ConvertChecked => Namespaces.Exs + Transform.NConvertChecked;
    public static XName Decrement => Namespaces.Exs + Transform.NDecrement;
    public static XName Default => Namespaces.Exs + Transform.NDefault;
    public static XName DefaultCase => Namespaces.Exs + Transform.NDefaultCase;
    public static XName Divide => Namespaces.Exs + Transform.NDivide;
    public static XName DivideAssign => Namespaces.Exs + Transform.NDivideAssign;
    public static XName ElementInit => Namespaces.Exs + Transform.NElementInit;
    public static XName ArrayElements => Namespaces.Exs + Transform.NArrayElements;
    public static XName Equal => Namespaces.Exs + Transform.NEqual;
    public static XName Event => Namespaces.Exs + Transform.NEvent;
    public static XName Exception => Namespaces.Exs + Transform.NException;
    public static XName ExclusiveOr => Namespaces.Exs + Transform.NExclusiveOr;
    public static XName ExclusiveOrAssign => Namespaces.Exs + Transform.NExclusiveOrAssign;
    public static XName Fault => Namespaces.Exs + Transform.NFault;
    public static XName Field => Namespaces.Exs + Transform.NField;
    public static XName Filter => Namespaces.Exs + Transform.NFilter;
    public static XName Finally => Namespaces.Exs + Transform.NFinally;
    public static XName Goto => Namespaces.Exs + Transform.NGoto;
    public static XName GreaterThan => Namespaces.Exs + Transform.NGreaterThan;
    public static XName GreaterThanOrEqual => Namespaces.Exs + Transform.NGreaterThanOrEqual;
    public static XName Increment => Namespaces.Exs + Transform.NIncrement;
    public static XName Index => Namespaces.Exs + Transform.NIndex;
    public static XName Invoke => Namespaces.Exs + Transform.NInvoke;
    public static XName IsFalse => Namespaces.Exs + Transform.NIsFalse;
    public static XName IsTrue => Namespaces.Exs + Transform.NIsTrue;
    public static XName Label => Namespaces.Exs + Transform.NLabel;
    public static XName Lambda => Namespaces.Exs + Transform.NLambda;
    public static XName LeftShift => Namespaces.Exs + Transform.NLeftShift;
    public static XName LeftShiftAssign => Namespaces.Exs + Transform.NLeftShiftAssign;
    public static XName LessThan => Namespaces.Exs + Transform.NLessThan;
    public static XName LessThanOrEqual => Namespaces.Exs + Transform.NLessThanOrEqual;
    public static XName ListInit => Namespaces.Exs + Transform.NListInit;
    public static XName Loop => Namespaces.Exs + Transform.NLoop;
    public static XName MemberAccess => Namespaces.Exs + Transform.NMemberAccess;
    public static XName MemberInit => Namespaces.Exs + Transform.NMemberInit;
    public static XName Members => Namespaces.Exs + Transform.NMembers;
    public static XName Modulo => Namespaces.Exs + Transform.NModulo;
    public static XName ModuloAssign => Namespaces.Exs + Transform.NModuloAssign;
    public static XName Multiply => Namespaces.Exs + Transform.NMultiply;
    public static XName MultiplyAssign => Namespaces.Exs + Transform.NMultiplyAssign;
    public static XName MultiplyAssignChecked => Namespaces.Exs + Transform.NMultiplyAssignChecked;
    public static XName MultiplyChecked => Namespaces.Exs + Transform.NMultiplyChecked;
    public static XName Negate => Namespaces.Exs + Transform.NNegate;
    public static XName NegateChecked => Namespaces.Exs + Transform.NNegateChecked;
    public static XName New => Namespaces.Exs + Transform.NNew;
    public static XName NewArrayBounds => Namespaces.Exs + Transform.NNewArrayBounds;
    public static XName NewArrayInit => Namespaces.Exs + Transform.NNewArrayInit;
    public static XName Not => Namespaces.Exs + Transform.NNot;
    public static XName NotEqual => Namespaces.Exs + Transform.NNotEqual;
    public static XName OnesComplement => Namespaces.Exs + Transform.NOnesComplement;
    public static XName Or => Namespaces.Exs + Transform.NOr;
    public static XName OrAssign => Namespaces.Exs + Transform.NOrAssign;
    public static XName OrElse => Namespaces.Exs + Transform.NOrElse;
    public static XName PostDecrementAssign => Namespaces.Exs + Transform.NPostDecrementAssign;
    public static XName PostIncrementAssign => Namespaces.Exs + Transform.NPostIncrementAssign;
    public static XName Power => Namespaces.Exs + Transform.NPower;
    public static XName PowerAssign => Namespaces.Exs + Transform.NPowerAssign;
    public static XName PreDecrementAssign => Namespaces.Exs + Transform.NPreDecrementAssign;
    public static XName PreIncrementAssign => Namespaces.Exs + Transform.NPreIncrementAssign;
    public static XName Property => Namespaces.Exs + Transform.NProperty;
    public static XName Quote => Namespaces.Exs + Transform.NQuote;
    public static XName RightShift => Namespaces.Exs + Transform.NRightShift;
    public static XName RightShiftAssign => Namespaces.Exs + Transform.NRightShiftAssign;
    public static XName Subtract => Namespaces.Exs + Transform.NSubtract;
    public static XName SubtractAssign => Namespaces.Exs + Transform.NSubtractAssign;
    public static XName SubtractAssignChecked => Namespaces.Exs + Transform.NSubtractAssignChecked;
    public static XName SubtractChecked => Namespaces.Exs + Transform.NSubtractChecked;
    public static XName Switch => Namespaces.Exs + Transform.NSwitch;
    public static XName Target => Namespaces.Exs + Transform.NTarget;
    public static XName Throw => Namespaces.Exs + Transform.NThrow;
    public static XName Try => Namespaces.Exs + Transform.NTry;
    public static XName TypeAs => Namespaces.Exs + Transform.NTypeAs;
    public static XName TypeEqual => Namespaces.Exs + Transform.NTypeEqual;
    public static XName TypeIs => Namespaces.Exs + Transform.NTypeIs;
    public static XName UnaryPlus => Namespaces.Exs + Transform.NUnaryPlus;
    public static XName Unbox => Namespaces.Exs + Transform.NUnbox;
    public static XName AssignmentBinding => Namespaces.Exs + Transform.NAssignmentBinding;
    public static XName MemberMemberBinding => Namespaces.Exs + Transform.NMemberMemberBinding;
    public static XName MemberListBinding => Namespaces.Exs + Transform.NMemberListBinding;
}
