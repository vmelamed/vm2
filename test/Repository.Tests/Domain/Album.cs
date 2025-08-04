namespace vm2.Repository.Tests.Domain;

[DebuggerDisplay("Album: {Title}")]
public class Album : IFindable<Album>, IAuditable, ISoftDeletable, IValidatable, IEquatable<Album>
{
    public const int MaxTitleLength = 250;

    HashSet<string> _genres = [];
    List<AlbumTrack> _tracks = [];
    HashSet<Person> _personnel = [];
    HashSet<AlbumPerson> _albumsPersons = [];

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    internal uint Id { get; private set; }

    /// <summary>
    /// Gets or sets the title of the album.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the release year of the album.
    /// </summary>
    public int? ReleaseYear { get; private set; }

    /// <summary>
    /// Gets the collection of genres that the tracks on this album can be categorized under.
    /// </summary>
    public IEnumerable<string> Genres => _genres;

    /// <summary>
    /// Gets or sets the recording label under which the album was released.
    /// </summary>
    public Label? Label { get; internal set; }

    /// <summary>
    /// Gets the unique identifier for the label.
    /// </summary>
    internal uint LabelId { get; private set; }

    /// <summary>
    /// Gets or sets the collection of artists associated with the album.
    /// </summary>
    public IEnumerable<Person> Personnel => _personnel;

    /// <summary>
    /// Gets the collection of personnel associated with the album, such as artists, producers, and contributors.
    /// </summary>
    internal IEnumerable<AlbumPerson> AlbumsPersons => _albumsPersons;

