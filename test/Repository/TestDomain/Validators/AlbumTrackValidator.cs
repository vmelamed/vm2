namespace vm2.Repository.TestDomain.Validators;

using vm2.Repository.EfRepository;

class AlbumTrackMinimalValidator : AbstractValidator<AlbumTrack>
{
    public AlbumTrackMinimalValidator(bool lazyLoading = false)
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
        => Include(new AlbumTrackMinimalValidator(repository?.IsLazyLoadingEnabled<AlbumTrack>() is true));
}