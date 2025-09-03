namespace vm2.Repository.UnitTests.EntityFramework.Ddd;

using vm2.TestUtilities;

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
                    actorAuditProvider: actorProvider ?? TestActor.Current,
                    dateTimeAuditProvider: timeProvider ?? TestClock.Now);

    static void ResetAll(string? firstActor = null)
    {
        TestClock.Reset();
        TestTenant.Reset();
        TestEntityId.Reset();
        TestActor.Reset(firstActor);
    }

    static bool AuditShouldBe(
        IAuditable a,
        DateTime createdAt,
        string createdBy,
        DateTime updatedAt,
        string updatedBy,
        DateTime? deletedAt = null,
        string deletedBy = "")
    {
        a.CreatedAt.Should().Be(createdAt);
        a.UpdatedAt.Should().Be(updatedAt);

        a.CreatedBy.Should().Be(createdBy);
        a.UpdatedBy.Should().Be(updatedBy);

        if (a is not ISoftDeletable d)
            return true;

        d.DeletedAt.Should().Be(deletedAt);
        d.DeletedBy.Should().Be(deletedBy);
        return true;
    }

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
        ResetAll("adder");

        await using var ctx = new TestEfContext(NewOptions().Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEquivalentTo(["AuditAdd", "Complete", "Validate"]);

        AuditShouldBe(
            e,
            TestClock.Times.ElementAt(0),
            TestActor.Actors.ElementAt(0),
            TestClock.Times.ElementAt(0),
            TestActor.Actors.ElementAt(0),
            null, "");
    }

    [Fact]
    public async Task ModifiedEntity_Performs_AuditUpdate_Complete_Validate_InOrder()
    {
        ResetAll("adder");

        await using var ctx = new TestEfContext(NewOptions().Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        AuditShouldBe(e,
            TestClock.Times.ElementAt(0),
            TestActor.Actors.ElementAt(0),
            TestClock.Times.ElementAt(0),
            TestActor.Actors.ElementAt(0),
            null, "");

        TestActor.Next("modifier");

        e.Calls.Clear();
        e.Name = "changed";
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEquivalentTo(["AuditUpdate", "Complete", "Validate"]);

        AuditShouldBe(e,
            TestClock.Times.ElementAt(0),
            TestActor.Actors.ElementAt(0),
            TestClock.Times.ElementAt(1),
            TestActor.Actors.ElementAt(1),
            null, "");
    }

    [Fact]
    public async Task DeletedEntity_SoftDelete_SetsDeletedDuringInterception()
    {
        ResetAll("adder");

        await using var ctx = new TestEfContext(NewOptions().Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        AuditShouldBe(e,
            TestClock.Times.ElementAt(0),
            TestActor.Actors.ElementAt(0),
            TestClock.Times.ElementAt(0),
            TestActor.Actors.ElementAt(0),
            null, "");

        TestActor.Next("deleter");

        e.Calls.Clear();
        ctx.TestEntitiesA.Remove(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEquivalentTo(["SoftDelete"]);
        e.IsDeleted.Should().BeTrue();

        AuditShouldBe(e,
            TestClock.Times.ElementAt(0),
            TestActor.Actors.ElementAt(0),
            TestClock.Times.ElementAt(0),
            TestActor.Actors.ElementAt(0),
            TestClock.Times.ElementAt(1),
            TestActor.Actors.ElementAt(1));

        TestActor.Next("un-deleter");

        e.Calls.Clear();
        e.Undelete();
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEquivalentTo(["Undelete", "AuditUpdate", "Complete", "Validate"]);
        e.IsDeleted.Should().BeFalse();

        AuditShouldBe(e,
            TestClock.Times.ElementAt(0),
            TestActor.Actors.ElementAt(0),
            TestClock.Times.ElementAt(2),
            TestActor.Actors.ElementAt(2),
            null, "");
    }

    [Fact]
    public async Task Delete_WhenAuditDisabled_DoesNotSoftDelete()
    {
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All & ~DddAggregateActions.Audit).Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        ctx.TestEntitiesA.Remove(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.IsDeleted.Should().BeFalse();

        ctx.Find<TestEntityA>(e.Id).Should().BeNull();
    }

    [Fact]
    public async Task TempActionsNone_ShortCircuits_AllLogic()
    {
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions().Options);

        ctx.AggregateActions = DddAggregateActions.None;

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEmpty();
    }

    [Fact]
    public async Task ExcludingCustomComplete_SkipsCompletion()
    {
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All & ~DddAggregateActions.Complete).Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEquivalentTo(["AuditAdd", "Validate"]);
    }

    [Fact]
    public async Task ExcludingInvariants_SkipsValidate()
    {
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All & ~DddAggregateActions.Invariants).Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEquivalentTo(["AuditAdd", "Complete"]);
    }

    [Fact]
    public async Task ExcludingAudit_RetainsOtherActions()
    {
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All & ~DddAggregateActions.Audit).Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        e.Calls.Should().BeEquivalentTo(["Complete", "Validate"]);
    }

    [Fact]
    public async Task AggregateBoundary_Violation_Throws()
    {
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All & ~DddAggregateActions.Audit).Options);

        var a = new TestEntityA(); // acts as RootA
        var b = new TestEntityB(); // acts as RootB
        ctx.TestEntitiesA.Add(a);
        ctx.TestEntitiesB.Add(b);

        var commit = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await commit.Should().ThrowAsync<InvalidOperationException>(string.Format(Interceptor.DddErrorMessageViolationOfAggregateBoundary1, nameof(RootA), nameof(RootB)));
    }

    [Fact]
    public async Task AggregateBoundary_AllowedRoots_PermitsMultiple()
    {
        ResetAll();

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
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All & ~DddAggregateActions.TenantBoundary).Options);

        // Because entity tenancy is entity instance itself, two entities appear as two tenants,
        // but tenant boundary disabled means success.
        var a = new TestEntityA();
        TestTenant.Next();
        var b = new TestEntityA();

        ctx.TestEntitiesA.Add(a);
        ctx.TestEntitiesA.Add(b);

        var commit = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await commit.Should().NotThrowAsync();
    }

    [Fact]
    public async Task TenantBoundary_Enabled_DifferentEntityTenants_Throws()
    {
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions().Options);

        var a = new TestEntityA();
        TestTenant.Next();
        var b = new TestEntityA();

        ctx.TestEntitiesA.Add(a);
        ctx.TestEntitiesA.Add(b);

        // Both have different TenantId references (self), so should violate boundary (depending on context SameTenantAs logic).
        var commit = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await commit.Should().ThrowAsync<Exception>("Enabled_DifferentEntityTenants").WithMessage(Interceptor.DifferentTenants);
    }

    [Fact]
    public async Task AddedThenModified_ProducesExpectedSequences()
    {
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions().Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);
        e.Calls.Should().BeEquivalentTo(["AuditAdd", "Complete", "Validate"]);

        e.Calls.Clear();
        e.Name = "change";
        await ctx.CommitAsync(TestContext.Current.CancellationToken);
        e.Calls.Should().BeEquivalentTo(["AuditUpdate", "Complete", "Validate"]);
    }
}