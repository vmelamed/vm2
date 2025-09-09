namespace vm2.Repository.DB.TestSQLite.Mapping.Converters;

/// <summary>
/// Provides a mechanism to convert between <see cref="LabelId"/> and <see cref="Guid"/>.
/// </summary>
public sealed class LabelIdConverter : ValueConverter<LabelId, string>
{
    /// <summary>
    /// Initializes a new instance of the class,  providing conversion logic between <see cref="LabelId"/> and <see cref="Guid"/>.
    /// </summary>
    public LabelIdConverter() : base(
        v => v.Id.ToString(),
        v => new LabelId(new Ulid(v)))
    {
    }
}
