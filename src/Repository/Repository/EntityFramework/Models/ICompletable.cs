namespace vm2.Repository.EntityFramework.Models;

/// <summary>
/// Represents an entity that can be completed in some custom-defined way by the repository before committing changes, similar <br/>
/// to the way the repository handles entities that implement <see cref="IAuditable"/>.
/// </summary>
/// <remarks>
/// See also: <list type="bullet">
/// <item><seealso cref="IAuditable"/> for entities that are audited on adding and updating.</item>
/// <item><seealso cref="IRepository.CommitAsync"/> for the context in which these interfaces are used.</item>
/// </list></remarks>
public interface ICompletable
{
    /// <summary>
    /// Entities that implement this interface can be completed by the repository just before <see cref="IRepository.CommitAsync"/>.<br/>
    /// </summary>
    /// <param name="repository">
    /// The repository.
    /// </param>
    /// <param name="entry">
    /// The <see cref="EntityEntry"/> of the entity to complete: contains information about the state of the entity, its collections, navigations, original values, etc.</param>
    /// <param name="now">
    /// The current date ands time according to some external (app. global?) clock provider.
    /// </param>
    /// <param name="userLogFragment">
    /// String representation of the actor who initiated the current UoW.
    /// </param>
    /// <param name="ct">Can be used by other objects or threads to receive notice of cancellation.</param>
    ValueTask CompleteAsync(IRepository? repository, EntityEntry entry, DateTime now, string userLogFragment, CancellationToken ct = default);
}
