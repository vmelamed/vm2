namespace vm2.Repository.EntityFramework.CommitInterceptor.PolicyRules;

/// <summary>
/// Enforces tenant boundary restrictions during entity operations. Ensures that entities belong to the same tenant as the
/// tenant of the current <see cref="DbContext"/> or the tenant of the first tenanted entity passed in this async context
/// (i.e. the current commit).
/// </summary>
public class EnforceTenantBoundary : IPolicyRule
{
    /// <summary>
    /// The exception message used when entities from different tenants are detected in a single unit of work (the current
    /// commit, async context, etc.)
    /// </summary>
    public const string DifferentTenants
        = "Detected entities from different tenants, or an entity from a tenant different from the tenant of the repository.";

    AsyncLocal<ITenanted> _tenant = new();
    Func<ITenanted>? _getCurrentTenant = null;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnforceTenantBoundary"/> class,  which enforces tenant-specific
    /// boundaries by retrieving the current tenant context from an external tenant accessor.
    /// </summary>
    /// <remarks>
    /// The provided <paramref name="getCurrentTenant"/> delegate is used to determine the current tenant context, which is
    /// essential for enforcing tenant-specific boundaries in multi-tenant applications. Ensure that the delegate is not null
    /// and returns a valid tenant context.
    /// </remarks>
    /// <param name="getCurrentTenant">A delegate that retrieves the current tenant.</param>
    public EnforceTenantBoundary(Func<ITenanted>? getCurrentTenant = null)
        => _getCurrentTenant = getCurrentTenant;

    /// <summary>
    /// Performs a tenant validation check on the specified entity  to ensure that it belongs to the same tenant as the tenant
    /// of the current <see cref="DbContext"/> or the tenant of the first tenanted entity passed in this async context (i.e. the
    /// current commit).
    /// </summary>
    /// <param name="entry">The <see cref="EntityEntry"/> representing an entity to validate.</param>
    /// <param name="_">The <see cref="CancellationToken"/> is not used here.</param>
    /// <returns>
    /// A <see cref="ValueTask"/> that represents the asynchronous operation. The task is completed immediately if no validation
    /// is required.
    /// </returns>
    /// <exception cref="UnauthorizedAccessException">Thrown if the entity does not belong to the current tenant.</exception>
    public ValueTask EntityActionAsync(EntityEntry entry, CancellationToken _ = default)
    {
        if (entry.Entity is not ITenanted entity)
            // nothing to check
            return ValueTask.CompletedTask;

        if (_tenant.Value is null)
        {
            if (_getCurrentTenant is not null)
                _tenant.Value = _getCurrentTenant();
            else
            if (entry.Context is ITenanted contextTenant)
                // this and all subsequent entities must be from the tenant of the current DbContext
                _tenant.Value = contextTenant;
            else
            {
                // or if this is the first tenanted entity checked in this async context - adopt it for all subsequent entities
                // in this async context (all entities touched in the current commit)
                _tenant.Value = entity;
                return ValueTask.CompletedTask;
            }
        }

        if (!_tenant.Value.SameTenantAs(entity))
            throw new InvalidOperationException(DifferentTenants);

        return ValueTask.CompletedTask;
    }
}
