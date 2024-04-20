namespace vm2.ExpressionSerialization.XmlTransform;

static class AttributeNames
{
    public static readonly XName Nil                    = Namespaces.Xsi+"nil";
    public static readonly XName Assembly               = "assembly";
    public static readonly XName DelegateType           = "delegateType";
    public static readonly XName Family                 = "family";
    public static readonly XName FamilyAndAssembly      = "familyAndAssembly";
    public static readonly XName FamilyOrAssembly       = "familyOrAssembly";
    public static readonly XName IsByRef                = "isByRef";
    public static readonly XName IsLiftedToNull         = "isLiftedToNull";
    public static readonly XName IsOut                  = "isOut";
    public static readonly XName Kind                   = "kind";
    public static readonly XName Name                   = "name";
    public static readonly XName Private                = "private";
    public static readonly XName Property               = "property";
    public static readonly XName Static                 = "static";
    public static readonly XName TailCall               = "tailCall";
    public static readonly XName Type                   = "type";
    public static readonly XName ConcreteType           = "concreteType";   // e.g. derived from ConstantExpression.Type
    public static readonly XName BaseType               = "baseType";
    public static readonly XName Uid                    = "uid";
    public static readonly XName UidRef                 = "uidRef";
    public static readonly XName Value                  = "value";
    public static readonly XName BaseValue              = "baseValue";
    public static readonly XName Visibility             = "visibility";
    public static readonly XName Length                 = "length";
    public static readonly XName ElementType            = "elementType";
    public static readonly XName KeyValuePair           = "key-value";
};
