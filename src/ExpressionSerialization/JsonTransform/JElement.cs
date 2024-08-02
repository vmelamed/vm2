namespace vm2.ExpressionSerialization.JsonTransform;
/// <summary>
/// struct JElement is a key-value pair, similar to <see cref="XElement"/>. The <paramref name="key"/> is the name of
/// the element in the parent JsonObject.
/// </summary>
/// <remarks>
/// The <see cref="KeyValuePair{TKey, TValue}"/> is a struct, so we cannot inherit
/// from it but we have implicit conversions to and from it and instances of this class can be used anywhere where
/// <see cref="KeyValuePair{TKey, TValue}"/> is required.
/// </remarks>
[DebuggerDisplay("{Name}: {Value}")]
public partial struct JElement(string key = "", JsonNode? value = null)
{
    #region Constructors
    /// <summary>
    /// Gets or sets the key of this JElement. When this JElement is added to a <see cref="JsonObject"/> or
    /// <see cref="JsonDocument"/>, the <see cref="Name"/> will become the name of a property in that parent.
    /// </summary>
    public string Name { get; set; } = key;

    /// <summary>
    /// Gets or sets the value of this JElement. When this JElement is added to a <see cref="JsonObject"/> or
    /// <see cref="JsonDocument"/>, the <see cref="Value"/> will become the value of the property with name
    /// <see cref="Name"/> in that parent.
    /// </summary>
    /// <element>The element.</element>
    public JsonNode? Value { get; set; } = value;

    /// <summary>
    /// Initializes a new instance of the <see cref="JElement" /> struct. Disambiguates the <see cref="JsonArray"/>
    /// parameter as a <see cref="JsonNode"/>, instead of <see cref="IEnumerable{JsonNode}"/>.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="jArray">The j array.</param>
    public JElement(string key, JsonArray jArray)
        : this(key, (JsonNode)jArray) { }

    /// <summary>
    /// Initializes a new instance with a <paramref key="key"/> and a new <see cref="JsonObject"/> in the <see cref="Value"/>
    /// with the given set of <see cref="JElement"/>-s.
    /// </summary>
    /// <param key="key">The name of the property in the parent JSON object that will contain this <see cref="JElement"/>.</param>
    /// <param key="properties">
    /// A set of <see cref="JElement"/>-s to be added to the new <see cref="JsonObject"/> to be set in the <see cref="Value"/>.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If there are two or more properties with the same key in the <paramref name="properties"/>.
    /// </exception>
    public JElement(string key, params JElement?[] properties)
        : this(key, new JsonObject(properties.Where(p => p is not null).Select(p => (KeyValuePair<string, JsonNode?>)p!))) { }

    /// <summary>
    /// Initializes a new instance with a <paramref key="key"/> and a new <see cref="JsonObject"/> in the <see cref="Value"/>
    /// with the given set of <see cref="JElement"/>-s.
    /// </summary>
    /// <param key="key">The name of the property in the parent JSON object that will contain this <see cref="JElement"/>.</param>
    /// <param key="properties">
    /// A set of <see cref="JElement"/>-s to be added to the new <see cref="JsonObject"/> to be set in the <see cref="Value"/>.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If there are two or more properties with the same key in the <paramref name="properties"/>.
    /// </exception>
    public JElement(string key, IEnumerable<JElement?> properties)
        : this(key, new JsonObject(properties.Where(p => p is not null).Select(p => (KeyValuePair<string, JsonNode?>)p!))) { }

    /// <summary>
    /// Initializes a new instance with a <paramref key="key"/> and a new <see cref="JsonObject"/> in the <see cref="Value"/>
    /// with the given set of <see cref="JElement"/>-s.
    /// </summary>
    /// <param key="key">The name of the property in the parent JSON object that will contain this <see cref="JElement"/>.</param>
    /// <param key="properties">
    /// A set of <see cref="JElement"/>-s to be added to the new <see cref="JsonObject"/> to be set in the <see cref="Value"/>.
    /// </param>
    /// <exception cref="ArgumentException">
    /// If there are two or more properties with the same key in the <paramref name="properties"/>.
    /// </exception>
    public JElement(string key, IEnumerable<JElement> properties)
        : this(key, new JsonObject(properties.Select(p => (KeyValuePair<string, JsonNode?>)p))) { }

