namespace vm2.Repository.TestDomain;
using System;

[DebuggerDisplay("Track {Id}: {Title}")]
public class Track : IFindable<Track>, IAuditable, IValidatable, IOptimisticConcurrency
{
    public const int MaxTitleLength = 256;

    HashSet<string> _genres = [];
    HashSet<Album> _albums = new(ReferenceEqualityComparer.Instance);
    HashSet<Person> _personnel = new(ReferenceEqualityComparer.Instance);
    HashSet<TrackPerson> _tracksPersons = new(ReferenceEqualityComparer.Instance);

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public TrackId Id { get; private set; }

    /// <summary>
    /// Gets or sets the title of the track (song).
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the duration of the track.
    /// </summary>
    public TimeSpan? Duration { get; set; } = null;

    /// <summary>
    /// Gets the collection of genres that this track can be categorized under. Yes, a track can belong to multiple genres, e.g.
    /// "jazz" and "fusion".
    /// </summary>
    public IEnumerable<string> Genres => _genres;

    /// <summary>
    /// Gets or sets the collection of albums featuring this track.
    /// </summary>
    public IEnumerable<Album> Albums => _albums;

    /// <summary>
    /// Gets or sets the collection of artists recorded on the track.
    /// </summary>
    public IEnumerable<Person> Personnel => _personnel;

    /// <summary>
    /// Gets a collection of persons associated with this track.
    /// </summary>
    public IEnumerable<TrackPerson> TracksPersons => _tracksPersons;

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

    #region IFindable<Track>
    public static Expression<Func<Track, object?>> KeyExpression => t => new { t.Id };

    /// <inheritdoc />
    public ValueTask ValidateFindable(object? _, CancellationToken __)
    {
        new TrackFindableValidator()
                .ValidateAndThrow(this);
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Returns a struct implementing <see cref="IFindable"/> that can be used to find a <see cref="Track"/> by its unique
    /// identifier. Can be used in <see cref="IRepository.Find{Track}(IFindable, CancellationToken)"/> and
    /// <see cref="QueryableExtensions.Find{Track}(IFindable, CancellationToken)"/>. E.g.<br/>
    /// <code><![CDATA[var track = await _repository.Find(Track.ById(42), ct);]]></code>
    /// </summary>
    /// <param name="id">The unique identifier for the track.</param>
    public static IFindable ById(int Id) => new Findable(Id);
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
    /// <param name="genres">The genres that the track can be categorized under, e.g. "jazz", "fusion".</param>
    /// <param name="trackPersons">The track-person objects listing the personnel on the track.</param>
    /// <param name="createdAt">The date and time when the track was created.</param>
    /// <param name="createdBy">The user or system that created the track.</param>
    /// <param name="updatedAt">The date and time when the track was last updated.</param>
    /// <param name="updatedBy">The user or system that last updated the track.</param>
    public Track(
        TrackId id,
        string title,
        TimeSpan duration,
        IEnumerable<string>? genres = null,
        IEnumerable<TrackPerson>? trackPersons = null,
        DateTime createdAt = default,
        string createdBy = "",
        DateTime updatedAt = default,
        string updatedBy = "")
    {
        Id              = id;
        Title           = title;
        Duration        = duration;
        _genres         = genres is not null ? [.. genres] : [];
        _tracksPersons  = trackPersons is not null ? new HashSet<TrackPerson>(trackPersons, ReferenceEqualityComparer.Instance) : [];
        CreatedAt       = createdAt;
        CreatedBy       = createdBy;
        UpdatedAt       = updatedAt;
        UpdatedBy       = updatedBy;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Track"/> class.
    /// </summary>
    /// <remarks>
    /// This parameterless constructor is required by Entity Framework Core for materializing instances of the <see cref="Track"/>
    /// class from the database. It is intended for use by EF Core and should not be called directly in application code.
    /// </remarks>
    private Track()
    {
    }

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(object? context = null, CancellationToken cancellationToken = default)
        => await new TrackValidator().ValidateAndThrowAsync(this, cancellationToken).ConfigureAwait(false);
    #endregion

    /// <summary>
    /// Adds a person to the track along with their instruments and roles for the track.
    /// </summary>
    /// <param name="person">The person to add to the track.</param>
    /// <param name="roles">A collection of roles associated with the person.</param>
    /// <param name="instruments">A collection of instruments associated with the person.</param>
    /// <returns>The current <see cref="Track"/> instance, allowing for method chaining.</returns>
    public Track AddPerson(
        Person person,
        IEnumerable<string> roles,
        IEnumerable<string> instruments)
    {
        var trackPerson = _tracksPersons.FirstOrDefault(tp => tp.Person == person);

        if (trackPerson is null)
            _tracksPersons.Add(new TrackPerson(person, roles, instruments));

        person.AddRoles(roles);
        person.AddInstruments(instruments);
        _personnel.Add(person);
        return this;
    }
}
