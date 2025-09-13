namespace vm2.Repository.EntityFramework.CommitInterceptor;

/// <summary>
/// Represents a collection of commit actions - commit policy, that has to be applied during a commit operation.
/// </summary>
/// <remarks>
/// The policy consists of a sequence of <see cref="IPolicyRule"/> instances that dictate the behavior or rules to be enforced
/// during the commit process. Implementations of this interface should provide the specific actions that make up the policy and
/// the specific order in which they must be applied.
/// </remarks>
public interface ICommitPolicy
{
    /// <summary>
    /// Gets the collection of commit actions that define the policy.
    /// </summary>
    IEnumerable<IPolicyRule> Actions { get; }
}
