namespace vm2.ExpressionSerialization.ExpressionsDeepEquals;
/// <summary>
/// Traverses the expression tree and enqueues each node in the order of visit.
/// Implements the <see cref="ExpressionVisitor" />
/// </summary>
/// <seealso cref="ExpressionVisitor" />
class EnqueueingVisitor : ExpressionVisitor
{
    public Queue<object?> VisitingQueue { get; } = new();

    protected override CatchBlock VisitCatchBlock(CatchBlock node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitCatchBlock(node);
    }

    protected override ElementInit VisitElementInit(ElementInit node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitElementInit(node);
    }

    protected override LabelTarget? VisitLabelTarget(LabelTarget? node)
    {
        VisitingQueue.Enqueue(node);
        return node;
    }

    //protected override MemberBinding VisitMemberBinding(MemberBinding node)
    //{
    //    VisitingQueue.Enqueue(node);
    //    return base.VisitMemberBinding(node);
    //}

    protected override Expression VisitMember(MemberExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitMember(node);
    }

    protected override Expression VisitMemberInit(MemberInitExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitMemberInit(node);
    }

    protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitMemberAssignment(node);
    }

    protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitMemberListBinding(node);
    }

    protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitMemberMemberBinding(node);
    }

    protected override SwitchCase VisitSwitchCase(SwitchCase node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitSwitchCase(node);
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitBinary(node);
    }

    protected override Expression VisitBlock(BlockExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitBlock(node);
    }

    protected override Expression VisitConditional(ConditionalExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitConditional(node);
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        VisitingQueue.Enqueue(node);
        return node;
    }

    protected override Expression VisitDebugInfo(DebugInfoExpression node) => node;

    protected override Expression VisitDefault(DefaultExpression node)
    {
        VisitingQueue.Enqueue(node);
        return node;
    }

    protected override Expression VisitDynamic(DynamicExpression node) => node;

    protected override Expression VisitExtension(Expression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitExtension(node);
    }

    protected override Expression VisitGoto(GotoExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitGoto(node);
    }

    protected override Expression VisitIndex(IndexExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitIndex(node);
    }

    protected override Expression VisitInvocation(InvocationExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitInvocation(node);
    }

    protected override Expression VisitLabel(LabelExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitLabel(node);
    }

    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitLambda<T>(node);
    }

    protected override Expression VisitListInit(ListInitExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitListInit(node);
    }

    protected override Expression VisitLoop(LoopExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitLoop(node);
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitMethodCall(node);
    }

    protected override Expression VisitNew(NewExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitNew(node);
    }

    protected override Expression VisitNewArray(NewArrayExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitNewArray(node);
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        VisitingQueue.Enqueue(node);
        return node;
    }

    protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node) => node;

    protected override Expression VisitSwitch(SwitchExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitSwitch(node);
    }

    protected override Expression VisitTry(TryExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitTry(node);
    }

    protected override Expression VisitTypeBinary(TypeBinaryExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitTypeBinary(node);
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        VisitingQueue.Enqueue(node);
        return base.VisitUnary(node);
    }
}
