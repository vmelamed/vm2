namespace vm2.Repository.DB.TestSQLite.DI;

using vm2.Repository.EfRepository;

public static class ServiceCollectionExtensions
{
    public const string EfSQLiteRepositoryKey = ContextOptionsBuilderExtensions.SQLiteEfRepositoryKey;

    /// <summary>
    /// Registers a keyed, scoped EfSQLiteRepository both as its concrete type and as IRepository.
    /// Retrieve with: provider.GetRequiredKeyedService<IRepository>(key)
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    public static IServiceCollection AddEfSQLiteRepository(
        this IServiceCollection services,
        IConfiguration configuration,
        Func<string> getActor,
        Func<string>? getConnectionString = null,
        Func<bool>? getEnableDetailedErrors = null,
        Func<bool>? getEnableSensitiveDataLogging = null,
        Action<string>? logMethod = null,
        Func<LogLevel>? getLogToMinLogLevel = null)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(getActor);

        // Keyed concrete registration
        services.AddKeyedScoped(
            EfSQLiteRepositoryKey,
            (sp, serviceKey) =>
            {
                var builder = new DbContextOptionsBuilder<EfSQLiteRepository>()
                                    .ConfigureBuilder(configuration,
                                                             getConnectionString,
                                                             getEnableDetailedErrors,
                                                             getEnableSensitiveDataLogging,
                                                             logMethod,
                                                             getLogToMinLogLevel);

                var options = builder.Options.WithExtension(new DddAggregateBoundaryChecking(DddBoundaryChecks.Full));

                return new EfSQLiteRepository(options, getActor);
            });

        // Keyed interface mapping reuses the concrete keyed instance
        services.AddKeyedScoped<IRepository>(
                            EfSQLiteRepositoryKey,
                            (sp, serviceKey) => sp.GetRequiredKeyedService<EfSQLiteRepository>(serviceKey));

        return services;
    }
}
