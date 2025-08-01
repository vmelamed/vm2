﻿namespace vm2.Linq.ExpressionSerialization.Shared.Abstractions;

/// <summary>
/// Class ExpressionTransformVisitor.
/// Implements <see cref="ExpressionVisitor" /> that recursively transforms the visited expression nodes into document
/// elements.
/// </summary>
/// <typeparam name="TElement">The type of the document nodes that represent expression nodes,
/// e.g. 'XElement' or 'JsonObject'.</typeparam>
/// <seealso cref="ExpressionVisitor" />
public abstract class ExpressionTransformVisitor<TElement> : ExpressionVisitor
{
    /// <summary>
    /// The intermediate results (XElement-s or JElement-s) are pushed here to be popped out and placed later as operands
    /// (sub-elements) into the parent XML element representing the parent expression.
    /// E.g. the sequence of operations while transforming "a+b+c" may look like this:
    /// <para>
    /// push Element(b)
    /// </para><para>
    /// push Element(a)
    /// </para><para>
    /// push AddElement(pop, pop)
    /// </para><para>
    /// push Element(c)
    /// </para><para>
    /// push AddElement(pop, pop)
    /// </para><para>
    /// As in a reversed polish record.
    /// </para>
    /// In the end of a successful visit the stack should contain only one XML element - the root of the whole expression.
    /// </summary>
    protected readonly Stack<TElement> _elements = new();

    /// <summary>
    /// Gets the top level document node result, e.g. 'XElement' or 'JsonTransform.JElement'.
    /// </summary>
    /// <value>The resultant top level document node.</value>
    /// <remarks>
    /// NOTE: The property  transfers the ownership of the resultant node to the caller, i.e. two consecutive calls to
    /// the property will result in a
    /// </remarks>
    public virtual TElement Result
    {
        get
        {
            if (_elements.Count > 1)
                throw new InternalTransformErrorException($"There must be exactly one element on the stack but there are {_elements.Count}.");
            if (_elements.Count < 1)
                throw new NoAvailableResultException();

            var element = _elements.Pop();
            Reset();

            return element;
        }
    }

    /// <summary>
    /// Pops one element from the stack
    /// <see cref="ExpressionTransformVisitor{TElement}._elements"/>.
    /// </summary>
    /// <returns>XElement.</returns>
    protected TElement Pop() => _elements.Pop();

    /// <summary>
    /// Pops a number of elements in the order they entered the stack
    /// <see cref="ExpressionTransformVisitor{TElement}._elements"/>  (FIFO, not LIFO).
    /// </summary>
    /// <param name="numberOfExpressions">The number of expressions.</param>
    /// <returns>System.Collections.Generic.IEnumerable&lt;System.Xml.Linq.XElement&gt;.</returns>
    protected IEnumerable<TElement> Pop(int numberOfExpressions)
    {
        // we need this intermediary stack to return the elements in FIFO order
        Stack<TElement> tempElements = new(numberOfExpressions);

        // pop the expressions:
        for (var i = 0; i < numberOfExpressions; i++)
            tempElements.Push(_elements.Pop());

        return tempElements;
    }

#if DEBUG
    /// <summary>
    /// Dispatches the expression to one of the more specialized visit methods in this class.
    /// </summary>
    /// <param name="node">The expression to visit.</param>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
    public override Expression? Visit(Expression? node)
    {
        if (node is null)
            return null;

        using var _ = OutputDebugScope(node.NodeType.ToString());

        return base.Visit(node);
    }
#endif

    /// <summary>
    /// Invokes the base class's visit method on the expression node (which may reduce it), creates the representing XML
    /// x and invokes the transforming delegate.
    /// </summary>
    /// <typeparam name="TExpression">The type of the visited expression node.</typeparam>
    /// <param name="node">The expression node to be transformed.</param>
    /// <param name="baseVisit">The base visit.</param>
    /// <param name="thisVisit">Delegate to the transforming method.</param>
    /// <returns>The possibly reduced expression.</returns>
    protected virtual Expression GenericVisit<TExpression>(
        TExpression node,
        Func<TExpression, Expression> baseVisit,
        Action<TExpression, TElement> thisVisit)
        where TExpression : Expression
    {
        var resNode = baseVisit(node)
                        ?? throw new InternalTransformErrorException($"The base visit of a {node.NodeType} node returned different node or null.");

        if (resNode is not TExpression n)
            return resNode;

        var x = GetEmptyNode(n);

        thisVisit(n, x);
        _elements.Push(x);

        return n;
    }

    /// <summary>
    /// Gets a properly named node corresponding to the current expression node.
    /// </summary>
    /// <param name="node">GetType of the node.</param>
    /// <returns>TDocument.</returns>
    protected abstract TElement GetEmptyNode(Expression node);

    /// <summary>
    /// Resets this instance.
    /// </summary>
    protected virtual void Reset() => _elements.Clear();

    #region Not implemented:
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
    #endregion
}
