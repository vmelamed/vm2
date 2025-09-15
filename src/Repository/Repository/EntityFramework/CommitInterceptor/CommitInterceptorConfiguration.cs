namespace vm2.Repository.EntityFramework.CommitInterceptor;

/// <summary>
/// Represents the interceptor's configuration settings and the associated rules (actions) to be performed before <see cref="IRepository.CommitAsync"/>.
/// </summary>
/// <remarks>
/// This class provides access to a collection of rules that are defined during the configuration building process. The rules are
/// operations to be executed as part of the committing of the entities' changes in the lifecycle of a unit of work
/// (<see cref="IRepository"/> or <see cref="DbContext"/>).
/// </remarks>
/// <param name="builder">
/// The <see cref="CommitInterceptorConfigurationBuilder"/> instance used to construct the configuration, containing the collection of
/// rules (actions) to be performed.
/// </param>
public class CommitInterceptorConfiguration(CommitInterceptorConfigurationBuilder builder)
{
    readonly IReadOnlyCollection<IPolicyRule> _commitRules = new ReadOnlyCollection<IPolicyRule>([..builder.Rules]);

    /// <summary>
    /// Gets the collection of commit actions to be performed.
    /// </summary>
    public IEnumerable<IPolicyRule> CommitActions => _commitRules;
}
