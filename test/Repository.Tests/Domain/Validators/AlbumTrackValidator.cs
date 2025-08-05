namespace vm2.Repository.Tests.Domain.Validators;

class AlbumTrackValidator : AbstractValidator<AlbumTrack>
{
    public AlbumTrackValidator(IRepository? _ = null)
    {
        RuleFor(at => at.TrackId)
            .Must(tid => tid > 0)
            .WithMessage("AlbumTrack must have a valid track ID.")
            ;
    }
}