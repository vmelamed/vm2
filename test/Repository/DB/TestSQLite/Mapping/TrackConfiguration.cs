namespace vm2.Repository.DB.TestSQLite.Mapping;

class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder
            .ToTable(nameof(Track))
            ;

        new FindableConfiguration<Track>().Configure(builder);

        builder
            .Property(t => t.Id)
            .HasValueGenerator<TrackIdGenerator>()
            .ValueGeneratedOnAdd()
            .HasColumnOrder(-100)
            ;

        builder.Property(t => t.Title)
            .HasMaxLength(Track.MaxTitleLength)
            .UseCollation("NOCASE")
            ;

        builder
            .OwnsMany(
                t => t.Personnel,
                onb => onb.ToJson())    // ???
            ;

        builder
            .HasMany(t => t.Albums)
            ;

        new TenantedConfiguration<Track, Guid>().Configure(builder);
        new AuditableConfiguration<Track>().Configure(builder);
        new OptimisticConcurrencyConfiguration<Track, byte[]>().Configure(builder);
    }
}