    /// <summary>
    /// Gets or sets the collection of tracks on this album.
    /// </summary>
    internal IEnumerable<AlbumTrack> Tracks => _tracks;

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
    /// Initializes a new instance of the <see cref="Album"/> class.
    /// </summary>
    /// <remarks>
    /// This parameterless constructor is required by Entity Framework Core for materializing instances of the <see cref="Album"/>
    /// class from the database. It is intended for use by EF Core and should not be called directly in application code.
    /// </remarks>
    private Album()
    {
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
    /// <param name="year">The year of the album's release.</param>
    /// <param name="force">
    /// If <see langword="true"/>, allows reassigning the album to a different label even if it is already assigned to one.
    /// </param>
    /// <returns>The current <see cref="Album"/> instance with the updated label assignment.</returns>
    public Album Release(Label label, int? year = null, bool force = false)
    {
        if (Label is not null && !force)
            throw new InvalidOperationException("Album is already assigned to a label.");

        Label = label;
        ReleaseYear = year;
        Label.Releases(this);
        return this;
    }

    /// <summary>
    /// Adds a person to the album's personnel list.
    /// </summary>
    /// <param name="person">The person to add to the album. Cannot be null.</param>
    /// <param name="roles">The roles that the person has on the album.</param>
    /// <param name="instruments">The instrument codes that the person plays on the album.</param>
    /// <returns>The current instance of the <see cref="Album"/> class, allowing for method chaining.</returns>
    public Album AddPerson(
        Person person,
        IEnumerable<string> roles,
        IEnumerable<string> instruments)
    {
        var albumPerson = _albumsPersons.FirstOrDefault(ap => ap.Person == person);

        if (albumPerson is null)
            _albumsPersons.Add(albumPerson = new AlbumPerson(this, person, roles, instruments));
        else
            albumPerson
                .AddRoles(roles)
                .AddInstruments(instruments)
                ;

        return this;
    }

    /// <summary>
    /// Removes the specified person from the album's personnel list.
    /// </summary>
    /// <param name="person">The person to be removed from the album. Cannot be null.</param>
    /// <returns>The current instance of the album, allowing for method chaining.</returns>
    public Album RemovePerson(Person person)
    {
        var albumPerson = _albumsPersons.FirstOrDefault(ap => ap.Person == person);

        if (albumPerson is not null)
        {
            _albumsPersons.Remove(albumPerson);
            _personnel.Remove(person);
            // here we do not want to remove roles and instruments from the person, as they may be used in other albums
            // yes, it is asymmetric operation
        }

        return this;
    }

    /// <summary>
    /// Adds a track to the album at the specified index or appends it to the end if no index is provided.
    /// </summary>
    /// <remarks>If the track is already present in the album at the specified index, no changes are made. Adding a track also
    /// updates the album's personnel list by including all personnel associated with the track.
    /// </remarks>
    /// <param name="track">The track to add to the album. Cannot be null.</param>
    /// <param name="order">
    /// The zero-based index at which to insert the track. If set to -1 (default) or equal or greater than the number of tracks
    /// that are already on the album, the track is appended to the end.
    /// </param>
    /// <param name="firstRelease">Indicates whether the track was released on this album for the first time.</param>
    /// <returns>The current <see cref="Album"/> instance, allowing for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the track is already in the album at a different index.</exception>
    public Album AddTrackAfter(
        Track track,
        Track? after)
    {
        var trackAti = _tracks.Select((at, i) => (at, i)).FirstOrDefault(ati => ati.at.Track == track);

        if (after is null)
        {
            if (trackAti.at is null)
            {
                // the track is not in the album and no after track is specified: add to the end of the tracks
                _tracks.Add(new AlbumTrack(track));
                TrackAdded(track);
            }
            else
            {
                if (trackAti.i != _tracks.Count-1)
                {
                    // the track is already in the album, but not at the end: remove it and add to the end
                    _tracks.Remove(trackAti.at);
                    _tracks.Add(trackAti.at);
                }
                // otherwise the track is already at the end of the album: do nothing
            }

            return this;
        }

        Debug.Assert(after is not null, "After cannot be null at this point");

        var afterAti = _tracks.Select((at, i) => (at, i)).FirstOrDefault(ati => ati.at.Track == after);

        if (afterAti.at is null)
            throw new ArgumentException("The specified track to add after is not in the album.", nameof(after));

        if (trackAti.at is not null && trackAti.i == afterAti.i + 1)
            return this; // Track is already in the album after the specified track, no need to add it again.

        AlbumTrack at;

        // If the track is already in the album, we need to remove it first.
        if (trackAti.at is not null)
        {
            at = trackAti.at;
            _tracks.RemoveAt(trackAti.i);
        }
        else
        {
            at = new AlbumTrack(track);
            TrackAdded(track);
        }

        _tracks.Insert(afterAti.i, at);

        return this;
    }

    /// <summary>
    /// Adds a track to the album at the specified index or appends it to the end if no index is provided.
    /// </summary>
    /// <remarks>If the track is already present in the album at the specified index, no changes are made. Adding a track also
    /// updates the album's personnel list by including all personnel associated with the track.
    /// </remarks>
    /// <param name="track">The track to add to the album. Cannot be null.</param>
    /// <param name="order">
    /// The zero-based index at which to insert the track. If set to -1 (default) or equal or greater than the number of tracks
    /// that are already on the album, the track is appended to the end.
    /// </param>
    /// <param name="firstRelease">Indicates whether the track was released on this album for the first time.</param>
    /// <returns>The current <see cref="Album"/> instance, allowing for method chaining.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the track is already in the album at a different index.</exception>
    public Album AddTrackBefore(
        Track track,
        Track? before)
    {
        var trackAti = _tracks.Select((at, i) => (at, i)).FirstOrDefault(ati => ati.at.Track == track);

        if (before is null)
        {
            if (trackAti.at is null)
            {
                // the track is not in the album and no before track is specified: add at the beginning of the tracks
                _tracks.Insert(0, new AlbumTrack(track));
                return TrackAdded(track);
            }
            else
            {
                if (trackAti.i != 0)
                {
                    // the track is already in the album, but not at the beginning: remove it and add at the beginning
                    _tracks.Remove(trackAti.at);
                    _tracks.Insert(0, trackAti.at);
                }
                // otherwise the track is already in the beginning of the album: do nothing
                return this;
            }
        }

        Debug.Assert(before is not null, "After cannot be null at this point");

        var beforeAti = _tracks.Select((at, i) => (at, i)).FirstOrDefault(ati => ati.at.Track == before);

        if (beforeAti.at is null)
            throw new ArgumentException("The specified track to add after is not in the album.", nameof(before));

        if (trackAti.at is not null && trackAti.i == beforeAti.i - 1)
            return this; // Track is already in the album before the specified track, no need to add it again.

        AlbumTrack at;

        // If the track is already in the album, we need to put it in the right place.
        if (trackAti.at is null)
        {
            at = new AlbumTrack(track);
            TrackAdded(track);
        }
        else
        {
            at = trackAti.at;
            _tracks.RemoveAt(trackAti.i);
        }

        _tracks.Insert(beforeAti.i, at);

        return this;
    }

    /// <summary>
    /// A new track was added to the album. Ensure that the album's personnel list is updated accordingly.
    /// </summary>
    /// <param name="track">The added track.</param>
    /// <returns>The current <see cref="Album"/> instance.</returns>
    Album TrackAdded(Track track)
    {
        // add the genres from the track to the album's genres
        foreach (var genre in track.Genres)
            _genres.Add(genre);

        // add the track's personnel to the album's personnel
        foreach (var pt in track.TracksPersons)
        {
            var ap = _albumsPersons.FirstOrDefault(ap => ap.Person == pt.Person);

            if (ap is null)
                _albumsPersons.Add(ap = new AlbumPerson(this, pt.Person, pt.Roles, pt.Instruments));
            else
            {
                // if the person is already in the album, ensure that their roles and instruments are updated
                ap.AddRoles(pt.Roles);
                ap.AddInstruments(pt.Instruments);
            }

            ap.Person.AddAlbum(ap);
        }

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
        var existingTrack = _tracks.FirstOrDefault(t => t.TrackId == track.Id);

        if (existingTrack is null)
            return this;

        _tracks.Remove(existingTrack);
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
        => other is not null
           && (ReferenceEquals(this, other)
               || typeof(Album) == other.GetType()
                  && Id         == other.Id);
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
    public override bool Equals(object? obj)
        => Equals(obj as Album);

    /// <summary>
    /// Serves as a hash function for the objects of <see cref="Album"/> and its derived types.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Album"/> instance.</returns>
    public override int GetHashCode()
        => HashCode.Combine(typeof(Album), Id);

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
