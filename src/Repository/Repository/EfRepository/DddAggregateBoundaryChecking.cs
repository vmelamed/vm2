namespace vm2.Repository.EfRepository;

/// <summary>
/// Specifies the modes of boundary checking for domain-driven design (DDD) aggregates.
/// </summary>
/// <remarks>
/// This enumeration defines the available options for performing boundary checks on aggregates in a DDD context. The flags can
/// be combined to enable multiple modes of boundary checking simultaneously.
/// </remarks>
[Flags]
public enum DddBoundaryChecks
{
    /// <summary>
    /// No boundary checking is performed.
    /// </summary>
    None = 0,

    /// <summary>
    /// The method <see cref="IValidatable.ValidateAsync(object?, CancellationToken)"/> of all <see cref="IValidatable"/> entities
    /// are invoked to verify the invariants.
    /// </summary>
    Validation = 1,

    /// <summary>
    /// Boundary checking is performed only for aggregates that implement <see cref="IAggregate{TRoot}"/>.
    /// </summary>
    AggregateBoundary = 2,

    /// <summary>
    /// Both validation and aggregate boundary checking are performed.
    /// </summary>
    Full = Validation | AggregateBoundary,
}

/// <summary>
/// Provides an extension for configuring DDD (Domain-Driven Design) boundary checking options in an Entity Framework
/// Core <see cref="DbContext"/>.
/// </summary>
/// <remarks>
/// This extension allows users to enable or configure additional boundary checking logic for DDD patterns when working with
/// Entity Framework Core. It is not a database provider and does not directly affect database interactions. Instead, it
/// provides hooks for applying services and validating options related to DDD boundary enforcement.
/// </remarks>
public sealed class DddAggregateBoundaryChecking : IDbContextOptionsExtension
{
    const string dddErrorMessageHasMoreThan1Aggregate        = "DDD error: Entity type {0} is marked with multiple IAggregate<TRoot> interfaces. "+
                                                               "An entity cannot be part of more than one aggregate.";
    const string dddErrorMessageHasNoAggregate               = "DDD error: Entity type {0} is not marked with IAggregate<TRoot>.";
    const string dddErrorMessageViolationOfAggregateBoundary = "DDD error: Violation of aggregate consistency and transaction boundaries: "+
                                                               "encountered entities of IAggregate<{0}> and IAggregate<{1}>. "+
                                                               "Only entities and values from the same aggregate can participate in a transaction.";

    /// <summary>
    /// Initializes a new instance of the <see cref="DddAggregateBoundaryChecking"/> class.
    /// </summary>
    /// <remarks>This constructor sets up the extension by initializing its associated metadata.</remarks>
    public DddAggregateBoundaryChecking(DddBoundaryChecks checks)
        => Info = new DddBoundaryCheckingOptionsExtensionInfo(this, checks);

    /// <inheritdoc />
    public void ApplyServices(IServiceCollection services) { }

    /// <inheritdoc />
    public void Validate(IDbContextOptions options) { }

    /// <summary>
    /// Gets the information about the current database context options extension.
    /// </summary>
    public DbContextOptionsExtensionInfo Info { get; init; }

    /// <summary>
    /// Checks the aggregate boundary for the entities of the current transaction held in the <see cref="ChangeTracker"/>.
    /// </summary>
    /// <param name="changeTracker"></param>
    public static void CheckAggregateBoundary(ChangeTracker changeTracker)
    {
        Type? typeOfRoot = null;

        foreach (var entry in changeTracker
                                .Entries()
                                .Where(e => e.State is not EntityState.Unchanged or EntityState.Detached))
        {
            var genInterfaces = entry.Entity
                                        .GetType()
                                        .GetInterfaces()
                                        .Where(i => i.IsGenericType
                                                    && i.GetGenericTypeDefinition() == typeof(IAggregate<>))
                                        .ToList()
                                        ;

            if (genInterfaces.Count > 1)
                throw new InvalidOperationException(string.Format(dddErrorMessageHasMoreThan1Aggregate, entry.Entity.GetType().Name));
            if (genInterfaces.Count <= 0)
                throw new InvalidOperationException(string.Format(dddErrorMessageHasNoAggregate, entry.Entity.GetType().Name));

            var currentEntityRoot = genInterfaces[0].GenericTypeArguments[0];

            if (typeOfRoot is null)
                typeOfRoot = currentEntityRoot;
            else
            if (typeOfRoot != currentEntityRoot)
                throw new InvalidOperationException(string.Format(dddErrorMessageViolationOfAggregateBoundary, typeOfRoot.Name, currentEntityRoot.Name));
        }
    }

    /// <summary>
    /// Checks the invariants of all <see cref="IValidatable"/> entities in the <see cref="ChangeTracker"/> that are marked as
    /// Added or Modified.
    /// </summary>
    /// <param name="changeTracker"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static async ValueTask CheckAggregateInvariantsAsync(
        ChangeTracker changeTracker,
        CancellationToken ct = default)
    {
        foreach (var entry in changeTracker
                                .Entries()
                                .Where(e => e.State is (EntityState.Added or EntityState.Modified)))
            if (entry.Entity is IValidatable validatable)
                await validatable.ValidateAsync(changeTracker.Context, ct);
    }
}

internal sealed class DddBoundaryCheckingOptionsExtensionInfo(
    DddAggregateBoundaryChecking extension,
    DddBoundaryChecks checks) : DbContextOptionsExtensionInfo(extension)
{
    public DddBoundaryChecks Checks { get; } = checks;

    public override bool IsDatabaseProvider => false;

    public override string LogFragment => "DDD Boundary Checking";

    public override int GetServiceProviderHashCode() => 0;

    public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        => debugInfo["DddAggregateBoundaryChecking"] = Checks.ToString();

    /// <summary>
    /// Determines whether the same service provider should be used for the specified
    /// <see cref="DbContextOptionsExtensionInfo"/> instance.
    /// </summary>
    /// <param name="other">
    /// The <see cref="DbContextOptionsExtensionInfo"/> instance to compare against.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the same service provider should be used; otherwise, <see langword="false"/>.
    /// </returns>
    public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => true;
}
