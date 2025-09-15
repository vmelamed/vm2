namespace vm2.Common.Abstractions;

/// <summary>
/// Provides functionality to manage and set the current tenant context for a multi-tenant application.
/// </summary>
/// <typeparam name="TTenantId">
/// The type of the tenant identifier. Must be a non-nullable type that implements <see cref="IEquatable{T}"/>.
/// </typeparam>
public interface ITenantProvider<TTenantId> : ITenanted<TTenantId> where TTenantId : notnull, IEquatable<TTenantId>
{
    /// <summary>
    /// Sets the current tenant context for the application.
    /// </summary>
    /// <param name="tenantId">The identifier of the tenant to set as the current context. Cannot be null.</param>
    void SetCurrentTenant(TTenantId tenantId);
}
