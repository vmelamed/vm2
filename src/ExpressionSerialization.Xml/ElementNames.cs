﻿namespace vm2.ExpressionSerialization.Xml;

static class ElementNames
{
    public static XName Expression => Namespaces.Exs + Vocabulary.Expression;
    public static XName Uri => Namespaces.Exs + Vocabulary.Uri;
    public static XName Boolean => Namespaces.Exs + Vocabulary.Boolean;
    public static XName Byte => Namespaces.Exs + Vocabulary.Byte;
    public static XName Char => Namespaces.Exs + Vocabulary.Char;
    public static XName DateTime => Namespaces.Exs + Vocabulary.DateTime;
    public static XName DateTimeOffset => Namespaces.Exs + Vocabulary.DateTimeOffset;
    public static XName DBNull => Namespaces.Exs + Vocabulary.DBNull;
    public static XName Decimal => Namespaces.Exs + Vocabulary.Decimal;
    public static XName Double => Namespaces.Exs + Vocabulary.Double;
    public static XName Duration => Namespaces.Exs + Vocabulary.Duration;
    public static XName Float => Namespaces.Exs + Vocabulary.Float;
    public static XName Guid => Namespaces.Exs + Vocabulary.Guid;
    public static XName Half => Namespaces.Exs + Vocabulary.Half;
    public static XName Int => Namespaces.Exs + Vocabulary.Int;
    public static XName IntPtr => Namespaces.Exs + Vocabulary.IntPtr;
    public static XName Long => Namespaces.Exs + Vocabulary.Long;
    public static XName Short => Namespaces.Exs + Vocabulary.Short;
    public static XName SignedByte => Namespaces.Exs + Vocabulary.SignedByte;
    public static XName String => Namespaces.Exs + Vocabulary.String;
    public static XName UnsignedInt => Namespaces.Exs + Vocabulary.UnsignedInt;
    public static XName UnsignedIntPtr => Namespaces.Exs + Vocabulary.UnsignedIntPtr;
    public static XName UnsignedLong => Namespaces.Exs + Vocabulary.UnsignedLong;
    public static XName UnsignedShort => Namespaces.Exs + Vocabulary.UnsignedShort;
    public static XName Enum => Namespaces.Exs + Vocabulary.Enum;
    public static XName Nullable => Namespaces.Exs + Vocabulary.Nullable;
    public static XName Object => Namespaces.Exs + Vocabulary.Object;
    public static XName Anonymous => Namespaces.Exs + Vocabulary.Anonymous;
    public static XName ByteSequence => Namespaces.Exs + Vocabulary.ByteSequence;
    public static XName Collection => Namespaces.Exs + Vocabulary.Sequence;
    public static XName Dictionary => Namespaces.Exs + Vocabulary.Dictionary;
    public static XName KeyValuePair => Namespaces.Exs + Vocabulary.KeyValuePair;
    public static XName Tuple => Namespaces.Exs + Vocabulary.Tuple;
    public static XName TupleItem => Namespaces.Exs + Vocabulary.TupleItem;
    public static XName Arguments => Namespaces.Exs + Vocabulary.Arguments;
    public static XName ParameterSpec => Namespaces.Exs + Vocabulary.ParameterSpec;
    public static XName Parameter => Namespaces.Exs + Vocabulary.Parameter;
    public static XName Parameters => Namespaces.Exs + Vocabulary.Parameters;
    public static XName ParameterSpecs => Namespaces.Exs + Vocabulary.ParameterSpecs;
    public static XName Variables => Namespaces.Exs + Vocabulary.Variables;
    public static XName Body => Namespaces.Exs + Vocabulary.Body;
    public static XName Indexes => Namespaces.Exs + Vocabulary.Indexes;
    public static XName IsLiftedToNull => Namespaces.Exs + Vocabulary.IsLiftedToNull;
    public static XName Left => Namespaces.Exs + Vocabulary.Left;
    public static XName Method => Namespaces.Exs + Vocabulary.Method;
    public static XName Right => Namespaces.Exs + Vocabulary.Right;
    public static XName ArrayIndex => Namespaces.Exs + Vocabulary.ArrayIndex;
    public static XName Add => Namespaces.Exs + Vocabulary.Add;
    public static XName AddAssign => Namespaces.Exs + Vocabulary.AddAssign;
    public static XName AddAssignChecked => Namespaces.Exs + Vocabulary.AddAssignChecked;
    public static XName AddChecked => Namespaces.Exs + Vocabulary.AddChecked;
    public static XName And => Namespaces.Exs + Vocabulary.And;
    public static XName AndAlso => Namespaces.Exs + Vocabulary.AndAlso;
    public static XName AndAssign => Namespaces.Exs + Vocabulary.AndAssign;
    public static XName ArrayLength => Namespaces.Exs + Vocabulary.ArrayLength;
    public static XName Assign => Namespaces.Exs + Vocabulary.Assign;
    public static XName Bindings => Namespaces.Exs + Vocabulary.Bindings;
    public static XName Block => Namespaces.Exs + Vocabulary.Block;
    public static XName Bounds => Namespaces.Exs + Vocabulary.Bounds;
    public static XName BreakLabel => Namespaces.Exs + Vocabulary.BreakLabel;
    public static XName Case => Namespaces.Exs + Vocabulary.Case;
    public static XName Catch => Namespaces.Exs + Vocabulary.Catch;
    public static XName Call => Namespaces.Exs + Vocabulary.Call;
    public static XName CaseValues => Namespaces.Exs + Vocabulary.CaseValues;
    public static XName Coalesce => Namespaces.Exs + Vocabulary.Coalesce;
    public static XName Conditional => Namespaces.Exs + Vocabulary.Conditional;
    public static XName Constant => Namespaces.Exs + Vocabulary.Constant;
    public static XName Constructor => Namespaces.Exs + Vocabulary.Constructor;
    public static XName ContinueLabel => Namespaces.Exs + Vocabulary.ContinueLabel;
    public static XName Convert => Namespaces.Exs + Vocabulary.Convert;
    public static XName ConvertChecked => Namespaces.Exs + Vocabulary.ConvertChecked;
    public static XName Decrement => Namespaces.Exs + Vocabulary.Decrement;
    public static XName Default => Namespaces.Exs + Vocabulary.Default;
    public static XName DefaultCase => Namespaces.Exs + Vocabulary.DefaultCase;
    public static XName Divide => Namespaces.Exs + Vocabulary.Divide;
    public static XName DivideAssign => Namespaces.Exs + Vocabulary.DivideAssign;
    public static XName ElementInit => Namespaces.Exs + Vocabulary.ElementInit;
    public static XName ArrayElements => Namespaces.Exs + Vocabulary.ArrayElements;
    public static XName Equal => Namespaces.Exs + Vocabulary.Equal;
    public static XName Event => Namespaces.Exs + Vocabulary.Event;
    public static XName Exception => Namespaces.Exs + Vocabulary.Exception;
    public static XName ExclusiveOr => Namespaces.Exs + Vocabulary.ExclusiveOr;
    public static XName ExclusiveOrAssign => Namespaces.Exs + Vocabulary.ExclusiveOrAssign;
    public static XName Fault => Namespaces.Exs + Vocabulary.Fault;
    public static XName Field => Namespaces.Exs + Vocabulary.Field;
    public static XName Filter => Namespaces.Exs + Vocabulary.Filter;
    public static XName Finally => Namespaces.Exs + Vocabulary.Finally;
    public static XName Goto => Namespaces.Exs + Vocabulary.Goto;
    public static XName GreaterThan => Namespaces.Exs + Vocabulary.GreaterThan;
    public static XName GreaterThanOrEqual => Namespaces.Exs + Vocabulary.GreaterThanOrEqual;
    public static XName Increment => Namespaces.Exs + Vocabulary.Increment;
    public static XName Index => Namespaces.Exs + Vocabulary.Index;
    public static XName Invoke => Namespaces.Exs + Vocabulary.Invoke;
    public static XName IsFalse => Namespaces.Exs + Vocabulary.IsFalse;
    public static XName IsTrue => Namespaces.Exs + Vocabulary.IsTrue;
    public static XName Label => Namespaces.Exs + Vocabulary.Label;
    public static XName Lambda => Namespaces.Exs + Vocabulary.Lambda;
    public static XName LeftShift => Namespaces.Exs + Vocabulary.LeftShift;
    public static XName LeftShiftAssign => Namespaces.Exs + Vocabulary.LeftShiftAssign;
    public static XName LessThan => Namespaces.Exs + Vocabulary.LessThan;
    public static XName LessThanOrEqual => Namespaces.Exs + Vocabulary.LessThanOrEqual;
    public static XName ListInit => Namespaces.Exs + Vocabulary.ListInit;
    public static XName Initializers => Namespaces.Exs + Vocabulary.Initializers;
    public static XName Loop => Namespaces.Exs + Vocabulary.Loop;
    public static XName MemberAccess => Namespaces.Exs + Vocabulary.MemberAccess;
    public static XName MemberInit => Namespaces.Exs + Vocabulary.MemberInit;
    public static XName Members => Namespaces.Exs + Vocabulary.Members;
    public static XName Modulo => Namespaces.Exs + Vocabulary.Modulo;
    public static XName ModuloAssign => Namespaces.Exs + Vocabulary.ModuloAssign;
    public static XName Multiply => Namespaces.Exs + Vocabulary.Multiply;
    public static XName MultiplyAssign => Namespaces.Exs + Vocabulary.MultiplyAssign;
    public static XName MultiplyAssignChecked => Namespaces.Exs + Vocabulary.MultiplyAssignChecked;
    public static XName MultiplyChecked => Namespaces.Exs + Vocabulary.MultiplyChecked;
    public static XName Negate => Namespaces.Exs + Vocabulary.Negate;
    public static XName NegateChecked => Namespaces.Exs + Vocabulary.NegateChecked;
    public static XName New => Namespaces.Exs + Vocabulary.New;
    public static XName NewArrayBounds => Namespaces.Exs + Vocabulary.NewArrayBounds;
    public static XName NewArrayInit => Namespaces.Exs + Vocabulary.NewArrayInit;
    public static XName Not => Namespaces.Exs + Vocabulary.Not;
    public static XName NotEqual => Namespaces.Exs + Vocabulary.NotEqual;
    public static XName OnesComplement => Namespaces.Exs + Vocabulary.OnesComplement;
    public static XName Or => Namespaces.Exs + Vocabulary.Or;
    public static XName OrAssign => Namespaces.Exs + Vocabulary.OrAssign;
    public static XName OrElse => Namespaces.Exs + Vocabulary.OrElse;
    public static XName PostDecrementAssign => Namespaces.Exs + Vocabulary.PostDecrementAssign;
    public static XName PostIncrementAssign => Namespaces.Exs + Vocabulary.PostIncrementAssign;
    public static XName Power => Namespaces.Exs + Vocabulary.Power;
    public static XName PowerAssign => Namespaces.Exs + Vocabulary.PowerAssign;
    public static XName PreDecrementAssign => Namespaces.Exs + Vocabulary.PreDecrementAssign;
    public static XName PreIncrementAssign => Namespaces.Exs + Vocabulary.PreIncrementAssign;
    public static XName Property => Namespaces.Exs + Vocabulary.Property;
    public static XName Quote => Namespaces.Exs + Vocabulary.Quote;
    public static XName RightShift => Namespaces.Exs + Vocabulary.RightShift;
    public static XName RightShiftAssign => Namespaces.Exs + Vocabulary.RightShiftAssign;
    public static XName Subtract => Namespaces.Exs + Vocabulary.Subtract;
    public static XName SubtractAssign => Namespaces.Exs + Vocabulary.SubtractAssign;
    public static XName SubtractAssignChecked => Namespaces.Exs + Vocabulary.SubtractAssignChecked;
    public static XName SubtractChecked => Namespaces.Exs + Vocabulary.SubtractChecked;
    public static XName Switch => Namespaces.Exs + Vocabulary.Switch;
    public static XName LabelTarget => Namespaces.Exs + Vocabulary.LabelTarget;
    public static XName Throw => Namespaces.Exs + Vocabulary.Throw;
    public static XName Try => Namespaces.Exs + Vocabulary.Try;
    public static XName TypeAs => Namespaces.Exs + Vocabulary.TypeAs;
    public static XName TypeEqual => Namespaces.Exs + Vocabulary.TypeEqual;
    public static XName TypeIs => Namespaces.Exs + Vocabulary.TypeIs;
    public static XName UnaryPlus => Namespaces.Exs + Vocabulary.UnaryPlus;
    public static XName Unbox => Namespaces.Exs + Vocabulary.Unbox;
    public static XName AssignmentBinding => Namespaces.Exs + Vocabulary.AssignmentBinding;
    public static XName MemberMemberBinding => Namespaces.Exs + Vocabulary.MemberMemberBinding;
    public static XName MemberListBinding => Namespaces.Exs + Vocabulary.MemberListBinding;
}