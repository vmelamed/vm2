namespace vm2.ExpressionSerialization.Exceptions;

/// <summary>
/// Initializes a new instance of the <see cref="InternalTransformErrorException"/> class.
/// </summary>
/// <param name="message">The exception message.</param>
/// <param name="inner">The inner exception.</param>
public class SchemaValidationErrorsException(string? message = null, Exception? inner = null) : Exception(message ?? defaultMessage, inner)
{
    const string defaultMessage = "Validation against the schema failed.";
}
