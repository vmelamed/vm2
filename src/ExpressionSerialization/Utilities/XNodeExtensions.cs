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
    /// Gets the first child element of the specified element.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>XElement.</returns>
    public static XElement FirstChild(this XElement element)
        => element.Elements().FirstOrDefault() ?? throw new SerializationException($"Deserialization error in element {element.Name}.");
}
