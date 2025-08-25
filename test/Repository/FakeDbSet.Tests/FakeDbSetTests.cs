namespace vm2.Repository.FakeDbSet.Tests;

public class FakeDbSetTests(
    FakeDbSetTestsFixture fixture,
    ITestOutputHelper output) : IClassFixture<FakeDbSetTestsFixture>
{
    public ITestOutputHelper Out { get; } = output;

    protected FakeDbSetTestsFixture _fixture = fixture;

    [Fact]
    public void Constructor()
    {
        var sut = new FakeDbSet<TestEntity>(e => e.Id);

        (sut.AsAsyncEnumerable() as FakeAsyncEnumerable<TestEntity>).Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Id_Is_Default_Key_Find()
    {
        var sut = new FakeDbSet<TestEntity>(e => e.Id);
        var te = new TestEntity("", id: Guid.NewGuid());
        sut.Add(te);
        sut.Find(te.Id).Should().Be(te);
    }

    [Fact]
    public void Id_zone_Are_Keys_Find()
    {
        var sut = new FakeDbSet<TestEntity>(e => e.Id);
        var te = new TestEntity("", id: Guid.NewGuid());
        sut.Add(te);
        sut.Find(te.Id).Should().Be(te);
    }

    [Fact]
    public async Task Id_Is_Default_KeyArray_FindAsync()
    {
        var sut = new FakeDbSet<TestEntity>(e => e.Id);
        var te = new TestEntity("", id: Guid.NewGuid(), zone: Guid.NewGuid());

        var add = async () => await sut.AddAsync(te);

        await add.Should().NotThrowAsync();

        var find = async () => await sut.FindAsync([te.Id], CancellationToken.None);

        (await find.Should().NotThrowAsync()).Which.Should().Be(te);
    }

    [Fact]
    public async Task Id_zone_Are_KeysArray_FindAsync()
    {
        var sut = new FakeDbSet<TestEntity>(te => te.Id, te => te.Zone);
        var te = new TestEntity("", id: Guid.NewGuid(), zone: Guid.NewGuid());

        var add = async () => await sut.AddAsync(te);

        await add.Should().NotThrowAsync();

        var find = async () => await sut.FindAsync([te.Id, te.Zone], CancellationToken.None);

        (await find.Should().NotThrowAsync()).Which.Should().Be(te);
    }

    [Fact]
    public async Task Id_zone_Are_NewObject_FindAsync()
    {
        var sut = new FakeDbSet<TestEntity>(te => new { te.Id, te.Zone });
        var te = new TestEntity("", id: Guid.NewGuid(), zone: Guid.NewGuid());

        var add = async () => await sut.AddAsync(te);

        await add.Should().NotThrowAsync();

        var find = async () => await sut.FindAsync([te.Id, te.Zone], CancellationToken.None);

        (await find.Should().NotThrowAsync()).Which.Should().Be(te);
    }

    [Fact]
    public async Task Id_Is_Default_Key_FindAsync()
    {
        var sut = new FakeDbSet<TestEntity>(e => e.Id);
        var te = new TestEntity("", id: Guid.NewGuid(), zone: Guid.NewGuid());

        var add = async () => await sut.AddAsync(te);

        await add.Should().NotThrowAsync();

        var find = async () => await sut.FindAsync(te.Id);

        (await find.Should().NotThrowAsync()).Which.Should().Be(te);
    }

    [Fact]
    public async Task Id_zone_Are_Keys_FindAsync()
    {
        var sut = new FakeDbSet<TestEntity>(te => te.Id, te => te.Zone) as DbSet<TestEntity>;
        var te = new TestEntity("", id: Guid.NewGuid(), zone: Guid.NewGuid());

        var add = async () => await sut.AddAsync(te);

        await add.Should().NotThrowAsync();

        var find = async () => await sut.FindAsync(te.Id, te.Zone);

        (await find.Should().NotThrowAsync()).Which.Should().Be(te);
    }

    [Fact]
    public async Task AsEnumerable_TestAsync()
    {
        var num = new string[] { "one", "two", "three" };
        var tes = new TestEntity[] { new(num[0], id: Guid.NewGuid()), new(num[1], id: Guid.NewGuid()), new(num[2], id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>([tes[0], tes[1], tes[2],]);
        var i = 0;

        await foreach (var te in sut)
            te.Name.Should().Be(num[i++]);
    }

    [Fact]
    public async Task AsQueryable_TestAsync()
    {
        var num = new string[] { "one", "two", "three" };
        var tes = new TestEntity[] { new(num[0], id: Guid.NewGuid()), new(num[1], id: Guid.NewGuid()), new(num[2], id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>([tes[0], tes[1], tes[2]]);
        var te = sut.AsQueryable().First(t => t.Name == "two");

        te.Name.Should().Be("two");

        te = await sut.AsQueryable().FirstAsync(t => t.Name == "three", TestContext.Current.CancellationToken);

        te.Name.Should().Be("three");
    }

    [Fact]
    public void AddRange_Test()
    {
        var num = new string[] { "one", "two", "three" };
        var tes = new TestEntity[] { new(num[0], id: Guid.NewGuid()), new(num[1], id: Guid.NewGuid()), new(num[2], id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>([tes[0]]);

        sut.AddRange(tes[0], tes[1], tes[2]);

        sut.Count().Should().Be(3);
        sut.First(t => t.Name == "two").Name.Should().Be("two");
        sut.First(t => t.Name == "three").Name.Should().Be("three");
    }

    [Fact]
    public void AddEnum_Test()
    {
        var num = new string[] { "one", "two", "three" };
        var tes = new TestEntity[] { new(num[0], id: Guid.NewGuid()), new(num[1], id: Guid.NewGuid()), new(num[2], id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>([tes[0]]);

        sut.AddRange(new List<TestEntity> { tes[0], tes[1], tes[2] });

        sut.Count().Should().Be(3);
        sut.First(t => t.Name == "two").Name.Should().Be("two");
        sut.First(t => t.Name == "three").Name.Should().Be("three");
    }

    [Fact]
    public async Task AddRange_TestAsync()
    {
        var num = new string[] { "one", "two", "three" };
        var tes = new TestEntity[] { new(num[0], id: Guid.NewGuid()), new(num[1], id: Guid.NewGuid()), new(num[2], id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>([tes[0]]);

        await sut.AddRangeAsync(tes[0], tes[1], tes[2]);

        sut.Count().Should().Be(3);
        sut.First(t => t.Name == "two").Name.Should().Be("two");
        sut.First(t => t.Name == "three").Name.Should().Be("three");
    }

    [Fact]
    public async Task AddEnum_TestAsync()
    {
        var num = new string[] { "one", "two", "three" };
        var tes = new TestEntity[] { new(num[0], id: Guid.NewGuid()), new(num[1], id: Guid.NewGuid()), new(num[2], id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>([tes[0]]);

        await sut.AddRangeAsync(new List<TestEntity> { tes[0], tes[1], tes[2] }, TestContext.Current.CancellationToken);

        sut.Count().Should().Be(3);
        sut.First(t => t.Name == "two").Name.Should().Be("two");
        sut.First(t => t.Name == "three").Name.Should().Be("three");
    }

    [Fact]
    public void AttachRange_Test()
    {
        var num = new string[] { "one", "two", "three" };
        var tes = new TestEntity[] { new(num[0], id: Guid.NewGuid()), new(num[1], id: Guid.NewGuid()), new(num[2], id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>([tes[0]]);

        sut.AttachRange(tes[0], tes[1], tes[2]);

        sut.Count().Should().Be(3);
        sut.First(t => t.Name == "two").Name.Should().Be("two");
        sut.First(t => t.Name == "three").Name.Should().Be("three");
    }

    [Fact]
    public void AttachEnum_Test()
    {
        var num = new string[] { "one", "two", "three" };
        var tes = new TestEntity[] { new(num[0], id: Guid.NewGuid()), new(num[1], id: Guid.NewGuid()), new(num[2], id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>([tes[0]]);

        sut.AttachRange(new List<TestEntity> { tes[0], tes[1], tes[2] });

        sut.First(t => t.Name == "two").Name.Should().Be("two");
        sut.First(t => t.Name == "three").Name.Should().Be("three");
    }

    [Fact]
    public void RemoveRange_Test()
    {
        var num = new string[] { "one", "two", "three" };
        var tes = new TestEntity[] { new(num[0], id: Guid.NewGuid()), new(num[1], id: Guid.NewGuid()), new(num[2], id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>(tes);

        sut.RemoveRange(tes[1], tes[2], new("four", id: Guid.NewGuid()));
        sut.Count().Should().Be(1);
    }

    [Fact]
    public void RemoveEnum_Test()
    {
        var num = new string[] { "one", "two", "three" };
        var tes = new TestEntity[] { new(num[0], id: Guid.NewGuid()), new(num[1], id: Guid.NewGuid()), new(num[2], id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>(tes);

        sut.RemoveRange(new List<TestEntity> { tes[1], tes[2], new("four", id: Guid.NewGuid()) });
        sut.Count().Should().Be(1);
    }

    [Fact]
    public void UpdateRange_Test()
    {
        var num = new string[] { "one", "two", "three" };
        var tes = new TestEntity[] { new(num[0], id: Guid.NewGuid()), new(num[1], id: Guid.NewGuid()), new(num[2], id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>([tes[0]]);

        sut.UpdateRange(tes[0], tes[1], tes[2]);
        sut.Count().Should().Be(3);
    }

    [Fact]
    public void UpdateEnum_Test()
    {
        var num = new string[] { "one", "two", "three" };
        var tes = new TestEntity[] { new(num[0], id: Guid.NewGuid()), new(num[1], id: Guid.NewGuid()), new(num[2], id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>([tes[0]]);

        sut.UpdateRange(new List<TestEntity> { tes[0], tes[1], tes[2] });
        sut.Count().Should().Be(3);
    }

    [Fact]
    public void Add_DuplicateEntity_ShouldNotDuplicate()
    {
        var id = Guid.NewGuid();
        var te = new TestEntity("dup", id: id);
        var sut = new FakeDbSet<TestEntity>(e => e.Id);

        sut.Add(te);
        sut.Add(te);

        sut.Count().Should().Be(1);
    }

    [Fact]
    public void Remove_NonexistentEntity_ShouldNotThrowOrChangeCount()
    {
        var te1 = new TestEntity("one", id: Guid.NewGuid());
        var te2 = new TestEntity("two", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>([te1]);

        var countBefore = sut.Count();
        sut.Remove(te2);
        sut.Count().Should().Be(countBefore);
    }

    [Fact]
    public void Find_MissingEntity_ReturnsNull()
    {
        var sut = new FakeDbSet<TestEntity>(e => e.Id);
        var missingId = Guid.NewGuid();

        sut.Find(missingId).Should().BeNull();
    }

#pragma warning disable xUnit1051 // Calls to methods which accept CancellationToken should use TestContext.Current.CancellationToken
    [Fact]
    public async Task FindAsync_MissingEntity_ReturnsNull()
    {
        var sut = new FakeDbSet<TestEntity>(e => e.Id);
        var missingId = Guid.NewGuid();

        var result = await sut.FindAsync(missingId);
        result.Should().BeNull();
    }
#pragma warning restore xUnit1051 // Calls to methods which accept CancellationToken should use TestContext.Current.CancellationToken

    [Fact]
    public void Update_NonexistentEntity_AddsEntity()
    {
        var te = new TestEntity("new", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>();

        sut.Update(te);
        sut.Should().Contain(te);
    }

    [Fact]
    public void Attach_NonexistentEntity_AddsEntity()
    {
        var te = new TestEntity("attach", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>();

        sut.Attach(te);
        sut.Should().Contain(te);
    }

    [Fact]
    public void Enumeration_ReflectsAddAndRemove()
    {
        var te1 = new TestEntity("one", id: Guid.NewGuid());
        var te2 = new TestEntity("two", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>();

        sut.Add(te1);
        sut.Add(te2);
        sut.Remove(te1);

        sut.Should().ContainSingle().Which.Should().Be(te2);
    }

    [Fact]
    public void Enumeration_ReflectsAttachAndRemove()
    {
        var te1 = new TestEntity("one", id: Guid.NewGuid());
        var te2 = new TestEntity("two", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>();

        sut.Attach(te1);
        sut.Attach(te2);
        sut.Remove(te1);

        sut.Should().ContainSingle().Which.Should().Be(te2);
    }

    [Fact]
    public void Find_WrongKeyCount_ThrowsArgumentException2()
    {
        var sut = new FakeDbSet<TestEntity>(TestEntity.KeyExpression);
        var id = Guid.NewGuid();

        // Only one key provided, but two are required
        Action act = () => sut.Find(id);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Find_WrongKeyCount_ThrowsArgumentException()
    {
        var sut = new FakeDbSet<TestEntity>(e => e.Id, e => e.Zone);
        var id = Guid.NewGuid();

        // Only one key provided, but two are required
        Action act = () => sut.Find(id);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public async Task AddRange_EmptyInput_DoesNothing()
    {
        var sut = new FakeDbSet<TestEntity>();
        sut.AddRange([]);
        sut.Should().BeEmpty();

        await sut.AddRangeAsync([]);
        sut.Should().BeEmpty();
    }

    [Fact]
    public void RemoveRange_EmptyInput_DoesNothing()
    {
        var te = new TestEntity("one", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>([te]);

        sut.RemoveRange([]);

        sut.Should().ContainSingle().Which.Should().Be(te);
    }

    [Fact]
    public void Attach_SameEntityTwice_NoDuplicate()
    {
        var te = new TestEntity("one", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>();

        sut.Attach(te);
        sut.Attach(te);

        sut.Count().Should().Be(1);
    }

    [Fact]
    public void AsQueryable_ProviderAndExpression_AreNotNull()
    {
        var sut = new FakeDbSet<TestEntity>();
        var queryable = sut.AsQueryable();

        queryable.Provider.Should().NotBeNull();
        queryable.Expression.Should().NotBeNull();
    }

    [Fact]
    public void IListSource_GetList_ReturnsUnderlyingList()
    {
        var te = new TestEntity("one", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>([te]);
        var listSource = (IListSource)sut;
        var list = listSource.GetList();

        list.Contains(te).Should().BeTrue();
        listSource.ContainsListCollection.Should().BeTrue();
    }

    [Fact]
    public async Task IAsyncEnumerable_YieldsAllEntities()
    {
        var tes = new TestEntity[] { new ("one", id: Guid.NewGuid()), new ("two", id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>(tes);
        var results = new List<TestEntity>();

        await foreach (var te in sut)
            results.Add(te);

        results.Should().BeEquivalentTo(tes);
    }

    [Fact]
    public void IInfrastructure_Instance_IsNotNull()
    {
        var sut = new FakeDbSet<TestEntity>();
        var infra = (IInfrastructure<IServiceProvider>)sut;

        infra.Instance.Should().NotBeNull();
    }

    [Fact]
    public void Update_ExistingEntity_DoesNotDuplicate()
    {
        var te = new TestEntity("one", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>([te]);

        sut.Update(te);

        sut.Count().Should().Be(1);
    }

    [Fact]
    public void LinqQuery_Works()
    {
        var tes = new TestEntity[] { new ("one", id: Guid.NewGuid()), new("two", id: Guid.NewGuid()) };
        var sut = new FakeDbSet<TestEntity>(tes);

        var result = sut.Where(e => e.Name == "two").SingleOrDefault();

        result.Should().NotBeNull();
        result.Name.Should().Be("two");
    }

    [Fact]
    public void Add_ReturnsEntityEntry_WithEntity()
    {
        var te = new TestEntity("add", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>();

        var entry = sut.Add(te);

        entry.Should().NotBeNull();
        // entry.State.Should().Be(EntityState.Added);
        entry.Entity.Should().Be(te);
    }

    [Fact]
    public void Attach_ReturnsEntityEntry_WithEntity()
    {
        var te = new TestEntity("attach", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>();

        var entry = sut.Attach(te);

        entry.Should().NotBeNull();
        entry.Entity.Should().Be(te);
    }

    [Fact]
    public void Remove_ReturnsEntityEntry_WithEntity()
    {
        var te = new TestEntity("remove", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>([te]);

        var entry = sut.Remove(te);

        entry.Should().NotBeNull();
        entry.Entity.Should().Be(te);
    }

    [Fact]
    public void Update_ReturnsEntityEntry_WithEntity()
    {
        var te = new TestEntity("update", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>();

        var entry = sut.Update(te);

        entry.Should().NotBeNull();
        entry.Entity.Should().Be(te);
    }

    [Fact]
    public async Task AddAsync_ReturnsEntityEntry_WithEntity()
    {
        var te = new TestEntity("addasync", id: Guid.NewGuid());
        var sut = new FakeDbSet<TestEntity>();

        var entry = await sut.AddAsync(te, TestContext.Current.CancellationToken);

        entry.Should().NotBeNull();
        entry.Entity.Should().Be(te);
    }
}
