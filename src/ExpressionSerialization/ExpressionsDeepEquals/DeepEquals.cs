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

    /// <summary>
    /// Visits the left expression nodes and compares them with the nodes of the right expression.
    /// </summary>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <param name="difference">Explains the difference.</param>
    /// <returns><c>true</c> if the expressions are equal, <c>false</c> otherwise.</returns>
    public static bool DeepEquals(this Expression left, Expression right, out string difference)
    {
        difference = "";
        if (left is null)
        {
            difference = right is not null ? $"Left is null but right is not: `{right}`" : "";
            return right is null;
        }
        if (right is null)
        {
            difference = $"Left is not null but right is null. left: `{left}`";
            return false;
        }
        if (ReferenceEquals(left, right))
            return true;
        if (left.Type != right.Type)
        {
            difference = $"Left and right are of different types: `{left.Type.FullName}` != `{right.Type.FullName}` (`{left}` != `{right}`)";
            return false;
        }

        var visitor = new DeepEqualsVisitor(right);

        visitor.Visit(left);
        difference = visitor.Difference;
        return visitor.Equal;
    }
}
