﻿namespace vm2.ExpressionSerialization.XmlTransform;

using System.Xml.Linq;

/// <summary>
/// Class that visits the nodes of an XML element to produce a LINQ expression tree.
/// </summary>
public partial class FromXmlTransformVisitor
{
    Dictionary<string, ParameterExpression> _parameters = [];
    Dictionary<string, LabelTarget> _labelTargets = [];

    internal void ResetVisitState()
    {
        _parameters.Clear();
        _labelTargets.Clear();
    }

    /// <summary>
    /// Dispatches the visit to the concrete implementation based on the element's name.
    /// </summary>
    /// <param name="element">The element to be visited.</param>
    /// <returns>The created expression.</returns>
    public virtual Expression Visit(XElement element)
        => _transforms.TryGetValue(element.Name.LocalName, out var visit)
                ? visit(this, element)
                : throw new SerializationException($"Don't know how to deserialize the element `{element.Name}`.");

    #region Concrete XML element visitors
    /// <summary>
    /// Visits the child node with index <paramref name="childIndex"/> of the element <paramref name="e"/>.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <param name="childIndex">The index of the child.</param>
    /// <returns>The sub-<see cref="Expression"/> represented by the child element.</returns>
    protected virtual Expression VisitChild(XElement e, int childIndex = 0)
        => Visit(e.GetChild(childIndex));

    /// <summary>
    /// Visits the child node with name <paramref name="childName"/> of the element <paramref name="e"/>.
    /// </summary>
    /// <param name="e">The parent element.</param>
    /// <param name="childName">The local name of the child.</param>
    /// <returns>The sub-<see cref="Expression"/> represented by the child element.</returns>
    protected virtual Expression VisitChild(XElement e, string childName)
        => Visit(e.GetChild(childName));

    /// <summary>
    /// Visits an XML element representing a constant expression, e.g. `42`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="ConstantExpression"/> represented by the element.</returns>
    protected virtual Expression VisitConstant(XElement e)
        => FromXmlDataTransform.ConstantTransform(e.GetChild(0));

    /// <summary>
    /// Visits an XML element representing a default expression (e.g. <c>default(int)</c>).
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="ParameterExpression"/> represented by the element.</returns>
    protected virtual Expression VisitDefault(XElement e)
        => Expression.Default(e.GetETypeFromAttribute());

    /// <summary>
    /// Visits an XML element representing a parameter expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <param name="expectedName">The expected local name of the element, e.g. 'variable' or `parameterDefinition`.</param>
    /// <returns>The <see cref="ParameterExpression" /> represented by the element.</returns>
    /// <exception cref="SerializationException">$</exception>
    protected virtual ParameterExpression VisitParameter(XElement e, string? expectedName = null)
    {
        if (expectedName is not null && e.Name.LocalName != expectedName)
            throw new SerializationException($"Expected element with name `{expectedName}` but got `{e.Name.LocalName}`.");

        var id = e.Attribute(AttributeNames.Id)?.Value
                    ?? e.Attribute(AttributeNames.IdRef)?.Value ?? throw new SerializationException($"Could not get the Id or the IdRef of a parameter or variable in {e.Name}");

        if (_parameters.TryGetValue(id, out var expression))
            return expression;

        return _parameters[id] = Expression.Parameter(
                                                e.GetEType(),
                                                e.TryGetName(out var name) ? name : null);
    }

    /// <summary>
    /// Visits an XML element representing a list of parameter definition expressions.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="IEnumerable{ParameterExpression}"/> represented by the element.</returns>
    protected virtual IEnumerable<ParameterExpression> VisitParameterDefinitionList(XElement e)
        => e.Elements()
            .Select(pe => VisitParameter(pe, Vocabulary.ParameterDefinition));

    /// <summary>
    /// Visits an XML element representing a lambda expression, e.g. `a => a.Abc + 42`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="LambdaExpression"/> represented by the element.</returns>
    protected virtual LambdaExpression VisitLambda(XElement e)
        => Expression.Lambda(
                            VisitChild(e.GetChild(Vocabulary.Body)),
                            XmlConvert.ToBoolean(e.Attribute(AttributeNames.TailCall)?.Value ?? "false"),
                            VisitParameterDefinitionList(e.GetChild(Vocabulary.Parameters)));

    /// <summary>
    /// Visits an XML element representing a unary expression, e.g. `-a`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual UnaryExpression VisitUnary(XElement e)
        => Expression.MakeUnary(
                            e.ExpressionType(),
                            VisitChild(e),
                            e.GetETypeFromAttribute(),
                            e.TryGetChild(Vocabulary.Method, out var method) ? VisitMemberInfo(method) as MethodInfo : null);

