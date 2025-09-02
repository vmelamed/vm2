# Q&A on ID types and mapping

## Can EF Core deal with strongly typed Id-s? As in `Label` and `LabelId`.

Yes. EF Core (incl. v9 previews) can work seamlessly with strongly‑typed IDs like your readonly record struct `LabelId`. You just need to tell EF how to persist and materialize it. Below are the main approaches, trade‑offs, and concrete configuration examples.

### Your Type Is Already a Good Value Object

`public readonly record struct LabelId(in `Ulid` Id)` gives:

- Value equality (record struct) → perfect for keys.
- Immutable by design.
- Small (just wraps a `Ulid`).

EF Core can’t infer how to map a custom struct automatically unless:

- The underlying type (`Ulid`) has a built‑in provider mapping, and
- EF can decompose the struct (it usually doesn’t for arbitrary value objects).

So: add a ValueConverter (or global convention) once.

### Storage Options for `Ulid`

   Choose one:

   | Storage | Pros | Cons |
   |---------|------|------|
   | BINARY(16) / varbinary(16) | Compact, ordered, efficient | Needs custom conversion if you currently serialize ULID as string |
   | CHAR(26) (canonical ULID text) | Human readable, log/search friendly | Slightly larger, collation considerations |
   | UNIQUEIDENTIFIER (map to Guid via round-trip) | Native tooling support | You lose ULID lexicographical ordering unless you introduce custom mapping |

   If you rely on ULID’s sortable nature → prefer binary or text (not Guid).

### Minimal Per‑Property Configuration

If you only use `LabelId` sparingly:

```c#
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using vm2.Repository.TestDomain;

internal sealed class LabelConfiguration : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasConversion(
                id => id.Id,                // to provider (Ulid)
                value => new LabelId(value) // from provider
            )
            .ValueGeneratedNever();         // ULIDs are app-generated

        // Optional: store as char(26)
        builder.Property(l => l.Id)
            .HasMaxLength(26)
            .IsFixedLength();

        // other properties…
    }
}
```

If EF already knows how to map `Ulid` → no extra converter for that inner step; if not, add one more level (`Ulid` <-> bytes/string).

### Global Convention (Recommended)
Avoid repeating the conversion everywhere:

```c#
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using vm2.Repository.TestDomain;

public partial class EfRepository
{
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Convert LabelId <-> Ulid (and let Ulid map naturally) OR convert directly to string
        configurationBuilder.Properties<LabelId>()
            .HaveConversion(new ValueConverter<LabelId, Ulid>(
                id => id.Id,
                value => new LabelId(value)))
            .HaveMaxLength(26); // if you map Ulid to canonical string.

        base.ConfigureConventions(configurationBuilder);
    }
}
```

If you want direct string storage (canonical ULID 26 chars):

```c#
configurationBuilder.Properties<LabelId>()
    .HaveConversion(
        id => id.Id.ToString(),          // provider value
        s  => new LabelId(Ulid.Parse(s)) // materialize
    )
    .HaveMaxLength(26)
    .AreUnicode(false);
```

For binary (compact):

```c#
configurationBuilder.Properties<LabelId>()
    .HaveConversion(
        id => id.Id.ToByteArray(),          // Ulid -> 16 bytes
        bytes => new LabelId(new Ulid(bytes))
    )
    .HaveMaxLength(16)
    .AreUnicode(false);
```

### Query Usage

After configuring a conversion, all LINQ just works:

```c#
var id = new LabelId(Ulid.NewUlid());

var label = await context.Labels.FindAsync(id); // works if PK
var q = await context.Labels
    .Where(l => l.Id == id)
    .ToListAsync();
```

Comparisons translate because EF rewrites l.Id == id through the converter to the underlying provider type.

### Migrations

Your migration will reflect the provider type you chose.
Example (string canonical):

```c#
migrationBuilder.CreateTable(
    name: "Labels",
    columns: table => new
    {
        Id = table.Column<string>(type: "char(26)", fixedLength: true, nullable: false),
        Name = table.Column<string>(maxLength: 200, nullable: false)
    },
    constraints: table => { table.PrimaryKey("PK_Labels", x => x.Id); });
```

