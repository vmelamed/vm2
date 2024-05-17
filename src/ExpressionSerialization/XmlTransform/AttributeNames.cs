namespace vm2.ExpressionSerialization.XmlTransform;

static class AttributeNames
{
    public static XName Nil => Namespaces.Xsi + "nil";
    public static XName Assembly => Transform.NAssembly;
    public static XName DelegateType => Transform.NDelegateType;
    public static XName Family => Transform.NFamily;
    public static XName FamilyAndAssembly => Transform.NFamilyAndAssembly;
    public static XName FamilyOrAssembly => Transform.NFamilyOrAssembly;
    public static XName IsByRef => Transform.NIsByRef;
    public static XName IsLifted => Transform.NIsLifted;
    public static XName IsLiftedToNull => Transform.NIsLiftedToNull;
    public static XName Kind => Transform.NKind;
    public static XName Name => Transform.NName;
    public static XName Private => Transform.NPrivate;
    public static XName Public => Transform.NPublic;
    public static XName Property => Transform.NProperty;
    public static XName Static => Transform.NStatic;
    public static XName TailCall => Transform.NTailCall;
    public static XName Type => Transform.NType;
    public static XName DeclaringType => Transform.NDeclaringType;
    public static XName TypeOperand => Transform.NTypeOperand;
    public static XName ConcreteType => Transform.NConcreteType;   // e.g. derived from ConstantExpression.GetEType
    public static XName BaseType => Transform.NBaseType;
    public static XName Id => Transform.NId;
    public static XName IdRef => Transform.NIdRef;
    public static XName Value => Transform.NValue;
    public static XName BaseValue => Transform.NBaseValue;
    public static XName Visibility => Transform.NVisibility;
    public static XName Length => Transform.NLength;
    public static XName ElementType => Transform.NElementType;
    public static XName ReadOnly => Transform.NReadOnly;
};
