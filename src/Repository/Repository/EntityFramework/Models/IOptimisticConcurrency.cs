namespace vm2.Repository.EntityFramework.Models;

/// <summary>
/// Marker interface for entities that support optimistic concurrency.
/// </summary>
public interface IOptimisticConcurrency<TTag>
{
    /// <summary>
    /// The property used for optimistic concurrency control.
    /// </summary>
    TTag ETag { get; set; }

    /// <summary>
    /// Updates the concurrency token for the specified entity entry if the entity implements <see cref="IOptimisticConcurrency{TTag}"/>.
    /// </summary>
    /// <remarks>If the entity associated with the <paramref name="entry"/> implements <see
    /// cref="IOptimisticConcurrency{TTag}"/>,  its <c>ETag</c> property is updated with a new unique identifier. This is
    /// typically used to support optimistic concurrency control.</remarks>
    /// <param name="entry">The entity entry to update. Must not be <c>null</c>.</param>
    /// <param name="newValue">The new value to set for the concurrency token.</param>
    static void UpdateConcurrency(EntityEntry entry, TTag newValue)
    {
        if (entry.Entity is IOptimisticConcurrency<TTag> oc)
            oc.ETag = newValue;
    }
}