Binary:

```c#
Id = table.Column<byte[]>(type: "binary(16)", nullable: false)
```

### Seeding

You can seed with strongly typed IDs directly:

```c#
builder.HasData(new Label { Id = new LabelId(Ulid.Parse("01H...")), Name = "System" });
```

EF will convert during migration generation.

### Serialization / APIs

If you expose IDs externally:

- Keep LabelId as a single `Ulid` JSON value via a custom converter (System.Text.Json).
- Example:

  ```c#
  public sealed class LabelIdJsonConverter : JsonConverter<LabelId>
  {
      public override LabelId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
          => new LabelId(Ulid.Parse(reader.GetString()!));

      public override void Write(Utf8JsonWriter writer, LabelId value, JsonSerializerOptions options)
          => writer.WriteStringValue(value.Id.ToString());
  }
  ```

Register: `options.Converters.Add(new LabelIdJsonConverter());`

### Gotchas / Tips

| Concern | Guidance |
|---------|----------|
| Performance | record struct (16 bytes) is fine; avoid making it large. |
| Immutability | Keep it immutable; EF only sets through the property with converter. |
| Shadow Keys | Disable value generation for externally supplied ULIDs. |
| Sorting | If you rely on chronological order, canonical string or ULID lex ordering (not Guid). |
| Collections | Works in owned types / aggregates the same way. |
| DddInterceptor | Its checks (aggregates, audit) see LabelId as just another property—no changes needed. |

### Optional: Generic Strongly-Typed ID Pattern

If you adopt many typed IDs:

```c#
public interface IStrongId<TPrimitive> where TPrimitive : notnull
{
    TPrimitive Value { get; }
}

public readonly record struct OrderId(Guid Value) : IStrongId<Guid>;

public sealed class StrongIdConverter<TStrong, TPrimitive> : ValueConverter<TStrong, TPrimitive>
    where TStrong : struct, IStrongId<TPrimitive>
    where TPrimitive : notnull
{
    public StrongIdConverter()
        : base(
            v => v.Value,
            p => (TStrong)Activator.CreateInstance(typeof(TStrong), p)!)
    {}
}
```

Global convention:

```c#
configurationBuilder
    .Properties()
    .Where(p => typeof(IStrongId<>).IsAssignableFrom(p.ClrType.GetInterfaces().FirstOrDefault()?.GetGenericTypeDefinition()))
    .HaveConversion(typeof(StrongIdConverter<,>).MakeGenericType(p.ClrType, p.ClrType.GetProperty("Value")!.PropertyType));
```

(Adjust reflection logic to taste.)

### Putting It All Together (Example Entity + Config)

```c#
namespace vm2.Repository.EntityFramework.Models;

public sealed class Label : IAggregate<Label>, IAuditable
{
    public LabelId Id { get; private set; }
    public string Name { get; private set; } = "";
    // audit fields…

    private Label() { } // EF
    public Label(LabelId id, string name)
    {
        Id = id;
        Name = name;
    }

    public void AuditOnAdd(DateTime at, string actor) { /* ... */ }
    public void AuditOnUpdate(DateTime at, string actor) { /* ... */ }
}
```

```c#
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using vm2.Repository.EntityFramework.Models;
using vm2.Repository.TestDomain;

internal sealed class LabelConfiguration : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
            .HasConversion(
                id => id.Id.ToString(),         // store as char(26)
                s  => new LabelId(Ulid.Parse(s))
            )
            .HasMaxLength(26)
            .IsFixedLength()
            .ValueGeneratedNever();

        builder.Property(l => l.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
```

### Summary

EF Core absolutely supports strongly typed IDs:

- Use a ValueConverter (per property or global).
- Decide on storage representation (string/binary).
- Mark ValueGeneratedNever() if you generate IDs yourself.
- After that: .FindAsync(labelId), comparisons, joins and tracking all work transparently.

Need a JSON converter, analyzer, or global multi‑ID convention sample fleshed out further? Ask.

## How about a composite Id (PK) like this: public readonly record struct LabelId(in `Ulid` Id, in Guid PartitionId)

