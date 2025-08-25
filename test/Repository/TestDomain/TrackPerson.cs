namespace vm2.Repository.TestDomain;

/// <summary>
/// Represents the many-to-many association between a track (the owning entity) and a person, including their roles and instruments on the track.
/// </summary>
/// <remarks>This record encapsulates the relationship between a person and a track, providing details such as the person's name,
/// roles, and instruments.<br/>F
/// It is designed to be immutable and supports validation through the
/// <see cref="IValidatable"/> interface.<br/>
/// TODO: replace with struct when available in, <see href="https://github.com/dotnet/efcore/issues/31237">EF 10(+?)</see>.
/// </remarks>
/// <param name="Person">Gets the person associated with the owning track.</param>
/// <param name="Name">Caches the name of the person to avoid excessive loading of Person instances.</param>
[DebuggerDisplay("TrackPerson: {PersonId}: {PersonName}")]
public record TrackPerson(Person Person, string Name) : IValidatable
{
    HashSet<string> _roles = [];
    HashSet<string> _instruments = [];

    ///// <summary>
    ///// Gets the unique identifier of the person.
    ///// </summary>
    // Shadow foreign key by convention:
    //public int PersonId { get; private set; }

    /// <summary>
    /// Gets the set of roles that the person has on the track.
    /// </summary>
    public IEnumerable<string> Roles => _roles;

    /// <summary>
    /// Gets the set of instrument codes that the person plays on the track.
    /// </summary>
    public IEnumerable<string> Instruments => _instruments;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrackPerson"/> class, associating the owning track with a person, their name, roles, and
    /// instruments.
    /// </summary>
    /// <remarks>This constructor allows specifying roles and instruments for the person being tracked. If
    /// either <paramref name="roles"/> or <paramref name="instruments"/> is <see langword="null"/>, they will default
    /// to empty collections.</remarks>
    /// <param name="person">The person on the track. Cannot be <see langword="null"/>.</param>
    /// <param name="roles">A collection of roles assigned to the person. If <see langword="null"/>, an empty collection is used.</param>
    /// <param name="instruments">A collection of instruments associated with the person. If <see langword="null"/>, an empty collection is used.</param>
    public TrackPerson(
        Person Person,
        string name,
        IEnumerable<string>? roles = null,
        IEnumerable<string>? instruments = null)
        : this(Person, name)
    {
        _roles       = roles is not null ? [.. roles] : [];
        _instruments = instruments is not null ? [.. instruments] : [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrackPerson"/> class, associating the owning track with a person, their name, roles, and
    /// instruments.
    /// </summary>
    /// <remarks>This constructor allows specifying roles and instruments for the person being tracked. If
    /// either <paramref name="roles"/> or <paramref name="instruments"/> is <see langword="null"/>, they will default
    /// to empty collections.</remarks>
    /// <param name="person">The person to be tracked. Cannot be <see langword="null"/>.</param>
    /// <param name="name">The name associated with the person. Cannot be <see langword="null"/> or empty.</param>
    /// <param name="roles">A collection of roles assigned to the person. If <see langword="null"/>, an empty collection is used.</param>
    /// <param name="instruments">A collection of instruments associated with the person. If <see langword="null"/>, an empty collection is used.</param>
    public TrackPerson(
        Person person,
        IEnumerable<string>? roles = null,
        IEnumerable<string>? instruments = null)
        : this(person, person.Name)
    {
        _roles       = roles is not null ? [.. roles] : [];
        _instruments = instruments is not null ? [.. instruments] : [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TrackPerson"/> class.
    /// </summary>
    /// <remarks>
    /// This parameterless constructor is required by Entity Framework Core for materializing instances of the <see cref="TrackPerson"/>
    /// class from the database. It is intended for use by EF Core and should not be called directly in application code.
    /// </remarks>
    public TrackPerson()
        : this(null!, "")
    {
    }

    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new TrackPersonValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken).ConfigureAwait(false);

    public TrackPerson AddRole(string role)
    {
        _roles.Add(role);
        _ = Person?.AddRoles([role]) ?? throw new InvalidOperationException("The person entity is not loaded");
        return this;
    }

    public TrackPerson RemoveRole(
        string role)
    {
        _roles.Remove(role);
        return this;
    }

    public TrackPerson AddInstrument(
        string
        instrumentCode)
    {
        _instruments.Add(instrumentCode);
        _ = Person?.AddInstruments([instrumentCode]) ?? throw new InvalidOperationException("The person entity is not loaded");
        return this;
    }

    public TrackPerson RemoveInstrument(
        string instrumentCode)
    {
        _instruments.Remove(instrumentCode);
        return this;
    }
}
