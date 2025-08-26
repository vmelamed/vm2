namespace vm2.Repository.EfRepository;

/// <summary>
/// Provides extension methods for configuring <see cref="DbContext"/> that implements <see cref="IRepository"/>.
/// </summary>
/// <remarks>
/// This class contains static methods that allow developers to extend and configure repository-related options for Entity
/// Framework <see cref="DbContext"/> instances. These methods are typically used during the setup of a database context in an
/// application's dependency injection or configuration pipeline.
/// </remarks>
public static class DbContextRepositoryOptionsExtensions
{
    /// <summary>
    /// Configures the <see cref="DbContextOptionsBuilder"/> to enable boundary checking for DDD aggregates.
    /// </summary>
    /// <param name="checks">
    /// The type of the DDD aggregate boundary checks to perform. This can be a combination of <see cref="DddBoundaryChecks"/> flags:
    /// <list type="bullet">
    /// <item><see cref="DddBoundaryChecks.Validation"/>: Enables validation of the invariants before saving changes.</item>
    /// <item>
    /// <see cref="DddBoundaryChecks.AggregateBoundary"/>: Ensures that operations respect the transactional boundaries for a
    /// single DDD aggregate.
    /// </item>
    /// <item><see cref="DddBoundaryChecks.Full"/>: Combines all available checks.</item>
    /// </list>
    /// </param>
    /// <remarks>
    /// This method adds or updates the <see cref="DddAggregateBoundaryChecking"/> extension in the
    /// <see cref="DbContextOptionsBuilder"/>. Aggregate boundary checking ensures that operations (e.g. SaveChanges) respect
    /// the invariant and transactional boundaries of DDD aggregates to enforce domain consistency. For this all entities must
    /// be marked with <see cref="IAggregate{TRoot}"/>.
    /// </remarks>
    /// <param name="builder">The <see cref="DbContextOptionsBuilder"/> to configure.</param>
    /// <returns>
    /// The same <see cref="DbContextOptionsBuilder"/> instance so that additional configuration calls can be chained.
    /// </returns>
    public static DbContextOptionsBuilder<DbContextRepository> WithDddAggregateBoundaryChecking<DbContextRepository>(
        this DbContextOptionsBuilder<DbContextRepository> builder,
        DddBoundaryChecks checks = DddBoundaryChecks.Full) where DbContextRepository : DbContext, IRepository
    {
        var extension = builder
                            .Options
                            .FindExtension<DddAggregateBoundaryChecking>() ?? new DddAggregateBoundaryChecking(checks);

        ((IDbContextOptionsBuilderInfrastructure)builder).AddOrUpdateExtension(extension);
        return builder;
    }
}
