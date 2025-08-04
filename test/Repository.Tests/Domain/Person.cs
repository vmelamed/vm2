namespace vm2.Repository.Tests.Domain;
using System;

public class Person : IFindable<Person>, IAuditable, IValidatable, IEquatable<Person>
{
    public const int MaxNameLength = 100;

    HashSet<string> _roles = [];
    HashSet<string> _genres = [];
    HashSet<string> _instruments = [];
    HashSet<Album> _albums = [];
    HashSet<AlbumPerson> _personsAlbums = [];

    /// <summary>
    /// Gets or sets the unique identifier for the entity.
    /// </summary>
    public uint Id { get; private set; }

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
    /// Gets or sets the collection (set) of roles associated with the person, e.g. performer, conductor, etc.
    /// </summary>
    public IEnumerable<string> Roles => _roles;

    /// <summary>
    /// Gets or sets the collection of genres that the person is known to have worked in, e.g. jazz, classical, etc.
    /// </summary>
    public IEnumerable<string> Genres => _genres;

    /// <summary>
    /// Gets or sets the collection of instrument codes that the person is known to have played, e.g. "tp" for trumpet, "g" for guitar, etc.
    /// </summary>
    public IEnumerable<string> Instruments => _instruments;

    /// <summary>
    /// Gets the collection of albums that the person appears on. Discography.
    /// </summary>
    public IEnumerable<Album> Albums => _albums;

