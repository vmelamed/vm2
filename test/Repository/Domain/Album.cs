namespace vm2.Repository.Domain;

[DebuggerDisplay("Album: {Title}")]
public class Album : IFindable<Album>, IAuditable, ISoftDeletable, IValidatable
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public int Id { get; set; } = -1;

    /// <summary>
    /// Gets or sets the title of the album.
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    /// Gets or sets the release year of the album.
    /// </summary>
    public int? ReleaseYear { get; set; } = null;

    /// <summary>
    /// Gets or sets the collection of artists associated with the album.
    /// </summary>
    public ICollection<Person> Personnel { get; set; } = [];

    /// <summary>
    /// Gets or sets the recording label under which the album was released.
    /// </summary>
    public Label? Label { get; set; } = null;

    /// <summary>
    /// Gets or sets the identifier for the label.
    /// </summary>
    public int LabelId { get; set; } = -1;

    /// <summary>
    /// Gets or sets the collection of tracks on this album.
    /// </summary>
    public ICollection<Track> Tracks { get; set; } = [];

    /// <summary>
    /// This constructor is intentionally left empty to allow EF Core to create instances of this class.
    /// It is not meant to be used directly in application code.
    /// </summary>
    private Album()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Album"/> class with the specified details.
    /// </summary>
    /// <param name="id">The unique identifier for the album.</param>
    /// <param name="title">The title of the album.</param>
    /// <param name="releaseYear">The year when the album was released.</param>
    /// <param name="personnel">An optional collection of <see cref="Person"/>-s - the personnel involved in the album.</param>
    /// <param name="label">An optional <see cref="Label"/> object representing the record label that released the album.</param>
    /// <param name="labelId">The identifier for the label.</param>
    /// <param name="tracks">An optional collection of <see cref="Track"/>-s - the tracks in the album.</param>
    /// <param name="createdAt">The date and time when the album instance was created.</param>
    /// <param name="createdBy">The name of the actor who created the album instance.</param>
    /// <param name="updatedAt">The date and time when the album record was last updated.</param>
    /// <param name="updatedBy">The name of the actor who last updated the album record.</param>
    public Album(
        int id,
        string title,
        int? releaseYear,
        ICollection<Person>? personnel = null,
        Label? label = null,
        int labelId = 0,
        ICollection<Track>? tracks = null,
        DateTimeOffset createdAt = default,
        string createdBy = "",
        DateTimeOffset updatedAt = default,
        string updatedBy = "")
    {
        Id          = id;
        Title       = title;
        ReleaseYear = releaseYear;
        Personnel   = personnel ?? [];
        Label       = label;
        LabelId     = label is null ? labelId : label.Id;
        Tracks      = tracks ?? [];
        CreatedAt   = createdAt;
        CreatedBy   = createdBy;
        UpdatedAt   = updatedAt;
        UpdatedBy   = updatedBy;
    }

    #region IFindable<Album>
    /// <inheritdoc />
    public static Expression<Func<Album, object?>> KeyExpression => a => new { a.Id };
    #endregion

    /// <summary>
    /// Returns a struct implementing <see cref="IFindable"/> that can be used to find a <see cref="Album"/> by its unique
    /// identifier. Can be used in <see cref="IRepository.Find{Album}(IFindable, CancellationToken)"/> and
    /// <see cref="QueryableExtensions.Find{Album}(IFindable, CancellationToken)"/>. E.g.<br/>
    /// <code><![CDATA[var album = await _repository.Find(Album.ById(42), ct);]]></code>
    /// </summary>
    /// <param name="id">The unique identifier for the album.</param>
    public static IFindable ById(int Id) => new Findable(Id);

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

    #region ISoftDeletable
    /// <inheritdoc />
    public DateTimeOffset? DeletedAt { get; set; } = default;

    /// <inheritdoc />
    public string DeletedBy { get; set; } = "";
    #endregion

    #region IValidatable
    public ValueTask Validate(object? context = null, CancellationToken cancellationToken = default) => throw new NotImplementedException();
    #endregion
}
