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
    Dictionary<LabelTarget, XElement> _labelTargets = [];
    Dictionary<ParameterExpression, XElement> _parameters = [];
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

    IEnumerable<XElement> VisitParameterDefinitionList(
        ReadOnlyCollection<ParameterExpression> parameterList,
        XName definitionElement)
    {
        foreach (var n in parameterList)
        {
            if (_parameters.TryGetValue(n, out var _))
                throw new InternalTransformErrorException($"Parameter with a name {n.Name} is already defined.");

            var id = $"P{++_lastParamIdNumber}";

            yield return _parameters[n] = new XElement(
                                                definitionElement,
                                                AttributeType(n),
                                                new XAttribute(AttributeNames.Name, n.Name ?? "_"),
                                                n.IsByRef ? new XAttribute(AttributeNames.IsByRef, true) : null,
                                                new XAttribute(AttributeNames.Id, id));
        }
    }

    /// <inheritdoc/>
    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        using var _ = OutputDebugScope(node.NodeType.ToString());
        // here we do not want the base.Visit to drive the immediate subexpressions - it visits them in an inconvenient order.
        var parameters = new XElement(
                                ElementNames.Parameters,
                                VisitParameterDefinitionList(node.Parameters, ElementNames.ParameterDefinition));

        Visit(node.Body);

        var body = new XElement(
                        ElementNames.Body,
                        _elements.Pop());

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
                var indexes = new XElement(
                                    ElementNames.Indexes,
                                    PopElements(n.Arguments.Count));
                x.Add(
                    _elements.Pop(),    // pop the indexes
                    indexes);
            });

    /// <inheritdoc/>
    protected override ElementInit VisitElementInit(ElementInit node)
    {
        using var _ = OutputDebugScope(nameof(ElementInit));
        var elementInit = base.VisitElementInit(node);

        _elements.Push(
            new XElement(
                    ElementNames.ElementInit,
                    VisitMemberInfo(node.AddMethod),
                    new XElement(
                            ElementNames.Arguments,
                            PopElements(node.Arguments.Count))));  // pop the elements init expressions

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
    {
        using var _ = OutputDebugScope(nameof(MemberBinding));

        return base.VisitMemberBinding(node);
    }

    /// <inheritdoc/>
    protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
    {
        using var _ = OutputDebugScope(nameof(MemberAssignment));
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
        using var _ = OutputDebugScope(nameof(MemberMemberBinding));
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
        using var _ = OutputDebugScope(nameof(MemberListBinding));
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
        using var _ = OutputDebugScope(node.NodeType.ToString());

        _elements.Push(
            _parameters.TryGetValue(node, out var x)
                ? new XElement(
                        x.Name == ElementNames.VariableDefinition ? ElementNames.VariableReference : ElementNames.ParameterReference,
                        AttributeType(node),
                        new XAttribute(AttributeNames.Name, node.Name ?? "_"),
                        node.IsByRef ? new XAttribute(AttributeNames.IsByRef, node.IsByRef) : null,
                        new XAttribute(AttributeNames.IdRef, x.Attribute(AttributeNames.Id)?.Value ?? throw new InternalTransformErrorException("A variable of parameter reference without Id.")))
                : new XElement(
                        ElementNames.VariableDefinition,
                        AttributeType(node),
                        new XAttribute(AttributeNames.Name, node.Name ?? "_"),
                        node.IsByRef ? new XAttribute(AttributeNames.IsByRef, node.IsByRef) : null,
                        new XAttribute(AttributeNames.Id, $"P{++_lastParamIdNumber}")));
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitBlock(BlockExpression node)
    {
        using var _ = OutputDebugScope(node.NodeType.ToString());
        // here we do not want the base.Visit to drive the immediate subexpressions - it visits them in an inconvenient order.
        var varElements = VisitParameterDefinitionList(node.Variables, ElementNames.VariableDefinition).ToList();
        var variables = varElements.Count > 0
                            ? new XElement(
                                ElementNames.Variables,
                                varElements)
                            : null;

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
            (n, x) =>
            {
                var op3 = n.IfFalse is not null ? _elements.Pop() : null;
                var op2 = n.IfTrue  is not null ? _elements.Pop() : null;
                var op1 = _elements.Pop();

                Debug.Assert(n.Type != null, "The expression node's type is null - remove the default type value of typeof(void) below.");
                x.Add(
                    op1,
                    op2,
                    op3);
            });

    /// <inheritdoc/>
    protected override Expression VisitNew(NewExpression node)
        => GenericVisit(
            node,
            base.VisitNew,
            (n, x) => x.Add(
                        VisitMemberInfo(n.Constructor!),
                        new XElement(
                                ElementNames.Arguments,
                                PopElements(n.Arguments.Count)),   // pop the c-tor arguments
                        n.Members is not null
                            ? new XElement(
                                    ElementNames.Members,
                                    n.Members.Select(m => VisitMemberInfo(m)))
                            : null));

    /// <inheritdoc/>
    protected override LabelTarget? VisitLabelTarget(LabelTarget? node)
    {
        using var _ = OutputDebugScope(nameof(LabelTarget));
        var n = base.VisitLabelTarget(node);

        if (n is null)
            return null;

        if (!_labelTargets.TryGetValue(n, out var targetElement))
            targetElement =
            _labelTargets[n] = new XElement(
                                        ElementNames.Target,
                                        !string.IsNullOrWhiteSpace(n.Name)
                                            ? new XAttribute(AttributeNames.Name, n.Name)
                                            : null,
                                        n.Type != typeof(void)
                                            ? AttributeType(n.Type)
                                            : null,
                                        new XAttribute(AttributeNames.Id, $"L{++_lastLabelIdNumber}"));

        _elements.Push(targetElement);

        return n;
    }

    /// <inheritdoc/>
    protected override Expression VisitLabel(LabelExpression node)
        => GenericVisit(
            node,
            base.VisitLabel,
            (n, x) =>
            {
                var value = n.DefaultValue is not null
                                ? _elements.Pop()   // pop the default result expression if present
                                : null;

                x.Add(
                    _elements.Pop(),
                    value);
            });

    /// <inheritdoc/>
    protected override Expression VisitGoto(GotoExpression node)
        => GenericVisit(
            node,
            base.VisitGoto,
            (n, x) =>
            {
                var expression = n.Value is not null
                                    ? _elements.Pop()   // pop the result expression if present
                                    : null;

                // VisitLabelTarget adds an attribute with name id - fixup: remove it and put attribute with name idref instead
                XElement targetElement = new(_elements.Pop());
                var id = targetElement.Attributes(AttributeNames.Id).First();

                id.Remove();
                targetElement.Add(new XAttribute(AttributeNames.IdRef, id.Value));

                x.Add(
                    targetElement,
                    new XAttribute(AttributeNames.Kind, Transform.Identifier(node.Kind.ToString(), IdentifierConventions.Camel)));
            });

    /// <inheritdoc/>
    protected override Expression VisitLoop(LoopExpression node)
        => GenericVisit(
            node,
            base.VisitLoop,
            (n, x) => x.Add(
                        _elements.Pop(),
                        n.ContinueLabel is not null
                            ? new XElement(
                                    ElementNames.ContinueLabel,
                                    _elements.Pop())
                            : null,
                        n.BreakLabel is not null
                            ? new XElement(
                                    ElementNames.BreakLabel,
                                    _elements.Pop())
                            : null));

    /////////////////////////////////////////////////////////////////
    // IN PROGRESS:
    /////////////////////////////////////////////////////////////////

    /// <inheritdoc/>
    protected override Expression VisitSwitch(SwitchExpression node)
        => GenericVisit(
            node,
            base.VisitSwitch,
            (n, x) =>
            {
                var comparison = n.Comparison != null                // get the non-default comparison method
                                        ? VisitMemberInfo(n.Comparison)
                                        : null;
                var @default = n.DefaultBody != null                 // the body of the default case
                                        ? new XElement(
                                                ElementNames.DefaultCase,
                                                _elements.Pop())
                                        : null;
                var cases = PopElements(n.Cases.Count);             // the cases
                var value = _elements.Pop();                        // the value to switch on

                x.Add(
                    value,
                    comparison,
                    cases,
                    @default);
            });

    /// <inheritdoc/>
    protected override SwitchCase VisitSwitchCase(SwitchCase node)
    {
        using var _ = OutputDebugScope(nameof(SwitchCase));
        var switchCase = base.VisitSwitchCase(node);
        var caseExpression = _elements.Pop();
        Stack<XElement> tempElements = [];

        for (int i = 0; i < node.TestValues.Count; i++)
            tempElements.Push(
                new XElement(
                        ElementNames.CaseValues,
                        _elements.Pop()));

        _elements.Push(new XElement(
                                ElementNames.Case,
                                tempElements,
                                caseExpression));

        return switchCase;
    }

    /// <inheritdoc/>
    protected override Expression VisitTry(TryExpression node)
        => GenericVisit(
            node,
            base.VisitTry,
            (n, x) =>
            {
                var @finally = n.Finally!=null
                                ? new XElement(
                                        ElementNames.Finally,
                                        _elements.Pop())
                                : null;
                var @catch = n.Fault!=null
                                ? new XElement(
                                        ElementNames.Fault,
                                        _elements.Pop())
                                : null;
                Stack<XElement> catches = [];

                for (var i = 0; i < n.Handlers?.Count; i++)
                    catches.Push(_elements.Pop());

                var @try = _elements.Pop();

                x.Add(
                    @try,
                    catches,
                    @catch,
                    @finally);
            });

    /// <inheritdoc/>
    protected override CatchBlock VisitCatchBlock(CatchBlock node)
    {
        using var _ = OutputDebugScope(nameof(CatchBlock));
        // here we do not want the base.Visit to drive the immediate subexpressions - it visits them in an inconvenient order.
        // var catchBlock = base.VisitCatchBlock(node);

        XElement? exception = null;

        if (node.Variable is not null)
        {
            base.Visit(node.Variable);
            exception = new XElement(
                                ElementNames.Exception,
                                _elements.Pop().Attributes());
        }

        XElement? filter = null;

        if (node.Filter is not null)
        {
            base.Visit(node.Filter);
            filter = new XElement(
                                ElementNames.Filter,
                                _elements.Pop());
        }

        base.Visit(node.Body);

        var body = _elements.Pop();

        _elements.Push(
            new XElement(
                    ElementNames.Catch,
                    node.Test != node.Variable?.Type
                        ? new XAttribute(
                            AttributeNames.Type,
                            Transform.TypeName(node.Test))
                        : null,
                    exception,
                    filter,
                    body));

        return node;
    }

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
