namespace vm2.Repository.DB.SQLite.DI;

/// <summary>
/// Provides extension methods for configuring <see cref="DbContextOptionsBuilder{TContext}"/> instances for repositories based
/// on <see cref="SQLiteEfRepository"/>.
/// </summary>
/// <remarks>
/// This class includes methods to simplify the configuration of SQLite-based repositories, allowing developers to specify
/// connection strings, logging behavior, and other options through dependency injection or custom delegates.
/// </remarks>
public static class ContextOptionsBuilderExtensions
{
    /// <summary>
    /// The default configuration key used to identify the SQLite connection string in the configuration section "ConnectionStrings".
    /// </summary>
    public const string SQLiteEfRepositoryKey = "SQLite";

    /// <summary>
    /// Configures a <see cref="DbContextOptionsBuilder{TContext}"/> for a repository based on <see cref="SQLiteEfRepository"/>.
    /// </summary>
    /// <typeparam name="TRepository"></typeparam>
    /// <param name="builder">
    /// The <see cref="DbContextOptionsBuilder{TContext}"/> instance to configure. Cannot be <see langword="null"/>.
    /// </param>
    /// <param name="configuration">
    /// The application configuration from which to retrieve settings. Cannot be <see langword="null"/>.
    /// </param>
    /// <param name="getConnectionString">
    /// A function that retrieves the database connection string. If <see langword="null"/>, the connection string is obtained
    /// from the configuration using the key "ConnectionStrings:SQLite".
    /// </param>
    /// <param name="getEnableDetailedErrors">
    /// A function that indicates whether detailed errors should be enabled. If <see langword="null"/>, the value is obtained
    /// from the configuration key "EnableDetailedErrors", defaulting to <see langword="false"/> if not found.
    /// </param>
    /// <param name="getEnableSensitiveDataLogging">
    /// A function that indicates whether sensitive data logging should be enabled. If <see langword="null"/>, the value is
    /// obtained from the configuration key "EnableSensitiveDataLogging", defaulting to <see langword="false"/> if not found.
    /// </param>
    /// <param name="logMethod">
    /// An action that defines how EF log messages are handled. If <see langword="null"/>, log messages are written to the
    /// console.
    /// </param>
    /// <param name="getLogToMinLogLevel">
    /// A function that retrieves the minimum log level for EF log messages. If <see langword="null"/>, defaults to
    /// <see cref="LogLevel.None"/>, i.e. no logging.
    /// </param>
    /// <returns></returns>
    /// <exception cref="ConfigurationErrorsException"></exception>
    public static DbContextOptionsBuilder<TRepository> ConfigureBuilder<TRepository>(
        this DbContextOptionsBuilder<TRepository> builder,
        IConfiguration configuration,
        Func<string>? getConnectionString = null,
        Func<bool>? getEnableDetailedErrors = null,
        Func<bool>? getEnableSensitiveDataLogging = null,
        Action<string>? logMethod = null,
        Func<LogLevel>? getLogToMinLogLevel = null) where TRepository : SQLiteEfRepository
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configuration);

        getConnectionString           ??= () => configuration.GetConnectionString(SQLiteEfRepositoryKey)
                                                    ?? throw new ConfigurationErrorsException("Invalid SQLite DB connection string.");
        getEnableDetailedErrors       ??= () => configuration.GetValue("EnableDetailedErrors", false);
        getEnableSensitiveDataLogging ??= () => configuration.GetValue("EnableSensitiveDataLogging", false);
        logMethod                     ??= Console.WriteLine;
        getLogToMinLogLevel           ??= () => LogLevel.None;

        var options = builder
                        .UseSqlite(getConnectionString())
                        .EnableDetailedErrors(getEnableDetailedErrors())
                        .EnableSensitiveDataLogging(getEnableSensitiveDataLogging())
                        .LogTo(logMethod, getLogToMinLogLevel())
                        .WithDddAggregateBoundaryChecking(DddBoundaryChecks.Full)
                        .Options
                        ;

        return new DbContextOptionsBuilder<TRepository>(options);
    }
}
