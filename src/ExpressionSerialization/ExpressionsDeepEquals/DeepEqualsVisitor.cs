namespace vm2.ExpressionSerialization.ExpressionsDeepEquals;
class DeepEqualsVisitor(Expression right) : ExpressionVisitor
{
    Queue<object?>? _rNodes;
    Expression _right = right;

    public bool Equal { get; private set; } = true;

    /// <summary>
    /// Dispatches the expression to one of the more specialized visit methods in this class.
    /// </summary>
    /// <param name="left">The expression to visit.</param>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
    public override Expression? Visit(Expression? left)
    {
        if (left is null)
        {
            Equal = _right is null;
            return left;
        }
        if (_right is null)
        {
            Equal = false;
            return left;
        }
        if (ReferenceEquals(left, _right))
            return left;

        if (_rNodes is null)
        {
            // this is the beginning of the visiting process - initialize the queue
            var qVisitor = new EnqueueingVisitor();
            qVisitor.Visit(_right);
            _rNodes = qVisitor.VisitingQueue;
        }

        return base.Visit(left);
    }

    bool GetRight<T>(out T? right) where T : class
    {
        // initialize right with null
        right = null;

        Debug.Assert(_rNodes is not null);

        Equal &= _rNodes.Count != 0;

        if (!Equal)
            return false;

        right = _rNodes.Dequeue() as T;
        return Equal;
    }

    protected override Expression VisitBinary(BinaryExpression left)
    {
        Equal &=
            Equal
            && GetRight<BinaryExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.IsLifted == right.IsLifted
            && left.IsLiftedToNull == right.IsLiftedToNull  // TODO: in serialization/deserialization
            && left.Method == right.Method
            ;

        return Equal ? base.VisitBinary(left) : left;
    }

    protected override Expression VisitBlock(BlockExpression left)
    {
        Equal &=
            Equal
            && GetRight<BlockExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Variables.Count == right.Variables.Count
            && left.Expressions.Count == right.Expressions.Count
            ;

        return Equal ? base.VisitBlock(left) : left;
    }

    protected override Expression VisitConditional(ConditionalExpression left)
    {
        Equal &=
            Equal
            && GetRight<ConditionalExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            ;
        return Equal ? base.VisitConditional(left) : left;
    }

    protected override Expression VisitConstant(ConstantExpression left)
    {
        if (!Equal)
            return left;

        Equal &= GetRight<ConstantExpression>(out var right);

        if (!Equal)
            return left;

        Equal &=
            right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Value is null == right.Value is null
            ;

        if (!Equal)
            return left;

        Debug.Assert(right is not null);

        if (left.Value is null && right.Value is null)
            return left;

        Debug.Assert(left.Value is not null);
        Debug.Assert(right.Value is not null);

        Equal &=
            left.Value.GetType() == right.Value.GetType()
            && left.Value.Equals(right.Value)
            ;

        return Equal ? base.VisitConstant(left) : left;
    }

    protected override Expression VisitDefault(DefaultExpression left)
    {
        Equal &=
            Equal
            && GetRight<DefaultExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            ;
        return Equal ? base.VisitDefault(left) : left;
    }

    protected override Expression VisitGoto(GotoExpression left)
    {
        Equal &=
            Equal
            && GetRight<GotoExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Kind == right.Kind
            ;
        return Equal ? base.VisitGoto(left) : left;
    }

    protected override Expression VisitLabel(LabelExpression left)
    {
        Equal &=
            Equal
            && GetRight<LabelExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.DefaultValue is null == right.DefaultValue is null
            ;
        return Equal ? base.VisitLabel(left) : left;
    }

    protected override LabelTarget? VisitLabelTarget(LabelTarget? left)
    {
        if (!Equal)
            return left;

        Equal &= GetRight<LabelTarget>(out var right);

        if (!Equal)
            return left;

        Equal &= left is null == right is null;

        if (!Equal || left is null)
            return left;

        Debug.Assert(left is not null);
        Debug.Assert(right is not null);

        Equal &=
            left.Name == right.Name
            && left.Type == right.Type
            ;

        return Equal ? base.VisitLabelTarget(left) : left;
    }