    /// <summary>
    /// Initializes a new instance with a <paramref key="key"/> and a new <see cref="JsonArray"/> in the <see cref="Value"/>
    /// with the given set of <see cref="JsonNode"/>-s.
    /// </summary>
    /// <param key="key">The name of the property in the parent JSON object that will contain this <see cref="JElement"/>.</param>
    /// <param key="elements">
    /// A set of <see cref="JsonNode"/>-s to be added to the new <see cref="JsonArray"/> that will be set in the <see cref="Value"/>.
    /// </param>
    public JElement(string key, IEnumerable<JsonNode?> elements)
        : this(key, (JsonNode)new JsonArray(elements.ToArray())) { }

    /// <summary>
    /// Initializes a new instance with a <paramref key="key"/> and a new <see cref="JsonArray"/> in the <see cref="Value"/>
    /// with the given set of <see cref="JsonNode"/>-s.
    /// </summary>
    /// <param key="key">The name of the property in the parent JSON object that will contain this <see cref="JElement"/>.</param>
    /// <param key="elements">
    /// A set of <see cref="JsonNode"/>-s to be added to the new <see cref="JsonArray"/> that will be set in the <see cref="Value"/>.
    /// </param>
    public JElement(string key, params JsonNode?[] elements)
        : this(key, (JsonNode)new JsonArray(elements)) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="JElement" /> <see langword="struct"/> from a <see cref="KeyValuePair{TKey, TValue}"/>
    /// </summary>
    /// <param name="kvp">The key-value pair.</param>
    public JElement(KeyValuePair<string, JsonNode?> kvp)
            : this(kvp.Key, kvp.Value) { }
    #endregion

    /// <summary>
    /// Adds the <paramref key="key" /> and the <paramref name="value" /> to the current <see cref="Value" /> if its type is
    /// <see cref="JsonObject" /> or <c>null</c> (in which case the method creates a new JsonObject element).
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The object to add.</param>
    /// <returns>This instance.</returns>
    /// <exception cref="InternalTransformErrorException">
    /// If the <see cref="Value"/> of this <see cref="JElement"/> is not <see cref="JsonObject"/>
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If a property with the same name as the <paramref name="key"/> already exists in the <see cref="Value"/> of this instance.
    /// If <paramref name="key"/> is an empty string.
    /// </exception>
    public JElement Add(string key, JsonNode? value = null)
    {
        Value ??= new JsonObject();

        if (Value is not JsonObject jObject)
            throw new InternalTransformErrorException($"Trying to add a string key and JsonNode to a Value of `{Value.GetValueKind()}` type of JSON element. The Value must be JsonObject type.");

        jObject.Add(key, value);

        return this;
    }

    /// <summary>
    /// Adds the <paramref key="key"/> and the <paramref name="value"/> to the current <see cref="Value"/> if its type is
    /// <see cref="JsonObject"/> or <c>null</c> (in which case the method creates a new JsonObject element).
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The object to add.</param>
    /// <returns>
    /// This instance and a boolean which will be <c>true</c> if the operation was successful,
    /// <c>false</c> if the <see cref="Value"/> of this element already has a property with a name <paramref name="key"/>.
    /// </returns>
    /// <exception cref="InternalTransformErrorException">
    /// If the <see cref="Value"/> of this <see cref="JElement"/> is not <see cref="JsonObject"/>
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="key"/> is an empty string.
    /// </exception>
    public (JElement, bool) TryAdd(string key, JsonNode? value = null)
    {
        Value ??= new JsonObject();

        if (Value is not JsonObject jObject)
            throw new InternalTransformErrorException($"Trying to add a string key and JsonNode to a Value of `{Value.GetValueKind()}` type of JSON element. The Value must be JsonObject type.");

        return (this, jObject.TryAdd(key, value).Item2);
    }

    /// <summary>
    /// Adds the <see cref="JElement"/> <paramref key="properties"/> to the current <see cref="Value"/> if its type is
    /// <see cref="JsonObject"/> or <c>null</c> (in which case the method creates a new JsonObject element).
    /// If any of the parameters are <c>null</c> the method quietly skips them.
    /// </summary>
    /// <param key="elements"></param>
    /// <returns>This instance.</returns>
    /// <exception cref="InternalTransformErrorException">
    /// If the <see cref="Value"/> of this <see cref="JElement"/> is not <see cref="JsonObject"/>
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If a property with the same name as the <see cref="Name"/> of any of the <paramref name="properties"/>
    /// already exists in the <see cref="Value"/> of this instance.
    /// </exception>
    public JElement Add(params JElement?[] properties)
        => Add(properties.AsEnumerable());

