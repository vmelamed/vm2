namespace vm2.Repository.EfRepository;

/// <summary>
/// Provides extension methods to <see cref="IQueryable{T}"/> that represents a &quot;domain object collection&quot;. It is
/// assumed and enforced that the <see cref="IQueryable{T}"/> is backed by Entity Framework Core <see cref="Microsoft.EntityFrameworkCore.DbSet{T}"/>,
/// otherwise the methods throw <see cref="NotSupportedException"/>.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Casts an <see cref="IQueryable{T}"/> to EF's <see cref="Microsoft.EntityFrameworkCore.DbSet{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">
    /// Thrown if the <paramref name="queryable"/> is not implemented by <see cref="Microsoft.EntityFrameworkCore.DbSet{TEntity}"/>.
    /// </exception>
    public static DbSet<T> DbSet<T>(this IQueryable<T> queryable)
        where T : class
        => queryable as DbSet<T>
                ?? throw new NotSupportedException("The method is not supported if IQueryable<TId>() is not implemented by Microsoft.EntityFrameworkCore.DbSet<T>.");

    /// <summary>
    /// Adds the specified entity to the underlying object collection represented by the given queryable.<br/>
    /// For Entity Framework Core, this means that the entity is added to the change-tracker and marked as <see cref="EntityState.Added"/>.<br/>
    /// The method usually finishes synchronously. However, if a whole graph of entities is added and some of the primary keys<br/>
    /// are generated at the store, the ORM may need to add some of the entities to the store now, to obtain the primary<br/>
    /// keys and fix-up the values of some of the foreign keys of the dependent entities.<br/>
    /// The entity is added to the physical store on <see cref="IRepository.CommitAsync"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity to add.</typeparam>
    /// <param name="queryable">The queryable source to which the entity will be added.</param>
    /// <param name="entity">The entity to add to the object collection.</param>
    /// <param name="ct">Can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A <see cref="ValueTask{T}"/> that represents the asynchronous operation. The result contains the added entity.</returns>
    /// <exception cref="NotSupportedException">Thrown when the queryable source is not a <see cref="Microsoft.EntityFrameworkCore.DbSet{T}"/>.</exception>
    public static async ValueTask<T> AddAsync<T>(this IQueryable<T> queryable, T entity, CancellationToken ct = default)
        where T : class
        => (await queryable.DbSet().AddAsync(entity, ct).ConfigureAwait(false)).Entity;

    /// <summary>
    /// Adds the specified entity to the underlying object collection represented by the given queryable.<br/>
    /// For Entity Framework Core, this means the entity is marked as <see cref="EntityState.Unchanged"/>.<br/>
    /// If subsequently the entity is modified, its state will change to <see cref="EntityState.Modified"/> and the new<br/>
    /// state of the entity will be stored in the data store on <see cref="IRepository.CommitAsync"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity to attach.</typeparam>
    /// <param name="queryable">The queryable source to which the entity will be added.</param>
    /// <param name="entity">The entity to attach to the context.</param>
    /// <returns>The attached entity.</returns>
    /// <exception cref="NotSupportedException">Thrown when the queryable source is not a <see cref="Microsoft.EntityFrameworkCore.DbSet{T}"/>.</exception>
    public static T Attach<T>(this IQueryable<T> queryable, T entity) where T : class
        => queryable.DbSet().Attach(entity).Entity;

    /// <summary>
    /// Finds an entity by the value of its unique, possibly composite, primary store key(s). The method searches first in the<br/>
    /// change-tracker and if not found - in the underlying physical store.<br/>
    /// Since the search is done by the primary key(s), the operation is usually faster and cheaper than other queries.
    /// </summary>
    /// <typeparam name="T">The type of the entity to find.</typeparam>
    /// <param name="queryable">The queryable source where the entity will be sought.</param>
    /// <param name="keyValues">
    /// An <see cref="IEnumerable{Object}"/> containing the values of the primary key for the entity to be found.
    /// </param>
    /// <param name="ct">Can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns><see cref="ValueTask{T}"/> which contains the found instance or <see langword="null"/> if not found.</returns>
    /// <exception cref="NotSupportedException">Thrown when the queryable source is not a <see cref="Microsoft.EntityFrameworkCore.DbSet{T}"/>.</exception>
    public static ValueTask<T?> FindAsync<T>(this IQueryable<T> queryable, IEnumerable<object?>? keyValues, CancellationToken ct = default)
        where T : class
        => queryable.DbSet().FindAsync(keyValues?.ToArray(), ct);

    /// <summary>
    /// Finds an entity by the value of its unique, possibly composite, primary store key(s). The method searches first in the<br/>
    /// change-tracker and if not found - in the underlying physical store.<br/>
    /// Since the search is done by the primary key(s), the operation is usually faster and cheaper than other queries.
    /// </summary>
    /// <typeparam name="T">The type of the entity to find.</typeparam>
    /// <param name="queryable">The queryable source where the entity will be sought.</param>
    /// <param name="keyValues">
    /// The primary key(s) as a variable list of parameters - <see langword="params"/>.<br/>
    /// Note that the order of the key values in a composite key is important.<br/>
    /// After the last key value you can also pass a <see cref="CancellationToken"/> to allow for cancellation of the operation.
    /// </param>
    /// <returns><see cref="ValueTask{T}"/> which contains the found instance or <see langword="null"/> if not found.</returns>
    /// <exception cref="NotSupportedException">Thrown when the queryable source is not a <see cref="Microsoft.EntityFrameworkCore.DbSet{T}"/>.</exception>
    public static ValueTask<T?> FindAsync<T>(this IQueryable<T> queryable, params object?[]? keyValues)
        where T : class
        => keyValues?[^1] is CancellationToken ct
            ? queryable.DbSet().FindAsync(keyValues[..^1], ct)
            : queryable.DbSet().FindAsync(keyValues);

    /// <summary>
    /// Finds an entity with the same primary key property value(s) as the keys in the <see cref="IFindable.KeyValues"/> from the
    /// <paramref name="findable"/>. All other properties of <paramref name="findable"/> are ignored.<br/>
    /// The method searches first in the change-tracker and if not found - in the underlying physical store.<br/>
    /// Since the search is done by the primary key(s), the operation is usually faster and cheaper than other queries.
    /// </summary>
    /// <typeparam name="T">The type of the entity to find.</typeparam>
    /// <param name="queryable">The queryable source to which the entity will be added. Must be a <see cref="Microsoft.EntityFrameworkCore.DbSet{T}"/>.</param>
    /// <param name="findable">An object containing the key values used to locate the entity.</param>
    /// <param name="ct">Can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns><see cref="ValueTask{T}"/> which contains the found instance or <see langword="null"/> if not found.</returns>
    /// <exception cref="NotSupportedException">Thrown when the queryable source is not a <see cref="Microsoft.EntityFrameworkCore.DbSet{T}"/>.</exception>
    public static async ValueTask<T?> FindAsync<T>(this IQueryable<T> queryable, IFindable findable, CancellationToken ct = default)
        where T : class, IFindable
    {
        await findable.ValidateFindableAsync(null, ct).ConfigureAwait(false);
        ct.ThrowIfCancellationRequested();
        return await queryable.DbSet().FindAsync(findable.KeyValues?.ToArray(), ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Adds the specified entity to the object collection underlying the given queryable in <see cref="EntityState.Deleted"/> state<br/>
    /// or if it is already being tracked in the change-tracker - changes the state of the entity to <see cref="EntityState.Deleted"/>.
    /// </summary>
    /// <remarks>
    /// Note that the entity doesn't need to be loaded in the change-tracker with a previous query. If the entity is not in the<br/>
    /// tracker, the method will add it as is in "Deleted" state. The only important thing for the entity is that it contains<br/>
    /// valid keys in the proper order. This warrants the following pattern:
    /// <code><![CDATA[
    /// async Task Delete(Id entityId)
    /// {
    ///     ...
    ///     Entity entity = new() { Id = entityId };
    ///     // NO trip to the DB:
    ///     _repository.Set<Entity>().Remove(entity);
    ///     ...
    ///     // the ONLY trip to the DB:
    ///     await _repository.CommitAsync();
    /// }
    /// ]]></code>instead of:<code><![CDATA[
    /// async Task Delete(Id entityId)
    /// {
    ///     ...
    ///     // FIRST trip to the DB!
    ///     Entity entity = await _repository.Set<Entity>().FindAsync(entityId);
    ///     _repository.Set<Entity>().Remove(entity);
    ///     ...
    ///     // SECOND trip to the DB
    ///     await _repository.CommitAsync();
    /// }
    /// ]]></code>
    /// </remarks>
    /// <typeparam name="T">The type of the entity to be removed.</typeparam>
    /// <param name="queryable">The queryable source to which the entity will be added. Must be a <see cref="Microsoft.EntityFrameworkCore.DbSet{T}"/>.</param>
    /// <param name="entity">The entity to remove from the data store.</param>
    /// <returns>The entity that was removed.</returns>
    /// <exception cref="NotSupportedException">Thrown when the queryable source is not a <see cref="Microsoft.EntityFrameworkCore.DbSet{T}"/>.</exception>
    public static T Remove<T>(this IQueryable<T> queryable, T entity)
        where T : class
        => queryable.DbSet().Remove(entity).Entity;

    /// <summary>
    /// Adds the specified entity to the object collection underlying the given queryable in <see cref="EntityState.Modified"/> state<br/>
    /// or if it is already being tracked in the change-tracker - changes the state of the entity to <see cref="EntityState.Modified"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity to be updated.</typeparam>
    /// <param name="queryable">The queryable source to which the entity will be added. Must be a <see cref="Microsoft.EntityFrameworkCore.DbSet{T}"/>.</param>
    /// <param name="entity">The entity that was modified.</param>
    /// <returns>The entity that was modified.</returns>
    /// <exception cref="NotSupportedException">Thrown when the queryable source is not a <see cref="Microsoft.EntityFrameworkCore.DbSet{T}"/>.</exception>
    public static T Update<T>(this IQueryable<T> queryable, T entity)
        where T : class
        => queryable.DbSet().Update(entity).Entity;

    /// <summary>
    /// Executes a string representing a parameterized native SQL query against the physical store.
    /// Any interpolated parameter values you supply in the <paramref name="query"/> parameter will
    /// automatically be converted to a DbParameter.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="queryable">The queryable. It must be <see cref="Microsoft.EntityFrameworkCore.DbSet{TEntity}"/>.</param>
    /// <param name="query">The query - preferable interpolated string.</param>
    /// <returns><see cref="IQueryable{TEntity}"/></returns>
    public static IQueryable<TEntity> FromRawSql<TEntity>(this IQueryable<TEntity> queryable, FormattableString query)
        where TEntity : class
        => queryable.DbSet().FromSql(query);

    /// <summary>
    /// Executes a string representing a raw, native SQL query against the queryable.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="queryable">The queryable. It must be <see cref="Microsoft.EntityFrameworkCore.DbSet{TEntity}" />.</param>
    /// <param name="query">The query.</param>
    /// <param name="parameters">The parameters. Any parameter values you supply will automatically be converted to a DbParameter.</param>
    /// <returns><see cref="IQueryable{TEntity}"/></returns>
    public static IQueryable<TEntity> FromRawSql<TEntity>(this IQueryable<TEntity> queryable, string query, params object[] parameters)
        where TEntity : class
        => queryable.DbSet().FromSqlRaw(query, parameters);
}
