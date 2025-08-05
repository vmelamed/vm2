namespace vm2.Repository.Tests.Domain.Dimensions;

/// <summary>
/// Represents a role of a person in their carrier, on an album, or on a track.
/// </summary>
/// <param name="Name">The name of the role. This value cannot be null or empty and must be unique.</param>
[DebuggerDisplay("Role: {Name}")]
public record Role(string Name) : IValidatable, IDimensionValidator<Role, string>
{
    public const int MaxNameLength = 50;

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new RoleValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken).ConfigureAwait(false);
    #endregion

    public static Expression<Func<Role, string>> ValueExpression
        => role => role.Name;

    public static bool HasValue(string value)
        => IDimensionValidator<Role, string>.Has(value);

    public static bool HasValues(IEnumerable<string> values)
        => IDimensionValidator<Role, string>.Has(values);
}
