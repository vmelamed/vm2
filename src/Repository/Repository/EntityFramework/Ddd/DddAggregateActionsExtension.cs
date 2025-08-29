namespace vm2.Repository.EntityFramework.Ddd;

/// <summary>
/// Provides an extension for configuring DDD (Domain-Driven Design) boundary checking options in an Entity Framework
/// Core <see cref="DbContext"/>.
/// </summary>
/// <remarks>
/// This extension allows users to enable or configure additional boundary checking logic for DDD patterns when working with
/// Entity Framework Core. It is not a database provider and does not directly affect database interactions. Instead, it
/// provides hooks for applying services and validating options related to DDD boundary enforcement.
/// </remarks>
public sealed class DddAggregateActionsExtension : IDbContextOptionsExtension
{
    /// <summary>
    /// Gets the actions to apply to the entities of the aggregate associated with the current unit of work.
    /// </summary>
    public DddAggregateActions Actions { get; init; }

    /// <summary>
    /// Gets a value indicating whether an interceptor has been added to the current context.
    /// </summary>
    internal bool InterceptorAdded { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DddAggregateActionsExtension"/> class.
    /// </summary>
    /// <remarks>This constructor sets up the extension by initializing its associated metadata.</remarks>
    public DddAggregateActionsExtension(
        DddAggregateActions checks,
        bool interceptorAdded)
    {
        Actions          = checks;
        InterceptorAdded = interceptorAdded;
        Info             = new DddAggregateActionsExtensionInfo(this);
    }

    /// <summary>
    /// Creates a new instance of <see cref="DddAggregateActionsExtension"/> with the specified actions and interceptor state,
    /// or returns the current instance if the provided values match the existing ones.
    /// </summary>
    /// <param name="actions">The <see cref="DddAggregateActions"/> to associate with the new instance.</param>
    /// <param name="interceptorAdded">A value indicating whether the interceptor has been added.</param>
    /// <returns>
    /// The current instance if the specified <paramref name="actions"/> and <paramref name="interceptorAdded"/> match the
    /// existing values; otherwise, a new <see cref="DddAggregateActionsExtension"/> instance with the specified values.
    /// </returns>
    public DddAggregateActionsExtension With(
        DddAggregateActions actions, bool interceptorAdded)
        => actions          == Actions &&
           interceptorAdded == InterceptorAdded
                ? this
                : new DddAggregateActionsExtension(actions, interceptorAdded);

    /// <inheritdoc />
    public void ApplyServices(IServiceCollection services) { }

    /// <inheritdoc />
    public void Validate(IDbContextOptions options) { }

    /// <summary>
    /// Gets the information about the current database context options extension.
    /// </summary>
    public DbContextOptionsExtensionInfo Info { get; init; }
}
