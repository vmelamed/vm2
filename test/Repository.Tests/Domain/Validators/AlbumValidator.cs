namespace vm2.Repository.Tests.Domain.Validators;

class AlbumInvariantValidator : AbstractValidator<Album>
{
    public AlbumInvariantValidator()
    {
        RuleFor(a => a.Title)
            .NotEmpty()
            .WithMessage("Title must not be null or empty.")
            .MaximumLength(Album.MaxTitleLength)
            .WithMessage($"Title cannot be longer than {Album.MaxTitleLength} characters.")
            ;

        RuleFor(a => a.ReleaseYear)
            .Must(releaseYear => releaseYear is null || releaseYear.Value >= 1900)
            .WithMessage("The release year must be equal or greater than 1900.")
            ;

        RuleFor(a => a.Personnel)
            .NotNull()
            .WithMessage("Personnel must not be null.")
            .Must(p => p.All(a => a is not null))
            .WithMessage("Personnel cannot contain null items.")
            ;

        RuleFor(a => a.AlbumTracks)
            .NotNull()
            .WithMessage("Tracks must not be null.")
            ;

        RuleForEach(a => a.AlbumTracks)
            .SetValidator(new AlbumTrackValidator())
            .WithMessage("Invalid track in the album.")
            ;
    }
}

class AlbumFindableValidator : AbstractValidator<Album>
{
    public AlbumFindableValidator()
    {
        RuleFor(a => a.Id)
            .Must(id => id > 0)
            .WithMessage("Album ID must be greater than 0.")
            ;
        Include(new FindableValidator(Album.KeyExpression));
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

        // TODO: we probably do not want this extra trip to the database, if we have unique DB constraints on the PK Id?
        RuleFor(l => l.Id)
            .MustAsync(async (l, id, ct) => await IsValid(repository, l, id, ct))
            .WithMessage("The Album Id must be unique.")
            ;
    }

    static async ValueTask<bool> IsValid(
        IRepository repository,
        Album album,
        uint id,
        CancellationToken cancellationToken)
        => repository.StateOf(album) switch {

            // if Added, make sure the id is unique in the database.
            EntityState.Added => !await repository.Set<Album>().AnyAsync(a => a.Id == id, cancellationToken),

            // if Modified, make sure there is an album with this id in the database.
            EntityState.Modified => await repository.Set<Album>().AnyAsync(a => a.Id == id, cancellationToken),

            _ => true
        };
}
