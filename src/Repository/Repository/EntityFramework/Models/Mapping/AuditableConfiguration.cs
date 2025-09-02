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
///     public void Configure(EntityTypeBuilder<MyEntity> builder)
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
            .HasColumnOrder(10000)
            ;
        builder.Property(e => e.CreatedBy)
            .HasMaxLength(IAuditable.MaxActorNameLength)
            .IsRequired()
            .HasColumnOrder(10001)
            ;
        builder.Property(e => e.UpdatedAt)
            .IsRequired()
            .HasColumnOrder(10002)
            ;
        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(IAuditable.MaxActorNameLength)
            .IsRequired()
            .HasColumnOrder(10003)
            ;
    }
}