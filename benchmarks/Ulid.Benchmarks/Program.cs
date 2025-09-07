namespace vm2.Ulid.Benchmarks;

using BenchmarkDotNet.Configs;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<UlidBenchmarks>(
            DefaultConfig
                .Instance
                .WithOptions(ConfigOptions.DisableOptimizationsValidator)); // TODO: remove
    }
}