Yes, you can model a composite primary key as a single strongly‑typed value object like:

```c#
public readonly record struct LabelId(`Ulid` Id, Guid PartitionId) : IFindable
```

EF Core cannot split a value via a ValueConverter into multiple columns. For a composite value object you must map it as an owned / complex type and declare the PK over its inner scalar members.
Below are the practical patterns, trade‑offs, and code.
---
1. Define the composite ID

```c#
namespace vm2.Repository.TestDomain;

public readonly record struct LabelId(`Ulid` Id, Guid PartitionId) : IFindable
{
    public IEnumerable<object?>? KeyValues
    {
        get { yield return Id; yield return PartitionId; }
    }

    public ValueTask ValidateFindableAsync(object? _ = null, CancellationToken __ = default)
        => (Id != default && PartitionId != Guid.Empty)
            ? ValueTask.CompletedTask
            : throw new ValidationException("Both Id and PartitionId must be non-empty.");
}
```

Important: Order of yielded values must match the order you declare the primary key.
---
2. Use it in the entity

```c#
public sealed class Label : IAggregate<Label>, IAuditable, IFindable<Label>
{
    public LabelId Id { get; private set; }
    public string Name { get; private set; } = "";

    // IFindable<Label>
    public static Expression<Func<Label, object?>> KeyExpression
        => l => new { l.Id.Id, l.Id.PartitionId };

    // audit methods...
}
```
You can still keep IFindable only on the entity (cleaner), and optionally drop it from the ID type if not used directly.
---
3. EF Core mapping (owned / complex type) + composite PK
EF Core 7+ (incl. 9) can treat a value object with multiple scalar properties as a complex/owned type.

```c#
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using vm2.Repository.TestDomain;

internal sealed class LabelConfiguration : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        // Flatten the value object into two columns:
        builder.OwnsOne(l => l.Id, id =>
        {
            id.Property(p => p.Id)
                .HasColumnName("Id")
                .IsRequired();

            id.Property(p => p.PartitionId)
                .HasColumnName("PartitionId")
                .IsRequired();
        });

        // Primary key over the inner scalar members
        builder.HasKey(l => new { l.Id.Id, l.Id.PartitionId });

        builder.Property(l => l.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
```

If you prefer the new complex type API (EF 8+), you can define:

```c#
builder.ComplexProperty(l => l.Id);
builder.HasKey(l => new { l.Id.Id, l.Id.PartitionId });
```

(Owned vs complex: owned entity types can have navigation/own collections; complex types are strictly value aggregates. Choose based on your EF version features.)
---
4. Global convention (optional)
If you have many `<Something>Id(`Ulid`, Guid)` composites:

```c#
protected override void ConfigureConventions(ModelConfigurationBuilder builder)
{
    builder.ComplexProperties<LabelId>(); // or .OwnsOne for older versions manually per entity
    base.ConfigureConventions(builder);
}
```

You still must declare each composite PK explicitly because EF cannot infer that Label.Id.* is the key unless you specify it.
---
5. Queries
LINQ naturally translates:

```c#
var key = new LabelId(someUlid, partition);
var label = await ctx.Set<Label>()
    .SingleAsync(l => l.Id == key); // EF rewrites to (Id == key.Id AND PartitionId == key.PartitionId)
```

Value equality of the record struct lets EF decompose the comparison.
---
6. Repository / Find patterns
If your repository expects IFindable:

```c#
var found = await repository.FindAsync<Label>(new Label { Id = new LabelId(u, p) }, ct);
// or if you expose a static factory: Label.ByIds(u, p)
```

Or pass the KeyValues from a LabelId instance if you keep IFindable on it.
---
7. Migration result
Creates two PK columns:

```c#
CREATE TABLE Labels (
    Id CHAR(26)     NOT NULL,        -- if `Ulid` mapped as string
    PartitionId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    CONSTRAINT PK_Labels PRIMARY KEY (Id, PartitionId)
);
```

