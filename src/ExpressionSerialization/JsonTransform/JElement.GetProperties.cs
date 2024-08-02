namespace vm2.ExpressionSerialization.JsonTransform;


#if JSON_SCHEMA
using Vocabulary = Conventions.Vocabulary;
#endif

public partial struct JElement
{
    /// <summary>
    /// Tries to the get and convert the <see cref="Value"/> to a strong type value.
    /// </summary>
    /// <typeparam name="T">The type to convert to.</typeparam>
    /// <param name="value">The strongly typed value.</param>
    /// <returns><c>true</c> if conversion was successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetValue<T>(out T? value)
    {
        value = default;
        return Value?.AsValue()?.TryGetValue<T>(out value) ?? false;
    }

    /// <summary>
    /// Convert the <see cref="Value"/> to a strong type value.
    /// </summary>
    /// <typeparam name="T">The type to convert to.</typeparam>
    /// <returns><c>true</c> if conversion was successful, <c>false</c> otherwise.</returns>
    public readonly T? GetValue<T>()
    {
        try
        {
            if (Value?.AsValue()?.TryGetValue<T>(out var value) is true)
                return value;
        }
        catch (Exception ex)
        {
            throw new SerializationException($"Error deserializing and converting to a strong type the value of the property `{Name}`.", ex);
        }

        throw new SerializationException($"Could not convert the value of property {Name} to `{nameof(T)}`");
    }

    /// <summary>
    /// Gets a <see cref="JElement"/> with a <see cref="Name"/> the <paramref name="childPropertyName"/> and the
    /// <see cref="Value"/> the value of the property <paramref name="childPropertyName"/> in this element's <see cref="Value"/>
    /// which must be <see cref="JsonObject"/>.
    /// </summary>
    /// <param name="childPropertyName">Name of the child property.</param>
    /// <returns>vm2.ExpressionSerialization.JsonTransform.JElement.</returns>
    public readonly JElement? TryGetChild(string childPropertyName)
    {
        if (Value is JsonObject jObject &&
            jObject.TryGetPropertyValue(childPropertyName, out var node))
            return new(childPropertyName, node);

        return null;
    }

    /// <summary>
    /// Gets a <see cref="JElement"/> with a <see cref="Name"/> <paramref name="childPropertyName"/> and the
    /// <see cref="Value"/> the value of the property <paramref name="childPropertyName"/> in this element's <see cref="Value"/>
    /// which must be <see cref="JsonObject"/>.
    /// </summary>
    /// <param name="childPropertyName">Name of the child property.</param>
    /// <returns>vm2.ExpressionSerialization.JsonTransform.JElement.</returns>
    public readonly JElement GetChild(string childPropertyName)
    {
        if (Value is not JsonObject jObject)
            throw new SerializationException($"The value of the property `{Name}` is not JsonObject and does not have named children.");

        if (!jObject.TryGetPropertyValue(childPropertyName, out var node))
            throw new SerializationException($"Could not get a child node with a name `{childPropertyName}` from the object in the property `{Name}`.");

        return new(childPropertyName, node);
    }

    /// <summary>
    /// Gets a <see cref="JElement" /> where the <see cref="Name" /> is <paramref name="childIndex" /> and the
    /// <see cref="Value" /> is the value of the property <paramref name="childIndex" />.
    /// </summary>
    /// <param name="childIndex">Name of the child property.</param>
    /// <param name="elementName">Name of the returned <see cref="JElement"/> element.
    /// By default it will be the string representation of <paramref name="childIndex"/></param>
    /// <returns>vm2.ExpressionSerialization.JsonTransform.JElement.</returns>
    public readonly JElement? TryGetChild(int childIndex, string? elementName = null)
    {
        if (Value is not JsonArray jArray ||
            (childIndex < 0 || childIndex >= jArray.Count))
            return null;

        return new(elementName ?? childIndex.ToString(), jArray[childIndex]);
    }

