namespace vm2.Repository.EfRepository.Models.Converters;

/// <summary>
/// Provides a mechanism to convert between <see cref="EntityId{Ulid}"/> and byte[].
/// </summary>
public sealed class EntityIdUlidBytesConverter : ValueConverter<EntityId<Ulid>, byte[]>
{
    /// <summary>
    /// Initializes a new instance of the class,  providing conversion logic between <see cref="EntityId{Ulid}"/> and byte[].
    /// </summary>
    public EntityIdUlidBytesConverter() : base(
        v => v.Id.ToByteArray(),
        v => new EntityId<Ulid>(new Ulid(v)))
    {
    }
}

/// <summary>
/// Provides a mechanism to convert between <see cref="EntityId{Ulid}"/> and <see cref="Guid"/>.
/// </summary>
public sealed class EntityIdUlidGuidConverter : ValueConverter<EntityId<Ulid>, Guid>
{
    /// <summary>
    /// Initializes a new instance of the class,  providing conversion logic between <see cref="EntityId{Ulid}"/> and <see cref="Guid"/>.
    /// </summary>
    public EntityIdUlidGuidConverter() : base(
        v => v.Id.ToGuid(),
        v => new EntityId<Ulid>(new Ulid(v)))
    {
    }
}

/// <summary>
/// Provides a mechanism to convert between <see cref="EntityId{Ulid}"/> and <see cref="string"/>.
/// </summary>
public sealed class EntityIdUlidStringConverter : ValueConverter<EntityId<Ulid>, string>
{
    /// <summary>
    /// Initializes a new instance of the class,  providing conversion logic between <see cref="EntityId{Ulid}"/> and <see cref="Guid"/>.
    /// </summary>
    public EntityIdUlidStringConverter() : base(
        v => v.Id.ToString(),
        v => new EntityId<Ulid>(Ulid.Parse(v)))
    {
    }
}
