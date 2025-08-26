namespace vm2.Repository.TestDomain;

/// <summary>
/// Marker interface for entities that support optimistic concurrency.
/// </summary>
public interface IOptimisticConcurrency
{
    /// <summary>
    /// The property used for optimistic concurrency control.
    /// </summary>
    Ulid ETag { get; set; }

    static void UpdateConcurrency(EntityEntry entry)
    {
        if (entry.Entity is IOptimisticConcurrency oc)
            oc.ETag = Ulid.NewUlid();
    }
}
