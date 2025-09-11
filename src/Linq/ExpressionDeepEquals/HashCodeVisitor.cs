namespace vm2.Linq.ExpressionDeepEquals;

/// <summary>
/// A visitor that computes a hash code for an expression tree.
/// </summary>
public class HashCodeVisitor : ExpressionVisitor
{
    HashCode _hc = new();

    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(node))]
    public override Expression? Visit(Expression? node)
    {
        ArgumentNullException.ThrowIfNull(node, nameof(node));

        _hc.Add(node.NodeType);
        _hc.Add(node.Type.GetHashCode());

        return base.Visit(node);
    }

    /// <summary>
    /// Computes the hash code for the current expression.
    /// </summary>
    /// <returns>An integer representing the hash code of the current expression.</returns>
    public int ToHashCode() => _hc.ToHashCode();

    /// <inheritdoc/>
    protected override Expression VisitDefault(DefaultExpression node) => base.VisitDefault(node);

    /// <inheritdoc/>
    protected override Expression VisitConstant(ConstantExpression node)
    {
        _hc.Add(node.Value is not null ? node.Value : 0);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitUnary(UnaryExpression node)
    {
        Visit(node.Operand);
        _hc.Add(node.IsLifted);
        _hc.Add(node.IsLiftedToNull);
        VisitMemberInfo(node.Method);
        return node;
    }

    private void VisitMemberInfo(MemberInfo? method)
    {
        if (method is null)
            return;
        _hc.Add(method.GetHashCode());
    }

    /// <inheritdoc/>
    protected override Expression VisitTypeBinary(TypeBinaryExpression node)
    {
        Visit(node.Expression);
        _hc.Add(node.TypeOperand.GetHashCode());
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitBinary(BinaryExpression node)
    {
        Visit(node.Left);
        Visit(node.Right);
        if (node.Conversion is not null)
            Visit(node.Conversion);
        _hc.Add(node.IsLifted);
        _hc.Add(node.IsLiftedToNull);
        VisitMemberInfo(node.Method);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitBlock(BlockExpression node)
    {
        foreach (var v in node.Variables)
            Visit(v);
        foreach (var e in node.Expressions)
            Visit(e);
        Visit(node.Result);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitConditional(ConditionalExpression node)
    {
        Visit(node.Test);
        Visit(node.IfTrue);
        Visit(node.IfFalse);
        return node;
    }

    /// <inheritdoc/>
    protected override CatchBlock VisitCatchBlock(CatchBlock node)
    {
        _hc.Add(node.Test.GetHashCode());
        if (node.Variable is not null)
            Visit(node.Variable);
        if (node.Filter is not null)
            Visit(node.Filter);
        Visit(node.Body);
        return node;
    }

    /// <inheritdoc/>
    protected override ElementInit VisitElementInit(ElementInit node)
    {
        foreach (var a in node.Arguments)
            Visit(a);
        VisitMemberInfo(node.AddMethod);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitGoto(GotoExpression node)
    {
        VisitLabelTarget(node.Target);
        if (node.Value is not null)
            Visit(node.Value);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitIndex(IndexExpression node)
    {
        if (node.Object is not null)
            Visit(node.Object);
        foreach (var a in node.Arguments)
            Visit(a);
        VisitMemberInfo(node.Indexer);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitInvocation(InvocationExpression node)
    {
        Visit(node.Expression);
        foreach (var a in node.Arguments)
            Visit(a);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitLabel(LabelExpression node)
    {
        VisitLabelTarget(node.Target);
        if (node.DefaultValue is not null)
            Visit(node.DefaultValue);
        return node;
    }

    /// <inheritdoc/>
    [return: NotNullIfNotNull(nameof(node))]
    protected override LabelTarget? VisitLabelTarget(LabelTarget? node)
    {
        if (node is not null)
        {
            _hc.Add(node.Name ?? string.Empty);
            _hc.Add(node.Type?.GetHashCode() ?? 0);
        }
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        foreach (var p in node.Parameters)
            Visit(p);
        Visit(node.Body);
        _hc.Add(node.Name ?? string.Empty);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitListInit(ListInitExpression node)
    {
        Visit(node.NewExpression);
        foreach (var e in node.Initializers)
            VisitElementInit(e);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitLoop(LoopExpression node)
    {
        Visit(node.Body);
        VisitLabelTarget(node.BreakLabel);
        VisitLabelTarget(node.ContinueLabel);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitMember(MemberExpression node)
    {
        VisitMemberInfo(node.Member);
        if (node.Expression is not null)
            Visit(node.Expression);
        return node;
    }

    /// <inheritdoc/>
    protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
    {
        VisitMemberInfo(node.Member);
        Visit(node.Expression);
        return node;
    }

    /// <inheritdoc/>
    protected override MemberBinding VisitMemberBinding(MemberBinding node)
    {
        _hc.Add(node.BindingType.GetHashCode());
        VisitMemberInfo(node.Member);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitMemberInit(MemberInitExpression node)
    {
        Visit(node.NewExpression);
        foreach (var b in node.Bindings)
            VisitMemberBinding(b);
        return node;
    }

    /// <inheritdoc/>
    protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
    {
        _hc.Add(node.BindingType.GetHashCode());
        VisitMemberInfo(node.Member);
        foreach (var e in node.Initializers)
            VisitElementInit(e);
        return node;
    }

    /// <inheritdoc/>
    protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
    {
        _hc.Add(node.BindingType.GetHashCode());
        VisitMemberInfo(node.Member);
        foreach (var b in node.Bindings)
            VisitMemberBinding(b);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Object is not null)
            Visit(node.Object);
        foreach (var a in node.Arguments)
            Visit(a);
        VisitMemberInfo(node.Method);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitNew(NewExpression node)
    {
        VisitMemberInfo(node.Constructor);
        foreach (var a in node.Arguments)
            Visit(a);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitNewArray(NewArrayExpression node)
    {
        foreach (var e in node.Expressions)
            Visit(e);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        _hc.Add(node.Name ?? string.Empty);
        _hc.Add(node.IsByRef.GetHashCode());
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
    {
        foreach (var v in node.Variables)
            Visit(v);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitSwitch(SwitchExpression node)
    {
        if (node.SwitchValue is not null)
            Visit(node.SwitchValue);
        VisitMemberInfo(node.Comparison);
        foreach (var c in node.Cases)
            VisitSwitchCase(c);
        if (node.DefaultBody is not null)
            Visit(node.DefaultBody);
        return node;
    }

    /// <inheritdoc/>
    protected override SwitchCase VisitSwitchCase(SwitchCase node)
    {
        foreach (var t in node.TestValues)
            if (t is not null)
                Visit(t);
            else
                _hc.Add(0);
        Visit(node.Body);
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitTry(TryExpression node)
    {
        Visit(node.Body);
        foreach (var c in node.Handlers)
            VisitCatchBlock(c);
        if (node.Finally is not null)
            Visit(node.Finally);
        if (node.Fault is not null)
            Visit(node.Fault);
        return node;
    }
}
