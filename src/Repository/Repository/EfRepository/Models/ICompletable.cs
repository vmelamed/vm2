namespace vm2.Repository.EfRepository.Models;
/// <summary>
/// Represents an entity that can be completed in some custom-defined way by the repository before committing changes.
/// </summary>
/// <remarks>
/// See also: <list type="bullet">
/// <item><seealso cref="IAuditable"/> for entities that are audited on adding and updating.</item>
/// <item><seealso cref="IRepository.Commit"/> for the context in which these interfaces are used.</item>
/// </list></remarks>
public interface ICompletable
{
    /// <summary>
    /// Entities that implement this interface can be completed by the repository just before <see cref="IRepository.Commit"/>.<br/>
    /// </summary>
    /// <param name="repository">The repository.</param>
    /// <param name="entry">The <see cref="EntityEntry"/> of the entity to complete: contains information about the state of the
    /// entity, its collections, navigations, original values, etc.</param>
    /// <param name="cancellationToken">Can be used by other objects or threads to receive notice of cancellation.</param>
    ValueTask Complete(IRepository repository, EntityEntry entry, CancellationToken cancellationToken = default);
}
