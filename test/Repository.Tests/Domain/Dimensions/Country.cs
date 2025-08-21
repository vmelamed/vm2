namespace vm2.Repository.Tests.Domain.Dimensions;

using vm2.Repository.EfRepository.Models;

/// <summary>
/// Represents a dimension of countries with their unique 2-letter ISO 3166 codes and full names.
/// ("trumpet").
/// </summary>
/// <remarks>The <see cref="Country"/> class provides properties to access the code and name of a country.
/// <param name="Code">The country's unique 2-letter ISO 3166 code, e.g. "US". This value cannot be null or empty and must be unique.</param>
/// <param name="Name">The country's full name, e.g. "United States of America". This value cannot be null or empty.</param>
[DebuggerDisplay("Country: {Name}")]
public sealed record Country(string Code, string Name) : IFindable<Country>, IValidatable, IDimensionValidator<Country, string>
{
    /// <summary>
    /// Represents the length for a country code.
    /// </summary>
    /// <remarks>This constant is typically used to validate or enforce constraints on code length in applications.</remarks>
    public const int CodeLength = 2;

    /// <summary>
    /// Represents the maximum allowable length for a country name.
    /// </summary>
    /// <remarks>
    /// This constant defines the upper limit for the number of characters in a country name. It can be used to validate input
    /// or enforce constraints in applications.
    /// </remarks>
    public const int MaxNameLength = 256;

    #region IFindable<Instrument>
    /// <inheritdoc />
    public static Expression<Func<Country, object?>> KeyExpression => c => c.Code;

    /// <inheritdoc />
    public ValueTask ValidateFindable(object? _, CancellationToken __)
    {
        new CountryFindableValidator()
                        .ValidateAndThrow(this);
        return ValueTask.CompletedTask;
    }
    #endregion

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new CountryValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken).ConfigureAwait(false);
    #endregion

    public static Expression<Func<Country, string>> ValueExpression => country => country.Code;

    /// <summary>
    /// Determines whether all specified values are present in the countries' set of values (are in the cache) in other words,
    /// determines if all specified values are known countries.
    /// </summary>
    /// <returns><see langword="true"/> if the collection contains at least one valid value; otherwise, <see langword="false"/>.</returns>
    /// <returns></returns>
    public static bool Has(params string[] values) => IDimensionValidator<Country, string>.Has(values);

    /// <summary>
    /// Determines whether all specified values are present in the countries' set of values (are in the cache) in other words,
    /// determines if all specified values are known countries.
    /// </summary>
    /// <param name="values">The collection of strings to validate. Cannot be null.</param>
    /// <returns>
    /// <see langword="true"/> if all strings in the collection are present in the cache (are known instruments); otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Has(IEnumerable<string> values) => IDimensionValidator<Country, string>.Has(values);
}
