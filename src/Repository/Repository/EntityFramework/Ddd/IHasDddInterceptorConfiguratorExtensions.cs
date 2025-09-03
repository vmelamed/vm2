namespace vm2.Repository.EntityFramework.Ddd;

/// <summary>
/// Provides extension methods for <see cref="IHasDddInterceptorConfigurator"/>, e.g. <see cref="DbContext"/>(or <see cref="EfRepository"/>).
/// </summary>
public static class IHasDddInterceptorConfiguratorExtensions
{
    /// <summary>
    /// Adds the specified types to the set of allowed aggregate root types.
    /// </summary>
    /// <remarks>
    /// If the set of allowed aggregate root types is not already initialized, it will be created and
    /// populated with the specified types.
    /// </remarks>
    /// <param name="contextWithDdd">An instance of <see cref="IHasDddInterceptorConfigurator"/>.</param>
    /// <param name="types">An array of <see cref="Type"/> objects representing the aggregate root types to allow.</param>
    /// <returns>A set of <see cref="Type"/> objects that includes the newly allowed aggregate root types.</returns>
    public static ISet<Type> AllowAggregateRoots(
        this IHasDddInterceptorConfigurator contextWithDdd,
        params Type[] types)
    {
        contextWithDdd.AllowedAggregateRoots ??= new HashSet<Type>();

        foreach (var t in types)
            contextWithDdd.AllowedAggregateRoots.Add(t);

        return contextWithDdd.AllowedAggregateRoots;
    }
}
