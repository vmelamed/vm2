namespace vm2.Repository.EfRepository.Models.Mapping;

using vm2.Repository.EfRepository.Models;

/// <summary>
/// Configures the key(s) of entities implementing <see cref="IFindable{TEntity}"/>.
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
///         new FindableConfiguration<MyEntity>().Configure(builder);
///     }
/// }
/// ]]></example>
public class FindableConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : class, IFindable<TEntity>
{
    /// <summary>
    /// Configures the entity of type <typeparamref name="TEntity" />.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TEntity> builder)
        => builder.HasKey(TEntity.KeyExpression);
}