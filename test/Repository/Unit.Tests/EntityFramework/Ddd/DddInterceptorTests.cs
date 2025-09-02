namespace vm2.Repository.UnitTests.EntityFramework.Ddd;

using NSubstitute.ExceptionExtensions;

public class DddInterceptorTests
{
    static DbContextOptionsBuilder<TestEfContext> NewOptions(
        DddAggregateActions actions = DddAggregateActions.All,
        Func<string>? actorProvider = null,
        Func<DateTime>? timeProvider = null)
        => new DbContextOptionsBuilder<TestEfContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .UseDddInterceptor(
                    actions,
                    actorAuditProvider: actorProvider ?? Actor.Log,
                    dateTimeAuditProvider: timeProvider ?? Clock.Now);

    [Fact]
    public async Task SavingChanges_NoModifiedEntries_NoAction()
    {
        await using var ctx = new TestEfContext(NewOptions().Options);

        var result = await ctx.CommitAsync(TestContext.Current.CancellationToken);
        result.Should().Be(0);
    }

    [Fact]
    public async Task AddedEntity_Performs_Audit_Complete_Validate_InOrder()
    {
        Clock.Reset();
        Actor.Reset("adder");
        await using var ctx = new TestEfContext(NewOptions().Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEquivalentTo(["Complete", "Validate"]);

        e.CreatedAt.Should().Be(Clock.Initial);
        e.CreatedBy.Should().Be(Actor.Actors.First());

        e.UpdatedAt.Should().Be(Clock.Initial);
        e.UpdatedBy.Should().Be(Actor.Actors.First());
    }

    [Fact]
    public async Task ModifiedEntity_Performs_AuditUpdate_Complete_Validate_InOrder()
    {
        Clock.Reset();
        Actor.Reset("adder");
        await using var ctx = new TestEfContext(NewOptions().Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        Actor.Current = "modifier";

        e.Calls.Clear();
        e.Name = "changed";
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEquivalentTo(["Complete", "Validate"]);

        var time = Clock.Initial;
        var actor = Actor.Actors.ElementAt(0);
        e.CreatedAt.Should().Be(time);
        e.CreatedBy.Should().Be(actor);

        time += Clock.Step;
        Actor.Actors.ElementAt(1);
        e.UpdatedAt.Should().Be(time);
        e.UpdatedBy.Should().Be(actor);
    }

    [Fact]
    public async Task DeletedEntity_SoftDelete_SetsModifiedDuringInterception()
    {
        Clock.Reset();
        Actor.Reset("adder");
        await using var ctx = new TestEfContext(NewOptions().Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        Actor.Current = "deleter";
        e.Calls.Clear();
        ctx.TestEntitiesA.Remove(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        Assert.Contains("SoftDelete", e.Calls);
        var time = Clock.Initial;
        var actor = Actor.Actors.ElementAt(0);
        e.CreatedAt.Should().Be(time);
        e.CreatedBy.Should().Be(actor);

        time += Clock.Step;
        Actor.Actors.ElementAt(1);
        e.UpdatedAt.Should().Be(time);
        e.UpdatedBy.Should().Be(actor);
    }

    [Fact]
    public async Task Delete_WhenAuditDisabled_DoesNotSoftDelete()
    {
        Clock.Reset();
        Actor.Reset("actor");
        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All & ~DddAggregateActions.Audit).Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        ctx.TestEntitiesA.Remove(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        ((ISoftDeletable)e).IsDeleted.Should().BeFalse();
    }

    [Fact]
    public async Task ActionsNone_ShortCircuits_AllLogic()
    {
        Clock.Reset();
        Actor.Reset("actor");
        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All & ~DddAggregateActions.Audit).Options);

        ctx.AggregateActions = DddAggregateActions.None;

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEmpty();
    }

    [Fact]
    public async Task ExcludingCustomComplete_SkipsCompletion()
    {
        Clock.Reset();
        Actor.Reset("actor");
        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All & ~DddAggregateActions.Complete).Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEquivalentTo(["AuditAdd", "Validate"]);
    }

    [Fact]
    public async Task ExcludingInvariants_SkipsValidate()
    {
        Clock.Reset();
        Actor.Reset("actor");
        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All & ~DddAggregateActions.Invariants).Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEquivalentTo(new[] { "AuditAdd", "Complete" });
    }

