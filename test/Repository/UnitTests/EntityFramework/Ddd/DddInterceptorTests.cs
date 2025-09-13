namespace vm2.Repository.UnitTests.EntityFramework.Ddd;

using vm2.Repository.EntityFramework.CommitInterceptor.PolicyRules;

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
    public async Task AddedAndModifiedEntities_InSameCommit_GetRespectivePipelines()
    {
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions().Options);

        // Seed existing entity
        var existing = new TestEntityA();
        ctx.TestEntitiesA.Add(existing);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        existing.Calls.Clear();

        // Modify existing
        existing.Name = "changed";

        // Add new one
        var added = new TestEntityA();
        ctx.TestEntitiesA.Add(added);

        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        existing.Calls.Should().BeEquivalentTo(["AuditUpdate", "Complete", "Validate"]);
        added.Calls.Should().BeEquivalentTo(["AuditAdd", "Complete", "Validate"]);
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
    public async Task Remove_SoftDelete_ChangesEntityState()
    {
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions().Options);

        var e = new TestEntityA();
        ctx.TestEntitiesA.Add(e);
        await ctx.CommitAsync(TestContext.Current.CancellationToken);
        e.Calls.Clear();

        // Track initial state change
        var entry = ctx.Entry(e);
        entry.State.Should().Be(EntityState.Unchanged);

        ctx.TestEntitiesA.Remove(e);
        entry.State.Should().Be(EntityState.Deleted); // pre-interceptor

        await ctx.CommitAsync(TestContext.Current.CancellationToken);

        // After interceptor soft delete: EF performed an update instead
        entry.State.Should().Be(EntityState.Unchanged);
        e.IsDeleted.Should().BeTrue();
        e.Calls.Should().BeEquivalentTo(["SoftDelete"]);

        // Verify still present in the DB
        ctx.ChangeTracker.Clear();

        var e1 = ctx.TestEntitiesA.Find(e.Id);
        e.IsDeleted.Should().BeTrue();
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
    public async Task ConfiguredActionsNone_ShortCircuits()
    {
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.None).Options);

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

        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All).Options);

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

        await using var ctx = new TestEfContext(NewOptions(DddAggregateActions.All).Options);

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

        await commit.Should().ThrowAsync<Exception>("Different Tenants").WithMessage(Interceptor.DifferentTenants);
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

    [Fact]
    public async Task MultipleAggregateInterfaces_Throws()
    {
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions().Options);

        ctx.Add(new TestEntityAB());

        var act = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await act.Should()
                 .ThrowAsync<InvalidOperationException>()
                 .WithMessage(string.Format(Interceptor.DddErrorMessageHasMoreThanOneAggregate, nameof(TestEntityAB)));
    }

    [Fact]
    public async Task NotAllowedAggregateRoot_Throws()
    {
        ResetAll();

        await using var ctx = new TestEfContext(NewOptions().Options);

        // Allow only RootA
        ctx.AllowAggregateRoots(typeof(RootA));

        // Add entity belonging to RootB only
        ctx.Add(new TestEntityB());

        var act = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await act.Should()
                 .ThrowAsync<InvalidOperationException>()
                 .WithMessage(string.Format(Interceptor.DddErrorMessageViolationOfAggregateBoundary2, nameof(RootB)));
    }

    // ---- CheckAggregateBoundary focused tests (using rootless TestEntityBase) ----

    [Fact]
    public async Task RootlessEntity_NoAllowedRoots_ThrowsViolation3()
    {
        ResetAll();
        await using var ctx = new TestEfContext(NewOptions().Options);

        ctx.Add(new TestEntity()); // no IAggregate<>

        var act = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await act.Should()
                 .ThrowAsync<InvalidOperationException>()
                 .WithMessage(string.Format(Interceptor.DddErrorMessageViolationOfAggregateBoundary2, nameof(NoRoot)));
    }

    [Fact]
    public async Task RootlessEntity_AllowedNoRoot_Succeeds()
    {
        ResetAll();
        await using var ctx = new TestEfContext(NewOptions().Options);

        ctx.AllowAggregateRoots(typeof(NoRoot));

        ctx.Add(new TestEntity());

        var act = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task RootA_Then_Rootless_NoRootNotAllowed_ThrowsViolation3()
    {
        ResetAll();
        await using var ctx = new TestEfContext(NewOptions().Options);

        ctx.Add(new TestEntityA());
        ctx.Add(new TestEntity()); // rootless

        var act = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await act.Should()
                 .ThrowAsync<InvalidOperationException>()
                 .WithMessage(string.Format(Interceptor.DddErrorMessageViolationOfAggregateBoundary2, nameof(NoRoot)));
    }

    [Fact]
    public async Task RootlessAllowed_Then_RootA_OnlyNoRootAllowed_ThrowsViolation2()
    {
        ResetAll();
        await using var ctx = new TestEfContext(NewOptions().Options);

        ctx.AllowAggregateRoots(typeof(NoRoot), typeof(RootA));

        ctx.Add(new TestEntity());      // allowed as NoRoot
        ctx.Add(new TestEntityA());     // RootA also allowed

        var act = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task RootA_Then_RootB_BothAllowed_Succeeds()
    {
        ResetAll();
        await using var ctx = new TestEfContext(NewOptions().Options);

        ctx.AllowAggregateRoots(typeof(RootA), typeof(RootB));

        ctx.Add(new TestEntityA());
        ctx.Add(new TestEntityB());

        var act = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task RootB_First_RootA_AllowedOnlyRootA_ThrowsViolation2()
    {
        ResetAll();
        await using var ctx = new TestEfContext(NewOptions().Options);

        ctx.AllowAggregateRoots(typeof(RootA));

        ctx.Add(new TestEntityB());

        var act = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await act.Should()
                 .ThrowAsync<InvalidOperationException>()
                 .WithMessage(string.Format(Interceptor.DddErrorMessageViolationOfAggregateBoundary2, nameof(RootB)));
    }

    [Fact]
    public async Task RootB_First_RootA_BothAllowed_Succeeds()
    {
        ResetAll();
        await using var ctx = new TestEfContext(NewOptions().Options);

        ctx.AllowAggregateRoots(typeof(RootA), typeof(RootB));

        ctx.Add(new TestEntityB());
        ctx.Add(new TestEntityA());

        var act = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task TwoRootlessEntities_NoRootAllowed_Succeeds()
    {
        ResetAll();
        await using var ctx = new TestEfContext(NewOptions().Options);

        ctx.AllowAggregateRoots(typeof(NoRoot));

        ctx.Add(new TestEntity());
        ctx.Add(new TestEntity());

        var act = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task RootA_Then_Rootless_BothAllowed_Succeeds()
    {
        ResetAll();
        await using var ctx = new TestEfContext(NewOptions().Options);

        ctx.AllowAggregateRoots(typeof(NoRoot), typeof(RootA));

        ctx.Add(new TestEntityA());
        ctx.Add(new TestEntity()); // rootless

        var act = async () => await ctx.CommitAsync(TestContext.Current.CancellationToken);

        await act.Should().NotThrowAsync();
    }
}