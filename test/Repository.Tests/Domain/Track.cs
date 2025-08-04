namespace vm2.Repository.Tests.Domain;
using System;

[DebuggerDisplay("{Title} ({Duration})")]
public class Track : IFindable<Track>, IAuditable, IValidatable, IEquatable<Track>
{
    public const int MaxTitleLength = 256;

    HashSet<string> _genres = [];
    HashSet<Album> _albums = [];
    HashSet<Person> _personnel = [];
    HashSet<TrackPerson> _tracksPersons = [];

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    internal uint Id { get; private set; }

    /// <summary>
    /// Gets or sets the title of the track (song).
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the duration of the track.
    /// </summary>
    public TimeSpan Duration { get; set; } = default;

    /// <summary>
    /// Gets the collection of genres that this track can be categorized under.
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
    internal IEnumerable<TrackPerson> TracksPersons => _tracksPersons;

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
    /// Initializes a new instance of the <see cref="Track"/> class.
    /// </summary>
    /// <remarks>
    /// This parameterless constructor is required by Entity Framework Core for materializing instances of the <see cref="Track"/>
    /// class from the database. It is intended for use by EF Core and should not be called directly in application code.
    /// </remarks>
    private Track()
    {
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

    public Track AddPerson(Person person, IEnumerable<string> instruments, IEnumerable<string> roles)
    {
        var trackPerson = _tracksPersons.FirstOrDefault(tp => tp.PersonId == person.Id);

        if (trackPerson is null)
        {
            trackPerson = new TrackPerson(this, person);
            _tracksPersons.Add(trackPerson);
        }

        _personnel.Add(person);


        return this;
    }

    #region Identity rules implementation.
    #region IEquatable<Track> Members
    /// <summary>
    /// Indicates whether the current object is equal to a reference to another object of the same type.
    /// </summary>
    /// <param name="other">A reference to another object of type <see cref="Track"/> to compare with the current object.</param>
    /// <returns><list type="number">
    ///     <item><see langword="false"/> if <paramref name="other"/> is equal to <see langword="null"/>, otherwise</item>
    ///     <item><see langword="true"/> if <paramref name="other"/> refers to <c>this</c> object, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="other"/> is not the same type as <c>this</c> object, otherwise</item>
    ///     <item><see langword="true"/> if the current object and the <paramref name="other"/> are considered to be equal,
    ///                                  e.g. their business identities are equal; otherwise, <see langword="false"/>.</item>
    /// </list></returns>
    public virtual bool Equals(Track? other)
        => other is not null
           && (ReferenceEquals(this, other)
               || typeof(Track) == other.GetType()
                  && Id         == other.Id);
    #endregion

    /// <summary>
    /// Determines whether this <see cref="Track"/> instance is equal to the specified <see cref="object"/> reference.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> reference to compare with this <see cref="Track"/> object.</param>
    /// <returns><list type="number">
    ///     <item><see langword="false"/> if <paramref name="obj"/> cannot be cast to <see cref="Track"/>, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="obj"/> is equal to <see langword="null"/>, otherwise</item>
    ///     <item><see langword="true"/> if <paramref name="obj"/> refers to <c>this</c> object, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="obj"/> is not the same type as <c>this</c> object, otherwise</item>
    ///     <item><see langword="true"/> if the current object and the <paramref name="obj"/> are considered to be equal,
    ///                                  e.g. their business identities are equal; otherwise, <see langword="false"/>.</item>
    /// </list></returns>
    public override bool Equals(object? obj)
        => Equals(obj as Track);

    /// <summary>
    /// Serves as a hash function for the objects of <see cref="Track"/> and its derived types.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Track"/> instance.</returns>
    public override int GetHashCode()
        => HashCode.Combine(typeof(Track), Id);

    /// <summary>
    /// Compares two <see cref="Track"/> objects.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// <see langword="true"/> if the objects are considered to be equal (<see cref="Equals(Track)"/>);
    /// otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Track left, Track right)
        => left is null
                ? right is null
                : left.Equals(right);

    /// <summary>
    /// Compares two <see cref="Track"/> objects.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// <see langword="true"/> if the objects are not considered to be equal (<see cref="Equals(Track)"/>);
    /// otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Track left, Track right)
        => !(left==right);
    #endregion
}
