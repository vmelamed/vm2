namespace vm2.ExpressionSerialization.Exceptions;

/// <summary>
/// Class NoAvailableResultException is thrown when there are no available results to be returned from a transform.
/// Implements the <see cref="Exception" />
/// </summary>
/// <seealso cref="Exception" />
public class NoAvailableResultException(
    string message = "There are no available transform results.",
    Exception? innerException = null) : Exception(message, innerException)
{
}
