namespace vm2.Repository.EntityFramework.Models.Generators;

/// <summary>
/// Generates <see cref="byte"/>[] values based on ULID (Universally Unique Lexicographically Sortable Identifier).
/// </summary>
/// <remarks>
/// This generator produces globally unique and lexicographically sortable GUIDs by converting ULIDs to <see cref="byte"/>[]-s.
/// The generated values are not temporary and are suitable for use as primary keys or other unique identifiers.
/// </remarks>
public sealed class UlidBinValueGenerator : ValueGenerator<byte[]>
{
    /// <summary>
    /// The length of the generated ULID in bytes.
    /// </summary>
    public const int IdLength = 16;

    /// <inheritdoc />
    public override bool GeneratesTemporaryValues => false;

    /// <summary>
    /// Generates a new <see cref="Guid"/> value using ULID.
    /// </summary>
    public override byte[] Next(EntityEntry entry) => Ulid.NewUlid().ToByteArray();
}