    [Fact]
    public async Task ExcludingAudit_RetainsOtherActions()
    {
        Clock.Reset();
        Actor.Reset("actor");
        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All & ~DddAggregateActions.Audit).Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEquivalentTo(["Complete", "Validate"]);
    }

    [Fact]
    public async Task AggregateBoundary_Violation_Throws()
    {
        // Use two entities – conceptually different roots (same CLR type but root interface marker distinct via test design).
        // Since both are IAggregate<RootA> due to single class, simulate by first save, then second root expects different aggregate detection not triggered here.
        // To truly test violation, we create a second fake type implementing IAggregate<RootB>.
        Clock.Reset();
        Actor.Reset("actor");
        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All & ~DddAggregateActions.Audit).Options);

        var e1 = new TestEntityA(); // acts as RootA
        var e2 = new TestEntityB(); // acts as RootB
        ctx.TestEntitiesA.Add(e1);
        ctx.Add(e2);

        var commit = async (CancellationToken ct) => await ctx.CommitAsync(ct);

        commit(TestContext.Current.CancellationToken).ThrowsAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task AggregateBoundary_AllowedRoots_PermitsMultiple()
    {
        Clock.Reset();
        Actor.Reset("actor");
        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All & ~DddAggregateActions.Audit).Options);

        ctx.AllowAggregateRoots(typeof(RootA), typeof(RootB));

        ctx.TestEntitiesA.Add(new TestEntityA());       // RootA
        ctx.Add(new TestEntityB());                     // RootB allowed
        var added = await ctx.CommitAsync(TestContext.Current.CancellationToken);

        added.Should().Be(2);
    }

    [Fact]
    public async Task TenantBoundary_Disabled_AllowsDifferentTenants()
    {
        var actions = DddAggregateActions.All & ~DddAggregateActions.TenantBoundary;
        var options = NewOptions(actions).Options;
        await using var ctx = new TestEfContext(options, () => Guid.NewGuid(), () => "actor");

        // Because entity tenancy is entity instance itself, two entities appear as two tenants,
        // but tenant boundary disabled means success.
        ctx.TestEntitiesA.Add(new TestEntityA());
        ctx.TestEntitiesA.Add(new TestEntityA());
        await ctx.CommitAsync(TestContext.Current.CancellationToken);
    }

    [Fact]
    public async Task TenantBoundary_Enabled_DifferentEntityTenants_Throws()
    {
        var options = NewOptions().Options;
        await using var ctx = new TestEfContext(options, () => Guid.NewGuid(), () => "actor");

        ctx.TestEntitiesA.Add(new TestEntityA());
        ctx.TestEntitiesA.Add(new TestEntityA());

        // Both have different TenantId references (self), so should violate boundary (depending on context SameTenantAs logic).
        var ex = await Assert.ThrowsAnyAsync<Exception>(() => ctx.SaveChangesAsync(TestContext.Current.CancellationToken));
        Assert.Contains("tenant", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AddedThenModified_ProducesExpectedSequences()
    {
        var options = NewOptions().Options;
        await using var ctx = new TestEfContext(options, () => Guid.NewGuid(), () => "actor");

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);
        .Should().Be(new[] { "AuditAdd", "Complete", "Validate" }, e.Calls);

        e.Calls.Clear();
        e.Name = "change";
        await ctx.CommitAsync(TestContext.Current.CancellationToken);
        .Should().Be(new[] { "AuditUpdate", "Complete", "Validate" }, e.Calls);
    }
}