namespace vm2.Repository.DB.TestSQLite.Mapping;

class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder
            .ToTable(nameof(Album))
            .HasQueryFilter(a => a.DeletedAt == null)   // Soft delete filter
            ;

        new FindableConfiguration<Album>().Configure(builder);

        builder
            .Property(a => a.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<UlidValueGenerator>()
            .HasColumnOrder(-100)
            ;

        builder
            .Property(a => a.Title)
            .HasMaxLength(Album.MaxTitleLength)
            .UseCollation("NOCASE")
            ;

        builder
            .HasOne(a => a.Label)
            .WithMany(l => l.Albums)
            .OnDelete(DeleteBehavior.Restrict)
            ;

        builder
            .HasMany(a => a.Personnel)
            .WithMany(p => p.Albums)
            .UsingEntity<AlbumPerson>()
            ;

        builder
            .OwnsMany(
                a => a.AlbumTracks,
                onb => onb.ToJson())    // ???
            ;

        new TenantedConfiguration<Album, Guid>().Configure(builder);
        new AuditableConfiguration<Album>().Configure(builder);
        new SoftDeletableConfiguration<Album>().Configure(builder);
        new OptimisticConcurrencyConfiguration<Album, byte[]>().Configure(builder);
    }
}
