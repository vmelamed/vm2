namespace vm2.Repository.Tests.Domain.Dimensions;

/// <summary>
/// Represents a dimension of musical instruments with their unique codes (the accepted abbreviation, e.g. "tp") and full names
/// ("trumpet").
/// </summary>
/// <remarks>The <see cref="Instrument"/> class provides properties to access the code and name of an instrument.
/// <param name="Code">The instrument's code, e.g. "ts" for tenor saxophone. This value cannot be null or empty and must be unique.</param>
/// <param name="Name">The instrument's full name, e.g. "French horn". This value cannot be null or empty.</param>
[DebuggerDisplay("Instrument: {Name}")]
public record Instrument(string Code, string Name) : IFindable<Instrument>, IValidatable, IDimensionValidator<Instrument, string>
{
    public const int MaxCodeLength = 8;

    public const int MaxNameLength = 256;

    #region IFindable<Instrument>
    /// <inheritdoc />
    public static Expression<Func<Instrument, object?>> KeyExpression => i => i.Code;

    /// <inheritdoc />
    public ValueTask ValidateFindable(
        object? context = null,
        CancellationToken cancellationToken = default)
    {
        new InstrumentFindableValidator()
                        .ValidateAndThrow(this);
        return ValueTask.CompletedTask;
    }
    #endregion

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new InstrumentValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken).ConfigureAwait(false);
    #endregion

    /// <inheritdoc />
    public static Expression<Func<Instrument, string>> ValueExpression => instrument => instrument.Code;

    /// <summary>
    /// Determines whether all specified values are present in the instruments' set of values (are in the cache) in other words,
    /// determines if all specified values are known instruments.
    /// </summary>
    /// <returns><see langword="true"/> if the collection contains at least one valid value; otherwise, <see langword="false"/>.</returns>
    /// <returns></returns>
    public static bool Has(params string[] values) => IDimensionValidator<Instrument, string>.Has(values);

    /// <summary>
    /// Determines whether all specified values are present in the instruments' set of values (are in the cache) in other words,
    /// determines if all specified values are known instruments.
    /// </summary>
    /// <param name="values">The collection of strings to validate. Cannot be null.</param>
    /// <returns>
    /// <see langword="true"/> if all strings in the collection are present in the cache (are known instruments); otherwise, <see langword="false"/>.
    /// </returns>
    public static bool Has(IEnumerable<string> values) => IDimensionValidator<Instrument, string>.Has(values);
}
