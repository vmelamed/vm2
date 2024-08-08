namespace vm2.ExpressionSerialization.JsonTransform;
public partial class FromJsonTransformVisitor
{
    readonly Dictionary<string, ParameterExpression> _parameters = [];
    readonly Dictionary<string, LabelTarget> _labelTargets = [];

    internal void ResetVisitState()
    {
        _parameters.Clear();
        _labelTargets.Clear();
    }

    ParameterExpression GetParameter(JElement e)
    {
        var id = e.GetId();

        if (_parameters.TryGetValue(id, out var expression))
            return expression;

        var type = e.GetTypeFromProperty();

        if (e.TryGetPropertyValue<bool>(out var isByRef, Vocabulary.IsByRef) && isByRef is true)
            type = type.MakeByRefType();

        return _parameters[id] = Expression.Parameter(type, e.TryGetName(out var name) ? name : null);
    }

    LabelTarget GetTarget(JElement e)
    {
        var id = e.GetId();

        if (_labelTargets.TryGetValue(id, out var target))
            return target;

        e.TryGetName(out var name);
        e.TryGetTypeFromProperty(out var type);

        return _labelTargets[id] = type is not null ? Expression.Label(type, name) : Expression.Label(name);
    }

    MemberInfo? GetMemberInfo(JElement e, string memberInfoName)
        => e.TryGetElement(out var member, memberInfoName) && member.HasValue
                ? VisitMemberInfo(member.Value)
                : null;

    /// <summary>
    /// Gets the member information that may be attached to the expression.
    /// </summary>
    /// <param name="e">The e representing the member info.</param>
    /// <returns>System.Reflection.MemberInfo.</returns>
    internal static MemberInfo? VisitMemberInfo(JElement e)
    {
        // get the declaring type - where to get the member info from
        var declTypeName = e.GetPropertyValue<string>(Vocabulary.DeclaringType);

        if (!Vocabulary.NamesToTypes.TryGetValue(declTypeName, out var declType))
            declType = Type.GetType(declTypeName)
                                ?? throw new SerializationException($"Could not get the required declaring type of the member info of the e '{e.Name}' at '{e.GetPath()}'");

        // get the name of the member
        e.TryGetName(out var name);
        if (name is null && e.Name != Vocabulary.Constructor)
            throw new SerializationException($"Could not get the name in the member info of the e '{e.Name}' at '{e.GetPath()}'");

        // get the visibility flags into BindingFlags
        var isStatic = e.TryGetPropertyValue<bool>(out var stat, Vocabulary.Static) && stat;
        var visibility = e.TryGetPropertyValue<string>(out var vis, Vocabulary.Visibility) ? vis : "";
        var bindingFlags = (isStatic ? BindingFlags.Static : BindingFlags.Instance) |
                           visibility switch
                           {
                               Vocabulary.Public or "" => BindingFlags.Public,
                               _ => BindingFlags.NonPublic,
                           };
        var (paramTypes, modifiers) = GetParameterSpecs(e);

        return e.Name switch {
            Vocabulary.Constructor => declType.GetConstructor(bindingFlags, null, paramTypes, [modifiers]) as MemberInfo,
            Vocabulary.Property => declType.GetProperty(name!, bindingFlags, null, e.GetTypeFromProperty(), paramTypes, [modifiers]),
            Vocabulary.Method => declType.GetMethod(name!, bindingFlags, null, paramTypes, [modifiers]),
            Vocabulary.Field => declType.GetField(name!, bindingFlags),
            Vocabulary.Event => declType.GetEvent(name!, bindingFlags),
            _ => null,
        } ?? throw new SerializationException($"Could not get the member info type represented by the e '{e.Name}' at {e.GetPath()}");
    }

    internal static (Type[], ParameterModifier) GetParameterSpecs(JElement e)
    {
        int? paramCount = !e.TryGetArray(out var parameters, Vocabulary.ParameterSpecs) || parameters is null
                            ? null
                            : parameters.Count;

        if (!paramCount.HasValue || paramCount.Value == 0)
            return ([], new ParameterModifier());

        Debug.Assert(parameters is not null);

        var types = new Type[paramCount.Value];
        var mods = new ParameterModifier(paramCount.Value);

        parameters?
            .Select(
                (n, i) =>
                {
                    var jsObj = n?.GetValueKind() is JsonValueKind.Object ? n.AsObject() : throw new SerializationException($"Invalid parameter info at '{parameters.GetPath()}'.");

                    types[i] = jsObj.TryGetType(out var type) && type is not null ? type : throw new SerializationException($"Could not get the type of a parameter info at {n.GetPath()}");
                    mods[i] = jsObj.TryGetPropertyValue<bool>(Vocabulary.IsByRef, out var isByRef) && isByRef;
                    return 1;
                })
            .Count();
        return (types, mods);
    }
}
