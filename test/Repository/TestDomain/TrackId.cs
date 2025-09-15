namespace vm2.Repository.TestDomain;

/// <summary>
/// Represents a strongly-typed identifier for a <see cref="Track"/>, encapsulating a ULID value.
/// </summary>
/// <remarks>
/// This type is designed to provide type safety and clarity when working with entity identifiers. It supports implicit
/// conversions to the ULID type and explicit conversion from ULID for ease of use.
/// </remarks>
[JsonConverter(typeof(TrackIdJsonConverter))]
public readonly record struct TrackId(in Ulid Id)
{
    #region Implicit type conversions
    /// <summary>
    /// Implicitly converts an <see cref="TrackId"/> to its underlying value type <see cref="Ulid"/>.
    /// </summary>
    /// <param name="id"></param>
    public static implicit operator Ulid(in TrackId id) => id.Id;

    /// <summary>
    /// Explicitly converts a value of type <see cref="Ulid"/> to an <see cref="TrackId"/>. The conversion is
    /// explicit to ensure that the developer is aware of the type change and to prevent accidental conversions.
    /// </summary>
    /// <param name="value"></param>
    public static explicit operator TrackId(in Ulid value) => new(value);
    #endregion

    public static bool TryParse(string? input, IFormatProvider? _, out TrackId trackId)
    {
        if (Ulid.TryParse(input, out Ulid ulid))
        {
            trackId = new TrackId(ulid);
            return true;
        }
        trackId = default;
        return false;
    }

    public static bool TryParse(string? input, out TrackId trackId)
        => TryParse(input, null, out trackId);
}
