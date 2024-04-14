namespace TestUtilities.AssemblyFixture.Extensions;

public class XunitTestFrameworkExecutorWithAssemblyFixture : XunitTestFrameworkExecutor
{
#pragma warning disable IDE0290 // Use primary constructor
    public XunitTestFrameworkExecutorWithAssemblyFixture(
        AssemblyName assemblyName,
        ISourceInformationProvider sourceInformationProvider,
        IMessageSink diagnosticMessageSink)
        : base(
            assemblyName,
            sourceInformationProvider,
            diagnosticMessageSink)
    { }
#pragma warning restore IDE0290 // Use primary constructor

    protected override async void RunTestCases(
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink executionMessageSink,
        ITestFrameworkExecutionOptions executionOptions)
    {
        using var assemblyRunner = new XunitTestAssemblyRunnerWithAssemblyFixture(
                                            TestAssembly,
                                            testCases,
                                            DiagnosticMessageSink,
                                            executionMessageSink,
                                            executionOptions);

        await assemblyRunner.RunAsync();
    }
}
