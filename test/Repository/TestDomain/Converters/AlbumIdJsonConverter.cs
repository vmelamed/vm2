namespace vm2.Repository.TestDomain.Converters;

public sealed class AlbumIdJsonConverter : JsonConverter<AlbumId>
{
    public override AlbumId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(Ulid.Parse(reader.GetString()!));

    public override void Write(Utf8JsonWriter writer, AlbumId value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Id.ToString());
}