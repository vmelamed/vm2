namespace vm2.ExpressionSerialization.Json;

/// <summary>
/// Class that visits the nodes of a JSON node to produce a LINQ expression tree.
/// </summary>
public partial class FromJsonTransformVisitor
{
    /// <summary>
    /// Dispatches the visit to the concrete implementation based on the element's name.
    /// </summary>
    /// <param name="e">The element to be visited.</param>
    /// <returns>The created expression.</returns>
    public virtual Expression Visit(JElement e)
        => _transforms.TryGetValue(e.Name, out var visit)
                ? visit(this, e)
                : e.ThrowSerializationException<Expression>($"Don't know how to deserialize the element");

    #region Concrete Json element visitors
    /// <summary>
    /// Visits the <paramref name="childIndex"/>-th child of the element.
    /// </summary>
    /// <param name="e">The element whose child must be visited.</param>
    /// <param name="childIndex">Index of the child.</param>
    /// <returns>Expression.</returns>
    /// <exception cref="System.Runtime.Serialization.SerializationException">Could not find object with children</exception>
    public virtual Expression VisitChild(JElement e, int childIndex = 0)
    {
        var jsObj = e.Value?.AsObject();

        if (jsObj is null)
            return e.ThrowSerializationException<Expression>($"Could not find object with children");

        foreach (var child in jsObj.Where(c => c.Value is JsonObject))
            if (childIndex-- == 0)
                return Visit(child);

        return jsObj.ThrowSerializationException<Expression>($"Could not find child 'JsonObject' #{childIndex}");
    }

    /// <summary>
    /// Visits the first a child node with name <paramref name="childName"/>.
    /// </summary>
    /// <param name="e">The JSON element which value's first JsonObject to visit.</param>
    /// <param name="childName">Name of the child.</param>
    /// <returns>Expression.</returns>
    protected virtual Expression VisitChild(JElement e, string childName)
        => Visit(e.GetElement(childName));

    /// <summary>
    /// Visits a Json element representing a constant expression, e.g. `42`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="ConstantExpression"/> represented by the element.</returns>
    protected virtual Expression VisitConstant(JElement e)
        => FromJsonDataTransform.ConstantTransform(e);

    /// <summary>
    /// Visits a Json element representing a default expression (e.g. <c>default(int)</c>).
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="ParameterExpression"/> represented by the element.</returns>
    protected virtual Expression VisitDefault(JElement e)
        => Expression.Default(e.GetTypeFromProperty());

    /// <summary>
    /// Visits an Json element representing a parameter expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <param name="expectedName">The expected name of the element, e.g. 'variable' or `parameter`.</param>
    /// <returns>The <see cref="ParameterExpression" /> represented by the element.</returns>
    /// <exception cref="SerializationException">$</exception>
    protected virtual ParameterExpression VisitParameter(
        JElement e,
        string? expectedName = null)
    {
        if (expectedName is not null
            && e.Name != expectedName)
            e.ThrowSerializationException<ParameterExpression>($"Expected element {(string.IsNullOrWhiteSpace(expectedName) ? "" : $"with name '{expectedName}' but got '{e.Name}'")}");

        return GetParameter(e);
    }

    /// <summary>
    /// Visits a Json element representing a list of parameter definition expressions.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="IEnumerable{ParameterExpression}"/> represented by the element.</returns>
    protected virtual IEnumerable<ParameterExpression> VisitParameterList(JElement e)
        => e.Value?
            .AsArray()?
            .Select((pe, i) => VisitParameter(($"param{i}", pe)))
                ?? e.ThrowSerializationException<IEnumerable<ParameterExpression>>($"Expected array of parameters");

