namespace vm2.Repository.EntityFramework.Ddd;

/// <summary>
/// A custom Entity Framework Core save changes interceptor that checks for domain-driven design (DDD) aggregate invariants and
/// boundaries, tenant boundaries, and performs auditing and completion actions on entities being added, modified, or deleted.
/// </summary>
class Interceptor : SaveChangesInterceptor
{
    /// <summary>
    /// Represents the default set of actions available for a Domain-Driven Design (DDD) aggregate.
    /// </summary>
    public const DddAggregateActions DefaultActions = DddAggregateActions.All;

    /// <summary>
    /// Gets the default function that provides the current actor audit fragment.
    /// </summary>
    public static readonly Func<string> DefaultActorAuditProvider = () => "unknown";

    /// <summary>
    /// Gets the default function used to provide the current audit timestamp.
    /// </summary>
    public static readonly Func<DateTime> DefaultDateTimeAuditProvider = () => DateTime.UtcNow;

    /// <summary>
    /// Gets the configuration settings for the interceptor.
    /// </summary>
    public InterceptorConfiguration Configuration { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Interceptor"/> class with the specified configuration.
    /// </summary>
    /// <param name="configuration">
    /// The configuration settings used to initialize the interceptor. This parameter must be provided and defines the default
    /// behavior of the interceptor. However, the configuration can be modified for each unit of work (transaction) by
    /// implementing <see cref="IHasDddInterceptorConfigurator"/> in the <see cref="DbContext"/>.
    /// </param>
    public Interceptor(
        in InterceptorConfiguration configuration)
        => Configuration = configuration with {
            ActorAuditProvider    = configuration.ActorAuditProvider ?? DefaultActorAuditProvider,
            DateTimeAuditProvider = configuration.DateTimeAuditProvider ?? DefaultDateTimeAuditProvider,
        };

    /// <inheritdoc />
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var dbContext  = eventData.Context
                                ?? throw new InvalidOperationException("The context in eventData is null or is not DbContext.");
        var changeTracker = dbContext.ChangeTracker;

        changeTracker.DetectChanges();

        // if nothing changed, do nothing
        if (changeTracker.Entries().All(e => e.State is EntityState.Unchanged or EntityState.Detached))
            return result;

        var configuration = (dbContext as IHasDddInterceptorConfigurator)?.ConfigureDddInterceptor(Configuration) ?? Configuration;

        // if no actions, do nothing
        if (configuration.Actions == DddAggregateActions.None)
            return result;

        // call all providers and build the initial actions parameters
        var actionParameters    = new ActionsParameters(
                                            Entry:             null!,
                                            AggregateRoot:     null,
                                            Actions:           configuration.Actions,
                                            AllowedRoots:      configuration.AllowedAggregateRoots           ?? new HashSet<Type>(),
                                            Tenanted:          configuration.TenantProvider?.Invoke()        ?? dbContext as ITenanted,
                                            Actor:             configuration.ActorAuditProvider?.Invoke()    ?? DefaultActorAuditProvider(),
                                            Now:               configuration.DateTimeAuditProvider?.Invoke() ?? DefaultDateTimeAuditProvider(),
                                            CancellationToken: ct);

        var tenanted          = actionParameters.Tenanted;
        var aggregateRootType = actionParameters.AllowedRoots.FirstOrDefault();

        foreach (var entry in changeTracker.Entries().Where(e => e.State is EntityState.Added
                                                                         or EntityState.Modified
                                                                         or EntityState.Deleted))
        {
            // update parameters with the current entry, the tenanted context from of the previous entry and aggregate root type
            // of the previous entry. There can be only one tenant context and a single aggregate root type in a single unit of
            // work (transaction). If AllowedAggregateRoots is set, the aggregate root type can be extended to any of the allowed
            // types.
            var parameters = actionParameters with
            {
                Entry         = entry,
                AggregateRoot = aggregateRootType,
                Tenanted      = actionParameters.Tenanted ?? tenanted,
            };

            Func<ValueTask> actionFunctions = entry.State switch {
                EntityState.Added => async () =>
                                        {
                                            tenanted = CheckTenantBoundary(parameters);
                                            aggregateRootType = CheckAggregateBoundary(parameters);
                                            AuditAdded(parameters);
                                            await CompleteAsync(parameters);
                                            await ValidateInvariantsAsync(parameters);
                                        }
                ,

                EntityState.Modified => async () =>
                                        {
                                            tenanted = CheckTenantBoundary(parameters);
                                            aggregateRootType = CheckAggregateBoundary(parameters);
                                            AuditUpdated(parameters);
                                            await CompleteAsync(parameters);
                                            await ValidateInvariantsAsync(parameters);
                                        }
                ,

                EntityState.Deleted => () =>
                                        {
                                            tenanted = CheckTenantBoundary(parameters);
                                            aggregateRootType = CheckAggregateBoundary(parameters);
                                            AuditDeleted(parameters);
                                            return ValueTask.CompletedTask;
                                        }
                ,

                _ => () => ValueTask.CompletedTask,
            };
            await actionFunctions().ConfigureAwait(false);
        }

        ct.ThrowIfCancellationRequested();

        return result;
    }

