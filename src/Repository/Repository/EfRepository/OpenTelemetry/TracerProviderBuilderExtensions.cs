namespace vm2.Repository.EfRepository.OpenTelemetry;

/// <summary>
/// Class TracerProviderBuilderExtensions.
/// </summary>
public static class TracerProviderBuilderExtensions
{
    /// <summary>
    /// AddAsync repository Entity Framework for Cosmos DB support.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="configure">Entity framework instrumentation.</param>
    /// <returns>TracerProviderBuilder.</returns>
    /// <exception cref="System.ArgumentNullException">builder</exception>
    /// <remarks>
    /// <seealso href="https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/main/src/OpenTelemetry.Instrumentation.EntityFrameworkCore/README.md"/>
    /// </remarks>
    public static TracerProviderBuilder AddRepositoryEntityFrameworkInstrumentation(
        this TracerProviderBuilder builder,
        Action<EntityFrameworkInstrumentationOptions>? configure = null)
        => builder
            .AddEntityFrameworkCoreInstrumentation(configure)
            .AddSource(EfRepositoryActivitySource.ActivitySourceName)
            ;
}