namespace vm2.Repository.EntityFramework.Models.Mapping;

/// <summary>
/// Configures the audit fields of entities implementing <see cref="IAuditable"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <example>
/// When using configuration class implementing <see cref="IEntityTypeConfiguration{TEntity}"/>:
/// <![CDATA[
/// class MyEntityConfiguration : IEntityTypeConfiguration<MyEntity>
/// {
///     ...
///     public void Configure(EntityTypeBuilder<Engine> builder)
///     {
///         ...
///         new AuditableConfiguration<MyEntity>().Configure(builder);
///     }
/// }
/// ]]></example>
public class AuditableConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IAuditable
{
    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.CreatedAt)
            .IsRequired()
            ;
        builder.Property(e => e.UpdatedAt)
            .IsRequired()
            ;
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(IAuditable.MaxActorNameLength)
            .IsRequired()
            ;
        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(IAuditable.MaxActorNameLength)
            .IsRequired()
            ;
    }
}