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
            { Transform.NConstant,             (v, e) => v.VisitConstant(e)               },
            { Transform.NParameterSpec,        (v, e) => v.VisitParameter(e)              },
            { Transform.NParameterDefinition,  (v, e) => v.VisitParameter(e)              },
            { Transform.NParameterReference,   (v, e) => v.VisitParameter(e)              },
            { Transform.NVariableDefinition,   (v, e) => v.VisitParameter(e)              },
            { Transform.NVariableReference,    (v, e) => v.VisitParameter(e)              },
            { Transform.NLambda,               (v, e) => v.VisitLambda(e)                 },
        // unary
            { Transform.NArrayLength,          (v, e) => v.VisitUnary(e)                  },
            { Transform.NConvert,              (v, e) => v.VisitUnary(e)                  },
            { Transform.NConvertChecked,       (v, e) => v.VisitUnary(e)                  },
            { Transform.NNegate,               (v, e) => v.VisitUnary(e)                  },
            { Transform.NNegateChecked,        (v, e) => v.VisitUnary(e)                  },
            { Transform.NNot,                  (v, e) => v.VisitUnary(e)                  },
            { Transform.NOnesComplement,       (v, e) => v.VisitUnary(e)                  },
            { Transform.NQuote,                (v, e) => v.VisitUnary(e)                  },
            { Transform.NTypeAs,               (v, e) => v.VisitUnary(e)                  },
            { Transform.NUnaryPlus,            (v, e) => v.VisitUnary(e)                  },
        // change by one
            { Transform.NDecrement,            (v, e) => v.VisitUnary(e)                  },
            { Transform.NIncrement,            (v, e) => v.VisitUnary(e)                  },
            { Transform.NPostDecrementAssign,  (v, e) => v.VisitUnary(e)                  },
            { Transform.NPostIncrementAssign,  (v, e) => v.VisitUnary(e)                  },
            { Transform.NPreDecrementAssign,   (v, e) => v.VisitUnary(e)                  },
            { Transform.NPreIncrementAssign,   (v, e) => v.VisitUnary(e)                  },
        // binary
            { Transform.NAdd,                  (v, e) => v.VisitBinary(e)                 },
            { Transform.NAddChecked,           (v, e) => v.VisitBinary(e)                 },
            { Transform.NAnd,                  (v, e) => v.VisitBinary(e)                 },
            { Transform.NAndAlso,              (v, e) => v.VisitBinary(e)                 },
            { Transform.NCoalesce,             (v, e) => v.VisitBinary(e)                 },
            { Transform.NDivide,               (v, e) => v.VisitBinary(e)                 },
            { Transform.NEqual,                (v, e) => v.VisitBinary(e)                 },
            { Transform.NExclusiveOr,          (v, e) => v.VisitBinary(e)                 },
            { Transform.NGreaterThan,          (v, e) => v.VisitBinary(e)                 },
            { Transform.NGreaterThanOrEqual,   (v, e) => v.VisitBinary(e)                 },
            { Transform.NLeftShift,            (v, e) => v.VisitBinary(e)                 },
            { Transform.NLessThan,             (v, e) => v.VisitBinary(e)                 },
            { Transform.NLessThanOrEqual,      (v, e) => v.VisitBinary(e)                 },
            { Transform.NModulo,               (v, e) => v.VisitBinary(e)                 },
            { Transform.NMultiply,             (v, e) => v.VisitBinary(e)                 },
            { Transform.NMultiplyChecked,      (v, e) => v.VisitBinary(e)                 },
            { Transform.NNotEqual,             (v, e) => v.VisitBinary(e)                 },
            { Transform.NOr,                   (v, e) => v.VisitBinary(e)                 },
            { Transform.NOrElse,               (v, e) => v.VisitBinary(e)                 },
            { Transform.NPower,                (v, e) => v.VisitBinary(e)                 },
            { Transform.NRightShift,           (v, e) => v.VisitBinary(e)                 },
            { Transform.NSubtract,             (v, e) => v.VisitBinary(e)                 },
            { Transform.NSubtractChecked,      (v, e) => v.VisitBinary(e)                 },
            { Transform.NArrayIndex,           (v, e) => v.VisitBinary(e)                 },
        // assignments
            { Transform.NAddAssign,            (v, e) => v.VisitBinary(e)                 },
            { Transform.NAddAssignChecked,     (v, e) => v.VisitBinary(e)                 },
            { Transform.NAndAssign,            (v, e) => v.VisitBinary(e)                 },
            { Transform.NAssign,               (v, e) => v.VisitBinary(e)                 },
            { Transform.NDivideAssign,         (v, e) => v.VisitBinary(e)                 },
            { Transform.NLeftShiftAssign,      (v, e) => v.VisitBinary(e)                 },
            { Transform.NModuloAssign,         (v, e) => v.VisitBinary(e)                 },
            { Transform.NMultiplyAssign,       (v, e) => v.VisitBinary(e)                 },
            { Transform.NMultiplyAssignChecked,(v, e) => v.VisitBinary(e)                 },
            { Transform.NOrAssign,             (v, e) => v.VisitBinary(e)                 },
            { Transform.NPowerAssign,          (v, e) => v.VisitBinary(e)                 },
            { Transform.NRightShiftAssign,     (v, e) => v.VisitBinary(e)                 },
            { Transform.NSubtractAssign,       (v, e) => v.VisitBinary(e)                 },
            { Transform.NSubtractAssignChecked,(v, e) => v.VisitBinary(e)                 },
            { Transform.NExclusiveOrAssign,    (v, e) => v.VisitBinary(e)                 },

            { Transform.NTypeIs,               (v, e) => v.VisitTypeBinary(e)             },
            { Transform.NBlock,                (v, e) => v.VisitBlock(e)                  },
            { Transform.NConditional,          (v, e) => v.VisitConditional(e)            },
            { Transform.NIndex,                (v, e) => v.VisitIndex(e)                  },
            { Transform.NNew,                  (v, e) => v.VisitNew(e)                    },
            { Transform.NThrow,                (v, e) => v.VisitThrow(e)                  },
            { Transform.NDefault,              (v, e) => v.VisitDefault(e)                },
            { Transform.NMemberAccess,         (v, e) => v.VisitMember(e)                 },
            { Transform.NCall,                 (v, e) => v.VisitMethodCall(e)             },
            { Transform.NInvoke,               (v, e) => v.VisitInvocation(e)             },
            { Transform.NException,            (v, e) => v.VisitParameter(e)              },
            { Transform.NLabel,                (v, e) => v.VisitLabel(e)                  },
            { Transform.NGoto,                 (v, e) => v.VisitGoto(e)                   },
            { Transform.NLoop,                 (v, e) => v.VisitLoop(e)                   },
            { Transform.NSwitch,               (v, e) => v.VisitSwitch(e)                 },
            { Transform.NTry,                  (v, e) => v.VisitTry(e)                    },
            { Transform.NMemberInit,           (v, e) => v.VisitMemberInit(e)             },
            { Transform.NListInit,             (v, e) => v.VisitListInit(e)               },
            { Transform.NNewArrayInit,         (v, e) => v.VisitNewArrayInit(e)           },
            { Transform.NNewArrayBounds,       (v, e) => v.VisitNewArrayBounds(e)         },
        };
    static readonly FrozenDictionary<string, Func<FromXmlTransformVisitor, XElement, Expression>> _transforms = _transforms_.ToFrozenDictionary();
    #endregion
}
