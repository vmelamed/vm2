namespace vm2.Repository.DB.TestSQLite.Mapping.Generators;

/// <summary>
/// Generates strongly typed <see cref="TrackId"/>.
/// </summary>
public class TrackIdGenerator : ValueGenerator<TrackId>
{
    static UlidFactory _ulidFactory = new();

    /// <inheritdoc />
    public override bool GeneratesTemporaryValues => false;

    /// <summary>
    /// Generates a new <see cref="Guid"/> value using ULID.
    /// </summary>
    public override TrackId Next(EntityEntry entry) => new(_ulidFactory.NewUlid());
}
