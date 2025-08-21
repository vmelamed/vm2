namespace vm2.Repository.Tests.Domain.Validators;

class AlbumInvariantValidator : AbstractValidator<Album>
{
    public AlbumInvariantValidator(bool lazyLoading = false)
    {
        RuleFor(a => a.Title)
            .NotEmpty()
            .WithMessage("Title must not be null or empty.")
            .MaximumLength(Album.MaxTitleLength)
            .WithMessage($"Title cannot be longer than {Album.MaxTitleLength} characters.")
            ;

        RuleFor(a => a.ReleaseYear)
            .Must(releaseYear => releaseYear is null || releaseYear.Value >= 1900 && releaseYear <= DateTime.Now.Year)
            .WithMessage("The release year must be equal or greater than 1900 and equal or less than the current year.")
            ;

        RuleFor(a => a.Genres)
            .NotNull()
            .WithMessage("The Genres collection must not be null.")
            .Must(genres => genres.All(g => !string.IsNullOrEmpty(g)))
            .WithMessage("If Genres is not empty, no genre can be null or empty.")
            ;

        if (lazyLoading)
            return;

        RuleFor(a => a.AlbumsPersons)
            .NotNull()
            .WithMessage("AlbumsPersons must not be null.")
            .Must(p => p.All(ap => ap is not null))
            .WithMessage("AlbumsPersons cannot contain null items.")
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
            .Must(t => t.All(at => at is not null))
            .WithMessage("AlbumTracks cannot contain null items.")
            ;
    }
}

class AlbumFindableValidator : AbstractValidator<Album>
{
    public AlbumFindableValidator()
    {
        RuleFor(a => a.Id.Id)
            .NotEmpty()
            .WithMessage("Album ID must be greater than 0.")
            ;
    }
}

class AlbumValidator : AbstractValidator<Album>
{
    public AlbumValidator(IRepository? repository = null)
    {
        Include(new AlbumInvariantValidator(repository?.IsLazyLoadingEnabled<Album>() is true));
        Include(new AlbumFindableValidator());
        Include(new AuditableValidator());
        Include(new SoftDeletableValidator());

        RuleForEach(a => a.AlbumsPersons)
            .SetValidator(new AlbumPersonValidator(repository))
            .WithMessage("Invalid AlbumPerson in the PersonsAlbums collection.")
            ;

        RuleForEach(a => a.AlbumTracks)
            .SetValidator(new AlbumTrackValidator(repository))
            .WithMessage("Invalid track in the album.")
            ;

        RuleFor(a => a.Label!)
            .SetValidator(new LabelValidator(repository))
            .WithMessage($"Invalid label.")
            .When(a => a.Label is not null)
            ;

        // TODO: we probably do not want this extra trip to the database, if we have unique DB constraints on the PK Id?
        if (repository is not null)
            RuleFor(a => a.Id)
                .MustAsync(async (a, id, ct) => await IsValid(repository, a, a.Id, ct).ConfigureAwait(false))
                .WithMessage("The Album Id must be unique.")
                ;
    }

    static async ValueTask<bool> IsValid(
        IRepository repository,
        Album album,
        AlbumId id,
        CancellationToken cancellationToken)
        => repository.StateOf(album) switch {

            // if Added, make sure the id is unique in the database.
            EntityState.Added => Genre.Has(album.Genres)
                                 && !await repository
                                                .Set<Album>()
                                                .AnyAsync(a => a.Id == id, cancellationToken)
                                                .ConfigureAwait(false)
                                 ,
            // if Modified, make sure there is an album with this id in the database.
            EntityState.Modified => Genre.Has(album.Genres)
                                    && await repository
                                                .Set<Album>()
                                                .AnyAsync(a => a.Id == id, cancellationToken)
                                                .ConfigureAwait(false)
                                    ,
            _ => true
        };
}
