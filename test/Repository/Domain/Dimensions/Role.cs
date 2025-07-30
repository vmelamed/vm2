namespace vm2.Repository.Domain.Dimensions;

/// <summary>
/// Represents a role of a person in their carrier, on an album, or on a track.
/// </summary>
/// <param name="Name">The name of the role. This value cannot be null or empty and must be unique.</param>
[DebuggerDisplay("Role: {Name}")]
public record Role(string Name) : IValidatable
{
    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new RoleValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken);
    #endregion
}
