﻿namespace vm2.ExpressionSerialization.XmlTransform;

public partial class FromXmlTransformVisitor
{
    /// <summary>
    /// Gets the member information that may be attached to the expression.
    /// </summary>
    /// <param name="e">The element representing the member info.</param>
    /// <returns>System.Reflection.MemberInfo.</returns>
    protected virtual MemberInfo? VisitMemberInfo(XElement? e)
    {
        if (e is null)
            return null;

        // get the declaring type - where to get the member info from
        var declTypeName = e.Attribute(AttributeNames.DeclaringType)?.Value
                ?? throw new SerializationException($"Could not get the declaring type of the member info of the e `{e.Name}`");

        if (!Vocabulary.NamesToTypes.TryGetValue(declTypeName, out var declType))
            declType = Type.GetType(declTypeName);

        if (declType is null)
            throw new SerializationException($"Could not get the required declaring type of the member info of the e `{e.Name}`");

        // get the name of the member
        e.TryGetName(out var name);
        if (name is null && e.Name.LocalName != Vocabulary.Constructor)
            throw new SerializationException($"Could not get the name in the member info of the e `{e.Name}`");

        // get the visibility flags into BindingFlags
        var isStatic = XmlConvert.ToBoolean(e.Attribute(AttributeNames.Static)?.Value ?? "false");
        var visibilityName = e.Attribute(AttributeNames.Visibility)?.Value ?? "";
        var bindingFlags = (isStatic ? BindingFlags.Static : BindingFlags.Instance) |
                           visibilityName switch
                           {
                               Vocabulary.Public => BindingFlags.Public,
                               "" => BindingFlags.Public,
                               _ => BindingFlags.NonPublic,
                           };
        var (paramTypes, modifiers) = GetParameterSpecs(e);

        return e.Name.LocalName switch {
            Vocabulary.Constructor => declType.GetConstructor(bindingFlags, null, paramTypes, [modifiers]) as MemberInfo,
            Vocabulary.Property => declType.GetProperty(name!, bindingFlags, null, e.GetEType(), paramTypes, [modifiers]),
            Vocabulary.Method => declType.GetMethod(name!, bindingFlags, null, paramTypes, [modifiers]),
            Vocabulary.Field => declType.GetField(name!, bindingFlags),
            Vocabulary.Event => declType.GetEvent(name!, bindingFlags),
            _ => throw new SerializationException($"Could not get the member info type represented by the e `{e.Name}`"),
        }
        ?? throw new SerializationException($"Could not get the member info type represented by the e `{e.Name}`");
    }

    static (Type[], ParameterModifier) GetParameterSpecs(XElement element)
    {
        var paramCount = element.Element(ElementNames.ParameterSpecs)?.Elements(ElementNames.ParameterSpec)?.Count();

        if (paramCount is null or 0)
            return ([], new ParameterModifier());

        var types = new Type[paramCount.Value];
        var mods = new ParameterModifier(paramCount.Value);
        var i = 0;

        element
            .Element(ElementNames.ParameterSpecs)?
            .Elements(ElementNames.ParameterSpec)
            .Select(
                p =>
                {
                    types[i] = p.GetEType();
                    mods[i] = XmlConvert.ToBoolean(p.Attribute(AttributeNames.IsByRef)?.Value ?? "false");
                    i++;
                    return 1;
                })
            .Count();
        return (types, mods);
    }
}