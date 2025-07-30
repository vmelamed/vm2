namespace vm2.Repository.Domain;

[DebuggerDisplay("Album: {Title}")]
public class Album : IFindable<Album>, IAuditable, ISoftDeletable, IValidatable
{
    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public uint Id { get; set; } = 0;

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
    public HashSet<Person> Personnel { get; set; } = [];

    /// <summary>
    /// Gets or sets the recording label under which the album was released.
    /// </summary>
    public Label? Label { get; set; } = null;

    /// <summary>
    /// Gets or sets the identifier for the label.
    /// </summary>
    public uint LabelId { get; set; } = 0;

    /// <summary>
    /// Gets or sets the collection of tracks on this album.
    /// </summary>
    public ICollection<Track> Tracks { get; set; } = [];

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
        uint id,
        string title,
        int? releaseYear,
        IEnumerable<Person>? personnel = null,
        Label? label = null,
        uint labelId = 0,
        IEnumerable<Track>? tracks = null,
        DateTimeOffset createdAt = default,
        string createdBy = "",
        DateTimeOffset updatedAt = default,
        string updatedBy = "")
    {
        Id          = id;
        Title       = title;
        ReleaseYear = releaseYear;
        Personnel   = personnel?.ToHashSet() ?? [];
        Label       = label;
        LabelId     = label?.Id ?? labelId;
        Tracks      = tracks?.ToList() ?? [];
        CreatedAt   = createdAt;
        CreatedBy   = createdBy;
        UpdatedAt   = updatedAt;
        UpdatedBy   = updatedBy;
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
}
