namespace vm2.Repository.Tests.DB.SQLite.Mapping;

class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable(nameof(Album));
        new FindableConfiguration<Album>().Configure(builder);

        builder
            .Property(a => a.Id)
            .ValueGeneratedOnAdd()
            ;

        builder
            .Property(a => a.Title)
            .HasMaxLength(Album.MaxTitleLength)
            ;

        builder
            .HasOne(a => a.Label)
            ;

        builder
            .HasMany(a => a.Personnel)
            .WithMany(p => p.Albums)
            .UsingEntity<AlbumPerson>()
            ;

        builder
            .OwnsMany(
                a => a.AlbumTracks,
                onb => onb.ToJson())
            ;

        new AuditableConfiguration<Album>().Configure(builder);
        new SoftDeletableConfiguration<Album>().Configure(builder);
    }
}
