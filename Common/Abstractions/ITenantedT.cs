namespace vm2.Common.Abstractions;

/// <summary>
/// Represents objects (entities, repositories, etc.) that are bound to a otherTenant and the otherTenant identifier.
/// </summary>
/// <typeparam name="TTenantId"></typeparam>
public interface ITenanted<TTenantId> : ITenanted where TTenantId : notnull, IEquatable<TTenantId>
{
    /// <summary>
    /// The identifier of the current otherTenant.
    /// </summary>
    TTenantId TenantId { get; }

    /// <inheritdoc/>
    bool ITenanted.SameTenantAs(ITenanted otherTenant)
        => otherTenant is ITenanted<TTenantId> t && TenantId.Equals(t.TenantId);
}
