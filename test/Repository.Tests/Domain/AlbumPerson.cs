namespace vm2.Repository.Tests.Domain;

/// <summary>
/// Represents the association between an album and a person, including their roles and instruments.
/// </summary>
/// <remarks>
/// This record is used to model the many-to-many relationship between an album and a person, such as a musician or
/// contributor. It includes details about the roles performed and instruments played by the person on the album.
/// </remarks>
/// <param name="Album">Gets the album associated with a person.</param>
/// <param name="Person">Gets the person associated with an album.</param>
[DebuggerDisplay("AlbumPerson {AlbumId-PersonId}")]
public class AlbumPerson : IValidatable, IOptimisticConcurrency
{
    HashSet<string> _roles = [];
    HashSet<string> _instruments = [];
    HashSet<string> _genres = [];

    /// <summary>
    /// Gets the album associated with the person.
    /// </summary>
    public Album Album { get; private set; } = null!;

    /// <summary>
    /// Gets the person associated with the album.
    /// </summary>
    public Person Person { get; private set; } = null!;

    ///// <summary>
    ///// Gets the unique identifier of the album.
    ///// </summary>
    // Shadow foreign key by convention:
    //public int AlbumId { get; private set; }

    ///// <summary>
    ///// Gets the unique identifier of the person.
    ///// </summary>
    // Shadow foreign key by convention:
    //public int PersonId { get; private set; }

    /// <summary>
    /// Gets the collection of roles performed by the person on the album.
    /// </summary>
    public IEnumerable<string> Roles => _roles;

    /// <summary>
    /// Gets the instruments played by the person on the album.
    /// </summary>
    public IEnumerable<string> Instruments => _instruments;

    /// <summary>
    /// Gets the genres that the person plays in on the tracks of the album, e.g. "jazz", "fusion.", "funk"
    /// </summary>
    public IEnumerable<string> Genres => _genres;

    /// <summary>
    /// Associates an album with a person and specifies their roles and instruments.
    /// </summary>
    /// <param name="album">The album to associate with the person.</param>
    /// <param name="person">The person to associate with the album.</param>
    /// <param name="roles">An optional collection of roles that the person has on the album, such as "Producer" or "Composer."</param>
    /// <param name="instruments">A collection of instruments played by the person on the album, such as "Guitar" or "Piano."</param>
    /// <param name="genres">The genres that the person plays in on the tracks of the album.</param>
    public AlbumPerson(
        Album album,
        Person person,
        IEnumerable<string>? roles = null,
        IEnumerable<string>? instruments = null,
        IEnumerable<string>? genres = null)
    {
        Album        = album;
        Person       = person;
        _roles       = roles is not null ? [.. roles] : [];
        _instruments = instruments is not null ? [.. instruments] : [];
        _genres      = genres is not null ? [.. genres] : [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumPerson"/> class.
    /// </summary>
    /// <remarks>
    /// This parameterless constructor is required by Entity Framework Core for materializing instances of the <see cref="AlbumPerson"/>
    /// class from the database. It is intended for use by EF Core and should not be called directly in application code.
    /// </remarks>
    private AlbumPerson()
    {
    }

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(object? context, CancellationToken ct)
        => await new AlbumPersonValidator(context)
            .ValidateAndThrowAsync(this, ct).ConfigureAwait(false);
    #endregion

    public AlbumPerson AddRoles(IEnumerable<string> roles)
    {
        // if there were any new roles added to the entity, add them to the person as well
        if (_roles.Aggregate(false, (acc, role) => acc |= _roles.Add(role)))
            Person.AddRoles(roles);

        return this;
    }

    public AlbumPerson AddInstruments(IEnumerable<string> instruments)
    {
        // if there are any new instruments added to the collection, add them to the person as well
        if (_instruments.Aggregate(false, (acc, instrument) => acc |= _instruments.Add(instrument)))
            Person.AddInstruments(instruments);

        return this;
    }

    public AlbumPerson AddGenres(IEnumerable<string> genres)
    {
        // if there are any new genres added to the collection, add them to the person as well
        if (_genres.Aggregate(false, (acc, genre) => acc |= _genres.Add(genre)))
            Person.AddGenres(genres);

        return this;
    }
}
