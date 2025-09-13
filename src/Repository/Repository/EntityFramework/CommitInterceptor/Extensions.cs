namespace vm2.Repository.EntityFramework.CommitInterceptor;

/// <summary>
/// Provides extension methods for configuring DDD (Domain-Driven Design) aggregate actions in an Entity Framework on the
/// <see cref="DbContextOptionsBuilder"/>.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Configures the <see cref="DbContextOptionsBuilder"/> to use DDD aggregate actions with optional auditing capabilities.
    /// </summary>
    /// <remarks>
    /// This method adds a <see cref="CommitInterceptor"/> to the <paramref name="optionsBuilder"/> to enable the specified DDD
    /// aggregate actions and optional auditing features. Use this method to integrate DDD patterns and auditing into your
    /// application's database context configuration.
    /// </remarks>
    /// <returns>The configured <see cref="DbContextOptionsBuilder"/> instance.</returns>
    public static DbContextOptionsBuilder UseCommitInterceptor<TPolicy>(
        this DbContextOptionsBuilder optionsBuilder,
        IServiceProvider services,
        IConfiguration configuration,
        Func<IServiceProvider, IConfiguration, TPolicy> configPolicy) where TPolicy : ICommitPolicy
        => optionsBuilder.AddInterceptors(
                new CommitInterceptor(
                    new InterceptorConfigurationBuilder(services, configuration)
                            .WithPolicy(configPolicy)
                            .Build()
                )
            );
}
