namespace vm2.ExpressionSerialization.ExpressionsDeepEquals;

class DeepEqualsVisitor : ExpressionVisitor
{
    Queue<object?> _rNodes;
    Expression _right;

    public bool Equal { get; private set; } = true;

    public string Difference { get; private set; } = "";

    public DeepEqualsVisitor(Expression right)
    {
        _right = right;

        var qVisitor = new EnqueueingVisitor();

        qVisitor.Visit(_right);
        _rNodes = qVisitor.VisitingQueue;
    }

    bool GetRight<T>(T? left, out T? right) where T : class
    {
        right = default;

        if (!(Equal &= _rNodes.Count != 0 &&
                       (_rNodes.Peek()?.GetType()?.IsAssignableTo(typeof(T)) ?? false)))    // if the right node is there and has the same type - equal is true so far
        {
            if (_rNodes.Count == 0)
                Difference = $"The left node is {left} but there is no right sub-node.";
            else
            {
                var r = _rNodes.Peek();
                if (r is null)
                    Difference = $"The left node is {left} but the right node is null.";
                else
                    Difference = $"The left node is {left} but the right node is of different type ({r.GetType().Name}): {right}.";
            }
            return false;
        }
        right = _rNodes.Dequeue() as T;

        return Equal;
    }

    bool AreEqual<T>(T? left, T? right)
    {
        if (Equal)
            return true;

        Difference += $"Difference at sub-nodes: `{left}` != `{right}` ";
        return false;
    }

    protected override Expression VisitBinary(BinaryExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<BinaryExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.IsLifted == right.IsLifted
            && left.IsLiftedToNull == right.IsLiftedToNull
            && left.Method == right.Method
            ;

        return AreEqual(left, right) ? base.VisitBinary(left) : left;
    }

    protected override Expression VisitBlock(BlockExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<BlockExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Variables.Count == right.Variables.Count
            && left.Expressions.Count == right.Expressions.Count
            ;

        return AreEqual(left, right) ? base.VisitBlock(left) : left;
    }

    protected override Expression VisitConditional(ConditionalExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<ConditionalExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            ;
        return AreEqual(left, right) ? base.VisitConditional(left) : left;
    }

    protected override Expression VisitConstant(ConstantExpression left)
    {
        if (!Equal)
            return left;

        Equal = GetRight<ConstantExpression>(left, out var right);

        if (!Equal)
            return left;

        Equal =
            right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Value is null == right.Value is null
            ;
        if (!AreEqual(left, right) || left.Value is null || left.Type == typeof(object))   // either different or both null or both System.Object
            return left;

        var lValue = left.Value;
        var rValue = right?.Value;

        Debug.Assert(lValue is not null);
        Debug.Assert(rValue is not null);

        IEnumerable? enumL = null;
        IEnumerable? enumR = null;

        if (lValue is not IEnumerable)
        {
            var genType = left.Type.IsGenericType ? left.Type.GetGenericTypeDefinition() : null;

            if (genType is not null)
            {
                var elemType = left.Type.GetGenericArguments()[0];

                if (genType == typeof(Memory<>) || genType == typeof(ReadOnlyMemory<>))
                {
                    var mi = lValue.GetType().GetMethod("ToArray") ?? throw new InternalTransformErrorException("Could not get the property for Memory<>.Span.");

                    if (mi.IsGenericMethod)
                        mi = mi.MakeGenericMethod(elemType);

                    enumL = mi.Invoke(lValue, []) as IEnumerable;
                    enumR = mi.Invoke(rValue, []) as IEnumerable;
                }
            }
        }
        else
        if (left.Type.IsArray || left.Type.Namespace?.StartsWith("System") is true)
        {
            enumL = (lValue as IEnumerable)?.Cast<object?>();
            enumR = (rValue as IEnumerable)?.Cast<object?>();
        }

        if (enumL is not null && enumR is not null)
        {
            Equal = enumL.Cast<object>().Count() == enumL.Cast<object>().Count();
            if (!AreEqual(left, right))
                return left;

            var itrL = enumL.GetEnumerator();
            var itrR = enumR.GetEnumerator();

            while (Equal && itrL.MoveNext() && itrR.MoveNext())
                Equal &= Equals(itrL.Current, itrR.Current);
        }
        else
            Equal &= Equals(lValue, rValue);

        AreEqual(left, right);
        return left; // visiting constant expression gives us nothing so do not go there
    }

