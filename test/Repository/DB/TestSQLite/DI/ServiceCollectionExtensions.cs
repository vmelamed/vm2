namespace vm2.Repository.DB.TestSQLite.DI;

using vm2.Repository.EntityFramework.CommitInterceptor;
using vm2.Repository.EntityFramework.CommitInterceptor.PolicyRules;

public static class ServiceCollectionExtensions
{
    public const string EfSQLiteRepositoryKey = "SQLite";

    /// <summary>
    /// Registers a keyed, scoped EfSQLiteRepository both as its concrete type and as IRepository.
    /// Retrieve with: provider.GetRequiredKeyedService<IRepository>(key)
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    public static IServiceCollection AddEfSQLiteRepository(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddSingleton<FullDddTenantedAuditedPolicy>();

        services.AddKeyedSingleton(
            EfSQLiteRepositoryKey,
            (sp, o) =>
            {
                var connectionString           = configuration.GetConnectionString(EfSQLiteRepositoryKey)
                                                           ?? throw new ConfigurationErrorsException("Invalid SQLite DB connection string.");
                var enableDetailedErrors       = configuration.GetValue("Repository:EnableDetailedErrors", true);
                var enableSensitiveDataLogging = configuration.GetValue("Repository:EnableSensitiveDataLogging", true);
                var minLogLevel                = configuration.GetValue("Repository:MinLogLevel", LogLevel.None);
                var logMethod                  = (string s) => Console.WriteLine(s);

                var repoBuilder = new DbContextOptionsBuilder<EfSQLiteRepository>()
                                        .UseSqlite(connectionString)
                                        .EnableDetailedErrors(enableDetailedErrors)
                                        .EnableSensitiveDataLogging(enableSensitiveDataLogging)
                                        .LogTo(logMethod, minLogLevel)
                                        .AddInterceptors(
                                            new CommitInterceptor(
                                                new CommitInterceptorConfigurationBuilder(sp, configuration)
                                                        .WithPolicy<FullDddTenantedAuditedPolicy>()
                                                        .Build()))
                                        ;

                return repoBuilder;
            });

        // Keyed concrete registration
        services.AddKeyedScoped(
            EfSQLiteRepositoryKey,
            (sp, serviceKey) =>
                new EfSQLiteRepository(
                        sp.GetRequiredKeyedService<DbContextOptionsBuilder<EfSQLiteRepository>>(EfSQLiteRepositoryKey).Options));

        return services;
    }
}