Adjust `Ulid` storage (string / binary(16)) per earlier guidance.
---
8. When NOT to wrap composite key in a single struct
Use two primitive properties directly if:
- You frequently filter by only one component and want direct property access patterns.
- The pair is not a ubiquitous domain concept.
- EF mapping complexity outweighs semantic value.
Otherwise wrapping gives:
- Single semantic concept
- Centralized validation
- Fewer parameter lists / mistakes
---
9. Summary of key points
- A composite value object must be mapped via owned/complex type; a ValueConverter cannot split into multiple columns.
- Declare the PK explicitly with the inner members.
- Ensure KeyValues (and KeyExpression if used) preserve order.
- Record struct equality enables natural LINQ translation.
- Keep it immutable and validate early.
Need an example using binary ULID + GUID partition or integrating with your `IFindable<TEntity>` default implementation? Ask and I’ll expand.

# I need it for say Azure Cosmos DB or Mongo and it is almost always needed. Does it still make sense?

Short answer: For Azure Cosmos DB (with EF Core provider) and MongoDB a single composite value-object primary key (e.g. LabelId(`Ulid` Id, Guid PartitionId)) is usually not the best persistence shape, even though it is fine (and nice) for purely relational EF Core. You can still keep the value object for domain clarity, but persist the two components separately.
Why it differs per store:
Azure Cosmos DB (EF Core provider):
- Cosmos requires a single logical id plus (optionally) a separate partition key path.
- EF Core’s Cosmos provider does not support a true composite primary key the way relational providers do; it serializes the entity to JSON with a single "id" plus a configured partition key.
- You can map a nested/owned (complex) type and point the partition key at a nested property, but you still have only one actual id. A composite .HasKey with two properties is ignored/not supported by the Cosmos provider. Implication: Model Id and PartitionId as separate properties for the container. Your domain value object can wrap them, but don’t rely on EF to treat the composite as a single PK.
MongoDB (native driver):
- Mongo documents have a single _id. Sharding keys (if used) or additional keys are separate indexed fields.
- Combining shard/partition key + id into one struct and storing it as _id removes native routing/index flexibility unless you deliberately embed a compound BSON document (which is rarely beneficial). Implication: Keep _id (e.g. ULID) distinct, keep partition / tenant / shard key as a separate field. Use a domain value object only at the domain layer if it improves invariants.
Recommended pattern (shared domain, provider‑friendly persistence):
Domain value object:

```c#
public readonly record struct LabelId(`Ulid` Id, Guid PartitionId)
{
    public static LabelId New(Guid partition) => new(`Ulid`.NewUlid(), partition);
    public override string ToString() => $"{PartitionId}:{Id}";
}
```

Entity (Cosmos / Mongo friendly):

```c#
public sealed class Label : IAggregate<Label>, IFindable<Label>
{
    public `Ulid` Id { get; private set; }              // maps to "id" (Cosmos) or "_id" (Mongo via custom BsonType if desired)
    public Guid PartitionId { get; private set; }     // maps to partition/shard key column/JSON field

    // Domain convenience
    public LabelId LabelKey => new(Id, PartitionId);

    private Label() {} // EF / deserializer
    public Label(LabelId key, string name)
    {
        Id = key.Id;
        PartitionId = key.PartitionId;
        Name = name;
    }

    public string Name { get; private set; } = "";

    static Expression<Func<Label, object?>> IFindable<Label>.KeyExpression
        => l => new { l.Id, l.PartitionId };
}
```

EF Core (Cosmos) configuration:

```c#
builder.HasKey(l => l.Id); // only the logical id
builder.HasPartitionKey(l => l.PartitionId);
builder.Property(l => l.Id)
       .ToJsonProperty("id");              // ensure correct property name for Cosmos
builder.Property(l => l.PartitionId)
       .ToJsonProperty("partitionId");
```

