namespace vm2.Repository.UnitTests.EntityFramework.Ddd;

// Aggregate root marker types
sealed class RootA : IAggregate<RootA> { }

sealed class RootB : IAggregate<RootB> { }


// Tracking entity implementing requested interfaces + tenancy
abstract class TestEntity :
    ITenanted<Guid>,
    IAuditable,
    ISoftDeletable,
    ICompletable,
    IValidatable,
    IOptimisticConcurrency<byte[]>
{
    public Guid Id { get; set; } = default;

    // Tenant
    public Guid TenantId { get; set; }

    public string Name { get; set; } = "initial";

    // Audit
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = "";
    public DateTime UpdatedAt { get; set; }
    public string UpdatedBy { get; set; } = "";

    // Soft delete
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; } = "";

    // Call trace to assert ordering / inclusion
    public List<string> Calls { get; } = [];

    public ValueTask CompleteAsync(IRepository? repo, EntityEntry entry, DateTime now, string actor, CancellationToken ct)
    {
        Calls.Add("Complete");
        return ValueTask.CompletedTask;
    }

    public ValueTask ValidateAsync(object? repo, CancellationToken ct)
    {
        Calls.Add("Validate");
        return ValueTask.CompletedTask;
    }

    public void SoftDelete(
        DateTime? now = default,
        string actor = "")
    {
        Calls.Add("SoftDelete");
        DeletedAt = now ?? DateTime.UtcNow;
        DeletedBy = actor;
    }

    public void Undelete(
        DateTime? now = default,
        string actor = "")
    {
        Calls.Add("Undelete");
        DeletedAt = null;
        DeletedBy = string.Empty;

        if (this is IAuditable a)
            a.AuditOnUpdate(now ?? DateTime.UtcNow, actor);
    }

    public byte[] ETag { get; set; } = [];
}

class TestEntityA : TestEntity, IAggregate<RootA>
{
}

class TestEntityB : TestEntity, IAggregate<RootB>
{
}