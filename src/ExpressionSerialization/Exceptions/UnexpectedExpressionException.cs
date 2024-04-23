namespace vm2.ExpressionSerialization.Exceptions;

/// <summary>
/// Class UnexpectedExpressionException is thrown when the expression visitor encounters unexpected or not-implemented expression.
/// Implements the <see cref="Exception" />
/// </summary>
/// <seealso cref="Exception" />
public class UnexpectedExpressionException(
    string messageFormat = UnexpectedExpressionException.defaultMessageFormat,
    string? param = null,
    Exception? innerException = null) : Exception(string.Format(messageFormat, param ?? ""), innerException)
{
    const string defaultMessageFormat = "Unexpected or unimplemented expression node {0} was encountered while visiting the expression."+
                                        "This visitors in this project do not process expression nodes of types: "+
                                        "`DebugInfoExpression`, `DynamicExpression`, `RuntimeVariablesExpression` and do not override the visitor method `ExpressionVisitor.VisitExtension`.";
}
