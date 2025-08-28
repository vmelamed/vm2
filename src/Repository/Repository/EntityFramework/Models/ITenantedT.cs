namespace vm2.Repository.EntityFramework.Models;

/// <summary>
/// Represents objects (entities, repositories, etc.) that are bound to a tenant and the tenant identifier.
/// </summary>
/// <typeparam name="TId"></typeparam>
public interface ITenanted<TId> : ITenanted where TId : notnull, IEquatable<TId>
{
    /// <summary>
    /// The identifier of the current tenant.
    /// </summary>
    TId TenantId { get; }

    /// <inheritdoc/>
    bool ITenanted.SameAs(ITenanted tenant)
        => tenant is ITenanted<TId> t && TenantId.Equals(t.TenantId);
}
