﻿namespace vm2.ExpressionSerialization.JsonTransform;

/// <summary>
/// Class that visits the nodes of a JSON node to produce a LINQ expression tree.
/// </summary>
public partial class FromJsonTransformVisitor
{
    /// <summary>
    /// Dispatches the visit to the concrete implementation based on the e's name.
    /// </summary>
    /// <param name="e">The e to be visited.</param>
    /// <returns>The created expression.</returns>
    public virtual Expression Visit(JElement e)
        => _transforms.TryGetValue(e.Name, out var visit)
                ? visit(this, e)
                : throw new SerializationException($"Don't know how to deserialize the e '{e.Name}' at {e.GetPath()}.");

    #region Concrete Json element visitors
    /// <summary>
    /// Visits the <paramref name="childIndex"/>-th child of the element.
    /// </summary>
    /// <param name="e">The element whose child must be visited.</param>
    /// <param name="childIndex">Index of the child.</param>
    /// <returns>Expression.</returns>
    /// <exception cref="System.Runtime.Serialization.SerializationException">Could not find object with children at {e.GetPath()}</exception>
    public virtual Expression VisitChild(JElement e, int childIndex = 0)
    {
        var jsObj = e.Value?.AsObject();

        if (jsObj is null)
            throw new SerializationException($"Could not find object with children at {e.GetPath()}");

        foreach (var child in jsObj.Where(c => c.Value is JsonObject))
            if (childIndex-- == 0)
                return Visit(child);

        throw new SerializationException($"Could not find child #{childIndex} at {e.GetPath()}");
    }

    /// <summary>
    /// Visits the first a child node with name <paramref name="childName"/>.
    /// </summary>
    /// <param name="e">The J e which value's first JsonObject to visit.</param>
    /// <param name="childName">Name of the child.</param>
    /// <returns>Expression.</returns>
    protected virtual Expression VisitChild(JElement e, string childName)
        => Visit(e.GetElement(childName));

    /// <summary>
    /// Visits a JSON e representing a constant expression, e.g. `42`.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>The <see cref="ConstantExpression"/> represented by the e.</returns>
    protected virtual Expression VisitConstant(JElement e)
        => FromJsonDataTransform.ConstantTransform(e);

    /// <summary>
    /// Visits an JSON e representing a default expression (e.g. <c>default(int)</c>).
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>The <see cref="ParameterExpression"/> represented by the e.</returns>
    protected virtual Expression VisitDefault(JElement e)
        => Expression.Default(e.GetTypeFromProperty());

    /// <summary>
    /// Visits an Json e representing a parameter expression.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <param name="expectedName">The expected name of the e, e.g. 'variable' or `parameter`.</param>
    /// <returns>The <see cref="ParameterExpression" /> represented by the e.</returns>
    /// <exception cref="SerializationException">$</exception>
    protected virtual ParameterExpression VisitParameter(
        JElement e,
        string? expectedName = null)
    {
        if (expectedName is not null
            && e.Name != expectedName)
            throw new SerializationException($"Expected e {(string.IsNullOrWhiteSpace(expectedName) ? "" : $" with name '{expectedName}' but got '{e.Name}'")} at '{e.GetPath()}'.");

        return GetParameter(e);
    }

    /// <summary>
    /// Visits a Json e representing a list of parameter definition expressions.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>The <see cref="IEnumerable{ParameterExpression}"/> represented by the e.</returns>
    protected virtual IEnumerable<ParameterExpression> VisitParameterList(JElement e)
        => e.Value?
            .AsArray()?
            .Select((pe, i) => VisitParameter(($"param{i}", pe)))
                ?? throw new SerializationException($"Expected array of parameters at '{e.GetPath()}'.");

    /// <summary>
    /// Visits an Json e representing a lambda expression, e.g. `a => a.Abc + 42`.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>The <see cref="LambdaExpression"/> represented by the e.</returns>
    protected virtual LambdaExpression VisitLambda(JElement e)
        => Expression.Lambda(
                        VisitChild(e.GetElement(Vocabulary.Body)),
                        e.TryGetPropertyValue<bool>(out var tailCall, Vocabulary.TailCall) && tailCall,
                        VisitParameterList((Vocabulary.Parameters, e.GetArray(Vocabulary.Parameters))));

    /// <summary>
    /// Visits an Json e representing a unary expression, e.g. `-a`.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>The <see cref="Expression"/> represented by the e.</returns>
    protected virtual UnaryExpression VisitUnary(JElement e)
    {
        var operands = e.GetArray(Vocabulary.Operands);

        if (operands.Count != 1 ||
            operands[0]?.AsObject() is null)
            throw new SerializationException($"Expected exactly one operand to unary expression at '{e.GetPath()}'");

        return Expression.MakeUnary(
                        e.GetExpressionType(),
                        Visit(operands[0]!.AsObject()!.GetFirstObject()),
                        e.GetTypeFromProperty(),
                        GetMemberInfo(e, Vocabulary.Method) as MethodInfo);
    }