    /// <summary>
    /// Adds a the <paramref key="properties"/> to the current <see cref="Value"/> if the type of the element is
    /// <see cref="JsonObject"/> or <c>null</c> (in which case the method creates a new JsonObject element). If any of the
    /// parameters are <c>null</c> the method quietly skips them.
    /// </summary>
    /// <param key="properties"></param>
    /// <returns>
    /// This instance and a boolean which will be <c>true</c> if the operation was successful, or
    /// <c>false</c> if a property with the same name as the <see cref="Name"/> of any of the <paramref name="properties"/>
    /// already exists in the <see cref="Value"/> of this instance.
    /// </returns>
    /// <exception cref="InternalTransformErrorException">
    /// If the <see cref="Value"/> of this <see cref="JElement"/> is not <see cref="JsonObject"/>
    /// </exception>
    public (JElement, bool) TryAdd(params JElement?[] properties)
        => TryAdd(properties.AsEnumerable());

    /// <summary>
    /// Adds the <see cref="JElement"/> and <see cref="IEnumerable{JElement}"/> <paramref key="properties"/> to the
    /// current <see cref="Value"/> if its type is <see cref="JsonObject"/> or <c>null</c> (in which case the method
    /// creates a new JsonObject element).
    /// If any of the parameters are <c>null</c> the method quietly skips them. The <see cref="IEnumerable{JElement}"/>
    /// parameters are added iteratively in the current <see cref="Value"/>.
    /// </summary>
    /// <param key="properties"></param>
    /// <returns>
    /// This instance and a boolean which will be <c>true</c> if the operation was successful, or
    /// <c>false</c> if a property with the same name as the <see cref="Name"/> of any of the <paramref name="properties"/>
    /// already exists in the <see cref="Value"/> of this instance.
    /// </returns>
    /// <exception cref="InternalTransformErrorException">
    /// If the <see cref="Value"/> of this <see cref="JElement"/> is not <see cref="JsonObject"/>
    /// </exception>
    public JElement Add(params object?[] properties)
    {
        Value ??= new JsonObject();

        if (Value is not JsonObject jObject)
            throw new InternalTransformErrorException($"Trying to add JElement-s to a Value of `{Value.GetValueKind()}` type of JSON element. The Value must be JsonObject type.");

        foreach (var prop in properties)
            _ = prop switch {
                IEnumerable<JElement?> p => Add(p),
                JElement p => Add(p),
                null => this,
                _ => throw new InternalTransformErrorException($"Don't know how to add {prop.GetType().Name} to a JElement")
            };
        return this;
    }

    /// <summary>
    /// Adds the <see cref="JElement"/> and <see cref="IEnumerable{JElement}"/> <paramref key="properties"/> to the
    /// current <see cref="Value"/> if its type is <see cref="JsonObject"/> or <c>null</c> (in which case the method
    /// creates a new JsonObject element).
    /// If any of the parameters are <c>null</c> the method quietly skips them. The <see cref="IEnumerable{JElement}"/>
    /// parameters are added iteratively in the current <see cref="Value"/>.
    /// </summary>
    /// <param key="properties"></param>
    /// <returns>
    /// This instance and a boolean which will be <c>true</c> if the operation was successful, or
    /// <c>false</c> if a property with the same name as the <see cref="Name"/> of any of the <paramref name="properties"/>
    /// already exists in the <see cref="Value"/> of this instance.
    /// </returns>
    /// <exception cref="InternalTransformErrorException">
    /// If the <see cref="Value"/> of this <see cref="JElement"/> is not <see cref="JsonObject"/>
    /// </exception>
    public (JElement, bool) TryAdd(params object?[] properties)
    {
        Value ??= new JsonObject();

        if (Value is not JsonObject jObject)
            throw new InternalTransformErrorException($"Trying to add JElement-s to a Value of `{Value.GetValueKind()}` type of JSON element. The Value must be JsonObject type.");

        bool ret = true;

        foreach (var prop in properties)
            ret &= prop switch {
                IEnumerable<JElement?> p => TryAdd(p).Item2,
                JElement p => TryAdd(p).Item2,
                null => true,
                _ => throw new InternalTransformErrorException($"Don't know how to add {prop.GetType().Name} to a JElement")
            };
        return (this, ret);
    }

