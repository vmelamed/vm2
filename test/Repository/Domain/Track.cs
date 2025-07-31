namespace vm2.Repository.Domain;

[DebuggerDisplay("{Title} ({Duration})")]
public class Track : IFindable<Track>, IAuditable, IValidatable
{
    public const int MaxTitleLength = 256;

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    internal uint Id { get; set; } = 0;

    /// <summary>
    /// Gets or sets the title of the track (song).
    /// </summary>
    public string Title { get; set; } = "";

    /// <summary>
    /// Gets or sets the duration of the track.
    /// </summary>
    public TimeSpan Duration { get; set; } = default;

    /// <summary>
    /// Gets or sets the collection of artists recorded on the track.
    /// </summary>
    public ICollection<TrackPerson> Personnel { get; set; } = [];

    /// <summary>
    /// Gets or sets the original album that this track appeared for first time on.
    /// </summary>
    public Album? OriginalAlbum { get; set; } = null;

    /// <summary>
    /// Gets or sets the collection of albums featuring this track.
    /// </summary>
    public HashSet<Album> Albums { get; set; } = [];

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
    /// Initializes a new instance of the <see cref="Track"/> class with the specified details.
    /// </summary>
    /// <remarks>
    /// This constructor allows for the creation of a track with detailed metadata, including its associated personnel, albums,
    /// and audit information. Used by EF when materializing <see cref="Track"/> instances.
    /// </remarks>
    /// <param name="id">The unique identifier for the track.</param>
    /// <param name="title">The title of the track. Cannot be null or empty.</param>
    /// <param name="duration">The duration of the track. Cannot be <c>default(TimeSpan)</c>.</param>
    /// <param name="createdAt">The date and time when the track was created.</param>
    /// <param name="createdBy">The user or system that created the track.</param>
    /// <param name="updatedAt">The date and time when the track was last updated.</param>
    /// <param name="updatedBy">The user or system that last updated the track.</param>
    public Track(
        uint id,
        string title,
        TimeSpan duration,
        DateTimeOffset createdAt = default,
        string createdBy = "",
        DateTimeOffset updatedAt = default,
        string updatedBy = "")
    {
        Id              = id;
        Title           = title;
        Duration        = duration;
        CreatedAt       = createdAt;
        CreatedBy       = createdBy;
        UpdatedAt       = updatedAt;
        UpdatedBy       = updatedBy;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Track"/> class with the specified details. Used by unit tests to
    /// materialize <see cref="Track"/> instances containing tracks (EF cannot do this).
    /// </summary>
    /// <remarks>
    /// This constructor allows for the creation of a track with detailed metadata, including its associated personnel, albums,
    /// and audit information. Used by EF when materializing <see cref="Track"/> instances.
    /// </remarks>
    /// <param name="id">The unique identifier for the track.</param>
    /// <param name="title">The title of the track. Cannot be null or empty.</param>
    /// <param name="duration">The duration of the track. Cannot be <c>default(TimeSpan)</c>.</param>
    /// <param name="personnel">An optional collection of personnel associated with the track, such as artists or contributors.</param>
    /// <param name="originalAlbum">The optional original album to which the track belongs.</param>
    /// <param name="albums">An optional collection of albums that include this track.</param>
    /// <param name="createdAt">The date and time when the track was created.</param>
    /// <param name="createdBy">The user or system that created the track.</param>
    /// <param name="updatedAt">The date and time when the track was last updated.</param>
    /// <param name="updatedBy">The user or system that last updated the track.</param>
    public Track(
        uint id,
        string title,
        TimeSpan duration,
        ICollection<TrackPerson>? personnel = null,
        Album? originalAlbum = null,
        IEnumerable<Album>? albums = null,
        DateTimeOffset createdAt = default,
        string createdBy = "",
        DateTimeOffset updatedAt = default,
        string updatedBy = "")
        : this(id, title, duration, createdAt, createdBy, updatedAt, updatedBy)
    {
        Personnel       = personnel ?? [];
        OriginalAlbum   = originalAlbum;
        Albums          = albums?.ToHashSet() ?? [];
    }

    #region IFindable<Track>
    public static Expression<Func<Track, object?>> KeyExpression => t => new { t.Id };

    /// <inheritdoc />
    public ValueTask ValidateFindable(object? _, CancellationToken __)
    {
        new TrackFindableValidator()
                .ValidateAndThrow(this);
        return ValueTask.CompletedTask;
    }
    #endregion

    /// <summary>
    /// Returns a struct implementing <see cref="IFindable"/> that can be used to find a <see cref="Track"/> by its unique
    /// identifier. Can be used in <see cref="IRepository.Find{Track}(IFindable, CancellationToken)"/> and
    /// <see cref="QueryableExtensions.Find{Track}(IFindable, CancellationToken)"/>. E.g.<br/>
    /// <code><![CDATA[var track = await _repository.Find(Track.ById(42), ct);]]></code>
    /// </summary>
    /// <param name="id">The unique identifier for the track.</param>
    public static IFindable ById(int Id) => new Findable(Id);

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(object? context = null, CancellationToken cancellationToken = default)
        => await new TrackValidator().ValidateAndThrowAsync(this, cancellationToken);
    #endregion

    /// <summary>
    /// Associates the track with the specified original album and updates the album's identifier.
    /// </summary>
    /// <param name="album">The album to associate as the original release for the track. Cannot be null.</param>
    /// <returns>The current <see cref="Track"/> instance with the updated original album information.</returns>
    public Track OriginallyReleasesOn(Album album, int index = -1)
    {
        OriginalAlbum   = album;
        return ReleasedOn(album, index);
    }

    public Track ReleasedOn(Album album, int index = -1)
    {
        album.AddTrack(this, index);
        return this;
    }
}
