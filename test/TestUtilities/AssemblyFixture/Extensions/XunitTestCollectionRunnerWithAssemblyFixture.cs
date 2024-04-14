﻿namespace TestUtilities.AssemblyFixture.Extensions;

public class XunitTestCollectionRunnerWithAssemblyFixture : XunitTestCollectionRunner
{
    readonly Dictionary<Type, object> _assemblyFixtureMappings;
    readonly IMessageSink _diagnosticMessageSink;

#pragma warning disable IDE0290 // Use primary constructor
    public XunitTestCollectionRunnerWithAssemblyFixture(Dictionary<Type, object> assemblyFixtureMappings,
                                                        ITestCollection testCollection,
                                                        IEnumerable<IXunitTestCase> testCases,
                                                        IMessageSink diagnosticMessageSink,
                                                        IMessageBus messageBus,
                                                        ITestCaseOrderer testCaseOrderer,
                                                        ExceptionAggregator aggregator,
                                                        CancellationTokenSource cancellationTokenSource)
        : base(testCollection, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource)
    {
        _assemblyFixtureMappings = assemblyFixtureMappings;
        _diagnosticMessageSink = diagnosticMessageSink;
    }
#pragma warning restore IDE0290 // Use primary constructor

    protected override Task<RunSummary> RunTestClassAsync(
        ITestClass testClass,
        IReflectionTypeInfo @class,
        IEnumerable<IXunitTestCase> testCases)
    {
        // Don't want to use .Concat + .ToDictionary because of the possibility of overriding types,
        // so instead we'll just let collection fixtures override assembly fixtures.
        var combinedFixtures = new Dictionary<Type, object>(_assemblyFixtureMappings);
        foreach (var (k, v) in CollectionFixtureMappings)
            combinedFixtures[k] = v;

        // We've done everything we need, so let the built-in types do the rest of the heavy lifting
        return new XunitTestClassRunner(
            testClass,
            @class,
            testCases,
            _diagnosticMessageSink,
            MessageBus,
            TestCaseOrderer,
            new ExceptionAggregator(Aggregator),
            CancellationTokenSource,
            combinedFixtures).RunAsync();
    }
}