    /// <summary>
    /// Visits an Json e representing a binary expression, e.g. `a + b`.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>The <see cref="Expression"/> represented by the e.</returns>
    protected virtual BinaryExpression VisitBinary(JElement e)
    {
        var operands = e.GetArray(Vocabulary.Operands);

        if (operands.Count != 2 ||
            operands[0]?.AsObject() is null)
            throw new SerializationException($"Expected exactly two operands to binary expression at '{e.GetPath()}'");

        return Expression.MakeBinary(
                            e.GetExpressionType(),
                            Visit(operands[0]!.AsObject()!.GetFirstObject()),
                            Visit(operands[1]!.AsObject()!.GetFirstObject()),
                            e.TryGetPropertyValue<bool>(out var isLiftedToNull, Vocabulary.IsLiftedToNull) && isLiftedToNull,
                            GetMemberInfo(e, Vocabulary.Method) as MethodInfo,
                            e.TryGetElement(out var convert, Vocabulary.Convert) && convert.HasValue
                                ? Visit(convert.Value) as LambdaExpression
                                : null);
    }

    ///// <summary>
    ///// Visits an Json e representing a type binary expression, e.g. `x is Type`.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
    //protected virtual TypeBinaryExpression VisitTypeBinary(JElement e)
    //    => e.Name switch {
    //        "typeIs" => Expression.TypeIs(
    //                        VisitChild(e),
    //                        e.GetTypeFromProperty(AttributeNames.TypeOperand)),

    //        "typeEqual" => Expression.TypeEqual(
    //                        VisitChild(e),
    //                        e.GetTypeFromProperty(AttributeNames.TypeOperand)),

    //        _ => throw new SerializationException($"Don't know how to transform {e.Name} to a `TypeBinaryExpression`."),
    //    };

    ///// <summary>
    ///// Visits an Json e representing an index expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
    //protected virtual Expression VisitIndex(JElement e)
    //    => Expression.ArrayAccess(
    //            VisitChild(e),
    //            VisitIndexes(e));

    ///// <summary>
    ///// Visits the indexes e of an index operation.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>System.Collections.Generic.IEnumerable&lt;System.Linq.Expressions.Expression&gt;.</returns>
    //protected virtual IEnumerable<Expression> VisitIndexes(JElement e)
    //    => e.GetElement(Vocabulary.Indexes)
    //                        .Elements()
    //                        .Select(Visit);

    ///// <summary>
    ///// Visits an Json e representing a block expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
    //protected virtual BlockExpression VisitBlock(JElement e)
    //    => Expression.Block(
    //                        (e.TryGetFirstElement(Vocabulary.Variables, out var vars) ? vars : null)?
    //                         .Elements()?
    //                         .Select(v => VisitParameter(v, Vocabulary.Parameter)),
    //                        e.Elements()
    //                         .Where(e => e.Name != Vocabulary.Variables)
    //                         .Select(Visit));

    ///// <summary>
    ///// Visits an Json e representing a conditional expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
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
    ///// Visits an Json e representing a `new` expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
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

    ///// <summary>
    ///// Visits an Json e representing a `throw` expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
    //protected virtual UnaryExpression VisitThrow(JElement e)
    //    => Expression.Throw(VisitChild(e, 0));

    ///// <summary>
    ///// Visits an Json e representing a `Member` access expression, e.g. `a.Abc`.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
    //protected virtual Expression VisitMember(JElement e)
    //    => Expression.MakeMemberAccess(
    //                        VisitChild(e, 0),
    //                        VisitMemberInfo(
    //                            e.TryGetFirstElement(Vocabulary.Property, out var mem) ||
    //                            e.TryGetFirstElement(Vocabulary.Field, out mem) ? mem : throw new SerializationException($"Could not deserialize `property` or `field` in `{e.Name}`"))
    //                                    ?? throw new SerializationException($"Could not deserialize `MemberInfo` in `{e.Name}`"));

    ///// <summary>
    ///// Visits an Json e representing a `XXXX` expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
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
    ///// Visits an Json e representing a `delegate` or lambda invocation expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
    //protected virtual InvocationExpression VisitInvocation(JElement e)
    //    => Expression.Invoke(
    //                        VisitChild(e, 0),
    //                        e.Elements(ElementNames.Arguments).Elements().Select(Visit));

