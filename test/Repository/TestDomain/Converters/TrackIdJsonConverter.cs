namespace vm2.Repository.TestDomain.Converters;

public sealed class TrackIdJsonConverter : JsonConverter<TrackId>
{
    public override TrackId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(Ulid.Parse(reader.GetString()!));

    public override void Write(Utf8JsonWriter writer, TrackId value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Id.ToString());
}