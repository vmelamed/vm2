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
public class AlbumPerson : IFindable<AlbumPerson>, IValidatable, IEquatable<AlbumPerson>
{
    HashSet<string> _roles = [];
    HashSet<string> _instruments = [];

    /// <summary>
    /// Gets the album associated with the person.
    /// </summary>
    public Album Album { get; private set; } = null!;

    /// <summary>
    /// Gets the person associated with the album.
    /// </summary>
    public Person Person { get; private set; } = null!;

    /// <summary>
    /// Gets the unique identifier of the album.
    /// </summary>
    public uint AlbumId { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the person.
    /// </summary>
    public uint PersonId { get; private set; }

    /// <summary>
    /// Gets the collection of roles performed by the person on the associated album.
    /// </summary>
    public IEnumerable<string> Roles => _roles;

    /// <summary>
    /// Gets the collection of roles performed by the person on the associated album.
    /// </summary>
    public IEnumerable<string> Instruments => _instruments;

    #region IFindable<AlbumPerson>
    /// <inheritdoc />
    public static Expression<Func<AlbumPerson, object?>> KeyExpression => ap => new { ap.AlbumId, ap.PersonId };
    #endregion

    /// <summary>
    /// Associates an album with a person and specifies their roles and instruments.
    /// </summary>
    /// <param name="album">The album to associate with the person.</param>
    /// <param name="person">The person to associate with the album.</param>
    /// <param name="roles">An optional collection of roles that the person has in relation to the album, such as "Producer" or "Composer."
    /// If <see langword="null"/>, no roles are assigned.</param>
    /// <param name="instruments">A collection of instruments played by the person on the album, such as "Guitar" or "Piano." Cannot be <see
    /// langword="null"/>.</param>
    public AlbumPerson(
        Album album,
        Person person,
        IEnumerable<string>? roles = null,
        IEnumerable<string>? instruments = null)
    {
        Album        = album;
        Person       = person;
        AlbumId      = album.Id;
        PersonId     = person.Id;
        _roles       = roles is not null ? [.. roles] : [];
        _instruments = instruments is not null ? [.. instruments] : [];
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

    public AlbumPerson AddRoles(IEnumerable<string> roles)
    {
        if (_roles.Aggregate(false, (acc, role) => acc |= _roles.Add(role)))
            // if there were any new roles added to the entity, add them to the person as well
            Person.AddRoles(roles);

        return this;
    }

    public AlbumPerson AddInstruments(IEnumerable<string> instruments)
    {
        if (_instruments.Aggregate(false, (acc, instrument) => acc |= _instruments.Add(instrument)))
            // if there are any new instruments added to the collection, add them to the person as well
            Person.AddInstruments(instruments);

        return this;
    }

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask ValidateFindable(object? context, CancellationToken ct)
        => await new AlbumPersonFindableValidator(context)
            .ValidateAndThrowAsync(this, ct).ConfigureAwait(false);

    /// <inheritdoc />
    public async ValueTask Validate(object? context, CancellationToken ct)
        => await new AlbumPersonValidator(context)
            .ValidateAndThrowAsync(this, ct).ConfigureAwait(false);
    #endregion

    #region Identity rules implementation.
    #region IEquatable<AlbumPerson> Members
    /// <summary>
    /// Indicates whether the current object is equal to a reference to another object of the same type.
    /// </summary>
    /// <param name="other">A reference to another object of type <see cref="AlbumPerson"/> to compare with the current object.</param>
    /// <returns><list type="number">
    ///     <item><see langword="false"/> if <paramref name="other"/> is equal to <see langword="null"/>, otherwise</item>
    ///     <item><see langword="true"/> if <paramref name="other"/> refers to <c>this</c> object, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="other"/> is not the same type as <c>this</c> object, otherwise</item>
    ///     <item><see langword="true"/> if the current object and the <paramref name="other"/> are considered to be equal,
    ///                                  e.g. their business identities are equal; otherwise, <see langword="false"/>.</item>
    /// </list></returns>
    public virtual bool Equals(AlbumPerson? other)
        => other is not null
           && (ReferenceEquals(this, other)
               || typeof(AlbumPerson) == other.GetType()
                  && AlbumId          == other.AlbumId
                  && PersonId         == other.PersonId);
    #endregion

    /// <summary>
    /// Determines whether this <see cref="AlbumPerson"/> instance is equal to the specified <see cref="object"/> reference.
    /// </summary>
    /// <param name="obj">The <see cref="object"/> reference to compare with this <see cref="AlbumPerson"/> object.</param>
    /// <returns><list type="number">
    ///     <item><see langword="false"/> if <paramref name="obj"/> cannot be cast to <see cref="AlbumPerson"/>, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="obj"/> is equal to <see langword="null"/>, otherwise</item>
    ///     <item><see langword="true"/> if <paramref name="obj"/> refers to <c>this</c> object, otherwise</item>
    ///     <item><see langword="false"/> if <paramref name="obj"/> is not the same type as <c>this</c> object, otherwise</item>
    ///     <item><see langword="true"/> if the current object and the <paramref name="obj"/> are considered to be equal,
    ///                                  e.g. their business identities are equal; otherwise, <see langword="false"/>.</item>
    /// </list></returns>
    public override bool Equals(object? obj)
        => Equals(obj as AlbumPerson);

    /// <summary>
    /// Serves as a hash function for the objects of <see cref="AlbumPerson"/> and its derived types.
    /// </summary>
    /// <returns>A hash code for the current <see cref="AlbumPerson"/> instance.</returns>
    public override int GetHashCode()
        => HashCode.Combine(typeof(AlbumPerson), AlbumId, PersonId);

    /// <summary>
    /// Compares two <see cref="AlbumPerson"/> objects.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// <see langword="true"/> if the objects are considered to be equal (<see cref="Equals(AlbumPerson)"/>);
    /// otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator ==(AlbumPerson left, AlbumPerson right)
        => left is null
                ? right is null
                : left.Equals(right);

    /// <summary>
    /// Compares two <see cref="AlbumPerson"/> objects.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>
    /// <see langword="true"/> if the objects are not considered to be equal (<see cref="Equals(AlbumPerson)"/>);
    /// otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator !=(AlbumPerson left, AlbumPerson right)
        => !(left==right);
    #endregion

}