    /// <summary>
    /// Visits a Json element representing a lambda expression, e.g. `a => a.Abc + 42`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="LambdaExpression"/> represented by the element.</returns>
    protected virtual LambdaExpression VisitLambda(JElement e)
        => Expression.Lambda(
                        VisitChild(e.GetElement(Vocabulary.Body)),
                        e.TryGetPropertyValue<bool>(out var tailCall, Vocabulary.TailCall) && tailCall,
                        VisitParameterList((Vocabulary.Parameters, e.GetArray(Vocabulary.Parameters))));

    /// <summary>
    /// Visits a Json element representing a unary expression, e.g. `-a`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual UnaryExpression VisitUnary(JElement e)
        => Expression.MakeUnary(
                        e.GetExpressionType(),
                        VisitChild(e.GetElement(Vocabulary.Operand)),
                        e.GetTypeFromProperty(),
                        GetMemberInfo(e, Vocabulary.Method) as MethodInfo);

    /// <summary>
    /// Visits a Json element representing a binary expression, e.g. `a + b`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual BinaryExpression VisitBinary(JElement e)
    {
        var operands = e.GetArray(Vocabulary.Operands);

        if (operands.Count != 2)
            e.ThrowSerializationException<int>($"Expected exactly two JsonObject operands of the binary expression");

        var operandL = operands[0]?.AsObject() ?? operands.ThrowSerializationException<JsonObject>($"Expected exactly two JsonObject operands of the binary expression");
        var operandR = operands[1]?.AsObject() ?? operands.ThrowSerializationException<JsonObject>($"Expected exactly two JsonObject operands of the binary expression");

        return Expression.MakeBinary(
                            e.GetExpressionType(),
                            Visit(operandL.GetFirstObject()),
                            Visit(operandR.GetFirstObject()),
                            e.TryGetPropertyValue<bool>(out var isLiftedToNull, Vocabulary.IsLiftedToNull) && isLiftedToNull,
                            GetMemberInfo(e, Vocabulary.Method) as MethodInfo,
                            e.TryGetElement(out var convert, Vocabulary.Convert) && convert.HasValue
                                ? Visit(convert.Value) as LambdaExpression
                                : null);
    }

    /// <summary>
    /// Visits a Json element representing a type binary expression, e.g. `x is Type`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual TypeBinaryExpression VisitTypeBinary(JElement e)
    {
        var operands = e.GetArray(Vocabulary.Operands);

        if (operands.Count != 1)
            e.ThrowSerializationException<JElement>($"Expected exactly one JsonObject operand to a binary type expression");

        var operand = operands[0]?.AsObject() ?? operands.ThrowSerializationException<JsonObject>($"Expected exactly one JsonObject operand to a binary type expression");

        return e.Name switch {
            "typeIs" => Expression.TypeIs(
                                Visit(operand.GetFirstObject()),
                                e.GetTypeFromProperty(Vocabulary.TypeOperand)),

            "typeEqual" => Expression.TypeEqual(
                                Visit(operand.GetFirstObject()),
                                e.GetTypeFromProperty(Vocabulary.TypeOperand)),

            _ => e.ThrowSerializationException<TypeBinaryExpression>($"Don't know how to transform {e.Name} to a `TypeBinaryExpression`"),
        };
    }

    /// <summary>
    /// Visits a Json element representing an index expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitIndex(JElement e)
        => Expression.ArrayAccess(
                VisitChild(e),
                VisitIndexes(e));

    /// <summary>
    /// Visits the indexes elements of an indexing operation.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>System.Collections.Generic.IEnumerable&lt;System.Linq.Expressions.Expression&gt;.</returns>
    protected virtual IEnumerable<Expression> VisitIndexes(JElement e)
        => e.GetArray(Vocabulary.Indexes)
            .Select(i => Visit(i.ToObject("Expected index expression").GetFirstObject()));

