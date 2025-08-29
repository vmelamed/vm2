namespace vm2.Repository.EntityFramework.Ddd;

/// <summary>
/// Provides extension methods for configuring DDD (Domain-Driven Design) aggregate actions in an Entity Framework on the
/// <see cref="DbContextOptionsBuilder"/>.
/// </summary>
public static class DddExtensions
{
    /// <summary>
    /// Configures the <see cref="DbContextOptionsBuilder"/> to enable the specified DDD aggregate actions before saving changes
    /// in <see cref="DbContext.SavedChanges"/>. Ensures the interceptor is added exactly once.
    /// </summary>
    /// <param name="optionsBuilder">
    /// The <see cref="DbContextOptionsBuilder"/> to configure.
    /// </param>
    /// <param name="actions">
    /// The <see cref="DddAggregateActions"/> to apply to the entities of the aggregate associated with the current unit of work.
    /// </param>
    /// <param name="interceptor">
    /// An optional instance of <see cref="EfRepositorySaveChangesInterceptor"/> to use.
    /// If not provided, a new instance will be created.
    /// </param>
    /// <returns>
    /// The same instance of the <see cref="DbContextOptionsBuilder"/> configured for DDD actions so that multiple calls can be
    /// chained.
    /// </returns>
    public static DbContextOptionsBuilder UseDddAggregateActions(
        this DbContextOptionsBuilder optionsBuilder,
        DddAggregateActions actions = DddAggregateActions.All,
        EfRepositorySaveChangesInterceptor? interceptor = null)
    {
        if (actions == DddAggregateActions.None)
            return optionsBuilder;

        // get existing extension (if any) and figure out if we need to update it
        var existingExtension = optionsBuilder.Options.FindExtension<DddAggregateActionsExtension>();

        // If the existing extension has the same actions and the interceptor was already added, there is nothing to do
        if (existingExtension?.Actions == actions && existingExtension.InterceptorAdded)
            return optionsBuilder;

        // Keep track if the interceptor was already added
        var interceptorAdded = existingExtension?.InterceptorAdded ?? false;

        // Update (or create) the extension with new actions
        var builderInfra = (IDbContextOptionsBuilderInfrastructure)optionsBuilder;
        var extension    = existingExtension is null
                                ? new DddAggregateActionsExtension(actions, interceptorAdded)
                                : existingExtension.With(actions, interceptorAdded);

        builderInfra.AddOrUpdateExtension(extension);

        if (!interceptorAdded)
        {
            // Add the parameter interceptor or create and add a new one
            optionsBuilder.AddInterceptors(interceptor ?? new EfRepositorySaveChangesInterceptor());

            // Mark the interceptor as added
            builderInfra.AddOrUpdateExtension(
                    extension.With(extension.Actions, interceptorAdded: true));
        }

        return optionsBuilder;
    }
}
