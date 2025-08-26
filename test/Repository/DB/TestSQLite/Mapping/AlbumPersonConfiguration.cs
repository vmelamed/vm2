namespace vm2.Repository.DB.TestSQLite.Mapping;

class AlbumPersonConfiguration : IEntityTypeConfiguration<AlbumPerson>
{
    public void Configure(EntityTypeBuilder<AlbumPerson> builder)
    {
        builder
            .ToTable(nameof(AlbumPerson))
            ;

        builder
            .HasOne(ap => ap.Album)
            .WithMany(a => a.AlbumsPersons)
            ;

        builder
            .HasOne(ap => ap.Person)
            .WithMany(p => p.PersonsAlbums)
            ;
    }
}
