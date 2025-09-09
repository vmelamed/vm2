namespace vm2.Repository.DB.TestSQLite.Mapping.Generators;

/// <summary>
/// Generates strongly typed <see cref="PersonId"/>.
/// </summary>
public class PersonIdGenerator : ValueGenerator<PersonId>
{
    static UlidFactory _ulidFactory = new();

    /// <inheritdoc />
    public override bool GeneratesTemporaryValues => false;

    /// <summary>
    /// Generates a new <see cref="Guid"/> value using ULID.
    /// </summary>
    public override PersonId Next(EntityEntry entry) => new(_ulidFactory.NewUlid());
}
