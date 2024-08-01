namespace vm2.ExpressionSerialization.JsonTransform;

#if JSON_SCHEMA
using Vocabulary = Conventions.Vocabulary;
#endif

/// <summary>
/// Class that visits the nodes of a JSON node to produce a LINQ expression tree.
/// </summary>
public partial class FromJsonTransformVisitor
{
    /// <summary>
    /// Dispatches the visit to the concrete implementation based on the element's name.
    /// </summary>
    /// <param name="element">The element to be visited.</param>
    /// <returns>The created expression.</returns>
    public virtual Expression Visit(JElement element)
        => _transforms.TryGetValue(element.Name, out var visit)
                ? visit(this, element)
                : throw new SerializationException($"Don't know how to deserialize the element `{element.Name}`.");

    #region Concrete Json element visitors
    /// <summary>
    /// Visits the child node with name <paramref name="childName"/> of the element <paramref name="e"/>, where the underlying object is JsonObject.
    /// </summary>
    /// <param name="e">The parent element.</param>
    /// <param name="childName">The name of the child.</param>
    /// <returns>The sub-<see cref="Expression"/> represented by the child element.</returns>
    protected virtual Expression VisitChild(JElement e, string childName)
        => Visit(e.GetChild(childName));

    /// <summary>
    /// Visits the child node with index <paramref name="childIndex"/> of the element <paramref name="e"/>, where the underlying object is JsonArray.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <param name="childIndex">The index of the child.</param>
    /// <returns>The sub-<see cref="Expression"/> represented by the child element.</returns>
    protected virtual Expression VisitChild(JElement e, int childIndex)
        => Visit(e.GetChild(childIndex));

    /// <summary>
    /// Visits a JSON element representing a constant expression, e.g. `42`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="ConstantExpression"/> represented by the element.</returns>
    protected virtual Expression VisitConstant(JElement e)
        => FromJsonDataTransform.ConstantTransform(e.GetChild(Vocabulary.Constant));

    ///// <summary>
    ///// Visits an JSON element representing a default expression (e.g. <c>default(int)</c>).
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="ParameterExpression"/> represented by the element.</returns>
    //protected virtual Expression VisitDefault(JElement e)
    //    => Expression.Default(e.GetETypeFromAttribute());

    ///// <summary>
    ///// Visits an Json element representing a parameter expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <param name="expectedName">The expected local name of the element, e.g. 'variable' or `parameter`.</param>
    ///// <returns>The <see cref="ParameterExpression" /> represented by the element.</returns>
    ///// <exception cref="SerializationException">$</exception>
    //protected virtual ParameterExpression VisitParameter(JElement e, string? expectedName = null)
    //{
    //    if (expectedName is not null && e.Name.LocalName != expectedName)
    //        throw new SerializationException($"Expected element with name `{expectedName}` but got `{e.Name.LocalName}`.");

    //    return GetParameter(e);
    //}

    ///// <summary>
    ///// Visits an Json element representing a list of parameter definition expressions.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="IEnumerable{ParameterExpression}"/> represented by the element.</returns>
    //protected virtual IEnumerable<ParameterExpression> VisitParameterList(JElement e)
    //    => e.Elements()
    //        .Select(pe => VisitParameter(pe, Vocabulary.Parameter));

    ///// <summary>
    ///// Visits an Json element representing a lambda expression, e.g. `a => a.Abc + 42`.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="LambdaExpression"/> represented by the element.</returns>
    //protected virtual LambdaExpression VisitLambda(JElement e)
    //    => Expression.Lambda(
    //                        VisitChild(e.GetChild(Vocabulary.Body)),
    //                        JsonConvert.ToBoolean(e.Attribute(AttributeNames.TailCall)?.Value ?? "false"),
    //                        VisitParameterList(e.GetChild(Vocabulary.Parameters)));

    ///// <summary>
    ///// Visits an Json element representing a unary expression, e.g. `-a`.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual UnaryExpression VisitUnary(JElement e)
    //    => Expression.MakeUnary(
    //                        e.ExpressionType(),
    //                        VisitChild(e),
    //                        e.GetETypeFromAttribute(),
    //                        e.TryGetChild(Vocabulary.Method, out var method) ? VisitMemberInfo(method) as MethodInfo : null);

    ///// <summary>
    ///// Visits an Json element representing a binary expression, e.g. `a + b`.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual BinaryExpression VisitBinary(JElement e)
    //    => Expression.MakeBinary(
    //                        e.ExpressionType(),
    //                        VisitChild(e, 0),
    //                        VisitChild(e, 1),
    //                        JsonConvert.ToBoolean(e.Attribute(AttributeNames.IsLiftedToNull)?.Value ?? "false"),
    //                        e.TryGetChild(Vocabulary.Method, out var method) ? VisitMemberInfo(method) as MethodInfo : null,
    //                        e.Element(ElementNames.Convert) is JElement convert ? Visit(convert) as LambdaExpression : null);

