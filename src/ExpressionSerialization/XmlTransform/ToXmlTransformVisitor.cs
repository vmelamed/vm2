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

    ToXmlDataTransform _dataTransform = new(options);

    // labels and parameters/variables are created in one value n and references to them are used in another.
    // These dictionaries keep their id-s so we can create `XAttribute` id-s and idRef-s to them.
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
    /// Gets a properly named n corresponding to the current value n.
    /// </summary>
    /// <param name="node">The currently visited value n from the value tree.</param>
    /// <returns>TDocument.</returns>
    protected override XElement GetEmptyNode(Expression node)
        => new(Namespaces.Exs + Transform.Identifier(node.NodeType.ToString(), IdentifierConventions.Camel),
                node switch {
                    // omit the type for expressions where their element says it all
                    ConstantExpression => null,
                    ListInitExpression => null,
                    NewExpression => null,
                    NewArrayExpression => null,
                    LabelExpression => null,
                    // do not omit the void return type for these nodes:
                    LambdaExpression n => new(AttributeNames.Type, Transform.TypeName(n.ReturnType)),
                    MethodCallExpression n => new(AttributeNames.Type, Transform.TypeName(n.Method.ReturnType)),
                    InvocationExpression n => new(AttributeNames.Type, Transform.TypeName(n.Expression.Type)),
                    // for the rest: add attribute type if it is not void:
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
            throw new SerializationException($"Don't know how to serialize {node.Type}", x);
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
        // here we do not want the base.Visit to drive the immediate subexpressions - it visits them in the opposite order.

        var parameters = new XElement(
                                ElementNames.Parameters,
                                VisitParameterDefinitionList(node.Parameters, ElementNames.ParameterDefinition));

        Visit(node.Body);

        var body = new XElement(
                        ElementNames.Body,
                        PopElement());

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
                        PopElement(),    // pop the operand
                        VisitMethodInfo(n)));

    /// <inheritdoc/>
    protected override Expression VisitBinary(BinaryExpression node)
        => GenericVisit(
            node,
            base.VisitBinary,
            (n, x) =>
            {
                x.Add(
                        n.IsLiftedToNull ? new XAttribute(AttributeNames.IsLiftedToNull, true) : null,
                        PopElements(2),     // pop operands. TODO test they are in the right order: left, right, (lambda)
                        VisitMethodInfo(n));

                if (n.Conversion is not null)
                {
                    var convElement = PopElement(); // TODO: debug

                    convElement.Name = Transform.NConvertLambda;
                    x.Add(convElement);
                }
            });

    /// <inheritdoc/>
    protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        => GenericVisit(
            node,
            base.VisitTypeBinary,
            (n, x) => x.Add(
                        new XAttribute(AttributeNames.TypeOperand, Transform.TypeName(n.TypeOperand)),
                        PopElement()));  // pop the value operand

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
                    PopElement(),    // pop the indexes
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
                    PopElement(),    // pop the expression/value that will give the object whose requested member is being accessed
                    VisitMemberInfo(n.Member)));

    /// <inheritdoc/>
    #region Member initialization
    protected override Expression VisitMemberInit(MemberInitExpression node)
        => GenericVisit(
            node,
            base.VisitMemberInit,
            (n, x) =>
            {
                var bindings = PopElements(n.Bindings.Count).ToList();     // pop the expressions to assign to members
                x.Add(
                    PopElement(),        // the new value
                    new XElement(
                            ElementNames.Bindings,
                            bindings));
            });

    #region Member Bindings
    /// <inheritdoc/>
    protected override MemberAssignment VisitMemberAssignment(MemberAssignment node)
    {
        using var _ = OutputDebugScope(nameof(MemberAssignment));
        var binding = base.VisitMemberAssignment(node);

        _elements.Push(
            new XElement(
                    ElementNames.AssignmentBinding,
                    VisitMemberInfo(node.Member),
                    PopElement()));  // pop the value to assign

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
                    PopElements(node.Bindings.Count)));

        return binding;
    }

    /// <inheritdoc/>
    protected override MemberListBinding VisitMemberListBinding(MemberListBinding node)
    {
        using var _ = OutputDebugScope(nameof(MemberListBinding));
        var binding = base.VisitMemberListBinding(node);

        _elements.Push(
            new XElement(
                    ElementNames.MemberListBinding,
                    VisitMemberInfo(node.Member),
                    PopElements(node.Initializers.Count)));

        return binding;
    }
    #endregion

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
                var instance = n.Object!=null ? PopElement() : null; // pop the object
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
                    PopElement(),    // pop the delegate or lambda
                    arguments);
            });

    /// <inheritdoc/>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        using var _ = OutputDebugScope(node.NodeType.ToString());
        XElement varElement;

        if (_parameters.TryGetValue(node, out var x))
            varElement = new XElement(
                            x.Name == ElementNames.VariableDefinition ? ElementNames.VariableReference : ElementNames.ParameterReference,
                            AttributeType(node),
                            new XAttribute(AttributeNames.Name, node.Name ?? "_"),
                            node.IsByRef ? new XAttribute(AttributeNames.IsByRef, node.IsByRef) : null,
                            new XAttribute(AttributeNames.IdRef, x.Attribute(AttributeNames.Id)?.Value ?? throw new InternalTransformErrorException("A variable of parameter reference without Id.")));
        else
            _parameters[node] =
            varElement = new XElement(
                            ElementNames.VariableDefinition,
                            AttributeType(node),
                            new XAttribute(AttributeNames.Name, node.Name ?? "_"),
                            node.IsByRef ? new XAttribute(AttributeNames.IsByRef, node.IsByRef) : null,
                            new XAttribute(AttributeNames.Id, $"P{++_lastParamIdNumber}"));

        _elements.Push(varElement);
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
                var op3 = n.IfFalse is not null ? PopElement() : null;
                var op2 = n.IfTrue  is not null ? PopElement() : null;
                var op1 = PopElement();

                Debug.Assert(n.Type != null, "The value n's type is null - remove the default type value of typeof(void) below.");
                x.Add(op1, op2, op3);
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
                                        !string.IsNullOrWhiteSpace(n.Name) ? new XAttribute(AttributeNames.Name, n.Name) : null,
                                        n.Type != typeof(void) ? AttributeType(n.Type) : null,
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
                                ? PopElement()   // pop the default result value if present
                                : null;

                x.Add(
                    PopElement(),   // add the target
                    value);
            });

    /// <inheritdoc/>
    protected override Expression VisitGoto(GotoExpression node)
        => GenericVisit(
            node,
            base.VisitGoto,
            (n, x) =>
            {
                var value = n.Value is not null ? PopElement() : null;

                // VisitLabelTarget adds an attribute with name id - fixup: remove it and put attribute with name idRef instead
                XElement targetElement = new(PopElement());
                var id = targetElement.Attributes(AttributeNames.Id).Single();

                id.Remove();
                targetElement.Add(
                    new XAttribute(AttributeNames.IdRef, id.Value));

                x.Add(
                    targetElement,
                    value,
                    new XAttribute(AttributeNames.Kind, Transform.Identifier(node.Kind.ToString(), IdentifierConventions.Camel)));
            });

    /// <inheritdoc/>
    protected override Expression VisitLoop(LoopExpression node)
        => GenericVisit(
            node,
            base.VisitLoop,
            (n, x) => x.Add(
                        PopElement(),
                        n.ContinueLabel is not null
                            ? new XElement(
                                    ElementNames.ContinueLabel,
                                    PopElement())
                            : null,
                        n.BreakLabel is not null
                            ? new XElement(
                                    ElementNames.BreakLabel,
                                    PopElement())
                            : null));

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
                                                PopElement())
                                        : null;
                var cases = PopElements(n.Cases.Count);             // the cases
                var value = PopElement();                        // the value to switch on

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
        var caseExpression = PopElement();
        Stack<XElement> tempElements = new(node.TestValues.Count);

        for (int i = 0; i < node.TestValues.Count; i++)
            tempElements.Push(PopElement());

        _elements.Push(new XElement(
                                ElementNames.Case,
                                new XElement(
                                    ElementNames.CaseValues,
                                    tempElements),
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
                                        PopElement())
                                : null;
                var @catch = n.Fault!=null
                                ? new XElement(
                                        ElementNames.Fault,
                                        PopElement())
                                : null;
                int countCatches = n.Handlers?.Count ?? 0;
                Stack<XElement> catches = new(countCatches);

                for (var i = 0; i < countCatches; i++)
                    catches.Push(PopElement());

                var @try = PopElement();

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
                                PopElement().Attributes());
        }

        XElement? filter = null;

        if (node.Filter is not null)
        {
            base.Visit(node.Filter);
            filter = new XElement(
                                ElementNames.Filter,
                                PopElement());
        }

        base.Visit(node.Body);

        var body = PopElement();

        _elements.Push(
            new XElement(
                    ElementNames.Catch,
                    new XAttribute(
                            AttributeNames.Type,
                            Transform.TypeName(node.Test)),
                    exception,
                    filter,
                    body));

        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitListInit(ListInitExpression node)
        => GenericVisit(
            node,
            base.VisitListInit,
            (n, x) =>
            {
                Stack<XElement> initializers = [];

                for (var i = 0; i < n.Initializers.Count; i++)
                    initializers.Push(PopElement());

                x.Add(
                    PopElement(),            // the new n
                    new XElement(
                        ElementNames.Initializers,
                        initializers));                // the elementsInit n
            });

    /// <inheritdoc/>
    protected override Expression VisitNewArray(NewArrayExpression node)
        => GenericVisit(
            node,
            base.VisitNewArray,
            (n, x) =>
            {
                var elemType = n.Type.GetElementType() ?? throw new InternalTransformErrorException($"Could not get the type of the element.");

                x.Add(new XAttribute(AttributeNames.Type, Transform.TypeName(elemType)));

                if (n.NodeType == ExpressionType.NewArrayInit)
                    VisitNewArrayInit(n, x);
                else
                    VisitNewArrayBounds(n, x);
            });

    void VisitNewArrayInit(NewArrayExpression n, XElement x)
    {
        Stack<XElement> initializers = [];

        for (var i = 0; i < n.Expressions.Count; i++)
            initializers.Push(PopElement());


        x.Add(
            new XElement(
                    ElementNames.ArrayElements,
                    initializers));
    }

    void VisitNewArrayBounds(NewArrayExpression n, XElement x)
    {
        Stack<XElement> bounds = [];

        for (var i = 0; i < n.Expressions.Count; i++)
            bounds.Push(PopElement());

        x.Add(
            new XElement(
                    ElementNames.Bounds,
                    bounds));
    }

    /////////////////////////////////////////////////////////////////
    // DO NOTHING:
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
