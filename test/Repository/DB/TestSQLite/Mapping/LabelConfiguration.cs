namespace vm2.Repository.DB.TestSQLite.Mapping;

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
            .HasColumnOrder(-100)
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

        new TenantedConfiguration<Label, Guid>().Configure(builder);
        new AuditableConfiguration<Label>().Configure(builder);
        new OptimisticConcurrencyConfiguration<Label, byte[]>().Configure(builder);
    }
}