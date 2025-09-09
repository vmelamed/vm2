namespace vm2.Repository.TestDomain;
using System;

using vm2.Repository.EntityFramework;

[DebuggerDisplay("Person {id}: {Name}")]
public class Person :
    ITenanted<Guid>,
    IAggregate<Album>,
    IAuditable,
    IOptimisticConcurrency<byte[]>,
    IValidatable,
    IFindable<Person>
{
    public const int MaxNameLength = 100;

    HashSet<string> _roles = [];
    HashSet<string> _instruments = [];
    HashSet<string> _genres = [];
    HashSet<Album> _albums = new(ReferenceEqualityComparer.Instance);
    HashSet<AlbumPerson> _personsAlbums = new(ReferenceEqualityComparer.Instance);

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public PersonId Id { get; private set; }

    /// <summary>
    /// Gets or sets the unique identifier for the tenant.
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// Gets or sets the names of the person.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The birth year of the person, or <see langword="null"/> if unknown.
    /// </summary>
    public int? BirthYear { get; set; } = null;

    /// <summary>
    /// The death year of the person, or <see langword="null"/> if the person is still alive or the year is unknown.
    /// </summary>
    public int? DeathYear { get; set; } = null;

    /// <summary>
    /// Gets the roles that the person is known to have performed, e.g. performer, conductor, etc.
    /// </summary>
    public IEnumerable<string> Roles => _roles;

    /// <summary>
    /// Gets the instrument codes that the person is known to have played, e.g. "tp" for trumpet, "g" for guitar, etc.
    /// </summary>
    public IEnumerable<string> Instruments => _instruments;

    /// <summary>
    /// Gets the genres that the person is known to have worked in, e.g. jazz, classical, etc.
    /// </summary>
    public IEnumerable<string> Genres => _genres;

    /// <summary>
    /// Gets the collection of personnel associated with the album, such as musicians, producers, or other contributors.
    /// </summary>
    public IEnumerable<AlbumPerson> PersonsAlbums => _personsAlbums;

    /// <summary>
    /// Gets the collection of albums that the person appears on. Discography.
    /// </summary>
    public IEnumerable<Album> Albums => _albums;

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

    #region IOptimisticConcurrency<byte[]>
    /// <inheritdoc />
    public byte[] ETag { get; set; } = [];
    #endregion

    #region IFindable<Person>
    /// <inheritdoc />
    public static Expression<Func<Person, object?>> KeyExpression => p => new { p.Id, p.TenantId };

    /// <inheritdoc />
    public async ValueTask ValidateFindableAsync(object? _, CancellationToken ct)
        => await new PersonFindableValidator().ValidateAndThrowAsync(this, ct);

    /// <summary>
    /// Returns a struct implementing <see cref="IFindable"/> that can be used to find a <see cref="Person"/> by its unique
    /// identifier. Can be used in <see cref="IRepository.Find{Person}(IFindable, CancellationToken)"/> and
    /// <see cref="QueryableExtensions.Find{Person}(IFindable, CancellationToken)"/>. E.g.<br/>
    /// <c><![CDATA[var person = await _repository.FindAsync(Person.ById(42), ct);]]></c>
    /// </summary>
    /// <param name="id">The unique identifier for the person.</param>
    public static IFindable ById(int id, Guid tenantId) => new Findable(id, tenantId);
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="Person"/> class with specified details.
    /// </summary>
    /// <param name="id">The unique identifier for the person.</param>
    /// <param name="tenantId">The unique identifier for the tenant who owns the current entity.</param>
    /// <param name="name">The name of the person. Cannot be null or empty.</param>
    /// <param name="birthYear">The birth year of the person.</param>
    /// <param name="deathYear">The death year of the person.</param>
    /// <param name="roles">A collection of roles associated with the person.</param>
    /// <param name="instruments">A collection of instruments the person plays.</param>
    /// <param name="genres">A collection of genres associated with the person.</param>
    /// <param name="personsAlbums">A collection of album-person-s related to the person.</param>
    /// <param name="createdAt">The date and time when the person entity was created.</param>
    /// <param name="createdBy">The identifier of the actor who created the person entity.</param>
    /// <param name="updatedAt">The date and time when the person entity was last updated.</param>
    /// <param name="updatedBy">The identifier of the user who last updated the person entity.</param>
    /// <param name="etag">The entity tag (ETag) for optimistic concurrency control.</param>
    public Person(
        PersonId id,
        Guid tenantId,
        string name,
        int? birthYear = null,
        int? deathYear = null,
        IEnumerable<string>? roles = null,
        IEnumerable<string>? instruments = null,
        IEnumerable<string>? genres = null,
        IEnumerable<AlbumPerson>? personsAlbums = null,
        DateTime createdAt = default,
        string createdBy = "",
        DateTime updatedAt = default,
        string updatedBy = "",
        byte[]? etag = default)
    {
        Id           = id;
        TenantId     = tenantId;
        Name         = name;
        BirthYear    = birthYear;
        DeathYear    = deathYear;
        _genres      = genres?.Select(g => g.Trim().ToLower()).ToHashSet() ?? [];
        _roles       = roles?.Select(r => r.Trim().ToLower()).ToHashSet() ?? [];
        _instruments = instruments?.Select(i => i.Trim().ToLower()).ToHashSet() ?? [];
        CreatedAt    = createdAt;
        CreatedBy    = createdBy;
        UpdatedAt    = updatedAt;
        UpdatedBy    = updatedBy;
        ETag         = etag ?? [];

        foreach (var _ in personsAlbums?.Select(AddAlbum) ?? [])
            ;

        new PersonMinimalValidator().ValidateAndThrow(this);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Person"/> class.
    /// </summary>
    /// <remarks>
    /// This parameterless constructor is required by Entity Framework Core for materializing instances of the <see cref="Person"/>
    /// class from the database. It is intended for use by EF Core and should not be called directly in application code.
    /// </remarks>
    private Person()
    {
    }

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask ValidateAsync(
        object? context = null,
        CancellationToken ct = default)
        => await new PersonValidator(context as IRepository).ValidateAndThrowAsync(this, ct).ConfigureAwait(false);
    #endregion

    /// <summary>
    /// Adds a role to the current person and returns the updated person.
    /// </summary>
    /// <param name="role">The role to add to the person. Cannot be null or empty.</param>
    /// <returns>The current person with the new role added.</returns>
    public Person AddRoles(IEnumerable<string> roles)
    {
        foreach (var role in roles)
            _roles.Add(role);
        return this;
    }

    /// <summary>
    /// Adds a musical instrument to the person's collection of instrument codes.
    /// </summary>
    /// <param name="instrumentCode">The code representing the musical instrument to add. Cannot be null or empty.</param>
    /// <returns>The current instance of <see cref="Person"/> with the updated instrument collection.</returns>
    public Person AddInstruments(IEnumerable<string> instrumentCodes)
    {
        foreach (var instrument in instrumentCodes)
            _instruments.Add(instrument);
        return this;
    }

    /// <summary>
    /// Adds a specified genre to the list of genres associated with the person.
    /// </summary>
    /// <param name="genre">The genre to add. Cannot be null or empty.</param>
    /// <returns>The current instance of <see cref="Person"/> with the updated list of genres.</returns>
    public Person AddGenres(IEnumerable<string> genres)
    {
        foreach (var genre in genres)
            _genres.Add(genre);
        return this;
    }

    /// <summary>
    /// Adds an album and its associated roles and instruments to the current person.
    /// </summary>
    /// <param name="albumPerson">An object containing the album, roles, and instruments associated with the person.</param>
    /// <returns>The current <see cref="Person"/> instance, allowing for method chaining.</returns>
    internal Person AddAlbum(AlbumPerson albumPerson)
    {
        // Do we really need these?
        _personsAlbums.Add(albumPerson);
        _albums.Add(albumPerson.Album);

        // AddAsync roles, instruments, and genres from the albumPerson to the person
        AddRoles(albumPerson.Roles);
        AddInstruments(albumPerson.Instruments);
        AddGenres(albumPerson.Genres);
        return this;
    }
}