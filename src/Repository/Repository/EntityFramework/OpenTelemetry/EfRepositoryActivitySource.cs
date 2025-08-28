namespace vm2.Repository.EntityFramework.OpenTelemetry;

/// <summary>
/// Provides a centralized <see cref="ActivitySource"/> for tracing/logging/metrics operations related to
/// <see cref="IRepository"/> implementation based on Entity Framework.
/// </summary>
/// <remarks>
/// Exposes a static <see cref="ActivitySource"/> instance that can be used to create and manage telemetry activities for
/// operations within the Entity Framework repository. The activity source is initialized with the assembly name and version of
/// the current library.
/// </remarks>
public static class EfRepositoryActivitySource
{
    static readonly AssemblyName _assemblyName = typeof(EfRepositoryActivitySource).Assembly.GetName();
    internal static AssemblyName AssemblyName => _assemblyName;
    internal static string ActivitySourceName => _assemblyName.Name!;

    /// <summary>
    /// Gets the version
    /// </summary>
    public static readonly Version Version = _assemblyName.Version!;

    /// <summary>
    /// Gets the activity source
    /// </summary>
    //TODO: Rename to ActivitySource
    public static readonly ActivitySource Source = new(ActivitySourceName, Version.ToString());
}
