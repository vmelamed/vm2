namespace vm2.Repository.TestDomain;

/// <summary>
/// Represents a strongly-typed identifier for a <see cref="Person"/>, encapsulating a ULID value.
/// </summary>
/// <remarks>
/// This type is designed to provide type safety and clarity when working with entity identifiers. It supports implicit
/// conversions to the ULID type and explicit conversion from ULID for ease of use.
/// </remarks>
[JsonConverter(typeof(PersonIdJsonConverter))]
public readonly record struct PersonId(in Ulid Id)
{
    #region Implicit type conversions
    /// <summary>
    /// Implicitly converts an <see cref="EntityId{TValue}"/> to its underlying value type <typeparamref name="TValue"/>.
    /// </summary>
    /// <param name="id"></param>
    public static implicit operator Ulid(in PersonId id) => id.Id;

    /// <summary>
    /// Explicitly converts a value of type <typeparamref name="TValue"/> to an <see cref="EntityId{TValue}"/>. The conversion is
    /// explicit to ensure that the developer is aware of the type change and to prevent accidental conversions.
    /// </summary>
    /// <param name="value"></param>
    public static explicit operator PersonId(in Ulid value) => new(value);
    #endregion
}
