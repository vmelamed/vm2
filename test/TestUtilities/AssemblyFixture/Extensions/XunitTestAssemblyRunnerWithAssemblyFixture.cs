namespace TestUtilities.AssemblyFixture.Extensions;

public class XunitTestAssemblyRunnerWithAssemblyFixture : XunitTestAssemblyRunner
{
    readonly Dictionary<Type, object> _assemblyFixtureMappings = [];

#pragma warning disable IDE0290 // Use primary constructor
    public XunitTestAssemblyRunnerWithAssemblyFixture(ITestAssembly testAssembly,
                                                      IEnumerable<IXunitTestCase> testCases,
                                                      IMessageSink diagnosticMessageSink,
                                                      IMessageSink executionMessageSink,
                                                      ITestFrameworkExecutionOptions executionOptions)
        : base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions)
    { }
#pragma warning restore IDE0290 // Use primary constructor

    protected override async Task AfterTestAssemblyStartingAsync()
    {
        // Let everything initialize
        await base.AfterTestAssemblyStartingAsync();

        // Go find all the AssemblyFixtureAttributes adorned on the test assembly
        Aggregator.Run(() =>
        {
            var fixturesAttributes = ((IReflectionAssemblyInfo)TestAssembly.Assembly).Assembly
                                                                                    .GetCustomAttributes(typeof(AssemblyFixtureAttribute), false)
                                                                                    .Cast<AssemblyFixtureAttribute>()
                                                                                    .ToList();

            // Instantiate all the fixtures
            foreach (var fixtureAttr in fixturesAttributes)
                _assemblyFixtureMappings[fixtureAttr.FixtureType] = Activator.CreateInstance(fixtureAttr.FixtureType)
                                                                        ?? throw new InvalidOperationException();
        });
    }

    protected override Task BeforeTestAssemblyFinishedAsync()
    {
        // Make sure we clean up everybody who is disposable, and use Aggregator.Run to isolate Dispose failures
        foreach (var disposable in _assemblyFixtureMappings.Values.OfType<IDisposable>())
            Aggregator.Run(disposable.Dispose);

        return base.BeforeTestAssemblyFinishedAsync();
    }

    protected override Task<RunSummary> RunTestCollectionAsync(IMessageBus messageBus,
                                                               ITestCollection testCollection,
                                                               IEnumerable<IXunitTestCase> testCases,
                                                               CancellationTokenSource cancellationTokenSource)
        => new XunitTestCollectionRunnerWithAssemblyFixture(_assemblyFixtureMappings, testCollection, testCases, DiagnosticMessageSink, messageBus, TestCaseOrderer, new ExceptionAggregator(Aggregator), cancellationTokenSource).RunAsync();
}