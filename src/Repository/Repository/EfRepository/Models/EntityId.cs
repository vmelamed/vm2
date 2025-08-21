namespace vm2.Repository.EfRepository.Models;

/// <summary>
/// Represents a strongly-typed identifier for an entity, encapsulating a value of the specified type.
/// </summary>
/// <remarks>
/// This type is designed to provide type safety and clarity when working with entity identifiers. It supports implicit
/// conversions to and from the underlying value type for ease of use.
/// </remarks>
/// <typeparam name="TValue">The type of the underlying value. Must be a value type (<see langword="struct"/>).</typeparam>
/// <param name="Id"></param>
public readonly record struct EntityId<TValue>(TValue Id) : IFindable where TValue : notnull
{
    #region IFindable
    /// <inheritdoc/>
    public IEnumerable<object?>? KeyValues
    {
        get { yield return Id; }
    }

    /// <inheritdoc/>
    public ValueTask ValidateFindable(object? _ = null, CancellationToken __ = default) => ValueTask.CompletedTask;
    #endregion

    #region Implicit type conversions
    /// <summary>
    /// Implicitly converts an <see cref="EntityId{TValue}"/> to its underlying value type <typeparamref name="TValue"/>.
    /// </summary>
    /// <param name="id"></param>
    public static implicit operator TValue(EntityId<TValue> id) => id.Id;

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="TValue"/> to an <see cref="EntityId{TValue}"/>.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator EntityId<TValue>(TValue value) => new(value);
    #endregion
}
