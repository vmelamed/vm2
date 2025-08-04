namespace vm2.Repository.Tests.Domain;

/// <summary>
/// Represents the many-to-many association between a track (the owning entity) and a person, including their roles and instruments on the track.
/// </summary>
/// <remarks>This record encapsulates the relationship between a person and a track, providing details such as the
/// person's name, roles, and instruments. It is designed to be immutable and supports validation through the <see
/// cref="IValidatable"/> interface.</remarks>
/// <param name="Person">Gets the person associated with the owning track.</param>
/// <param name="Name">Caches the name of the person to avoid excessive loading of Person instances.</param>
[DebuggerDisplay("{TrackId}-{PersonId}: {PersonName}")]
public record TrackPerson(Person Person, string Name) : IValidatable
{
    HashSet<string> _roles = [];
    HashSet<string> _instruments = [];

    /// <summary>
    /// Gets the unique identifier for the person.
    /// </summary>
    public uint PersonId { get; private set; }

    /// <summary>
    /// Gets the set of roles that the person has on the track.
    /// </summary>
    public ICollection<string> Roles => _roles;

    /// <summary>
    /// Gets the set of instrument codes that the person plays on the track.
    /// </summary>
    public ICollection<string> Instruments => _instruments;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrackPerson"/> class.
    /// </summary>
    /// <remarks>
    /// This parameterless constructor is required by Entity Framework Core for materializing instances of the <see cref="TrackPerson"/>
    /// class from the database. It is intended for use by EF Core and should not be called directly in application code.
    /// </remarks>
    private TrackPerson()
        : this(null!, null!)
    {
    }

    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new TrackPersonValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken);

    public TrackPerson AddRole(string role)
    {
        _roles.Add(role);
        Person.AddRole(role);
        return this;
    }

    public TrackPerson RemoveRole(
        string role,
        bool removeFromPerson = false)
    {
        _roles.Remove(role);
        if (removeFromPerson)
            Person.RemoveRole(role);
        return this;
    }

    public TrackPerson AddInstrument(
        string
        instrumentCode)
    {
        _instruments.Add(instrumentCode);
        Person.AddInstrument(instrumentCode);
        return this;
    }

    public TrackPerson RemoveInstrument(
        string instrumentCode,
        bool removeFromPerson = false)
    {
        _instruments.Remove(instrumentCode);
        if (removeFromPerson)
            Person.RemoveInstrument(instrumentCode);
        return this;
    }
}
