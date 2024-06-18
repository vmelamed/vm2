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
    static readonly Dictionary<string, Func<FromXmlTransformVisitor, XElement, Expression>> _transforms_ = new()
    {
            { Vocabulary.Expression,           (v, e) => v.VisitChild(e, 0)       },
            { Vocabulary.Constant,             (v, e) => v.VisitConstant(e)       },
            { Vocabulary.ParameterSpec,        (v, e) => v.VisitParameter(e)      },
            { Vocabulary.Parameter,            (v, e) => v.VisitParameter(e)      },
            { Vocabulary.Lambda,               (v, e) => v.VisitLambda(e)         },
        // unary
            { Vocabulary.ArrayLength,          (v, e) => v.VisitUnary(e)          },
            { Vocabulary.Convert,              (v, e) => v.VisitUnary(e)          },
            { Vocabulary.ConvertChecked,       (v, e) => v.VisitUnary(e)          },
            { Vocabulary.Negate,               (v, e) => v.VisitUnary(e)          },
            { Vocabulary.NegateChecked,        (v, e) => v.VisitUnary(e)          },
            { Vocabulary.Not,                  (v, e) => v.VisitUnary(e)          },
            { Vocabulary.OnesComplement,       (v, e) => v.VisitUnary(e)          },
            { Vocabulary.Quote,                (v, e) => v.VisitUnary(e)          },
            { Vocabulary.TypeAs,               (v, e) => v.VisitUnary(e)          },
            { Vocabulary.UnaryPlus,            (v, e) => v.VisitUnary(e)          },
        // change by one
            { Vocabulary.Decrement,            (v, e) => v.VisitUnary(e)          },
            { Vocabulary.Increment,            (v, e) => v.VisitUnary(e)          },
            { Vocabulary.PostDecrementAssign,  (v, e) => v.VisitUnary(e)          },
            { Vocabulary.PostIncrementAssign,  (v, e) => v.VisitUnary(e)          },
            { Vocabulary.PreDecrementAssign,   (v, e) => v.VisitUnary(e)          },
            { Vocabulary.PreIncrementAssign,   (v, e) => v.VisitUnary(e)          },
        // binary
            { Vocabulary.Add,                  (v, e) => v.VisitBinary(e)         },
            { Vocabulary.AddChecked,           (v, e) => v.VisitBinary(e)         },
            { Vocabulary.And,                  (v, e) => v.VisitBinary(e)         },
            { Vocabulary.AndAlso,              (v, e) => v.VisitBinary(e)         },
            { Vocabulary.Coalesce,             (v, e) => v.VisitBinary(e)         },
            { Vocabulary.Divide,               (v, e) => v.VisitBinary(e)         },
            { Vocabulary.Equal,                (v, e) => v.VisitBinary(e)         },
            { Vocabulary.ExclusiveOr,          (v, e) => v.VisitBinary(e)         },
            { Vocabulary.GreaterThan,          (v, e) => v.VisitBinary(e)         },
            { Vocabulary.GreaterThanOrEqual,   (v, e) => v.VisitBinary(e)         },
            { Vocabulary.LeftShift,            (v, e) => v.VisitBinary(e)         },
            { Vocabulary.LessThan,             (v, e) => v.VisitBinary(e)         },
            { Vocabulary.LessThanOrEqual,      (v, e) => v.VisitBinary(e)         },
            { Vocabulary.Modulo,               (v, e) => v.VisitBinary(e)         },
            { Vocabulary.Multiply,             (v, e) => v.VisitBinary(e)         },
            { Vocabulary.MultiplyChecked,      (v, e) => v.VisitBinary(e)         },
            { Vocabulary.NotEqual,             (v, e) => v.VisitBinary(e)         },
            { Vocabulary.Or,                   (v, e) => v.VisitBinary(e)         },
            { Vocabulary.OrElse,               (v, e) => v.VisitBinary(e)         },
            { Vocabulary.Power,                (v, e) => v.VisitBinary(e)         },
            { Vocabulary.RightShift,           (v, e) => v.VisitBinary(e)         },
            { Vocabulary.Subtract,             (v, e) => v.VisitBinary(e)         },
            { Vocabulary.SubtractChecked,      (v, e) => v.VisitBinary(e)         },
            { Vocabulary.ArrayIndex,           (v, e) => v.VisitBinary(e)         },
        // assignments
            { Vocabulary.AddAssign,            (v, e) => v.VisitBinary(e)         },
            { Vocabulary.AddAssignChecked,     (v, e) => v.VisitBinary(e)         },
            { Vocabulary.AndAssign,            (v, e) => v.VisitBinary(e)         },
            { Vocabulary.Assign,               (v, e) => v.VisitBinary(e)         },
            { Vocabulary.DivideAssign,         (v, e) => v.VisitBinary(e)         },
            { Vocabulary.LeftShiftAssign,      (v, e) => v.VisitBinary(e)         },
            { Vocabulary.ModuloAssign,         (v, e) => v.VisitBinary(e)         },
            { Vocabulary.MultiplyAssign,       (v, e) => v.VisitBinary(e)         },
            { Vocabulary.MultiplyAssignChecked,(v, e) => v.VisitBinary(e)         },
            { Vocabulary.OrAssign,             (v, e) => v.VisitBinary(e)         },
            { Vocabulary.PowerAssign,          (v, e) => v.VisitBinary(e)         },
            { Vocabulary.RightShiftAssign,     (v, e) => v.VisitBinary(e)         },
            { Vocabulary.SubtractAssign,       (v, e) => v.VisitBinary(e)         },
            { Vocabulary.SubtractAssignChecked,(v, e) => v.VisitBinary(e)         },
            { Vocabulary.ExclusiveOrAssign,    (v, e) => v.VisitBinary(e)         },

            { Vocabulary.TypeEqual,            (v, e) => v.VisitTypeBinary(e)     },
            { Vocabulary.TypeIs,               (v, e) => v.VisitTypeBinary(e)     },
            { Vocabulary.Block,                (v, e) => v.VisitBlock(e)          },
            { Vocabulary.Conditional,          (v, e) => v.VisitConditional(e)    },
            { Vocabulary.Index,                (v, e) => v.VisitIndex(e)          },
            { Vocabulary.New,                  (v, e) => v.VisitNew(e)            },
            { Vocabulary.Throw,                (v, e) => v.VisitThrow(e)          },
            { Vocabulary.Default,              (v, e) => v.VisitDefault(e)        },
            { Vocabulary.MemberAccess,         (v, e) => v.VisitMember(e)         },
            { Vocabulary.Call,                 (v, e) => v.VisitMethodCall(e)     },
            { Vocabulary.Invoke,               (v, e) => v.VisitInvocation(e)     },
            { Vocabulary.Exception,            (v, e) => v.VisitParameter(e)      },
            { Vocabulary.Label,                (v, e) => v.VisitLabel(e)          },
            { Vocabulary.Goto,                 (v, e) => v.VisitGoto(e)           },
            { Vocabulary.Loop,                 (v, e) => v.VisitLoop(e)           },
            { Vocabulary.Switch,               (v, e) => v.VisitSwitch(e)         },
            { Vocabulary.Try,                  (v, e) => v.VisitTry(e)            },
            { Vocabulary.MemberInit,           (v, e) => v.VisitMemberInit(e)     },
            { Vocabulary.ListInit,             (v, e) => v.VisitListInit(e)       },
            { Vocabulary.NewArrayInit,         (v, e) => v.VisitNewArrayInit(e)   },
            { Vocabulary.NewArrayBounds,       (v, e) => v.VisitNewArrayBounds(e) },
        };
    static readonly FrozenDictionary<string, Func<FromXmlTransformVisitor, XElement, Expression>> _transforms = _transforms_.ToFrozenDictionary();
    #endregion
}
