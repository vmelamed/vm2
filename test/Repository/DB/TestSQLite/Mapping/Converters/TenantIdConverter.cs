namespace vm2.Repository.DB.TestSQLite.Mapping.Converters;

/// <summary>
/// Provides a mechanism to convert between <see cref="TenantId"/> and <see cref="Guid"/>.
/// </summary>
public sealed class TenantIdConverter : ValueConverter<TenantId, Guid>
{
    /// <summary>
    /// Initializes a new instance of the class,  providing conversion logic between <see cref="TenantId"/> and <see cref="Guid"/>.
    /// </summary>
    public TenantIdConverter() : base(
        v => v.Id,
        v => new TenantId(v))
    {
    }
}
