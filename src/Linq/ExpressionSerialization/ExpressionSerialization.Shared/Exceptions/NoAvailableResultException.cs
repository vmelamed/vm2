namespace vm2.Linq.ExpressionSerialization.Shared.Exceptions;

/// <summary>
/// Class NoAvailableResultException is thrown when there are no available results to be returned from a transform.
/// Implements the <see cref="Exception" />
/// </summary>
/// <seealso cref="Exception" />
public class NoAvailableResultException(string? message = null, Exception? innerException = null) : Exception(message ?? defaultMessage, innerException)
{
    const string defaultMessage = "There are no available transform results. Did you call `visitor.Equal` already?";
}
