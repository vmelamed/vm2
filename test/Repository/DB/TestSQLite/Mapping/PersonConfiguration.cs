namespace vm2.Repository.DB.TestSQLite.Mapping;

class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder
            .ToTable(nameof(Person))
            ;

        new FindableConfiguration<Person>().Configure(builder);

        builder
            .Property(p => p.Id)
            .HasValueGenerator<UlidValueGenerator>()
            .ValueGeneratedOnAdd()  // SQLite does not support HiLo, so we use ValueGeneratedOnAdd
            ;

        builder
            .Property(p => p.Name)
            .HasMaxLength(Person.MaxNameLength)
            .UseCollation("NOCASE")
            ;

        // The properties Roles, Genres, and InstrumentCodes are collections of primitive types (strings).
        // EF Core + SQLite handle collections of primitive types automatically with JSON columns.

        builder
            .HasMany(p => p.Albums)
            .WithMany(a => a.Personnel)
            .UsingEntity<AlbumPerson>()
            ;

        new AuditableConfiguration<Person>().Configure(builder);
        new OptimisticConcurrencyConfiguration<Person>().Configure(builder);
    }
}
