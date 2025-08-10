namespace vm2.Repository.DB.SQLiteRepository;

/// <summary>
/// A repository implementation using Entity Framework Core with SQLite as the database provider.
/// </summary>
public class SQLiteDbContextRepository(DbContextOptions options) : DbContextRepository(options)
{
    static SqliteConnection? _inMemConnection;

    internal static SqliteConnection GetConnection(string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ConfigurationErrorsException("SQLite DB connection string cannot be null, empty, or consist of whitespace characters only.");

        var cb = new DbConnectionStringBuilder() { ConnectionString = connectionString };
        var db = cb.GetDatabase();

        if (string.IsNullOrWhiteSpace(db))
            throw new ConfigurationErrorsException("Invalid SQLite DB connection string.");

        if (db.Equals(":memory:", StringComparison.OrdinalIgnoreCase) is true)
            _inMemConnection ??= new SqliteConnection(connectionString);

        var connection = new SqliteConnection(connectionString);

        connection.Open();
        return connection;
    }

    internal const string OptimisticConcurrencyFieldName = "_etag";

    /// <inheritdoc/>
    protected override ValueTask CompleteAndValidateAddedEntity(
        EntityEntry entry,
        DateTime now,
        string actor,
        CancellationToken cancellationToken)
    {
        entry.Property(OptimisticConcurrencyFieldName).CurrentValue = Guid.NewGuid();
        return base.CompleteAndValidateAddedEntity(entry, now, actor, cancellationToken);
    }

    /// <inheritdoc/>
    protected override ValueTask CompleteAndValidateUpdatedEntity(
        EntityEntry entry,
        DateTime now,
        string actor,
        CancellationToken cancellationToken)
    {
        entry.Property(OptimisticConcurrencyFieldName).CurrentValue = Guid.NewGuid();
        return base.CompleteAndValidateUpdatedEntity(entry, now, actor, cancellationToken);
    }
}
