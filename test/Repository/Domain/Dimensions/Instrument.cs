namespace vm2.Repository.Domain.Dimensions;

/// <summary>
/// Represents a dimension of musical instruments with their unique codes (the accepted abbreviation, e.g. "tp") and full names
/// ("trumpet").
/// </summary>
/// <remarks>The <see cref="Instrument"/> class provides properties to access the code and name of an instrument.
/// <param name="Code">The instrument's code, e.g. "ts" for tenor saxophone. This value cannot be null or empty and must be unique.</param>
/// <param name="Name">The instrument's full name, e.g. "French horn". This value cannot be null or empty.</param>
[DebuggerDisplay("Instrument: {Name}")]
public record Instrument(string Code, string Name) : IFindable<Instrument>, IValidatable
{
    #region IFindable<Instrument>
    /// <inheritdoc />
    public static Expression<Func<Instrument, object?>> KeyExpression => i => i.Code;
    #endregion

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new InstrumentValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken);
    #endregion
}
