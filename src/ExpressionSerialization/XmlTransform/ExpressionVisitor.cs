﻿namespace vm2.ExpressionSerialization.XmlTransform;

using vm2.ExpressionSerialization.Conventions;

/// <summary>
/// Class ExpressionVisitor.
/// Implements the <see cref="ExpressionTransformVisitor{XNode}" />
/// </summary>
/// <seealso cref="ExpressionTransformVisitor{XNode}" />
public partial class ExpressionVisitor(Options? options = null) : ExpressionTransformVisitor<XContainer>
{
    /// <summary>
    /// The transform options.
    /// </summary>
    protected Options _options = options ?? new();

    /// <summary>
    /// Gets a properly named node corresponding to the current expression node.
    /// </summary>
    /// <param name="nodeType">Type of the node.</param>
    /// <returns>TNode.</returns>
    protected override XContainer GetEmptyNode(ExpressionType nodeType)
        => new XElement(XmlNamespace.Exs + Transform.Identifier(nodeType.ToString(), IdentifierConventions.Camel));

    DataTransform _dataTransform = new(options);

    /// <summary>
    /// Visits the <see cref="ConstantExpression" />.
    /// </summary>
    /// <param name="node">The expression to visit.</param>
    /// <returns>The modified expression, if it or any subexpression was modified; otherwise, returns the original expression.</returns>
    protected override Expression VisitConstant(ConstantExpression node)
        => GenericVisit(
                node,
                base.VisitConstant,
                (node, element) => _dataTransform.Get(node.Type)(node, element));
}
