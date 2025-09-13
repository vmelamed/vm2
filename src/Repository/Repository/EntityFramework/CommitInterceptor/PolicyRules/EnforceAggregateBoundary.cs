namespace vm2.Repository.EntityFramework.CommitInterceptor.PolicyRules;
/// <summary>
/// Enforces aggregate boundary rules in a Domain-Driven Design (DDD) context.
/// </summary>
/// <remarks>
/// This class ensures that entities and value objects adhere to the aggregate boundary constraints within a single unit of work
/// (transaction). Specifically, it validates that:
/// <list type="bullet">
/// <item> An entity does not implement more than one <c>IAggregate&lt;TRoot&gt;</c> interface. </item>
/// <item> Entities or values from multiple aggregates are not used together in the same unit of work. </item>
/// <item> All entities are part of a defined aggregate, unless explicitly allowed for the current context only. </item>
/// </list>
/// Violations of these rules result in exceptions with detailed error messages to help identify the issue.</remarks>
public class EnforceAggregateBoundary : IPolicyRule
{
    /// <summary>
    /// The exception message used when an entity implements more than one IAggregate&lt;TRoot&gt; interface.
    /// </summary>
    public const string DddErrorMessageHasMoreThanOneAggregate
        = "Type \"{0}\" is marked with multiple IAggregate<TRoot> interfaces. A DDD entity or value cannot be part of more than one aggregate.";

    /// <summary>
    /// The exception message used when a violation of aggregate boundaries is detected in a single unit of work (transaction),
    /// </summary>
    public const string DddErrorMessageViolationOfAggregateBoundary
        = "Violation of aggregate boundaries: entities of IAggregate<{0}> are not allowed.";

    AsyncLocal<IEnumerable<Type>> _allowedAggregateRootTypes = new();

    /// <summary>
    /// Validates the aggregate root type of the specified entity and ensures it adheres to the allowed aggregate boundary rules.
    /// </summary>
    /// <remarks>
    /// This method determines the aggregate root type of the entity by inspecting its implemented interfaces. If the entity's
    /// aggregate root type is not allowed, or if the entity implements multiple aggregate root interfaces, an exception is thrown.
    /// </remarks>
    /// <param name="entry">The <see cref="EntityEntry"/> representing the entity being validated.</param>
    /// <param name="_">The <see cref="CancellationToken"/> is not used here.</param>
    /// <returns>
    /// A <see cref="ValueTask"/> that represents the asynchronous operation. The task completes successfully if the entity's
    /// aggregate root type is valid; otherwise, an exception is thrown.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown if the entity's aggregate root type violates the allowed aggregate
    /// boundary rules, or if the entity implements more than one aggregate root interface.</exception>
    public ValueTask EntityActionAsync(EntityEntry entry, CancellationToken _ = default)
    {
        // if we do not know the allowed root types yet, try to get them from the context
        var roots = _allowedAggregateRootTypes.Value;

        if (roots is null && entry.Context is IHasAllowedAggregateRoots hasAggregateRoots)
        {
            roots = hasAggregateRoots.AllowedAggregateRootTypes;

            if (roots is not null)
                _allowedAggregateRootTypes.Value = roots;
        }

        var entity = entry.Entity;

        // determine the aggregate root type of the current entity
        var rootTypes = entity
                            .GetType()
                            .GetInterfaces()
                            .Where(i => i.IsGenericType  &&
                                        i.GetGenericTypeDefinition() == typeof(IAggregate<>))
                            .Select(i => i.GetGenericArguments()[0])
                            .ToList()
                            ;

        var rootType = rootTypes.Count is <= 1
                            ? rootTypes.SingleOrDefault(typeof(NoRoot))
                            : throw new InvalidOperationException(
                                            string.Format(DddErrorMessageHasMoreThanOneAggregate, entity.GetType().Name));

        Debug.Assert(rootType is not null);

        if (roots is null && rootType != typeof(NoRoot))
        {
            // rootType becomes the one and only required root type for the rest of the entities in the tracker
            // NoRoot can never become required root type
            _allowedAggregateRootTypes.Value ??= [rootType];
            return ValueTask.CompletedTask;
        }

        if (roots?.Contains(rootType) is not true)
            throw new InvalidOperationException(
                        string.Format(DddErrorMessageViolationOfAggregateBoundary, rootType.Name));

        // TODO: add OpenTelemetry metric for _allowedAggregateRootTypes.Count > 1
        return ValueTask.CompletedTask;
    }
}
