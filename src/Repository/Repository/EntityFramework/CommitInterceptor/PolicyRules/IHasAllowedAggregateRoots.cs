namespace vm2.Repository.EntityFramework.CommitInterceptor.PolicyRules;

/// <summary>
/// Defines a contract for objects (typically <see cref="DbContext"/>) that specify a collection of allowed aggregate root types.
/// </summary>
/// <remarks>
/// This interface is typically used to enforce constraints on the types of aggregate roots that can be associated with a
/// particular context or operation. Implementations should provide the list of allowed types through the
/// <see cref="AllowedAggregateRootTypes"/> property. Note that all aggregate types that are allowed must be in this list, including
/// the main aggregate and <see cref="NoRoot"/> if applicable.
/// </remarks>
public interface IHasAllowedAggregateRoots
{
    /// <summary>
    /// Gets the collection of allowed aggregate root types. Note that if it is not <see langword="null"/> all allowed aggregate
    /// types must be in this list, including the main aggregate and <see cref="NoRoot"/> if applicable.
    /// </summary>
    IEnumerable<Type>? AllowedAggregateRootTypes { get; }
}
