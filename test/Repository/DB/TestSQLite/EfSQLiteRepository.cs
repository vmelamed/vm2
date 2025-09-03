namespace vm2.Repository.DB.TestSQLite;

/// <summary>
/// <see cref="IRepository"/> based on Entity Framework and SQLite.
/// </summary>
public class EfSQLiteRepository : SQLiteEfRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EfSQLiteRepository"/> class using the specified database context
    /// options and actor retrieval function.
    /// </summary>
    /// <param name="options">The options to configure the database context for the repository. Cannot be null.</param>
    public EfSQLiteRepository(
        DbContextOptions options)
        : base(options)
    {
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
            .Properties<LabelId>()
            .HaveConversion<LabelIdConverter>()
            ;

        configurationBuilder
            .Properties<AlbumId>()
            .HaveConversion<AlbumIdConverter>()
            ;

        configurationBuilder
            .Properties<PersonId>()
            .HaveConversion<PersonIdConverter>()
            ;

        configurationBuilder
            .Properties<TrackId>()
            .HaveConversion<TrackIdConverter>()
            ;
    }
}
