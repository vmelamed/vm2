namespace vm2.ExpressionSerialization.Conventions;

static partial class Transform
{
    /// <summary>
    /// Gets the type corresponding to a type name written in an xml string.
    /// </summary>
    /// <param name="typeName">The name of the type.</param>
    /// <returns>The specified type.</returns>
    public static Type? GetType(string typeName)
    {
        if (string.IsNullOrWhiteSpace(typeName))
            return null;

        if (Vocabulary.NamesToTypes.TryGetValue(typeName, out var type))
            return type;

        return Type.GetType(typeName, true, false);
    }

    /// <summary>
    /// Transformer the name of the type <paramref name="type"/> to a possibly ugly string.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>System.String.</returns>
    public static string TypeName(Type type)
    {
        if (Vocabulary.TypesToNames.TryGetValue(type, out var typeName))
            return typeName;

        return type.AssemblyQualifiedName ?? type.FullName ?? type.Name;
    }

    /// <summary>
    /// Transforms the name of the type <paramref name="type"/> to a human readable string according to 
    /// <paramref name="convention"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="convention">The convention.</param>
    /// <returns>System.String.</returns>
    public static string TypeName(
        Type type,
        TypeNameConventions convention)
    {
        if (Vocabulary.TypesToNames.TryGetValue(type, out var typeName))
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