    /// <summary>
    /// Visits an XML element representing a binary expression, e.g. `a + b`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual BinaryExpression VisitBinary(XElement e)
        => Expression.MakeBinary(
                            e.ExpressionType(),
                            VisitChild(e, 0),
                            VisitChild(e, 1),
                            XmlConvert.ToBoolean(e.Attribute(AttributeNames.IsLiftedToNull)?.Value ?? "false"),
                            e.TryGetChild(Vocabulary.Method, out var method) ? VisitMemberInfo(method) as MethodInfo : null,
                            e.Element(ElementNames.Convert) is XElement convert ? Visit(convert) as LambdaExpression : null);

    /// <summary>
    /// Visits an XML element representing a type binary expression, e.g. `x is Type`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual TypeBinaryExpression VisitTypeBinary(XElement e)
        => e.Name.LocalName switch {
            "typeIs" => Expression.TypeIs(
                            VisitChild(e),
                            e.GetETypeFromAttribute(AttributeNames.TypeOperand)),

            "typeEqual" => Expression.TypeEqual(
                            VisitChild(e),
                            e.GetETypeFromAttribute(AttributeNames.TypeOperand)),

            _ => throw new SerializationException($"Don't know how to transform {e.Name} to a `TypeBinaryExpression`."),
        };

    /// <summary>
    /// Visits an XML element representing an index expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitIndex(XElement e)
        => Expression.ArrayAccess(
                VisitChild(e),
                VisitIndexes(e));

    /// <summary>
    /// Visits the indexes element of an index operation.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>System.Collections.Generic.IEnumerable&lt;System.Linq.Expressions.Expression&gt;.</returns>
    protected virtual IEnumerable<Expression> VisitIndexes(XElement e)
        => e.GetChild(Vocabulary.Indexes)
                            .Elements()
                            .Select(Visit);

    /// <summary>
    /// Visits an XML element representing a block expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual BlockExpression VisitBlock(XElement e)
        => Expression.Block(
                            (e.TryGetChild(Vocabulary.Variables, out var vars) ? vars : null)?
                             .Elements()?
                             .Select(v => VisitParameter(v, Vocabulary.VariableDefinition)),
                            e.Elements()
                             .Where(e => e.Name.LocalName != Vocabulary.Variables)
                             .Select(Visit));

    /// <summary>
    /// Visits an XML element representing a conditional expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual ConditionalExpression VisitConditional(XElement e)
        => e.Elements().Count() == 2
                ? Expression.IfThen(
                                VisitChild(e, 0),
                                VisitChild(e, 1))
                : e.TryGetEType(out var type) && type != typeof(void)
                        ? Expression.Condition(
                                    VisitChild(e, 0),
                                    VisitChild(e, 1),
                                    VisitChild(e, 2),
                                    type!)
                        : Expression.IfThenElse(
                                    VisitChild(e, 0),
                                    VisitChild(e, 1),
                                    VisitChild(e, 2));

    /// <summary>
    /// Visits an XML element representing a `new` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual NewExpression VisitNew(XElement e)
    {
        var ciElement = e.Element(ElementNames.Constructor);

        if (ciElement is null)
            return Expression.New(e.GetETypeFromAttribute());

        var ci = VisitMemberInfo(ciElement) as ConstructorInfo ?? throw new SerializationException($"Could not deserialize ConstructorInfo from `{e.Name}`");
        var args = e.Element(ElementNames.Arguments)?
                    .Elements()
                    .Select(Visit);
        var mems = e.Element(ElementNames.Members)?
                    .Elements()
                    .Select(me => VisitMemberInfo(me) ?? throw new SerializationException($"Could not deserialize MemberInfo from `{e.Name}`"));

        return mems is null ? Expression.New(ci, args) : Expression.New(ci, args, mems);
    }

    /// <summary>
    /// Visits an XML element representing a `throw` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual UnaryExpression VisitThrow(XElement e)
        => Expression.Throw(VisitChild(e, 0));

    /// <summary>
    /// Visits an XML element representing a `Member` access expression, e.g. `a.Abc`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitMember(XElement e)
        => Expression.MakeMemberAccess(
                            VisitChild(e, 0),
                            VisitMemberInfo(
                                e.TryGetChild(Vocabulary.Property, out var mem) ||
                                e.TryGetChild(Vocabulary.Field, out mem) ? mem : throw new SerializationException($"Could not deserialize `property` or `field` in `{e.Name}`"))
                                        ?? throw new SerializationException($"Could not deserialize `MemberInfo` in `{e.Name}`"));

    /// <summary>
    /// Visits an XML element representing a `XXXX` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual MethodCallExpression VisitMethodCall(XElement e)
    {
        var child = e.GetChild(0);

        return child.Name.LocalName == Vocabulary.Method
                    ? Expression.Call(
                            VisitMemberInfo(child) as MethodInfo ?? throw new SerializationException($"Could not deserialize `MethodInfo` from `{e.Name}`"),
                            e.GetChild(Vocabulary.Arguments).Elements().Select(Visit))
                    : Expression.Call(
                            Visit(child),
                            VisitMemberInfo(e.GetChild(Vocabulary.Method)) as MethodInfo ?? throw new SerializationException($"Could not deserialize `MethodInfo` from `{e.Name}`"),
                            e.GetChild(Vocabulary.Arguments).Elements().Select(Visit));
    }