(If relational too, add builder.HasKey(l => new { l.Id, l.PartitionId }); conditionally only for relational providers.)
If you insist on mapping the composite value object directly:
- Relational: Use OwnsOne(l => l.LabelKey, ...) and HasKey(l => new { l.LabelKey.Id, l.LabelKey.PartitionId }).
- Cosmos: You still must expose a single property as “id”; better not to wrap for the physical model.
Pros of keeping the value object (domain layer):
- Centralized construction + invariant (non-empty both).
- Less parameter noise across services/commands.
- Easier repository FindAsync(LabelId key) helpers.
Cons (Cosmos/Mongo):
- Direct mapping of the composite as a single PK not supported.
- You’ll still expose raw scalar properties for the provider; duplicating state if you try to persist the composite.
Pragmatic compromise:
- Keep LabelId purely as a convenience type (factory + semantics).
- Persist scalars; project to/from LabelId where needed (command handler, API DTO, repository methods).
- Optionally add an implicit conversion tuple: public static implicit operator (`Ulid` Id, Guid PartitionId)(LabelId x) => (x.Id, x.PartitionId);
When to skip the composite value object:
- If the pair is only ever used at persistence boundary and not a ubiquitous domain concept.
- If performance micro-optimizations around struct copies matter (rare).
When it clearly makes sense:
- You routinely pass both together across layers (commands, events).
- Your invariants require coordinated validation (e.g. partition must belong to a tenant subset tied to the ULID prefix).
Summary:
- Cosmos/Mongo: keep separate physical properties; use a value object only for domain ergonomics.
- Relational: you may map the composite as owned + composite key.
- Don’t force EF Cosmos into unsupported composite PK semantics—it won’t add value.
If you want a conditional configuration helper that picks relational vs Cosmos automatically, ask and I can generate it.

# Say the entities are tenanted. The tenants are of varying sizes: some may have Nx100 objects but others Nx1000000. Does the TenantId make a good partition key? If not, suggest a better way to partition the DB.

Short answer: A raw TenantId is usually fine only when tenants are roughly similar in size and traffic. With highly skewed tenants (some 10² objects, some 10⁶+), using TenantId alone as the partition/shard key (Cosmos logical partition key or Mongo shard key) creates hot partitions and hits size / throughput limits early.

Below are viable strategies and when to apply them.

---

Why pure TenantId can be bad

Cosmos DB:
- Logical partition size limit (check current limit; historically 20 GB).
- RU/s per logical partition caps out; a “whale” tenant monopolizes throughput.
- Hot logical partition → throttling (429s) while other partitions idle.
MongoDB:
- Unbalanced chunks → uneven distribution; balancer overhead.
- Range-based shard key on TenantId causes hotspot if one tenant is most active.
---
Strategy Options
1. Tiered Multi-Tenancy (Pool + Dedicated)
- Small/medium tenants: shared container/collection.
- Large (“whale”) tenants: isolated container / collection (or even account) with TenantId partition key.
- Pros: Simplicity for 90%, isolation for whales.
- Cons: Operational branching (need routing layer).
2. Bucketed Tenant Partition Key (Synthetic Key)
Partition key = $"{tenantId}:{bucket}"
- bucket = hash(objectId) % N (N determined per tenant size tier; small tenants N=1).
- Stores an additional field for bucket; queries by tenant require IN over that tenant’s bucket set (client-side fan-out).
- Pros: Spreads load for large tenants while keeping tenant-local fan-out bounded.
- Cons: Query complexity (need to query all buckets for full tenant scans); cannot change N retroactively without data migration.
3. Hashed TenantId (Mongo: hashed shard key; Cosmos: pre-hash field)
- Partition key = Hash(TenantId) (stable hash).
- Pros: Uniform distribution with zero extra per-tenant logic.
- Cons: Query by tenant must compute hash (fine) but all of a tenant’s data still ends up in exactly one logical partition (so it does NOT solve whale tenants in Cosmos). Good for Mongo (hashed splits across chunks over time) but not Cosmos (hash value still a single exact key). => Only use for Mongo when tenants vary; not helpful for Cosmos scale-out of a single tenant.
4. ObjectId / ULID Based Partition Key
- Partition key = ULID (or prefix slice) while embedding TenantId inside the document.
- Pros: Excellent distribution automatically (write scaling).
- Cons: All tenant-scoped queries become cross-partition (Cosmos fan-out) or cluster scatter (Mongo) increasing RU / latency.
- Use only if per-tenant queries are rare vs object-by-id lookups.
5. Time-Sliced Tenant Buckets
- Partition key = $"{tenantId}:{yyyyMM}" (or week/day).
- Pros: Natural data lifecycle (TTL by slice), spreads load over time if workload is time-series heavy.
- Cons: Queries spanning time ranges require multiple partition keys. Large tenant within a single period may still hot-spot.
6. Hybrid: TenantId + Modulo Shard + Time
- partitionKey = $"{tenantId}:{shard}:{yyyyMM}"
- Overkill unless extreme scale and time-windowed heavy writes.
7. Separate “Index/Aggregate” Container
- Keep write-optimized bucketed storage.
- Maintain (eventually consistent) per-tenant metadata (counts, summaries) in another container keyed purely by tenant for quick dashboards.

