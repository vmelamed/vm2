namespace vm2.Repository.Tests.DB.SQLite.Mapping;

class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.ToTable(nameof(Track));
        new FindableConfiguration<Track>().Configure(builder);

        builder
            .Property(t => t.Id)
            .ValueGeneratedOnAdd()
            ;

        builder.Property(t => t.Title)
            .HasMaxLength(Track.MaxTitleLength)
            .UseCollation("NOCASE")
            ;

        builder
            .HasMany(t => t.Albums)
            ;

        builder
            .OwnsMany(
                t => t.Personnel,
                onb => onb.ToJson())    // ???
            ;
    }
}

