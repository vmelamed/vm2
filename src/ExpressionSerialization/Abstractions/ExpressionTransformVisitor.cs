﻿namespace vm2.ExpressionSerialization.Abstractions;

using vm2.ExpressionSerialization.XmlTransform;

/// <summary>
/// Class ExpressionSerializingVisitor.
/// Implements <see cref="System.Linq.Expressions.ExpressionVisitor" /> that recursively transforms the visited expression nodes into document 
/// elements.
/// </summary>
/// <typeparam name="TElement">The type of the document nodes that represent expression nodes, 
/// e.g. <see cref="XElement"/> or <see cref="JObject"/>.</typeparam>
/// <seealso cref="System.Linq.Expressions.ExpressionVisitor" />
public abstract class ExpressionTransformVisitor<TElement>(Options? options = null) : System.Linq.Expressions.ExpressionVisitor
{
    /// <summary>
    /// The transform options.
    /// </summary>
    protected Options _options = options ?? new();

    /// <summary>
    /// The intermediate results (XElements) are pushed here to be popped out and placed later as operands (sub-elements) into a parent element, 
    /// representing an expression node's operation.
    /// E.g. the sequence of operations while serializing "a+b+c" may look like this:
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
    /// In the end of a successful visit the stack should contain only one element - the root of the whole expression.
    /// </summary>
    protected readonly Stack<TElement> _elements = new();

    /// <summary>
    /// Gets the top level document node result, e.g. <see cref="XElement"/> or <see cref="JObject"/>.
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
                throw new InternalTransformErrorException("There must be exactly one element in the queue.");
            if (_elements.Count < 1)
                throw new NoAvailableResultException();
            return _elements.Pop();
        }
    }

#if DEBUG
    /// <summary>
    /// Dispatches the expression to one of the more specialized visit methods in this class.
    /// </summary>
    /// <param name="node">The expression to visit.</param>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
    public override System.Linq.Expressions.Expression? Visit(System.Linq.Expressions.Expression? node)
    {
        if (node is null)
            return null;

        Debug.WriteLine("==== " + node.NodeType);
        Debug.Indent();

        var x = base.Visit(node);

        Debug.Unindent();
        return x;
    }
#endif

    /// <summary>
    /// Invokes the base class's visit method on the expression node (which may reduce it), creates the representing XML
    /// element and invokes the XML serializing delegate.
    /// </summary>
    /// <typeparam name="TExpression">The type of the visited expression node.</typeparam>
    /// <param name="node">The expression node to be serialized.</param>
    /// <param name="baseVisit">
    /// Delegates to the base class's visiting method that will reduce the node to a simpler node (if possible).
    /// </param>
    /// <param name="thisVisit">Delegate to the XML serializing method.</param>
    /// <returns>The possibly reduced expression.</returns>
    protected virtual System.Linq.Expressions.Expression GenericVisit<TExpression>(
        TExpression node,
        Func<TExpression, System.Linq.Expressions.Expression> baseVisit,
        Action<TExpression, TElement> thisVisit) where TExpression : System.Linq.Expressions.Expression
    {
        var reducedNode = baseVisit(node);

        if (reducedNode is not TExpression n)
            return reducedNode;

        var element = GetEmptyNode(reducedNode.NodeType);

        thisVisit(n, element);
        _elements.Push(element);

        return node;
    }

    /// <summary>
    /// Gets a properly named document node corresponding to the current expression node.
    /// </summary>
    /// <param name="nodeType">Type of the node.</param>
    /// <returns>TNode.</returns>
    protected abstract TElement GetEmptyNode(ExpressionType nodeType);
}