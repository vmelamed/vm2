namespace vm2.Repository.TestDomain.Validators;

using vm2.Repository.EfRepository;

class AlbumTrackInvariantValidator : AbstractValidator<AlbumTrack>
{
    public AlbumTrackInvariantValidator(bool lazyLoading = false)
    {
        if (lazyLoading)
            return;

        RuleFor(at => at.Track)
            .NotNull()
            .WithMessage("AlbumTrack must not be null.")
            ;
    }
}
class AlbumTrackValidator : AbstractValidator<AlbumTrack>
{
    public AlbumTrackValidator(IRepository? repository = null)
        => Include(new AlbumTrackInvariantValidator(repository?.IsLazyLoadingEnabled<AlbumTrack>() is true));
}