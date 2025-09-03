namespace vm2.Repository.UnitTests.EntityFramework.Ddd;

sealed class TestEfContext : EfRepository, ITenanted<Guid>, IHasDddInterceptorConfigurator
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
    public DbSet<TestEntityA> TestEntitiesA => Set<TestEntityA>();

    public DbSet<TestEntityB> TestEntitiesB => Set<TestEntityB>();

    // Simulate tenancy: override SameTenantAs via ITenanted (EfRepository already implements ITenanted? If not, adapt.)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.Entity<TestEntityA>().HasKey(e => e.Id);

    /// <inheritdoc/>
    public DddAggregateActions? AggregateActions { get; set; }

    /// <inheritdoc/>
    public ISet<Type>? AllowedAggregateRoots { get; set; } = new HashSet<Type>();

    /// <inheritdoc/>
    public Func<DateTime>? DateTimeAuditProvider { get; set; }

    /// <inheritdoc/>
    public Func<string>? CurrentActorAuditProvider { get; set; } = TestActor.Current;

    public InterceptorConfiguration ConfigureDddInterceptor(
        InterceptorConfiguration currentConfiguration)
        => currentConfiguration with {
            AllowedAggregateRoots = AllowedAggregateRoots ?? currentConfiguration.AllowedAggregateRoots,
            Actions               = AggregateActions ?? currentConfiguration.Actions,
            ActorAuditProvider    = TestActor.Current,
            DateTimeAuditProvider = TestClock.Now,
            TenantProvider        = () => this,
        };
}
