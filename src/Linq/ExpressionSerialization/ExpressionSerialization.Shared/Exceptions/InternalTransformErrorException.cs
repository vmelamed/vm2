namespace vm2.Linq.ExpressionSerialization.Shared.Exceptions;

/// <summary>
/// Initializes a new instance of the <see cref="InternalTransformErrorException"/> class.
/// </summary>
/// <param name="message">The exception message.</param>
/// <param name="inner">The inner exception.</param>
public class InternalTransformErrorException(string? message = null, Exception? inner = null)
                : InvalidOperationException(message ?? defaultMessage, inner)
{
    const string defaultMessage = "Unexpected transform error.";
}
