namespace vm2.Repository.EntityFramework.Models.Mapping;

/// <summary>
/// Configures the entity of type <typeparamref name="TEntity" /> to support optimistic concurrency.
/// </summary>
/// <typeparam name="TEntity">The type of the entity.</typeparam>
/// <typeparam name="TTag">The type of the concurrency token.</typeparam>
public class OptimisticConcurrencyConfiguration<TEntity, TTag> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IOptimisticConcurrency<TTag>
{
    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // Configure the concurrency token
        builder
            .Property(e => e.ETag)
            .IsRequired()
            .ValueGeneratedOnAddOrUpdate()
            .IsConcurrencyToken()
            .HasColumnOrder(10100)  // Ensure the concurrency token is the last column - no one cares about its value
            ;
    }
}
