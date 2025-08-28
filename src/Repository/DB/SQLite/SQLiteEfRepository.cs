namespace vm2.Repository.DB.SQLite;

/// <summary>
/// A repository implementation using Entity Framework Core with SQLite as the database provider.
/// </summary>
public class SQLiteEfRepository(DbContextOptions options) : EfRepository(options)
{
    static SqliteConnection? _inMemConnection;

    /// <summary>
    /// Creates and opens a new SQLite database connection using the specified connection string.
    /// </summary>
    /// <remarks>
    /// If the connection string specifies an in-memory database (e.g., ":memory:"), a shared in-memory connection  is used for
    /// the lifetime of the application. Otherwise, a new connection is created and opened each time this method is called. The
    /// in-memory connection is reused to ensure that the database remains accessible across multiple calls.
    /// </remarks>
    /// <param name="connectionString">The connection string used to configure the SQLite database connection.  Must not be null, empty, or consist
    /// only of whitespace characters.</param>
    /// <returns>An open <see cref="SqliteConnection"/> instance configured with the specified connection string.</returns>
    /// <exception cref="ConfigurationErrorsException">Thrown if <paramref name="connectionString"/> is null, empty, consists only of whitespace, or is invalid.</exception>
    public static SqliteConnection GetConnection(string? connectionString)
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
}
