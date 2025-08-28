namespace vm2.Repository.TestDomain.Dimensions;

using vm2.Repository.EntityFramework.Models;

/// <summary>
/// Represents a category or type of music.
/// </summary>
/// <param name="Name">The name of the genre. This value cannot be null or empty and must be unique.</param>
[DebuggerDisplay("Genre: {Name}")]
public record Genre(string Name) : IValidatable, IDimensionValidator<Genre, string>
{
    public const int MaxNameLength = 50;

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask ValidateAsync(
        object? context = null,
        CancellationToken ct = default)
        => await new GenreValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, ct).ConfigureAwait(false);
    #endregion

    public static Expression<Func<Genre, string>> ValueExpression => genre => genre.Name;

    /// <summary>
    /// Determines whether all specified values are present in the genres' set of values (are in the cache) in other words,
    /// determines if all specified values are known genres.
    /// </summary>
    /// <returns><see langword="true"/> if the collection contains at least one valid value; otherwise, <see langword="false"/>.</returns>
    /// <returns></returns>
    public static bool Has(params string[] values) => IDimensionValidator<Genre, string>.Has(values);

    /// <summary>
    /// Determines whether all specified values are present in the genre' set of values (are in the cache) in other words,
    /// determines if all specified values are known genres.
    /// </summary>
    /// <param name="values">The collection of strings to validate. Cannot be null.</param>
    /// <returns>
    /// <see langword="true"/> if all strings in the collection are present in the cache (are known instruments); otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Has(IEnumerable<string> values) => IDimensionValidator<Genre, string>.Has(values);
}