    /// <summary>
    /// Visits an XML element representing a `delegate` or lambda invocation expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual InvocationExpression VisitInvocation(XElement e)
        => Expression.Invoke(
                            VisitChild(e, 0),
                            e.Elements(ElementNames.Arguments).Elements().Select(Visit));

    /// <summary>
    /// Visits an XML element representing a `XXXX` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression" /> represented by the element.</returns>
    /// <exception cref="SerializationException">$"Expected element with name `{expectedName}` but got `{e.Key.LocalName}`.</exception>
    protected virtual LabelExpression VisitLabel(XElement e)
        => Expression.Label(
                            VisitLabelTarget(e.GetChild(Vocabulary.Target), false),
                            e.TryGetChild(1, out var value) && value != null ? Visit(value) : null);

    /// <summary>
    /// Visits an XML element representing a `LabelTarget` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <param name="isRef">
    /// Set to <c>true</c> if this is a label reference as in a `goto` statement, 
    /// or <c>false</c> if this is a label definition as in a label.
    /// </param>
    /// <returns>System.Linq.Expressions.LabelTarget.</returns>
    /// <exception cref="SerializationException">$"Expected XML attribute `{(isRef.Value ? Vocabulary.IdRef : Vocabulary.Id)}` in the element `{e.Key}`.</exception>
    protected virtual LabelTarget VisitLabelTarget(XElement e, bool isRef)
    {
        var id = (isRef
                    ? e.Attribute(AttributeNames.IdRef)?.Value
                    : e.Attribute(AttributeNames.Id)?.Value) ?? throw new SerializationException($"Could not get the Id or the IdRef of a label target in `{e.Name}`");

        if (_labelTargets.TryGetValue(id, out var target))
            return target;

        e.TryGetName(out var name);
        e.TryGetEType(out var type);

        return _labelTargets[id] = type is not null ? Expression.Label(type, name) : Expression.Label(name);
    }

    /// <summary>
    /// Visits an XML element representing a `goto` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual GotoExpression VisitGoto(XElement e)
    {
        var target = VisitLabelTarget(e.GetChild(Vocabulary.Target), true);

        return Expression.MakeGoto(
                            Enum.Parse<GotoExpressionKind>(
                                    e.Attribute(AttributeNames.Kind)?.Value ?? throw new SerializationException($"Could not get the kind of the goto expression from `{e.Name}`."),
                                    true),
                            target,
                            e.TryGetChild(1, out var ve) && ve is not null ? Visit(ve) : null,
                            target.Type);
    }

    /// <summary>
    /// Visits an XML element representing a `loop` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitLoop(XElement e)
        => Expression.Loop(
                            VisitChild(e, 0),
                            e.TryGetChild(Vocabulary.BreakLabel, out var breakLabel) && breakLabel is not null
                                        ? VisitLabel(breakLabel).Target
                                        : null,
                            e.TryGetChild(Vocabulary.ContinueLabel, out var continueLabel) && continueLabel is not null
                                        ? VisitLabel(continueLabel).Target
                                        : null);

    /// <summary>
    /// Visits an XML element representing a `switch` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual SwitchExpression VisitSwitch(XElement e)
        => Expression.Switch(
                            e.TryGetETypeFromAttribute(out var type) ? type : null,
                            VisitChild(e, 0),
                            e.TryGetChild(Vocabulary.DefaultCase, out var elem) && elem is not null ? Visit(elem.GetChild(0)) : null,
                            e.TryGetChild(Vocabulary.Method, out var comp) ? VisitMemberInfo(comp) as MethodInfo : null,
                            e.Elements(ElementNames.Case).Select(VisitSwitchCase));

    /// <summary>
    /// Visits an XML element representing a `switch case` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="SwitchExpression"/> represented by the element.</returns>
    protected virtual SwitchCase VisitSwitchCase(XElement e)
        => Expression.SwitchCase(
                            e.Elements().Where(e => e.Name.LocalName is not Vocabulary.CaseValues).Select(Visit).Single(),
                            e.Element(ElementNames.CaseValues)?.Elements().Select(Visit) ?? throw new SerializationException($"Could not get a switch case's test values in `{e.Name}`"));

