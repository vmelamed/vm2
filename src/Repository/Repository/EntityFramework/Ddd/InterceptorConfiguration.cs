namespace vm2.Repository.EntityFramework.Ddd;

/// <summary>
/// Represents the configuration options for a Domain-Driven Design (DDD) <see cref="SaveChangesInterceptor"/> that can be
/// changed for the duration of a unit of work (transaction). Usually, the repository (<see cref="EfRepository"/>) implements
/// <see cref="IHasDddInterceptorConfigurator"/> to provide the modified configuration.
/// </summary>
/// <param name="AllowedAggregateRoots">
/// A set of allowed aggregate root types that the interceptor can extend the boundary to. If <see langword="null"/>
/// (the default) or empty,<br/>
/// only one aggregate root type is allowed - the type of the root of the first encountered entity in added, modified or deleted
/// state.<para/>
/// Please, use with caution. Allowing multiple aggregate roots in a unit of work (transaction) can lead to optimistic<br/>
/// concurrency exceptions. Before using this option, consider if your aggregates are properly designed.
/// </param>
/// <param name="TenantProvider">
/// An optional tenancy provider that specifies the tenant context for the interceptor. If <see langword="null"/>, no tenancy
/// context is applied.
/// </param>
/// <param name="Actions">
/// Specifies the actions that the interceptor should perform to ensure the invariants and consistency boundaries of the current
/// tenant and the current aggregate. Defaults to <see cref="DddAggregateActions.All"/>.
/// </param>
/// <param name="ActorAuditProvider">
/// A function that provides an audit fragment string that represents the current actor who initiated the current unit of work.
/// If <see langword="null"/>, an empty string is used.
/// </param>
/// <param name="DateTimeAuditProvider">
/// A function that provides the current date and time. If <see langword="null"/>, the default system UTC date and time is
/// used.
/// </param>
public readonly record struct InterceptorConfiguration(
    DddAggregateActions Actions = Interceptor.DefaultActions,
    ISet<Type>? AllowedAggregateRoots = default,
    Func<ITenanted>? TenantProvider = default,
    Func<string>? ActorAuditProvider = default,
    Func<DateTime>? DateTimeAuditProvider = default);
