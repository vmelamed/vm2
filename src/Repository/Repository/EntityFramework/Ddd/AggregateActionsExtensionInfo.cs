namespace vm2.Repository.EntityFramework.Ddd;

internal sealed class AggregateActionsExtensionInfo(
    AggregateActionsExtension extension,
    AggregateActions actions) : DbContextOptionsExtensionInfo(extension)
{
    /// <summary>
    /// Gets the collection of aggregate actions to be performed during the actions process.
    /// </summary>
    public AggregateActions Actions { get; } = actions;

    /// <inheritdoc/>
    public override bool IsDatabaseProvider => false;

    /// <inheritdoc/>
    public override string LogFragment => "DDD Aggregate Actions";

    /// <inheritdoc/>
    public override int GetServiceProviderHashCode() => 0;

    /// <inheritdoc/>
    public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        => debugInfo[nameof(AggregateActionsExtension)] = Actions.ToString();

    /// <inheritdoc/>
    public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => true;
}