    /// <summary>
    /// Visits a Json element representing a block expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual BlockExpression VisitBlock(JElement e)
        => e.TryGetTypeFromProperty(out var type) && type is not null
            ? Expression.Block(
                    type,
                    (e.TryGetArray(out var vars, Vocabulary.Variables) ? vars : null)?
                        .Select((v, i) => VisitParameter(new($"var{i}", v))),
                    e.GetArray(Vocabulary.Expressions)
                        .Select(x => Visit(x.ToObject("Expected expression").GetFirstObject())))
            : Expression.Block(
                    (e.TryGetArray(out vars, Vocabulary.Variables) ? vars : null)?
                        .Select((v, i) => VisitParameter(new($"var{i}", v))),
                    e.GetArray(Vocabulary.Expressions)
                        .Select(x => Visit(x.ToObject("Expected expression").GetFirstObject())));

    ///// <summary>
    ///// Visits a Json element representing a conditional expression.
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
    ///// Visits a Json element representing a `new` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual NewExpression VisitNew(JElement e)
    //{
    //    var ciElement = e.Element(ElementNames.Constructor);

    //    if (ciElement is null)
    //        return Expression.New(e.GetTypeFromProperty());

    //    var ci = VisitMemberInfo(ciElement) as ConstructorInfo ?? throw new SerializationException($"Could not deserialize ConstructorInfo from `{e.Name}`");
    //    var args = e.Element(ElementNames.Arguments)?
    //                .Elements()
    //                .Select(Visit);
    //    var mems = e.Element(ElementNames.Members)?
    //                .Elements()
    //                .Select(me => VisitMemberInfo(me) ?? throw new SerializationException($"Could not deserialize MemberInfo from `{e.Name}`"));

    //    return mems is null ? Expression.New(ci, args) : Expression.New(ci, args, mems);
    //}

    /// <summary>
    /// Visits a Json element representing a `throw` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual UnaryExpression VisitThrow(JElement e)
        => e.TryGetTypeFromProperty(out var type) && type is not null
                ? Expression.Throw(VisitChild(e.GetElement(Vocabulary.Operand)), type)
                : Expression.Throw(VisitChild(e.GetElement(Vocabulary.Operand)));

    ///// <summary>
    ///// Visits a Json element representing a `Member` access expression, e.g. `a.Abc`.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual Expression VisitMember(JElement e)
    //    => Expression.MakeMemberAccess(
    //                        VisitChild(e),
    //                        VisitMemberInfo(
    //                            e.TryGetFirstElement(Vocabulary.Property, out var mem) ||
    //                            e.TryGetFirstElement(Vocabulary.Field, out mem) ? mem : throw new SerializationException($"Could not deserialize `property` or `field` in `{e.Name}`"))
    //                                    ?? throw new SerializationException($"Could not deserialize `MemberInfo` in `{e.Name}`"));

    ///// <summary>
    ///// Visits a Json element representing a `XXXX` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual MethodCallExpression VisitMethodCall(JElement e)
    //{
    //    var child = e.GetElement(0);

    //    return child.Name == Vocabulary.Method
    //                ? Expression.Call(
    //                        VisitMemberInfo(child) as MethodInfo ?? throw new SerializationException($"Could not deserialize `MethodInfo` from `{e.Name}`"),
    //                        e.GetElement(Vocabulary.Arguments).Elements().Select(Visit))
    //                : Expression.Call(
    //                        Visit(child),
    //                        VisitMemberInfo(e.GetElement(Vocabulary.Method)) as MethodInfo ?? throw new SerializationException($"Could not deserialize `MethodInfo` from `{e.Name}`"),
    //                        e.GetElement(Vocabulary.Arguments).Elements().Select(Visit));
    //}

    ///// <summary>
    ///// Visits a Json element representing a `delegate` or lambda invocation expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual InvocationExpression VisitInvocation(JElement e)
    //    => Expression.Invoke(
    //                        VisitChild(e, 0),
    //                        e.Elements(ElementNames.Arguments).Elements().Select(Visit));

