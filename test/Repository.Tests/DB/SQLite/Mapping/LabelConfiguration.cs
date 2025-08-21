namespace vm2.Repository.Tests.DB.SQLite.Mapping;

class LabelConfiguration : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        builder
            .ToTable(nameof(Label))
            ;

        new FindableConfiguration<Label>().Configure(builder);

        builder
            .Property(c => c.Id)
            .HasValueGenerator<UlidValueGenerator>()
            .ValueGeneratedOnAdd()
            ;

        builder
            .Property(c => c.Name)
            .HasMaxLength(Label.MaxNameLength)
            .UseCollation("NOCASE")
            ;

        builder
            .Property(c => c.CountryCode)
            .HasMaxLength(2)
            .IsFixedLength(true)
            .IsUnicode(false)
            .UseCollation("NOCASE")
            ;

        new AuditableConfiguration<Label>().Configure(builder);
        new OptimisticConcurrencyConfiguration<Label>().Configure(builder);
    }
}