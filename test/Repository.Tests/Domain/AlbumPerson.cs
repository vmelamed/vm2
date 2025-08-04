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
public record AlbumPerson(Album Album, Person Person) : IFindable<AlbumPerson>, IValidatable
{
    HashSet<string> _roles = [];
    HashSet<string> _instruments = [];

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

    public AlbumPerson(
        Album album,
        Person person,
        IEnumerable<string> roles,
        IEnumerable<string> instruments)
        : this(album, person)
    {
        _roles       = [.. roles];
        _instruments = [.. instruments];
    }

    public AlbumPerson AddRoles(IEnumerable<string> roles)
    {
        if (_roles.Aggregate(false, (acc, role) => acc |= _roles.Add(role)))
            // if there are any new roles added to the collection, add them to the person as well
            foreach (var role in roles)
                Person.AddRole(role);

        return this;
    }

    public AlbumPerson AddInstruments(IEnumerable<string> instruments)
    {
        if (_instruments.Aggregate(false, (acc, instrument) => acc |= _instruments.Add(instrument)))
            // if there are any new instruments added to the collection, add them to the person as well
            foreach (var instrument in instruments)
                Person.AddInstrument(instrument);

        return this;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumPerson"/> class.
    /// </summary>
    /// <remarks>
    /// This parameterless constructor is required by Entity Framework Core for materializing instances of the <see cref="AlbumPerson"/>
    /// class from the database. It is intended for use by EF Core and should not be called directly in application code.
    /// </remarks>
    private AlbumPerson()
        : this(null!, null!)
    {
    }

    #region IValidatable
    /// <inheritdoc />
    public async ValueTask ValidateFindable(object? context, CancellationToken ct)
        => await new AlbumPersonFindableValidator(context)
            .ValidateAndThrowAsync(this, ct);

    /// <inheritdoc />
    public async ValueTask Validate(object? context, CancellationToken ct)
        => await new AlbumPersonValidator(context)
            .ValidateAndThrowAsync(this, ct);
    #endregion
}
