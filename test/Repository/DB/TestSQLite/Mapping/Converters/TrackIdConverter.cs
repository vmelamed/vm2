namespace vm2.Repository.DB.TestSQLite.Mapping.Converters;

/// <summary>
/// Provides a mechanism to convert between <see cref="PersonId"/> and <see cref="Guid"/>.
/// </summary>
public sealed class TrackIdConverter : ValueConverter<TrackId, string>
{
    /// <summary>
    /// Initializes a new instance of the class,  providing conversion logic between <see cref="PersonId"/> and <see cref="Guid"/>.
    /// </summary>
    public TrackIdConverter() : base(
        v => v.Id.ToString(),
        v => new TrackId(new Ulid(v)))
    {
    }
}
