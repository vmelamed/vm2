using FluentAssertions.Extensibility;

[assembly: AssertionEngineInitializer(
    typeof(AssertionEngineInitializer),
    nameof(AssertionEngineInitializer.AcknowledgeSoftWarning))]

/// <summary>
/// Provides methods to initialize and configure the assertion engine.
/// </summary>
/// <remarks>This class contains static methods for setting up the assertion engine, including handling
/// license-related warnings. It is intended to be used during the initialization phase of the application.</remarks>
public static class AssertionEngineInitializer
{
    /// <summary>
    /// Acknowledges and accepts the current soft warning related to the license.
    /// </summary>
    public static void AcknowledgeSoftWarning()
    {
        License.Accepted = true;
    }
}