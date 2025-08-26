namespace vm2.Repository.TestDomain;

/// <summary>
/// Represents a strongly-typed identifier for a <see cref="Label"/>, encapsulating a ULID value.
/// </summary>
/// <remarks>
/// This type is designed to provide type safety and clarity when working with entity identifiers. It supports implicit
/// conversions to the ULID type and explicit conversion from ULID for ease of use.
/// </remarks>
public readonly record struct LabelId(in Ulid Id) : IFindable
{
    #region IFindable
    /// <inheritdoc/>
    public IEnumerable<object?>? KeyValues
    {
        get { yield return Id; }
    }

    /// <inheritdoc/>
    public ValueTask ValidateFindableAsync(object? _ = null, CancellationToken __ = default)
        => Id != default ? ValueTask.CompletedTask : throw new ValidationException("The Id must not be an empty ULID.");
    #endregion

    #region Implicit type conversions
    /// <summary>
    /// Implicitly converts an <see cref="LabelId"/> to its underlying value type <see cref="Ulid"/>.
    /// </summary>
    /// <param name="id"></param>
    public static implicit operator Ulid(in LabelId id) => id.Id;

    /// <summary>
    /// Explicitly converts a value of type <see cref="Ulid"/> to an <see cref="LabelId"/>.
    /// </summary>
    /// <param name="value"></param>
    public static explicit operator LabelId(in Ulid value) => new(value);
    #endregion
}
