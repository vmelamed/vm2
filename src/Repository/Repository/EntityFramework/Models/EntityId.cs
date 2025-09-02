namespace vm2.Repository.EntityFramework.Models;

/// <summary>
/// Represents a strongly-typed identifier for an entity, encapsulating an underlying value of type <typeparamref name="TId"/>.
/// </summary>
/// <remarks>
/// This type is designed to provide type safety and clarity when working with entity identifiers, ensuring that identifiers are
/// explicitly associated with their corresponding entity types. It supports implicit conversions to and from the underlying
/// value type <typeparamref name="TId"/> for convenience while maintaining the benefits of strong typing.
/// </remarks>
/// <typeparam name="TId">The type of the underlying identifier value.</typeparam>
/// <param name="Id">The name of the property identifier.</param>
public readonly record struct EntityId<TId>(in TId Id)
{
    #region Implicit type conversions
    /// <summary>
    /// Implicitly converts an <see cref="EntityId{TId}"/> to its underlying value type <typeparamref name="TId"/>.
    /// </summary>
    /// <param name="id">The identifier value to be converted to the underlying <typeparamref name="TId"/>.</param>
    public static implicit operator TId(in EntityId<TId> id) => id.Id;

    /// <summary>
    /// Explicitly converts a value of type <typeparamref name="TId"/> to an <see cref="EntityId{TId}"/>. The conversion is
    /// explicit to ensure that the developer is aware of the type change and to prevent accidental conversions.
    /// </summary>
    /// <param name="id">The value to be converted to strongly typed identifier type.</param>
    public static explicit operator EntityId<TId>(in TId id) => new(id);
    #endregion
}