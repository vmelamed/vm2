namespace vm2.Repository.EfRepository;

using vm2.Repository.EfRepository.Models;

/// <summary>
/// <see cref="DbContext"/> implementation of the interface <see cref="IRepository"/> - the repository pattern, providing methods
/// to add, remove, attach, update, and query with LINQ entities.
/// </summary>
/// <param name="options">
/// The options to be used by the <see cref="DbContext"/>. The options can be created with the <see cref="DbContextOptionsBuilder"/>.
/// </param>
/// <remarks>
/// Note that the <see cref="IRepository"/> is implemented explicitly by the <see cref="DbContextRepository"/> class. Therefore, in
/// order to access the interface methods, your <see cref="DbContext"/> that inherits from <see cref="DbContextRepository"/>, can be
/// passed in as <see cref="IRepository"/> to the clients. Alternatively, the clients can cast your <see cref="DbContext"/>
/// to <see cref="IRepository"/>. The best approach would be to use dependency injection that resolves <see cref="IRepository"/>
/// to the concrete <see cref="DbContextRepository"/> descendant.<para/>
/// <see cref="IRepository"/> does not claim, nor tries to cover the full functionality of <see cref="DbContext"/>. To access
/// the full functionality of <see cref="DbContext"/>, you can use the extension method <see cref="EfRepositoryExtensions.DbContext"/>.
/// </remarks>
public class DbContextRepository(DbContextOptions options) : DbContext(options), IRepository
{
    IRepository ThisRepo => this;

    /// <summary>
    /// Represents an abstract collection of domain objects (entities) of type <typeparamref name="T"/>. Since the entity set is
    /// represented as <see cref="IQueryable{T}"/>, the <c>IRepository</c>'s clients can declaratively construct LINQ queries.
    /// Entities can be added to the set (<see cref="IRepository.Add"/>), and removed from the set (<see cref="IRepository.Remove"/>).
    /// </summary>
    /// <typeparam name="T">The type of the entity in the set.</typeparam>
    /// <returns><see cref="IQueryable{T}"/>.</returns>
    /// <remarks>
    /// Note that the returned <see cref="IQueryable"/> is the Entity Framework's <see cref="DbSet{TEntity}"/>. Some methods may
    /// be executed first on the internal in-memory entity change-tracker and then, if needed, on the underlying physical store,
    /// e.g. <see cref="DbSet{TEntity}.FindAsync(object[])"/> and <see cref="DbSet{TEntity}.AddAsync"/>.
    /// </remarks>
    IQueryable<T> IRepository.Set<T>() => base.Set<T>();

    /// <summary>
    /// Finds an entity by the value(s) of its unique, possibly composite, primary store key. The method searches first in the<br/>
    /// change-tracker and if not found, searches for it in the underlying physical store. Since the search is done by<br/>
    /// the primary key(s), the operation is usually faster and cheaper than other queries.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="keyValues">
    /// The primary key(s). Note that the order of the key values in a composite key is important.<br/>
    /// Note that the order of the key values in a composite key is important.
    /// </param>
    /// <param name="cancellationToken">Can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns><see cref="Task{T}"/> which contains the found instance or <see langword="null"/> if not found.</returns>
    /// <remarks>Note that the method is asynchronous.</remarks>
    public ValueTask<T?> Find<T>(
        IEnumerable<object?>? keyValues,
        CancellationToken cancellationToken = default) where T : class
        => ThisRepo.Set<T>().Find(keyValues, cancellationToken);

