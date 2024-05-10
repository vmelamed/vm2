namespace vm2.ExpressionSerialization.XmlTransform;

using System.Xml.Linq;

/// <summary>
/// Class that visits the nodes of an XML element to produce a LINQ expression tree.
/// </summary>
public partial class FromXmlTransformVisitor
{
    #region Dispatch map for the concrete XML element Expression transforming visitors
    /// <summary>
    /// Holds dictionary of expression element name - delegate to the respective transform.
    /// </summary>
    static readonly Dictionary<XName, Func<FromXmlTransformVisitor, XElement, Expression>> _transforms = new()
    {
            { ElementNames.Expression,           (v, e) => v.VisitExpression(e)             },
            { ElementNames.Constant,             (v, e) => VisitConstant(e)                 },
            { ElementNames.ParameterSpec,        (v, e) => v.VisitParameter(e)              },
            { ElementNames.ParameterDefinition,  (v, e) => v.VisitParameter(e)              },
            { ElementNames.ParameterReference,   (v, e) => v.VisitParameter(e)              },
            { ElementNames.VariableDefinition,   (v, e) => v.VisitParameter(e)              },
            { ElementNames.VariableReference,    (v, e) => v.VisitParameter(e)              },
            { ElementNames.Lambda,               (v, e) => v.VisitLambda(e)                 },
        // unary
            { ElementNames.ArrayLength,          (v, e) => v.VisitUnary(e)                  },
            { ElementNames.Convert,              (v, e) => v.VisitUnary(e)                  },
            { ElementNames.ConvertChecked,       (v, e) => v.VisitUnary(e)                  },
            { ElementNames.Negate,               (v, e) => v.VisitUnary(e)                  },
            { ElementNames.NegateChecked,        (v, e) => v.VisitUnary(e)                  },
            { ElementNames.Not,                  (v, e) => v.VisitUnary(e)                  },
            { ElementNames.OnesComplement,       (v, e) => v.VisitUnary(e)                  },
            { ElementNames.Quote,                (v, e) => v.VisitUnary(e)                  },
            { ElementNames.TypeAs,               (v, e) => v.VisitUnary(e)                  },
            { ElementNames.UnaryPlus,            (v, e) => v.VisitUnary(e)                  },
        // change by one
            { ElementNames.Decrement,            (v, e) => v.VisitUnary(e)                  },
            { ElementNames.Increment,            (v, e) => v.VisitUnary(e)                  },
            { ElementNames.PostDecrementAssign,  (v, e) => v.VisitUnary(e)                  },
            { ElementNames.PostIncrementAssign,  (v, e) => v.VisitUnary(e)                  },
            { ElementNames.PreDecrementAssign,   (v, e) => v.VisitUnary(e)                  },
            { ElementNames.PreIncrementAssign,   (v, e) => v.VisitUnary(e)                  },
        // binary
            { ElementNames.Add,                  (v, e) => v.VisitBinary(e)                 },
            { ElementNames.AddChecked,           (v, e) => v.VisitBinary(e)                 },
            { ElementNames.And,                  (v, e) => v.VisitBinary(e)                 },
            { ElementNames.AndAlso,              (v, e) => v.VisitBinary(e)                 },
            { ElementNames.Coalesce,             (v, e) => v.VisitBinary(e)                 },
            { ElementNames.Divide,               (v, e) => v.VisitBinary(e)                 },
            { ElementNames.Equal,                (v, e) => v.VisitBinary(e)                 },
            { ElementNames.ExclusiveOr,          (v, e) => v.VisitBinary(e)                 },
            { ElementNames.GreaterThan,          (v, e) => v.VisitBinary(e)                 },
            { ElementNames.GreaterThanOrEqual,   (v, e) => v.VisitBinary(e)                 },
            { ElementNames.LeftShift,            (v, e) => v.VisitBinary(e)                 },
            { ElementNames.LessThan,             (v, e) => v.VisitBinary(e)                 },
            { ElementNames.LessThanOrEqual,      (v, e) => v.VisitBinary(e)                 },
            { ElementNames.Modulo,               (v, e) => v.VisitBinary(e)                 },
            { ElementNames.Multiply,             (v, e) => v.VisitBinary(e)                 },
            { ElementNames.MultiplyChecked,      (v, e) => v.VisitBinary(e)                 },
            { ElementNames.NotEqual,             (v, e) => v.VisitBinary(e)                 },
            { ElementNames.Or,                   (v, e) => v.VisitBinary(e)                 },
            { ElementNames.OrElse,               (v, e) => v.VisitBinary(e)                 },
            { ElementNames.Power,                (v, e) => v.VisitBinary(e)                 },
            { ElementNames.RightShift,           (v, e) => v.VisitBinary(e)                 },
            { ElementNames.Subtract,             (v, e) => v.VisitBinary(e)                 },
            { ElementNames.SubtractChecked,      (v, e) => v.VisitBinary(e)                 },
            { ElementNames.ArrayIndex,           (v, e) => v.VisitBinary(e)                 },
        // assignments
            { ElementNames.AddAssign,            (v, e) => v.VisitBinary(e)                 },
            { ElementNames.AddAssignChecked,     (v, e) => v.VisitBinary(e)                 },
            { ElementNames.AndAssign,            (v, e) => v.VisitBinary(e)                 },
            { ElementNames.Assign,               (v, e) => v.VisitBinary(e)                 },
            { ElementNames.DivideAssign,         (v, e) => v.VisitBinary(e)                 },
            { ElementNames.LeftShiftAssign,      (v, e) => v.VisitBinary(e)                 },
            { ElementNames.ModuloAssign,         (v, e) => v.VisitBinary(e)                 },
            { ElementNames.MultiplyAssign,       (v, e) => v.VisitBinary(e)                 },
            { ElementNames.MultiplyAssignChecked,(v, e) => v.VisitBinary(e)                 },
            { ElementNames.OrAssign,             (v, e) => v.VisitBinary(e)                 },
            { ElementNames.PowerAssign,          (v, e) => v.VisitBinary(e)                 },
            { ElementNames.RightShiftAssign,     (v, e) => v.VisitBinary(e)                 },
            { ElementNames.SubtractAssign,       (v, e) => v.VisitBinary(e)                 },
            { ElementNames.SubtractAssignChecked,(v, e) => v.VisitBinary(e)                 },
            { ElementNames.ExclusiveOrAssign,    (v, e) => v.VisitBinary(e)                 },

            { ElementNames.TypeIs,               (v, e) => v.VisitTypeBinary(e)             },
            { ElementNames.Block,                (v, e) => v.VisitBlock(e)                  },
            { ElementNames.Conditional,          (v, e) => v.VisitConditional(e)            },
            { ElementNames.Index,                (v, e) => v.VisitIndex(e)                  },
            { ElementNames.New,                  (v, e) => v.VisitNew(e)                    },
            { ElementNames.Throw,                (v, e) => v.VisitThrow(e)                  },
            { ElementNames.Default,              (v, e) => v.VisitDefault(e)                },
            { ElementNames.MemberAccess,         (v, e) => v.VisitMember(e)                 },
            { ElementNames.Call,                 (v, e) => v.VisitMethodCall(e)             },
            { ElementNames.Exception,            (v, e) => v.VisitParameter(e)              },
            { ElementNames.Label,                (v, e) => v.VisitLabel(e)                  },
            { ElementNames.Goto,                 (v, e) => v.VisitGoto(e)                   },
            { ElementNames.Loop,                 (v, e) => v.VisitLoop(e)                   },
            { ElementNames.Switch,               (v, e) => v.VisitSwitch(e)                 },
            { ElementNames.Try,                  (v, e) => v.VisitTry(e)                    },
            { ElementNames.Filter,               (v, e) => v.VisitExpressionContainer(e)    },
            { ElementNames.Fault,                (v, e) => v.VisitExpressionContainer(e)    },
            { ElementNames.Finally,              (v, e) => v.VisitExpressionContainer(e)    },
            { ElementNames.ListInit,             (v, e) => v.VisitListInit(e)               },
            { ElementNames.NewArrayInit,         (v, e) => v.VisitNewArrayInit(e)           },
            { ElementNames.NewArrayBounds,       (v, e) => v.VisitNewArrayBounds(e)         },
            { ElementNames.MemberInit,           (v, e) => v.VisitMemberInit(e)             },
        };
    #endregion

