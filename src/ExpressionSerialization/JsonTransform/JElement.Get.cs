﻿namespace vm2.ExpressionSerialization.JsonTransform;

public partial struct JElement
{
    /// <summary>
    /// Gets the JSON kind of the value in <see cref="Value"/>.
    /// </summary>
    /// <returns>JsonValueKind.</returns>
    public readonly JsonValueKind GetValueKind()
        => Value?.GetValueKind() ?? JsonValueKind.Undefined;

    /// <summary>
    /// Gets the JSON kind of the value in <see cref="Value"/>.
    /// </summary>
    /// <returns>JsonValueKind.</returns>
    public readonly string GetPath()
        => Value?.GetPath() ?? Name;

    /// <summary>
    /// Determines whether this element's <see cref="Value"/> is <see langword="null"/>, or
    /// if it has a property called 'value' and its value is <see langword="null"/>.
    /// </summary>
    /// <returns><c>true</c> if this instance represents a nil element; otherwise, <c>false</c>.</returns>
    public readonly bool IsNil()
        => Value switch {
            null => true,
            JsonObject jsObj => jsObj.IsNil(),
            JsonValue jsVal => jsVal.GetValueKind() == JsonValueKind.Null,
            _ => false,
        };

    /// <summary>
    /// Tries to convert the <see cref="Value"/> of this element to a simple (non-object or array) value.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetValue<T>(out T? value)
    {
        value = default;
        return Value switch {
            null => true,
            JsonValue jsVal => jsVal.TryGetValue(out value),
            _ => false,
        };
    }

    /// <summary>
    /// Converts the <see cref="Value"/> of this element to a simple (non-object or array) value.
    /// </summary>
    /// <returns><see cref="JsonNode"/>?</returns>
    public readonly T? GetValue<T>()
        => TryGetValue<T>(out var value)
                ? value
                : throw new NotImplementedException($"Could not get the integer, string, boolean, or null value at '{GetPath()}'.");