    /// <summary>
    /// Adds a the elements from the parameter to the current <see cref="Value"/> if the type of the element is
    /// <see cref="JsonObject"/> or <c>null</c> (in which case the method creates a new JsonObject element). If any of the
    /// elements in the <paramref key="elements"/> are <c>null</c> the method quietly skips them.
    /// </summary>
    /// <param key="elements"></param>
    /// <returns>This instance.</returns>
    /// <exception cref="InternalTransformErrorException">
    /// If the <see cref="Value"/> of this <see cref="JElement"/> is not <see cref="JsonObject"/>
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If a property with the same name as the <see cref="Name"/> of any of the <paramref name="properties"/>
    /// already exists in the <see cref="Value"/> of this instance.
    /// </exception>
    public JElement Add(IEnumerable<JElement?> properties)
    {
        Value ??= new JsonObject();

        if (Value is not JsonObject jObject)
            throw new InternalTransformErrorException($"Trying to add JElement-s to a Value of `{Value.GetValueKind()}` type of JSON element. The Value must be JsonObject type.");

        foreach (var property in properties.Where(p => p is not null))
            jObject.Add(property!);

        return this;
    }

    /// <summary>
    /// Adds a the elements from the parameter to the current <see cref="Value"/> if the type of the element is
    /// <see cref="JsonObject"/> or <c>null</c> (in which case the method creates a new JsonObject element). If any of the
    /// elements in the <paramref key="elements"/> are <c>null</c> the method quietly skips them.
    /// </summary>
    /// <param key="elements"></param>
    /// <returns>
    /// This instance and a boolean which will be <c>true</c> if the operation was successful, or
    /// <c>false</c> if a property with the same name as the <see cref="Name"/> of any of the <paramref name="properties"/>
    /// already exists in the <see cref="Value"/> of this instance.
    /// </returns>
    /// <exception cref="InternalTransformErrorException">
    /// If the <see cref="Value"/> of this <see cref="JElement"/> is not <see cref="JsonObject"/>
    /// </exception>
    public (JElement, bool) TryAdd(IEnumerable<JElement?> properties)
    {
        Value ??= new JsonObject();

        if (Value is not JsonObject jObject)
            throw new InternalTransformErrorException($"Trying to add JElement-s to a Value of `{Value.GetValueKind()}` type of JSON element. The Value must be JsonObject type.");

        var ret = true;

        foreach (var property in properties.Where(p => p is not null))
            ret &= jObject.TryAdd(property!).Item2;

        return (this, ret);
    }

    /// <summary>
    /// Adds a the elements from the parameter to the current <see cref="Value"/> if the type of the element is
    /// <see cref="JsonObject"/> or <c>null</c> (in which case the method creates a new JsonObject element). If any of the
    /// elements in the <paramref key="elements"/> are <c>null</c> the method quietly skips them.
    /// </summary>
    /// <param key="elements"></param>
    /// <returns>This instance.</returns>
    /// <exception cref="InternalTransformErrorException">
    /// If the <see cref="Value"/> of this <see cref="JElement"/> is not <see cref="JsonObject"/>
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If a property with the same name as the <see cref="Name"/> of any of the <paramref name="properties"/>
    /// already exists in the <see cref="Value"/> of this instance.
    /// </exception>
    public JElement Add(IEnumerable<JElement> properties)
    {
        Value ??= new JsonObject();

        if (Value is not JsonObject jObject)
            throw new InternalTransformErrorException($"Trying to add JElement-s to a Value of `{Value.GetValueKind()}` type of JSON element. The Value must be JsonObject type.");

        foreach (var property in properties)
            jObject.Add(property!);

        return this;
    }

