namespace vm2.ExpressionSerialization.Extensions;

using vm2.ExpressionSerialization.XmlTransform;

/// <summary>
/// Class XNodeExtensions defines extension methods to XNode-s.
/// </summary>
public static class XNodeExtensions
{
    /// <summary>
    /// Determines whether the specified element is nil.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns><c>true</c> if the specified element is nil; otherwise, <c>false</c>.</returns>
    public static bool IsNil(this XElement element)
        => element.Attribute(AttributeNames.Nil)?.Value is string v && XmlConvert.ToBoolean(v);

    /// <summary>
    /// Gets the length of the sub-elements in the element from attribute <see cref="AttributeNames.Length" />
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="length">The length.</param>
    /// <returns><c>true</c> if the specified element is nil; otherwise, <c>false</c>.</returns>
    public static bool TryLength(this XElement element, out int length)
    {
        length = 0;

        if (element.Attribute(AttributeNames.Length)?.Value is not string v)
            return false;

        length = XmlConvert.ToInt32(v);
        return true;
    }

    /// <summary>
    /// Gets the length of the sub-elements in the element from attribute <see cref="AttributeNames.Length"/>
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns><c>true</c> if the specified element is nil; otherwise, <c>false</c>.</returns>
    public static int Length(this XElement element)
        => TryLength(element, out var length)
                ? length
                : throw new SerializationException($"Could not get the length attribute of the element `{element.Name}`.");

    /// <summary>
    /// Tries to get the name of the type of an element either from the name of the element or from its attribute "type".
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="name">The name.</param>
    /// <returns><c>true</c> if getting the type was successful; otherwise, <c>false</c>.</returns>
    public static bool TryTypeName(this XElement element, out string name)
    {
        name = "";
        var nm = Transform.NamesToTypes.ContainsKey(element.Name.LocalName)
                    ? element.Name.LocalName
                    : element.Attribute(AttributeNames.Type)?.Value;

        if (string.IsNullOrWhiteSpace(nm))
            return false;

        name = nm;
        return true;
    }

    /// <summary>
    /// Tries to get the name of the type of an element either from the name of the element or from its attribute "type".
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns><c>true</c> if getting the type was successful; otherwise, <c>false</c>.</returns>
    public static string TypeName(this XElement element)
        => element.TryTypeName(out var name) && name is not null
                        ? name
                        : throw new SerializationException($"Could not get the type name of the element `{element.Name}`.");

    /// <summary>
    /// Tries to get the .NET type of the element either from the name of the element or from its attribute "type".
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if getting the type was successful; otherwise, <c>false</c>.</returns>
    public static bool TryType(this XElement element, out Type? type)
    {
        type = null;

        if (!element.TryTypeName(out var typeName))
            return false;

        if (Transform.NamesToTypes.TryGetValue(typeName, out type))
            return true;

        return (type = System.Type.GetType(typeName)) is not null;
    }

    /// <summary>
    /// Tries to get the .NET type of the element either from the name of the element or from its attribute "type".
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The <see cref="System.Type"/>  if getting the type was successful; otherwise, <c>false</c>.</returns>
    public static Type Type(this XElement element)
        => element.TryType(out var type)
                ? type!
                : throw new SerializationException($"Could not get the .NET type of the element `{element.Name}`.");

    /// <summary>
    /// Tries to get the .NET type of the element either from the name of the element or from its attribute "type".
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if getting the type was successful; otherwise, <c>false</c>.</returns>
    public static bool TryAttributeType(this XElement element, out Type? type)
    {
        type = null;

        var typeName = element.Attribute(AttributeNames.Type)?.Value;

        if (typeName is null)
            return false;

        return Transform.NamesToTypes.TryGetValue(typeName, out type) ||
               (type = System.Type.GetType(typeName)) is not null;
    }

    /// <summary>
    /// Tries to get the .NET type of the element either from the name of the element or from its attribute "type".
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The <see cref="System.Type"/>  if getting the type was successful; otherwise, <c>false</c>.</returns>
    public static Type AttributeType(this XElement element)
        => element.TryType(out var type)
                ? type!
                : throw new SerializationException($"Could not get the .NET type of element `{element.Name}`.");

    /// <summary>
    /// Gets the name of the element from attribute <see cref="AttributeNames.Name" />
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="name">The name.</param>
    /// <returns><c>true</c> if the specified element is nil; otherwise, <c>false</c>.</returns>
    public static bool TryName(this XElement element, out string name)
    {
        name = "";
        if (element.Attribute(AttributeNames.Name)?.Value is not string v)
            return false;

        name = v;
        return true;
    }

    /// <summary>
    /// Gets the name of the element from attribute <see cref="AttributeNames.Name"/>
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns><c>true</c> if the specified element is nil; otherwise, <c>false</c>.</returns>
    public static string Name(this XElement element)
        => TryName(element, out var name)
                ? name
                : throw new SerializationException($"Could not get the name attribute of the element `{element.Name}`.");

    /// <summary>
    /// Gets the first child element of the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>XElement.</returns>
    public static XElement FirstChild(this XElement element)
        => element.Elements().FirstOrDefault() ?? throw new SerializationException($"Could not get the first child element of the element `{element.Name}`.");

    /// <summary>
    /// Translates an element's name to expression type enum.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The <see cref="ExpressionType"/> represented by the element.</returns>
    public static ExpressionType ExpressionType(this XElement element)
        => (ExpressionType)Enum.Parse(typeof(ExpressionType), element.Name.LocalName, true);

    /// <summary>
    /// Finds the reflection method info about of the method described in the passed element.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns><see cref="MethodInfo"/>?</returns>
    public static MethodInfo? MethodInfo(this XElement element)
    {
        var me = element.Element(ElementNames.Method);

        if (me is null)
            return null;

        var type = me.AttributeType();

        return type.GetMethod(
                    me.Name(),
                    BindingFlags.Static | BindingFlags.Public,
                    (me.Element(ElementNames.Parameters) ?? throw new SerializationException($"Could not get the methods parameter definition specification from the element `{element.Name}`"))
                        .Elements(ElementNames.ParameterSpec)
                        .Select(p => p.Type())
                        .ToArray());
    }
}