    ///// <summary>
    ///// Visits an Json element representing a type binary expression, e.g. `x is Type`.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual TypeBinaryExpression VisitTypeBinary(JElement e)
    //    => e.Name.LocalName switch {
    //        "typeIs" => Expression.TypeIs(
    //                        VisitChild(e),
    //                        e.GetETypeFromAttribute(AttributeNames.TypeOperand)),

    //        "typeEqual" => Expression.TypeEqual(
    //                        VisitChild(e),
    //                        e.GetETypeFromAttribute(AttributeNames.TypeOperand)),

    //        _ => throw new SerializationException($"Don't know how to transform {e.Name} to a `TypeBinaryExpression`."),
    //    };

    ///// <summary>
    ///// Visits an Json element representing an index expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual Expression VisitIndex(JElement e)
    //    => Expression.ArrayAccess(
    //            VisitChild(e),
    //            VisitIndexes(e));

    ///// <summary>
    ///// Visits the indexes element of an index operation.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>System.Collections.Generic.IEnumerable&lt;System.Linq.Expressions.Expression&gt;.</returns>
    //protected virtual IEnumerable<Expression> VisitIndexes(JElement e)
    //    => e.GetChild(Vocabulary.Indexes)
    //                        .Elements()
    //                        .Select(Visit);

    ///// <summary>
    ///// Visits an Json element representing a block expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual BlockExpression VisitBlock(JElement e)
    //    => Expression.Block(
    //                        (e.TryGetChild(Vocabulary.Variables, out var vars) ? vars : null)?
    //                         .Elements()?
    //                         .Select(v => VisitParameter(v, Vocabulary.Parameter)),
    //                        e.Elements()
    //                         .Where(e => e.Name.LocalName != Vocabulary.Variables)
    //                         .Select(Visit));

    ///// <summary>
    ///// Visits an Json element representing a conditional expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual ConditionalExpression VisitConditional(JElement e)
    //    => e.Elements().Count() == 2
    //            ? Expression.IfThen(
    //                            VisitChild(e, 0),
    //                            VisitChild(e, 1))
    //            : e.TryGetEType(out var type) && type != typeof(void)
    //                    ? Expression.Condition(
    //                                VisitChild(e, 0),
    //                                VisitChild(e, 1),
    //                                VisitChild(e, 2),
    //                                type!)
    //                    : Expression.IfThenElse(
    //                                VisitChild(e, 0),
    //                                VisitChild(e, 1),
    //                                VisitChild(e, 2));

    ///// <summary>
    ///// Visits an Json element representing a `new` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual NewExpression VisitNew(JElement e)
    //{
    //    var ciElement = e.Element(ElementNames.Constructor);

    //    if (ciElement is null)
    //        return Expression.New(e.GetETypeFromAttribute());

    //    var ci = VisitMemberInfo(ciElement) as ConstructorInfo ?? throw new SerializationException($"Could not deserialize ConstructorInfo from `{e.Name}`");
    //    var args = e.Element(ElementNames.Arguments)?
    //                .Elements()
    //                .Select(Visit);
    //    var mems = e.Element(ElementNames.Members)?
    //                .Elements()
    //                .Select(me => VisitMemberInfo(me) ?? throw new SerializationException($"Could not deserialize MemberInfo from `{e.Name}`"));

    //    return mems is null ? Expression.New(ci, args) : Expression.New(ci, args, mems);
    //}

    ///// <summary>
    ///// Visits an Json element representing a `throw` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual UnaryExpression VisitThrow(JElement e)
    //    => Expression.Throw(VisitChild(e, 0));

    ///// <summary>
    ///// Visits an Json element representing a `Member` access expression, e.g. `a.Abc`.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual Expression VisitMember(JElement e)
    //    => Expression.MakeMemberAccess(
    //                        VisitChild(e, 0),
    //                        VisitMemberInfo(
    //                            e.TryGetChild(Vocabulary.Property, out var mem) ||
    //                            e.TryGetChild(Vocabulary.Field, out mem) ? mem : throw new SerializationException($"Could not deserialize `property` or `field` in `{e.Name}`"))
    //                                    ?? throw new SerializationException($"Could not deserialize `MemberInfo` in `{e.Name}`"));

    ///// <summary>
    ///// Visits an Json element representing a `XXXX` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual MethodCallExpression VisitMethodCall(JElement e)
    //{
    //    var child = e.GetChild(0);

