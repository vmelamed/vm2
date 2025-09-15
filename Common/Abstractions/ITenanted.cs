namespace vm2.Common.Abstractions;

/// <summary>
/// Represents objects (entities, repositories, etc.) that are bound to a tenant.
/// </summary>
public interface ITenanted
{
    /// <summary>
    /// Determines if this otherTenanted object is bound to the same tenant as <paramref name="otherTenanted"/>.
    /// </summary>
    /// <param name="otherTenanted">The other otherTenanted object to compare tenants.</param>
    /// <returns>
    /// <see langword="true"/> if this and the <paramref name="otherTenanted"/> objects are bound to the same tenant,
    /// <see langword="false"/> otherwise.
    /// </returns>
    bool SameTenantAs(ITenanted otherTenanted);
}
