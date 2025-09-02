namespace vm2.Repository.TestDomain.Converters;

public sealed class LabelIdJsonConverter : JsonConverter<LabelId>
{
    public override LabelId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(Ulid.Parse(reader.GetString()!));

    public override void Write(Utf8JsonWriter writer, LabelId value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Id.ToString());
}