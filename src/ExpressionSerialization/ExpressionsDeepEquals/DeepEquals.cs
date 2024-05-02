namespace vm2.ExpressionSerialization.ExpressionsDeepEquals;

/// <summary>
/// Class ExpressionExtensions.
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    /// Visits the left expression nodes and compares them with the nodes of the right expression.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns><c>true</c> if the expressions are equal, <c>false</c> otherwise.</returns>
    public static bool DeepEquals(this Expression left, Expression right)
    {
        if (left is null)
            return right is null;
        if (right is null)
            return false;
        if (ReferenceEquals(left, right))
            return true;
        if (left.Type != right.Type)
            return false;

        var visitor = new DeepEqualsVisitor(right);

        visitor.Visit(left);
        return visitor.Equal;
    }
}
