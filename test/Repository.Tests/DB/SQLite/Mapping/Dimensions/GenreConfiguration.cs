namespace vm2.Repository.Tests.DB.SQLite.Mapping.Dimensions;

class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable(nameof(Genre));
        builder.HasKey(c => c.Name);

        builder
            .Property(c => c.Name)
            .HasMaxLength(Genre.MaxNameLength)
            .UseCollation("NOCASE")
            .UseCollation("RTRIM")
            ;
    }
}