    /// <summary>
    /// Gets a <see cref="JElement" /> where the <see cref="Name" /> is <paramref name="childIndex" /> and the
    /// <see cref="Value" /> is the value of the property <paramref name="childIndex" />.
    /// </summary>
    /// <param name="childIndex">Name of the child property.</param>
    /// <param name="elementName">Name of the returned <see cref="JElement"/> element.
    /// By default it will be the string representation of <paramref name="childIndex"/></param>
    /// <returns>vm2.ExpressionSerialization.JsonTransform.JElement.</returns>
    public readonly JElement GetChild(int childIndex, string? elementName = null)
    {
        if (Value is not JsonArray jArray)
            throw new SerializationException($"The value of the property `{Name}` is not JsonArray and does not have indexable children.");

        if (jArray.Count >= childIndex)
            throw new SerializationException($"The JSON array in the property `{Name}` has less children than the requested index.");

        return new(elementName ?? childIndex.ToString(), jArray[childIndex]);
    }

    /// <summary>
    /// Tries to construct a JElement from the name and value of the first property where the property value is a JsonObject.
    /// </summary>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    public readonly JElement? TryGetChildObject()
    {
        var kvp = Value?.AsObject()?.FirstOrDefault(nn => nn.Value is JsonObject);

        if (kvp is null)
            return null;

        return new JElement(kvp.Value);
    }

    /// <summary>
    /// Tries to construct a JElement from the name and value of the first property where the property value is a JsonObject.
    /// </summary>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    public readonly JElement GetChildObject()
        => new(Value?.AsObject()?.FirstOrDefault(nn => nn.Value is JsonObject)
                        ?? throw new SerializationException("Could not find a child object."));

    /// <summary>
    /// Tries to construct a JElement from the name and value of the first property where the property name is one of <paramref name="names"/>.
    /// </summary>
    /// <param name="names">The names.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    public readonly JElement? TryGetChildFromPropertyNames(IEnumerable<string> names)
    {
        var kvp = Value?.AsObject()?.FirstOrDefault(nn => names.Contains(nn.Key));

        if (kvp is null)
            return null;

        return new JElement(kvp.Value);
    }

    /// <summary>
    /// Construct a JElement from the name and value of the first property where the property name is one of <paramref name="names"/>.
    /// </summary>
    /// <param name="names">The names.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    public readonly JElement GetChildFromPropertyNames(IEnumerable<string> names)
        => new(Value?.AsObject()?.FirstOrDefault(nn => names.Contains(nn.Key))
                        ?? throw new SerializationException($"Could not find a property with any of the names: {string.Join(", ", names)}."));

    /// <summary>
    /// Translates an element's name to the enum ExpressionType.
    /// </summary>
    /// <returns>The <see cref="ExpressionType"/> represented by the element.</returns>
    public readonly bool TryGetExpressionType(out ExpressionType expressionType)
        => Enum.TryParse(Name, true, out expressionType);

    /// <summary>
    /// Translates an element's name to the enum ExpressionType.
    /// </summary>
    /// <returns>The <see cref="ExpressionType"/> represented by the element.</returns>
    public readonly ExpressionType GetExpressionType()
        => Enum.Parse<ExpressionType>(Name, true);

    /// <summary>
    /// Tries to get the length of the sub-elements in the element from attribute <see cref="Vocabulary.Length" />
    /// </summary>
    /// <param name="length">The length.</param>
    /// <returns><c>true</c> if the specified element is nil; otherwise, <c>false</c>.</returns>
    public readonly bool TryGetLength(out int length)
    {
        length = 0;
        var child = TryGetChild(Vocabulary.Length);

        if (!child.HasValue ||
            child?.Value?.AsValue()?.GetValue<int>() is not int len)
            return false;

        length = len;
        return true;
    }

    /// <summary>
    /// Gets the length of the sub-elements in the element from child <see cref="Vocabulary.Length"/>
    /// </summary>
    /// <returns><c>true</c> if the specified element is nil; otherwise, <c>false</c>.</returns>
    /// <exception cref="SerializationException"/>
    public readonly int GetLength()
        => TryGetLength(out var length)
                ? length
                : throw new SerializationException($"Could not get the length property of the element `{Name}`.");

