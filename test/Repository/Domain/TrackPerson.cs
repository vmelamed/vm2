namespace vm2.Repository.Domain;

[DebuggerDisplay("{Track}-{PersonId}: {PersonName}")]
public class TrackPerson : IValidatable
{
    /// <summary>
    /// Gets the track of an album (can be more than 1, e.g. "The Best Of...").
    /// </summary>
    public Track Track { get; init; }

    /// <summary>
    /// Gets the unique identifier for the track. For use by EF.
    /// </summary>
    internal int TrackId { get; init; }

    /// <summary>
    /// Gets the person associated with the track from this instance.
    /// </summary>
    public Person Person { get; init; }

    /// <summary>
    /// Gets the unique identifier for a person. For use by EF.
    /// </summary>
    internal int PersonId { get; init; }

    /// <summary>
    /// Caches the name of the person to avoid excessive loading of Person instances.
    /// </summary>
    public string PersonName { get; init; }

    /// <summary>
    /// Gets the set of roles that the person has on the track.
    /// </summary>
    public HashSet<string> Roles { get; init; } = [];

    /// <summary>
    /// Gets the set of instruments that the person plays on the track.
    /// </summary>
    public HashSet<string> InstrumentCodes { get; init; } = [];

    public TrackPerson(
        Person person,
        Track track,
        IEnumerable<string> roles,
        IEnumerable<string> instrumentCodes)
    {
        Person          = person;
        PersonId        = person.Id;
        PersonName      = person.Name;
        Track           = track;
        TrackId         = track.Id;
        Roles           = [.. roles];
        InstrumentCodes = [.. instrumentCodes];

        foreach (var role in roles.Except(person.Roles, StringComparer.OrdinalIgnoreCase).ToList())
            person.AddRole(role);

        foreach (var instrument in instrumentCodes.Except(person.InstrumentCodes, StringComparer.OrdinalIgnoreCase).ToList())
            person.AddInstrumentCode(instrument);
    }

    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new TrackPersonValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken);
}
