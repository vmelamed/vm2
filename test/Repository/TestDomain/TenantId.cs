namespace vm2.Repository.TestDomain;

/// <summary>
/// Represents a strongly-typed identifier for a <see cref="Tenant"/>, encapsulating a ULID value.
/// </summary>
/// <remarks>
/// This type is designed to provide type safety and clarity when working with entity identifiers. It supports implicit
/// conversions to the ULID type and explicit conversion from ULID for ease of use.
/// </remarks>
[JsonConverter(typeof(TenantIdJsonConverter))]
public readonly record struct TenantId(in Guid Id)
{
    #region Implicit type conversions
    /// <summary>
    /// Implicitly converts an <see cref="TenantId"/> to its underlying value type <see cref="Guid"/>.
    /// </summary>
    /// <param name="id"></param>
    public static implicit operator Guid(in TenantId id) => id.Id;

    /// <summary>
    /// Explicitly converts a value of type <see cref="Guid"/> to an <see cref="TenantId"/>. The conversion is
    /// explicit to ensure that the developer is aware of the type change and to prevent accidental conversions.
    /// </summary>
    /// <param name="value"></param>
    public static explicit operator TenantId(in Guid value) => new(value);
    #endregion
}
