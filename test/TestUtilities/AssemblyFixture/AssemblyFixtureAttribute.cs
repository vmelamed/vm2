#pragma warning disable CA1050 // Declare types in namespaces

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class AssemblyFixtureAttribute(Type fixtureType) : Attribute
{
    public Type FixtureType { get; private set; } = fixtureType;
}