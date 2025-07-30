namespace vm2.Repository.Domain.Dimensions;

/// <summary>
/// Represents a dimension of countries with their unique 2-letter ISO 3166 codes and full names.
/// ("trumpet").
/// </summary>
/// <remarks>The <see cref="Instrument"/> class provides properties to access the code and name of an instrument.
/// <param name="Code">The country's unique 2-letter ISO 3166 code, e.g. "US".</param>
/// <param name="Name">The country's full name, e.g. "United States of America".</param>
public record Country(string Code, string Name) : IFindable<Country>, IValidatable
{
    #region IFindable<Instrument>
    /// <inheritdoc />
    public static Expression<Func<Country, object?>> KeyExpression => c => c.Code;
    #endregion

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new CountryValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken);
    #endregion
}
