﻿namespace vm2.ExpressionSerialization.XmlTransform;

using System.Reflection;
using System.Xml.Linq;

public partial class ToXmlTransformVisitor
{
    int _lastParamIdNumber;
    int _lastLabelIdNumber;
    // labels, parameters, and variables are created in one value and references to them are used in another.
    // These dictionaries keep their id-s so we can create `XAttribute` id-s and idRef-s to them.
    readonly Dictionary<ParameterExpression, XElement> _parameters = [];
    readonly Dictionary<LabelTarget, XElement> _labelTargets = [];

    /// <inheritdoc/>
    protected override void Reset()
    {
        base.Reset();

        _parameters.Clear();
        _labelTargets.Clear();
        _lastParamIdNumber = 0;
        _lastLabelIdNumber = 0;
    }

    string NewParameterId => $"P{++_lastParamIdNumber}";

    string NewLabelId => $"L{++_lastLabelIdNumber}";

    bool IsDefined(ParameterExpression parameterExpression)
        => _parameters.ContainsKey(parameterExpression);

    bool IsDefined(LabelTarget labelTarget)
        => _labelTargets.ContainsKey(labelTarget);

    XElement GetParameter(ParameterExpression parameterExpression)
        => _parameters.TryGetValue(parameterExpression, out var parameterElement)
                ? parameterElement
                : _parameters[parameterExpression] =
                        new XElement(
                                ElementNames.Parameter,
                                    AttributeType(parameterExpression),
                                    new XAttribute(AttributeNames.Id, NewParameterId),
                                    !string.IsNullOrWhiteSpace(parameterExpression.Name) ? new XAttribute(AttributeNames.Name, parameterExpression.Name) : null,
                                    parameterExpression.IsByRef ? new XAttribute(AttributeNames.IsByRef, parameterExpression.IsByRef) : null);

    XElement GetLabelTarget(LabelTarget labelTarget)
        => _labelTargets.TryGetValue(labelTarget, out var targetElement)
                ? targetElement
                : _labelTargets[labelTarget] =
                        new XElement(
                                ElementNames.LabelTarget,
                                    new XAttribute(AttributeNames.Id, NewLabelId),
                                    !string.IsNullOrWhiteSpace(labelTarget.Name) ? new XAttribute(AttributeNames.Name, labelTarget.Name) : null,
                                    labelTarget.Type != typeof(void) ? AttributeType(labelTarget.Type) : null);

    /// <summary>
    /// Pops one element from the stack
    /// <see cref="ExpressionTransformVisitor{TElement}._elements"/>.
    /// </summary>
    /// <returns>XElement.</returns>
    XElement PopElement() => _elements.Pop();

    /// <summary>
    /// Pops a number of elements in the order they entered the stack
    /// <see cref="ExpressionTransformVisitor{TElement}._elements"/>  (FIFO, not LIFO).
    /// </summary>
    /// <param name="numberOfExpressions">The number of expressions.</param>
    /// <returns>System.Collections.Generic.IEnumerable&lt;System.Xml.Linq.XElement&gt;.</returns>
    IEnumerable<XElement> PopElements(int numberOfExpressions)
    {
        // we need this intermediary stack to return the elements in FIFO order
        Stack<XElement> tempElements = new(numberOfExpressions);

        // pop the expressions:
        for (var i = 0; i < numberOfExpressions; i++)
            tempElements.Push(_elements.Pop());

        return tempElements;
    }

    XAttribute? AttributeName(string identifier)
        => !string.IsNullOrWhiteSpace(identifier)
                ? new XAttribute(AttributeNames.Name, Transform.Identifier(identifier, options.Identifiers))
                : null;

    static XAttribute? AttributeType(Expression? node)
        => node is not null
                ? AttributeType(node.Type)
                : null;

    static XAttribute? AttributeType(Type type)
        => type is not null
                ? new(AttributeNames.Type, Transform.TypeName(type))
                : null;

    static XElement? VisitMethodInfo(BinaryExpression node)
        => node.Method is MemberInfo mi
                ? VisitMemberInfo(mi)
                : null;

    static XElement? VisitMethodInfo(UnaryExpression node)
        => node.Method is MemberInfo mi
                ? VisitMemberInfo(mi)
                : null;