    /// <summary>
    /// Tries to get the .NET type of the element from its property (default "type").
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="propertyName">Name of the attribute (if null - defaults to <see cref="Vocabulary.Type"/>).</param>
    /// <returns><c>true</c> if getting the type was successful; otherwise, <c>false</c>.</returns>
    public readonly bool TryGetTypeFromProperty(out Type? type, string? propertyName = null)
    {
        type = null;

        if (GetChild(propertyName ?? Vocabulary.Type)
                .Value?
                .AsValue()?
                .GetValue<string>() is not string typeName ||
            string.IsNullOrWhiteSpace(typeName))
            return false;

        return Vocabulary.NamesToTypes.TryGetValue(typeName, out type) ||
               (type = Type.GetType(typeName)) is not null;
    }

    /// <summary>
    /// Gets the .NET type of the element only from its property (default "type"). If not found - throws Exception.
    /// </summary>
    /// <param name="propertyName">Name of the attribute (if null - defaults to <see cref="Vocabulary.Type"/>).</param>
    /// <returns>The <see cref="Type"/>  if getting the type was successful; otherwise, <c>false</c>.</returns>
    public readonly Type GetTypeFromProperty(string? propertyName = null)
        => TryGetTypeFromProperty(out var type, propertyName)
                ? type!
                : throw new SerializationException($"Could not get the .NET type of element `{Name}`.");

    /// <summary>
    /// Tries to get the name of the type of an element
    /// <list type="bullet">
    /// <item>either from its attribute "type".</item>
    /// <item>or from the name of the element</item>
    /// </list>
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="attributeName">Name of the attribute (if null - defaults to <see cref="Vocabulary.Type"/>).</param>
    /// <returns><c>true</c> if getting the type was successful; otherwise, <c>false</c>.</returns>
    public readonly bool TryGetTypeName(out string name, string? attributeName = null)
    {
        name = "";
        string? nm = null;

        if (!GetChild(attributeName ?? Vocabulary.Type)
                .Value?
                .AsValue()?
                .TryGetValue<string>(out nm) is null or false)
            nm = Vocabulary.NamesToTypes.ContainsKey(Name) ? Name : null;

        if (string.IsNullOrWhiteSpace(nm))
            return false;

        name = nm;
        return true;
    }

    /// <summary>
    /// Gets the name of the type of an element
    /// <list type="bullet">
    /// <item>either from its attribute "type".</item>
    /// <item>or from the name of the element</item>
    /// </list>
    /// If it fails, throws exception.
    /// </summary>
    /// <param name="attributeName">Name of the attribute (if null - defaults to <see cref="Vocabulary.Type"/>).</param>
    /// <returns><c>true</c> if getting the type was successful; otherwise, <c>false</c>.</returns>
    /// <exception cref="SerializationException"/>
    public readonly string GetTypeName(string? attributeName = null)
        => TryGetTypeName(out var name, attributeName) && name is not null
                        ? name
                        : throw new SerializationException($"Could not get the type name of the element `{Name}`.");

    /// <summary>
    /// Tries to get the .NET type of the element
    /// <list type="bullet">
    /// <item>either from its attribute "type".</item>
    /// <item>or from the name of the element</item>
    /// </list>
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="attributeName">Name of the attribute (if null - defaults to <see cref="Vocabulary.Type"/>).</param>
    /// <returns><c>true</c> if getting the type was successful; otherwise, <c>false</c>.</returns>
    public readonly bool TryGetElementType(out Type? type, string? attributeName = null)
    {
        type = null;

        return Vocabulary.NamesToTypes.TryGetValue(Name, out type)
               || TryGetTypeFromProperty(out type, attributeName);
    }

    /// <summary>
    /// Tries to get the .NET type of the element
    /// <list type="bullet">
    /// <item>either from its attribute "type".</item>
    /// <item>or from the name of the element</item>
    /// </list>
    /// </summary>
    /// <param name="attributeName">Name of the attribute (if null - defaults to <see cref="Vocabulary.Type"/>).</param>
    /// <returns>The <see cref="Type"/>  if getting the type was successful; otherwise, <c>false</c>.</returns>
    public readonly Type GetElementType(string? attributeName = null)
        => TryGetElementType(out var type, attributeName)
                ? type!
                : throw new SerializationException($"Could not get the .NET type of the element `{Name}`.");
}
