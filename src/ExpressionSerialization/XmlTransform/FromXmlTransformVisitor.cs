namespace vm2.ExpressionSerialization.XmlTransform;

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
        => Expression.Default(e.GetTypeFromAttribute());

    /// <summary>
    /// Visits an XML element representing a parameter expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <param name="expectedName">The expected local name of the element, e.g. 'variable' or `parameterDefinition`.</param>
    /// <returns>The <see cref="ParameterExpression" /> represented by the element.</returns>
    /// <exception cref="SerializationException">$</exception>
    protected virtual ParameterExpression VisitParameter(XElement e, string? expectedName = null)
    {
        if (expectedName is not null && expectedName != e.Name.LocalName)
            throw new SerializationException($"Expected element with name `{expectedName}` but got `{e.Name.LocalName}`.");

        var id = e.Attribute(AttributeNames.Id)?.Value
                    ?? e.Attribute(AttributeNames.IdRef)?.Value
                        ?? throw new SerializationException($"Could not get the Id or the IdRef of a parameter or variable in {e.Name}");

        if (_parameters.TryGetValue(id, out var expression))
            return expression;

        return _parameters[id] = Expression.Parameter(
                                                e.GetType(),
                                                e.TryGetName(out var name) ? name : null);
    }

    /// <summary>
    /// Visits an XML element representing a list of parameter definition expressions.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="IEnumerable{ParameterExpression}"/> represented by the element.</returns>
    protected virtual IEnumerable<ParameterExpression> VisitParameterDefinitionList(XElement e)
        => e.Elements()
            .Select(pe => VisitParameter(pe, Transform.NParameterDefinition));

    /// <summary>
    /// Visits an XML element representing a lambda expression, e.g. `a => a.Abc + 42`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="LambdaExpression"/> represented by the element.</returns>
    protected virtual LambdaExpression VisitLambda(XElement e)
        => Expression.Lambda(
                            VisitChild(e, Transform.NBody),
                            XmlConvert.ToBoolean(e.Attribute(AttributeNames.TailCall)?.Value ?? "false"),
                            VisitParameterDefinitionList(e.GetChild(Transform.NParameters)));

    /// <summary>
    /// Visits an XML element representing a unary expression, e.g. `-a`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual UnaryExpression VisitUnary(XElement e)
        => Expression.MakeUnary(
                            e.ExpressionType(),
                            VisitChild(e),
                            e.GetTypeFromAttribute(),
                            e.TryGetChild(Transform.NMethod, out var method) ? VisitMemberInfo(method) as MethodInfo : null);

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
                            e.TryGetChild(Transform.NMethod, out var method) ? VisitMemberInfo(method) as MethodInfo : null,
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
                            e.GetTypeFromAttribute(AttributeNames.TypeOperand)),

            "typeEqual" => Expression.TypeEqual(
                            VisitChild(e),
                            e.GetTypeFromAttribute(AttributeNames.TypeOperand)),

            _ => throw new SerializationException($"Don't know how to transform {e.Name} to a `TypeBinaryExpression`."),
        };

    /// <summary>
    /// Visits an XML element representing an index expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual MethodCallExpression VisitIndex(XElement e)
        => Expression.ArrayIndex(
                            VisitChild(e),
                            VisitIndexes(e));

    /// <summary>
    /// Visits the indexes element of an index operation.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>System.Collections.Generic.IEnumerable&lt;System.Linq.Expressions.Expression&gt;.</returns>
    protected virtual IEnumerable<Expression> VisitIndexes(XElement e)
        => e.GetChild(Transform.NIndexes)
                            .Elements()
                            .Select(Visit);

    /// <summary>
    /// Visits an XML element representing a block expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual BlockExpression VisitBlock(XElement e)
        => Expression.Block(
                            e.GetChild(Transform.NVariables)
                             .Elements()
                             .Select(v => VisitParameter(v, Transform.NVariableDefinition)),
                            e.Elements()
                             .Where(e => e.Name.LocalName != Transform.NVariables)
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
            return Expression.New(e.GetTypeFromAttribute());

        return Expression.New(
                            VisitMemberInfo(ciElement) as ConstructorInfo
                                    ?? throw new SerializationException($"Could not deserialize ConstructorInfo from `{e.Name}`"),
                            e.Element(ElementNames.Arguments)?
                                    .Elements()
                                    .Select(Visit),
                            e.Element(ElementNames.Members)?
                                    .Elements()
                                    .Select(me => VisitMemberInfo(me)
                                                        ?? throw new SerializationException($"Could not deserialize MemberInfo from `{e.Name}`")));
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
                            VisitMemberInfo(e.GetChild(Transform.NMemberAccess))
                                    ?? throw new SerializationException($"Could not deserialize `MemberInfo` in `{e.Name}`"));

    /// <summary>
    /// Visits an XML element representing a `XXXX` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual MethodCallExpression VisitMethodCall(XElement e)
    {
        var child = e.GetChild(0);

        return child.Name.LocalName == Transform.NCall
                    ? Expression.Call(
                            VisitMemberInfo(child) as MethodInfo
                                ?? throw new SerializationException($"Could not deserialize `MethodInfo` from `{e.Name}`"),
                            e.GetChild(1).Elements().Select(Visit))
                    : Expression.Call(
                            Visit(child),
                            VisitMemberInfo(e.GetChild(1)) as MethodInfo
                                ?? throw new SerializationException($"Could not deserialize `MethodInfo` from `{e.Name}`"),
                            e.GetChild(2).Elements().Select(Visit));
    }

    /// <summary>
    /// Visits an XML element representing a `delegate` or lambda invocation expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual InvocationExpression VisitInvocation(XElement e)
        => Expression.Invoke(
                            VisitChild(e, 0),
                            e.Elements(ElementNames.Arguments).Select(Visit));

    /// <summary>
    /// Visits an XML element representing a `XXXX` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression" /> represented by the element.</returns>
    /// <exception cref="SerializationException">$"Expected element with name `{expectedName}` but got `{e.Name.LocalName}`.</exception>
    protected virtual LabelExpression VisitLabel(XElement e)
        => Expression.Label(
                            VisitLabelTarget(e.GetChild(0), false),
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
    /// <exception cref="SerializationException">$"Expected XML attribute `{(isRef.Value ? Transform.NIdRef : Transform.NId)}` in the element `{e.Name}`.</exception>
    protected virtual LabelTarget VisitLabelTarget(XElement e, bool isRef)
    {
        var id = (isRef
                    ? e.Attribute(AttributeNames.IdRef)?.Value
                    : e.Attribute(AttributeNames.Id)?.Value)
                        ?? throw new SerializationException($"Could not get the Id or the IdRef of a label target in `{e.Name}`");

        if (_labelTargets.TryGetValue(id, out var target))
            return target;

        e.TryGetName(out var name);
        e.TryGetType(out var type);

        return _labelTargets[id] = type is not null
                                        ? Expression.Label(type, name)
                                        : Expression.Label(name);
    }

    /// <summary>
    /// Visits an XML element representing a `goto` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual GotoExpression VisitGoto(XElement e)
    {
        var target = VisitLabelTarget(e.GetChild(Transform.NTarget), true);

        return Expression.MakeGoto(
                            Enum.Parse<GotoExpressionKind>(
                                    e.Attribute(AttributeNames.Kind)?.Value
                                                        ?? throw new SerializationException($"Could not get the kind of the goto expression from `{e.Name}`."),
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
                            e.TryGetChild(Transform.NBreakLabel, out var breakLabel) && breakLabel is not null
                                        ? VisitLabel(breakLabel).Target
                                        : null,
                            e.TryGetChild(Transform.NContinueLabel, out var continueLabel) && continueLabel is not null
                                        ? VisitLabel(continueLabel).Target
                                        : null);

    /// <summary>
    /// Visits an XML element representing a `switch` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual SwitchExpression VisitSwitch(XElement e)
        => Expression.Switch(
                            VisitChild(e, 0),
                            e.TryGetChild(Transform.NDefaultCase, out var elem) && elem is not null ? Visit(elem) : null,
                            e.TryGetChild(Transform.NMethod, out var comp) ? VisitMemberInfo(comp) as MethodInfo : null,
                            e.Elements(ElementNames.Case).Select(VisitSwitchCase));

    /// <summary>
    /// Visits an XML element representing a `switch case` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="SwitchExpression"/> represented by the element.</returns>
    protected virtual SwitchCase VisitSwitchCase(XElement e)
        => Expression.SwitchCase(
                            e.Elements().Where(e => e.Name.LocalName is not Transform.NCaseValues).Select(Visit).Single(),
                            e.Element(ElementNames.CaseValues)?.Elements().Select(Visit)
                                ?? throw new SerializationException($"Could not get a switch case's test values in `{e.Name}`"));

    /// <summary>
    /// Visits an XML element representing a `try...catch(x)...catch...finally` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual TryExpression VisitTry(XElement e)
        => Expression.MakeTry(
                            null,
                            VisitChild(e, 0),
                            e.TryGetChild(Transform.NFinally, out var final) && final is not null ? Visit(final) : null,
                            e.TryGetChild(Transform.NFault, out var fault) && fault is not null ? Visit(fault) : null,
                            e.Elements(Transform.NCatch).Select(VisitCatchBlock));

    /// <summary>
    /// Visits an XML element representing a `catch(x) where filter {}` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="CatchBlock"/> represented by the element.</returns>
    protected virtual CatchBlock VisitCatchBlock(XElement e)
        => Expression.MakeCatchBlock(
                            e.TryGetTypeFromAttribute(out var type) ? type! : typeof(void),
                            e.TryGetChild(Transform.NException, out var ex) && ex is not null ? VisitParameter(ex) : null,
                            e.Elements().Where(e => e.Name.LocalName is not Transform.NException and not Transform.NFilter).Select(Visit).Single(),
                            e.TryGetChild(Transform.NFilter, out var f) && f is not null ? Visit(f) : null);

    /// <summary>
    /// Visits an XML element representing a member init expression, e.g. the part `Name = "abc"` or `List = new() { 1, 2, 3 }` from the
    /// member initialization `new Obj() { Name = "abc", List = new() { 1, 2, 3 }, };`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual MemberInitExpression VisitMemberInit(XElement e)
        => Expression.MemberInit(
                            VisitNew(e.GetChild(0)),
                            e.GetChild(Transform.NBindings).Elements().Select(VisitBinding));

    #region MemberBindings
    /// <summary>
    /// Dispatches the binding visitation to the right handler.
    /// </summary>
    /// <param name="e">The binding element.</param>
    /// <returns>System.Linq.Expressions.MemberBinding.</returns>
    protected virtual MemberBinding VisitBinding(XElement e)
        => e.GetName() switch {
            Transform.NAssignmentBinding => Expression.Bind(
                                                    VisitMemberInfo(
                                                        e.TryGetChild(Transform.NProperty, out var memberInfo) || e.TryGetChild(Transform.NField, out memberInfo)
                                                                        ? memberInfo : null)
                                                            ?? throw new SerializationException($"Could not deserialize member info from `{e.Name}`"),
                                                    VisitChild(e, 1)),
            Transform.NMemberMemberBinding => Expression.MemberBind(
                                                    VisitMemberInfo(
                                                        e.TryGetChild(Transform.NProperty, out var memberInfo) || e.TryGetChild(Transform.NField, out memberInfo)
                                                                        ? memberInfo : null)
                                                            ?? throw new SerializationException($"Could not deserialize member info from `{e.Name}`"),
                                                    e.GetChild(1).Elements().Select(VisitBinding)),
            Transform.NMemberListBinding => Expression.ListBind(
                                                    VisitMemberInfo(
                                                        e.TryGetChild(Transform.NProperty, out var memberInfo) || e.TryGetChild(Transform.NField, out memberInfo)
                                                                        ? memberInfo : null)
                                                            ?? throw new SerializationException($"Could not deserialize member info from `{e.Name}`"),
                                                    e.GetChild(1).Elements().Select(VisitElementInit)),
            _ => throw new SerializationException($"Don't know how to deserialize member binding `{e.GetName()}`"),
        };

    #endregion

    /// <summary>
    /// Visits an XML element that represents a collection element initialization.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>System.Linq.Expressions.ElementInit.</returns>
    protected virtual ElementInit VisitElementInit(XElement e)
        => Expression.ElementInit(
                    VisitMemberInfo(e.GetChild(0)) as MethodInfo
                        ?? throw new SerializationException($"Could not deserialize member info from `{e.Name}`"),
                    e.Elements(Transform.NArguments).Select(Visit));

    /// <summary>
    /// Visits a new list with initializers, e.g. `new() { 1, a++, b+c }`.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>System.Linq.Expressions.ListInitExpression.</returns>
    protected virtual ListInitExpression VisitListInit(XElement e)
        => Expression.ListInit(
                VisitNew(e.GetChild(0)),
                VisitMemberInfo(e.TryGetChild(Transform.NMethod, out var mi) ? mi : null) as MethodInfo,
                e.Elements(ElementNames.ListInit).Select(Visit));

    /// <summary>
    /// Visits an XML element representing a `XXXX` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual NewArrayExpression VisitNewArrayInit(XElement e)
        => Expression.NewArrayInit(
                e.GetType(),
                e.GetChild(Transform.NArrayElements).Elements().Select(Visit));

    /// <summary>
    /// Visits an XML element representing a `XXXX` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual NewArrayExpression VisitNewArrayBounds(XElement e)
        => Expression.NewArrayBounds(
                e.GetType(),
                e.GetChild(Transform.NBounds).Elements().Select(Visit));
    #endregion
}
