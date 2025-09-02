namespace vm2.Repository.EntityFramework.Ddd;

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
    /// This method adds a <see cref="Interceptor"/> to the <paramref name="optionsBuilder"/> to enable the specified DDD
    /// aggregate actions and optional auditing features. Use this method to integrate DDD patterns and auditing into your
    /// application's database context configuration.
    /// </remarks>
    /// <param name="optionsBuilder">
    /// The <see cref="DbContextOptionsBuilder"/> to configure.
    /// </param>
    /// <param name="actions">
    /// The <see cref="DddAggregateActions"/> to enable. Defaults to <see cref="DddAggregateActions.All"/>.
    /// </param>
    /// <param name="tenantProvider">
    /// An optional delegate that provides the current tenant context. If specified, this delegate is invoked to supply
    /// </param>
    /// <param name="actorAuditProvider">
    /// An optional delegate that provides the current actor's audit fragment. If specified, this delegate is invoked to supply
    /// the actor information for auditing purposes.
    /// </param>
    /// <param name="dateTimeAuditProvider">
    /// An optional delegate that provides the current date and time for auditing purposes. If specified, this delegate is
    /// invoked to supply the audit timestamp.
    /// </param>
    /// <returns>The configured <see cref="DbContextOptionsBuilder"/> instance.</returns>
    public static DbContextOptionsBuilder UseDddInterceptor(
        this DbContextOptionsBuilder optionsBuilder,
        DddAggregateActions actions = DddAggregateActions.All,
        Func<ITenanted>? tenantProvider = default,
        Func<string>? actorAuditProvider = default,
        Func<DateTime>? dateTimeAuditProvider = default)
        => optionsBuilder.AddInterceptors(
                new Interceptor(
                    new InterceptorConfiguration(
                            actions,
                            TenantProvider: tenantProvider,
                            ActorAuditProvider: actorAuditProvider,
                            DateTimeAuditProvider: dateTimeAuditProvider)));

    /// <summary>
    /// Configures the <see cref="DbContextOptionsBuilder"/> to use DDD aggregate actions with optional auditing capabilities.
    /// </summary>
    /// <remarks>
    /// This method adds a <see cref="Interceptor"/> to the <paramref name="optionsBuilder"/> to enable the specified DDD
    /// aggregate actions and optional auditing features. Use this method to integrate DDD patterns and auditing into your
    /// application's database context configuration.
    /// </remarks>
    /// <typeparam name="TRepository">The type of the <see cref="DbContext"/> being configured.</typeparam>
    /// <param name="optionsBuilder">The <see cref="DbContextOptionsBuilder"/> to configure.</param>
    /// <param name="actions">
    /// The <see cref="DddAggregateActions"/> to enable. Defaults to <see cref="DddAggregateActions.All"/>.
    /// </param>
    /// <param name="tenantProvider">
    /// An optional delegate that provides the current tenant context. If specified, this delegate is invoked to supply
    /// </param>
    /// <param name="actorAuditProvider">
    /// An optional delegate that provides the current actor's audit fragment. If specified, this delegate is invoked to supply
    /// the actor information for auditing purposes.
    /// </param>
    /// <param name="dateTimeAuditProvider">
    /// An optional delegate that provides the current date and time for auditing purposes. If specified, this delegate is
    /// invoked to supply the audit timestamp.
    /// </param>
    /// <returns>The configured <see cref="DbContextOptionsBuilder"/> instance.</returns>
    public static DbContextOptionsBuilder<TRepository> UseDddInterceptor<TRepository>(
        this DbContextOptionsBuilder<TRepository> optionsBuilder,
        DddAggregateActions actions = DddAggregateActions.All,
        Func<ITenanted>? tenantProvider = default,
        Func<string>? actorAuditProvider = default,
        Func<DateTime>? dateTimeAuditProvider = default) where TRepository : DbContext
        => (DbContextOptionsBuilder<TRepository>)UseDddInterceptor(
                                                    (DbContextOptionsBuilder)optionsBuilder,
                                                    actions,
                                                    tenantProvider,
                                                    actorAuditProvider,
                                                    dateTimeAuditProvider);
}
