namespace vm2.Repository.EfRepository.Models.Generators;

/// <summary>
/// Generates <see cref="string"/> values based on ULID (Universally Unique Lexicographically Sortable Identifier).
/// </summary>
/// <remarks>
/// This generator produces globally unique and lexicographically sortable GUIDs by converting ULIDs to <see cref="string"/>-s.
/// The generated values are not temporary and are suitable for use as primary keys or other unique identifiers.
/// </remarks>
public sealed class UlidStringValueGenerator : ValueGenerator<string>
{
    /// <summary>
    /// The length of the generated ULID string in Base64 format.
    /// </summary>
    public const int IdLength = 26; // the Base64 length of ULID

    /// <inheritdoc />
    public override bool GeneratesTemporaryValues => false;

    /// <summary>
    /// Generates a new <see cref="Guid"/> value using ULID.
    /// </summary>
    public override string Next(EntityEntry entry) => Ulid.NewUlid().ToBase64();
}
