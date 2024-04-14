namespace TestUtilities.AssemblyFixture;

using TestUtilities.AssemblyFixture.Extensions;

public class XunitTestFrameworkWithAssemblyFixture(IMessageSink messageSink) : XunitTestFramework(messageSink)
{
    protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
        => new XunitTestFrameworkExecutorWithAssemblyFixture(assemblyName, SourceInformationProvider, DiagnosticMessageSink);
}
