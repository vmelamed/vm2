namespace vm2.Ulid.Benchmarks;

public static class Program
{
    public static void Main(string[] args)
    {
        var artifactsFolder = args.Length >= 1 ? args[0] : ".\\BenchmarkDotNet.Artifacts\\results";
        BenchmarkSwitcher
            .FromAssembly(typeof(Program).Assembly)
            .Run(
                args,
                DefaultConfig
                    .Instance
                    .WithArtifactsPath(artifactsFolder)
                    .WithOptions(ConfigOptions.StopOnFirstError)
#if DEBUG
                    .WithOptions(
                        ConfigOptions.DisableOptimizationsValidator)
#endif
            );
    }
}