    /// <summary>
    /// Tries to get the value of the property with name <paramref name="propertyValueName"/>.
    /// </summary>
    /// <param name="node">The property value.</param>
    /// <param name="propertyValueName">Name of the property.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetPropertyValue(
        out JsonNode? node,
        string propertyValueName = Vocabulary.Value)
    {
        node = default;
        return Value is JsonObject jsObj && jsObj.TryGetValue(out node, propertyValueName) is true;
    }

    /// <summary>
    /// Gets the value of property <paramref name="propertyValueName"/>.
    /// </summary>
    /// <param name="propertyValueName">Name of the property.</param>
    /// <returns><see cref="JsonNode"/>?</returns>
    public readonly JsonNode? GetPropertyValue(
        string propertyValueName = Vocabulary.Value)
        => TryGetPropertyValue(out var node, propertyValueName)
            ? node
            : throw new SerializationException($"Could not get the value at '{GetPath()}'.");

    /// <summary>
    /// Tries to get the strongly typed value of the property with name <paramref name="propertyValueName"/>.
    /// </summary>
    /// <param name="value">The property value.</param>
    /// <param name="propertyValueName">Name of the property.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetPropertyValue<T>(
        out T? value,
        string propertyValueName = Vocabulary.Value)
    {
        value = default;
        return Value is JsonObject jsObj
                && jsObj.TryGetValue(out var node, propertyValueName) is true
                && node is JsonValue jsVal
                && jsVal.TryGetValue<T>(out value) is true;
    }

    /// <summary>
    /// Gets the strongly typed value of the property with name <paramref name="propertyValueName"/>.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="propertyValueName">Name of the property.</param>
    /// <returns>The value of the property.</returns>
    public readonly T GetPropertyValue<T>(
        string propertyValueName = Vocabulary.Value)
        => (Value is JsonObject jsObj
            && jsObj.TryGetValue(out var node, propertyValueName) is true
            && node?.AsValue()?.TryGetValue<T>(out var value) is true)
            && value is not null
                ? value
                : throw new SerializationException($"Could not get '{nameof(T)}' property value at '{GetPath()}'.");

    /// <summary>
    /// Tries to get the integer value from a property <paramref name="propertyLengthName"/> representing the length of the object.
    /// </summary>
    /// <param name="length">The length.</param>
    /// <param name="propertyLengthName">Name of the property length.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetLength(
        out int length,
        string propertyLengthName = Vocabulary.Length)
    {
        length = default;
        return Value is JsonObject jsObj
                && jsObj.TryGetLength(out length, propertyLengthName);
    }

    /// <summary>
    /// Gets the length of the sub-elements in the element from element <see cref="Vocabulary.Length"/>
    /// </summary>
    /// <param name="propertyLengthName">The name of the property containing the length.</param>
    /// <returns>The <see cref="int"/> length.</returns>
    public readonly int GetLength(
        string propertyLengthName = Vocabulary.Length)
        => Value is JsonObject jsObj
                ? jsObj.GetLength(propertyLengthName)
                : throw new SerializationException($"Could not get the property 'Length' from the element at '{GetPath()}'.");

    /// <summary>
    /// Tries to translate this element's name to the enum <see cref="ExpressionType" />.
    /// </summary>
    /// <param name="expressionType">Type of the expression.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetExpressionType(
        out ExpressionType expressionType)
        => Enum.TryParse(Name, true, out expressionType);

    /// <summary>
    /// Translates this element's name to the enum ExpressionType.
    /// </summary>
    /// <returns>The <see cref="ExpressionType"/> represented by the element.</returns>
    public readonly ExpressionType GetExpressionType()
        => TryGetExpressionType(out var expressionType)
            ? expressionType
            : throw new SerializationException($"The name of the element '{Name}' is not from the 'enum ExpressionType' at {GetPath()}.");

    /// <summary>
    /// Tries to get the .NET type of this element from its <see cref="Value"/>'s property <paramref name="propertyTypeName"/>.
    /// </summary>
    /// <param name="type">The element's type.</param>
    /// <param name="propertyTypeName">The name of the property representing the type.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetTypeFromProperty(
        out Type? type,
        string propertyTypeName = Vocabulary.Type)
    {
        type = default;
        return Value is JsonObject jsObj
                && jsObj.TryGetType(out type, propertyTypeName);
    }

    /// <summary>
    /// Gets the .NET type of the element only from its property (default "type"). If not found - throws Exception.
    /// </summary>
    /// <param name="propertyTypeName">The name of the property representing the type.</param>
    /// <returns><see cref="Type"/>.</returns>
    public readonly Type GetTypeFromProperty(
        string propertyTypeName = Vocabulary.Type)
        => TryGetTypeFromProperty(out var type, propertyTypeName) && type is not null
                ? type
                : throw new SerializationException($"Could not get the .NET type at '{GetPath()}'.");

    /// <summary>
    /// Tries to get the .NET type of the element
    /// <list type="number">
    /// <item>from the name of the element or</item>
    /// <item>from its attribute "type".</item>
    /// </list>
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="propertyTypeName">The name of the property representing the type.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetType(
        out Type? type,
        string propertyTypeName = Vocabulary.Type)
    {
        type = default;
        return Vocabulary.NamesToTypes.TryGetValue(Name, out type)
                || TryGetTypeFromProperty(out type, propertyTypeName);
    }

    /// <summary>
    /// Tries to get the .NET type of the element
    /// <list type="number">
    /// <item>from the name of the element or</item>
    /// <item>from its attribute "type".</item>
    /// </list>
    /// </summary>
    /// <param name="propertyTypeName">The name of the property representing the type.</param>
    /// <returns><see cref="Type"/>.</returns>
    public readonly Type GetType(
        string propertyTypeName = Vocabulary.Type)
        => TryGetType(out var type, propertyTypeName) && type is not null
                ? type
                : throw new SerializationException($"Could not get the .NET type at '{GetPath()}'.");

    /// <summary>
    /// Tries to construct a JElement from the name and the value of a property with one of the names in <paramref name="names"/>.
    /// </summary>
    /// <param name="names">The names of properties to search for.</param>
    /// <param name="element">The element.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetOneOf(
        IEnumerable<string> names,
        out JElement? element)
    {
        if (Value is JsonObject jsObj
            && jsObj.TryGetOneOf(names, out var propertyName, out var node) is true
            && propertyName is not null)
        {
            element = new(propertyName, node);
            return true;
        }

        element = default;
        return false;
    }

    /// <summary>
    /// Constructs a JElement from the name and the value of a property with one of the names in <paramref name="names"/>.
    /// </summary>
    /// <param name="names">The names.</param>
    /// <returns>JElement.</returns>
    public readonly JElement GetOneOf(
        IEnumerable<string> names)
        => TryGetOneOf(names, out var element) && element.HasValue
                ? element.Value
                : throw new SerializationException($"Could not find a property with one of the names '{string.Join("', '", names)}' at '{GetPath()}'.");

    /// <summary>
    /// Tries to construct a <see cref="JElement" /> from this <see cref="Value"/>'s property <paramref name="childPropertyName" /> and
    /// its JsonObject value.
    /// </summary>
    /// <param name="childPropertyName">Name of the element property.</param>
    /// <param name="element">The element.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetElement(
        out JElement? element,
        string childPropertyName = Vocabulary.Value)
    {
        if (Value is JsonObject jsObj
            && jsObj.TryGetObject(childPropertyName, out var obj))
        {
            element = new(childPropertyName, obj);
            return true;
        }

        element = default;
        return false;
    }

    /// <summary>
    /// Constructs a <see cref="JElement" /> from this <see cref="Value"/>'s property <paramref name="childPropertyName" /> and
    /// its JsonObject value.
    /// </summary>
    /// <param name="childPropertyName">Name of the element property.</param>
    public readonly JElement GetElement(
        string childPropertyName = Vocabulary.Value)
        => TryGetElement(out var element, childPropertyName)
            && element.HasValue
                ? element.Value
                : throw new SerializationException($"Could not get JsonObject at '{GetPath()}'.");

    /// <summary>
    /// Tries to construct a <see cref="JElement" /> from this <see cref="Value"/>'s property <paramref name="childPropertyName" /> and
    /// its JsonObject value.
    /// </summary>
    /// <param name="childPropertyName">Name of the element property.</param>
    /// <param name="array">The element.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetArray(
        out JsonArray? array,
        string childPropertyName = Vocabulary.Value)
    {
        if (Value is JsonObject jsObj
            && jsObj.TryGetArray(childPropertyName, out array))
            return true;

        array = default;
        return false;
    }

    /// <summary>
    /// Constructs a <see cref="JElement" /> from this <see cref="Value"/>'s property <paramref name="childPropertyName" /> and
    /// its JsonObject value.
    /// </summary>
    /// <param name="childPropertyName">Name of the element property.</param>
    public readonly JsonArray GetArray(
        string childPropertyName = Vocabulary.Value)
        => TryGetArray(out var array, childPropertyName)
            && array is not null
            ? array
            : throw new SerializationException($"Could not get JsonObject at '{GetPath()}'.");

    /// <summary>
    /// Tries to construct a JElement from the name and value of the first property where the property value is a JsonObject.
    /// </summary>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetFirstElement(out JElement? element)
    {
        if (Value is JsonObject jsObj
            && jsObj.TryGetFirstObject(out var propertyName, out var child) is true
            && propertyName is not null
            && child is not null)
        {
            element = new(propertyName, child);
            return true;
        }

        element = default;
        return false;
    }

    /// <summary>
    /// Tries to construct a JElement from the name and value of the first property where the property value is a JsonObject.
    /// </summary>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    public readonly JElement GetFirstElement()
        => TryGetFirstElement(out var element) && element.HasValue
                ? element.Value
                : throw new SerializationException($"Could not find a JsonObject at '{GetPath()}'.");
}