﻿namespace vm2.Repository.Domain.Validators;

class TrackInvariantValidator : AbstractValidator<Track>
{
    public TrackInvariantValidator()
    {
        RuleFor(track => track.Title)
            .NotEmpty()
            .WithMessage("The track title must not be null or empty.")
            .MaximumLength(Track.MaxTitleLength)
            .WithMessage($"The track title cannot be longer than {Track.MaxTitleLength} characters.")
            ;

        RuleFor(track => track.Duration)
            .Must(duration => duration.TotalSeconds > 0)
            .WithMessage("Duration must be greater than 0 sec.")
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
        RuleFor(track => track.Id)
            .Must(id => id > 0)
            .WithMessage("Track ID must be greater than 0.")
            ;
        Include(new FindableValidator(Track.KeyExpression));
    }
}

class TrackValidator : AbstractValidator<Track>
{
    public TrackValidator(IRepository? repository = null)
    {
        Include(new TrackInvariantValidator());
        Include(new TrackFindableValidator());
        Include(new AuditableValidator());

        if (repository is null)
            return;

        // Do we want this extra trip to the database, if we have unique DB constraints on the PK Id?
        RuleFor(track => track.Id)
            .MustAsync(async (track, id, ct) => await IsValid(repository, track, id, ct))
            .WithMessage("The track ID must be unique.")
            ;
    }

    static async Task<bool> IsValid(
        IRepository repository,
        Track track,
        uint id,
        CancellationToken cancellationToken)
        => repository.StateOf(track) switch {
            // If the track is being added, the ID must not exist in the database.
            EntityState.Added => !await repository
                                            .Set<Track>()
                                            .AnyAsync(t => t.Id == id, cancellationToken),

            // If the track is being modified, the ID must exist in the database.
            EntityState.Modified => await repository
                                            .Set<Track>()
                                            .AnyAsync(t => t.Id == id, cancellationToken),

            _ => true,
        };
}
