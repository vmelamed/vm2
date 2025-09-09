namespace vm2.Repository.DB.TestSQLite.Mapping.Generators;

/// <summary>
/// Generates strongly typed <see cref="LabelId"/>.
/// </summary>
public class LabelIdGenerator : ValueGenerator<LabelId>
{
    static UlidFactory _ulidFactory = new();

    /// <inheritdoc />
    public override bool GeneratesTemporaryValues => false;

    /// <summary>
    /// Generates a new <see cref="Guid"/> value using ULID.
    /// </summary>
    public override LabelId Next(EntityEntry entry) => new(_ulidFactory.NewUlid());
}
