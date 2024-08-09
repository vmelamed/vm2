namespace vm2.ExpressionSerialization.Shared.Conventions;
/// <summary>
/// Enum TypeNameConventions specify how to transform the type names.
/// Note: no casing transformation.
/// </summary>
public enum TypeNameConventions
{
    /// <summary>
    /// The full name of the type, c.e. `namespace.name`, e.g. "vm.Aspects.Linq.Expressions.Serialization.Tests.EnumTest"
    /// </summary>
    FullName,
    /// <summary>
    /// The name of the type, e.g. "EnumTest"
    /// </summary>
    Name,
    /// <summary>
    /// The assembly qualified name of the type, e.g. "vm.Aspects.Linq.Expressions.Serialization.Tests.EnumTest, vm.Aspects.Linq.Expressions.Serialization.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1fb2eb0544466393"
    /// </summary>
    AssemblyQualifiedName,
}
