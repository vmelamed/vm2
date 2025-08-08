namespace vm2.Repository.Tests.DB.SQLite.Mapping.Dimensions;

class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable(nameof(Country));

        new FindableConfiguration<Country>().Configure(builder);

        builder
            .Property(c => c.Code)
            .HasMaxLength(Country.CodeLength)
            .IsFixedLength()
            .IsUnicode(false)
            ;

        builder
            .Property(c => c.Name)
            .HasMaxLength(Country.MaxNameLength)
            .UseCollation("NOCASE")
            .UseCollation("RTRIM")
            ;
    }
}