---


Decision Matrix (Cosmos focus)

| Dominant Query Pattern | Recommended Partition Key Strategy |
|------------------------|------------------------------------|
| Mostly per-tenant full scans / filters;
tenants uniform | TenantId |
| Heavy per-tenant writes; a few huge tenants | Bucketed tenant key (tenantId:bucket) + isolated container for largest |
| Mostly point lookups by Id across tenants | ULID (Id) as partition key + tenant field |
| Time-series / retention important | tenantId:yyyyMM (or day) |
| Extreme skew + ad-hoc queries | Hybrid: bucketed + isolate whales |

---
MongoDB Differences

- Mongo hashed shard key on TenantId helps distribute chunks for skewed tenants (better than Cosmos) but all docs of one tenant still hashed to multiple chunks only after chunk splits; if one tenant dominates inserts, it can still create hotspot windows.
- For true intra-tenant spread: compound shard key: { tenantId: 1, entityHash: 1 } where entityHash = hash(objectId) or { tenantId: 1, createdAt: 1 } for time progression. Avoid monotonically increasing second component alone (range hotspot).

---

Practical Implementation (Cosmos Example)

1.	Add fields:
- tenantId (GUID/ULID)
- bucket (int)
- partitionKey (string: $"{tenantId}:{bucket}")
2.	Choose bucket count:
- If estimatedTotalObjects < 50K → 1
- 50K–500K → 4
- 500K–5M → 16
- 5M → 32 (or isolate container)
3.	Derive bucket: bucket = (hash(objectId) & 0x7FFFFFFF) % bucketCount
4.	Configure container partition key path: /partitionKey
5.	Query for a single tenant:
- If bucketCount=1, just one key.
- Else issue N parallel point-partition queries and consolidate (or maintain a small registry telling how many buckets per tenant).
You can cache bucket count per tenant in a metadata container:

```c#
public sealed class TenantPartitionInfo
{
    public Guid TenantId { get; init; }
    public int BucketCount { get; init; }
    public DateTime EvaluatedAtUtc { get; init; }
}
```

---

Migration Path When a Tenant Grows
1.	Detect threshold breach (counts / RU patterns).
2.	Allocate new container (dedicated) OR increase bucket count (requires re-write because partition key immutable).
3.	Background copy / dual-write + cutover flag.
4.	Update routing metadata.
(The “increase bucket count in place” is not trivial; thus early correct sizing or container isolation is safer.)

---

Avoid Common Pitfalls
- Don’t concatenate large JSON blobs into the partition key (keep < 100 bytes).
- Don’t choose a partition key with ultra-low cardinality (e.g. environment or fixed SKU).
- Avoid changing the partition key later (forces full data migration).
- Don’t overshard early; each extra bucket increases query fan-out and RU cost.

---

Recommended Baseline

- Start with bucketed tenant partition key only for tenants forecasted to exceed mid-size thresholds; others use single bucket.
- Maintain a tenant metadata table specifying bucket count; derive partition key deterministically.
- Isolate mega-tenants into dedicated containers if they begin to dominate RU or storage > ~50% of shared pool.

---

Summary
TenantId alone: simple but unsafe under skew (hot partitions / size cap). Better: planned distribution strategy:
- Bucketed partition key or per-tenant containers (Cosmos).
- Compound or hashed+secondary component (Mongo). Keep the domain value object (LabelId) separate from the persistence partitioning concerns—treat partition key derivation as an infrastructure concern encapsulated in a repository / partitioning service.
