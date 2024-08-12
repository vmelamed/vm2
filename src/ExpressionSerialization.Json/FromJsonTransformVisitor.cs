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
    /// Visits a Json element representing a `throw` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual UnaryExpression VisitThrow(JElement e)
        => e.TryGetTypeFromProperty(out var type) && type is not null
                ? Expression.Throw(VisitChild(e.GetElement(Vocabulary.Operand)), type)
                : Expression.Throw(VisitChild(e.GetElement(Vocabulary.Operand)));

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
                        .Select(x => Visit(x.ToObject("Expected expression")
                        .GetFirstObject())))
            : Expression.Block(
                    (e.TryGetArray(out vars, Vocabulary.Variables) ? vars : null)?
                        .Select((v, i) => VisitParameter(new($"var{i}", v))),
                    e.GetArray(Vocabulary.Expressions)
                        .Select(x => Visit(x.ToObject("Expected expression")
                        .GetFirstObject())));

    /// <summary>
    /// Visits a Json element representing a `Member` access expression, e.g. `a.Abc`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitMember(JElement e)
    {
        var memberInfo = VisitMemberInfo(
                            e.GetElement(Vocabulary.Member)
                             .GetFirstElement());

        if (memberInfo is not PropertyInfo and not FieldInfo)
            return e.ThrowSerializationException<Expression>($"Expected '{Vocabulary.Member}/{Vocabulary.Property}' or '{Vocabulary.Member}/{Vocabulary.Field}' element");

        return Expression.MakeMemberAccess(
                e.TryGetElement(out var obj, Vocabulary.Object) && obj.HasValue
                  && obj.Value.Value is JsonObject
                    ? VisitChild(obj.Value)
                    : null,
                memberInfo);
    }

    /// <summary>
    /// Visits a Json element representing a Method call expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual MethodCallExpression VisitMethodCall(JElement e)
        => Expression.Call(
                e.TryGetElement(out var obj, Vocabulary.Object) && obj.HasValue
                    ? VisitChild(obj.Value)
                    : null,
                VisitMemberInfo(e.GetElement(Vocabulary.Method)) as MethodInfo
                    ?? e.ThrowSerializationException<MethodInfo>($"Expected '{Vocabulary.Method}' element"),
                e.GetArray(Vocabulary.Arguments)
                    .Select((n, i) => VisitChild(new($"arg{i}", n.ToObject("Expected argument expression")))));

    /// <summary>
    /// Visits a Json element representing a `delegate` or lambda invocation expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual InvocationExpression VisitInvocation(JElement e)
        => Expression.Invoke(
                VisitChild(
                    e.GetElement(Vocabulary.Delegate)),
                e.GetArray(Vocabulary.Arguments)
                    .Select((n, i) => VisitChild(new($"arg{i}", n.ToObject("Expected argument expression")))));

    /// <summary>
    /// Visits a Json element representing a `XXXX` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression" /> represented by the element.</returns>
    /// <exception cref="SerializationException">$"Expected element with name `{expectedName}` but got `{e.Name}`.</exception>
    protected virtual LabelExpression VisitLabel(JElement e)
        => Expression.Label(
                VisitLabelTarget(e.GetElement(Vocabulary.LabelTarget)),
                e.TryGetElement(out var value, Vocabulary.Default) && value.HasValue
                    ? Visit(value.Value)
                    : null);

    /// <summary>
    /// Visits a Json element representing a `LabelTarget` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    ///
    /// <returns>System.Linq.Expressions.LabelTarget.</returns>
    /// <exception cref="SerializationException">$"Expected Json attribute `{(isRef.Value ? Vocabulary.IdRef : Vocabulary.Id)}` in the element `{e.Name}`.</exception>
    protected virtual LabelTarget VisitLabelTarget(JElement e)
        => GetTarget(e);

    /// <summary>
    /// Visits a Json element representing a `goto` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual GotoExpression VisitGoto(JElement e)
    {
        var target = VisitLabelTarget(e.GetElement(Vocabulary.LabelTarget));

        return Expression.MakeGoto(
                e.GetPropertyValue<GotoExpressionKind>(Vocabulary.Kind),
                target,
                e.TryGetElement(out var ve) && ve.HasValue ? Visit(ve.Value) : null,
                target.Type);
    }

    /// <summary>
    /// Visits a Json element representing a `loop` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitLoop(JElement e)
        => Expression.Loop(
            VisitChild(e.GetElement(Vocabulary.Body)),
            e.TryGetElement(out var breakLabel, Vocabulary.BreakLabel)
              && breakLabel.HasValue
                ? VisitLabel(breakLabel.Value).Target
                : null,
            e.TryGetElement(out var continueLabel, Vocabulary.ContinueLabel)
              && continueLabel.HasValue
                ? VisitLabel(continueLabel.Value).Target
                : null);

    /// <summary>
    /// Visits a Json element representing a `switch` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual SwitchExpression VisitSwitch(JElement e)
        => Expression.Switch(
            e.TryGetTypeFromProperty(out var type)
                ? type
                : null,
            VisitChild(e.GetElement(Vocabulary.Value)),
            e.TryGetElement(out var elem, Vocabulary.DefaultCase)
              && elem.HasValue
                ? Visit(elem.Value.GetFirstElement())
                : null,
            e.TryGetElement(out var comp, Vocabulary.Method)
              && comp.HasValue
                ? VisitMemberInfo(comp.Value.GetFirstElement()) as MethodInfo
                : null,
            e.GetArray(Vocabulary.Cases).Select((c, i) => VisitSwitchCase(new($"cases{i}", c))));

    /// <summary>
    /// Visits a Json element representing a `switch case` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="SwitchExpression"/> represented by the element.</returns>
    protected virtual SwitchCase VisitSwitchCase(JElement e)
        => Expression.SwitchCase(
            VisitChild(e.GetElement(Vocabulary.Body)),
            e.GetArray(Vocabulary.CaseValues)
             .Select((e, i) => VisitChild(new($"case{i}", e))));

    /// <summary>
    /// Visits a Json element representing a conditional expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual ConditionalExpression VisitConditional(JElement e)
        => e.TryGetElement(out var eElse, Vocabulary.Else) && eElse.HasValue
            ? e.TryGetTypeFromProperty(out var type) && type is not null
                ? Expression.Condition(
                    VisitChild(e.GetElement(Vocabulary.If)),
                    VisitChild(e.GetElement(Vocabulary.Then)),
                    VisitChild(eElse!.Value),
                    type)
                : Expression.Condition(
                    VisitChild(e.GetElement(Vocabulary.If)),
                    VisitChild(e.GetElement(Vocabulary.Then)),
                    VisitChild(eElse!.Value))
            : Expression.IfThen(
                    VisitChild(e.GetElement(Vocabulary.If)),
                    VisitChild(e.GetElement(Vocabulary.Then)));

    /// <summary>
    /// Visits a Json element representing a `try...catch(x)...catch...finally` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual TryExpression VisitTry(JElement e)
        => Expression.MakeTry(
                e.TryGetTypeFromProperty(out var type) ? type : null,
                VisitChild(e.GetElement(Vocabulary.Body)),
                e.TryGetElement(out var final, Vocabulary.Finally) && final is not null
                    ? VisitChild(final.Value) : null,
                e.TryGetElement(out var catchAll, Vocabulary.Fault) && catchAll is not null
                    ? VisitChild(catchAll.Value)
                    : null,
                e.GetArray(Vocabulary.Catches)
                 .Select((c, i) => VisitCatchBlock(new($"catch{i}", c))));

    /// <summary>
    /// Visits a Json element representing a `catch(x) where filter {}` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="CatchBlock"/> represented by the element.</returns>
    protected virtual CatchBlock VisitCatchBlock(JElement e)
        => Expression.MakeCatchBlock(
                e.GetTypeFromProperty(),
                e.TryGetElement(out var exc, Vocabulary.Exception)
                  && exc.HasValue
                    ? VisitParameter(exc.Value)
                    : null,
                VisitChild(e.GetElement(Vocabulary.Body)),
                e.TryGetElement(out var f, Vocabulary.Filter)
                  && f.HasValue
                    ? VisitChild(f.Value)
                    : null);

    /// <summary>
    /// Visits a Json element representing a `new` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual NewExpression VisitNew(JElement e)
    {
        if (!e.TryGetElement(out var ciElement, Vocabulary.Constructor)
            || !ciElement.HasValue)
            return Expression.New(e.GetTypeFromProperty());

        var ci = VisitMemberInfo(ciElement.Value) as ConstructorInfo
                    ?? ciElement.Value.ThrowSerializationException<ConstructorInfo>($"Could not deserialize ConstructorInfo");
        var args = e.GetArray(Vocabulary.Arguments)
                    .Select((a,i) => VisitChild(new($"arg{i}", a)));
        var mems = e.TryGetArray(out var arrMem, Vocabulary.Members) && arrMem is not null
                        ? arrMem.Select((m,i) => VisitMemberInfo(new($"member{i}", m)) ?? throw new SerializationException($"Could not deserialize member 'member{i}' -- at '{arrMem[i]?.GetPath()}'."))
                        : null;

        return mems is null ? Expression.New(ci, args) : Expression.New(ci, args, mems);
    }

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
