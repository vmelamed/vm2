namespace vm2.Repository.EntityFramework.Models;

/// <summary>
/// Represents objects (entities, repositories, etc.) that are bound to a tenant.
/// </summary>
public interface ITenanted
{
    /// <summary>
    /// Determines if two tenanted objects are bound to the same tenant.
    /// </summary>
    /// <param name="tenanted"></param>
    /// <returns>
    /// <see langword="true"/> if this and the <paramref name="tenanted"/> objects are bound to the same tenant,
    /// <see langword="false"/> otherwise.
    /// </returns>
    bool SameAs(ITenanted tenanted);
}
