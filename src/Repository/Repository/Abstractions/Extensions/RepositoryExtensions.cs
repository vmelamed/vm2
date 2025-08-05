namespace vm2.Repository.Abstractions.Extensions;

using vm2.Repository.EfRepository.Models;

/// <summary>
/// Provides extension methods to <see cref="IRepository"/>.
/// </summary>
public static class RepositoryExtensions
{
    /// <summary>
    /// Finds an entity by the value(s) of its unique, possibly composite, primary store key.<br/>
    /// The method searches first in the change-tracker and if not found - in the underlying physical store.<br/>
    /// Since the search is done by the primary key(s), the operation is usually faster and cheaper than other queries.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="repository">The repository instance to search.</param>
    /// <param name="keyValues">
    /// The primary key(s) as a variable list of parameters - <see langword="params"/>.<br/>
    /// Note that the order of the key values in a composite key is important.<br/>
    /// After the last key value you can also pass a <see cref="CancellationToken"/> to allow for cancellation of the operation.
    /// </param>
    /// <returns><see cref="Task{T}"/> which contains the found instance or <see langword="null"/> if not found.</returns>
    /// <remarks>Note that the method is asynchronous.</remarks>
    public static ValueTask<T?> Find<T>(
        this IRepository repository,
        params object?[]? keyValues) where T : class
    {
        var cancellationToken = default(CancellationToken);

        if (keyValues?[^1] is CancellationToken ct)
        {
            cancellationToken = ct;
            keyValues = keyValues[..^1];
        }

        return repository.Find<T>(keyValues, cancellationToken);
    }

    /// <summary>
    /// Finds an entity with the same primary key property value(s) as the keys in the <see cref="IFindable.KeyValues"/> from the
    /// <paramref name="findable"/>. All other properties of <paramref name="findable"/> are ignored.<br/>
    /// The method searches first in the change-tracker and if not found - in the underlying physical store.<br/>
    /// Since the search is done by the primary key(s), the operation is usually faster and cheaper than other queries.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    /// <param name="repository">The repository instance to search.</param>
    /// <param name="findable">An object containing the key values used to locate the entity.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns><see cref="ValueTask{T}"/> which contains the found instance or <see langword="null"/> if not found.</returns>
    /// <remarks>Note that the method is asynchronous.</remarks>
    public static async ValueTask<T?> Find<T>(
        this IRepository repository,
        IFindable findable,
        CancellationToken cancellationToken = default) where T : class
    {
        await findable.ValidateFindable(repository, cancellationToken).ConfigureAwait(false);
        return await repository.Find<T>(findable.KeyValues, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds the <see langword="params"/> array <paramref name="range"/> of <typeparamref name="T"/> entities to the
    /// change-tracker in "Added" state. The method usually finishes synchronously.<br/>
    /// However, if a whole graph of entities is added and some of the primary keys are generated at the store, the ORM may need to<br/>
    /// add some of the entities to the store now, to obtain the primary keys and fix-up the values of some of the foreign keys<br/>
    /// of the dependent entities.<br/>
    /// The entities are added to the data store on <see cref="IRepository.Commit"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entities in the <paramref name="range"/>.</typeparam>
    /// <param name="repository">The repository instance.</param>
    /// <param name="range">The range or set of entities to add.</param>
    /// <returns><see cref="Task"/>.</returns>
    /// <remarks>Note that the method is asynchronous.</remarks>
    public static Task AddRange<T>(
        this IRepository repository,
        params T[] range)
        => range.Length > 0
            ? repository.AddRange(range.AsEnumerable(), default)
            : Task.CompletedTask;

    /// <summary>
    /// Attaches a <paramref name="range"/> or a set of entities to the internal object change-tracker in a "Modified" state.<br/>
    /// At the next <see cref="IRepository.Commit"/> the ORM will update the entities in "Modified" state in the physical store.
    /// <para>
    /// Use with caution! The use of this method implies that the code "knows" what is the current state of the entities better<br/>
    /// than the DB. In effect the DB stops being the ultimate source of truth. This is a strategy sometimes referred to as
    /// "client-wins" vs. "store-wins".
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the entities in the <paramref name="range"/>.</typeparam>
    /// <param name="repository">The repository instance.</param>
    /// <param name="range">The modified entities.</param>
    public static void UpdateRange<T>(
        this IRepository repository,
        params T[] range)
        => repository.UpdateRange(range.AsEnumerable());

    /// <summary>
    /// Attaches a range (or a set) of <typeparamref name="T"/> entities to the change-tracker in a "Unmodified" state.
    /// <para>
    /// Use with caution! The use of this method implies that the code "knows" what is the current state of the entity better<br/>
    /// than the repository or the DB. In effect the DB stops being the ultimate source of truth. This is a strategy sometimes<br/>
    /// known as "client-wins" (vs. "store-wins").
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the entities in the range.</typeparam>
    /// <param name="repository">The repository instance.</param>
    /// <param name="range">The entities to be attached.</param>
    public static void AttachRange<T>(
        this IRepository repository,
        params T[] range)
        => repository.AttachRange(range.AsEnumerable());

    /// <summary>
    /// Attaches a range (or a set) of <typeparamref name="T"/> entities to the change-tracker in a "Unmodified" state.
    /// <para>
    /// Use with caution! The use of this method implies that the code "knows" what is the current state of the entity better<br/>
    /// than the repository or the DB. In effect the DB stops being the ultimate source of truth. This is a strategy sometimes<br/>
    /// known as "client-wins" (vs. "store-wins").
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the entities in the range.</typeparam>
    /// <param name="repository">The repository instance.</param>
    /// <param name="range">The entities to be attached.</param>
    public static void RemoveRange<T>(
        this IRepository repository,
        params T[] range)
        => repository.RemoveRange(range.AsEnumerable());
}
