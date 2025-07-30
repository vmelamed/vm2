namespace vm2.Repository.Domain.Dimensions;

/// <summary>
/// Represents a category or type of music.
/// </summary>
/// <param name="Name">The name of the genre. Cannot be empty and must be unique.</param>
public record Genre(string Name) : IValidatable
{
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new GenreValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken);
}
