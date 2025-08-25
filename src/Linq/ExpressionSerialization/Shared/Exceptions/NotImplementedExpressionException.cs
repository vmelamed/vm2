namespace vm2.Linq.ExpressionSerialization.Shared.Exceptions;

/// <summary>
/// Class NotImplementedExpressionException is thrown when the expression visitor encounters unexpected or not-implemented expression.
/// Implements the <see cref="Exception" />
/// </summary>
/// <seealso cref="Exception" />
public class NotImplementedExpressionException(
    string messageFormat = NotImplementedExpressionException.defaultMessageFormat,
    string? param = null,
    Exception? innerException = null) : Exception(string.Format(messageFormat, param ?? ""), innerException)
{
    const string defaultMessageFormat = "Expression node {0} was encountered while visiting the expression. "+
                                        "This visitor does not process expression nodes of types: `DebugInfoExpression`, "+
                                        "`DynamicExpression`, `RuntimeVariablesExpression` and does not override the "+
                                        "visitor method `ExpressionVisitor.VisitExtension`.";
}
