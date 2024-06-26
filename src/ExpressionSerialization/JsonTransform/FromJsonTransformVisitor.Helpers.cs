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
}
