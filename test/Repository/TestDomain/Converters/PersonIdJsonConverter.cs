namespace vm2.Repository.TestDomain.Converters;

public sealed class PersonIdJsonConverter : JsonConverter<PersonId>
{
    public override PersonId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => new(Ulid.Parse(reader.GetString()!));

    public override void Write(Utf8JsonWriter writer, PersonId value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.Id.ToString());
}