namespace vm2.Repository.EntityFramework.CommitInterceptor.PolicyRules;

/// <summary>
/// Provides functionality to audit entity changes, such as additions, updates, and deletions, by recording timestamps
/// and current actor information.
/// </summary>
/// <remarks>
/// This class is designed to work with entities that implement the <see cref="IAuditable"/> or <see cref="ISoftDeletable"/>
/// interfaces. It automatically sets audit information during entity state transitions, such as when an entity is added,
/// modified, or soft-deleted.
/// </remarks>
public class AuditEntities : IPolicyRule
{
    Func<DateTime> _clock;
    Func<string> _getCurrentActor;
    AsyncLocal<DateTime?> _now = new();
    AsyncLocal<string> _actor = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AuditEntities"/> class with optional delegates for retrieving the current time and actor.
    /// </summary>
    /// <param name="clock">
    /// A delegate that returns the current date and time. If <paramref name="clock"/> is <see langword="null"/>, the default
    /// implementation returns <see cref="DateTime.UtcNow"/>.
    /// </param>
    /// <param name="getCurrentActor">
    /// A delegate that returns the current actor as a string. If <paramref name="getCurrentActor"/> is <see langword="null"/>,
    /// the default implementation returns an empty string.</param>
    public AuditEntities(Func<DateTime>? clock, Func<string>? getCurrentActor)
    {
        _clock           = clock is not null ? clock : () => DateTime.UtcNow;
        _getCurrentActor = getCurrentActor is not null ? getCurrentActor : () => "";
    }

    /// <summary>
    /// Performs an action on the specified entity based on its state.
    /// </summary>
    /// <remarks>
    /// This method applies specific actions to entities implementing certain interfaces based on
    /// their state:
    /// <list type="bullet"><item>
    /// If the entity is in the <see cref="EntityState.Added"/> state and implements <see cref="IAuditable"/>, the
    /// <see cref="IAuditable.AuditOnAdd"/> method is called.
    /// </item><item>
    /// If the entity is in the <see cref="EntityState.Modified"/> state and implements <see cref="IAuditable"/>, the
    /// <see cref="IAuditable.AuditOnUpdate"/> method is called.
    /// </item><item>
    /// If the entity is in the <see cref="EntityState.Deleted"/> state and implements <see cref="ISoftDeletable"/>, the
    /// <see cref="ISoftDeletable.SoftDelete"/> method is called, and the entity's state is changed to
    /// <see cref="EntityState.Modified"/>.
    /// </item></list>
    /// The method ensures that the current timestamp and actor are set before performing these actions.
    /// </remarks>
    /// <param name="entry">The <see cref="EntityEntry"/> representing the entity and its state.</param>
    /// <param name="_">The <see cref="CancellationToken"/> is not used here.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    public ValueTask EntityActionAsync(
        EntityEntry entry,
        CancellationToken _ = default)
    {
        _now.Value   ??= _clock();
        _actor.Value ??= _getCurrentActor();

        var entity = entry.Entity;

        switch (entry.State)
        {
            case EntityState.Added when entity is IAuditable auditable:
                auditable.AuditOnAdd(_now.Value, _actor.Value);
                break;

            case EntityState.Modified when entity is IAuditable auditable:
                auditable.AuditOnUpdate(_now.Value, _actor.Value);
                break;

            case EntityState.Deleted when entity is ISoftDeletable softDeletable:
                softDeletable.SoftDelete(_now.Value, _actor.Value);
                entry.State = EntityState.Modified;
                break;
        }

        return ValueTask.CompletedTask;
    }
}
