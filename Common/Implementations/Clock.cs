namespace vm2.Common.Implementations;

/// <summary>
/// Represents a clock that provides the current date and time in UTC.
/// </summary>
/// <remarks>
/// This is the standard implementation of the interface.
/// </remarks>
public class Clock : IClock
{
    /// <summary>
    /// Gets the current date and time in Coordinated Universal Time (UTC).
    /// </summary>
    public DateTime Now => DateTime.UtcNow;
}
