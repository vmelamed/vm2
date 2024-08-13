namespace vm2.ExpressionSerialization.Json;

using System.Linq;

/// <summary>
/// Class that visits the nodes of a JSON node to produce a LINQ expression tree.
/// </summary>
public partial class FromJsonTransformVisitor
{
    /// <summary>
    /// Dispatches the visit to the concrete implementation based on the JSON element's name.
    /// </summary>
    /// <param name="e">The JSON element to be visited.</param>
    /// <returns>The deserialized sub-expression.</returns>
    /// <exception cref="SerializationException"/>
    public virtual Expression Visit(JElement e)
        => _transforms.TryGetValue(e.Name, out var visit) && visit is not null
                ? visit(this, e)
                : e.ThrowSerializationException<Expression>($"Don't know how to deserialize the element");

    #region Visiting children and grandchildren helpers
    /// <summary>
    /// Tries to visits the JsonObject property value with property name <paramref name="propertyName"/>.
    /// </summary>
    /// <param name="e">The JSON element which property's JsonObject to visit.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>Expression?</returns>
    protected Expression? TryVisitChild(JElement e, string propertyName)
        => e.TryGetElement(out var child, propertyName)
            && child is not null
            && child.Value.Node is JsonObject
                ? Visit(child.Value)
                : null;

    /// <summary>
    /// Visits the JsonObject property value with property name <paramref name="propertyName"/>.
    /// </summary>
    /// <param name="e">The JSON element which property's JsonObject to visit.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>Expression.</returns>
    /// <exception cref="SerializationException"/>
    protected Expression VisitChild(JElement e, string propertyName)
        => Visit(e.GetElement(propertyName));

    /// <summary>
    /// Visits the first JsonObject property value regardless of its name.
    /// </summary>
    /// <param name="e">The element whose JsonObject must be visited.</param>
    /// <returns>Expression.</returns>
    /// <exception cref="SerializationException"/>
    public Expression VisitFirstChild(JElement e)
        => e.TryGetFirstElement(out var child) && child is not null
                ? Visit(child.Value)
                : e.ThrowSerializationException<Expression>($"Could not find a child of 'JElement'");

    /// <summary>
    /// Visits the first child of the child with name <paramref name="propertyName"/>.
    /// </summary>
    /// <param name="e">The JSON element which grandchild to visit.</param>
    /// <param name="propertyName">Name of the parent property.</param>
    /// <returns>Expression?</returns>
    protected Expression? TryVisitFirstGrandChildOf(
        JElement e,
        string propertyName)
        => e.TryGetElement(out var child, propertyName)
            && child is not null
            && child.Value.Node is JsonObject
            && child.Value.TryGetFirstElement(out var grandchild)
            && grandchild is not null
                ? Visit(grandchild.Value)
                : null;

    /// <summary>
    /// Visits the element at e.child-name/[0]
    /// </summary>
    /// <param name="e">The JSON element which value's first JsonObject to visit.</param>
    /// <param name="childName">Name of the jsObj.</param>
    /// <returns>Expression.</returns>
    /// <exception cref="SerializationException"/>
    protected Expression VisitFirstGrandChildOf(
        JElement e,
        string childName)
        => VisitFirstChild(e.GetElement(childName));

    /// <summary>
    /// Visits the grandchild <paramref name="grandChildPropertyName" /> of the child <paramref name="childPropertyName" />.
    /// </summary>
    /// <param name="e">The JSON element which grandchild to visit.</param>
    /// <param name="childPropertyName">Name of the child property.</param>
    /// <param name="grandChildPropertyName">Name of the grand child property.</param>
    /// <returns>Expression?</returns>
    protected Expression? TryVisitGrandchild(
        JElement e,
        string childPropertyName,
        string grandChildPropertyName)
        => e.TryGetElement(out var child, childPropertyName)
            && child is not null
            && child.Value.Node is JsonObject
            && child.Value.TryGetElement(out var grandchild, grandChildPropertyName)
            && grandchild is not null
            && grandchild.Value.Node is JsonObject
                ? Visit(grandchild.Value)
                : null;