    const string differentTenants
        = "Found entities from different tenants, or from a tenant different from the repository's tenant.";

    const string dddErrorMessageHasMoreThanOneAggregate
        = "DDD error: Type \"{0}\" is marked with multiple IAggregate<TRoot> interfaces. An entity or value cannot be part of more than one aggregate.";

    const string dddErrorMessageViolationOfAggregateBoundary1
        = "DDD error: Violation of aggregate boundaries: encountered entities of more than one aggregate: IAggregate<{0}> and IAggregate<{1}>.";

    const string dddErrorMessageViolationOfAggregateBoundary2
        = "DDD error: Violation of aggregate boundaries: entities of IAggregate<{0}> are not allowed.";

    static ITenanted? CheckTenantBoundary(in ActionsParameters p)
    {
        // if no tenant boundary check is required, return the current tenanted context
        if (!p.Actions.HasFlag(DddAggregateActions.TenantBoundary))
            return p.Tenanted;

        // if the entity is not tenanted, return the current tenanted context
        if (p.Entry.Entity is not ITenanted tenanted)
            return p.Tenanted;

        // if the context is still unknown, return the entity's tenanted context
        // (this will become the required tenant context for the following entities)
        if (p.Tenanted is null)
            return tenanted;

        // if the entity's tenant is the same as the context's tenant, return the current tenanted context.
        if (p.Tenanted.SameTenantAs(tenanted))
            return p.Tenanted;

        // if the entity's tenant is different from the context's tenant, throw exception.
        // All tenanted entities must belong to the same tenant.
        throw new InvalidOperationException(differentTenants);
    }

    static Type? CheckAggregateBoundary(in ActionsParameters p)
    {
        // if no aggregate boundary check is required, return the current aggregate root type
        if (!p.Actions.HasFlag(DddAggregateActions.AggregateBoundary))
            return p.AggregateRoot;

        // determine the aggregate root type of the current entity
        var rootTypes = p.Entry
                            .Entity
                            .GetType()
                            .GetInterfaces()
                            .Where(i => i.IsGenericType  &&  i.GetGenericTypeDefinition() == typeof(IAggregate<>))
                            .Select(i => i.GetGenericArguments()[0])
                            .ToList()
                            ;

        var rootType = rootTypes.Count is <= 1
                            ? rootTypes.SingleOrDefault()
                            : throw new InvalidOperationException(
                                            string.Format(dddErrorMessageHasMoreThanOneAggregate, p.Entry.Entity.GetType().Name));

        Debug.Assert(rootType is not null);

        if (p.AggregateRoot == rootType  ||  p.AllowedRoots.Contains(rootType))
            return rootType;

        if (p.AggregateRoot is null  &&  p.AllowedRoots.Count == 0) // rootType will become the required root type for the other entities in the tracker
            return rootType;

        throw new InvalidOperationException(
                    p.AllowedRoots.Count == 0
                        ? string.Format(dddErrorMessageViolationOfAggregateBoundary1, p.AggregateRoot?.Name, rootType.Name)
                        : string.Format(dddErrorMessageViolationOfAggregateBoundary2, rootType.Name));
    }

    static void AuditAdded(in ActionsParameters p)
    {
        if (p.Actions.HasFlag(DddAggregateActions.Audit)  &&  p.Entry.Entity is IAuditable auditable)
            auditable.AuditOnAdd(p.Now, p.Actor);
    }

    static void AuditUpdated(in ActionsParameters p)
    {
        if (p.Actions.HasFlag(DddAggregateActions.Audit)  &&   p.Entry.Entity is IAuditable auditable)
            auditable.AuditOnUpdate(p.Now, p.Actor);
    }

    static void AuditDeleted(in ActionsParameters p)
    {
        if (p.Actions.HasFlag(DddAggregateActions.Audit)  &&  p.Entry.Entity is ISoftDeletable deletable)
        {
            deletable.SoftDelete(p.Now, p.Actor);
            p.Entry.State = EntityState.Modified;   // do not delete the entity physically
        }
    }

    static ValueTask CompleteAsync(in ActionsParameters p)
        => p.Actions.HasFlag(DddAggregateActions.Complete)  &&  p.Entry.Entity is ICompletable completable
                ? completable.CompleteAsync(p.Entry.Context as IRepository, p.Entry, p.Now, p.Actor, p.CancellationToken)
                : ValueTask.CompletedTask;

    static ValueTask ValidateInvariantsAsync(in ActionsParameters p)
        => p.Actions.HasFlag(DddAggregateActions.Invariants)  &&  p.Entry.Entity is IValidatable validatable
                ? validatable.ValidateAsync(p.Entry.Context as IRepository, p.CancellationToken)
                : ValueTask.CompletedTask;
}
