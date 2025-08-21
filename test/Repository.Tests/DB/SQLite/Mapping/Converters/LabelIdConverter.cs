namespace vm2.Repository.Tests.DB.SQLite.Mapping.Converters;

/// <summary>
/// Provides a mechanism to convert between <see cref="LabelId"/> and <see cref="Guid"/>.
/// </summary>
public sealed class LabelIdConverter : ValueConverter<LabelId, Guid>
{
    /// <summary>
    /// Initializes a new instance of the class,  providing conversion logic between <see cref="LabelId"/> and <see cref="Guid"/>.
    /// </summary>
    public LabelIdConverter() : base(
        v => v.Id.ToGuid(),
        v => new LabelId(new Ulid(v)))
    {
    }
}