    /// <summary>
    /// Visits the grandchild <paramref name="grandChildPropertyName" /> of the child <paramref name="childPropertyName" />.
    /// </summary>
    /// <param name="e">The JSON element which grandchild to visit.</param>
    /// <param name="childPropertyName">Name of the child property.</param>
    /// <param name="grandChildPropertyName">Name of the grand child property.</param>
    /// <returns>Expression.</returns>
    /// <exception cref="SerializationException"/>
    protected Expression VisitGrandchild(
        JElement e,
        string childPropertyName,
        string grandChildPropertyName)
        => Visit(
                e.GetElement(childPropertyName)
                 .GetElement(grandChildPropertyName));

    /// <summary>
    /// Visits the items of an array from the element <paramref name="e"/> with name <paramref name="arrayName"/>
    /// producing a sequence of results of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result from visiting an array item, e.g. <see cref="Expression"/>.</typeparam>
    /// <param name="e">The element that has the array in a property.</param>
    /// <param name="arrayName">Name of the array property.</param>
    /// <param name="visitor">The visitor function.</param>
    /// <returns>IEnumerable&lt;T&gt;.</returns>
    protected static IEnumerable<T>? TryVisitArray<T>(
        JElement e,
        string arrayName,
        Func<JElement, T> visitor)
        => e.TryGetArray(out var array, arrayName) && array is not null
                ? array.Select((e, i) => visitor(new($"item{i}", e)))
                : null;

    /// <summary>
    /// Visits the items of an array from the element <paramref name="e"/> with name <paramref name="arrayName"/>
    /// producing a sequence of results of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result from visiting an array item, e.g. <see cref="Expression"/>.</typeparam>
    /// <param name="e">The element that has the array in a property.</param>
    /// <param name="arrayName">Name of the array property.</param>
    /// <param name="visitor">The visitor function.</param>
    /// <returns>IEnumerable&lt;T&gt;.</returns>
    protected static IEnumerable<T> VisitArray<T>(
        JElement e,
        string arrayName,
        Func<JElement, T> visitor)
        => e.GetArray(arrayName).Select((e, i) => visitor(new($"item{i}", e)));
    #endregion

    #region Json element visitors
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
        => Expression.Default(
                e.GetTypeFromProperty());

    #region Unary, binary, block
    /// <summary>
    /// Visits a Json element representing a unary expression, e.g. `-a`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual UnaryExpression VisitUnary(JElement e)
        => Expression.MakeUnary(
                e.GetExpressionType(),
                VisitFirstGrandChildOf(e, Vocabulary.Operand),
                e.GetTypeFromProperty(),
                TryGetMemberInfo(e, Vocabulary.Method) as MethodInfo);

