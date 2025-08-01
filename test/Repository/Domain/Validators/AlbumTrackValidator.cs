namespace vm2.Repository.Domain.Validators;

class AlbumTrackValidator : AbstractValidator<AlbumTrack>
{
    public AlbumTrackValidator(IRepository? _)
    {
        RuleFor(at => at.Track)
            .NotNull()
            .WithMessage("AlbumTrack must have a valid Track.");
    }
}