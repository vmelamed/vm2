namespace vm2.Repository.Domain;

public class Track : IFindable<Track>, IAuditable, ISoftDeletable, IValidatable
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public uint Id { get; set; } = 0;

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
    /// Gets or sets the album associated with the current context.
    /// </summary>
    public Album? Album { get; init; } = null;

    /// <summary>
    /// Gets or sets the unique identifier of the album this track belongs to. (Navigation property!)
    /// </summary>
    public uint AlbumId { get; set; } = 0;

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
    public DateTimeOffset? DeletedAt { get; set; } = null;

    /// <inheritdoc />
    public string DeletedBy { get; set; } = "";
    #endregion

    public Track(
        uint id,
        string title,
        TimeSpan duration,
        ICollection<TrackPerson>? personnel = null,
        Album? album = null,
        uint albumId = 0,
        DateTimeOffset createdAt = default,
        string createdBy = "",
        DateTimeOffset updatedAt = default,
        string updatedBy = "")
    {
        Id        = id;
        Title     = title;
        Duration  = duration;
        Personnel = personnel ?? [];
        Album     = album;
        AlbumId   = album is null ? albumId : album.Id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        UpdatedAt = updatedAt;
        UpdatedBy = updatedBy;
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
}
