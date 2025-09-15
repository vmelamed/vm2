namespace vm2.Repository.EntityFramework.CommitInterceptor;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Provides an implementation of <see cref="SaveChangesInterceptor"/> to intercept the behavior before
/// <see cref="DbContext.SaveChangesAsync(CancellationToken)"/>.
/// </summary>
public class CommitInterceptor(in CommitInterceptorConfiguration configuration) : SaveChangesInterceptor
{
    /// <summary>
    /// Gets the collection of commit actions to be performed.
    /// </summary>
    public IEnumerable<IPolicyRule> CommitActions { get; } = [.. configuration.CommitActions];

    /// <summary>
    /// Intercepts the asynchronous saving changes operation in the database context.
    /// </summary>
    /// <param name="eventData">The event data associated with the save operation, including the <see cref="DbContext"/> instance.</param>
    /// <param name="result">The current <see cref="InterceptionResult{TResult}"/> that determines whether the operation should proceed, be
    /// suppressed, or be replaced.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>
    /// A <see cref="ValueTask{TResult}"/> containing an <see cref="InterceptionResult{TResult}"/> that indicates the result of
    /// the interception.
    /// </returns>
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        // if no actions - return
        if (!CommitActions.Any())
            return result;

        var changeTracker = eventData.Context?.ChangeTracker
                                ?? throw new InvalidOperationException("The context in eventData is null or is not DbContext.");

        changeTracker.DetectChanges();

        // if nothing changed, do nothing
        if (changeTracker.Entries().All(e => e.State is EntityState.Unchanged
                                                     or EntityState.Detached))
            return result;

        foreach (var entry in changeTracker.Entries().Where(e => e.State is EntityState.Added
                                                                         or EntityState.Modified
                                                                         or EntityState.Deleted))
            foreach (var action in CommitActions)
            {
                await action.EntityActionAsync(entry, ct);
                ct.ThrowIfCancellationRequested();
            }

        return result;
    }
}
