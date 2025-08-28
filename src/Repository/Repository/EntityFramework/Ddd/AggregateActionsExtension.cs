namespace vm2.Repository.EntityFramework.Ddd;

/// <summary>
/// Provides an extension for configuring DDD (Domain-Driven Design) boundary checking options in an Entity Framework
/// Core <see cref="DbContext"/>.
/// </summary>
/// <remarks>
/// This extension allows users to enable or configure additional boundary checking logic for DDD patterns when working with
/// Entity Framework Core. It is not a database provider and does not directly affect database interactions. Instead, it
/// provides hooks for applying services and validating options related to DDD boundary enforcement.
/// </remarks>
public sealed class AggregateActionsExtension : IDbContextOptionsExtension
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateActionsExtension"/> class.
    /// </summary>
    /// <remarks>This constructor sets up the extension by initializing its associated metadata.</remarks>
    public AggregateActionsExtension(AggregateActions checks)
        => Info = new AggregateActionsExtensionInfo(this, checks);

    /// <inheritdoc />
    public void ApplyServices(IServiceCollection services) { }

    /// <inheritdoc />
    public void Validate(IDbContextOptions options) { }

    /// <summary>
    /// Gets the information about the current database context options extension.
    /// </summary>
    public DbContextOptionsExtensionInfo Info { get; init; }
}
