namespace vm2.Repository.EntityFramework.CommitInterceptor.PolicyRules;

/// <summary>
/// Represents an action that enforces invariants on an entity during a commit operation.
/// </summary>
public class EnforceInvariants : IPolicyRule
{
    /// <summary>
    /// Performs an asynchronous action on the specified entity, such as validation, if the entity implements <see
    /// cref="IValidatable"/>.
    /// </summary>
    /// <param name="entry">The <see cref="EntityEntry"/> representing the entity and its state in the context.</param>
    /// <param name="ct">
    /// A <see cref="CancellationToken"/> to observe while waiting for the operation to complete. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>
    /// A <see cref="ValueTask"/> that represents the asynchronous operation. If the entity implements <see cref="IValidatable"/>,
    /// the task completes after the validation is performed; otherwise, it completes immediately.
    /// </returns>
    public ValueTask EntityActionAsync(
        EntityEntry entry,
        CancellationToken ct = default)
        => entry.Entity is IValidatable validatable
            ? validatable.ValidateAsync(entry.Context as IRepository, ct)
            : ValueTask.CompletedTask;

}
