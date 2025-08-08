namespace vm2.Repository.EfRepository.Models;
using vm2.Repository.Abstractions;

/// <summary>
/// Represents an entity that supports soft deletion functionality.
/// </summary>
/// <remarks>
/// Implementing this interface indicates that the entity can be marked as deleted without being permanently removed from the
/// data store. This is typically used to allow for recovery of deleted entities or to maintain historical data.<para/>
/// See also: <list type="bullet">
/// <item><seealso cref="IAuditable"/> for entities that are audited on adding and updating.</item>
/// <item><seealso cref="ICompletable"/> for entities that can be completed by the repository before committing changes.</item>
/// <item><seealso cref="IRepository.Commit"/> for the context in which these interfaces are used.</item>
/// </list></remarks>
public interface ISoftDeletable
{
    /// <summary>
    /// Represents the maximum allowed length for an actor's name - deleter.
    /// </summary>
    public const int MaxActorNameLength = IAuditable.MaxActorNameLength;

    /// <summary>
    /// Gets or sets the timestamp indicating when the entity was deleted.
    /// </summary>
    DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the actor who deleted the entity.
    /// </summary>
    string DeletedBy { get; set; }

    /// <summary>
    /// Gets an expression that determines whether an <see cref="ISoftDeletable"/> entity is marked as deleted. The expression
    /// can be used in LINQ queries to filter out deleted entities from results.
    /// </summary>
    static virtual Expression<Func<ISoftDeletable, bool>> IsDeletedExpression => entity => entity.DeletedAt.HasValue;

    /// <summary>
    /// Gets a value indicating whether the entity is marked as deleted.
    /// </summary>
    bool IsDeleted => DeletedAt.HasValue;

    /// <summary>
    /// Marks the entity as deleted by setting the deletion timestamp and actor.
    /// </summary>
    /// <param name="now">The timestamp to record as the deletion time. If <see langword="null"/>, the current UTC time is used.</param>
    /// <param name="actor">The identifier of the actor performing the deletion. Can be an empty string if not specified.</param>
    void SoftDelete(
        DateTime? now = default,
        string actor = "")
    {
        DeletedAt = now ?? DateTime.UtcNow;
        DeletedBy = actor;
    }

    /// <summary>
    /// Restores a previously saft-deleted item by clearing its deletion metadata.
    /// </summary>
    /// <param name="now">The current date and time, used for logging or auditing purposes. This parameter is optional.</param>
    /// <param name="actor">The identifier of the user or process performing the undelete operation. This parameter is optional.</param>
    void Undelete(
        DateTime? now = default,
        string actor = "")
    {
        DeletedAt = null;
        DeletedBy = string.Empty;

        if (this is IAuditable a)
            a.AuditOnUpdate(now ?? DateTime.UtcNow, actor);
    }
}
