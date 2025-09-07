namespace vm2.Ulid.Benchmarks;

[SimpleJob(RuntimeMoniker.HostProcess)]
[MemoryDiagnoser]
[HtmlExporter]
public class UlidBenchmarks
{
    Ulid1Factory? _factory;

    [GlobalSetup]
    public void Setup()
    {
        _factory = new Ulid1Factory();
    }

    [Benchmark(Description = "vm2.VmUlid.NewUlid")]
    public VmUlid Generate_MyUlid() => _factory!.NewUlid();

    [Benchmark(Description = "vm2.VmUlid.ToString")]
    public string MyUlid_ToString() => _factory!.NewUlid().ToString();

    [Benchmark(Description = "vm2.VmUlid.Parse")]
    public VmUlid MyUlid_Parse()
    {
        var s = _factory!.NewUlid().ToString();
        return VmUlid.Parse(s);
    }
}