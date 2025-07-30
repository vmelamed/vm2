namespace vm2.Repository.Domain.Validators;

class AlbumInvariantValidator : AbstractValidator<Album>
{
    public AlbumInvariantValidator()
    {
        RuleFor(a => a.Title)
            .NotEmpty()
            .WithMessage("Title must not be null or empty.")
            ;

        RuleFor(a => a.ReleaseYear)
            .Must(releaseYear => releaseYear is null || releaseYear.Value >= 1900)
            .WithMessage("The release year must be equal or greater than 1900.")
            ;

        RuleFor(a => a.Personnel)
            .NotEmpty()
            .Must(ps => ps.All(a => a is not null))
            .WithMessage("Personnel must not be null or empty.")
            ;

        RuleFor(a => a.Label)
            .NotNull()
            .WithMessage("Label must not be null.")
            ;

        RuleFor(a => a.LabelId)
            .GreaterThan(0)
            .WithMessage("Label must not be null.")
            ;

        RuleFor(a => a.Tracks)
            .NotEmpty()
            .WithMessage("Tracks must not be null or empty.")
            ;

        RuleForEach(a => a.Tracks)
            .SetValidator(new TrackValidator())
            .WithMessage("Invalid track in the album.")
            ;
    }
}

class AlbumFindableValidator : AbstractValidator<Album>
{
    public AlbumFindableValidator()
    {
        Include(new FindableValidator(Album.KeyExpression));
        RuleFor(a => a.Id)
            .GreaterThan(0)
            .WithMessage("Album ID must be a positive number.")
            ;
    }
}

class AlbumValidator : AbstractValidator<Album>
{
    public AlbumValidator(IRepository? repository = null)
    {
        Include(new AlbumInvariantValidator());
        Include(new AlbumFindableValidator());
        Include(new AuditableValidator());
        Include(new SoftDeletableValidator());

        if (repository is null)
            return;
    }
}