    /// <summary>
    /// Adds the <paramref name="entity"/> to the change-tracker (in memory) in <see cref="EntityState.Added"/> state.
    /// The method usually finishes synchronously. However, if a whole graph of entities is added and some of the primary keys
    /// are generated at the store, EF may need to add some of the entities to the store now, to obtain the primary keys and
    /// fix-up the values of some of the foreign keys of the dependent entities.  The entity is added to the data store on
    /// <see cref="IRepository.Commit"/> (<see cref="DbContext.SaveChangesAsync(CancellationToken)"/>).
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity instance to add.</param>
    /// <param name="cancellationToken">Can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns><see cref="ValueTask{T}"/> which contains the added instance.</returns>
    /// <remarks>Note that the method is asynchronous.</remarks>
    async ValueTask<T> IRepository.Add<T>(
        T entity,
        CancellationToken cancellationToken) where T : class
        => await ThisRepo.Set<T>().Add(entity, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Attaches the specified entity to the internal object change-tracker in a <see cref="EntityState.Modified"/> state.
    /// At the next <see cref="IRepository.Commit"/> EF will update the entities in <see cref="EntityState.Modified"/> state in
    /// the DB.
    /// <para>
    /// Use with caution! The use of this method implies that the code "knows" what is the current state of the entity better
    /// than the DB. In effect the DB stops being the ultimate source of truth. This is a strategy sometimes known as
    /// "client-wins" vs. "store-wins".
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The modified entity instance.</param>
    /// <returns>The added entity.</returns>
    T IRepository.Update<T>(T entity)
        => ThisRepo.Set<T>().Update(entity);

    /// <summary>
    /// If the <paramref name="entity"/> is not in the tracker yet, the method will add it in <see cref="EntityState.Deleted"/> state.<br/>
    /// If it is already there, the tracker will make sure that the entity's state is <see cref="EntityState.Deleted"/>.<br/>
    /// At the next <see cref="IRepository.Commit"/> the entity will be deleted from the DB as well.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to be deleted.</param>
    /// <returns>The entity to be removed.</returns>
    /// <remarks>
    /// Note that the entity doesn't need to be loaded in the change-tracker with a previous query. If the entity is not in the<br/>
    /// tracker, the method will add it as is in "Deleted" state. The only important thing for the entity is that it contains<br/>
    /// valid keys in the proper order. This warrants the following pattern:
    /// <code><![CDATA[
    /// async Task Delete(Id entityId)
    /// {
    ///     ...
    ///     Entity entity = new() { Id = entityId };
    ///
    ///     // NO trip to the DB:
    ///     _repository.Remove(entity);
    ///     ...
    ///     // SINGLE trip to the DB:
    ///     await _repository.Commit();
    /// }
    /// ]]></code>instead of:<code><![CDATA[
    /// async Task Delete(Id entityId)
    /// {
    ///     ...
    ///     // FIRST trip to the DB!
    ///     Entity entity = await _repository.Find(entityId);
    ///     _repository.Remove(entity);
    ///     ...
    ///     // SECOND trip to the DB
    ///     await _repository.Commit();
    /// }
    /// ]]></code>
    /// </remarks>
    T IRepository.Remove<T>(T entity)
        => ThisRepo.Set<T>().Remove(entity);

    /// <summary>
    /// Attaches the specified entity to the change-tracker in a <see cref="EntityState.Unchanged"/> state. If any changes are done to the entity, its<br/>
    /// state will transition to <see cref="EntityState.Modified"/>. At the next <see cref="IRepository.Commit"/> EF will update the entities in<br/>
    /// <see cref="EntityState.Modified"/> state in the DB and will ignore the <see cref="EntityState.Unchanged"/> entities.<br/>
    /// <para>
    /// Use with caution! The use of this method implies that the code "knows" what is the current state of the entity better<br/>
    /// than the repository or the DB. In effect the DB stops being the ultimate source of truth. This is a strategy sometimes<br/>
    /// known as "client-wins" (vs. "store-wins").
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="entity">The entity to be attached.</param>
    /// <returns>The entity.</returns>
    T IRepository.Attach<T>(T entity)
        => ThisRepo.Set<T>().Attach(entity);

    /// <summary>
    /// Commits the added, modified and deleted entities in the change-tracker to the physical store invoking the respective <br/>
    /// back-end actions. For some DB-s  the action is a single transaction, for others it might be a sequence of transactions.
    /// </summary>
    /// <param name="cancellationToken">Can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns><see cref="Task"/></returns>
    /// <remarks>Note that the method is asynchronous.</remarks>
    public Task<int> Commit(CancellationToken cancellationToken = default)
        => SaveChangesAsync(cancellationToken);

    /// <inheritdoc />
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await CompleteAndValidate(cancellationToken).ConfigureAwait(false);
        return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

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

        // Complete all entities that implement ICompletable
        foreach (var entry in ChangeTracker.Entries())
            await (entry.State switch {
                EntityState.Added => CompleteAndValidateAddedEntity(entry, now, actor, cancellationToken),
                EntityState.Modified => CompleteAndValidateUpdatedEntity(entry, now, actor, cancellationToken),
                EntityState.Deleted => CompleteAndValidateDeletedEntity(entry, now, actor, cancellationToken),
                _ => ValueTask.CompletedTask, // Do nothing for Unchanged or Detached
            });
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

        if (entity is IValidatable validatable)
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

        if (entity is IValidatable validatableUpdated)
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