    /// <summary>
    /// Visits a Json element representing a binary expression, e.g. `a + b`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual BinaryExpression VisitBinary(JElement e)
    {
        var operands = e.GetArray(Vocabulary.Operands);
        const string excMessage = "Expected exactly two JsonObject operands in the binary expression";

        if (operands.Count != 2)
            e.ThrowSerializationException<int>(excMessage);

        var operandL = operands[0]?.AsObject() ?? operands.ThrowSerializationException<JsonObject>(excMessage);
        var operandR = operands[1]?.AsObject() ?? operands.ThrowSerializationException<JsonObject>(excMessage);

        return Expression.MakeBinary(
                            e.GetExpressionType(),
                            Visit(operandL.GetFirstObject()),
                            Visit(operandR.GetFirstObject()),
                            e.TryGetPropertyValue<bool>(out var isLiftedToNull, Vocabulary.IsLiftedToNull) && isLiftedToNull,
                            TryGetMemberInfo(e, Vocabulary.Method) as MethodInfo,
                            e.TryGetElement(out var convert, Vocabulary.Convert) && convert is not null
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
        const string excMessage = "Expected exactly one JsonObject operand to a type binary expression";

        if (operands.Count != 1)
            e.ThrowSerializationException<JElement>(excMessage);

        var operand = operands[0]?.AsObject() ?? operands.ThrowSerializationException<JsonObject>(excMessage);

        return e.Name switch {
            "typeIs" => Expression.TypeIs(
                            Visit(operand.GetFirstObject()),
                            e.GetTypeFromProperty(Vocabulary.TypeOperand)),

            "typeEqual" => Expression.TypeEqual(
                            Visit(operand.GetFirstObject()),
                            e.GetTypeFromProperty(Vocabulary.TypeOperand)),

            _ => e.ThrowSerializationException<TypeBinaryExpression>($"Don't know how to transform '{e.Name}' to a 'TypeBinaryExpression'"),
        };
    }

    /// <summary>
    /// Visits a Json element representing a block expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual BlockExpression VisitBlock(JElement e)
        => e.TryGetTypeFromProperty(out var type) && type is not null
            ? Expression.Block(
                    type,
                    TryVisitArray(e, Vocabulary.Variables, VisitParameter),
                    VisitArray(e, Vocabulary.Expressions, x => Visit(x.GetFirstElement())))
            : Expression.Block(
                    TryVisitArray(e, Vocabulary.Variables, VisitParameter),
                    VisitArray(e, Vocabulary.Expressions, x => Visit(x.GetFirstElement())));
    #endregion

    #region Index
    /// <summary>
    /// Visits a Json element representing an index expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitIndex(JElement e)
        => Expression.ArrayAccess(
                VisitFirstGrandChildOf(e, Vocabulary.Object),
                VisitIndexes(e));

    /// <summary>
    /// Visits the indexes elements of an indexing operation.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>System.Collections.Generic.IEnumerable&lt;System.Linq.Expressions.Expression&gt;.</returns>
    protected virtual IEnumerable<Expression> VisitIndexes(JElement e)
        => VisitArray(e, Vocabulary.Indexes, i => Visit(i.GetFirstElement()));
    #endregion

    #region Access Fields or Properties. Invoke Methods
    /// <summary>
    /// Visits a Json element representing a `Member` access expression, e.g. `a.Abc`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitMember(JElement e)
    {
        var memberInfo = TryVisitMemberInfo(
                            e.GetElement(Vocabulary.Member)
                             .GetFirstElement());

        if (memberInfo is not PropertyInfo and not FieldInfo)
            return e.ThrowSerializationException<Expression>($"Expected '{Vocabulary.Member}/{Vocabulary.Property}' or '{Vocabulary.Member}/{Vocabulary.Field}' element");

        return Expression.MakeMemberAccess(
                    TryVisitFirstGrandChildOf(e, Vocabulary.Object),
                    memberInfo);
    }

    /// <summary>
    /// Visits a Json element representing a Method call expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual MethodCallExpression VisitMethodCall(JElement e)
        => Expression.Call(
                TryVisitFirstGrandChildOf(e, Vocabulary.Object),
                TryVisitMemberInfo(e.GetElement(Vocabulary.Method)) as MethodInfo
                        ?? e.ThrowSerializationException<MethodInfo>($"Expected '{Vocabulary.Method}' element"),
                VisitArray(e, Vocabulary.Arguments, VisitFirstChild));
    #endregion

    #region Goto, labels, if, loop, switch
    /// <summary>
    /// Visits a Json element representing a `XXXX` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression" /> represented by the element.</returns>
    /// <exception cref="SerializationException">$"Expected element with name `{expectedName}` but got `{e.Name}`.</exception>
    protected virtual LabelExpression VisitLabel(JElement e)
        => Expression.Label(
                GetTarget(e.GetElement(Vocabulary.LabelTarget)),
                TryVisitFirstGrandChildOf(e, Vocabulary.Default));

    /// <summary>
    /// Visits a Json element representing a `goto` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual GotoExpression VisitGoto(JElement e)
    {
        var target = GetTarget(e.GetElement(Vocabulary.LabelTarget));

        return Expression.MakeGoto(
                    e.GetPropertyValue<GotoExpressionKind>(Vocabulary.Kind),
                    target,
                    TryVisitFirstGrandChildOf(e, Vocabulary.Value),
                    target.Type);
    }

    /// <summary>
    /// Visits the first JsonObject child with name <paramref name="labelName"/>.
    /// </summary>
    /// <param name="e">The JSON element which value's first JsonObject to visit.</param>
    /// <param name="labelName">Name of the jsObj.</param>
    /// <returns>Expression.</returns>
    protected virtual LabelTarget? TryVisitChildLabelTarget(JElement e, string labelName)
        => e.TryGetElement(out var label, labelName)
            && label is not null
            && label.Value.Node is JsonObject
                ? VisitLabel(label.Value).Target
                : null;

    /// <summary>
    /// Visits a Json element representing a conditional expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual ConditionalExpression VisitConditional(JElement e)
        => e.TryGetElement(out var eElse, Vocabulary.Else) && eElse is not null
            ? e.TryGetTypeFromProperty(out var type) && type is not null
                ? Expression.Condition(
                    VisitFirstGrandChildOf(e, Vocabulary.If),
                    VisitFirstGrandChildOf(e, Vocabulary.Then),
                    VisitFirstChild(eElse!.Value),
                    type)
                : Expression.Condition(
                    VisitFirstGrandChildOf(e, Vocabulary.If),
                    VisitFirstGrandChildOf(e, Vocabulary.Then),
                    VisitFirstChild(eElse!.Value))
            : Expression.IfThen(
                    VisitFirstGrandChildOf(e, Vocabulary.If),
                    VisitFirstGrandChildOf(e, Vocabulary.Then));

    /// <summary>
    /// Visits a Json element representing a `loop` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual Expression VisitLoop(JElement e)
        => Expression.Loop(
                VisitFirstGrandChildOf(e, Vocabulary.Body),
                TryVisitChildLabelTarget(e, Vocabulary.BreakLabel),
                TryVisitChildLabelTarget(e, Vocabulary.ContinueLabel));

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
            VisitFirstGrandChildOf(e, Vocabulary.Value),
            TryVisitFirstGrandChildOf(e, Vocabulary.DefaultCase),
            e.TryGetElement(out var compare, Vocabulary.Method)
              && compare is not null
              && compare.Value.Node is JsonObject
                ? TryVisitMemberInfo(compare.Value.GetFirstElement()) as MethodInfo
                : null,
            e.GetArray(Vocabulary.Cases).Select((c, i) => VisitSwitchCase(new($"cases{i}", c))));

