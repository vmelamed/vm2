namespace vm2.Repository.EntityFramework.Models.Generators;

/// <summary>
/// Generates <see cref="Guid"/> values based on ULID (Universally Unique Lexicographically Sortable Identifier).
/// </summary>
/// <remarks>
/// This generator produces globally unique and lexicographically sortable GUIDs by converting ULIDs to GUIDs. The generated
/// values are not temporary and are suitable for use as primary keys or other unique identifiers.
/// </remarks>
public sealed class UlidGuidValueGenerator : ValueGenerator<Guid>
{
    static UlidFactory _ulidFactory = new();

    /// <inheritdoc />
    public override bool GeneratesTemporaryValues => false;

    /// <summary>
    /// Generates a new <see cref="Guid"/> value using ULID.
    /// </summary>
    public override Guid Next(EntityEntry entry) => _ulidFactory.NewUlid().ToGuid();
}
