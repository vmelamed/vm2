namespace vm2.Repository.EntityFramework.Models.Mapping;

/// <summary>
/// Configures the tenantId field of entities implementing <see cref="ITenanted{TTenantId}"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TTenantId">The type of the tenant identifier.</typeparam>
/// <example>
/// When using configuration class implementing <see cref="IEntityTypeConfiguration{TEntity}"/>:
/// <![CDATA[
/// class MyEntityConfiguration : IEntityTypeConfiguration<MyEntity>
/// {
///     ...
///     public void Configure(EntityTypeBuilder<MyEntity> builder)
///     {
///         ...
///         new TenantedConfiguration<MyEntity>().Configure(builder);
///     }
/// }
/// ]]></example>
public class TenantedConfiguration<TEntity, TTenantId> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, ITenanted<TTenantId>
    where TTenantId : notnull, IEquatable<TTenantId>
{
    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.TenantId)
            .IsRequired(true)
            .HasColumnOrder(0)
            ;
    }
}