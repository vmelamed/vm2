# YABORP (Yet Another Blog On the Repository Pattern)

I am starting this blog fully aware that it is somewhat controversial topic. Naturally, I will take my position and will do my
best to defend it. At the same time, I will try to give full due to the other side too.

Persisting and accessing the state of a program has been one of the center pieces of the software architecture since...
Well, the dawn of the software, I guess. I remember all sorts of acronyms, technologies, and patterns: ODBC, OLEDB, Active
Record, Table Data Gateway, DAL, Transaction Script, Anemic Model, and on, and on, and of course the **Repository Pattern**.
One of the main problems has always been that in your program you access and modify the state of your model - the data - in one
way: through variables, arrays, lists, etc. But when it comes to large amounts of data, and especially when the data must
survive program restarts, you have to resort to some existing, preferably well established, external technology that
unfortunately has its own access patterns, paradigms, and other "peculiarities". One of the goals of all those acronyms and
names is to bridge the gap (a.k.a. the *impedance mismatch*) between the straight forward, CS 101-like, access patterns and the
chosen external data system. Along with solving the impedance mismatch, we also aim at better testability, clean domain logic,
modularity, standardized data practices throughout the organization. To summarize them all with one word: *maintainability*.
Maintainability has always been amongst the top NFRs (non-functional requirements) along with scalability, performance,
security. If your code is highly maintainable, you can not only add new features, but also address the other NFRs, right?

These days many teams, that are committed to the OOP, approach the problem by first selecting some ORM (Entity Framework,
(N)Hibernate, DAPR, etc.) and then implementing the *Repository Pattern* on top of it. And here is where I can repeat after
Inigo Montoya: "You keep saying that word, but I don't think it means what you think it means.", because I am firmly
in the Martin Fowler's [Repository Pattern camp](https://martinfowler.com/eaaCatalog/repository.html).

Let me emphasize the main points here one more time:

> - A layer that isolates domain objects from details of the database access code. A *Repository* mediates between the domain
>   and data mapping layers
> - Acting like an in-memory domain object collection
>   - objects can be added to and
>   - removed from the repository, as they can from a simple collection of objects
> - Client objects construct query specifications declaratively and submit them to Repository for satisfaction
> - The mapping code encapsulated by the Repository will carry out the appropriate operations behind the scenes

The spirit here is **domain-first**: the repository speaks the language of the model, not the database. It should feel like a
collection where the domain and the service layer can add, remove, and find aggregates and entities using expressions meaningful
to the domain. Speaking of language, LINQ is the perfect "query specification". Behind the scene it has the potential to
translate from LINQ expressions to DB language model (e.g. SQL), and then execute it in the DBMS.

Let's summarize it one more time in C# language:

```csharp
interface IRepository : IDisposable
{
    // Acting like an in-memory domain object collection. Finding entities by key:
    TEntity Find<TEntity>(params object[] keys) where TEntity : class;

    // construct query specifications declaratively and submit them to Repository for satisfaction
    IQueryable<TEntity> Set<TEntity>() where TEntity : class;

    // objects can be added to and removed from the repository, as they can from a simple collection of objects
    void Add<TEntity>(TEntity entity) where TEntity : class;
    TEntity Remove<TEntity>(TEntity entity) where TEntity : class;

    // Figures out everything that needs to be done to alter the database as a result of your work.
    // The mapping code encapsulated by the Repository will carry out the appropriate operations behind the scenes
    void Commit();
}
```

Of course the function `Set<TEntity>()` is the one that enables the query specification via `IQueryable<TEntity>`. We can
chain a bunch of late-executing LINQ functions and lambdas, e.g.:

```csharp
repository
    .Set<Person>()
    .Where(person => person.Name == command.Person.Name)
    .OrderBy(person => person.Height)
    .Select(PersonToDto)
    .ToList()
    ;
```

The LINQ functions allow the service layer to declaratively specify a query that almost reads like the English text of a
business requirement.

A repository with an interface like this lets you "map and implement" all business requirements in the domain model, "scripted"
in one place only - the service layer.

