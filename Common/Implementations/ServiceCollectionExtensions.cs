namespace vm2.Common.Implementations;

/// <summary>
/// Provides extension methods for registering the common services in an <see cref="IServiceCollection"/>.
/// </summary>
/// <remarks>This class contains methods to simplify the registration of common services in a dependency injection
/// container.</remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the standard implementation of <see cref="IClock"/> to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the clock to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddClock(
        this IServiceCollection services)
        => services.AddSingleton<IClock, Clock>();
}