namespace vm2.ExpressionSerialization.ExpressionsDeepEquals;

class DeepEqualsVisitor : ExpressionVisitor
{
    Queue<object?> _rNodes;
    Expression _right;

    public bool Equal { get; private set; } = true;

    public DeepEqualsVisitor(Expression right)
    {
        _right = right;

        var qVisitor = new EnqueueingVisitor();

        qVisitor.Visit(_right);
        _rNodes = qVisitor.VisitingQueue;
    }

    bool GetRight<T>(out T? right) where T : class
    {
        right = default;

        if (!(Equal &= _rNodes.Count != 0 &&
                       _rNodes.Peek() is T))    // if the right node is there and has the same type - equal is true so far
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
            && left.IsLiftedToNull == right.IsLiftedToNull
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
        Equal = GetRight<ConstantExpression>(out var right);
        if (!Equal)
            return left;

        Equal =
            right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Value is null == right.Value is null
            ;
        if (!Equal || left.Value is null)   // either different or both null
            return left;

        Debug.Assert(left.Value is not null);
        Debug.Assert(right?.Value is not null);

        if (left.Value is IEnumerable enumL && right.Value is IEnumerable enumR &&
            (left.Type.IsArray || left.Type.Namespace?.StartsWith("System") is true))
        {
            Equal = enumL.Cast<object>().Count() == enumL.Cast<object>().Count();
            if (!Equal)
                return left;

            var itrL = enumL.GetEnumerator();
            var itrR = enumR.GetEnumerator();

            while (itrL.MoveNext() && itrR.MoveNext())
                Equal &= itrL.Current.Equals(itrR.Current);
        }
        else
            Equal = left.Value.Equals(right.Value);

        return left; // visiting constant expression gives us nothing so do not go there
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
        return left;   // visiting default expression gives us nothing so do not go there
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

        return left;   // visiting label target gives us nothing so do not go there
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
            && left.Member == right.Member
            ;
        return Equal ? base.VisitMember(left) : left;
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

    protected override MemberListBinding VisitMemberListBinding(MemberListBinding left)
    {
        Equal &=
            Equal
            && GetRight<MemberListBinding>(out var right)
            && right is not null
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
            && left.Members is null == right.Members is null
            ;

        if (!Equal)
            return left;

        if (left.Members is not null && right.Members is not null)
        {
            Debug.Assert(left.Members is not null);
            Debug.Assert(right.Members is not null);

            Equal &= left.Members.SequenceEqual(right.Members);
        }

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
        return left;     // visiting parameter expression gives us nothing so do not go there
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
            && left.IsLiftedToNull == right.IsLiftedToNull
            && left.Method == right.Method
            ;
        return Equal ? base.VisitUnary(left) : left;
    }

    ///////////////////////////////////////////////////////////////
    // Won't do:
    ///////////////////////////////////////////////////////////////

    protected override Expression VisitExtension(Expression left) => left;

    protected override Expression VisitDebugInfo(DebugInfoExpression left) => left;

    protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression left) => left;

    protected override Expression VisitDynamic(DynamicExpression left) => left;
}