> *Personally, I really dislike SQL stored procedures and triggers. They split the business logic between the service layer and
> the database into two different languages (C# and T-SQL). Deployment-wise the business logic is also split between two
> different subsystems with completely different deployment models and very likely, done by different teams.*

Note that in the above `IRepository` we have also "tucked-in" another important concept - the *unit of work*. Adding, removing,
finding and modifying entities should reflect the requirements for the implemented business transactions. The repository must
track all changes and in the end of the transaction should **`Commit`** those changes to the DB. If even one of the changes
fails for some reason, **all changes must fail** as if nothing happened.

A repository like this allow the developer to concentrate on the domain model and on the services that orchestrate that model.
Or we can say that the thanks to the repository, the ubiquitous language translates very well to programming language.

This conception also maps well to Eric Evans' *bounded contexts*. The repository expresses the model's access patterns and
semantics very well in a bounded context: the class names of all aggregate root entities should be arguments for the type
parameters of the repository methods.

Here is an example of the alternative repository interface (the other side of the argument):

```csharp
public interface IRepository<TEntity> where TEntity : class
{
    TEntity Find(params object[] keys);
    IQueryable<TEntity> Set();
    void Add(TEntity entity);
    TEntity Remove(TEntity entity);
    void Commit();
}
```

Some may even add domain specific methods to this interface. Let's stick to the more generic form above for now.

This looks almost the same, doesn't it? Well, no it isn't. Note that the type parameter is on the interface - not on the
functions. Which means that this repository limits the orchestration of the domain model to only one type of entity. This
might be a very important, central, (God?) type of entity, undoubtedly an aggregate root. What about the other types? It looks
like we will need one repository for each aggregate root. So, if we have a service that operates on more than one aggregate
root from the bounded context, we'd have to inject more than one repository.

Don't get me wrong: this may be enough for small models or bounded contexts with a single aggregate root. One good thing here is
that we can easily enforce consistency and transaction boundaries within the aggregate bounds - the repository.
My problem is with the practicality of it. If our bounded context comes from a large domain model (banking application,
non-trivial social app, etc.) the service layer has to often bridge between different aggregate roots.

Even on a lower, DB access level we have problem to solve. Say we implement a repository in .NET with *Entity Framework* (EF).
In order to limit the transactional boundaries to one aggregate root, we have to have a `DbContext` per aggregate root too.
Hence, a DB connection per root. If we implementing many repositories over a single `DbContext` we will defeat the purpose of
this type of repository - clean consistency and strict transaction boundaries.

So for a moderately complex application with fair size of a domain model that is divided into:

- bounded contexts,
- where each context has one or more aggregate roots
- where the roots from a single context most likely reference other roots by ID or by reference

my preference is single repository per bounded context.

---

## Evans' DDD and Microservices: Bounded Contexts First

Eric Evans' Domain-Driven Design emphasizes:

- Ubiquitous language within a bounded context.
- Context maps between bounded contexts.
- Aggregates enforcing invariants and transactional boundaries inside the context.

Microservices should be cut along bounded-context seams. If you slice services by single aggregates in isolation, you risk:

- Excessive chatty cross-service calls for simple business flows.
- Leaky invariants and fragile distributed transactions.
- A fragmented ubiquitous language and duplicated concepts.

A better approach:

- Start with bounded contexts (a cohesive domain model and language).
- Within each context, use aggregates to enforce invariants and define transactional boundaries.
- Let repositories serve the domain model—sometimes broad within the context, sometimes per-aggregate for clarity—but always in service of the bounded context's language.

Meme interlude #2:

- The "This is fine" dog, sitting in a room labeled "Cross-aggregate queries inside a command handler," while the fire is named "distributed transactions."

---

## The Querying Language: LINQ as a Domain-Facing Abstraction

Fowler mentions using a querying language to work with repositories as if they were in-memory collections. In C#, that language is LINQ.

- Model access (domain view):
  - Business logic composes queries in LINQ over repositories that behave like collections.
  - The domain expresses intent: filters, projections, and ordering in ubiquitous language.
- Store access (infrastructure view):
  - The ORM (e.g., EF Core) translates those LINQ queries into SQL or provider operations.
  - Database concerns (joins, indexes, tracking) are hidden behind the repository/ORM boundary.

This distinction matters: your code should read as "accessing the model," not "accessing the database." The database is an implementation detail.

Two practical patterns for LINQ-friendly repositories:

- Expression-based methods (keeps provider hidden but allows translation):

  ```csharp
  public interface IReadOnlyRepository<T>
  {
      Task<IReadOnlyList<T>> ListAsync(
          Expression<Func<T, bool>> predicate,
          CancellationToken ct = default);
  }
  ```

- Specification/Query objects (encapsulates predicates, includes, and pagination without leaking EF specifics):

  ```csharp
  public interface ISpecification<T>
  {
      Expression<Func<T, bool>> Criteria { get; }
      // optional: ordering, paging, projections
  }

  public interface ISpecRepository<T>
  {
      Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec, CancellationToken ct = default);
  }
  ```

Guidance:

- Prefer passing expressions/specifications to repositories rather than returning `IQueryable` to the domain. This preserves LINQ as your domain query language while avoiding a hard dependency on a specific provider.
- Keep EF-only operators (e.g., `Include`) inside infrastructure; expose domain operations or specifications that imply necessary eager loading.

Example of "model access" in application code:

```csharp
var overdue = await _invoiceRepository.ListAsync(
    i => i.DueDate < now && !i.Paid,
    ct);

// Business rule follows naturally from the model, not the database.
foreach (var invoice in overdue)
{
    invoice.MarkOverdue();
}
```

---

## The C# Angle: Why Per-Aggregate Repositories Are Popular

In C#, many teams use one repository per aggregate root:

- It reinforces aggregate boundaries and local transactions.
- It's easy to test.
- It maps neatly onto application services handling commands.

Caveat (Fowler/Evans perspective):

- Don't confuse the convenience of per-aggregate repositories with the architectural boundary of a microservice. Microservices should reflect bounded contexts, which may contain multiple aggregates.
- A per-aggregate repository is an implementation detail inside the bounded context—not an automatic blueprint for a standalone service.

Where EF Core fits:

- `DbSet<TEntity>` already looks like a repository. Wrapping it is useful only when the abstraction adds domain language, testability, or storage interchangeability.
- Avoid "generic repository over EF" if it merely re-implements LINQ; prefer domain-centric contracts and, where needed, specifications or query objects.

---

## How vm2 Organizes a Pragmatic Implementation

The vm2 Repository project uses a structure that supports both Fowler-style domain boundaries and C#-friendly per-aggregate repos:

```
src/Repository
├─ DB/                 # Database-related assets (context/configuration/migrations/seeding/etc.)
├─ FakeDbSet/          # In-memory/fake support for tests
└─ Repository/
   ├─ Abstractions/    # Domain-facing interfaces and contracts
   ├─ EfRepository/    # EF Core-based implementation of the abstractions
   ├─ Repository.csproj
   └─ usings.cs
```

- Abstractions: Put technology-agnostic contracts here, expressed in the domain's language.
- EfRepository: Keep EF-specific concerns here (query shape, projection, includes, compiled queries).
- FakeDbSet: Enable fast, deterministic tests that treat repositories "like collections," per Fowler's spirit.
- DB: Centralize EF context and database wiring away from domain-facing contracts.

Note on scope:

- The C# repository in this solution is scoped to a single aggregate root by design. That fits well for command handling and invariants.
- For microservices, compose these aggregate repositories (and additional query patterns) within the bounded context of the service. Don't equate "aggregate repository" with "microservice boundary."

---

## Illustrative Examples (Shape and Intent)

These examples show the flavor of domain-centric contracts and implementations (not verbatim from vm2).

### Abstractions (domain-facing, aggregate-focused but context-aware)

```csharp
public interface IRepository<TAggregate>
    where TAggregate : class
{
    Task<TAggregate?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(TAggregate aggregate, CancellationToken ct = default);
    void Remove(TAggregate aggregate);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
}
```

A domain-specific repository should speak ubiquitous language:

```csharp
public interface ISubscriptionRepository
{
    Task<Subscription?> GetByIdAsync(SubscriptionId id, CancellationToken ct = default);
    Task AddAsync(Subscription subscription, CancellationToken ct = default);
    Task<IReadOnlyList<Subscription>> FindActiveByUserAsync(UserId userId, CancellationToken ct = default);
    void Remove(Subscription subscription);
}
```

### EF Implementation (isolated infrastructure)

```csharp
public sealed class EfSubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _db;

    public EfSubscriptionRepository(AppDbContext db) => _db = db;

    public async Task<Subscription?> GetByIdAsync(SubscriptionId id, CancellationToken ct = default)
        => await _db.Set<Subscription>().FindAsync(new object[] { id.Value }, ct);

    public async Task AddAsync(Subscription subscription, CancellationToken ct = default)
        => await _db.Set<Subscription>().AddAsync(subscription, ct);

    public async Task<IReadOnlyList<Subscription>> FindActiveByUserAsync(UserId userId, CancellationToken ct = default)
        => await _db.Set<Subscription>()
                    .Where(s => s.UserId == userId && s.IsActive)
                    .ToListAsync(ct);

    public void Remove(Subscription subscription)
        => _db.Set<Subscription>().Remove(subscription);
}
```

### In-Memory/Fake for Tests (Fowler's "collection" vibe)

```csharp
public sealed class InMemorySubscriptionRepository : ISubscriptionRepository
{
    private readonly List<Subscription> _items = new();

    public Task<Subscription?> GetByIdAsync(SubscriptionId id, CancellationToken ct = default)
        => Task.FromResult(_items.FirstOrDefault(s => s.Id == id));

    public Task AddAsync(Subscription subscription, CancellationToken ct = default)
    {
        _items.Add(subscription);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Subscription>> FindActiveByUserAsync(UserId userId, CancellationToken ct = default)
        => Task.FromResult<IReadOnlyList<Subscription>>(
            _items.Where(s => s.UserId == userId && s.IsActive).ToList()
        );

    public void Remove(Subscription subscription)
        => _items.Remove(subscription);
}
```

---

## Practical Guidance (Fowler/Evans-aligned)

- Start with bounded contexts. Let them define your service boundaries. Use aggregate roots inside that context to enforce invariants.
- Shape repositories around the ubiquitous language. If a generic repository adds no domain value, use `DbContext`/`DbSet` directly in the infrastructure layer.
- Treat LINQ as your domain-facing query language:
  - Accept expressions/specifications in repository methods.
  - Keep EF-specific operators inside infrastructure.
  - Project to DTOs/read models when crossing application boundaries.
- Prefer per-aggregate repositories for commands; use specifications, query objects, or dedicated read models for cross-aggregate queries.
- Keep transaction semantics explicit in the application layer (Unit of Work, when used, belongs in infrastructure).
- Test domain logic with in-memory/fake repositories; use integration tests for EF and database behavior.

---

## Conclusion

If you judge by the spirit of Fowler and Evans, repositories are not about generic CRUD wrappers; they are about expressing the domain's language at a boundary, ideally at the scale of a bounded context. LINQ plays the role of the querying language, letting business logic access the model while the ORM hides the physical store. Microservices should follow bounded-context seams. Per-aggregate repositories are a useful C# idiom inside that context—but don't let that pattern dictate your service boundaries.

The vm2 Repository project gives you a clean, testable foundation: abstractions, EF implementations, and in-memory support. Use them to implement aggregate-focused repositories while keeping your architectural north star aligned with bounded contexts—and keep your LINQ at the domain level, not the database level.

---

## Further Reading

- Eric Evans, "Domain-Driven Design: Tackling Complexity in the Heart of Software" (Addison-Wesley)
- Martin Fowler, "Repository" and "Patterns of Enterprise Application Architecture"
- Martin Fowler & James Lewis, "Microservices" (martinfowler.com)
- Vaughn Vernon, "Implementing Domain-Driven Design" (specifications, aggregates)
- Domain Language (Evans) resources on bounded contexts and context mapping

References (vm2):

- Abstractions: <https://github.com/vmelamed/vm2/tree/23f9c276d26457f8c08f9817463e23b8f75a5770/src/Repository/Repository/Abstractions>
- EF implementation: <https://github.com/vmelamed/vm2/tree/23f9c276d26457f8c08f9817463e23b8f75a5770/src/Repository/Repository/EfRepository>
- Project file: <https://github.com/vmelamed/vm2/blob/23f9c276d26457f8c08f9817463e23b8f75a5770/src/Repository/Repository/Repository.csproj>
- Usings: <https://github.com/vmelamed/vm2/blob/23f9c276d26457f8c08f9817463e23b8f75a5770/src/Repository/Repository/usings.cs>
- Fake/in-memory support: <https://github.com/vmelamed/vm2/tree/23f9c276d26457f8c08f9817463e23b8f75a5770/src/Repository/FakeDbSet>
- DB-related assets: <https://github.com/vmelamed/vm2/tree/23f9c276d26457f8c08f9817463e23b8f75a5770/src/Repository/DB>
