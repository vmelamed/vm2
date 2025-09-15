namespace vm2.Repository.UnitTests.EntityFramework.Ddd;

// Use two entities – conceptually different roots (same CLR type but root interface marker distinct via test design).
// Since both are IAggregate<RootA> due to single class, simulate by first save, then second root expects different
// aggregate detection not triggered here. To truly test violation, we create a second fake type implementing
// IAggregate<RootB>.

// Aggregate root marker types
sealed class RootA : IAggregate<RootA> { }

sealed class RootB : IAggregate<RootB> { }

class TestEntity : TestEntityBase
{
}

class TestEntityA : TestEntityBase, IAggregate<RootA>
{
}

class TestEntityB : TestEntityBase, IAggregate<RootB>
{
}

class TestEntityAB : TestEntityBase, IAggregate<RootA>, IAggregate<RootB>
{
}

// Tracking entity implementing requested interfaces + tenancy
abstract class TestEntityBase :
    ITenanted<Guid>,
    IAuditable,
    ISoftDeletable,
    //ICompletable,
    IValidatable,
    IOptimisticConcurrency<byte[]>
{
    public Guid Id { get; set; } = TestEntityId.Next();

    // TestTenant
    public Guid TenantId { get; set; } = TestTenant.Current();

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
    [NotMapped]
    public List<string> Calls { get; } = [];

    public ValueTask CompleteAsync(IRepository? _, EntityEntry __, DateTime ___, string ____, CancellationToken _____)
    {
        Calls.Add("Complete");
        return ValueTask.CompletedTask;
    }

    public ValueTask ValidateAsync(object? repo, CancellationToken ct)
    {
        Calls.Add("Validate");
        return ValueTask.CompletedTask;
    }

    public void AuditOnAdd(
        DateTime? now = default,
        string actor = "")
    {
        Calls.Add("AuditAdd");
        CreatedAt = now ?? DateTime.UtcNow;
        CreatedBy = actor;
        UpdatedAt = CreatedAt;
        UpdatedBy = CreatedBy;
    }

    /// <summary>
    /// Sets all properties of an updated <see cref="IAuditable"/> with current values.
    /// </summary>
    /// <param name="now">
    /// </param>
    /// <param name="actor">
    /// The identifier of the actor performing the update. Can be an empty string if not specified.
    /// </param>
    public void AuditOnUpdate(
        DateTime? now = default,
        string actor = "")
    {
        Calls.Add("AuditUpdate");
        UpdatedAt = now ?? DateTime.UtcNow;
        UpdatedBy = actor;
    }
    /// <summary>
    /// Gets a value indicating whether the entity is marked as deleted.
    /// </summary>
    public bool IsDeleted => DeletedAt.HasValue;

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
        DeletedBy = "";
        UpdatedAt = now ?? DateTime.UtcNow;
        UpdatedBy = actor;
    }

    public byte[] ETag { get; set; } = [];
}
