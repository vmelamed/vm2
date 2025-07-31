namespace vm2.Repository.Domain;

[DebuggerDisplay("Album: {Title}")]
public class Album : IFindable<Album>, IAuditable, ISoftDeletable, IValidatable
{
    public const int MaxTitleLength = 250;

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    internal uint Id { get; private set; } = 0;

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
    public HashSet<Person> Personnel { get; private set; } = [];

    /// <summary>
    /// Gets or sets the recording label under which the album was released.
    /// </summary>
    public Label? Label { get; set; } = null;

    /// <summary>
    /// Gets or sets the collection of tracks on this album.
    /// </summary>
    public List<Track> Tracks { get; private set; } = [];

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

    /// <summary>
    /// Initializes a new instance of the <see cref="Album"/> class with the specified details. Used by EF to materialize
    /// <see cref="Album"/> instances from the DB.
    /// </summary>
    /// <param name="id">The unique identifier for the album.</param>
    /// <param name="title">The title of the album.</param>
    /// <param name="releaseYear">The year when the album was released.</param>
    /// <param name="createdAt">The date and time when the album instance was created.</param>
    /// <param name="createdBy">The name of the actor who created the album instance.</param>
    /// <param name="updatedAt">The date and time when the album record was last updated.</param>
    /// <param name="updatedBy">The name of the actor who last updated the album record.</param>
    /// <param name="deletedAt">The dater and time when the album was soft-deleted.</param>
    /// <param name="deletedAt">The name of the actor who soft-deleted the album.</param>
    public Album(
        uint id,
        string title,
        int? releaseYear,
        DateTimeOffset createdAt = default,
        string createdBy = "",
        DateTimeOffset updatedAt = default,
        string updatedBy = "",
        DateTimeOffset? deletedAt = null,
        string deletedBy = "")
    {
        Id          = id;
        Title       = title;
        ReleaseYear = releaseYear;
        CreatedAt   = createdAt;
        CreatedBy   = createdBy;
        UpdatedAt   = updatedAt;
        UpdatedBy   = updatedBy;
        DeletedAt   = deletedAt;
        DeletedBy   = deletedBy;

        new AlbumInvariantValidator()
                .ValidateAndThrow(this);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Album"/> class with the specified details. Used by unit tests to
    /// materialize <see cref="Album"/> instances containing tracks (EF cannot do this).
    /// </summary>
    /// <param name="id">The unique identifier for the album.</param>
    /// <param name="title">The title of the album.</param>
    /// <param name="releaseYear">The year when the album was released.</param>
    /// <param name="personnel">An optional collection of <see cref="Person"/>-s - the personnel involved in the album.</param>
    /// <param name="label">An optional <see cref="Label"/> object representing the record label that released the album.</param>
    /// <param name="tracks">An optional collection of <see cref="Track"/>-s - the tracks in the album.</param>
    /// <param name="createdAt">The date and time when the album instance was created.</param>
    /// <param name="createdBy">The name of the actor who created the album instance.</param>
    /// <param name="updatedAt">The date and time when the album record was last updated.</param>
    /// <param name="updatedBy">The name of the actor who last updated the album record.</param>
    /// <param name="deletedAt">The dater and time when the album was soft-deleted.</param>
    /// <param name="deletedAt">The name of the actor who soft-deleted the album.</param>
    public Album(
        uint id,
        string title,
        int? releaseYear,
        IEnumerable<Person>? personnel = null,
        Label? label = null,
        IEnumerable<Track>? tracks = null,
        DateTimeOffset createdAt = default,
        string createdBy = "",
        DateTimeOffset updatedAt = default,
        string updatedBy = "",
        DateTimeOffset? deletedAt = null,
        string deletedBy = "")
        : this(id, title, releaseYear, createdAt, createdBy, updatedAt, updatedBy, deletedAt, deletedBy)
    {
        Tracks      = tracks?.ToList() ?? [];
        Personnel   = personnel?.ToHashSet() ?? [];
        Label       = label;
    }

    #region IFindable<Album>
    /// <inheritdoc />
    public static Expression<Func<Album, object?>> KeyExpression => a => new { a.Id };

    /// <inheritdoc />
    public ValueTask ValidateFindable(object? _, CancellationToken __)
    {
        new AlbumFindableValidator()
                .ValidateAndThrow(this);
        return ValueTask.CompletedTask;
    }
    #endregion

    /// <summary>
    /// Returns a struct implementing <see cref="IFindable"/> that can be used to find a <see cref="Album"/> by its unique
    /// identifier. Can be used in <see cref="IRepository.Find{Album}(IFindable, CancellationToken)"/> and
    /// <see cref="QueryableExtensions.Find{Album}(IFindable, CancellationToken)"/>. E.g.<br/>
    /// <c><![CDATA[var album = await _repository.Find(Album.ById(42), ct);]]></c>
    /// </summary>
    /// <param name="id">The unique identifier for the album.</param>
    public static IFindable ById(int Id) => new Findable(Id);

    #region IValidatable
    /// <inheritdoc />
    public ValueTask Validate(object? context = null, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
    #endregion

    /// <summary>
    /// Assigns the album to the specified label.
    /// </summary>
    /// <param name="label">The label to which the album will be assigned. Cannot be <see langword="null"/>.</param>
    /// <returns>The current <see cref="Album"/> instance with the updated label assignment.</returns>
    public Album AssignToLabel(Label label)
    {
        if (Label is not null)
            throw new InvalidOperationException("Album is already assigned to a label.");

        Label = label;
        return this;
    }

    /// <summary>
    /// Adds a person to the album's personnel list.
    /// </summary>
    /// <param name="person">The person to add to the album. Cannot be null.</param>
    /// <returns>The current instance of the <see cref="Album"/> class, allowing for method chaining.</returns>
    public Album AddPerson(Person person)
    {
        Personnel.Add(person);
        return this;
    }

    /// <summary>
    /// Removes the specified person from the album's personnel list.
    /// </summary>
    /// <param name="person">The person to be removed from the album. Cannot be null.</param>
    /// <returns>The current instance of the album, allowing for method chaining.</returns>
    public Album RemovePerson(Person person)
    {
        Personnel.Remove(person);
        return this;
    }

    /// <summary>
    /// Adds a track to the collection of tracks.
    /// </summary>
    /// <param name="track">The track to add to the collection. Cannot be null.</param>
    /// <returns>The current instance of the album, allowing for method chaining.</returns>
    public Album AddTrack(Track track)
    {
        Tracks.Add(track);
        return this;
    }

    /// <summary>
    /// Removes a track from the collection of tracks.
    /// </summary>
    /// <param name="track">The track to add to the collection. Cannot be null.</param>
    /// <returns>The current instance of the album, allowing for method chaining.</returns>
    public Album RemoveTrack(Track track)
    {
        Tracks.Add(track);
        return this;
    }

    /// <summary>
    /// Adds a track to the album at the specified index or appends it to the end if no index is provided.
    /// </summary>
    /// <remarks>If the track is already present in the album at the specified index, no changes are made. Adding a track also
    /// updates the album's personnel list by including all personnel associated with the track.
    /// </remarks>
    /// <param name="track">The track to add to the album. Cannot be null.</param>
    /// <param name="index">
    /// The zero-based index at which to insert the track. If set to -1 (default) or equal or greater than the number of tracks
    /// that are already on the album, the track is appended to the end.
    /// </param>
    /// <returns>The current <see cref="Album"/> instance, allowing for method chaining.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the track is already in the album at a different index.
    /// </exception>
    public Album AddTrack(Track track, int index = -1)
    {
        var currentIndex = Tracks.IndexOf(track);

        if (currentIndex is not -1)
        {
            if (currentIndex == index)
                return this;
            throw new InvalidOperationException("The track is already at a different index.");
        }

        if (index < 0 || index >= Tracks.Count)
            Tracks.Add(track);
        else
            Tracks.Insert(index, track);

        foreach (var trackPerson in track.Personnel)
            Personnel.Add(trackPerson.Person);

        return this;
    }
}
