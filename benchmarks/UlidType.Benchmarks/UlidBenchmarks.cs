namespace vm2.UlidType.Benchmarks;

using vm2.UlidType;
using vm2.UlidType.Rng;

//using SyUlid = System.Ulid;

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
    [Params("Cryptographic", "Pseudo")]
    public string RngAlgorithm { get; set; } = "";

    UlidFactory _factory = null!;

    Action _method = null!;

    [GlobalSetup]
    public void Setup()
    {
        _factory = RngAlgorithm switch {
            "Cryptographic" => new UlidFactory(new CryptographicRng()),
            "Pseudo" => new UlidFactory(new PseudoRng()),
            _ => new UlidFactory(new CryptographicRng()),
        };

        _method = RngAlgorithm switch {
            //"System" => () => SyUlid.NewUlid(),
            _ => () => _factory.NewUlid(),
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
    UlidFactory _factory = null!;
    PreGeneratedData<Ulid> _dataVm = null!;
    //PreGeneratedData<SyUlid> _dataSys = null!;

    [GlobalSetup]
    public void Setup()
    {
        _factory = new();
        _dataVm = new(MaxDataItems, _ => _factory.NewUlid());
        //_dataSys = new(MaxDataItems, _ => SyUlid.NewUlid());
    }

    [Benchmark(Description = "Ulid.ToString")]
    public string MyUlid_ToString() => _dataVm.GetNext().ToString();

    //[Benchmark(Description = "SysUlid.ToString", Baseline = true)]
    //public string SysUlid_ToString() => _dataSys.GetNext().ToString();
}

[SimpleJob(RuntimeMoniker.HostProcess)]
[MemoryDiagnoser]
[HtmlExporter]
[CPUUsageDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class ParseUlid
{
    const int MaxDataItems = 1000;
    UlidFactory _factory = null!;
    PreGeneratedData<string> _data = null!;

    [GlobalSetup]
    public void Setup()
    {
        _factory = new();
        _data = new(MaxDataItems, _ => _factory.NewUlid().ToString());
        //_data = new(MaxDataItems, _ => SyUlid.NewUlid().ToString());
    }

    [Benchmark(Description = "Ulid.ToString")]
    public Ulid MyUlid_Parse() => Ulid.Parse(_data.GetNext());

    //[Benchmark(Description = "SysUlid.ToString", Baseline = true)]
    //public SyUlid SysUlid_ToString() => SyUlid.Parse(_data.GetNext());
}