    /// <summary>
    /// This is the starting point of the visitor.
    /// </summary>
    /// <param name="element">The element to be visited.</param>
    /// <returns>The created expression.</returns>
    public Expression Visit(XElement element) => _transforms[element.Name](this, element);

    internal static ExpressionType GetExpressionType(XElement element)
        => element != null
            ? (ExpressionType)Enum.Parse(
                                typeof(ExpressionType),
                                element.Name.LocalName,
                                true)
            : throw new ArgumentNullException(nameof(element));

    #region Concrete XML element visitors
    Expression VisitExpression(XElement e) => Visit(e.FirstChild());

    static Expression VisitConstant(XElement e)
        => FromXmlDataTransform.ConstantTransform(e.FirstChild());

    Expression VisitParameter(XElement e) => throw new NotImplementedException();

    Expression VisitLambda(XElement e) => throw new NotImplementedException();
    Expression VisitUnary(XElement e) => throw new NotImplementedException();
    Expression VisitBinary(XElement e) => throw new NotImplementedException();
    Expression VisitTypeBinary(XElement e) => throw new NotImplementedException();
    Expression VisitBlock(XElement e) => throw new NotImplementedException();
    Expression VisitConditional(XElement e) => throw new NotImplementedException();
    Expression VisitIndex(XElement e) => throw new NotImplementedException();
    Expression VisitNew(XElement e) => throw new NotImplementedException();
    Expression VisitThrow(XElement e) => throw new NotImplementedException();
    Expression VisitDefault(XElement e) => throw new NotImplementedException();
    Expression VisitMember(XElement e) => throw new NotImplementedException();
    Expression VisitMethodCall(XElement e) => throw new NotImplementedException();
    Expression VisitLabel(XElement e) => throw new NotImplementedException();
    Expression VisitGoto(XElement e) => throw new NotImplementedException();
    Expression VisitLoop(XElement e) => throw new NotImplementedException();
    Expression VisitSwitch(XElement e) => throw new NotImplementedException();
    Expression VisitTry(XElement e) => throw new NotImplementedException();
    Expression VisitExpressionContainer(XElement e) => throw new NotImplementedException();
    Expression VisitListInit(XElement e) => throw new NotImplementedException();
    Expression VisitNewArrayInit(XElement e) => throw new NotImplementedException();
    Expression VisitNewArrayBounds(XElement e) => throw new NotImplementedException();
    Expression VisitMemberInit(XElement e) => throw new NotImplementedException();
    #endregion
}