    //    return child.Name.LocalName == Vocabulary.Method
    //                ? Expression.Call(
    //                        VisitMemberInfo(child) as MethodInfo ?? throw new SerializationException($"Could not deserialize `MethodInfo` from `{e.Name}`"),
    //                        e.GetChild(Vocabulary.Arguments).Elements().Select(Visit))
    //                : Expression.Call(
    //                        Visit(child),
    //                        VisitMemberInfo(e.GetChild(Vocabulary.Method)) as MethodInfo ?? throw new SerializationException($"Could not deserialize `MethodInfo` from `{e.Name}`"),
    //                        e.GetChild(Vocabulary.Arguments).Elements().Select(Visit));
    //}

    ///// <summary>
    ///// Visits an Json element representing a `delegate` or lambda invocation expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual InvocationExpression VisitInvocation(JElement e)
    //    => Expression.Invoke(
    //                        VisitChild(e, 0),
    //                        e.Elements(ElementNames.Arguments).Elements().Select(Visit));

    ///// <summary>
    ///// Visits an Json element representing a `XXXX` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression" /> represented by the element.</returns>
    ///// <exception cref="SerializationException">$"Expected element with name `{expectedName}` but got `{e.Name.LocalName}`.</exception>
    //protected virtual LabelExpression VisitLabel(JElement e)
    //    => Expression.Label(
    //                        VisitLabelTarget(e.GetChild(Vocabulary.LabelTarget)),
    //                        e.TryGetChild(1, out var value) && value != null ? Visit(value) : null);

    ///// <summary>
    ///// Visits an Json element representing a `LabelTarget` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    /////
    ///// <returns>System.Linq.Expressions.LabelTarget.</returns>
    ///// <exception cref="SerializationException">$"Expected Json attribute `{(isRef.Value ? Vocabulary.IdRef : Vocabulary.Id)}` in the element `{e.Name}`.</exception>
    //protected virtual LabelTarget VisitLabelTarget(JElement e)
    //    => GetTarget(e);

    ///// <summary>
    ///// Visits an Json element representing a `goto` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual GotoExpression VisitGoto(JElement e)
    //{
    //    var target = VisitLabelTarget(e.GetChild(Vocabulary.LabelTarget));

    //    return Expression.MakeGoto(
    //                        Enum.Parse<GotoExpressionKind>(
    //                                e.Attribute(AttributeNames.Kind)?.Value ?? throw new SerializationException($"Could not get the kind of the goto expression from `{e.Name}`."),
    //                                true),
    //                        target,
    //                        e.TryGetChild(1, out var ve) && ve is not null ? Visit(ve) : null,
    //                        target.Type);
    //}

    ///// <summary>
    ///// Visits an Json element representing a `loop` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual Expression VisitLoop(JElement e)
    //    => Expression.Loop(
    //                        VisitChild(e, 0),
    //                        e.TryGetChild(Vocabulary.BreakLabel, out var breakLabel) && breakLabel is not null
    //                                    ? VisitLabel(breakLabel).Target
    //                                    : null,
    //                        e.TryGetChild(Vocabulary.ContinueLabel, out var continueLabel) && continueLabel is not null
    //                                    ? VisitLabel(continueLabel).Target
    //                                    : null);

    ///// <summary>
    ///// Visits an Json element representing a `switch` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual SwitchExpression VisitSwitch(JElement e)
    //    => Expression.Switch(
    //                        e.TryGetETypeFromAttribute(out var type) ? type : null,
    //                        VisitChild(e, 0),
    //                        e.TryGetChild(Vocabulary.DefaultCase, out var elem) && elem is not null ? Visit(elem.GetChild(0)) : null,
    //                        e.TryGetChild(Vocabulary.Method, out var comp) ? VisitMemberInfo(comp) as MethodInfo : null,
    //                        e.Elements(ElementNames.Case).Select(VisitSwitchCase));

    ///// <summary>
    ///// Visits an Json element representing a `switch case` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="SwitchExpression"/> represented by the element.</returns>
    //protected virtual SwitchCase VisitSwitchCase(JElement e)
    //    => Expression.SwitchCase(
    //                        e.Elements().Where(e => e.Name.LocalName is not Vocabulary.CaseValues).Select(Visit).Single(),
    //                        e.Element(ElementNames.CaseValues)?.Elements().Select(Visit) ?? throw new SerializationException($"Could not get a switch case's test values in `{e.Name}`"));

    ///// <summary>
    ///// Visits an Json element representing a `try...catch(x)...catch...finally` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual TryExpression VisitTry(JElement e)
    //    => Expression.MakeTry(
    //                        e.TryGetETypeFromAttribute(out var type) ? type : null,
    //                        VisitChild(e, 0),
    //                        e.TryGetChild(Vocabulary.Finally, out var final) && final is not null ? Visit(final.GetChild(0)) : null,
    //                        e.TryGetChild(Vocabulary.Fault, out var catchAll) && catchAll is not null ? Visit(catchAll.GetChild(0)) : null,
    //                        e.Elements(ElementNames.Catch).Select(VisitCatchBlock));

