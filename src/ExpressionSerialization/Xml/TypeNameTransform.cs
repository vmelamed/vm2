namespace vm2.ExpressionSerialization.Xml;

static class TypeNameTransform
{
    #region Maps of types and type names
    static readonly ReaderWriterLockSlim _typesToNamesLock = new(LockRecursionPolicy.SupportsRecursion);
    /// <summary>
    /// The map of base type to type names
    /// </summary>
    static Dictionary<Type, string> _typesToNames = new()
        {
            { typeof(void),         "void"          },
            { typeof(char),         "char"          },
            { typeof(bool),         "boolean"       },
            { typeof(byte),         "unsignedByte"  },
            { typeof(sbyte),        "byte"          },
            { typeof(short),        "short"         },
            { typeof(ushort),       "unsignedShort" },
            { typeof(int),          "int"           },
            { typeof(uint),         "unsignedInt"   },
            { typeof(long),         "long"          },
            { typeof(ulong),        "unsignedLong"  },
            { typeof(Half),         "half"          },
            { typeof(float),        "float"         },
            { typeof(double),       "double"        },
            { typeof(decimal),      "decimal"       },
            { typeof(Guid),         "guid"          },
            { typeof(Uri),          "anyURI"        },
            { typeof(string),       "string"        },
            { typeof(TimeSpan),     "duration"      },
            { typeof(DateTime),     "dateTime"      },
            { typeof(DBNull),       "dbNull"        },
            { typeof(Nullable<>),   "nullable"      },
            { typeof(object),       "custom"        },
            { typeof(Enum),         "enum"          },
        };

    /// <summary>
    /// The map of type names to base type
    /// </summary>
    static Dictionary<string, Type> _namesToTypes = new()
        {
            { "void",           typeof(void)        },
            { "char",           typeof(char)        },
            { "boolean",        typeof(bool)        },
            { "unsignedByte",   typeof(byte)        },
            { "byte",           typeof(sbyte)       },
            { "short",          typeof(short)       },
            { "unsignedShort",  typeof(ushort)      },
            { "int",            typeof(int)         },
            { "unsignedInt",    typeof(uint)        },
            { "long",           typeof(long)        },
            { "unsignedLong",   typeof(ulong)       },
            { "half",           typeof(Half)        },
            { "float",          typeof(float)       },
            { "double",         typeof(double)      },
            { "decimal",        typeof(decimal)     },
            { "guid",           typeof(Guid)        },
            { "anyURI",         typeof(Uri)         },
            { "string",         typeof(string)      },
            { "duration",       typeof(TimeSpan)    },
            { "dateTime",       typeof(DateTime)    },
            { "dbNull",         typeof(DBNull)      },
            { "nullable",       typeof(Nullable<>)  },
            { "custom",         typeof(object)      },
            { "enum",           typeof(Enum)        },
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

        Type? type = null;

        using (_typesToNamesLock.UpgradableReaderLock())
        {
            if (!_namesToTypes.TryGetValue(typeName, out type))
            {
                type = Type.GetType(typeName);

                if (type is not null)
                    using (_typesToNamesLock.WriterLock())
                    {
                        Debug.Assert(!_typesToNames.ContainsKey(type));
                        _namesToTypes[typeName] = type;
                        _typesToNames[type] = typeName;
                    }
            }
        }

        return type;
    }

    /// <summary>
    /// Gets the name of the type appropriate for writing to an XML element.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The name of the type.</returns>
    public static string GetTypeName(Type type)
    {
        if (type == null)
            return "";

        using (_typesToNamesLock.UpgradableReaderLock())
        {
            if (!_typesToNames.TryGetValue(type, out var typeName))
            {
                typeName = type.AssemblyQualifiedName ?? "";

                if (!string.IsNullOrWhiteSpace(typeName))
                    using (_typesToNamesLock.WriterLock())
                    {
                        Debug.Assert(!_namesToTypes.ContainsKey(typeName));
                        _typesToNames[type] = typeName;
                        _namesToTypes[typeName] = type;
                    }
            }
            return typeName!;
        }
    }
}
