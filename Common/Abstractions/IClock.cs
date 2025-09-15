namespace vm2.Common.Abstractions;

/// <summary>
/// Represents a clock that provides the current date and time.
/// </summary>
/// <remarks>
/// This interface is useful for abstracting time-related functionality, allowing for easier testing and dependency injection.
/// It should be assumed that implementations of this interface return the current date and time in UTC time-zone.
/// </remarks>
public interface IClock
{
    /// <summary>
    /// Gets the current date and time, expressed as UTC.
    /// </summary>
    DateTime Now { get; }
}