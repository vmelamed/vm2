namespace vm2.Repository.EntityFramework.CommitInterceptor;

/// <summary>
/// Defines an action to be performed on an entity before a commit operation.
/// </summary>
public interface IPolicyRule
{
    /// <summary>
    /// Performs an action on the specified entity entry.
    /// </summary>
    /// <param name="entry">The <see cref="EntityEntry"/> representing the entity to act upon. Cannot be null.</param>
    /// <param name="ct">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    ValueTask EntityActionAsync(EntityEntry entry, CancellationToken ct = default);
}
