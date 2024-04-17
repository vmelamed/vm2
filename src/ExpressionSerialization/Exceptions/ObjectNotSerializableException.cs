namespace vm2.ExpressionSerialization.Exceptions;

/// <summary>
/// Class ObjectNotSerializableException.
/// Implements the <see cref="System.Exception" />
/// </summary>
/// <seealso cref="Exception" />
[Serializable]
public class ObjectNotSerializableException(string? message = null, Exception? inner = null) : Exception(message ?? defaultMessage, inner)
{
    const string defaultMessage = "Cannot serialize object.";
    const string defaultMessageFormat = "Object of type '{0}' cannot be serialized.";

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectNotSerializableException"/> class.
    /// </summary>
    public ObjectNotSerializableException(Type objectType, Exception? inner = null)
        : this(
            string.Format(defaultMessageFormat, objectType.AssemblyQualifiedName ?? objectType.FullName ?? objectType.Name),
            inner)
    { }
}
