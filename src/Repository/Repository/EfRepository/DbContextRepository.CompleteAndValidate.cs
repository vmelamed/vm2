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
    /// <param name="ct">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    protected virtual async ValueTask CompleteAndValidateAsync(CancellationToken ct = default)
    {
        ChangeTracker.DetectChanges();

        if (DddBoundaryChecks.HasFlag(DddBoundaryChecks.AggregateBoundary))
            DddAggregateBoundaryChecking.CheckAggregateBoundary(ChangeTracker);

        if (DddBoundaryChecks.HasFlag(DddBoundaryChecks.Validation))
            await DddAggregateBoundaryChecking.CheckAggregateInvariantsAsync(ChangeTracker, ct).ConfigureAwait(false);

        string actor = "";    // TODO: replace with actual actor, e.g. from some call context or user token
        var now = DateTime.UtcNow;

        // ValidateAsync that all modified entities belong to the same aggregate and then complete the entities that implement ICompletable
        foreach (var entry in ChangeTracker
                                .Entries()
                                .Where(e => e.State is not (EntityState.Unchanged or EntityState.Detached)))
            await (
                entry.State switch {
                    EntityState.Added => CompleteAndValidateAddedEntityAsync(entry, now, actor, ct),
                    EntityState.Modified => CompleteAndValidateUpdatedEntityAsync(entry, now, actor, ct),
                    EntityState.Deleted => CompleteAndValidateDeletedEntityAsync(entry, now, actor, ct),
                    _ => ValueTask.CompletedTask, // Do nothing for Unchanged or Detached
                }
            ).ConfigureAwait(false);
    }

    /// <summary>
    /// Completes and validates an entity that has been added to the context. The method can be overridden in derived contexts
    /// to customize the actions performed on the entity after it has been added with <see cref="IRepository.AddAsync"/>.
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
    /// <param name="ct">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    protected virtual async ValueTask CompleteAndValidateAddedEntityAsync(
        EntityEntry entry,
        DateTime now,
        string actor,
        CancellationToken ct)
    {
        var entity = entry.Entity;

        if (entity is IAuditable auditableAdded)
            auditableAdded.AuditOnAdd(now, actor);

        if (entity is ICompletable completable)
            await completable.CompleteAsync(this, entry, ct);
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
    /// <param name="ct">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    protected virtual async ValueTask CompleteAndValidateUpdatedEntityAsync(
        EntityEntry entry,
        DateTime now,
        string actor,
        CancellationToken ct)
    {
        var entity = entry.Entity;

        if (entity is IAuditable auditable)
            auditable.AuditOnUpdate(now, actor);

        if (entity is ICompletable completable)
            await completable.CompleteAsync(this, entry, ct);
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
    /// <param name="ct">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation.</returns>
    protected virtual async ValueTask CompleteAndValidateDeletedEntityAsync(
        EntityEntry entry,
        DateTime now,
        string actor,
        CancellationToken ct)
    {
        var entity = entry.Entity;

        // Delete other, logically related entities as well?
        if (entity is ICompletable completable)
            await completable.CompleteAsync(this, entry, ct);

        if (entity is ISoftDeletable deletable)
        {
            deletable.SoftDelete(now, actor);
            // Change state to Modified to avoid actual deletion:
            entry.State = EntityState.Modified;
        }

        return;
    }
}
