namespace vm2.Common.Implementations;

/// <summary>
/// Provides the current tenant identifier for a multi-tenant application.
/// </summary>
/// <typeparam name="TTenantId">
/// The type of the tenant identifier. Must be non-nullable and implement <see cref="IEquatable{T}"/>.
/// </typeparam>
public class CurrentTenantProvider<TTenantId> : ITenantProvider<TTenantId> where TTenantId : notnull, IEquatable<TTenantId>
{
    /// <summary>
    /// Represents an asynchronous, thread-local storage for the tenant identifier.
    /// </summary>
    /// <remarks>
    /// This field is used to store the tenant identifier in a way that is specific to the current asynchronous context. It
    /// ensures that the tenant identifier is preserved across asynchronous calls within the same logical execution flow, i.e.,
    /// the current unit of work which is equivalent to the current request or operation.
    /// </remarks>
    AsyncLocal<TTenantId> _tenantId = new();

    /// <summary>
    /// Gets the identifier of the current tenant for the current asynchronous context/UoW.
    /// </summary>
    public TTenantId TenantId => _tenantId.Value ?? throw new InvalidOperationException("The current tenant is not set.");

    /// <summary>
    /// Sets the current tenant identifier for the current asynchronous context/UoW.
    /// </summary>
    /// <param name="tenantId">The identifier of the tenant to set as the current tenant.</param>
    public void SetCurrentTenant(TTenantId tenantId) => _tenantId.Value = tenantId;
}
