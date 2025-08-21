namespace vm2.Repository.EfRepository.Models.Converters;

using Cysharp.Serialization.Json;

/// <summary>
/// Converter for a concrete EntityId&lt;TValue&gt; that delegates to the converter for TValue.
/// </summary>
public sealed class EntityIdJsonConverter<TValue> : JsonConverter<EntityId<TValue>> where TValue : notnull
{
    private readonly JsonConverter<TValue>? _valueConverter;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityIdJsonConverter{TValue}"/> class.
    /// </summary>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> used to configure the JSON serialization process. This is used to
    /// retrieve an existing converter for the value type, if available.</param>
    public EntityIdJsonConverter(JsonSerializerOptions options)
    {
        if (!options.Converters.Any(c => c is UlidJsonConverter))
            options.Converters.Add(new UlidJsonConverter());
        _valueConverter = options.GetConverter(typeof(TValue)) as JsonConverter<TValue>;
        if (_valueConverter is null && typeof(TValue) == typeof(Ulid))
            options.Converters.Add(_valueConverter = (new UlidJsonConverter() as JsonConverter<TValue>)!);

        throw new InvalidOperationException($"No JSON converter found for type {typeof(TValue)}. Ensure that a converter is registered for this type in the JsonSerializerOptions.");
    }

    /// <inheritdoc />
    public override EntityId<TValue> Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            throw new JsonException("EntityId value cannot be null.");

        TValue? value = _valueConverter is not null
                            ? _valueConverter.Read(ref reader, typeof(TValue), options)
                            : JsonSerializer.Deserialize<TValue>(ref reader, options);

        return value is null
            ? throw new JsonException("EntityId value cannot be null.")
            : new EntityId<TValue>(value);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, EntityId<TValue> value, JsonSerializerOptions options)
    {
        if (_valueConverter is not null)
            _valueConverter.Write(writer, value.Id, options);
        else
            JsonSerializer.Serialize(writer, value.Id, options);
    }
}

/// <summary>
/// Factory that creates converters for EntityId&lt;TValue&gt;.
/// </summary>
public sealed class EntityIdJsonConverterFactory<TValue> : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert.IsGenericType
           && typeToConvert.GetGenericTypeDefinition() == typeof(EntityId<>);

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var valueType = typeToConvert.GetGenericArguments()[0];
        var converterType = typeof(EntityIdJsonConverter<>).MakeGenericType(valueType);
        return (JsonConverter)Activator.CreateInstance(converterType, options)!;
    }
}
