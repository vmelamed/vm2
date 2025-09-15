namespace vm2.Repository.UnitTests.EntityFramework.Ddd;

using vm2.Repository.EntityFramework.CommitInterceptor.PolicyRules;

sealed class TestEfContext : EfRepository, ITenanted<Guid>, IHasAllowedAggregateRoots
{
    public TestEfContext(
        DbContextOptions options,
        Func<Guid>? currentTenantId = null)
        : base(options)
    {
        TenantId = (currentTenantId ?? TestTenant.Current)();
    }

    public Guid TenantId { get; init; }

    // Expose DbSets for two aggregates
    public DbSet<TestEntity> TestEntities => Set<TestEntity>();

    public DbSet<TestEntityA> TestEntitiesA => Set<TestEntityA>();

    public DbSet<TestEntityB> TestEntitiesB => Set<TestEntityB>();

    public DbSet<TestEntityAB> TestEntitiesAB => Set<TestEntityAB>();

    // Simulate tenancy: override SameTenantAs via ITenanted (EfRepository already implements ITenanted? If not, adapt.)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.Entity<TestEntityA>().HasKey(e => e.Id);

    /// <inheritdoc/>
    public IEnumerable<Type>? AllowedAggregateRootTypes { get; }
}
