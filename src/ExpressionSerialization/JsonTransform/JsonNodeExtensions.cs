﻿namespace vm2.ExpressionSerialization.JsonTransform;
/// <summary>
/// Class JsonObject extensions.
/// </summary>
public static class JsonNodeExtensions
{
    /// <summary>
    /// Adds the specified elements (key, JsonNode) to the object.
    /// </summary>
    /// <param name="jsObj">The JSON object.</param>
    /// <param name="element">The elements.</param>
    /// <returns>The <see cref="JsonObject"/>.</returns>
    /// <exception cref="ArgumentException">
    /// If a property with the key of the <paramref name="element"/> already exists in the <paramref name="jsObj"/>.
    /// </exception>
    public static JsonObject Add(this JsonObject jsObj, JElement? element)
    {
        if (!element.HasValue)
            return jsObj;

        if (element.Value.Key is "")
            throw new ArgumentException("The key of the elements is an empty string.");

        jsObj.Add(element.Value.Key, element.Value.Value);
        return jsObj;
    }

    /// <summary>
    /// Adds the specified elements (key, JsonNode) to the object.
    /// </summary>
    /// <param name="jsObj">The JSON object.</param>
    /// <param name="element">The elements.</param>
    /// <returns>
    /// The <paramref name="jsObj"/> and a boolean which will be <c>true</c> if the operation was successful, or
    /// <c>false</c> if the <paramref name="jsObj"/> already has a property with the elements' <see cref="JElement.Key"/>.
    /// </returns>
    public static (JsonObject, bool) TryAdd(this JsonObject jsObj, JElement? element)
    {
        if (!element.HasValue)
            return (jsObj, true);

        if (element.Value.Key is "")
            throw new ArgumentException("The key of the elements is an empty string.");

        return (jsObj, jsObj.TryAdd(element.Value.Key, element.Value.Value));
    }

    /// <summary>
    /// Adds the specified elements (key, JsonNode) to the object.
    /// </summary>
    /// <param name="jsObj">The JSON object.</param>
    /// <param name="elements">The elements to add.</param>
    /// <returns>The <see cref="JsonObject"/>.</returns>
    /// <exception cref="ArgumentException">
    /// If a property with the key of any of the <paramref name="elements"/> already exists in the <paramref name="jsObj"/>.
    /// If there is a property with empty key.
    /// </exception>
    public static JsonObject Add(this JsonObject jsObj, params JElement?[] elements)
        => jsObj.Add(elements.AsEnumerable());

    /// <summary>
    /// Adds the specified elements (key, JsonNode) to the object.
    /// </summary>
    /// <param name="jsObj">The JSON object.</param>
    /// <param name="elements">The elements.</param>
    /// <returns>
    /// The <paramref name="jsObj"/> and a boolean which will be <c>true</c> if the operation was successful, or
    /// <c>false</c> if the <paramref name="jsObj"/> already has a property with the elements' <see cref="JElement.Key"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If there is a property with empty key.
    /// </exception>
    public static (JsonObject, bool) TryAdd(this JsonObject jsObj, params JElement?[] elements)
        => jsObj.TryAdd(elements.AsEnumerable());

    /// <summary>
    /// Adds the <see cref="JElement"/> and <see cref="IEnumerable{JElement}"/> <paramref key="elements"/> to the 
    /// current <paramref name="jsObj"/>.
    /// If any of the parameters are <c>null</c> the method quietly skips them. The <see cref="IEnumerable{JElement}"/>
    /// parameters are added iteratively in the current <paramref name="jsObj"/>.
    /// </summary>
    /// <param name="jsObj">The JSON object.</param>
    /// <param name="elements">The elements to add.</param>
    /// <returns>The <see cref="JsonObject"/>.</returns>
    /// <exception cref="ArgumentException">
    /// If a property with the key of any of the <paramref name="elements"/> already exists in the <paramref name="jsObj"/>.
    /// If there is a property with empty key.
    /// </exception>
    public static JsonObject Add(this JsonObject jsObj, params object?[] elements)
    {
        foreach (var prop in elements)
            _ = prop switch {
                IEnumerable<JElement?> p => jsObj.Add(p),
                JElement p => jsObj.Add(p, null),
                null => jsObj,
                _ => throw new InternalTransformErrorException($"Don't know how to add {prop.GetType().Name} to a JElement")
            };
        return jsObj;
    }

    /// <summary>
    /// Adds the <see cref="JElement"/> and <see cref="IEnumerable{JElement}"/> <paramref key="elements"/> to the 
    /// current <paramref name="jsObj"/>.
    /// If any of the parameters are <c>null</c> the method quietly skips them. The <see cref="IEnumerable{JElement}"/>
    /// parameters are added iteratively in the current <paramref name="jsObj"/>.
    /// </summary>
    /// <param name="jsObj">The JSON object.</param>
    /// <param name="elements">The elements.</param>
    /// <returns>
    /// The <paramref name="jsObj"/> and a boolean which will be <c>true</c> if the operation was successful, or
    /// <c>false</c> if the <paramref name="jsObj"/> already has a property with the elements' <see cref="JElement.Key"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If there is a property with empty key.
    /// </exception>
    public static (JsonObject, bool) TryAdd(this JsonObject jsObj, params object?[] elements)
    {
        bool ret = true;

        foreach (var prop in elements)
            ret &= prop switch {
                IEnumerable<JElement?> p => jsObj.TryAdd(p).Item2,
                JElement p => jsObj.TryAdd(p).Item2,
                null => true,
                _ => throw new InternalTransformErrorException($"Don't know how to add {prop.GetType().Name} to a JElement")
            };
        return (jsObj, ret);
    }

    /// <summary>
    /// Adds the specified elements (key, JsonNode) to the object.
    /// </summary>
    /// <param name="jsObj">The JSON object.</param>
    /// <param name="elements">The elements to add.</param>
    /// <returns>The <see cref="JsonObject"/>.</returns>
    /// <exception cref="ArgumentException">
    /// If a property with the key of any of the <paramref name="elements"/> already exists in the <paramref name="jsObj"/>.
    /// If there is a property with empty key.
    /// </exception>
    public static JsonObject Add(this JsonObject jsObj, IEnumerable<JElement?> elements)
    {
        foreach (var element in elements)
            if (element is not null)
                jsObj.Add(element.Value.Key, element.Value.Value);

        return jsObj;
    }

    /// <summary>
    /// Adds the specified elements (key, JsonNode) to the object.
    /// </summary>
    /// <param name="jsObj">The JSON object.</param>
    /// <param name="elements">The elements.</param>
    /// <returns>
    /// The <paramref name="jsObj"/> and a boolean which will be <c>true</c> if the operation was successful, or
    /// <c>false</c> if the <paramref name="jsObj"/> already has a property with the elements' <see cref="JElement.Key"/>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If there is a property with empty key.
    /// </exception>
    public static (JsonObject, bool) TryAdd(this JsonObject jsObj, IEnumerable<JElement?> elements)
    {
        var ret = true;

        foreach (var element in elements)
            if (element is not null)
                ret &= jsObj.TryAdd(element.Value.Key, element.Value.Value);

        return (jsObj, ret);
    }
}
