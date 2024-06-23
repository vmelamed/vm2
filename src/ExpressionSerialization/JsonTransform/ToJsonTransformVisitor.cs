namespace vm2.ExpressionSerialization.JsonTransform;
/// <summary>
/// Class ToJsonTransformVisitor.
/// Implements the <see cref="ExpressionTransformVisitor{XNode}" />
/// </summary>
/// <seealso cref="ExpressionTransformVisitor{XNode}" />
public partial class ToJsonTransformVisitor(JsonOptions options) : ExpressionTransformVisitor<JElement>
{
    /// <summary>
    /// Gets a properly named node corresponding to the current expression node.
    /// </summary>
    /// <param name="node">The expression node for which to create an empty document node.</param>
    /// <returns>TDocument.</returns>
    protected override JElement GetEmptyNode(Expression node)
        => new(
            Transform.Identifier(node.NodeType.ToString(), IdentifierConventions.Camel),
                node switch {
                    // omit the type for expressions where their element says it all
                    ConstantExpression => null,
                    ListInitExpression => null,
                    NewExpression => null,
                    NewArrayExpression => null,
                    LabelExpression => null,
                    // do not omit the void return type for these nodes:
                    LambdaExpression n => new(Vocabulary.Type, Transform.TypeName(n.ReturnType)),
                    MethodCallExpression n => new(Vocabulary.Type, Transform.TypeName(n.Method.ReturnType)),
                    InvocationExpression n => new(Vocabulary.Type, Transform.TypeName(n.Expression.Type)),
                    // for the rest: add attribute type if it is not void:
                    _ => PropertyType(node),
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
                         // options.AddComment(x, n);
                         x.Add(
                            options.TypeComment(n.Type),
                            _dataTransform.TransformNode(n));
                     });
        }
        catch (Exception x)
        {
            throw new SerializationException($"Don't know how to serialize {node.Type}", x);
        }
    }

    IEnumerable<JsonNode?> VisitParameterDefinitionList(ReadOnlyCollection<ParameterExpression> parameterList)
        => parameterList.Select(p => !IsDefined(p)
                                        ? GetParameter(p).Value
                                        : throw new InternalTransformErrorException($"Parameter with a name `{p.Name}` is already defined."));

    /// <inheritdoc/>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        using var _ = OutputDebugScope(node.NodeType.ToString());
        var n = (ParameterExpression)base.VisitParameter(node);

        _elements.Push(GetParameter(n));
        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitLambda<T>(Expression<T> node)
        => GenericVisit(
            node,
            base.VisitLambda,
            (n, x) => x.Add(
                        PropertyDelegateType(node.Type),
                        new JElement(Vocabulary.Parameters, PopElementsValues(n.Parameters.Count)),
                        new JElement(Vocabulary.Body, PopElement()),
                        PropertyName(node.Name),
                        node.TailCall ? new JElement(Vocabulary.TailCall, node.TailCall) : null));

    /// <inheritdoc/>
    protected override Expression VisitUnary(UnaryExpression node)
        => GenericVisit(
            node,
            base.VisitUnary,
            (n, x) => x.Add(
                        new JElement(Vocabulary.Operands, new JsonArray(PopWrappedElement())),    // pop the operand
                        VisitMethodInfo(n),
                        n.IsLifted ? new JElement(Vocabulary.IsLifted, true) : null,
                        n.IsLiftedToNull ? new JElement(Vocabulary.IsLiftedToNull, true) : null));

    /// <inheritdoc/>
    protected override Expression VisitBinary(BinaryExpression node)
        => GenericVisit(
            node,
            base.VisitBinary,
            (n, x) =>
            {
                var right = PopWrappedElement();
                var convert = n.Conversion is not null ? PopElementValue() : null;
                var left = PopWrappedElement();

                x.Add(
                        new JElement(
                            Vocabulary.Operands, left, right),
                        VisitMethodInfo(n),
                        n.IsLifted ? new JElement(Vocabulary.IsLifted, true) : null,
                        n.IsLiftedToNull ? new JElement(Vocabulary.IsLiftedToNull, true) : null,
                        convert is not null ? new JElement(Vocabulary.ConvertLambda, convert) : null);
            });

    /// <inheritdoc/>
    protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        => GenericVisit(
            node,
            base.VisitTypeBinary,
            (n, x) => x.Add(
                        new JElement(Vocabulary.TypeOperand, Transform.TypeName(n.TypeOperand)),
                        new JElement(Vocabulary.Operands, new JsonArray(PopWrappedElement()))));

    /// <inheritdoc/>
    protected override Expression VisitIndex(IndexExpression node)
        => GenericVisit(
            node,
            base.VisitIndex,
            (n, x) =>
            {
                var indexes = new JElement(
                                    Vocabulary.Indexes,
                                        PopWrappedElements(n.Arguments.Count));   // pop the index expressions

                x.Add(
                    new JElement(Vocabulary.Instance, PopElement()),    // pop the object being indexed
                    indexes,
                    VisitMemberInfo(n.Indexer));
            });

    /// <inheritdoc/>
    protected override Expression VisitBlock(BlockExpression node)
        => GenericVisit(
            node,
            base.VisitBlock,
            (n, x) => x.Add(
                        node.Variables.Count is > 0 ? new JElement(Vocabulary.Variables, PopElementsValues(node.Variables.Count)) : null,
                        new JElement(Vocabulary.Expressions, PopWrappedElements(node.Expressions.Count))));

    /// <inheritdoc/>
    protected override Expression VisitMethodCall(MethodCallExpression node)
        => GenericVisit(
            node,
            base.VisitMethodCall,
            (n, x) =>
            {
                var arguments = new JElement(Vocabulary.Arguments, PopWrappedElements(n.Arguments.Count));        // pop the argument expressions

                x.Add(
                    n.Object != null ? new JElement(Vocabulary.Object, PopElement()) : null,
                    VisitMemberInfo(n.Method),
                    arguments);
            });

    /// <inheritdoc/>
    protected override Expression VisitInvocation(InvocationExpression node)
        => GenericVisit(
            node,
            base.VisitInvocation,
            (n, x) =>
            {
                var arguments = new JElement(Vocabulary.Arguments, PopWrappedElements(n.Arguments.Count));   // pop the argument expressions

                x.Add(
                    new JElement(Vocabulary.Delegate, PopElement()),        // pop the delegate or lambda
                    arguments);
            });

    /// <inheritdoc/>
    protected override Expression VisitConditional(ConditionalExpression node)
        => GenericVisit(
            node,
            base.VisitConditional,
            (n, x) =>
            {
                JElement? @else = n.IfFalse is not null ? new JElement(Vocabulary.If, PopElement()) : null;
                JElement? then = n.IfTrue  is not null ? new JElement(Vocabulary.Then, PopElement()) : null;
                JElement @if = new JElement(Vocabulary.Else, PopElement());
                x.Add(@if, then, @else);
            });


    /// <inheritdoc/>
    protected override LabelTarget? VisitLabelTarget(LabelTarget? node)
    {
        using var _ = OutputDebugScope(nameof(LabelTarget));
        var n = base.VisitLabelTarget(node);

        if (n is not null)
            _elements.Push(GetLabelTarget(n));

        return n;
    }

    /// <inheritdoc/>
    protected override Expression VisitLabel(LabelExpression node)
        => GenericVisit(
            node,
            base.VisitLabel,
            (n, x) =>
            {
                JElement? value = n.DefaultValue is not null ? PopElement() : null;   // pop the default result value if present

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
                JElement? value = n.Value is not null ? PopElement() : null;

                x.Add(
                    PopElement(),
                    value is not null ? new JElement(Vocabulary.Value, value) : null,
                    new JElement(Vocabulary.Kind, Transform.Identifier(node.Kind.ToString(), IdentifierConventions.Camel)));
            });

    /// <inheritdoc/>
    protected override Expression VisitNew(NewExpression node)
        => GenericVisit(
            node,
            base.VisitNew,
            (n, x) => x.Add(
                        VisitMemberInfo(n.Constructor),
                        new JElement(Vocabulary.Arguments, PopWrappedElements(n.Arguments.Count)),   // pop the c-tor arguments
                        n.Members is not null && n.Members.Count > 0
                            ? new JElement(Vocabulary.Members, new JsonObject(n.Members.Select(m => (KeyValuePair<string, JsonNode?>)VisitMemberInfo(m)!)))
                            : null));

    /// <inheritdoc/>
    protected override Expression VisitNewArray(NewArrayExpression node)
        => GenericVisit(
            node,
            base.VisitNewArray,
            (n, x) => x.Add(
                        PropertyType(n.Type.GetElementType()),
                        new JElement(
                                n.NodeType is ExpressionType.NewArrayInit ? Vocabulary.ArrayElements : Vocabulary.Bounds,
                                PopWrappedElements(n.Expressions.Count))));

    /// <inheritdoc/>
    protected override Expression VisitListInit(ListInitExpression node)
        => GenericVisit(
            node,
            base.VisitListInit,
            (n, x) =>
            {
                var initializers = PopWrappedElements(n.Initializers.Count);

                x.Add(
                    PopElement(),            // the new n
                    new JElement(Vocabulary.Initializers, initializers));                // the elementsInit n
            });

    /// <inheritdoc/>
    protected override ElementInit VisitElementInit(ElementInit node)
    {
        using var _ = OutputDebugScope(nameof(ElementInit));
        var elementInit = base.VisitElementInit(node);

        _elements.Push(
            new JElement(
                    Vocabulary.ElementInit,
                        VisitMemberInfo(node.AddMethod),
                        new JElement(
                                Vocabulary.Arguments,
                                    PopWrappedElements(node.Arguments.Count))));  // pop the elements init expressions

        return elementInit;
    }
}