    /// <summary>
    /// Visits an XML element representing a `try...catch(x)...catch...finally` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual TryExpression VisitTry(XElement e)
        => Expression.MakeTry(
                            e.TryGetETypeFromAttribute(out var type) ? type : null,
                            VisitChild(e, 0),
                            e.TryGetChild(Vocabulary.Finally, out var final) && final is not null ? Visit(final.GetChild(0)) : null,
                            e.TryGetChild(Vocabulary.Fault, out var catchAll) && catchAll is not null ? Visit(catchAll.GetChild(0)) : null,
                            e.Elements(ElementNames.Catch).Select(VisitCatchBlock));

    /// <summary>
    /// Visits an XML element representing a `catch(x) where filter {}` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="CatchBlock"/> represented by the element.</returns>
    protected virtual CatchBlock VisitCatchBlock(XElement e)
        => Expression.MakeCatchBlock(
                            e.GetETypeFromAttribute(),
                            e.TryGetChild(Vocabulary.Exception, out var exc) && exc is not null ? VisitParameter(exc) : null,
                            e.Elements()
                             .Where(e => e.Name.LocalName is not Vocabulary.Exception
                                                         and not Vocabulary.Filter)
                             .Select(Visit)
                             .Single(),
                            e.TryGetChild(Vocabulary.Filter, out var f) && f is not null ? Visit(f.GetChild(0)) : null);

    /// <summary>
    /// Visits an XML element representing a member init expression, e.g. the part `Key = "abc"` or `List = new() { 1, 2, 3 }` from the
    /// member initialization `new Obj() { Key = "abc", List = new() { 1, 2, 3 }, };`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual MemberInitExpression VisitMemberInit(XElement e)
        => Expression.MemberInit(
                            VisitNew(e.GetChild(0)),
                            e.GetChild(Vocabulary.Bindings).Elements().Select(VisitBinding));

    #region MemberBindings
    /// <summary>
    /// Dispatches the binding visitation to the right handler.
    /// </summary>
    /// <param name="e">The binding element.</param>
    /// <returns>System.Linq.Expressions.MemberBinding.</returns>
    protected virtual MemberBinding VisitBinding(XElement e)
    {
        if (!e.TryGetChild(Vocabulary.Property, out var mi) &&
            !e.TryGetChild(Vocabulary.Field, out mi))
            throw new SerializationException($"Could not deserialize member info from `{e.Name}`");

        return e.Name.LocalName switch {
            Vocabulary.AssignmentBinding => Expression.Bind(
                                                    VisitMemberInfo(mi) ?? throw new SerializationException($"Could not deserialize member info from `{e.Name}`"),
                                                    VisitChild(e, 1)),
            Vocabulary.MemberMemberBinding => Expression.MemberBind(
                                                    VisitMemberInfo(mi) ?? throw new SerializationException($"Could not deserialize member info from `{e.Name}`"),
                                                    e.Elements()
                                                     .Where(e => new[] { ElementNames.AssignmentBinding, ElementNames.MemberMemberBinding, ElementNames.MemberListBinding }.Contains(e.Name))
                                                     .Select(VisitBinding)),
            Vocabulary.MemberListBinding => Expression.ListBind(
                                                    VisitMemberInfo(mi) ?? throw new SerializationException($"Could not deserialize member info from `{e.Name}`"),
                                                    e.Elements(ElementNames.ElementInit)
                                                     .Select(VisitElementInit)),
            _ => throw new SerializationException($"Don't know how to deserialize member binding `{e.GetName()}`"),
        };
    }
    #endregion

    /// <summary>
    /// Visits an XML element that represents a collection element initialization.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>System.Linq.Expressions.ElementInit.</returns>
    protected virtual ElementInit VisitElementInit(XElement e)
        => Expression.ElementInit(
                    VisitMemberInfo(e.GetChild(0)) as MethodInfo ?? throw new SerializationException($"Could not deserialize member info from `{e.Name}`"),
                    e.GetChild(Vocabulary.Arguments)
                     .Elements()
                     .Select(Visit));

    /// <summary>
    /// Visits a new list with initializers, e.g. `new() { 1, a++, b+c }`.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>System.Linq.Expressions.ListInitExpression.</returns>
    protected virtual ListInitExpression VisitListInit(XElement e)
        => Expression.ListInit(
                VisitNew(e.GetChild(Vocabulary.New)),
                e.GetChild(Vocabulary.Initializers)
                 .Elements()
                 .Select(VisitElementInit));

    /// <summary>
    /// Visits an XML element representing a `XXXX` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual NewArrayExpression VisitNewArrayInit(XElement e)
        => Expression.NewArrayInit(
                e.GetEType(),
                e.GetChild(Vocabulary.ArrayElements)
                 .Elements()
                 .Select(Visit));

    /// <summary>
    /// Visits an XML element representing a `XXXX` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual NewArrayExpression VisitNewArrayBounds(XElement e)
        => Expression.NewArrayBounds(
                e.GetEType(),
                e.GetChild(Vocabulary.Bounds)
                 .Elements()
                 .Select(Visit));
    #endregion
}
