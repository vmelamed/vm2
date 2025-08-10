namespace vm2.Repository.DB.SQLiteRepository.Models.Mapping;

/// <summary>
/// Configures the entity of type <typeparamref name="TEntity" /> to support optimistic concurrency.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class OptimisticConcurrencyConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IOptimisticConcurrency
{
    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // Configure the concurrency token
        builder
            .Property<Guid>(SQLiteDbContextRepository.OptimisticConcurrencyFieldName)
            .IsConcurrencyToken()
            ;
    }
}
