namespace vm2.Repository.Domain.Dimensions;

/// <summary>
/// Represents a dimension of musical instruments with their unique codes (the accepted abbreviation, e.g. "tp") and full names
/// ("trumpet").
/// </summary>
/// <remarks>The <see cref="Instrument"/> class provides properties to access the code and name of an instrument.
/// <param name="Code">The instrument's code, e.g. "ts" for tenor saxophone. Cannot be empty and must be unique.</param>
/// <param name="Name">The instrument's full name, e.g. "French horn". Cannot be empty.</param>
public record Instrument(string Code, string Name) : IFindable<Instrument>, IValidatable
{
    /// <summary>
    /// This constructor is intentionally left empty to allow EF Core to create instances of this class.
    /// It is not meant to be used directly in application code.
    /// </summary>
    private Instrument()
        : this("", "")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Instrument"/> class with the specified code. This will make it appropriate
    /// for using the instance in methods that require <see cref="IFindable{Instrument}"/>.
    /// </summary>
    /// <param name="code">The unique code identifying the instrument. Cannot be null or empty.</param>
    public Instrument(string code)
        : this(code, "")
    {
    }

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
