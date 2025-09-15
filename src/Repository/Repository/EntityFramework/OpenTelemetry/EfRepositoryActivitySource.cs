namespace vm2.Repository.EntityFramework.OpenTelemetry;

/// <summary>
/// Provides a centralized <see cref="System.Diagnostics.ActivitySource"/> for tracing/logging/metrics operations related to
/// <see cref="IRepository"/> implementation based on Entity Framework.
/// </summary>
/// <remarks>
/// Exposes a static <see cref="System.Diagnostics.ActivitySource"/> instance that can be used to create and manage telemetry activities for
/// operations within the Entity Framework repository. The activity source is initialized with the assembly name and version of
/// the current library.
/// </remarks>
public static class EfRepositoryActivitySource
{
    internal static AssemblyName AssemblyName { get; } = typeof(EfRepositoryActivitySource).Assembly.GetName();

    internal static string ActivitySourceName { get; } = AssemblyName.FullName;

    /// <summary>
    /// Gets the version
    /// </summary>
    public static readonly Version Version = AssemblyName.Version ?? new(0, 0, 0, 0);

    /// <summary>
    /// Gets the activity source
    /// </summary>
    public static readonly ActivitySource ActivitySource = new(ActivitySourceName, Version.ToString());
}
