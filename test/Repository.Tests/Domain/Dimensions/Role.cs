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

    public static Expression<Func<Role, string>> ValueExpression => role => role.Name;

    /// <summary>
    /// Determines whether all specified values are present in the roles' set of values (are in the cache) in other words,
    /// determines if all specified values are known roles.
    /// </summary>
    /// <returns><see langword="true"/> if the collection contains at least one valid value; otherwise, <see langword="false"/>.</returns>
    /// <returns></returns>
    public static bool Has(params string[] values) => IDimensionValidator<Role, string>.Has(values);

    /// <summary>
    /// Determines whether all specified values are present in the roles' set of values (are in the cache) in other words,
    /// determines if all specified values are known roles.
    /// </summary>
    /// <param name="values">The collection of strings to validate. Cannot be null.</param>
    /// <returns>
    /// <see langword="true"/> if all strings in the collection are present in the cache (are known instruments); otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Has(IEnumerable<string> values) => IDimensionValidator<Role, string>.Has(values);
}