    ///// <summary>
    ///// Visits an Json element representing a `catch(x) where filter {}` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="CatchBlock"/> represented by the element.</returns>
    //protected virtual CatchBlock VisitCatchBlock(JElement e)
    //    => Expression.MakeCatchBlock(
    //                        e.GetETypeFromAttribute(),
    //                        e.TryGetChild(Vocabulary.Exception, out var exc) && exc is not null ? VisitParameter(exc) : null,
    //                        e.Elements()
    //                         .Where(e => e.Name.LocalName is not Vocabulary.Exception
    //                                                     and not Vocabulary.Filter)
    //                         .Select(Visit)
    //                         .Single(),
    //                        e.TryGetChild(Vocabulary.Filter, out var f) && f is not null ? Visit(f.GetChild(0)) : null);

    ///// <summary>
    ///// Visits an Json element representing a member init expression, e.g. the part `Name = "abc"` or `List = new() { 1, 2, 3 }` from the
    ///// member initialization `new Obj() { Name = "abc", List = new() { 1, 2, 3 }, };`.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual MemberInitExpression VisitMemberInit(JElement e)
    //    => Expression.MemberInit(
    //                        VisitNew(e.GetChild(0)),
    //                        e.GetChild(Vocabulary.Bindings).Elements().Select(VisitBinding));

    //#region MemberBindings
    ///// <summary>
    ///// Dispatches the binding visitation to the right handler.
    ///// </summary>
    ///// <param name="e">The binding element.</param>
    ///// <returns>System.Linq.Expressions.MemberBinding.</returns>
    //protected virtual MemberBinding VisitBinding(JElement e)
    //{
    //    if (!e.TryGetChild(Vocabulary.Property, out var mi) &&
    //        !e.TryGetChild(Vocabulary.Field, out mi))
    //        throw new SerializationException($"Could not deserialize member info from `{e.Name}`");

    //    return e.Name.LocalName switch {
    //        Vocabulary.AssignmentBinding => Expression.Bind(
    //                                                VisitMemberInfo(mi) ?? throw new SerializationException($"Could not deserialize member info from `{e.Name}`"),
    //                                                VisitChild(e, 1)),
    //        Vocabulary.MemberMemberBinding => Expression.MemberBind(
    //                                                VisitMemberInfo(mi) ?? throw new SerializationException($"Could not deserialize member info from `{e.Name}`"),
    //                                                e.Elements()
    //                                                 .Where(e => new[] { ElementNames.AssignmentBinding, ElementNames.MemberMemberBinding, ElementNames.MemberListBinding }.Contains(e.Name))
    //                                                 .Select(VisitBinding)),
    //        Vocabulary.MemberListBinding => Expression.ListBind(
    //                                                VisitMemberInfo(mi) ?? throw new SerializationException($"Could not deserialize member info from `{e.Name}`"),
    //                                                e.Elements(ElementNames.ElementInit)
    //                                                 .Select(VisitElementInit)),
    //        _ => throw new SerializationException($"Don't know how to deserialize member binding `{e.GetName()}`"),
    //    };
    //}
    //#endregion

    ///// <summary>
    ///// Visits an Json element that represents a collection element initialization.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>System.Linq.Expressions.ElementInit.</returns>
    //protected virtual ElementInit VisitElementInit(JElement e)
    //    => Expression.ElementInit(
    //                VisitMemberInfo(e.GetChild(0)) as MethodInfo ?? throw new SerializationException($"Could not deserialize member info from `{e.Name}`"),
    //                e.GetChild(Vocabulary.Arguments)
    //                 .Elements()
    //                 .Select(Visit));

    ///// <summary>
    ///// Visits a new list with initializers, e.g. `new() { 1, a++, b+c }`.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>System.Linq.Expressions.ListInitExpression.</returns>
    //protected virtual ListInitExpression VisitListInit(JElement e)
    //    => Expression.ListInit(
    //            VisitNew(e.GetChild(Vocabulary.New)),
    //            e.GetChild(Vocabulary.Initializers)
    //             .Elements()
    //             .Select(VisitElementInit));

    ///// <summary>
    ///// Visits an Json element representing a `XXXX` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual NewArrayExpression VisitNewArrayInit(JElement e)
    //    => Expression.NewArrayInit(
    //            e.GetEType(),
    //            e.GetChild(Vocabulary.ArrayElements)
    //             .Elements()
    //             .Select(Visit));

    ///// <summary>
    ///// Visits an Json element representing a `XXXX` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual NewArrayExpression VisitNewArrayBounds(JElement e)
    //    => Expression.NewArrayBounds(
    //            e.GetEType(),
    //            e.GetChild(Vocabulary.Bounds)
    //             .Elements()
    //             .Select(Visit));
    #endregion
}
