namespace vm2.Repository.EntityFramework.CommitInterceptor;

/// <summary>
/// Represents the interceptor's configuration settings and associated commit actions to be performed.
/// </summary>
/// <remarks>This class provides access to a collection of commit actions that are defined during the
/// configuration building process. The commit actions represent operations to be executed  as part of the
/// configuration's lifecycle.</remarks>
/// <param name="builder"></param>
public class InterceptorConfiguration(InterceptorConfigurationBuilder builder)
{
    readonly IReadOnlyCollection<IPolicyRule> _commitActions = new ReadOnlyCollection<IPolicyRule>([..builder.Actions]);

    /// <summary>
    /// Gets the collection of commit actions to be performed.
    /// </summary>
    public IEnumerable<IPolicyRule> CommitActions => _commitActions;
}
