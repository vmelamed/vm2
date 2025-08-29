namespace vm2.Repository.EntityFramework.Models;

/// <summary>
/// Represents objects (entities, repositories, etc.) that are bound to a tenant and the tenant identifier.
/// </summary>
/// <typeparam name="TTenantId"></typeparam>
public interface ITenanted<TTenantId> : ITenanted where TTenantId : notnull, IEquatable<TTenantId>
{
    /// <summary>
    /// The identifier of the current tenant.
    /// </summary>
    TTenantId TenantId { get; }

    /// <inheritdoc/>
    bool ITenanted.SameAs(ITenanted tenant)
        => tenant is ITenanted<TTenantId> t && TenantId.Equals(t.TenantId);
}
