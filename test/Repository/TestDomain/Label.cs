namespace vm2.Repository.TestDomain;

[DebuggerDisplay("Label {Id}: {Name} ({CountryCode})")]
public class Label : IFindable<Label>, IAuditable, IValidatable, IOptimisticConcurrency
{
    /// <summary>
    /// Represents the maximum allowed length for Label's name.
    /// </summary>
    /// <remarks>This constant defines the upper limit for the number of characters in a name. It can be used
    /// to validate input or enforce constraints in applications.</remarks>
    public const int MaxNameLength = 100;

    /// <summary>
    /// The collection of albums released by this label.
    /// </summary>
    HashSet<Album> _albums = new(ReferenceEqualityComparer.Instance);

    /// <summary>
    /// Gets or sets the unique identifier for the recording label entities.
    /// </summary>
    public LabelId Id { get; private set; }

    /// <summary>
    /// Gets or sets the name of the recording label.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the country code where the recording label company is registered.
    /// </summary>
    public string CountryCode { get; set; } = null!;

    /// <summary>
    /// Gets the collection of albums released by this label.
    /// </summary>
    public ICollection<Album> Albums => _albums;

    #region IAuditable
    /// <inheritdoc />
    public DateTime CreatedAt { get; set; } = default;

    /// <inheritdoc />
    public string CreatedBy { get; set; } = "";

    /// <inheritdoc />
    public DateTime UpdatedAt { get; set; } = default;

    /// <inheritdoc />
    public string UpdatedBy { get; set; } = "";
    #endregion

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

    /// <summary>
    /// Returns a struct implementing <see cref="IFindable"/> that can be used to find a <see cref="Label"/> by its unique
    /// identifier. Can be used in <see cref="IRepository.Find{Label}(IFindable, CancellationToken)"/> and
    /// <see cref="QueryableExtensions.Find{Label}(IFindable, CancellationToken)"/>. E.g.<br/>
    /// <c><![CDATA[var label = await _repository.Find(Label.ById(42), ct);]]></c>
    /// </summary>
    /// <param name="id">The unique identifier for the label.</param>
    public static IFindable ById(int Id) => new Findable(Id);
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Label"/> class with specified creation and update metadata. For use mostly
    /// by EF.
    /// </summary>
    /// <param name="id">The unique identifier for the label.</param>
    /// <param name="name">The name of the label. Cannot be null or empty.</param>
    /// <param name="countryCode">The country code associated with the label. Must be a valid ISO 3166-1 alpha-2 code.</param>
    /// <param name="createdAt">The date and time when the label was created.</param>
    /// <param name="createdBy">The identifier of the actor who created the label.</param>
    /// <param name="updatedAt">The date and time when the label was last updated.</param>
    /// <param name="updatedBy">The identifier of the actor who last updated the label.</param>
    public Label(
        LabelId id,
        string name,
        string countryCode,
        DateTime createdAt = default,
        string createdBy = "",
        DateTime updatedAt = default,
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

    /// <summary>
    /// Initializes a new instance of the <see cref="Label"/> class.
    /// </summary>
    /// <remarks>
    /// This parameterless constructor is required by Entity Framework Core for materializing instances of the <see cref="Label"/>
    /// class from the database. It is intended for use by EF Core and should not be called directly in application code.
    /// </remarks>
    private Label()
    {
    }

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new LabelValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken).ConfigureAwait(false);
    #endregion

    /// <summary>
    /// Assigns the specified album to this label.
    /// </summary>
    /// <param name="album">The album to be assigned to this label. Must not already be assigned to another label.</param>
    public Label Releases(Album album)
    {
        album.Label = this;
        return this;
    }
}
