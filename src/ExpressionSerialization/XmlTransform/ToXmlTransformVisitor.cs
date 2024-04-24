namespace vm2.ExpressionSerialization.XmlTransform;

using System.Linq.Expressions;
using System.Xml.Linq;

/// <summary>
/// Class XmlTransformVisitor.
/// Implements the <see cref="ExpressionTransformVisitor{XNode}" />
/// </summary>
/// <seealso cref="ExpressionTransformVisitor{XNode}" />
public partial class ToXmlTransformVisitor(Options? options = null) : ExpressionTransformVisitor<XElement>
{
    /// <summary>
    /// The transform options.
    /// </summary>
    protected Options _options = options ?? new();

    /// <summary>
    /// Gets a properly named n corresponding to the current expression n.
    /// </summary>
    /// <param name="node">The currently visited expression node from the expression tree.</param>
    /// <returns>TNode.</returns>
    protected override XElement GetEmptyNode(Expression node)
        => new(Namespaces.Exs + Transform.Identifier(node.NodeType.ToString(), IdentifierConventions.Camel),
                node switch {
                    LambdaExpression n => new XAttribute(AttributeNames.Type, Transform.TypeName(n.ReturnType)),
                    ConstantExpression => null,
                    _ => new XAttribute(AttributeNames.Type, Transform.TypeName(node.Type)),
                });

    DataTransform _dataTransform = new(options);

    /// <inheritdoc/>
    protected override Expression VisitConstant(ConstantExpression node)
    {
        try
        {
            return GenericVisit(
                     node,
                     base.VisitConstant,
                     (n, e) =>
                     {
                         _options.AddComment(e, n);
                         e.Add(
                            _options.TypeComment(n.Type),
                            _dataTransform.TransformNode(n));
                     });
        }
        catch (Exception x)
        {
            throw new NonSerializableObjectException(node.Type, x);
        }
    }

    /// <inheritdoc/>
    protected override Expression VisitDefault(DefaultExpression node)
        => GenericVisit(
            node,
            base.VisitDefault,
            (n, e) => e.Add(
                        _options.TypeComment(n.Type)));

    /// <inheritdoc/>
    protected override Expression VisitParameter(ParameterExpression node)
        => GenericVisit(
            node,
            base.VisitParameter,
            (n, e) => e.Add(
                        new XAttribute(AttributeNames.Name, node.Name ?? "_"),
                        n.IsByRef ? new XAttribute(AttributeNames.IsByRef, n.IsByRef) : null));

    /// <inheritdoc/>
    protected override Expression VisitLambda<T>(Expression<T> node)
        => GenericVisit(
            node,
            base.VisitLambda,
            (n, e) => e.Add(
                        n.TailCall ? new XAttribute(AttributeNames.TailCall, n.TailCall) : null,
                        string.IsNullOrWhiteSpace(n.Name) ? null : new XAttribute(AttributeNames.Name, n.Name),
                        n.ReturnType == n.Body.Type ? null : new XAttribute(AttributeNames.DelegateType, n.ReturnType),
                        new XElement(ElementNames.Parameters, PopElements(n.Parameters.Count)),
                        new XElement(ElementNames.Body, _elements.Pop())));

    /// <inheritdoc/>
    protected override Expression VisitUnary(UnaryExpression node)
        => GenericVisit(
            node,
            base.VisitUnary,
            (n, e) => e.Add(
                        _elements.Pop(),
                        VisitMethodInfo(n)));

    /// <inheritdoc/>
    protected override Expression VisitBinary(BinaryExpression node)
        => GenericVisit(
            node,
            base.VisitBinary,
            (n, e) => e.Add(
                        n.IsLiftedToNull ? new XAttribute(AttributeNames.IsLiftedToNull, true) : null,
                        PopElements(2),
                        VisitMethodInfo(n)));

    /// <inheritdoc/>
    protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        => GenericVisit(
            node,
            base.VisitTypeBinary,
            (n, e) => e.Add(
                        new XAttribute(AttributeNames.TypeOperand, Transform.TypeName(n.TypeOperand)),
                        _elements.Pop()));

    /// <inheritdoc/>
    protected override Expression VisitIndex(IndexExpression node)
        => GenericVisit(
            node,
            base.VisitIndex,
            (n, e) =>
            {
                var indexes = new XElement(ElementNames.Indexes, PopElements(n.Arguments.Count));
                e.Add(
                    _elements.Pop(),
                    indexes);
            });

    /// <inheritdoc/>
    protected override ElementInit VisitElementInit(ElementInit node)
    {
        var elementInit = base.VisitElementInit(node);

        _elements.Push(
            new XElement(
                    ElementNames.ElementInit,
                    VisitMemberInfo(node.AddMethod),
                    new XElement(ElementNames.Arguments, PopElements(node.Arguments.Count))));

        return elementInit;
    }

    /// <inheritdoc/>
    protected override Expression VisitMember(MemberExpression node)
        => GenericVisit(
            node,
            base.VisitMember,
            (n, e) =>
                e.Add(
                    _elements.Pop(),
                    VisitMemberInfo(n.Member)));

    #region Member Bindings
    /// <inheritdoc/>
    protected override MemberBinding VisitMemberBinding(MemberBinding node)
        => base.VisitMemberBinding(node);

    /// <inheritdoc/>
    protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
    {
        var binding = base.VisitMemberAssignment(node);

        _elements.Push(
            new XElement(
                    ElementNames.AssignmentBinding,
                    VisitMemberInfo(node.Member),
                    _elements.Pop()));

        return binding;
    }

