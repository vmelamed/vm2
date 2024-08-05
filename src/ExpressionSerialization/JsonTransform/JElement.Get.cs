namespace vm2.ExpressionSerialization.JsonTransform;

public partial struct JElement
{
    /// <summary>
    /// Tries to get the value of property <paramref name="propertyValueName"/>.
    /// </summary>
    /// <param name="node">The node.</param>
    /// <param name="propertyValueName">Name of the value property.</param>
    /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetValue(
        out JsonNode? node,
        string propertyValueName = Vocabulary.Value)
    {
        node = default;
        return !Value?.AsObject()?.TryGetNode(out node, propertyValueName) is not true;
    }

    /// <summary>
    /// Gets the value of property <paramref name="propertyValueName"/>.
    /// </summary>
    /// <param name="propertyValueName">Name of the value property.</param>
    public readonly JsonNode? GetValue(string propertyValueName = Vocabulary.Value)
        => TryGetValue(out var node, propertyValueName)
            ? node
            : throw new SerializationException($"Could not get the property {propertyValueName} from node property {Name}");

    /// <summary>
    /// Tries to get and convert the <see cref="Value"/> member to a strongly type value.
    /// </summary>
    /// <typeparam name="T">The type to convert to.</typeparam>
    /// <param name="value">The strongly typed value.</param>
    /// <returns><c>true</c> if conversion was successful, <c>false</c> otherwise.</returns>
    public readonly bool TryGetValue<T>(out T? value)
    {
        value = default;
        return Value?.AsValue()?.TryGetValue<T>(out value) is true;
    }

    /// <summary>
    /// Convert the <see cref="Value"/> member to a strongly type value.
    /// </summary>
    /// <typeparam name="T">The type to convert to.</typeparam>
    /// <returns><c>true</c> if conversion was successful, <c>false</c> otherwise.</returns>
    public readonly T? GetValue<T>()
        => TryGetValue<T>(out var value)
                ? value
                : throw new SerializationException($"Could not get or convert the JElement value to `{nameof(T)}` from node property {Name}");

    /// <summary>
    /// Determines whether the value of this JElement is nil: it has a property called 'value' and its value is <see langword="null"/>.
    /// </summary>
    /// <returns><c>true</c> if this instance is nil; otherwise, <c>false</c>.</returns>
    public readonly bool IsNil()
        => Value switch {
            null => true,
            JsonObject jsObj => jsObj.IsNil(),
            JsonValue jsVal => jsVal.GetValueKind() == JsonValueKind.Null,
            _ => false,
        };

    /// <summary>
    /// Tries to get a <see cref="JElement"/> with a <see cref="Name"/> the <paramref name="childPropertyName"/> and the
    /// <see cref="Value"/> the value of the property <paramref name="childPropertyName"/> in this element's <see cref="Value"/>
    /// which must be <see cref="JsonObject"/>.
    /// </summary>
    /// <param name="childPropertyName">Name of the child property.</param>
    /// <returns>vm2.ExpressionSerialization.JsonTransform.JElement.</returns>
    public readonly JElement? TryGetChild(string childPropertyName)
        => Value is JsonObject jsObj
            && jsObj.TryGetPropertyValue(childPropertyName, out var node)
                ? new(childPropertyName, node)
                : null;

    /// <summary>
    /// Gets a <see cref="JElement"/> with a <see cref="Name"/> <paramref name="childPropertyName"/> and the
    /// <see cref="Value"/> the value of the property <paramref name="childPropertyName"/> in this element's <see cref="Value"/>
    /// which must be <see cref="JsonObject"/>.
    /// </summary>
    /// <param name="childPropertyName">Name of the child property.</param>
    /// <returns>vm2.ExpressionSerialization.JsonTransform.JElement.</returns>
    public readonly JElement GetChild(string childPropertyName)
        => Value is JsonObject jsObj
            ? new(childPropertyName, jsObj.GetChildObject(childPropertyName, $" at property '{Name}'"))
            : throw new SerializationException($"'Value' at property '{Name}' is not a JsonObject.");

    /// <summary>
    /// Tries to construct a JElement from the name and value of the first property where the property value is a JsonObject.
    /// </summary>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    public readonly JElement? TryGetChildObject()
        => Value is JsonObject jsObj
            && jsObj.TryGetChildObject(out var propertyName, out var child) is true
            && propertyName is not null
            && child is not null
                ? new(propertyName, child)
                : null;

    /// <summary>
    /// Tries to construct a JElement from the name and value of the first property where the property value is a JsonObject.
    /// </summary>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    public readonly JElement GetChildObject()
        => TryGetChildObject()
                ?? throw new SerializationException($"Could not find a JsonObject property in '{Name}'.");