    /// <summary>
    /// Visits a Json element representing a `switch case` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="SwitchExpression"/> represented by the element.</returns>
    protected virtual SwitchCase VisitSwitchCase(JElement e)
        => Expression.SwitchCase(
            VisitFirstGrandChildOf(e, Vocabulary.Body),
            VisitArray(e, Vocabulary.CaseValues, VisitFirstChild));
    #endregion

    #region Parameters, lambda, invocation
    /// <summary>
    /// Visits an Json element representing a parameter expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="ParameterExpression" /> represented by the element.</returns>
    /// <exception cref="SerializationException">$</exception>
    protected virtual ParameterExpression VisitParameter(JElement e)
        => GetParameter(e);

    /// <summary>
    /// Visits an Json element representing a parameter expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <param name="expectedName">The expected name of the element, e.g. 'variable' or `parameter`.</param>
    /// <returns>The <see cref="ParameterExpression" /> represented by the element.</returns>
    /// <exception cref="SerializationException">$</exception>
    protected virtual ParameterExpression VisitParameter(
        JElement e,
        string expectedName)
        => e.Name == expectedName
                ? GetParameter(e)
                : e.ThrowSerializationException<ParameterExpression>($"Expected parameter with name '{expectedName}' but got '{e.Name}'");

    /// <summary>
    /// Visits a Json element representing a lambda expression, e.g. `a => a.Abc + 42`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="LambdaExpression"/> represented by the element.</returns>
    protected virtual LambdaExpression VisitLambda(JElement e)
        => Expression.Lambda(
                VisitFirstGrandChildOf(e, Vocabulary.Body),
                e.TryGetPropertyValue<bool>(out var tailCall, Vocabulary.TailCall) && tailCall,
                VisitArray(e, Vocabulary.Parameters, VisitParameter));

    /// <summary>
    /// Visits a Json element representing invocation of a `delegate` or lambda expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual InvocationExpression VisitInvocation(JElement e)
        => Expression.Invoke(
                VisitFirstChild(e.GetElement(Vocabulary.Delegate)),
                VisitArray(e, Vocabulary.Arguments, VisitFirstChild));
    #endregion

