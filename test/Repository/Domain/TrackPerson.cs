namespace vm2.Repository.Domain;

[DebuggerDisplay("{TrackId}-{PersonId}: {PersonName}")]
public readonly record struct TrackPerson : IValidatable
{
    /// <summary>
    /// Gets the person associated with the track from this instance.
    /// </summary>
    public Person Person { get; init; }

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
        IEnumerable<string>? roles,
        IEnumerable<string>? instrumentCodes)
    {
        Person          = person;
        PersonName      = person.Name;
        Roles           = roles?.ToHashSet() ?? [];
        InstrumentCodes = instrumentCodes?.ToHashSet() ?? [];

        if (roles is not null)
            foreach (var role in roles.Except(person.Roles, StringComparer.OrdinalIgnoreCase))
                person.AddRole(role);

        if (instrumentCodes is not null)
            foreach (var instrument in instrumentCodes.Except(person.InstrumentCodes, StringComparer.OrdinalIgnoreCase))
                person.AddInstrument(instrument);
    }

    public async ValueTask Validate(
        object? context = null,
        CancellationToken cancellationToken = default)
        => await new TrackPersonValidator(context as IRepository)
                        .ValidateAndThrowAsync(this, cancellationToken);
}
