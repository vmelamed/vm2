namespace vm2.XmlExpressionSerialization.Conventions;
static partial class Transform
{
    #region Maps of types and type names
    static readonly ReaderWriterLockSlim _typesNamesLock = new(LockRecursionPolicy.SupportsRecursion);

    /// <summary>
    /// The map of base type to type names
    /// </summary>
    static Dictionary<Type, string> _typesToNames;

    /// <summary>
    /// The map of type names to base type
    /// </summary>
    static Dictionary<string, Type> _namesToTypes;

    /// <summary>
    /// Resets the translation tables typesToNames and namesToTypes.
    /// </summary>
    /// <remarks>
    /// NOTE: this method is meant to be used only in unit tests, where the conventions change and the other methods 
    /// may throw <see cref="InternalTransformErrorException"/>.
    /// </remarks>
    internal static void ResetTypesNames()
    {
        using var _ = _typesNamesLock.WriterLock();
        _typesToNames = new()
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

        _namesToTypes = new()
        {
            { "void",               typeof(void)        },
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
    }
    #endregion

#pragma warning disable CS8618
    static Transform() => ResetTypesNames();
#pragma warning restore CS8618

    /// <summary>
    /// Gets the type corresponding to a type name written in an xml string.
    /// </summary>
    /// <param name="typeName">The name of the type.</param>
    /// <returns>The specified type.</returns>
    public static Type? GetType(string typeName)
    {
        if (string.IsNullOrWhiteSpace(typeName))
            return null;

        using (_typesNamesLock.UpgradableReaderLock())
        {
            if (_namesToTypes.TryGetValue(typeName, out var type))
                return type;

            type = Type.GetType(typeName, false, false);

            if (type is null)
                return null;

            using (_typesNamesLock.WriterLock())
            {
                if (_typesToNames.TryGetValue(type, out var nm))
                    throw new InternalTransformErrorException($"Cannot map name '{typeName}' to type '{type.AssemblyQualifiedName}'. The type is already mapped to '{nm}'. Did you serialize with different conventions for type names?");

                _namesToTypes[typeName] = type;
                _typesToNames[type] = typeName;
            }
            return type;
        }
    }

    /// <summary>
    /// Transform the name of the type <paramref name="type"/> to string according to <paramref name="convention"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="convention">The convention.</param>
    /// <returns>System.String.</returns>
    public static string TypeName(Type type, TypeNameConventions convention)
    {
        using (_typesNamesLock.UpgradableReaderLock())
        {
            if (_typesToNames.TryGetValue(type, out var typeName))
                return typeName;

            typeName = type.IsGenericType &&
                       !type.IsGenericTypeDefinition &&
                       convention != TypeNameConventions.AssemblyQualifiedName
                            ? $"{TypeName(type.GetGenericTypeDefinition(), convention).Split('`')[0]}<{string.Join(", ", type.GetGenericArguments().Select(t => TypeName(t, convention)))}>"
                            : convention switch {
                                TypeNameConventions.AssemblyQualifiedName => type.AssemblyQualifiedName ?? type.FullName ?? type.Name,
                                TypeNameConventions.FullName => type.FullName ?? type.Name,
                                TypeNameConventions.Name => type.Name,
                                _ => throw new InternalTransformErrorException("Invalid TypeNameConventions value.")
                            };

            using (_typesNamesLock.WriterLock())
            {
                if (_namesToTypes.TryGetValue(typeName, out var tp))
                    throw new InternalTransformErrorException($"Cannot map type '{type.AssemblyQualifiedName}' to name '{typeName}'. The name is already mapped to '{tp.AssemblyQualifiedName}'.");

                _namesToTypes[typeName] = type;
                _typesToNames[type] = typeName;
            }

            return typeName;
        }
    }
}
