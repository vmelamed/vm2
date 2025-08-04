namespace vm2.Repository.Tests.DB.SQLite.Mapping.Dimensions;

class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role));
        builder.HasKey(c => c.Name);

        builder
            .Property(c => c.Name)
            .HasMaxLength(Role.MaxNameLength)
            ;
    }
}