    #region Throw, try-catch
    /// <summary>
    /// Visits a Json element representing a `throw` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual UnaryExpression VisitThrow(JElement e)
        => e.TryGetTypeFromProperty(out var type) && type is not null
                ? Expression.Throw(VisitFirstGrandChildOf(e, Vocabulary.Operand), type)
                : Expression.Throw(VisitFirstGrandChildOf(e, Vocabulary.Operand));

    /// <summary>
    /// Visits a Json element representing a `try...catch(x)...catch...finally` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual TryExpression VisitTry(JElement e)
        => Expression.MakeTry(
                e.TryGetTypeFromProperty(out var type) ? type : null,
                VisitFirstGrandChildOf(e, Vocabulary.Body),
                TryVisitFirstGrandChildOf(e, Vocabulary.Finally),
                TryVisitFirstGrandChildOf(e, Vocabulary.Fault),
                VisitArray(e, Vocabulary.Catches, VisitCatchBlock));

    /// <summary>
    /// Visits a Json element representing a `catch(x) where filter {}` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="CatchBlock"/> represented by the element.</returns>
    protected virtual CatchBlock VisitCatchBlock(JElement e)
        => Expression.MakeCatchBlock(
                e.GetTypeFromProperty(),
                e.TryGetElement(out var exc, Vocabulary.Exception)
                    && exc is not null
                    && exc.Value.Node is JsonObject
                    ? VisitParameter(exc.Value)
                    : null,
                VisitFirstGrandChildOf(e, Vocabulary.Body),
                TryVisitFirstGrandChildOf(e, Vocabulary.Filter));
    #endregion

    #region New object, array
    /// <summary>
    /// Visits a Json element representing a `new` expression.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual NewExpression VisitNew(JElement e)
    {
        if (!e.TryGetElement(out var ciElement, Vocabulary.Constructor) || ciElement is null)
            return Expression.New(e.GetTypeFromProperty());

        var ctor = VisitMemberInfo(ciElement.Value) as ConstructorInfo
                            ?? ciElement.Value.ThrowSerializationException<ConstructorInfo>($"Could not deserialize ConstructorInfo");
        var args = TryVisitArray(e, Vocabulary.Arguments, VisitFirstChild);
        var members = TryVisitArray(e, Vocabulary.Members, VisitMemberInfo);

        return members is null
            ? Expression.New(ctor, args)
            : Expression.New(ctor, args, members);
    }

    /// <summary>
    /// Visits a Json element representing a `new` array of the specified type with the provided initializers.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual NewArrayExpression VisitNewArrayInit(JElement e)
        => Expression.NewArrayInit(
                e.GetType(),
                VisitArray(e, Vocabulary.ArrayElements, VisitFirstChild));

    /// <summary>
    /// Visits a Json element representing a `new` multidimensional array of the specified type with the specified bounds.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual NewArrayExpression VisitNewArrayBounds(JElement e)
        => Expression.NewArrayBounds(
                e.GetType(),
                VisitArray(e, Vocabulary.Bounds, VisitFirstChild));
    #endregion

    #region Type member and collection items with Bindings
    /// <summary>
    /// Visits a new list with initializers, e.g. `new() { 1, a++, b+c }`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>System.Linq.Expressions.ListInitExpression.</returns>
    protected virtual ListInitExpression VisitListInit(JElement e)
        => Expression.ListInit(
                VisitNew(e.GetElement(Vocabulary.New)),
                VisitArray(e, Vocabulary.Initializers, VisitElementInit));

    /// <summary>
    /// Visits a Json element representing a member init expression, e.g. the part `Name = "abc"` or `List = new() { 1, 2, 3 }` from the
    /// member initialization `new Obj() { Name = "abc", List = new() { 1, 2, 3 }, };`.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>The <see cref="Expression"/> represented by the element.</returns>
    protected virtual MemberInitExpression VisitMemberInit(JElement e)
        => Expression.MemberInit(
                VisitNew(e.GetFirstElement()),
                VisitArray(e, Vocabulary.Bindings, e => VisitBinding(e.GetFirstElement())));