    /// <inheritdoc/>
    protected override MemberMemberBinding VisitMemberMemberBinding(MemberMemberBinding node)
    {
        var binding = base.VisitMemberMemberBinding(node);

        _elements.Push(
            new XElement(
                    ElementNames.MemberMemberBinding,
                    VisitMemberInfo(node.Member),
                    new XElement(ElementNames.Bindings, PopElements(node.Bindings.Count))));

        return binding;
    }

    /// <inheritdoc/>
    protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
    {
        var binding = base.VisitMemberListBinding(node);

        _elements.Push(
            new XElement(
                    ElementNames.ListBinding,
                    VisitMemberInfo(node.Member),
                    _elements.Pop()));

        return binding;
    }

    /// <inheritdoc/>
    protected override Expression VisitMemberInit(MemberInitExpression node)
        => GenericVisit(
            node,
            base.VisitMemberInit,
            (n, e) => e.Add(
                        _elements.Pop(),        // the new expression
                        new XElement(
                                ElementNames.Bindings,
                                PopElements(n.Bindings.Count))));
    #endregion

    /// <inheritdoc/>
    protected override Expression VisitMethodCall(MethodCallExpression node)
        => GenericVisit(
            node,
            base.VisitMethodCall,
            (n, e) =>
            {
                var arguments = new XElement(ElementNames.Arguments, PopElements(n.Arguments.Count));
                var instance = n.Object!=null ? _elements.Pop() : null;
                var method = VisitMemberInfo(n.Method);

                e.Add(
                    instance,
                    method,
                    arguments);
            });

    /// <inheritdoc/>
    protected override Expression VisitInvocation(InvocationExpression node)
        => GenericVisit(
            node,
            base.VisitInvocation,
            (n, e) =>
            {
                var arguments = new XElement(ElementNames.Arguments, PopElements(n.Arguments.Count));

                e.Add(
                    _elements.Pop(),
                    arguments);
            });

    /// <inheritdoc/>
    protected override Expression VisitBlock(BlockExpression node)
        => GenericVisit(
            node,
            base.VisitBlock,
            (n, e) => e.Add(
                        new XElement(ElementNames.Variables, PopElements(n.Variables.Count)),
                        PopElements(n.Expressions.Count)));

    /// <inheritdoc/>
    protected override Expression VisitConditional(ConditionalExpression node)
        => GenericVisit(
            node,
            base.VisitConditional,
            (n, e) => e.Add(PopElements(3)));

    /////////////////////////////////////////////////////////////////
    // IN PROCESS:
    /////////////////////////////////////////////////////////////////

    /// <inheritdoc/>
    protected override Expression VisitNew(NewExpression node)
        => GenericVisit(
            node,
            base.VisitNew,
            (n, e) => e.Add(
                        VisitMemberInfo(n.Constructor!),
                        new XElement(ElementNames.Arguments, PopElements(n.Arguments.Count)),
                        n.Members is null
                            ? null
                            : new XElement(ElementNames.Members, n.Members.Select(m => VisitMemberInfo(m)))));

    /// <inheritdoc/>
    protected override Expression VisitLoop(LoopExpression node)
        => GenericVisit(
            node,
            base.VisitLoop,
            (n, e) => { });

    /////////////////////////////////////////////////////////////////
    // TODO:
    /////////////////////////////////////////////////////////////////

    /// <inheritdoc/>
    protected override Expression VisitGoto(GotoExpression node)
        => GenericVisit(
            node,
            base.VisitGoto,
            (n, e) => { });

    /// <inheritdoc/>
    protected override Expression VisitLabel(LabelExpression node)
        => GenericVisit(
            node,
            base.VisitLabel,
            (n, e) => { });

    /// <inheritdoc/>
    protected override Expression VisitListInit(ListInitExpression node)
        => GenericVisit(
            node,
            base.VisitListInit,
            (n, e) => { });

    /// <inheritdoc/>
    protected override Expression VisitNewArray(NewArrayExpression node)
        => GenericVisit(
            node,
            base.VisitNewArray,
            (n, e) => { });

    /// <inheritdoc/>
    protected override Expression VisitSwitch(SwitchExpression node)
        => GenericVisit(
            node,
            base.VisitSwitch,
            (n, e) => { });

    /// <inheritdoc/>
    protected override Expression VisitTry(TryExpression node)
        => GenericVisit(
            node,
            base.VisitTry,
            (n, e) => { });

    /// <inheritdoc/>
    protected override CatchBlock VisitCatchBlock(CatchBlock node) => base.VisitCatchBlock(node);

    /// <inheritdoc/>
    protected override LabelTarget? VisitLabelTarget(LabelTarget? node) => base.VisitLabelTarget(node);

    /// <inheritdoc/>
    protected override SwitchCase VisitSwitchCase(SwitchCase node) => base.VisitSwitchCase(node);

    /////////////////////////////////////////////////////////////////
    // WOUN'T DO:
    /////////////////////////////////////////////////////////////////

    /// <inheritdoc/>
    protected override Expression VisitDebugInfo(DebugInfoExpression node)
        => GenericVisit(
            node,
            base.VisitDebugInfo,
            (n, e) => throw new UnexpectedExpressionException(n.GetType().Name));

    /// <inheritdoc/>
    protected override Expression VisitDynamic(DynamicExpression node)
        => GenericVisit(
            node,
            base.VisitDynamic,
            (n, e) => throw new UnexpectedExpressionException(n.GetType().Name));

    /// <inheritdoc/>
    protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        => GenericVisit(
            node,
            base.VisitRuntimeVariables,
            (n, e) => throw new UnexpectedExpressionException(n.GetType().Name));

    /// <inheritdoc/>
    protected override Expression VisitExtension(Expression node)
        => GenericVisit(
            node,
            base.VisitExtension,
            (n, e) => throw new UnexpectedExpressionException($"{nameof(ExpressionVisitor)}.{VisitExtension}(Expression node)"));
}
