namespace vm2.Repository.EntityFramework.Ddd;

/// <summary>
/// Represents a contract for re-configuring the DDD interceptor for the duration of the  current unit of work. Usually
/// is implemented by the repository (<see cref="EfRepository"/>).
/// </summary>
/// <remarks>
/// Please note that the interceptor configuration is modified only for the current unit of work (transaction). After that it
/// reverts to the previous configuration.
/// </remarks>
public interface IHasDddInterceptorConfigurator
{
    /// <summary>
    /// Used to modify the configured aggregate boundary actions.
    /// </summary>
    /// <remarks>
    /// Hint: before modifying <see cref="AggregateActions"/> or <see cref="AllowedAggregateRoots"/>, please, consider employing
    /// eventual consistency (e.g. sending messages for updating the affected entities in the other aggregates) and/or to
    /// rethink the boundaries of your aggregates.
    /// </remarks>
    public DddAggregateActions? AggregateActions { get; set; }

    /// <summary>
    /// Used to allow more than one aggregate to participate in the transaction of the UoW. <see cref="EfRepository"/> always
    /// allows entities from one aggregate to participate in the current transaction (UoW). If you want to allow other aggregate
    /// roots' types - add them to this collection. In that case add also the type of the main aggregate root.
    /// </summary>
    /// <remarks>
    /// Hint: before modifying <see cref="AggregateActions"/> or <see cref="AllowedAggregateRoots"/>, please, consider employing
    /// eventual consistency (e.g. sending messages for updating the affected entities in the other aggregates) and/or to
    /// rethink the boundaries of your aggregates.
    /// </remarks>
    public ISet<Type>? AllowedAggregateRoots { get; set; }

    /// <summary>
    /// Gets the function used to provide the current date and time for auditing purposes.
    /// </summary>
    /// <remarks>
    /// The default implementation returns the current UTC date and time using <see cref="DateTime.UtcNow"/>. For testing
    /// purposes, you can override this property to return a fixed date and time or to use a different time source.
    /// </remarks>
    public Func<DateTime>? DateTimeAuditProvider { get; set; }

    /// <summary>
    /// Gets or sets the function that provides the identifier of the current actor for auditing purposes.
    /// </summary>
    /// <remarks>
    /// This property is typically used to supply the identity of the user or system performing the current action, which can be
    /// included in audit logs. Ensure the provided function returns a meaningful identifier for the current context. E.g.,
    /// retrieving the user ID from the current security context (JWT, HttpContext, gRPC context, etc.)
    /// </remarks>
    public Func<string>? CurrentActorAuditProvider { get; set; }

    /// <summary>
    /// Temporarily modifies the <paramref name="currentConfiguration"/> with the values of the properties of this interface and
    /// <see cref="ITenanted"/> (if implemented).<br/>
    /// It is called by the <see cref="Interceptor"/> before <see cref="IRepository.CommitAsync"/>, giving the domain services
    /// the opportunity<br/>
    /// to modify the default DDD behavior for the current transaction only.
    /// </summary>
    /// <remarks>
    /// Please, use with caution. Allowing multiple aggregate roots in a unit of work (transaction) can lead to optimistic<br/>
    /// concurrency exceptions. Before overriding this method, consider if your aggregates are properly designed.
    /// </remarks>
    /// <param name="currentConfiguration">
    /// The current <see cref="InterceptorConfiguration"/> to be modified.
    /// </param>
    /// <returns>A new <see cref="InterceptorConfiguration"/> instance containing the updated settings.</returns>
    InterceptorConfiguration ConfigureDddInterceptor(InterceptorConfiguration currentConfiguration)
        => new(
            Actions: AggregateActions                     ?? currentConfiguration.Actions,
            AllowedAggregateRoots: AllowedAggregateRoots  ?? currentConfiguration.AllowedAggregateRoots,
            TenantProvider: this is ITenanted t ? () => t : currentConfiguration.TenantProvider,
            ActorAuditProvider: CurrentActorAuditProvider ?? currentConfiguration.ActorAuditProvider,
            DateTimeAuditProvider: DateTimeAuditProvider  ?? currentConfiguration.DateTimeAuditProvider
        );
}