    ///// <summary>
    ///// Visits a Json element representing a `XXXX` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression" /> represented by the element.</returns>
    ///// <exception cref="SerializationException">$"Expected element with name `{expectedName}` but got `{e.Name}`.</exception>
    //protected virtual LabelExpression VisitLabel(JElement e)
    //    => Expression.Label(
    //                        VisitLabelTarget(e.GetElement(Vocabulary.LabelTarget)),
    //                        e.TryGetFirstElement(1, out var value) && value != null ? Visit(value) : null);

    ///// <summary>
    ///// Visits a Json element representing a `LabelTarget` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    /////
    ///// <returns>System.Linq.Expressions.LabelTarget.</returns>
    ///// <exception cref="SerializationException">$"Expected Json attribute `{(isRef.Value ? Vocabulary.IdRef : Vocabulary.Id)}` in the element `{e.Name}`.</exception>
    //protected virtual LabelTarget VisitLabelTarget(JElement e)
    //    => GetTarget(e);

    ///// <summary>
    ///// Visits a Json element representing a `goto` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual GotoExpression VisitGoto(JElement e)
    //{
    //    var target = VisitLabelTarget(e.GetElement(Vocabulary.LabelTarget));

    //    return Expression.MakeGoto(
    //                        Enum.Parse<GotoExpressionKind>(
    //                                e.Attribute(AttributeNames.Kind)?.Value ?? throw new SerializationException($"Could not get the kind of the goto expression from `{e.Name}`."),
    //                                true),
    //                        target,
    //                        e.TryGetFirstElement(1, out var ve) && ve is not null ? Visit(ve) : null,
    //                        target.Type);
    //}

    ///// <summary>
    ///// Visits a Json element representing a `loop` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual Expression VisitLoop(JElement e)
    //    => Expression.Loop(
    //                        VisitChild(e, 0),
    //                        e.TryGetFirstElement(Vocabulary.BreakLabel, out var breakLabel) && breakLabel is not null
    //                                    ? VisitLabel(breakLabel).Target
    //                                    : null,
    //                        e.TryGetFirstElement(Vocabulary.ContinueLabel, out var continueLabel) && continueLabel is not null
    //                                    ? VisitLabel(continueLabel).Target
    //                                    : null);

    ///// <summary>
    ///// Visits a Json element representing a `switch` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual SwitchExpression VisitSwitch(JElement e)
    //    => Expression.Switch(
    //                        e.TryGetETypeFromAttribute(out var type) ? type : null,
    //                        VisitChild(e, 0),
    //                        e.TryGetFirstElement(Vocabulary.DefaultCase, out var elem) && elem is not null ? Visit(elem.GetElement(0)) : null,
    //                        e.TryGetFirstElement(Vocabulary.Method, out var comp) ? VisitMemberInfo(comp) as MethodInfo : null,
    //                        e.Elements(ElementNames.Case).Select(VisitSwitchCase));

    ///// <summary>
    ///// Visits a Json element representing a `switch case` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="SwitchExpression"/> represented by the element.</returns>
    //protected virtual SwitchCase VisitSwitchCase(JElement e)
    //    => Expression.SwitchCase(
    //                        e.Elements().Where(e => e.Name is not Vocabulary.CaseValues).Select(Visit).Single(),
    //                        e.Element(ElementNames.CaseValues)?.Elements().Select(Visit) ?? throw new SerializationException($"Could not get a switch case's test values in `{e.Name}`"));

    ///// <summary>
    ///// Visits a Json element representing a `try...catch(x)...catch...finally` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual TryExpression VisitTry(JElement e)
    //    => Expression.MakeTry(
    //                        e.TryGetETypeFromAttribute(out var type) ? type : null,
    //                        VisitChild(e, 0),
    //                        e.TryGetFirstElement(Vocabulary.Finally, out var final) && final is not null ? Visit(final.GetElement(0)) : null,
    //                        e.TryGetFirstElement(Vocabulary.Fault, out var catchAll) && catchAll is not null ? Visit(catchAll.GetElement(0)) : null,
    //                        e.Elements(ElementNames.Catch).Select(VisitCatchBlock));

