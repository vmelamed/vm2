﻿namespace vm2.ExpressionSerialization.Exceptions;

/// <summary>
/// Initializes a new instance of the <see cref="InternalTransformErrorException"/> class.
/// </summary>
/// <param name="message">The exception message.</param>
/// <param name="inner">The inner exception.</param>
public class InternalTransformErrorException(
    string message = "Unexpected transform error.",
    Exception? inner = null) : Exception(message, inner)
{
}