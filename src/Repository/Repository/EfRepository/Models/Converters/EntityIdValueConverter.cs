namespace vm2.Repository.EfRepository.Models.Converters;

/// <summary>
/// Provides a mechanism to convert between <see cref="EntityId{TValue}"/> and its underlying value type.
/// </summary>
/// <remarks>
/// This converter is designed to simplify the transformation of <see cref="EntityId{TValue}"/> objects to the underlying
/// value type <typeparamref name="TValue"/> and vice versa. Especially useful in Entity Framework Core for mapping, well
/// entity IDs.
/// </remarks>
public sealed class EntityIdValueConverter<TValue> : ValueConverter<EntityId<TValue>, TValue> where TValue : notnull
{
    /// <summary>
    /// Initializes a new instance of the class,  providing conversion logic between
    /// <see cref="EntityId{TValue}"/> and <typeparamref name="TValue"/>.
    /// </summary>
    /// <remarks>This constructor sets up the base conversion logic to transform a <typeparamref name="TValue"/>  into an
    /// <see cref="EntityId{TValue}"/> and vice versa. The forward conversion extracts the
    /// <see cref="EntityId{TValue}.Id"/>, while the reverse conversion creates a new  <see cref="EntityId{TValue}"/>
    /// from the provided value.
    /// </remarks>
    public EntityIdValueConverter() : base(
        v => (TValue)v,
        v => (EntityId<TValue>)(v))
    {
    }
}
