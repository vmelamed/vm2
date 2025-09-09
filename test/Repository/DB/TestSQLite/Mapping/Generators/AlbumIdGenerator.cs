namespace vm2.Repository.DB.TestSQLite.Mapping.Generators;

/// <summary>
/// Generates strongly typed <see cref="AlbumId"/>.
/// </summary>
public class AlbumIdGenerator : ValueGenerator<AlbumId>
{
    static UlidFactory _ulidFactory = new();

    /// <inheritdoc />
    public override bool GeneratesTemporaryValues => false;

    /// <summary>
    /// Generates a new <see cref="Guid"/> value using ULID.
    /// </summary>
    public override AlbumId Next(EntityEntry entry) => new(_ulidFactory.NewUlid());
}
