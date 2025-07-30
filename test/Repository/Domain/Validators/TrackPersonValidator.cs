namespace vm2.Repository.Domain.Validators;

class TrackPersonValidator : AbstractValidator<TrackPerson>
{
    public TrackPersonValidator(IRepository? repository = null)
    {
        RuleFor(ta => ta.Track)
            .NotNull()
            .WithMessage("Track must not be null.")
            ;

        RuleFor(ta => ta.Person)
            .NotNull()
            .WithMessage("Person must not be null.")
            ;

        RuleFor(ta => ta.PersonName)
            .NotEmpty()
            .WithMessage("Person name must not be null or empty.")
            ;

        RuleFor(ta => ta.Roles)
            .NotEmpty()
            .Must(roles => roles.All(t => !string.IsNullOrEmpty(t)))
            .WithMessage("Roles must not be null. If not empty, the individual roles must not be null or empty.")
            ;

        RuleFor(ta => ta.InstrumentCodes)
            .NotNull()
            .Must(instruments => instruments.All(i => !string.IsNullOrEmpty(i)))
            .WithMessage("Instruments must not be null. If not empty, the individual instruments must not be null or empty.")
            ;

        if (repository is null)
            return;

        // Make sure the person exists in the database.
        RuleFor(ta => ta.Person)
            .SetValidator(new PersonValidator(repository))
                .When(ta => repository.Entry(ta).Reference(nameof(TrackPerson.Person)).IsLoaded)
            .WithMessage("Invalid person.")
            ;

        // Make sure all the assigned roles exist in the database.
        RuleFor(ta => ta.Roles)
            .MustAsync(async (ta, r, ct) => await AreValidRoles(repository, ta, r, ct))
            .WithMessage("The roles must exist.")
            ;

        // Make sure all the assigned instruments exist in the database.
        RuleFor(ta => ta.InstrumentCodes)
            .MustAsync(async (ta, i, ct) => await AreValidInstruments(repository, ta, i, ct))
            .WithMessage("The roles must exist.")
            ;
    }

    static async ValueTask<bool> AreValidRoles(
        IRepository repository,
        TrackPerson trackPerson,
        HashSet<string> roles,
        CancellationToken ct)
        => repository.StateOf(trackPerson) switch {
            // If the track-person is being added, the roles must exist in the database.
            EntityState.Added => (await repository.Set<Role>().CountAsync(r => roles.Contains(r.Name), ct)) == roles.Count,

            // If the track artist is being modified, the Roles collection is either not modified or the roles must exist in the database.
            EntityState.Modified => !repository.Entry(trackPerson).Collection(nameof(TrackPerson.Roles)).IsModified ||
                                    (await repository.Set<Role>().CountAsync(r => roles.Contains(r.Name), ct)) == roles.Count,

            _ => true,
        };

    static async Task<bool> AreValidInstruments(
        IRepository repository,
        TrackPerson trackArtist,
        HashSet<string> instruments,
        CancellationToken ct)
        => repository.StateOf(trackArtist) switch {
            // If the track artist is being added, the instruments must exist in the database.
            EntityState.Added => (await repository.Set<Instrument>().CountAsync(r => instruments.Contains(r.Code), ct)) == instruments.Count,

            // If the track artist is being modified, the Instruments collection is either not modified or the instruments must exist in the database.
            EntityState.Modified => !repository.Entry(trackArtist).Collection(nameof(TrackPerson.Roles)).IsModified ||
                                    (await repository.Set<Instrument>().CountAsync(r => instruments.Contains(r.Code), ct)) == instruments.Count,

            _ => true,
        };
}
