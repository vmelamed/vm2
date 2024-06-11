namespace vm2.ExpressionSerialization.JsonTransform;

/// <summary>
/// Class JsonObject extensions.
/// </summary>
public static class JsonObjectExtension
{
    /// <summary>
    /// Adds the specified element (key, JsonNode) to the object.
    /// </summary>
    /// <param name="jsObj">The JSON object.</param>
    /// <param name="element">The element.</param>
    /// <returns>JsonObject.</returns>
    public static JsonObject Add(this JsonObject jsObj, JElement? element)
    {
        if (element.HasValue)
            jsObj.Add(element.Value.Key, element.Value.Value);
        return jsObj;
    }
}
