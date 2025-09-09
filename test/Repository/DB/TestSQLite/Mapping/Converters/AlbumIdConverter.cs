namespace vm2.Repository.DB.TestSQLite.Mapping.Converters;

/// <summary>
/// Provides a mechanism to convert between <see cref="AlbumId"/> and <see cref="Guid"/>.
/// </summary>
public sealed class AlbumIdConverter : ValueConverter<AlbumId, string>
{
    /// <summary>
    /// Initializes a new instance of the class,  providing conversion logic between <see cref="AlbumId"/> and <see cref="Guid"/>.
    /// </summary>
    public AlbumIdConverter() : base(
        v => v.Id.ToString(),
        v => new AlbumId(new Ulid(v)))
    {
    }
}
