namespace vm2.Repository.DB.TestSQLite.Mapping.Converters;

/// <summary>
/// Provides a mechanism to convert between <see cref="PersonId"/> and <see cref="Guid"/>.
/// </summary>
public sealed class PersonIdConverter : ValueConverter<PersonId, Guid>
{
    /// <summary>
    /// Initializes a new instance of the class,  providing conversion logic between <see cref="PersonId"/> and <see cref="Guid"/>.
    /// </summary>
    public PersonIdConverter() : base(
        v => v.Id.ToGuid(),
        v => new PersonId(new Ulid(v)))
    {
    }
}