    static XElement? VisitMemberInfo(MemberInfo? member)
    {
        if (member is null)
            return null;

        XAttribute? declaringType = member.DeclaringType is Type dt ? new XAttribute(AttributeNames.DeclaringType, Transform.TypeName(dt)) : null;
        XAttribute? nameAttribute = member.Name is not null ? new XAttribute(AttributeNames.Name, member.Name) : null;
        XAttribute? visibility = member switch
            {
                ConstructorInfo ci => ci.IsPublic
                                        ? null
                                        : (ci.Attributes & MethodAttributes.MemberAccessMask) switch
                                            {
                                                MethodAttributes.Private     => new XAttribute(AttributeNames.Visibility, AttributeNames.Private),
                                                MethodAttributes.Assembly    => new XAttribute(AttributeNames.Visibility, AttributeNames.Assembly),
                                                MethodAttributes.Family      => new XAttribute(AttributeNames.Visibility, AttributeNames.Family),
                                                MethodAttributes.FamANDAssem => new XAttribute(AttributeNames.Visibility, AttributeNames.FamilyAndAssembly),
                                                MethodAttributes.FamORAssem  => new XAttribute(AttributeNames.Visibility, AttributeNames.FamilyOrAssembly),
                                                _                            => null
                                            },
                MethodInfo mi => mi.IsPublic
                                        ? null
                                        : (mi.Attributes & MethodAttributes.MemberAccessMask) switch
                                            {
                                                MethodAttributes.Private     => new XAttribute(AttributeNames.Visibility, AttributeNames.Private),
                                                MethodAttributes.Assembly    => new XAttribute(AttributeNames.Visibility, AttributeNames.Assembly),
                                                MethodAttributes.Family      => new XAttribute(AttributeNames.Visibility, AttributeNames.Family),
                                                MethodAttributes.FamANDAssem => new XAttribute(AttributeNames.Visibility, AttributeNames.FamilyAndAssembly),
                                                MethodAttributes.FamORAssem  => new XAttribute(AttributeNames.Visibility, AttributeNames.FamilyOrAssembly),
                                                _                            => null
                                            },
                FieldInfo fi =>  fi.IsPublic
                                        ? null
                                        : (fi.Attributes & FieldAttributes.FieldAccessMask) switch
                                            {
                                                FieldAttributes.Private     => new XAttribute(AttributeNames.Visibility, AttributeNames.Private),
                                                FieldAttributes.Assembly    => new XAttribute(AttributeNames.Visibility, AttributeNames.Assembly),
                                                FieldAttributes.Family      => new XAttribute(AttributeNames.Visibility, AttributeNames.Family),
                                                FieldAttributes.FamANDAssem => new XAttribute(AttributeNames.Visibility, AttributeNames.FamilyAndAssembly),
                                                FieldAttributes.FamORAssem  => new XAttribute(AttributeNames.Visibility, AttributeNames.FamilyOrAssembly),
                                                _                            => null
                                            },
                _ => null
            };

        return member switch {
            ConstructorInfo ci => new XElement(
                                    ElementNames.Constructor,
                                        declaringType,
                                        visibility,
                                        !ci.IsStatic ? null : throw new InternalTransformErrorException($"Don't know how to use static constructors."),
                                        new XElement(
                                                ElementNames.ParameterSpecs,
                                                VisitParameters(ci.GetParameters()))),

            PropertyInfo pi => new XElement(
                                    ElementNames.Property,
                                        declaringType,
                                        visibility,
                                        AttributeType(pi.PropertyType ?? throw new InternalTransformErrorException("PropertyInfo's DeclaringType is null.")),
                                        nameAttribute,
                                        pi.GetIndexParameters().Length != 0
                                            ? new XElement(
                                                    ElementNames.ParameterSpecs,
                                                    VisitParameters(pi.GetIndexParameters()))
                                            : null),

            MethodInfo mi => new XElement(
                                    ElementNames.Method,
                                        declaringType,
                                        mi.IsStatic ? new XAttribute(AttributeNames.Static, true) : null,
                                        visibility,
                                        AttributeType(mi.ReturnType),
                                        nameAttribute,
                                        new XElement(ElementNames.ParameterSpecs, VisitParameters(mi.GetParameters()))),

            EventInfo ei => new XElement(
                                    ElementNames.Event,
                                        declaringType,
                                        AttributeType(ei.EventHandlerType ?? throw new InternalTransformErrorException("EventInfo's EventHandlerType is null.")),
                                        nameAttribute),

            FieldInfo fi => new XElement(
                                    ElementNames.Field,
                                        declaringType,
                                        fi.IsStatic ? new XAttribute(AttributeNames.Static, true) : null,
                                        visibility,
                                        fi.IsInitOnly ? new XAttribute(AttributeNames.ReadOnly, true) : null,
                                        AttributeType(fi.FieldType ?? throw new InternalTransformErrorException("GetMethodInfo's DeclaringType is null.")),
                                        nameAttribute),

            _ => throw new InternalTransformErrorException("Unknown MemberInfo.")
        };
    }

    /// <summary>
    /// Creates a sequence of XML elements for each of the <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>A sequence of elements.</returns>
    static IEnumerable<XElement> VisitParameters(IEnumerable<ParameterInfo> parameters)
        => parameters.Select(param => new XElement(
                                            ElementNames.ParameterSpec,
                                                AttributeType(param.ParameterType),
                                                param.Name is not null ? new XAttribute(AttributeNames.Name, param.Name) : null,
                                                param.ParameterType.IsByRef || param.IsOut ? new XAttribute(AttributeNames.IsByRef, true) : null));
}
