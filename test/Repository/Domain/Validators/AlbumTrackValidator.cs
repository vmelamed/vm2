namespace vm2.Repository.Domain.Validators;

class AlbumTrackValidator : AbstractValidator<AlbumTrack>
{
    public AlbumTrackValidator(IRepository? repository = null)
    {
        RuleFor(at => at.Album)
            .NotNull()
            .WithMessage("AlbumTrack must have a valid Album.");

        RuleFor(at => at.Track)
            .NotNull()
            .WithMessage("AlbumTrack must have a valid Track.");

        RuleFor(at => at.OrderNumber)
            .Must(orderNumber => orderNumber > 0)
            .WithMessage("Track order must be greater than zero.");

        // Add repository-dependent rules if needed
        if (repository is null)
            return;

        // TODO: Do we need this?
        RuleFor(at => at)
            .MustAsync(async (at, ct) => await IsValid(repository, at, ct))
            .WithMessage("The album/track combination must be unique.")
            ;
    }

    static async ValueTask<bool> IsValid(
        IRepository repository,
        AlbumTrack albumTrack,
        CancellationToken cancellationToken)
    {
        // If the album track is being added, the combination of Album and Track must not exist in the database.
        return repository.StateOf(albumTrack) switch {
            EntityState.Added => !await repository
                                            .Set<AlbumTrack>()
                                            .AnyAsync(at => at.Album == albumTrack.Album &&
                                                            at.Track == albumTrack.Track, cancellationToken),

            EntityState.Modified => await repository
                                            .Set<AlbumTrack>()
                                            .AnyAsync(at => at.Album == albumTrack.Album &&
                                                            at.Track == albumTrack.Track, cancellationToken),

            _ => true // For modified or deleted states, we assume the combination is valid.
        };
    }
}