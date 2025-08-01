namespace vm2.Repository.Domain;

[DebuggerDisplay("Album: {Title}")]
public class Album : IFindable<Album>, IAuditable, ISoftDeletable, IValidatable, IEquatable<Album>
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
    public List<AlbumTrack> Tracks { get; private set; } = [];

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
    /// Adds a track to the album at the specified index or appends it to the end if no index is provided.
    /// </summary>
    /// <remarks>If the track is already present in the album at the specified index, no changes are made. Adding a track also
    /// updates the album's personnel list by including all personnel associated with the track.
    /// </remarks>
    /// <param name="track">The track to add to the album. Cannot be null.</param>
    /// <param name="orderNumber">
    /// The zero-based index at which to insert the track. If set to -1 (default) or equal or greater than the number of tracks
    /// that are already on the album, the track is appended to the end.
    /// </param>
    /// <param name="firstRelease">Indicates whether the track was released on this album for the first time.</param>
    /// <returns>The current <see cref="Album"/> instance, allowing for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the track is already in the album at a different index.</exception>
    public Album AddTrack(Track track, uint orderNumber, bool firstRelease)
    {
        var existingTrack = Tracks.FirstOrDefault(t => t.Track == track);

        if (existingTrack.Track is not null)
        {
            var currentOrder = (uint)Tracks.IndexOf(existingTrack);

            if (currentOrder != orderNumber)
                throw new InvalidOperationException("The track is already at a different order number.");

            Tracks.Remove(existingTrack);
            Tracks.Insert((int)orderNumber, existingTrack with { FirstRelease = firstRelease });

            return this;
        }

        if (orderNumber == 0 || orderNumber >= Tracks.Count)
            Tracks.Add(new AlbumTrack(track, firstRelease));
        else
            Tracks.Insert((int)orderNumber, new AlbumTrack(track, firstRelease));

        foreach (var trackPerson in track.Personnel)
            Personnel.Add(trackPerson.Person);

        return this;
    }

    /// <summary>
    /// Removes the specified track from the album.
    /// </summary>
    /// <remarks>
    /// This method removes the specified track from the album's track list and reorders the remaining
    /// tracks to maintain sequential order numbers.
    /// </remarks>
    /// <param name="track">The track to remove from the album. Cannot be <see langword="null"/>.</param>
    /// <returns>The current <see cref="Album"/> instance after the track has been removed.</returns>
    public Album RemoveTrack(Track track)
    {
        var existingTrack = Tracks.FirstOrDefault(t => t.Track == track);

        if (existingTrack.Track is null)
            return this;

        Tracks.Remove(existingTrack);
        return this;
    }

    #region Identity rules implementation.
    #region IEquatable<Album> Members
    /// <summary>
    /// Indicates whether the current object is equal to a reference to another object of the same type.
    /// </summary>
    /// <param name="other">A reference to another object of type <see cref="Album"/> to compare with the current object.</param>
    /// <returns>
    /// <list type="number">
    ///     <item><see langword="false"/> if <paramref name="other"/> is equal to <see langword="null"/>, otherwise</item>
    ///     <item><see langword="true"/> if <paramref name="other"/> refers to <c>this</c> object, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="other"/> is not the same type as <c>this</c> object, otherwise</item>
    ///     <item><see langword="true"/> if the current object and the <paramref name="other"/> are considered to be equal,
    ///                                  e.g. their business identities are equal; otherwise, <see langword="false"/>.</item>
    /// </list>
    /// </returns>
    public virtual bool Equals(Album? other)
        => other is not null  &&
           (ReferenceEquals(this, other)  ||  GetType() == other.GetType() && Id == other.Id);
    #endregion

    /// <summary>
    /// Determines whether this <see cref="Album"/> instance is equal to the specified <see cref="object"/> reference.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> reference to compare with this <see cref="Album"/> object.</param>
    /// <returns>
    /// <list type="number">
    ///     <item><see langword="false"/> if <paramref name="obj"/> cannot be cast to <see cref="Album"/>, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="obj"/> is equal to <see langword="null"/>, otherwise</item>
    ///     <item><see langword="true"/> if <paramref name="obj"/> refers to <c>this</c> object, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="obj"/> is not the same type as <c>this</c> object, otherwise</item>
    ///     <item><see langword="true"/> if the current object and the <paramref name="obj"/> are considered to be equal,
    ///                                  e.g. their business identities are equal; otherwise, <see langword="false"/>.</item>
    /// </list>
    /// </returns>
    public override bool Equals(object? obj) => Equals(obj as Album);

    /// <summary>
    /// Serves as a hash function for the objects of <see cref="Album"/> and its derived types.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Album"/> instance.</returns>
    public override int GetHashCode() => Id.GetHashCode();

    /// <summary>
    /// Compares two <see cref="Album"/> objects.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// <see langword="true"/> if the objects are considered to be equal (<see cref="Equals(Album)"/>);
    /// otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Album left, Album right) => left is null ? right is null : left.Equals(right);

    /// <summary>
    /// Compares two <see cref="Album"/> objects.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// <see langword="true"/> if the objects are not considered to be equal (<see cref="Equals(Album)"/>);
    /// otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Album left, Album right) => !(left==right);
    #endregion
}
