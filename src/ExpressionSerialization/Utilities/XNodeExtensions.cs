namespace vm2.ExpressionSerialization.Utilities;

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
    {
        var v = element.Attribute(AttributeNames.Nil)?.Value;

        if (v is not null)
            return XmlConvert.ToBoolean(v);
        return false;
    }

    /// <summary>
    /// Gets the length of the sub-elements in the element from attribute <see cref="AttributeNames.Length"/>
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns><c>true</c> if the specified element is nil; otherwise, <c>false</c>.</returns>
    public static int? Length(this XElement element)
    {
        var v = element.Attribute(AttributeNames.Length)?.Value;

        return v is not null ? XmlConvert.ToInt32(v) : null;
    }

    /// <summary>
    /// Tries to get the name of the type of an element either from the name of the element or from its attribute "type".
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns><c>true</c> if getting the type was successful; otherwise, <c>false</c>.</returns>
    public static string? GetTypeName(this XElement element)
    {
        if (Transform.NamesToTypes.ContainsKey(element.Name.LocalName))
            return element.Name.LocalName;

        return element.Attribute(AttributeNames.Type)?.Value;
    }

    /// <summary>
    /// Tries to get the .NET type of the element either from the name of the element or from its attribute "type".
    /// </summary>
    /// <param name="element">The element.</param>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if getting the type was successful; otherwise, <c>false</c>.</returns>
    public static bool TryGetType(this XElement element, out Type? type)
    {
        type = null;

        var typeName = element.GetTypeName();

        if (string.IsNullOrWhiteSpace(typeName))
            return false;

        if (Transform.NamesToTypes.TryGetValue(typeName, out type))
            return true;

        type = Type.GetType(typeName);

        return type is not null;
    }

    /// <summary>
    /// Tries to get the .NET type of the element either from the name of the element or from its attribute "type".
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>The <see cref="Type"/>  if getting the type was successful; otherwise, <c>false</c>.</returns>
    public static Type OfType(this XElement element)
        => element.TryGetType(out var type)
                ? type!
                : throw new SerializationException($"Could not get the .NET type of element {element.Name}.");

    /// <summary>
    /// Gets the first child element of the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>XElement.</returns>
    public static XElement FirstChild(this XElement element)
        => element.Elements().FirstOrDefault() ?? throw new SerializationException($"Could not get the first child element of the element {element.Name}.");
}
