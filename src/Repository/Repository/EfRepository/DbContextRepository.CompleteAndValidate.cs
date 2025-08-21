namespace vm2.Repository.EfRepository;

public partial class DbContextRepository : DbContext, IRepository
{
    /// <summary>
    /// Completes and validates all tracked entities based on their state.
    /// </summary>
    /// <remarks>This method processes entities in the change tracker, performing completion and validation actions depending on
    /// whether the entity is added, modified, or deleted. It is designed to be overridden in derived classes to customize the
    /// completion and validation logic.
    /// </remarks>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    protected virtual async ValueTask CompleteAndValidate(CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();

        string actor = "";    // TODO: replace with actual actor, e.g. from some call context or user token
        var now = DateTime.UtcNow;
        Type? typeOfRoot = null;

        // Validate that all modified entities belong to the same aggregate and then complete the entities that implement ICompletable
        foreach (var entry in ChangeTracker
                                .Entries()
                                .Where(e => e.State is not EntityState.Unchanged or EntityState.Detached))
        {
            if (DddBoundaryChecks.HasFlag(DddBoundaryChecks.AggregateBoundary))
            {
                var genInterfaces = entry.Entity
                                    .GetType()
                                    .GetInterfaces()
                                    .Where(i => i.IsGenericType
                                                && i.GetGenericTypeDefinition() == typeof(IAggregate<>))
                                    .ToList()
                                        ;

                if (genInterfaces.Count > 1)
                    throw new InvalidOperationException($"DDD error: Entity {entry.Entity.GetType().Name} is marked with multiple IAggregate<TRoot> interfaces. An entity cannot be part of more than one aggregate.");
                if (genInterfaces.Count() <= 0)
                    throw new InvalidOperationException($"DDD error: Entity {entry.Entity.GetType().Name} is not marked with IAggregate<TRoot>.");

                var currentEntityRoot = genInterfaces[0].GenericTypeArguments[0];

                if (typeOfRoot is null)
                    typeOfRoot = currentEntityRoot;
                else
                if (typeOfRoot != currentEntityRoot)
                    throw new InvalidOperationException(
                        $"DDD error: Violation of aggregate consistency and transaction boundaries: encountered entities of IAggregate<{typeOfRoot.Name}> and IAggregate<{currentEntityRoot.Name}>.");
            }

            await (entry.State switch {
                EntityState.Added => CompleteAndValidateAddedEntity(entry, now, actor, cancellationToken),
                EntityState.Modified => CompleteAndValidateUpdatedEntity(entry, now, actor, cancellationToken),
                EntityState.Deleted => CompleteAndValidateDeletedEntity(entry, now, actor, cancellationToken),
                _ => ValueTask.CompletedTask, // Do nothing for Unchanged or Detached
            });
        }
    }

    /// <summary>
    /// Completes and validates an entity that has been added to the context. The method can be overridden in derived contexts
    /// to customize the actions performed on the entity after it has been added with <see cref="IRepository.Add"/>.
    /// </summary>
    /// <remarks>This method performs several operations on the entity if it implements specific interfaces:
    /// <list type="bullet">
    /// <item>If the entity implements <see cref="IAuditable"/>, it applies auditing information using the provided
    /// <paramref name="now"/> and <paramref name="actor"/>.
    /// </item><item>
    /// If the entity implements <see cref="ICompletable"/>, it completes the entity.
    /// </item><item>
    /// If the entity implements <see cref="IValidatable"/>, it validates the entity.
    /// </item>
    /// </list></remarks>
    /// <param name="entry">The <see cref="EntityEntry"/> representing the entity to be processed.</param>
    /// <param name="now">The current date and time, used for auditing purposes.</param>
    /// <param name="actor">The identifier of the actor performing the operation, used for auditing purposes.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    protected virtual async ValueTask CompleteAndValidateAddedEntity(
        EntityEntry entry,
        DateTime now,
        string actor,
        CancellationToken cancellationToken)
    {
        var entity = entry.Entity;

        if (entity is IAuditable auditableAdded)
            auditableAdded.AuditOnAdd(now, actor);

        if (entity is ICompletable completable)
            await completable.Complete(this, entry, cancellationToken).ConfigureAwait(false);

        if (DddBoundaryChecks.HasFlag(DddBoundaryChecks.Validation)
            && entity is IValidatable validatable)
            await validatable.Validate(this, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Completes and validates the updated entity by applying auditing, completion, and validation processes. The method can
    /// be overridden in derived contexts to customize the actions performed on the entity after it has been modified.
    /// </summary>
    /// <remarks>This method performs several operations on the entity if it implements specific interfaces:
    /// <list type="bullet">
    /// <item>If the entity implements <see cref="IAuditable"/>, it applies auditing information using the provided
    /// <paramref name="now"/> and <paramref name="actor"/>.
    /// </item><item>
    /// If the entity implements <see cref="ICompletable"/>, it completes the entity asynchronously.
    /// </item><item>
    /// If the entity implements <see cref="IValidatable"/>, it validates the entity asynchronously.
    /// </item></list></remarks>
    /// <param name="entry">The <see cref="EntityEntry"/> representing the entity to be processed.</param>
    /// <param name="now">The current date and time used for auditing purposes.</param>
    /// <param name="actor">The identifier of the actor performing the update, used for auditing.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    protected virtual async ValueTask CompleteAndValidateUpdatedEntity(
        EntityEntry entry,
        DateTime now,
        string actor,
        CancellationToken cancellationToken)
    {
        var entity = entry.Entity;

        if (entity is IAuditable auditable)
            auditable.AuditOnUpdate(now, actor);

        if (entity is ICompletable completable)
            await completable.Complete(this, entry, cancellationToken).ConfigureAwait(false);

        if (DddBoundaryChecks.HasFlag(DddBoundaryChecks.Validation)
            && entity is IValidatable validatableUpdated)
            await validatableUpdated.Validate(this, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Completes the soft deletion process for an entity and validates its state.
    /// </summary>
    /// <remarks>This method marks the entity as soft deleted by updating its state to <see
    /// cref="EntityState.Deleted"/>. It can be overridden in derived classes to customize the soft deletion behavior.
    /// </remarks>
    /// <param name="entry">The <see cref="EntityEntry"/> representing the entity to be soft deleted.</param>
    /// <param name="now">The current date and time, used to timestamp the deletion.</param>
    /// <param name="actor">The identifier of the user or process performing the deletion.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    protected virtual async ValueTask CompleteAndValidateDeletedEntity(
        EntityEntry entry,
        DateTime now,
        string actor,
        CancellationToken cancellationToken)
    {
        var entity = entry.Entity;

        // Delete other, logically related entities as well?
        if (entity is ICompletable completable)
            await completable.Complete(this, entry, cancellationToken).ConfigureAwait(false);

        if (entity is ISoftDeletable deletable)
        {
            deletable.SoftDelete(now, actor);
            // Change state to Modified to avoid actual deletion:
            entry.State = EntityState.Modified;
        }

        return;
    }
}
