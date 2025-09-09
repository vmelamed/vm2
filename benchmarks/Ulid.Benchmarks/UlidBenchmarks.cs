namespace vm2.Ulid.Benchmarks;

using vm2.VmUlid;
using vm2.VmUlid.Rng;

using SyUlid = System.Ulid;

class PreGeneratedData<T>
{
    int _numberItems;
    int _index;
    T[] _data = null!;

    public PreGeneratedData(int number, Func<int, T> factory)
    {
        _numberItems = number;
        _index = 0;
        _data = [.. Enumerable.Range(0, _numberItems).Select(factory)];
    }

    public T GetNext()
    {
        if (_index == _numberItems)
            _index = 0;
        return _data[_index];
    }
}

[SimpleJob(RuntimeMoniker.HostProcess)]
[MemoryDiagnoser]
[HtmlExporter]
[CPUUsageDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class NewUlid
{
    [Params("Cryptographic", "HKDF", "Pseudo", "System")]
    public string RngAlgorithm { get; set; } = "";

    VmUlidFactory _factory = null!;

    Action _method = null!;

    [GlobalSetup]
    public void Setup()
    {
        _factory = RngAlgorithm switch {
            "Cryptographic" => new VmUlidFactory(new CryptographicRng()),
            "HKDF" => new VmUlidFactory(),
            "Pseudo" => new VmUlidFactory(new PseudoRng()),
            _ => new VmUlidFactory(new CryptographicRng()),
        };

        _method = RngAlgorithm switch {
            "Cryptographic" => () => _factory.NewUlid(),
            "HKDF" => () => _factory.NewUlid(),
            "Pseudo" => () => _factory.NewUlid(),
            _ => () => SyUlid.NewUlid(),
        };
    }

    [Benchmark]
    public void Generate_Ulid() => _method();
}

[SimpleJob(RuntimeMoniker.HostProcess)]
[MemoryDiagnoser]
[HtmlExporter]
[CPUUsageDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class UlidToString
{
    const int MaxDataItems = 1000;
    VmUlidFactory _factory = null!;
    PreGeneratedData<VmUlid> _dataVm = null!;
    PreGeneratedData<SyUlid> _dataSys = null!;

    [GlobalSetup]
    public void Setup()
    {
        _factory = new();
        _dataVm = new(MaxDataItems, _ => _factory.NewUlid());
        _dataSys = new(MaxDataItems, _ => SyUlid.NewUlid());
    }

    [Benchmark(Description = "VmUlid.ToString")]
    public string MyUlid_ToString() => _dataVm.GetNext().ToString();

    [Benchmark(Description = "SysUlid.ToString", Baseline = true)]
    public string SysUlid_ToString() => _dataSys.GetNext().ToString();
}

[SimpleJob(RuntimeMoniker.HostProcess)]
[MemoryDiagnoser]
[HtmlExporter]
[CPUUsageDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class ParseUlid
{
    const int MaxDataItems = 1000;
    PreGeneratedData<string> _data = null!;

    [GlobalSetup]
    public void Setup()
    {
        _data = new(MaxDataItems, _ => SyUlid.NewUlid().ToString());
    }

    [Benchmark(Description = "VmUlid.ToString")]
    public VmUlid MyUlid_Parse() => VmUlid.Parse(_data.GetNext());

    [Benchmark(Description = "SysUlid.ToString", Baseline = true)]
    public System.Ulid SysUlid_ToString() => SyUlid.Parse(_data.GetNext());
}