    /// <summary>
    /// Gets the collection of personnel associated with the album, such as musicians, producers, or other contributors.
    /// </summary>
    internal IEnumerable<AlbumPerson> PersonsAlbums => _personsAlbums;

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
    /// Initializes a new instance of the <see cref="Person"/> class with specified details.
    /// </summary>
    /// <param name="id">The unique identifier for the person.</param>
    /// <param name="name">The name of the person. Cannot be null or empty.</param>
    /// <param name="birthYear">The birth year of the person, or <see langword="null"/> if unknown.</param>
    /// <param name="deathYear">
    /// The death year of the person, or <see langword="null"/> if the person is still alive or the year is unknown.
    /// </param>
    /// <param name="roles">
    /// A collection of roles associated with the person. If <see langword="null"/>, an empty collection is used.
    /// </param>
    /// <param name="genres">
    /// A collection of genres associated with the person. If <see langword="null"/>, an empty collection is used.
    /// </param>
    /// <param name="instrumentCodes">
    /// A collection of instruments the person plays. If <see langword="null"/>, an empty collection is used.
    /// </param>
    /// <param name="createdAt">
    /// The date and time when the person record was created. Defaults to <see cref="DateTimeOffset.MinValue"/> if not specified.
    /// </param>
    /// <param name="createdBy">The identifier of the actor who created the person record.</param>
    /// <param name="updatedAt">The date and time when the person record was last updated.</param>
    /// <param name="updatedBy">The identifier of the user who last updated the person record.</param>
    public Person(
        uint id,
        string name,
        int? birthYear = null,
        int? deathYear = null,
        IEnumerable<string>? roles = null,
        IEnumerable<string>? genres = null,
        IEnumerable<string>? instrumentCodes = null,
        DateTimeOffset createdAt = default,
        string createdBy = "",
        DateTimeOffset updatedAt = default,
        string updatedBy = "")
    {
        Id               = id;
        Name             = name;
        BirthYear        = birthYear;
        DeathYear        = deathYear;
        _genres          = genres?.Select(g => g.Trim().ToLower()).ToHashSet() ?? [];
        _roles           = roles?.Select(r => r.Trim().ToLower()).ToHashSet() ?? [];
        _instruments = instrumentCodes?.Select(i => i.Trim().ToLower()).ToHashSet() ?? [];
        CreatedAt        = createdAt;
        CreatedBy        = createdBy;
        UpdatedAt        = updatedAt;
        UpdatedBy        = updatedBy;

        new PersonInvariantValidator()
                .ValidateAndThrow(this);
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

    #region IFindable<Person>
    /// <inheritdoc />
    public static Expression<Func<Person, object?>> KeyExpression => p => new { p.Id };

    /// <inheritdoc />
    public ValueTask ValidateFindable(object? _, CancellationToken __)
    {
        new PersonFindableValidator().ValidateAndThrow(this);
        return ValueTask.CompletedTask;
    }
    #endregion

    /// <summary>
    /// Returns a struct implementing <see cref="IFindable"/> that can be used to find a <see cref="Person"/> by its unique
    /// identifier. Can be used in <see cref="IRepository.Find{Person}(IFindable, CancellationToken)"/> and
    /// <see cref="QueryableExtensions.Find{Person}(IFindable, CancellationToken)"/>. E.g.<br/>
    /// <c><![CDATA[var person = await _repository.Find(Person.ById(42), ct);]]></c>
    /// </summary>
    /// <param name="id">The unique identifier for the person.</param>
    public static IFindable ById(int Id) => new Findable(Id);

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new PersonValidator(context as IRepository).ValidateAndThrowAsync(this, cancellationToken);
    #endregion

    /// <summary>
    /// Adds a role to the current person and returns the updated person.
    /// </summary>
    /// <param name="role">The role to add to the person. Cannot be null or empty.</param>
    /// <returns>The current person with the new role added.</returns>
    public Person AddRole(string role)
    {
        _roles.Add(role);
        return this;
    }

    /// <summary>
    /// Removes the specified role from the person's list of roles.
    /// </summary>
    /// <param name="role">The role to be removed. Cannot be null or empty.</param>
    /// <returns>The current instance of <see cref="Person"/> with the role removed.</returns>
    public Person RemoveRole(string role)
    {
        _roles.Remove(role);
        return this;
    }

    /// <summary>
    /// Adds a musical instrument to the person's collection of instrument codes.
    /// </summary>
    /// <param name="instrumentCode">The code representing the musical instrument to add. Cannot be null or empty.</param>
    /// <returns>The current instance of <see cref="Person"/> with the updated instrument collection.</returns>
    public Person AddInstrument(string instrumentCode)
    {
        _instruments.Add(instrumentCode);
        return this;
    }

    /// <summary>
    /// Removes the specified instrument code from the collection of instrument codes.
    /// </summary>
    /// <param name="instrumentCode">The code of the instrument to remove. Cannot be null or empty.</param>
    /// <returns>The current instance of <see cref="Person"/> with the specified instrument code removed.</returns>
    public Person RemoveInstrument(string instrumentCode)
    {
        _instruments.Remove(instrumentCode);
        return this;
    }

    /// <summary>
    /// Adds a specified genre to the list of genres associated with the person.
    /// </summary>
    /// <param name="genre">The genre to add. Cannot be null or empty.</param>
    /// <returns>The current instance of <see cref="Person"/> with the updated list of genres.</returns>
    public Person AddGenre(string genre)
    {
        _genres.Add(genre);
        return this;
    }

    /// <summary>
    /// Removes the specified genre from the list of genres associated with the person.
    /// </summary>
    /// <param name="genre">The genre to be removed. Cannot be null or empty.</param>
    /// <returns>The current instance of <see cref="Person"/> with the specified genre removed.</returns>
    public Person RemoveGenre(string genre)
    {
        _genres.Remove(genre);
        return this;
    }

    #region Identity rules implementation.
    #region IEquatable<Person> Members
    /// <summary>
    /// Indicates whether the current object is equal to a reference to another object of the same type.
    /// </summary>
    /// <param name="other">A reference to another object of type <see cref="Person"/> to compare with the current object.</param>
    /// <returns><list type="number">
    ///     <item><see langword="false"/> if <paramref name="other"/> is equal to <see langword="null"/>, otherwise</item>
    ///     <item><see langword="true"/> if <paramref name="other"/> refers to <c>this</c> object, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="other"/> is not the same type as <c>this</c> object, otherwise</item>
    ///     <item><see langword="true"/> if the current object and the <paramref name="other"/> are considered to be equal,
    ///                                  e.g. their business identities are equal; otherwise, <see langword="false"/>.</item>
    /// </list></returns>
    public virtual bool Equals(Person? other)
        => other is not null  &&
           (ReferenceEquals(this, other)  ||  GetType() == other.GetType() && Id == other.Id);
    #endregion

    /// <summary>
    /// Determines whether this <see cref="Person"/> instance is equal to the specified <see cref="object"/> reference.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> reference to compare with this <see cref="Person"/> object.</param>
    /// <returns><list type="number">
    ///     <item><see langword="false"/> if <paramref name="obj"/> cannot be cast to <see cref="Person"/>, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="obj"/> is equal to <see langword="null"/>, otherwise</item>
    ///     <item><see langword="true"/> if <paramref name="obj"/> refers to <c>this</c> object, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="obj"/> is not the same type as <c>this</c> object, otherwise</item>
    ///     <item><see langword="true"/> if the current object and the <paramref name="obj"/> are considered to be equal,
    ///                                  e.g. their business identities are equal; otherwise, <see langword="false"/>.</item>
    /// </list></returns>
    public override bool Equals(object? obj) => Equals(obj as Person);

    /// <summary>
    /// Serves as a hash function for the objects of <see cref="Person"/> and its derived types.
    /// </summary>
    /// <returns>A hash code for the current <see cref="Person"/> instance.</returns>
    public override int GetHashCode() => HashCode.Combine(typeof(Person), Id);

    /// <summary>
    /// Compares two <see cref="Person"/> objects.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// <see langword="true"/> if the objects are considered to be equal (<see cref="Equals(Person)"/>);
    /// otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Person left, Person right) => left is null ? right is null : left.Equals(right);

    /// <summary>
    /// Compares two <see cref="Person"/> objects.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// <see langword="true"/> if the objects are not considered to be equal (<see cref="Equals(Person)"/>);
    /// otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Person left, Person right) => !(left==right);
    #endregion
}