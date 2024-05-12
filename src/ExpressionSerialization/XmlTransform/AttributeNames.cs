namespace vm2.ExpressionSerialization.XmlTransform;

static class AttributeNames
{
    public static readonly XName Nil                = Namespaces.Xsi+"nil";
    public static readonly XName Assembly           = Transform.NAssembly;
    public static readonly XName DelegateType       = Transform.NDelegateType;
    public static readonly XName Family             = Transform.NFamily;
    public static readonly XName FamilyAndAssembly  = Transform.NFamilyAndAssembly;
    public static readonly XName FamilyOrAssembly   = Transform.NFamilyOrAssembly;
    public static readonly XName IsByRef            = Transform.NIsByRef;
    public static readonly XName IsLifted           = Transform.NIsLifted;
    public static readonly XName IsLiftedToNull     = Transform.NIsLiftedToNull;
    public static readonly XName IsOut              = Transform.NIsOut;
    public static readonly XName Kind               = Transform.NKind;
    public static readonly XName Name               = Transform.NName;
    public static readonly XName Private            = Transform.NPrivate;
    public static readonly XName Public             = Transform.NPublic;
    public static readonly XName Property           = Transform.NProperty;
    public static readonly XName Static             = Transform.NStatic;
    public static readonly XName TailCall           = Transform.NTailCall;
    public static readonly XName Type               = Transform.NType;
    public static readonly XName TypeOperand        = Transform.NTypeOperand;
    public static readonly XName ConcreteType       = Transform.NConcreteType;   // e.g. derived from ConstantExpression.Type
    public static readonly XName BaseType           = Transform.NBaseType;
    public static readonly XName Id                 = Transform.NId;
    public static readonly XName IdRef              = Transform.NIdRef;
    public static readonly XName Value              = Transform.NValue;
    public static readonly XName BaseValue          = Transform.NBaseValue;
    public static readonly XName Visibility         = Transform.NVisibility;
    public static readonly XName Length             = Transform.NLength;
    public static readonly XName ElementType        = Transform.NElementType;
    public static readonly XName ReadOnly           = Transform.NReadOnly;
};
