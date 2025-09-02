namespace vm2.Repository.EntityFramework.Ddd;

/// <summary>
/// Specifies the modes of boundary checking for domain-driven design (DDD) aggregates.
/// </summary>
/// <remarks>
/// This enumeration defines the available options for performing boundary checks on aggregates in a DDD context. The flags can
/// be combined to enable multiple modes of boundary checking simultaneously.
/// </remarks>
[Flags]
public enum DddAggregateActions
{
    /// <summary>
    /// No aggregate actions are performed.
    /// </summary>
    None = 0,

    /// <summary>
    /// Check if all tenanted added, modified and deleted objects are bound to the same tenant. If the repository is tenanted,
    /// these objects must be bound to the same tenant as the repository.
    /// </summary>
    TenantBoundary = 1,

    /// <summary>
    /// Check aggregate's boundary by making sure that all added, modified or deleted entities belong to the same aggregate.<br/>
    /// For that, the entities must be marked with <see cref="IAggregate{TRoot}"/>. If an entity is not marked with that interface<br/>
    /// it is considered to belong to <see cref="IAggregate{Unknown}"/>. A unit of work (transaction) must not contain added, modified,<br/>
    /// or deleted entities from different aggregates.
    /// </summary>
    AggregateBoundary = 2,

    /// <summary>
    /// Audit added, updated and deleted entities if they implement <see cref="IAuditable"/> and <see cref="ISoftDeletable"/>.
    /// </summary>
    Audit = 4,

    /// <summary>
    /// Completion of entities implementing <see cref="ICompletable"/>.
    /// </summary>
    Complete = 8,

    /// <summary>
    /// Validates the invariants of all added or modified entities that implement <see cref="IValidatable"/>.
    /// </summary>
    Invariants = 16,

    /// <summary>
    /// All checks and actions are performed.
    /// </summary>
    All = TenantBoundary | AggregateBoundary | Audit | Complete | Invariants,
}
