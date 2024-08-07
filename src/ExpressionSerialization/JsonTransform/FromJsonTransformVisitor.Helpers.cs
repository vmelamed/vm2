namespace vm2.ExpressionSerialization.JsonTransform;

public partial class FromJsonTransformVisitor
{
    readonly Dictionary<string, ParameterExpression> _parameters = [];
    readonly Dictionary<string, LabelTarget> _labelTargets = [];

    internal void ResetVisitState()
    {
        _parameters.Clear();
        _labelTargets.Clear();
    }

    ParameterExpression GetParameter(JElement e)
    {
        var id = e.GetPropertyId();

        if (_parameters.TryGetValue(id, out var expression))
            return expression;

        var type = e.GetType();

        if (e.TryGetPropertyValue<bool>(out var isByRef, Vocabulary.IsByRef) && isByRef is true)
            type = type.MakeByRefType();

        return _parameters[id] = Expression.Parameter(type, e.TryGetPropertyName(out var name) ? name : null);
    }
}
