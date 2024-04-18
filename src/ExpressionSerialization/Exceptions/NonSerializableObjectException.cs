namespace vm2.ExpressionSerialization.Exceptions;

/// <summary>
/// Class NonSerializableObjectException.
/// Implements the <see cref="System.Exception" />
/// </summary>
/// <seealso cref="Exception" />
[Serializable]
public class NonSerializableObjectException(string? message = null, Exception? inner = null) : Exception(message ?? defaultMessage, inner)
{
    const string defaultMessage = "Cannot serialize object.";
    const string defaultMessageFormat = "Object '{0}' of type '{1}' cannot be serialized.";

    /// <summary>
    /// Initializes a new instance of the <see cref="NonSerializableObjectException"/> class.
    /// </summary>
    public NonSerializableObjectException(Type objectType, object? objectValue, Exception? inner = null)
        : this(
            string.Format(defaultMessageFormat, objectValue, objectType.AssemblyQualifiedName ?? objectType.FullName ?? objectType.Name),
            inner)
    {
    }
}
