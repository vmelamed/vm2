namespace vm2.Repository.Domain.Validators;

class TrackInvariantValidator : AbstractValidator<Track>
{
    public TrackInvariantValidator()
    {
        RuleFor(track => track.Title)
            .NotEmpty()
            .WithMessage("The track title must not be null or empty.")
            ;

        RuleFor(track => track.Duration)
            .Must(duration => duration.TotalSeconds >= 0)
            .WithMessage("Duration must be non-negative.")
            ;

        RuleFor(track => track.Personnel)
            .NotNull()
            .WithMessage("Personnel must not be null.")
            ;

        RuleForEach(track => track.Personnel)
            .SetValidator(new TrackPersonValidator())
            .WithMessage("Invalid personnel in the track.")
            ;
    }
}

class TrackFindableValidator : AbstractValidator<Track>
{
    public TrackFindableValidator()
    {
        Include(new FindableValidator(Track.KeyExpression));
        RuleFor(track => track.Id)
            .GreaterThan(0)
            .WithMessage("Track ID must be a positive number.")
            ;
    }
}

class TrackValidator : AbstractValidator<Track>
{
    public TrackValidator(IRepository? repository = null)
    {
        Include(new TrackInvariantValidator());
        Include(new TrackFindableValidator());
        Include(new AuditableValidator());
        Include(new SoftDeletableValidator());

        if (repository is null)
            return;

        RuleFor(track => track.Id)
            .MustAsync(async (track, id, ct) => await IsValid(repository, track, id, ct))
            .WithMessage("The track ID must be unique.")
            ;

        RuleForEach(track => track.Personnel)
            .SetValidator(new TrackPersonValidator(repository))
            .WithMessage("Invalid personnel in the track.")
            ;

        When(
            track => track.Album is not null,
            () => RuleFor(track => track.Album!)
                    .SetValidator(new AlbumValidator(repository))
                    .When(track => track.Album != null)
                    .WithMessage("Album must be valid.")
            );
    }

    static async Task<bool> IsValid(
        IRepository repository,
        Track track,
        int id,
        CancellationToken cancellationToken)
        => repository.StateOf(track) switch {
            // If the track is being added, the ID must not exist in the database.
            EntityState.Added => !await repository
                                            .Set<Track>()
                                            .AnyAsync(t => t.Id == id, cancellationToken)
                                            ,

            // If the track is being modified, the ID must exist in the database.
            EntityState.Modified => await repository
                                            .Set<Track>()
                                            .AnyAsync(t => t.Id == id, cancellationToken),
            _ => true,
        };
}
