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

    public static Expression<Func<Instrument, string>> ValueExpression
        => instrument => instrument.Code;

    public static bool HasValue(string value)
        => IDimensionValidator<Instrument, string>.Has(value);

    public static bool HasValues(IEnumerable<string> values)
        => IDimensionValidator<Instrument, string>.Has(values);
}