    /// <summary>
    /// Tries to construct a JElement from the name and value of the first property where the property name is one of <paramref name="names"/>.
    /// </summary>
    /// <param name="names">The names.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    public readonly JElement? TryGetOneOf(IEnumerable<string> names)
        => Value is JsonObject jsObj
            && jsObj.TryGetOneOf(names, out var propertyName, out var node) is true
            && propertyName is not null
            && node is not null
                ? new(propertyName, node)
                : null;

    /// <summary>
    /// Construct a JElement from the name and value of the first property where the property name is one of <paramref name="names"/>.
    /// </summary>
    /// <param name="names">The names.</param>
    /// <returns>System.Nullable&lt;JElement&gt;.</returns>
    public readonly JElement GetOneOf(IEnumerable<string> names)
        => Value is JsonObject jsObj
                ? new(jsObj.GetOneOf(names, $"{Name}"))
                : throw new SerializationException($"'Value' at property '{Name}' is not a JsonObject.");

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
    /// <param name="propertyLengthName">Name of the property length.</param>
    /// <returns><c>true</c> if the specified element is nil; otherwise, <c>false</c>.</returns>
    public readonly bool TryGetLength(
        out int length,
        string propertyLengthName = Vocabulary.Length)
    {
        length = default;
        return Value is JsonObject jsObj
                && jsObj.TryGetLength(out length, propertyLengthName);
    }

    /// <summary>
    /// Gets the length of the sub-elements in the element from child <see cref="Vocabulary.Length"/>
    /// </summary>
    /// <returns><c>true</c> if the specified element is nil; otherwise, <c>false</c>.</returns>
    /// <exception cref="SerializationException"/>
    public readonly int GetLength(string propertyLengthName = Vocabulary.Length)
        => Value is JsonObject jsObj
                ? jsObj.GetLength(propertyLengthName, $" at property '{Name}'")
                : throw new SerializationException($"Could not get the property 'Length' from the element at property '{Name}'.");

    /// <summary>
    /// Tries to get the .NET type of the element from its property (default "type").
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="propertyTypeName">Name of the attribute (if null - defaults to <see cref="Vocabulary.Type"/>).</param>
    /// <returns><c>true</c> if getting the type was successful; otherwise, <c>false</c>.</returns>
    public readonly bool TryGetTypeFromProperty(
        out Type? type,
        string propertyTypeName = Vocabulary.Type)
    {
        type = null;
        return Value is JsonObject jsObj
                && jsObj.TryGetType(out type, propertyTypeName);
    }

    /// <summary>
    /// Gets the .NET type of the element only from its property (default "type"). If not found - throws Exception.
    /// </summary>
    /// <param name="propertyTypeName">Name of the attribute (if null - defaults to <see cref="Vocabulary.Type"/>).</param>
    /// <returns>The <see cref="Type"/>  if getting the type was successful; otherwise, <c>false</c>.</returns>
    public readonly Type GetTypeFromProperty(string propertyTypeName = Vocabulary.Type)
        => TryGetTypeFromProperty(out var type, propertyTypeName)
                ? type!
                : throw new SerializationException($"Could not get the .NET type of element '{Name}'.");

    /// <summary>
    /// Tries to get the .NET type of the element
    /// <list type="bullet">
    /// <item>either from its attribute "type".</item>
    /// <item>or from the name of the element</item>
    /// </list>
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="propertyTypeName">Name of the attribute (if null - defaults to <see cref="Vocabulary.Type"/>).</param>
    /// <returns><c>true</c> if getting the type was successful; otherwise, <c>false</c>.</returns>
    public readonly bool TryGetType(
        out Type? type,
        string propertyTypeName = Vocabulary.Type)
    {
        type = null;

        return Vocabulary.NamesToTypes.TryGetValue(Name, out type)
                || TryGetTypeFromProperty(out type, propertyTypeName);
    }

    /// <summary>
    /// Tries to get the .NET type of the element
    /// <list type="bullet">
    /// <item>either from its attribute "type".</item>
    /// <item>or from the name of the element</item>
    /// </list>
    /// </summary>
    /// <param name="propertyTypeName">Name of the attribute (if null - defaults to <see cref="Vocabulary.Type"/>).</param>
    /// <returns>The <see cref="Type"/>  if getting the type was successful; otherwise, <c>false</c>.</returns>
    public readonly Type GetType(string propertyTypeName = Vocabulary.Type)
        => TryGetType(out var type, propertyTypeName)
                ? type!
                : throw new SerializationException($"Could not get the .NET type of the element '{Name}'.");
}
