namespace vm2.TestUtilities;

/// <summary>
/// Apply this attribute to your test method to replace the <see cref="Thread.CurrentPrincipal"/> with another role.
/// </summary>
/// <param name="roleName">The role name</param>
/// <param name="userName">The user name (defaults to "xUnit")</param>
/// <remarks>
/// https://github.com/xunit/samples.xunit/blob/main/v3/AssumeIdentityExample/AssumeIdentityAttribute.cs
/// </remarks>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class AssumeIdentityAttribute(string roleName, string userName = "xUnit") : BeforeAfterTestAttribute
{
    IPrincipal? _originalPrincipal;

    public string RoleName { get; } = roleName;

    public string UserName { get; } = userName;

    public override void After(MethodInfo methodUnderTest, IXunitTest test)
    {
        if (_originalPrincipal is not null)
            Thread.CurrentPrincipal = _originalPrincipal;
    }

    public override void Before(MethodInfo methodUnderTest, IXunitTest test)
    {
        _originalPrincipal = Thread.CurrentPrincipal;
        var identity = new GenericIdentity(UserName);
        var principal = new GenericPrincipal(identity, [RoleName]);
        Thread.CurrentPrincipal = principal;
    }
}