namespace vm2.Repository.DB.TestSQLite;

/// <summary>
/// <see cref="IRepository"/> based on Entity Framework and SQLite.
/// </summary>
public class EfSQLiteRepository : SQLiteEfRepository
{
    public string Actor { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EfSQLiteRepository"/> class using the specified database context
    /// options and actor retrieval function.
    /// </summary>
    /// <remarks>
    /// The <paramref name="getActor"/> function is typically used to associate operations with a specific user or system actor.
    /// Ensure that both <paramref name="options"/> and <paramref name="getActor"/> are valid and non-null before calling this
    /// constructor.
    /// </remarks>
    /// <param name="options">The options to configure the database context for the repository. Cannot be null.</param>
    /// <param name="getActor">A function that retrieves the name or identifier of the current actor performing operations.</param>
    public EfSQLiteRepository(
        DbContextOptions options,
        Func<string> getActor)
        : base(options)
    {
        Actor = getActor();
    }

    /// <inheritdoc/>
    protected override ValueTask CompleteAndValidateAddedEntityAsync(
        EntityEntry entry,
        DateTime now,
        string actor,
        CancellationToken ct)
    {
        UpdateOptimisticConcurrencyTag(entry.Entity);
        return base.CompleteAndValidateAddedEntityAsync(entry, now, actor, ct);
    }

    /// <inheritdoc/>
    protected override ValueTask CompleteAndValidateUpdatedEntityAsync(
        EntityEntry entry,
        DateTime now,
        string actor,
        CancellationToken ct)
    {
        UpdateOptimisticConcurrencyTag(entry.Entity);
        return base.CompleteAndValidateUpdatedEntityAsync(entry, now, actor, ct);
    }

    static void UpdateOptimisticConcurrencyTag(object entity)
    {
        if (entity is IOptimisticConcurrency oc)
            oc.ETag = Ulid.NewUlid();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO: try this approach to apply all configurations from the assembly:
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfSQLiteRepository).Assembly);

        new CountryConfiguration().Configure(modelBuilder.Entity<Country>());
        new RoleConfiguration().Configure(modelBuilder.Entity<Role>());
        new InstrumentConfiguration().Configure(modelBuilder.Entity<Instrument>());
        new GenreConfiguration().Configure(modelBuilder.Entity<Genre>());

        new LabelConfiguration().Configure(modelBuilder.Entity<Label>());
        new AlbumConfiguration().Configure(modelBuilder.Entity<Album>());
        new TrackConfiguration().Configure(modelBuilder.Entity<Track>());
        new PersonConfiguration().Configure(modelBuilder.Entity<Person>());

        new AlbumPersonConfiguration().Configure(modelBuilder.Entity<AlbumPerson>());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // All enums are saved as text:
        configurationBuilder
            .Properties<Enum>()
            .HaveConversion<string>()
            ;

        configurationBuilder
            .Properties<AlbumId>()
            .HaveConversion<PersonIdConverter>()
            ;
    }
}
