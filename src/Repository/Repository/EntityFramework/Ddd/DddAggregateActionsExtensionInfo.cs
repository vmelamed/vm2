namespace vm2.Repository.EntityFramework.Ddd;

internal sealed class DddAggregateActionsExtensionInfo(DddAggregateActionsExtension extension)
    : DbContextOptionsExtensionInfo(extension)
{
    DddAggregateActions _actions = extension.Actions;
    bool _interceptorAdded = extension.InterceptorAdded;
    string? _logFragment;
    int? _hashCode;
    string? _debugView;

    DddAggregateActionsExtension DddExtension => extension;

    /// <inheritdoc/>
    public override bool IsDatabaseProvider => false;

    /// <inheritdoc/>
    public override string LogFragment
        => _logFragment ??= $"DDD Aggregate Actions: {_actions} ({_interceptorAdded})";

    /// <inheritdoc/>
    public override int GetServiceProviderHashCode() => _hashCode ??= HashCode.Combine(_actions, _interceptorAdded);

    /// <inheritdoc/>
    public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
        => debugInfo[nameof(DddExtension)] = _debugView ??= $"{_actions} ({_interceptorAdded})";

    /// <inheritdoc/>
    public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) => true;
}