    protected override Expression VisitDefault(DefaultExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<DefaultExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            ;
        AreEqual(left, right);
        return left;   // visiting default expression gives us nothing so do not go there
    }

    protected override Expression VisitGoto(GotoExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<GotoExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Kind == right.Kind
            ;
        return AreEqual(left, right) ? base.VisitGoto(left) : left;
    }

    protected override Expression VisitLabel(LabelExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<LabelExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.DefaultValue is null == right.DefaultValue is null
            ;
        return AreEqual(left, right) ? base.VisitLabel(left) : left;
    }

    protected override LabelTarget? VisitLabelTarget(LabelTarget? left)
    {
        if (!Equal)
            return left;

        Equal &= GetRight<LabelTarget>(left, out var right);

        if (!AreEqual(left, right))
            return left;

        Equal &= left is null == right is null;

        if (!AreEqual(left, right) || left is null)
            return left;

        Debug.Assert(left is not null);
        Debug.Assert(right is not null);

        Equal &=
            left.Name == right.Name
            && left.Type == right.Type
            ;

        AreEqual(left, right);
        return left;   // visiting label target gives us nothing so do not go there
    }

    protected override Expression VisitIndex(IndexExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<IndexExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Arguments.Count == right.Arguments.Count
            && left.Indexer == right.Indexer
            && left.Object is null == right.Object is null
            ;
        return AreEqual(left, right) ? base.VisitIndex(left) : left;
    }

    protected override Expression VisitInvocation(InvocationExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<InvocationExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Arguments.Count == right.Arguments.Count
            ;
        return AreEqual(left, right) ? base.VisitInvocation(left) : left;
    }

    protected override Expression VisitLambda<T>(Expression<T> left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<Expression<T>>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.ReturnType == right.ReturnType
            && left.Parameters.Count == right.Parameters.Count
            && left.TailCall == right.TailCall
            ;
        return AreEqual(left, right) ? base.VisitLambda<T>(left) : left;
    }

    protected override Expression VisitListInit(ListInitExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<ListInitExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Initializers.Count == right.Initializers.Count
            ;
        return AreEqual(left, right) ? base.VisitListInit(left) : left;
    }

    protected override Expression VisitLoop(LoopExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<LoopExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.BreakLabel is null == right.BreakLabel is null
            && left.ContinueLabel is null == right.ContinueLabel is null
            ;
        return AreEqual(left, right) ? base.VisitLoop(left) : left;
    }

    protected override Expression VisitMember(MemberExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<MemberExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Member == right.Member
            ;
        return AreEqual(left, right) ? base.VisitMember(left) : left;
    }

    protected override Expression VisitMemberInit(MemberInitExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<MemberInitExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Bindings.Count == right.Bindings.Count
            ;
        return AreEqual(left, right) ? base.VisitMemberInit(left) : left;
    }

    protected override MemberAssignment VisitMemberAssignment(MemberAssignment left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<MemberAssignment>(left, out var right)
            && right is not null
            && left.BindingType == right.BindingType
            && left.Member == right.Member
            ;
        return AreEqual(left, right) ? base.VisitMemberAssignment(left) : left;
    }

    protected override MemberListBinding VisitMemberListBinding(MemberListBinding left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<MemberListBinding>(left, out var right)
            && right is not null
            && left.BindingType == right.BindingType
            && left.Member == right.Member
            && left.Initializers.Count == right.Initializers.Count
            ;
        return AreEqual(left, right) ? base.VisitMemberListBinding(left) : left;
    }

    protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<MemberMemberBinding>(left, out var right)
            && right is not null
            && left.BindingType == right.BindingType
            && left.Member == right.Member
            && left.Bindings.Count == right.Bindings.Count
            ;
        return AreEqual(left, right) ? base.VisitMemberMemberBinding(left) : left;
    }

    protected override Expression VisitMethodCall(MethodCallExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<MethodCallExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Method == right.Method
            && left.Arguments.Count == right.Arguments.Count
            && left.Object is null == right.Object is null
            ;
        return AreEqual(left, right) ? base.VisitMethodCall(left) : left;
    }

    protected override Expression VisitNew(NewExpression left)
    {
        if (!Equal || !GetRight<NewExpression>(left, out var right))
            return left;

        Debug.Assert(right is not null);

        Equal &=
            left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Constructor == right.Constructor
            && left.Arguments.Count == right.Arguments.Count
            && left.Members is null == right.Members is null
            ;

        if (!AreEqual(left, right))
            return left;

        if (left.Members is not null && right.Members is not null)
        {
            Debug.Assert(left.Members is not null);
            Debug.Assert(right.Members is not null);

            Equal &= left.Members.SequenceEqual(right.Members);
        }

        return AreEqual(left, right) ? base.VisitNew(left) : left;
    }

    protected override Expression VisitNewArray(NewArrayExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<NewArrayExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Expressions?.Count == right.Expressions?.Count
            ;
        return AreEqual(left, right) ? base.VisitNewArray(left) : left;
    }

    protected override ElementInit VisitElementInit(ElementInit left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<ElementInit>(left, out var right)
            && right is not null
            && left.Arguments.Count == right.Arguments.Count
            ;
        return AreEqual(left, right) ? base.VisitElementInit(left) : left;
    }

    protected override Expression VisitParameter(ParameterExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<ParameterExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.IsByRef == right.IsByRef
            && left.Name == right.Name
            ;
        AreEqual(left, right);
        return left;     // visiting parameter expression gives us nothing so do not go there
    }

    protected override Expression VisitSwitch(SwitchExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<SwitchExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Comparison == right.Comparison
            && left.DefaultBody is null == right.DefaultBody is null
            && left.Cases.Count == right.Cases.Count
            ;
        return AreEqual(left, right) ? base.VisitSwitch(left) : left;
    }

    protected override SwitchCase VisitSwitchCase(SwitchCase left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<SwitchCase>(left, out var right)
            && right is not null
            && left.TestValues.Count == right.TestValues.Count
            ;
        return AreEqual(left, right) ? base.VisitSwitchCase(left) : left;
    }

    protected override Expression VisitTry(TryExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<TryExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.Finally is null == right.Finally is null
            && left.Fault is null == right.Fault is null
            && left.Handlers.Count == right.Handlers.Count
            ;
        return AreEqual(left, right) ? base.VisitTry(left) : left;
    }

    protected override CatchBlock VisitCatchBlock(CatchBlock left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<CatchBlock>(left, out var right)
            && right is not null
            && left.Test == right.Test
            && left.Variable is null == right.Variable is null
            && left.Filter is null == right.Filter is null
            ;
        return AreEqual(left, right) ? base.VisitCatchBlock(left) : left;
    }

    protected override Expression VisitTypeBinary(TypeBinaryExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<TypeBinaryExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.TypeOperand == right.TypeOperand
            ;
        return AreEqual(left, right) ? base.VisitTypeBinary(left) : left;
    }

    protected override Expression VisitUnary(UnaryExpression left)
    {
        if (!Equal)
            return left;

        Equal &=
            GetRight<UnaryExpression>(left, out var right)
            && right is not null
            && left.NodeType == right.NodeType
            && left.Type == right.Type
            && left.IsLifted == right.IsLifted
            && left.IsLiftedToNull == right.IsLiftedToNull
            && left.Method == right.Method
            ;
        return AreEqual(left, right) ? base.VisitUnary(left) : left;
    }

    ///////////////////////////////////////////////////////////////
    // Won't do:
    ///////////////////////////////////////////////////////////////

    protected override Expression VisitExtension(Expression left) => left;

    protected override Expression VisitDebugInfo(DebugInfoExpression left) => left;

    protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression left) => left;

    protected override Expression VisitDynamic(DynamicExpression left) => left;
}
