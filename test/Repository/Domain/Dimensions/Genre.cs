namespace vm2.Repository.Domain.Dimensions;

/// <summary>
/// Represents a category or type of music.
/// </summary>
/// <param name="Name">The name of the genre. This value cannot be null or empty and must be unique.</param>
[DebuggerDisplay("Genre: {Name}")]
public record Genre(string Name) : IValidatable
{
    public const int MaxLength = 50;

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new GenreValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken);
    #endregion
}
