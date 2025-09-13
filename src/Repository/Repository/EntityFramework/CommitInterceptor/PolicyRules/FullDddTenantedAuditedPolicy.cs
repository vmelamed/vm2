namespace vm2.Repository.EntityFramework.CommitInterceptor.PolicyRules;

/// <summary>
/// Represents a policy that enforces:
/// <list type="bullet"><item>
/// tenant boundaries,
/// </item><item>
/// DDD aggregate boundaries,
/// </item><item>
/// auditing,
/// </item><item>
/// entity completion, and
/// </item><item>
/// invariants validation
/// </item></list>
/// for domain-driven design (DDD) entities in a multi-tenant environment.
/// </summary>
/// <remarks>
/// This policy is designed for use in systems that implement domain-driven design (DDD) principles and require auditing and
/// tenant-aware behavior. It applies a series of actions before changes are committed to the underlying repository to ensure
/// that entities comply with tenant boundaries, aggregate  boundaries, and invariants, while also performing auditing and
/// entity completion tasks.
/// </remarks>
public class FullDddTenantedAuditedPolicy : ICommitPolicy
{
    Func<DateTime> _clock;
    Func<string> _getCurrentActor;
    Func<ITenanted>? _getCurrentTenant = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="FullDddTenantedAuditedPolicy"/> class, configuring tenant
    /// enforcement, auditing, and domain-driven design (DDD) policies.
    /// </summary>
    /// <remarks>
    /// This constructor sets up a series of actions (policy) to enforce tenant boundaries, aggregate boundaries, entity
    /// auditing, entity completion, and invariant enforcement. These actions are applied in the context of domain-driven design
    /// (DDD) and are intended to ensure consistency and correctness within a multi-tenant application.
    /// </remarks>
    /// <param name="getCurrentTenant">A function that retrieves the current tenant. This function may return <see langword="null"/> if tenant
    /// information is not available.</param>
    /// <param name="clock">A function that provides the current date and time. If <see langword="null"/>, the default implementation uses
    /// <see cref="DateTime.UtcNow"/>.</param>
    /// <param name="getCurrentActor">A function that retrieves the identifier of the current actor (e.g., user or system process). If <see
    /// langword="null"/>, the default implementation returns an empty string.</param>
    public FullDddTenantedAuditedPolicy(
        Func<ITenanted>? getCurrentTenant,
        Func<DateTime>? clock,
        Func<string>? getCurrentActor)
    {
        _getCurrentTenant = getCurrentTenant;
        _clock            = clock is not null ? clock : () => DateTime.UtcNow;
        _getCurrentActor  = getCurrentActor is not null ? getCurrentActor : () => "";

        Actions = new IPolicyRule[]
        {
            new EnforceTenantBoundary(_getCurrentTenant),
            new EnforceAggregateBoundary(),
            new AuditEntities(_clock, _getCurrentActor),
            new CompleteEntities(_clock, _getCurrentActor),
            new EnforceInvariants(),
        };
    }

    /// <summary>
    /// Gets the collection of commit actions (the policy rules) to be performed.
    /// </summary>
    public IEnumerable<IPolicyRule> Actions { get; }
}
