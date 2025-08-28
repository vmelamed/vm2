namespace vm2.Repository.EntityFramework.Ddd;

using vm2.Repository.EntityFramework;

/// <summary>
/// A custom Entity Framework Core save changes interceptor that checks for domain-driven design (DDD) aggregate invariants and
/// boundaries.
/// </summary>
public class EfRepositorySaveChangesInterceptor : SaveChangesInterceptor
{
    record struct ActionParameters(
        EntityEntry Entry,
        AggregateActions Actions,
        ITenanted? Tenanted,
        Type? AggregateRoot,
        HashSet<Type> AllowedRoots,
        DateTime Now,
        string Actor,
        CancellationToken CancellationToken);

    /// <inheritdoc />
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        var efRepository = eventData.Context as EfRepository
                                ?? throw new InvalidOperationException("The context in eventData is null or is not EfRepository.");
        var changeTracker = efRepository.ChangeTracker;

        changeTracker.DetectChanges();

        if (efRepository.AggregateActions == AggregateActions.None ||
            changeTracker
                .Entries()
                .All(e => e.State is EntityState.Unchanged or EntityState.Detached))
            return await base.SavingChangesAsync(eventData, result, ct);

        ITenanted? tenanted = efRepository as ITenanted;
        Type? aggregateRootType = null;
        var actionParameters = new ActionParameters(
                                        Entry:         null!,
                                        Actions:       efRepository.AggregateActions,
                                        Tenanted:      tenanted,
                                        AggregateRoot: aggregateRootType,
                                        AllowedRoots:  efRepository.AllowedRoots,
                                        Now:           DateTime.UtcNow,
                                        Actor:         "",  // TODO: replace with actual actor, e.g. from some call context or user token
                                        CancellationToken: ct);

        foreach (var entry in changeTracker
                                    .Entries()
                                    .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted))
        {
            var parameters = actionParameters with { Entry = entry, Tenanted = tenanted, AggregateRoot = aggregateRootType };

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

    const string differentTenants                            = "Found entities from different tenants, or from a tenant different "+
                                                               "from the repository's tenant.";
    const string dddErrorMessageHasMoreThanOneAggregate      = "DDD error: Entity type {0} is marked with multiple IAggregate<TRoot> interfaces. "+
                                                               "An entity cannot be part of more than one aggregate.";
    const string dddErrorMessageViolationOfAggregateBoundary = "DDD error: Violation of aggregate consistency and transaction boundaries: "+
                                                               "encountered entities of IAggregate<{0}> and IAggregate<{1}>. "+
                                                               "Only entities and values from the same aggregate can participate in a transaction.";

    static ITenanted? CheckTenantBoundary(in ActionParameters p)
    {
        if (p.Entry.Entity is not ITenanted tenantEntity)
            return p.Tenanted;

        if (p.Tenanted is null)
            return tenantEntity;

        if (!p.Tenanted.SameAs(tenantEntity))
            throw new InvalidOperationException(differentTenants);

        return p.Tenanted;
    }

    static Type CheckAggregateBoundary(in ActionParameters p)
    {
        if (!p.Actions.HasFlag(AggregateActions.AggregateBoundary))
            return typeof(Unknown);

        var rootTypes = p.Entry
                            .Entity
                            .GetType()
                            .GetInterfaces()
                            .Where(i => i.IsGenericType
                                        && i.GetGenericTypeDefinition() == typeof(IAggregate<>))
                            .Select(i => i.GetGenericArguments()[0])
                            .ToList()
                            ;

        var rootType = rootTypes.Count is <= 1
                            ? rootTypes.SingleOrDefault(typeof(Unknown))
                            : throw new InvalidOperationException(string.Format(dddErrorMessageHasMoreThanOneAggregate, p.Entry.Entity.GetType().Name));

        if (p.AggregateRoot is null)
            return rootType;

        if (p.AggregateRoot == rootType)
            return p.AggregateRoot;

        if (!p.AllowedRoots.Contains(rootType))
            throw new InvalidOperationException(string.Format(dddErrorMessageViolationOfAggregateBoundary, p.AggregateRoot.Name, rootType.Name));

        return p.AggregateRoot;
    }

    static void AuditAdded(in ActionParameters p)
    {
        if (p.Actions.HasFlag(AggregateActions.Audit)
            && p.Entry.Entity is IAuditable auditable)
            auditable.AuditOnAdd(p.Now, p.Actor);
    }

    static void AuditUpdated(in ActionParameters p)
    {
        if (p.Actions.HasFlag(AggregateActions.Audit)
            && p.Entry.Entity is IAuditable auditable)
            auditable.AuditOnUpdate(p.Now, p.Actor);
    }

    static void AuditDeleted(in ActionParameters p)
    {
        if (p.Actions.HasFlag(AggregateActions.Audit)
            && p.Entry.Entity is ISoftDeletable deletable)
        {
            deletable.SoftDelete(p.Now, p.Actor);
            // do not delete the entity physically
            p.Entry.State = EntityState.Modified;
        }
    }

    static ValueTask CompleteAsync(in ActionParameters p)
        => p.Actions.HasFlag(AggregateActions.CustomComplete)
           && p.Entry.Entity is ICompletable completable
           && p.Entry.Context is IRepository repository
                ? completable.CompleteAsync(repository, p.Entry, p.CancellationToken)
                : ValueTask.CompletedTask;

    static ValueTask ValidateInvariantsAsync(in ActionParameters p)
        => p.Actions.HasFlag(AggregateActions.Invariants)
           && p.Entry.Entity is IValidatable validatable
           && p.Entry.Context is IRepository repository
                ? validatable.ValidateAsync(repository, p.CancellationToken)
                : ValueTask.CompletedTask;
}

