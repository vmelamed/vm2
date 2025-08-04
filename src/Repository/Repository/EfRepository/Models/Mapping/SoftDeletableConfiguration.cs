namespace vm2.Repository.EfRepository.Models.Mapping;

using vm2.Repository.EfRepository.Models;

/// <summary>
/// Configures the soft-delete fields of entities implementing <see cref="ISoftDeletable"/>.
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
///         new DeletableConfiguration<MyEntity>().Configure(builder);
///     }
/// }
/// ]]></example>
public class SoftDeletableConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, ISoftDeletable
{
    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.DeletedAt)
            .IsRequired(false)
            ;
        builder.Property(e => e.DeletedBy)
            .HasMaxLength(128)
            .IsRequired()
            ;
    }
}