namespace vm2.Repository.EntityFramework.Ddd;

/// <summary>
/// Represents the parameters required to perform an action on an entity.
/// </summary>
/// <remarks>
/// This record struct encapsulates the necessary context and metadata for executing operations on a DDD aggregate(s). It
/// includes information about the entity being acted upon, the required actions, the aggregate root type, and other
/// contextual details.
/// </remarks>
/// <param name="Entry">
/// The <see cref="EntityEntry"/> representing the entity being acted upon.
/// </param>
/// <param name="Actions">
/// The <see cref="DddAggregateActions"/> specifying the actions that should be performed on the aggregate.
/// </param>
/// <param name="AggregateRoot">
/// The type of the aggregate root, or <see langword="null"/> if the aggregate root is not determined yet.
/// .</param>
/// <param name="AllowedRoots">
/// A set of aggregate root types that are allowed for the operation.
/// </param>
/// <param name="Tenanted">
/// The tenanted context, or <see langword="null"/> if it is not determined yet.
/// </param>
/// <param name="Actor">
/// The the current actor's audit information.
/// </param>
/// <param name="Now">
/// The current timestamp representing the time of the operation.
/// </param>
/// <param name="CancellationToken">
/// A token to monitor for cancellation requests, allowing the operation to be canceled.
/// </param>
readonly record struct ActionsParameters(
    EntityEntry Entry,
    Type? AggregateRoot,
    DddAggregateActions Actions,
    ISet<Type> AllowedRoots,
    ITenanted? Tenanted,
    string Actor,
    DateTime Now,
    CancellationToken CancellationToken);