    ///// <summary>
    ///// Visits a Json element representing a `catch(x) where filter {}` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="CatchBlock"/> represented by the element.</returns>
    //protected virtual CatchBlock VisitCatchBlock(JElement e)
    //    => Expression.MakeCatchBlock(
    //                        e.GetTypeFromProperty(),
    //                        e.TryGetFirstElement(Vocabulary.Exception, out var exc) && exc is not null ? VisitParameter(exc) : null,
    //                        e.Elements()
    //                         .Where(e => e.Name is not Vocabulary.Exception
    //                                                     and not Vocabulary.Filter)
    //                         .Select(Visit)
    //                         .Single(),
    //                        e.TryGetFirstElement(Vocabulary.Filter, out var f) && f is not null ? Visit(f.GetElement(0)) : null);

    ///// <summary>
    ///// Visits a Json element representing a member init expression, e.g. the part `Name = "abc"` or `List = new() { 1, 2, 3 }` from the
    ///// member initialization `new Obj() { Name = "abc", List = new() { 1, 2, 3 }, };`.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual MemberInitExpression VisitMemberInit(JElement e)
    //    => Expression.MemberInit(
    //                        VisitNew(e.GetElement(0)),
    //                        e.GetElement(Vocabulary.Bindings).Elements().Select(VisitBinding));

    //#region MemberBindings
    ///// <summary>
    ///// Dispatches the binding visitation to the right handler.
    ///// </summary>
    ///// <param name="e">The binding e.</param>
    ///// <returns>System.Linq.Expressions.MemberBinding.</returns>
    //protected virtual MemberBinding VisitBinding(JElement e)
    //{
    //    if (!e.TryGetFirstElement(Vocabulary.Property, out var mi) &&
    //        !e.TryGetFirstElement(Vocabulary.Field, out mi))
    //        throw new SerializationException($"Could not deserialize member info from `{e.Name}`");

    //    return e.Name switch {
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
    ///// Visits a Json element that represents a collection element initialization.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>System.Linq.Expressions.ElementInit.</returns>
    //protected virtual ElementInit VisitElementInit(JElement e)
    //    => Expression.ElementInit(
    //                VisitMemberInfo(e.GetElement(0)) as MethodInfo ?? throw new SerializationException($"Could not deserialize member info from `{e.Name}`"),
    //                e.GetElement(Vocabulary.Arguments)
    //                 .Elements()
    //                 .Select(Visit));

    ///// <summary>
    ///// Visits a new list with initializers, e.g. `new() { 1, a++, b+c }`.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>System.Linq.Expressions.ListInitExpression.</returns>
    //protected virtual ListInitExpression VisitListInit(JElement e)
    //    => Expression.ListInit(
    //            VisitNew(e.GetElement(Vocabulary.New)),
    //            e.GetElement(Vocabulary.Initializers)
    //             .Elements()
    //             .Select(VisitElementInit));

    ///// <summary>
    ///// Visits a Json element representing a `XXXX` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual NewArrayExpression VisitNewArrayInit(JElement e)
    //    => Expression.NewArrayInit(
    //            e.GetEType(),
    //            e.GetElement(Vocabulary.ArrayElements)
    //             .Elements()
    //             .Select(Visit));

    ///// <summary>
    ///// Visits a Json element representing a `XXXX` expression.
    ///// </summary>
    ///// <param name="e">The element.</param>
    ///// <returns>The <see cref="Expression"/> represented by the element.</returns>
    //protected virtual NewArrayExpression VisitNewArrayBounds(JElement e)
    //    => Expression.NewArrayBounds(
    //            e.GetEType(),
    //            e.GetElement(Vocabulary.Bounds)
    //             .Elements()
    //             .Select(Visit));
    #endregion
}
