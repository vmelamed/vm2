namespace vm2.TestUtilities;
using System;

public class TenantProvider : ITenantProvider<Guid>
{
    /// <summary>
    /// Sets the current tenant identifier for the current asynchronous context.
    /// </summary>
    /// <param name="tenantId">The unique identifier of the tenant to set as the current tenant for the current asynchronous context.</param>
    public void SetCurrentTenant(Guid tenantId) => TenantId = tenantId;

    AsyncLocal<Guid> _tenantId = new();

    /// <summary>
    /// Gets the unique identifier of the tenant for the current asynchronous context.
    /// </summary>
    public Guid TenantId
    {
        get
        {
            if (_tenantId.Value == Guid.Empty)
                throw new InvalidOperationException("Current tenant is not set.");

            return _tenantId.Value;
        }

        private set => _tenantId.Value = value;
    }
}
