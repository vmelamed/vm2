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

    DataTransform _dataTransform = new(options);

    // labels and parameters/variables are created in one expression node and references to them are used in another.
    // These dictionaries keep their id-s so we can create `XAttribute` id-s and idref-s to them.
    Dictionary<LabelTarget, string> _labelTargets = [];
    Dictionary<ParameterExpression, string> _parameters = [];
    int _lastLabelIdNumber;
    int _lastParamIdNumber;

    /// <inheritdoc/>>
    protected override void Reset()
    {
        base.Reset();

        _parameters = [];
        _labelTargets = [];
        _lastParamIdNumber = 0;
        _lastLabelIdNumber = 0;
    }

    /// <summary>
    /// Gets a properly named n corresponding to the current expression n.
    /// </summary>
    /// <param name="node">The currently visited expression n from the expression tree.</param>
    /// <returns>TNode.</returns>
    protected override XElement GetEmptyNode(Expression node)
        => new(Namespaces.Exs + Transform.Identifier(node.NodeType.ToString(), IdentifierConventions.Camel),
                node switch {
                    // omit the type for constant expressions - their element says it all
                    ConstantExpression => null,
                    // do not omit the void return type for these nodes
                    LambdaExpression n => new(AttributeNames.Type, Transform.TypeName(n.ReturnType)),
                    MethodCallExpression n => new(AttributeNames.Type, Transform.TypeName(n.Method.ReturnType)),
                    InvocationExpression n => new(AttributeNames.Type, Transform.TypeName(n.Expression.Type)),
                    // for the rest: add attribute type if it is not void
                    _ => AttributeType(node),
                });

    /// <inheritdoc/>
    protected override Expression VisitConstant(ConstantExpression node)
    {
        try
        {
            return GenericVisit(
                     node,
                     base.VisitConstant,
                     (n, x) =>
                     {
                         _options.AddComment(x, n);
                         x.Add(
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
            (n, x) => x.Add(_options.TypeComment(n.Type)));

    IEnumerable<XElement> VisitParameterDefinitionList(ReadOnlyCollection<ParameterExpression> parameterList)
    {
        foreach (var n in parameterList)
        {
            if (_parameters.TryGetValue(n, out var id))
                throw new InternalTransformErrorException($"Parameter with a name {n.Name} is already defined.");

            id = _parameters[n] = $"P{++_lastParamIdNumber}";

            yield return new XElement(
                                ElementNames.ParameterDefinition,
                                AttributeType(n),
                                new XAttribute(AttributeNames.Name, n.Name ?? "_"),
                                new XAttribute(AttributeNames.Id, id));
        }
    }

    /// <inheritdoc/>
    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        // here we do not want the base.Visit to drive the immediate subexpressions - it visits them in an inconvenient order.
        var parameters = new XElement(ElementNames.Parameters, VisitParameterDefinitionList(node.Parameters));

        Visit(node.Body);

        var body = new XElement(ElementNames.Body, _elements.Pop());

        var x = GetEmptyNode(node);

        x.Add(
            node.TailCall ? new XAttribute(AttributeNames.TailCall, node.TailCall) : null,
            string.IsNullOrWhiteSpace(node.Name) ? null : new XAttribute(AttributeNames.Name, node.Name),
            node.ReturnType == node.Body.Type ? null : new XAttribute(AttributeNames.DelegateType, node.ReturnType),
            parameters,
            body);

        _elements.Push(x);

        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitUnary(UnaryExpression node)
        => GenericVisit(
            node,
            base.VisitUnary,
            (n, x) => x.Add(
                        _elements.Pop(),    // pop the operand
                        VisitMethodInfo(n)));

    /// <inheritdoc/>
    protected override Expression VisitBinary(BinaryExpression node)
        => GenericVisit(
            node,
            base.VisitBinary,
            (n, x) => x.Add(
                        n.IsLiftedToNull ? new XAttribute(AttributeNames.IsLiftedToNull, true) : null,
                        PopElements(2),     // pop operands. TODO test they are in the right order: left right
                        VisitMethodInfo(n)));

    /// <inheritdoc/>
    protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        => GenericVisit(
            node,
            base.VisitTypeBinary,
            (n, x) => x.Add(
                        new XAttribute(AttributeNames.TypeOperand, Transform.TypeName(n.TypeOperand)),
                        _elements.Pop()));  // pop the value operand

    /// <inheritdoc/>
    protected override Expression VisitIndex(IndexExpression node)
        => GenericVisit(
            node,
            base.VisitIndex,
            (n, x) =>
            {
                var indexes = new XElement(ElementNames.Indexes, PopElements(n.Arguments.Count));
                x.Add(
                    _elements.Pop(),    // pop the indexes
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
                    new XElement(ElementNames.Arguments, PopElements(node.Arguments.Count))));  // pop the elements init expressions

        return elementInit;
    }

    /// <inheritdoc/>
    protected override Expression VisitMember(MemberExpression node)
        => GenericVisit(
            node,
            base.VisitMember,
            (n, x) =>
                x.Add(
                    _elements.Pop(),    // pop the expression that will give the object whose requested member to access
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
                    _elements.Pop()));  // pop the expression to assign

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
                    new XElement(ElementNames.Bindings, PopElements(node.Bindings.Count))));    // visit the members to chain obj.p1.p2.p3...

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
                    _elements.Pop()));  // pop the list of expressions to assign to elements of a member

        return binding;
    }

    /// <inheritdoc/>
    protected override Expression VisitMemberInit(MemberInitExpression node)
        => GenericVisit(
            node,
            base.VisitMemberInit,
            (n, x) => x.Add(
                        _elements.Pop(),        // the new expression
                        new XElement(
                                ElementNames.Bindings,
                                PopElements(n.Bindings.Count))));   // pop the expressions to assign to members
    #endregion

    /// <inheritdoc/>
    protected override Expression VisitMethodCall(MethodCallExpression node)
        => GenericVisit(
            node,
            base.VisitMethodCall,
            (n, x) =>
            {
                var arguments = new XElement(
                                        ElementNames.Arguments,
                                        PopElements(n.Arguments.Count));   // pop the argument expressions
                var instance = n.Object!=null ? _elements.Pop() : null; // pop the object
                var method = VisitMemberInfo(n.Method);

                x.Add(
                    instance,
                    method,
                    arguments);
            });

    /// <inheritdoc/>
    protected override Expression VisitInvocation(InvocationExpression node)
        => GenericVisit(
            node,
            base.VisitInvocation,
            (n, x) =>
            {
                var arguments = new XElement(
                                        ElementNames.Arguments,
                                        PopElements(n.Arguments.Count));   // pop the argument expressions

                x.Add(
                    _elements.Pop(),    // pop the delegate or lambda
                    arguments);
            });

    /// <inheritdoc/>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        _elements.Push(
            _parameters.TryGetValue(node, out var id)
                ? new XElement(
                        ElementNames.ParameterReference,
                        AttributeType(node),
                        new XAttribute(AttributeNames.Name, node.Name ?? "_"),
                        node.IsByRef ? new XAttribute(AttributeNames.IsByRef, node.IsByRef) : null,
                        new XAttribute(AttributeNames.IdRef, id))
                : new XElement(
                        ElementNames.ParameterDefinition,
                        AttributeType(node),
                        new XAttribute(AttributeNames.Name, node.Name ?? "_"),
                        node.IsByRef ? new XAttribute(AttributeNames.IsByRef, node.IsByRef) : null,
                        new XAttribute(AttributeNames.Id, $"P{++_lastParamIdNumber}")));
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitBlock(BlockExpression node)
    {
        // here we do not want the base.Visit to drive the immediate subexpressions - it visits them in an inconvenient order.
        var variables = new XElement(
                                ElementNames.Variables,
                                VisitParameterDefinitionList(node.Variables));

        Visit(node.Expressions);

        _elements.Push(new XElement(
                            ElementNames.Block,
                            variables,
                            PopElements(node.Expressions.Count)));

        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitConditional(ConditionalExpression node)
        => GenericVisit(
            node,
            base.VisitConditional,
            (n, x) => x.Add(PopElements(3)));   // visit the condition, the if-true expression, else expression

    /// <inheritdoc/>
    protected override Expression VisitNew(NewExpression node)
        => GenericVisit(
            node,
            base.VisitNew,
            (n, x) => x.Add(
                        VisitMemberInfo(n.Constructor!),
                        new XElement(ElementNames.Arguments, PopElements(n.Arguments.Count)),   // pop the c-tor argfuments
                        n.Members is not null
                            ? new XElement(ElementNames.Members, n.Members.Select(m => VisitMemberInfo(m)))
                            : null));

    /////////////////////////////////////////////////////////////////
    // IN PROCESS:
    /////////////////////////////////////////////////////////////////

    /// <inheritdoc/>
    protected override Expression VisitLoop(LoopExpression node)
        => GenericVisit(
            node,
            base.VisitLoop,
            (n, x) => x.Add(
                        _elements.Pop(),    // pop the loop-ed expression
                        n.BreakLabel is not null ? new XElement(ElementNames.BreakLabel, _elements.Pop()) : null,           // pop the break target label
                        n.ContinueLabel is not null ? new XElement(ElementNames.ContinueLabel, _elements.Pop()) : null));   // pop the continue target label

    /// <inheritdoc/>
    protected override Expression VisitLabel(LabelExpression node)
        => GenericVisit(
            node,
            base.VisitLabel,
            (n, x) =>
            {
                var value = n.DefaultValue is not null ? _elements.Pop() : null;    // pop the default result if present
                var targetElement = _elements.Pop();

                // VisitLabelTarget adds an attribute with name idref - remove it and put attribute with name id instead
                var targetElementIdRef = targetElement.Attributes(AttributeNames.IdRef).First();

                targetElementIdRef.Remove();
                targetElement.Add(new XAttribute(AttributeNames.Id, targetElementIdRef.Value));

                x.Add(
                    targetElement,
                    value);
            });

    /// <inheritdoc/>
    protected override LabelTarget? VisitLabelTarget(LabelTarget? n)
    {
        var targetNode = base.VisitLabelTarget(n);

        if (targetNode is null)
            return null;

        if (!_labelTargets.TryGetValue(targetNode, out var id))
            id = _labelTargets[targetNode] = $"P{++_lastLabelIdNumber}";

        _elements.Push(
            new XElement(
                    ElementNames.LabelTarget,
                    AttributeType(targetNode.Type),
                    !string.IsNullOrWhiteSpace(targetNode.Name)
                        ? new XAttribute(AttributeNames.Name, targetNode.Name)
                        : null,
                    new XAttribute(AttributeNames.IdRef, id)));

        return targetNode;
    }

    /// <inheritdoc/>
    protected override Expression VisitGoto(GotoExpression node)
        => GenericVisit(
            node,
            base.VisitGoto,
            (n, x) =>
            {
                var expression = n.Value != null ? _elements.Pop() : null;

                x.Add(
                    new XAttribute(AttributeNames.Kind, Transform.Identifier(n.Kind.ToString(), IdentifierConventions.Camel)),
                    _elements.Pop(),    // pop the targetLabel XElement
                    expression);
            });

    /////////////////////////////////////////////////////////////////
    // TODO:
    /////////////////////////////////////////////////////////////////

    /// <inheritdoc/>
    protected override Expression VisitListInit(ListInitExpression node)
        => GenericVisit(
            node,
            base.VisitListInit,
            (n, x) => { });

    /// <inheritdoc/>
    protected override Expression VisitNewArray(NewArrayExpression node)
        => GenericVisit(
            node,
            base.VisitNewArray,
            (n, x) => { });

    /// <inheritdoc/>
    protected override Expression VisitSwitch(SwitchExpression node)
        => GenericVisit(
            node,
            base.VisitSwitch,
            (n, x) => { });

    /// <inheritdoc/>
    protected override Expression VisitTry(TryExpression node)
        => GenericVisit(
            node,
            base.VisitTry,
            (n, x) => { });

    /// <inheritdoc/>
    protected override CatchBlock VisitCatchBlock(CatchBlock node) => base.VisitCatchBlock(node);

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
            (n, x) => throw new NotImplementedExpressionException(n.GetType().Name));

    /// <inheritdoc/>
    protected override Expression VisitDynamic(DynamicExpression node)
        => GenericVisit(
            node,
            base.VisitDynamic,
            (n, x) => throw new NotImplementedExpressionException(n.GetType().Name));

    /// <inheritdoc/>
    protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        => GenericVisit(
            node,
            base.VisitRuntimeVariables,
            (n, x) => throw new NotImplementedExpressionException(n.GetType().Name));

    /// <inheritdoc/>
    protected override Expression VisitExtension(Expression node)
        => GenericVisit(
            node,
            base.VisitExtension,
            (n, x) => throw new NotImplementedExpressionException($"{nameof(ExpressionVisitor)}.{VisitExtension}(Expression n)"));
}
