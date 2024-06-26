namespace vm2.ExpressionSerialization.JsonTransform;

static partial class FromJsonDataTransform
{
    /// <summary>
    /// Transforms the element to a <see cref="ConstantExpression"/> object.
    /// </summary>
    /// <param name="element">The element.</param>
    /// <returns>ConstantExpression.</returns>
    internal static ConstantExpression ConstantTransform(JElement element)
    {
        var (value, type) = ValueTransform(element);

        return Expression.Constant(value, type);
    }

    static (object?, Type) ValueTransform(JElement element)
    {
        var type = element.GetElementType();

        if (type == typeof(void))
            throw new SerializationException($"Got 'void' type of constant data in the element `{element.Name}`");

        return (GetTransformation(element)(element, ref type), type);
    }

    delegate object? Transformation(JElement element, ref Type type);

    static Transformation GetTransformation(JElement element) => _constantTransformations[element.Name];
}
