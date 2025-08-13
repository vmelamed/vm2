namespace vm2.Repository.Abstractions.Extensions;

/// <summary>
/// Provides extension methods for relational database operations - executing raw SQL
/// </summary>
public static class RelationalExtensions
{
    const string notSupportedMessageFormat = "The method {0} is not supported. IRepository must be implemented by Microsoft.EntityFrameworkCore.DbSet<TEntity>";

    private static NotSupportedException NotSupported(string methodName = "")
        => new(string.Format(notSupportedMessageFormat, methodName));

    /// <summary>
    /// Executes a string representing a parameterized native SQL query against the physical store.
    /// Any interpolated parameter values you supply in the <paramref name="query"/> parameter will
    /// automatically be converted to a DbParameter.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="queryable">The queryable. It must be <see cref="DbSet{TEntity}"/>.</param>
    /// <param name="query">The query - preferable interpolated string.</param>
    /// <returns><see cref="IQueryable{TEntity}"/></returns>
    public static IQueryable<TEntity> FromRawSql<TEntity>(
        this IQueryable<TEntity> queryable,
        FormattableString query)
        where TEntity : class
        => queryable is DbSet<TEntity> dbSet
                ? RelationalQueryableExtensions.FromSql(dbSet, query)
                : throw NotSupported(nameof(FromRawSql));

    /// <summary>
    /// Executes a string representing a raw, native SQL query against the queryable.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="queryable">The queryable. It must be <see cref="DbSet{TEntity}" />.</param>
    /// <param name="query">The query.</param>
    /// <param name="parameters">The parameters. Any parameter values you supply will automatically be converted to a DbParameter.</param>
    /// <returns><see cref="IQueryable{TEntity}"/></returns>
    public static IQueryable<TEntity> FromRawSql<TEntity>(
        this IQueryable<TEntity> queryable,
        string query,
        params object[] parameters)
        where TEntity : class
        => queryable is DbSet<TEntity> dbSet
                ? RelationalQueryableExtensions.FromSqlRaw(dbSet, query, parameters)
                : throw NotSupported(nameof(FromRawSql));
}
