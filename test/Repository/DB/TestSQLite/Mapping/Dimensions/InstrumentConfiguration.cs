namespace vm2.Repository.DB.TestSQLite.Mapping.Dimensions;

class InstrumentConfiguration : IEntityTypeConfiguration<Instrument>
{
    public void Configure(EntityTypeBuilder<Instrument> builder)
    {
        builder.ToTable(nameof(Instrument));

        new FindableConfiguration<Instrument>().Configure(builder);

        builder
            .Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(Instrument.MaxCodeLength)
            .IsFixedLength(false)
            ;

        builder
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(Instrument.MaxNameLength)
            .UseCollation("NOCASE")
            .UseCollation("RTRIM")
            ;
    }
}
