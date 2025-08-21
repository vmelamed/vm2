namespace vm2.Repository.Tests.Domain.Validators;

class TrackInvariantValidator : AbstractValidator<Track>
{
    public TrackInvariantValidator(bool lazyLoading = false)
    {
        RuleFor(track => track.Title)
            .NotEmpty()
            .WithMessage("The track title must not be null or empty.")
            .MaximumLength(Track.MaxTitleLength)
            .WithMessage($"The track title cannot be longer than {Track.MaxTitleLength} characters.")
            ;

        RuleFor(track => track.Duration)
            .Must(duration => duration?.TotalSeconds > 0)
            .WithMessage("Duration must be greater than 0 sec.")
            ;

        if (lazyLoading)
            return;

        RuleFor(a => a.TracksPersons)
            .NotNull()
            .WithMessage("TracksPersons must not be null.")
            .Must(p => p.All(ap => ap is not null))
            .WithMessage("TracksPersons cannot contain null items.")
            ;

        RuleFor(a => a.Personnel)
            .NotNull()
            .WithMessage("Personnel must not be null.")
            .Must(p => p.All(a => a is not null))
            .WithMessage("Personnel cannot contain null items.")
            ;
    }
}

class TrackFindableValidator : AbstractValidator<Track>
{
    public TrackFindableValidator()
    {
        RuleFor(track => track.Id)
            .NotEmpty()
            .WithMessage("Track ID must be greater than 0.")
            ;
    }
}

class TrackValidator : AbstractValidator<Track>
{
    public TrackValidator(IRepository? repository = null)
    {
        Include(new TrackInvariantValidator(repository?.IsLazyLoadingEnabled<Track>() is true));
        Include(new TrackFindableValidator());
        Include(new AuditableValidator());

        RuleForEach(t => t.TracksPersons)
            .SetValidator(new TrackPersonValidator(repository))
            .WithMessage("Invalid TrackPerson in the TracksPersons collection.")
            ;

        RuleForEach(a => a.Personnel)
            .SetValidator(new PersonValidator(repository))
            .WithMessage("Invalid track in the album.")
            ;

        if (repository is null)
            return;

        // Do we want this extra trip to the database, if we have unique DB constraints on the PK Id?
        RuleFor(track => track.Id)
            .MustAsync(async (track, id, ct) => await IsValid(repository, track, id, ct).ConfigureAwait(false))
            .WithMessage("The track ID must be unique.")
            ;
    }

    static async Task<bool> IsValid(
        IRepository repository,
        Track track,
        TrackId id,
        CancellationToken cancellationToken)
        => repository.StateOf(track) switch {
            // If the track is being added, the ID must not exist in the database.
            EntityState.Added => Genre.Has(track.Genres)
                                 && !await repository
                                                .Set<Track>()
                                                .AnyAsync(t => t.Id == id, cancellationToken)
                                                .ConfigureAwait(false),

            // If the track is being modified, the ID must exist in the database.
            EntityState.Modified => Genre.Has(track.Genres)
                                    && await repository
                                                .Set<Track>()
                                                .AnyAsync(t => t.Id == id, cancellationToken)
                                                .ConfigureAwait(false),

            _ => true,
        };
}