    /// <summary>
    /// Dispatches the binding visitation to the right handler.
    /// </summary>
    /// <param name="e">The binding e.</param>
    /// <returns>System.Linq.Expressions.MemberBinding.</returns>
    protected virtual MemberBinding VisitBinding(JElement e)
        => e.Name switch {
            Vocabulary.AssignmentBinding => VisitAssignmentBinding(e),
            Vocabulary.MemberMemberBinding => VisitMemberMemberBinding(e),
            Vocabulary.MemberListBinding => VisitMemberListBinding(e),
            _ => throw new SerializationException($"Don't know how to deserialize member binding `{e.GetName()}`"),
        };

    static MemberInfo VisitMemberInfoForBinding(JElement e)
    {
        var me = e.GetElement(Vocabulary.Member);

        if (!me.TryGetElement(out var mi, Vocabulary.Property)
            && !me.TryGetElement(out mi, Vocabulary.Field)
            && !me.TryGetElement(out mi, Vocabulary.Method)
                || mi is null)
            throw new SerializationException("Could not find property, field, or method to bind to");

        return TryVisitMemberInfo(mi.Value) ?? me.ThrowSerializationException<MemberInfo>($"Could not deserialize member info from");
    }

    /// <summary>
    /// Visits a JSON representation of an assignment binding of a value (expression) to
    /// a member - field, property, or accessor method.
    /// </summary>
    /// <param name="e">The node representing the binding expression.</param>
    /// <returns>MemberBinding.</returns>
    /// <exception cref="SerializationException">Could not find property or field to bind to</exception>
    protected virtual MemberAssignment VisitAssignmentBinding(JElement e)
        => VisitMemberInfoForBinding(e) switch {
            MethodInfo methodInfo => Expression.Bind(methodInfo, VisitFirstGrandChildOf(e, Vocabulary.Value)),
            MemberInfo memberInfo => Expression.Bind(memberInfo, VisitFirstGrandChildOf(e, Vocabulary.Value)),
            _ => e.ThrowSerializationException<MemberAssignment>("Could not find property, field, or method to bind to"),
        };

    /// <summary>
    /// Visits a JSON representation of a recursive initialization of the members of a
    /// complex type member - field, property, or accessor method.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>MemberBinding.</returns>
    protected virtual MemberBinding VisitMemberMemberBinding(JElement e)
        => VisitMemberInfoForBinding(e) switch {
            MethodInfo methodInfo => Expression.MemberBind(methodInfo, VisitArray(e, Vocabulary.Bindings, VisitBinding)),
            MemberInfo memberInfo => Expression.MemberBind(memberInfo, VisitArray(e, Vocabulary.Bindings, VisitBinding)),
            _ => e.ThrowSerializationException<MemberAssignment>("Could not find property, field, or method to bind to"),
        };

    /// <summary>
    /// Visits a JSON representation of initialization of the items of a
    /// sequence type of member - field, property, or accessor method.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>System.Linq.Expressions.MemberBinding.</returns>
    protected virtual MemberBinding VisitMemberListBinding(JElement e)
        => VisitMemberInfoForBinding(e) switch {
            MethodInfo methodInfo => Expression.ListBind(methodInfo, VisitArray(e, Vocabulary.ElementInit, VisitElementInit)),
            MemberInfo memberInfo => Expression.ListBind(memberInfo, VisitArray(e, Vocabulary.ElementInit, VisitElementInit)),
            _ => e.ThrowSerializationException<MemberAssignment>("Could not find property, field, or method to bind to"),
        };

    /// <summary>
    /// Visits a Json element that represents a collection element initialization.
    /// </summary>
    /// <param name="e">The element.</param>
    /// <returns>System.Linq.Expressions.ElementInit.</returns>
    protected virtual ElementInit VisitElementInit(JElement e)
        => Expression.ElementInit(
                VisitMemberInfo(e.GetElement(Vocabulary.Method)) as MethodInfo
                                    ?? e.ThrowSerializationException<MethodInfo>("Could not deserialize add MethodInfo"),
                VisitArray(e, Vocabulary.Arguments, VisitFirstChild));
    #endregion
    #endregion
}