    protected override Expression VisitIndex(IndexExpression left)
    {
        Equal &=
            Equal
            && GetRight<IndexExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Arguments.Count == right.Arguments.Count
            && left.Indexer == right.Indexer
            && left.Object is null == right.Object is null
            ;
        return Equal ? base.VisitIndex(left) : left;
    }

    protected override Expression VisitInvocation(InvocationExpression left)
    {
        Equal &=
            Equal
            && GetRight<InvocationExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Arguments.Count == right.Arguments.Count
            ;
        return Equal ? base.VisitInvocation(left) : left;
    }

    protected override Expression VisitLambda<T>(Expression<T> left)
    {
        Equal &=
            Equal
            && GetRight<Expression<T>>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.ReturnType == right.ReturnType
            && left.Parameters.Count == right.Parameters.Count
            && left.TailCall == right.TailCall
            ;
        return Equal ? base.VisitLambda<T>(left) : left;
    }

    protected override Expression VisitListInit(ListInitExpression left)
    {
        Equal &=
            Equal
            && GetRight<ListInitExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Initializers.Count == right.Initializers.Count
            ;
        return Equal ? base.VisitListInit(left) : left;
    }

    protected override Expression VisitLoop(LoopExpression left)
    {
        Equal &=
            Equal
            && GetRight<LoopExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.BreakLabel is null == right.BreakLabel is null
            && left.ContinueLabel is null == right.ContinueLabel is null
            ;
        return Equal ? base.VisitLoop(left) : left;
    }

    protected override Expression VisitMember(MemberExpression left)
    {
        Equal &=
            Equal
            && GetRight<MemberExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Member == left.Member
            ;
        return Equal ? base.VisitMember(left) : left;
    }

    protected override MemberAssignment VisitMemberAssignment(MemberAssignment left)
    {
        Equal &=
            Equal
            && GetRight<MemberAssignment>(out var right)
            && right is not null
            && left.BindingType == right.BindingType
            && left.Member == right.Member
            ;
        return Equal ? base.VisitMemberAssignment(left) : left;
    }

    protected override Expression VisitMemberInit(MemberInitExpression left)
    {
        Equal &=
            Equal
            && GetRight<MemberInitExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Bindings.Count == right.Bindings.Count
            ;
        return Equal ? base.VisitMemberInit(left) : left;
    }

    protected override MemberBinding VisitMemberBinding(MemberBinding left)
    {
        Equal &=
            Equal
            && GetRight<MemberBinding>(out var right)
            && right is not null
            && left.BindingType == right.BindingType
            && left.Member == right.Member
            ;
        return Equal ? base.VisitMemberBinding(left) : left;
    }

    protected override MemberListBinding VisitMemberListBinding(MemberListBinding left)
    {
        Equal &=
            Equal
            && GetRight<MemberListBinding>(out var right)
            && right is not null
            // TODO: aren't the next 2 already done in VisitMemberBinding?
            && left.BindingType == right.BindingType
            && left.Member == right.Member
            && left.Initializers.Count == right.Initializers.Count
            ;
        return Equal ? base.VisitMemberListBinding(left) : left;
    }

    protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding left)
    {
        Equal &=
            Equal
            && GetRight<MemberMemberBinding>(out var right)
            && right is not null
            // TODO: aren't the next 2 already done in VisitMemberBinding?
            && left.BindingType == right.BindingType
            && left.Member == right.Member
            && left.Bindings.Count == right.Bindings.Count
            ;
        return Equal ? base.VisitMemberMemberBinding(left) : left;
    }

    protected override Expression VisitMethodCall(MethodCallExpression left)
    {
        Equal &=
            Equal
            && GetRight<MethodCallExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Method == right.Method
            && left.Arguments.Count == right.Arguments.Count
            && left.Object is null == right.Object is null
            ;
        return Equal ? base.VisitMethodCall(left) : left;
    }

    protected override Expression VisitNew(NewExpression left)
    {
        if (!Equal || !GetRight<NewExpression>(out var right))
            return left;

        Debug.Assert(right is not null);

        Equal &=
            left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Constructor == right.Constructor
            && left.Arguments.Count == right.Arguments.Count
            && left.Members is null == right.Members is null    // TODO: in serialization/deserialization
            ;

        if (!Equal || left.Members is null)
            return left;

        Debug.Assert(left.Members is not null);
        Debug.Assert(right.Members is not null);

        Equal &= left.Members.SequenceEqual(right.Members);

        return Equal ? base.VisitNew(left) : left;
    }

    protected override Expression VisitNewArray(NewArrayExpression left)
    {
        Equal &=
            Equal
            && GetRight<NewArrayExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Expressions?.Count == right.Expressions?.Count
            ;
        return Equal ? base.VisitNewArray(left) : left;
    }

    protected override ElementInit VisitElementInit(ElementInit left)
    {
        Equal &=
            Equal
            && GetRight<ElementInit>(out var right)
            && right is not null
            && left.Arguments.Count == right.Arguments.Count
            ;
        return Equal ? base.VisitElementInit(left) : left;
    }

    protected override Expression VisitParameter(ParameterExpression left)
    {
        Equal &=
            Equal
            && GetRight<ParameterExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.IsByRef == right.IsByRef
            && left.Name == right.Name
            ;
        return Equal ? base.VisitParameter(left) : left;
    }

    protected override Expression VisitSwitch(SwitchExpression left)
    {
        Equal &=
            Equal
            && GetRight<SwitchExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Comparison == right.Comparison
            && left.DefaultBody is null == right.DefaultBody is null
            && left.Cases.Count == right.Cases.Count
            ;
        return Equal ? base.VisitSwitch(left) : left;
    }

    protected override SwitchCase VisitSwitchCase(SwitchCase left)
    {
        Equal &=
            Equal
            && GetRight<SwitchCase>(out var right)
            && right is not null
            && left.TestValues.Count == right.TestValues.Count
            ;
        return Equal ? base.VisitSwitchCase(left) : left;
    }

    protected override Expression VisitTry(TryExpression left)
    {
        Equal &=
            Equal
            && GetRight<TryExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Finally is null == right.Finally is null
            && left.Fault is null == right.Fault is null
            && left.Handlers.Count == right.Handlers.Count
            ;
        return Equal ? base.VisitTry(left) : left;
    }

    protected override CatchBlock VisitCatchBlock(CatchBlock left)
    {
        Equal &=
            Equal
            && GetRight<CatchBlock>(out var right)
            && right is not null
            && left.Test == right.Test
            && left.Variable is null == right.Variable is null
            && left.Filter is null == right.Filter is null
            ;
        return Equal ? base.VisitCatchBlock(left) : left;
    }

    protected override Expression VisitTypeBinary(TypeBinaryExpression left)
    {
        Equal &=
            Equal
            && GetRight<TypeBinaryExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.TypeOperand == right.TypeOperand
            ;
        return Equal ? base.VisitTypeBinary(left) : left;
    }

    protected override Expression VisitUnary(UnaryExpression left)
    {
        Equal &=
            Equal
            && GetRight<UnaryExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.IsLifted == right.IsLifted
            && left.IsLiftedToNull == right.IsLiftedToNull  // TODO: in serialization/deserialization
            && left.Method == right.Method
            ;
        return Equal ? base.VisitUnary(left) : left;
    }

    ///////////////////////////////////////////////////////////////
    // Won't do:
    ///////////////////////////////////////////////////////////////

    protected override Expression VisitExtension(Expression left)
    {
        Equal &=
            Equal
            && GetRight<Expression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            ;
        return Equal ? base.VisitExtension(left) : left;
    }

    protected override Expression VisitDebugInfo(DebugInfoExpression left)
    {
        Equal &=
            Equal
            && GetRight<BlockExpression>(out var right)
            ;
        return Equal ? base.VisitDebugInfo(left) : left;
    }

    protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression left)
    {
        Equal &=
            Equal
            && GetRight<RuntimeVariablesExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            ;
        return Equal ? base.VisitRuntimeVariables(left) : left;
    }

    protected override Expression VisitDynamic(DynamicExpression left)
    {
        Equal &=
            Equal
            && GetRight<DynamicExpression>(out var right)
            && right is not null
            && left.NodeType == right.NodeType
            ;
        return Equal ? base.VisitDynamic(left) : left;
    }
}
