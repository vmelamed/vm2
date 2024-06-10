namespace vm2.ExpressionSerialization.JsonTransform;

/// <summary>
/// struct JElement is a key-value pair, similar to JElement with no JSON element value. The 
/// <see cref="KeyValuePair{TKey, TValue}"/> is a struct, so we cannot inherit from it but we have implicit conversions 
/// to and from it.
/// </summary>
[DebuggerDisplay("\"{Key}\": {Value}")]
public struct JElement(string key = "", JsonNode? value = null)
{
    /// <summary>
    /// Gets or sets the key of the element which will become the key of a property in the parent <see cref="JsonObject"/> 
    /// or <see cref="JsonDocument"/>.
    /// </summary>
    /// <value>The key.</value>
    public string Key { get; set; } = key;

    /// <summary>
    /// Gets or sets the value of the element, which will become the value of the property in the parent <see cref="JsonObject"/> 
    /// or <see cref="JsonDocument"/>.
    /// </summary>
    /// <value>The value.</value>
    public JsonNode? Value { get; set; } = value;

    /// <summary>
    /// Initializes a new instance with a <paramref key="key"/> and a new <see cref="JsonObject"/> in the <see cref="Value"/>
    /// with the given set of properties.
    /// </summary>
    /// <param key="key">The key of the property represented by this <see cref="JElement"/>.</param>
    /// <param key="properties">The set of key-value pairs/properties to be inserted in the <see cref="Value"/>.</param>
    public JElement(string key, params JElement?[] properties)
        : this(key, new JsonObject())
        => Add(properties);

    /// <summary>
    /// Initializes a new instance with a <paramref key="key"/> and a new <see cref="JsonObject"/> in the <see cref="Value"/>
    /// with the given set of properties.
    /// </summary>
    /// <param key="key">The key of the property represented by this <see cref="JElement"/>.</param>
    /// <param key="properties">The set of key-value pairs/properties to be inserted in the <see cref="Value"/>.</param>
    public JElement(string key, IEnumerable<JElement> properties)
        : this(key, new JsonObject())
        => Add(properties);

    /// <summary>
    /// Initializes a new instance with a <paramref key="key"/> and a new <see cref="JsonObject"/> in the <see cref="Value"/>
    /// with the given set of properties.
    /// </summary>
    /// <param key="key">The key of the property represented by this <see cref="JElement"/>.</param>
    /// <param key="properties">The set of key-value pairs/properties to be inserted in the <see cref="Value"/>.</param>
    public JElement(string key, IEnumerable<JElement?> properties)
        : this(key, new JsonObject())
        => Add(properties);

    /// <summary>
    /// Adds the <paramref key="key"/> and <paramref name="value"/> to the current <see cref="Value"/> if its type is 
    /// <see cref="JsonObject"/> or <c>null</c> (in which case the method creates a new JsonObject value).
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns>vm2.ExpressionSerialization.JsonTransform.JElement.</returns>
    public JElement Add(string key, JsonNode? value)
    {
        Value ??= new JsonObject();

        if (Value is not JsonObject jObject)
            throw new InternalTransformErrorException($"Trying to add a property to a `{Value.GetValueKind()}` JSON element.");

        jObject.Add(key, value);

        return this;
    }

    /// <summary>
    /// Adds the <paramref key="property"/> to the current <see cref="Value"/> if the type of the value is 
    /// <see cref="JsonObject"/> or <c>null</c> (in which case the method creates a new JsonObject value).
    /// </summary>
    /// <param key="property"></param>
    /// <returns>this</returns>
    /// <exception cref="InternalTransformErrorException"></exception>
    public JElement Add(JElement property)
    {
        Value ??= new JsonObject();

        if (Value is not JsonObject jObject)
            throw new InternalTransformErrorException($"Trying to add a property to a `{Value.GetValueKind()}` JSON element.");

        jObject.Add(property);

        return this;
    }

    /// <summary>
    /// Adds a the <paramref key="properties"/> to the current <see cref="Value"/> if the type of the value is 
    /// <see cref="JsonObject"/> or <c>null</c> (in which case the method creates a new JsonObject value). If any of the
    /// parameters are <c>null</c> the method quietly skips them.
    /// </summary>
    /// <param key="properties"></param>
    /// <returns>this</returns>
    /// <exception cref="InternalTransformErrorException"></exception>
    public JElement Add(params JElement?[] properties)
        => Add(properties.AsEnumerable());

    /// <summary>
    /// Adds a the properties from the parameter to the current <see cref="Value"/> if the type of the value is 
    /// <see cref="JsonObject"/> or <c>null</c> (in which case the method creates a new JsonObject value). If any of the
    /// properties in the <paramref key="properties"/> are <c>null</c> the method quietly skips them.
    /// </summary>
    /// <param key="properties"></param>
    /// <returns>this</returns>
    /// <exception cref="InternalTransformErrorException"></exception>
    public JElement Add(IEnumerable<JElement?> properties)
    {
        Value ??= new JsonObject();

        if (Value is not JsonObject jObject)
            throw new InternalTransformErrorException($"Trying to add the properties to a `{Value.GetValueKind()}` JSON element.");

        foreach (var property in properties.Where(p => p is not null))
            jObject.Add(property!);

        return this;
    }

    /// <summary>
    /// Adds a the properties from the parameter to the current <see cref="Value"/> if the type of the value is 
    /// <see cref="JsonObject"/> or <c>null</c> (in which case the method creates a new JsonObject value). If any of the
    /// properties in the <paramref key="properties"/> are <c>null</c> the method quietly skips them.
    /// </summary>
    /// <param key="properties"></param>
    /// <returns>this</returns>
    /// <exception cref="InternalTransformErrorException"></exception>
    public JElement Add(IEnumerable<JElement> properties)
    {
        Value ??= new JsonObject();

        if (Value is not JsonObject jObject)
            throw new InternalTransformErrorException($"Trying to add the properties to a `{Value.GetValueKind()}` JSON element.");

        foreach (var property in properties)
            jObject.Add(property!);

        return this;
    }

    /// <summary>
    /// Performs an implicit conversion from <see cref="JElement"/> to <see cref="KeyValuePair{String, JsonNode}"/>.
    /// </summary>
    /// <param key="je">The je.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator KeyValuePair<string, JsonNode?>(JElement je) => new(je.Key, je.Value);

    /// <summary>
    /// Performs an implicit conversion from <see cref="JElement" /> to <see cref="KeyValuePair{String, JsonNode}" />.
    /// </summary>
    /// <param key="kvp">The KVP.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator JElement(KeyValuePair<string, JsonNode?> kvp) => new(kvp.Key, kvp.Value);

    /// <summary>
    /// Deconstructs to the specified key and value.
    /// </summary>
    /// <param key="key">The key.</param>
    /// <param key="value">The value.</param>
    public readonly void Deconstruct(out string key, out JsonNode? value)
    {
        key = Key;
        value = Value;
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override readonly string ToString() => $"[{Key}, {Value}]";
}