    ///// <summary>
    ///// Visits an Json e representing a `XXXX` expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression" /> represented by the e.</returns>
    ///// <exception cref="SerializationException">$"Expected e with name `{expectedName}` but got `{e.Name}`.</exception>
    //protected virtual LabelExpression VisitLabel(JElement e)
    //    => Expression.Label(
    //                        VisitLabelTarget(e.GetElement(Vocabulary.LabelTarget)),
    //                        e.TryGetFirstElement(1, out var value) && value != null ? Visit(value) : null);

    ///// <summary>
    ///// Visits an Json e representing a `LabelTarget` expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    /////
    ///// <returns>System.Linq.Expressions.LabelTarget.</returns>
    ///// <exception cref="SerializationException">$"Expected Json attribute `{(isRef.Value ? Vocabulary.IdRef : Vocabulary.Id)}` in the e `{e.Name}`.</exception>
    //protected virtual LabelTarget VisitLabelTarget(JElement e)
    //    => GetTarget(e);

    ///// <summary>
    ///// Visits an Json e representing a `goto` expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
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
    ///// Visits an Json e representing a `loop` expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
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
    ///// Visits an Json e representing a `switch` expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
    //protected virtual SwitchExpression VisitSwitch(JElement e)
    //    => Expression.Switch(
    //                        e.TryGetETypeFromAttribute(out var type) ? type : null,
    //                        VisitChild(e, 0),
    //                        e.TryGetFirstElement(Vocabulary.DefaultCase, out var elem) && elem is not null ? Visit(elem.GetElement(0)) : null,
    //                        e.TryGetFirstElement(Vocabulary.Method, out var comp) ? VisitMemberInfo(comp) as MethodInfo : null,
    //                        e.Elements(ElementNames.Case).Select(VisitSwitchCase));

    ///// <summary>
    ///// Visits an Json e representing a `switch case` expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="SwitchExpression"/> represented by the e.</returns>
    //protected virtual SwitchCase VisitSwitchCase(JElement e)
    //    => Expression.SwitchCase(
    //                        e.Elements().Where(e => e.Name is not Vocabulary.CaseValues).Select(Visit).Single(),
    //                        e.Element(ElementNames.CaseValues)?.Elements().Select(Visit) ?? throw new SerializationException($"Could not get a switch case's test values in `{e.Name}`"));

    ///// <summary>
    ///// Visits an Json e representing a `try...catch(x)...catch...finally` expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
    //protected virtual TryExpression VisitTry(JElement e)
    //    => Expression.MakeTry(
    //                        e.TryGetETypeFromAttribute(out var type) ? type : null,
    //                        VisitChild(e, 0),
    //                        e.TryGetFirstElement(Vocabulary.Finally, out var final) && final is not null ? Visit(final.GetElement(0)) : null,
    //                        e.TryGetFirstElement(Vocabulary.Fault, out var catchAll) && catchAll is not null ? Visit(catchAll.GetElement(0)) : null,
    //                        e.Elements(ElementNames.Catch).Select(VisitCatchBlock));

    ///// <summary>
    ///// Visits an Json e representing a `catch(x) where filter {}` expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="CatchBlock"/> represented by the e.</returns>
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
    ///// Visits an Json e representing a member init expression, e.g. the part `Name = "abc"` or `List = new() { 1, 2, 3 }` from the
    ///// member initialization `new Obj() { Name = "abc", List = new() { 1, 2, 3 }, };`.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
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
    ///// Visits an Json e that represents a collection e initialization.
    ///// </summary>
    ///// <param name="e">The e.</param>
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
    ///// <param name="e">The e.</param>
    ///// <returns>System.Linq.Expressions.ListInitExpression.</returns>
    //protected virtual ListInitExpression VisitListInit(JElement e)
    //    => Expression.ListInit(
    //            VisitNew(e.GetElement(Vocabulary.New)),
    //            e.GetElement(Vocabulary.Initializers)
    //             .Elements()
    //             .Select(VisitElementInit));

    ///// <summary>
    ///// Visits an Json e representing a `XXXX` expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
    //protected virtual NewArrayExpression VisitNewArrayInit(JElement e)
    //    => Expression.NewArrayInit(
    //            e.GetEType(),
    //            e.GetElement(Vocabulary.ArrayElements)
    //             .Elements()
    //             .Select(Visit));

    ///// <summary>
    ///// Visits an Json e representing a `XXXX` expression.
    ///// </summary>
    ///// <param name="e">The e.</param>
    ///// <returns>The <see cref="Expression"/> represented by the e.</returns>
    //protected virtual NewArrayExpression VisitNewArrayBounds(JElement e)
    //    => Expression.NewArrayBounds(
    //            e.GetEType(),
    //            e.GetElement(Vocabulary.Bounds)
    //             .Elements()
    //             .Select(Visit));
    #endregion
}
