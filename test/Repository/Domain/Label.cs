namespace vm2.Repository.Domain;

[DebuggerDisplay("{Name} ({CountryCode})")]
public class Label : IFindable<Label>, IAuditable, IValidatable
{
    /// <summary>
    /// Represents the maximum allowed length for Label's name.
    /// </summary>
    /// <remarks>This constant defines the upper limit for the number of characters in a name. It can be used
    /// to validate input or enforce constraints in applications.</remarks>
    public const int MaxNameLength = 100;

    /// <summary>
    /// Gets or sets the unique identifier for the recording label entities.
    /// </summary>
    public uint Id { get; private set; }

    /// <summary>
    /// Gets or sets the name of the recording label.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the country code where the recording label company is registered.
    /// </summary>
    public string CountryCode { get; set; }

    /// <summary>
    /// Gets the collection of albums released by this label.
    /// </summary>
    public HashSet<Album> Albums { get; private set; } = [];

    #region IAuditable
    /// <inheritdoc />
    public DateTimeOffset CreatedAt { get; set; } = default;

    /// <inheritdoc />
    public string CreatedBy { get; set; } = "";

    /// <inheritdoc />
    public DateTimeOffset UpdatedAt { get; set; } = default;

    /// <inheritdoc />
    public string UpdatedBy { get; set; } = "";
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Label"/> class with specified creation and update metadata. For use mostly
    /// by EF.
    /// </summary>
    /// <param name="id">The unique identifier for the label.</param>
    /// <param name="name">The name of the label. Cannot be null or empty.</param>
    /// <param name="countryCode">The country code associated with the label. Must be a valid ISO 3166-1 alpha-2 code.</param>
    /// <param name="createdAt">The date and time when the label was created. Defaults to the current date and time if not specified.</param>
    /// <param name="createdBy">The identifier of the actor who created the label. Defaults to an empty string if not specified.</param>
    /// <param name="updatedAt">The date and time when the label was last updated. Defaults to the current date and time if not specified.</param>
    /// <param name="updatedBy">The identifier of the actor who last updated the label. Defaults to an empty string if not specified.</param>
    public Label(
        uint id,
        string name,
        string countryCode,
        DateTimeOffset createdAt = default,
        string createdBy = "",
        DateTimeOffset updatedAt = default,
        string updatedBy = "")
    {
        Id          = id;
        Name        = name;
        CountryCode = countryCode;
        CreatedAt   = createdAt;
        CreatedBy   = createdBy;
        UpdatedAt   = updatedAt;
        UpdatedBy   = updatedBy;

        new LabelInvariantValidator()
                .ValidateAndThrow(this);
    }

    #region IFindable<Label>
    /// <inheritdoc />
    public static Expression<Func<Label, object?>> KeyExpression => l => new { l.Id };

    /// <inheritdoc />
    public ValueTask ValidateFindable(object? _, CancellationToken __)
    {
        new LabelFindableValidator()
                .ValidateAndThrow(this);
        return ValueTask.CompletedTask;
    }
    #endregion

    /// <summary>
    /// Returns a struct implementing <see cref="IFindable"/> that can be used to find a <see cref="Label"/> by its unique
    /// identifier. Can be used in <see cref="IRepository.Find{Label}(IFindable, CancellationToken)"/> and
    /// <see cref="QueryableExtensions.Find{Label}(IFindable, CancellationToken)"/>. E.g.<br/>
    /// <c><![CDATA[var label = await _repository.Find(Label.ById(42), ct);]]></c>
    /// </summary>
    /// <param name="id">The unique identifier for the label.</param>
    public static IFindable ById(int Id) => new Findable(Id);

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new LabelValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken);
    #endregion

    public Label ReleasesAlbum(Album album)
    {
        if (album.Label is null)
            throw new InvalidOperationException("Album is already assigned to a different label.");

        album.Label = this;
        return this;
    }
}
