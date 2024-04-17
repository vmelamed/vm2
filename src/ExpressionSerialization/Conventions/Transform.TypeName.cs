namespace vm2.ExpressionSerialization.Conventions;
static partial class Transform
{
    #region Maps of types and type names
    /// <summary>
    /// The map of base type to type names
    /// </summary>
    static Dictionary<Type, string> _typesToNames = new()
        {
            { typeof(void),         "void"              },
            { typeof(char),         "char"              },
            { typeof(bool),         "boolean"           },
            { typeof(byte),         "unsignedByte"      },
            { typeof(sbyte),        "byte"              },
            { typeof(short),        "short"             },
            { typeof(ushort),       "unsignedShort"     },
            { typeof(int),          "int"               },
            { typeof(uint),         "unsignedInt"       },
            { typeof(IntPtr),       "intPtr"            },
            { typeof(UIntPtr),      "unsignedIntPtr"    },
            { typeof(long),         "long"              },
            { typeof(ulong),        "unsignedLong"      },
            { typeof(Half),         "half"              },
            { typeof(float),        "float"             },
            { typeof(double),       "double"            },
            { typeof(decimal),      "decimal"           },
            { typeof(Guid),         "guid"              },
            { typeof(Uri),          "uri"               },
            { typeof(string),       "string"            },
            { typeof(TimeSpan),     "duration"          },
            { typeof(DateTime),     "dateTime"          },
            { typeof(DBNull),       "dbNull"            },
            { typeof(Nullable<>),   "nullable"          },
            { typeof(object),       "custom"            },
            { typeof(Enum),         "enum"              },
        };

    /// <summary>
    /// The map of type names to base type
    /// </summary>
    static Dictionary<string, Type> _namesToTypes = new ()
        {
            { "void",               typeof(void)
},
            { "char",               typeof(char)        },
            { "boolean",            typeof(bool)        },
            { "unsignedByte",       typeof(byte)        },
            { "byte",               typeof(sbyte)       },
            { "short",              typeof(short)       },
            { "unsignedShort",      typeof(ushort)      },
            { "int",                typeof(int)         },
            { "unsignedInt",        typeof(uint)        },
            { "intPtr",             typeof(IntPtr)      },
            { "unsignedIntPtr",     typeof(UIntPtr)     },
            { "long",               typeof(long)        },
            { "unsignedLong",       typeof(ulong)       },
            { "half",               typeof(Half)        },
            { "float",              typeof(float)       },
            { "double",             typeof(double)      },
            { "decimal",            typeof(decimal)     },
            { "guid",               typeof(Guid)        },
            { "uri",                typeof(Uri)         },
            { "string",             typeof(string)      },
            { "duration",           typeof(TimeSpan)    },
            { "dateTime",           typeof(DateTime)    },
            { "dbNull",             typeof(DBNull)      },
            { "nullable",           typeof(Nullable<>)  },
            { "custom",             typeof(object)      },
            { "enum",               typeof(Enum)        },
        };
    #endregion

    /// <summary>
    /// Gets the type corresponding to a type name written in an xml string.
    /// </summary>
    /// <param name="typeName">The name of the type.</param>
    /// <returns>The specified type.</returns>
    public static Type? GetType(string typeName)
    {
        if (string.IsNullOrWhiteSpace(typeName))
            return null;

        if (_namesToTypes.TryGetValue(typeName, out var type))
            return type;

        return Type.GetType(typeName, true, false);
    }

    /// <summary>
    /// Transform the name of the type <paramref name="type"/> to a possibly ugly string.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>System.String.</returns>
    public static string TypeName(Type type)
    {
        if (_typesToNames.TryGetValue(type, out var typeName))
            return typeName;

        return type.AssemblyQualifiedName ?? type.FullName ?? type.Name;
    }

    /// <summary>
    /// Transform the name of the type <paramref name="type"/> to a human readable string according to 
    /// <paramref name="convention"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="convention">The convention.</param>
    /// <returns>System.String.</returns>
    public static string TypeName(Type type, TypeNameConventions convention)
    {
        if (_typesToNames.TryGetValue(type, out var typeName))
            return typeName;

        if (type.IsGenericType && !type.IsGenericTypeDefinition && convention != TypeNameConventions.AssemblyQualifiedName)
        {
            var genericName = TypeName(type.GetGenericTypeDefinition(), convention).Split('`')[0];
            var parameters  = string.Join(", ", type.GetGenericArguments().Select(t => TypeName(t, convention)));

            return $"{genericName}<{parameters}>";
        }

        return convention switch {
            TypeNameConventions.AssemblyQualifiedName => type.AssemblyQualifiedName ?? type.FullName ?? type.Name,
            TypeNameConventions.FullName => type.FullName ?? type.Name,
            TypeNameConventions.Name => type.Name,
            _ => throw new InternalTransformErrorException("Invalid TypeNameConventions value.")
        };
    }
}
