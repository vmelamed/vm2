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

    /// <inheritdoc/>
    protected override Expression VisitDefault(DefaultExpression node)
        => GenericVisit(
            node,
            base.VisitDefault,
            (n, x) => x.Add(options.TypeComment(n.Type)));

    IEnumerable<JsonNode?> VisitParameterDefinitionList(ReadOnlyCollection<ParameterExpression> parameterList)
        => parameterList.Select(p => !IsDefined(p)
                                        ? GetParameter(p).Value
                                        : throw new InternalTransformErrorException($"Parameter with a name {p.Name} is already defined."));

    /// <inheritdoc/>
    protected override Expression VisitLambda<T>(Expression<T> node)
    {
        using var _ = OutputDebugScope(node.NodeType.ToString());
        // here we do not want the base.Visit to drive the immediate subexpressions - it visits them in the reversed order.

        var paramList = VisitParameterDefinitionList(node.Parameters).ToList();

        Visit(node.Body);

        _elements.Push(
            GetEmptyNode(node)
                .Add(
                    new JElement(Vocabulary.DelegateType, Transform.TypeName(node.Type)),
                    new JElement(Vocabulary.Parameters, paramList),
                    new JElement(Vocabulary.Body, PopElement()),
                    PropertyName(node.Name),
                    node.TailCall ? new JElement(Vocabulary.TailCall, node.TailCall) : null)
        );

        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        var n = (ParameterExpression)base.VisitParameter(node);

        using var _ = OutputDebugScope(n.NodeType.ToString());

        _elements.Push(GetParameter(n));

        return node;
    }

    /// <inheritdoc/>
    protected override Expression VisitUnary(UnaryExpression node)
        => GenericVisit(
            node,
            base.VisitUnary,
            (n, x) => x.Add(
                            new JElement(
                                    Vocabulary.Operands,
                                        new JsonArray(PopWrappedValue())),    // pop the operand
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
                var right = PopWrappedValue();
                var convert = n.Conversion is not null ? PopElementValue() : null;
                var left = PopWrappedValue();

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
                        new JElement(Vocabulary.Expression, PopElement()),
                        new JElement(Vocabulary.TypeOperand, Transform.TypeName(n.TypeOperand))));

    /// <inheritdoc/>
    protected override Expression VisitIndex(IndexExpression node)
        => GenericVisit(
            node,
            base.VisitIndex,
            (n, x) =>
            {
                var indexes = new JElement(
                                    Vocabulary.Indexes,
                                        PopWrappedValues(n.Arguments.Count));   // pop the index expressions

                x.Add(
                    PopElement(),    // pop the object being indexed
                    indexes,
                    VisitMemberInfo(n.Indexer));
            });

    /// <inheritdoc/>
    protected override Expression VisitBlock(BlockExpression node)
        => GenericVisit(
            node,
            base.VisitBlock,
            (n, x) => x.Add(
                        node.Variables?.Count is > 0
                                            ? new JElement(
                                                    Vocabulary.Variables,
                                                        PopElementsValues(node.Variables.Count))
                                            : null,
                        new JElement(
                                Vocabulary.Expressions,
                                    PopElementsValues(node.Expressions.Count))
                    ));
}
