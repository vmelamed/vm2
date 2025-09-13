namespace vm2.Repository.EntityFramework.CommitInterceptor.PolicyRules;

/// <summary>
/// Provides functionality to complete entities that implement the <see cref="ICompletable"/> interface by invoking their
/// completion logic with the current timestamp and actor information.
/// </summary>
public class CompleteEntities : IPolicyRule
{
    Func<DateTime> _clock;
    Func<string> _getCurrentActor;
    AsyncLocal<DateTime?> _now = new();
    AsyncLocal<string> _actor = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="CompleteEntities"/> class with optional delegates for retrieving the
    /// current time and actor.
    /// </summary>
    /// <param name="clock">
    /// A delegate that returns the current date and time. If <paramref name="clock"/> is <see langword="null"/>, the default
    /// implementation returns <see cref="DateTime.UtcNow"/>.
    /// </param>
    /// <param name="getCurrentActor">
    /// A delegate that returns the current actor as a string. If <paramref name="getCurrentActor"/> is <see langword="null"/>,
    /// the default implementation returns an empty string.
    /// </param>
    public CompleteEntities(Func<DateTime>? clock, Func<string>? getCurrentActor)
    {
        _clock           = clock is not null ? clock : () => DateTime.UtcNow;
        _getCurrentActor = getCurrentActor is not null ? getCurrentActor : () => "";
    }

    /// <summary>
    /// Performs an asynchronous action on the specified entity entry.
    /// </summary>
    /// <remarks>
    /// This method ensures that the current timestamp and actor are initialized before performing the action.  If the entity
    /// implements <see cref="ICompletable"/>, its <c>CompleteAsync</c> method is called with the  necessary parameters,
    /// including the repository context, the entity entry, the current timestamp, and the actor.
    /// </remarks>
    /// <param name="entry">The <see cref="EntityEntry"/> representing the entity and its state in the context.</param>
    /// <param name="ct">An optional <see cref="CancellationToken"/> to observe while waiting for the operation to complete.</param>
    /// <returns>
    /// A <see cref="ValueTask"/> that represents the asynchronous operation. If the entity implements  <see cref="ICompletable"/>,
    /// the task completes after invoking its <c>CompleteAsync</c> method; otherwise,  the task completes immediately.
    /// </returns>
    public ValueTask EntityActionAsync(
        EntityEntry entry,
        CancellationToken ct = default)
    {
        if (entry.Entity is not ICompletable completable)
            return ValueTask.CompletedTask;

        if (_now.Value is null)
        {
            _now.Value   = _clock();
            _actor.Value = _getCurrentActor();
        }

        Debug.Assert(_actor.Value is not null);

        return completable.CompleteAsync(entry.Context as IRepository, entry, _now.Value.Value, _actor.Value, ct);
    }
}