    /// <summary>
    /// Adds a the elements from the parameter to the current <see cref="Value"/> if the type of the element is
    /// <see cref="JsonObject"/> or <c>null</c> (in which case the method creates a new JsonObject element). If any of the
    /// elements in the <paramref key="elements"/> are <c>null</c> the method quietly skips them.
    /// </summary>
    /// <param key="elements"></param>
    /// <returns>
    /// This instance and a boolean which will be <c>true</c> if the operation was successful, or
    /// <c>false</c> if a property with the same name as the <see cref="Name"/> of any of the <paramref name="properties"/>
    /// already exists in the <see cref="Value"/> of this instance.
    /// </returns>
    /// <exception cref="InternalTransformErrorException">
    /// If the <see cref="Value"/> of this <see cref="JElement"/> is not <see cref="JsonObject"/>
    /// </exception>
    public (JElement, bool) TryAdd(IEnumerable<JElement> properties)
    {
        Value ??= new JsonObject();

        if (Value is not JsonObject jObject)
            throw new InternalTransformErrorException($"Trying to add JElement-s to a Value of `{Value.GetValueKind()}` type of JSON element. The Value must be JsonObject type.");

        var ret = true;

        foreach (var property in properties)
            ret &= jObject.TryAdd(property!).Item2;

        return (this, ret);
    }

    /// <summary>
    /// Adds the <paramref name="element"/> to the current <see cref="Value"/> if its type is
    /// <see cref="JsonArray"/> or <c>null</c> (in which case the method creates a new JsonArray).
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>This instance.</returns>
    /// <exception cref="InternalTransformErrorException">
    /// If the <see cref="Value"/> of this <see cref="JElement"/> is not <see cref="JsonArray"/>
    /// </exception>
    public JElement Add(JsonNode? element)
    {
        Value ??= new JsonArray();

        if (Value is not JsonArray jArray)
            throw new InternalTransformErrorException($"Trying to add a JsonNode to a Value of `{Value.GetValueKind()}` type of JSON element. The Value must be JsonArray type.");

        jArray.Add(element);

        return this;
    }

    /// <summary>
    /// Adds the <paramref name="elements"/> to the current <see cref="Value"/> if its type is
    /// <see cref="JsonArray"/> or <c>null</c> (in which case the method creates a new JsonArray).
    /// </summary>
    /// <param name="elements">The element.</param>
    /// <returns>This instance.</returns>
    /// <exception cref="InternalTransformErrorException">
    /// If the <see cref="Value"/> of this <see cref="JElement"/> is not <see cref="JsonArray"/>
    /// </exception>
    public JElement Add(params JsonNode?[] elements)
        => Add(elements.AsEnumerable());

    /// <summary>
    /// Adds a the elements from the parameter to the current <see cref="Value"/> if the type of the element is
    /// <see cref="JsonArray"/> or <c>null</c> (in which case the method creates a new JsonObject element). If any of the
    /// elements in the <paramref key="elements"/> are <c>null</c> the method quietly skips them.
    /// </summary>
    /// <param key="elements"></param>
    /// <returns>This instance.</returns>
    /// <exception cref="InternalTransformErrorException">
    /// If the <see cref="Value"/> of this <see cref="JElement"/> is not <see cref="JsonArray"/>
    /// </exception>
    public JElement Add(IEnumerable<JsonNode?> elements)
    {
        Value ??= new JsonObject();

        if (Value is not JsonArray jArray)
            throw new InternalTransformErrorException($"Trying to add JsonNode-s to a Value of `{Value.GetValueKind()}` type of JSON element. The Value must be JsonArray type.");

        foreach (var element in elements)
            jArray.Add(element);

        return this;
    }

    /// <summary>
    /// Deeply clones this element.
    /// </summary>
    /// <returns>vm2.ExpressionSerialization.JsonTransform.JElement.</returns>
    public readonly JElement DeepClone()
            => new(Name, Value?.DeepClone());

    /// <summary>
    /// Performs an implicit conversion from <see cref="JElement"/> to <see cref="KeyValuePair{String, JsonNode}"/>.
    /// </summary>
    /// <param key="je">The je.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator KeyValuePair<string, JsonNode?>(JElement je) => new(je.Name, je.Value);

    /// <summary>
    /// Performs an implicit conversion from <see cref="JElement" /> to <see cref="KeyValuePair{String, JsonNode}" />.
    /// </summary>
    /// <param key="kvp">The KVP.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator JElement(KeyValuePair<string, JsonNode?> kvp) => new(kvp.Key, kvp.Value);

    /// <summary>
    /// Deconstructs to the specified key and element.
    /// </summary>
    /// <param key="key">The key.</param>
    /// <param key="element">The element.</param>
    public readonly void Deconstruct(out string key, out JsonNode? value)
    {
        key = Name;
        value = Value;
    }

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override readonly string ToString() => $"[{Name}, {Value}]";
}
