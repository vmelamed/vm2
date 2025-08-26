namespace vm2.Repository.TestDomain.Validators;

using vm2.Repository.EfRepository;

class AlbumPersonMinimalValidator : AbstractValidator<AlbumPerson>
{
    public AlbumPersonMinimalValidator(bool lazyLoading = false)
    {
        if (lazyLoading)
            return;

        RuleFor(ap => ap.Album)
            .NotNull()
            .WithMessage("Album must not be null.");

        RuleFor(ap => ap.Person)
            .NotNull()
            .WithMessage("Person must not be null.");
    }
}

class AlbumPersonValidator : AbstractValidator<AlbumPerson>
{
    public AlbumPersonValidator(IRepository? repository = null)
    {
        Include(new AlbumPersonMinimalValidator(repository?.IsLazyLoadingEnabled<AlbumPerson>() is true));

        if (repository is not null)
            RuleFor(ap => ap)
                .Must((ap, ct) => HasValidDimensions(repository, ap))
                .WithMessage("The AlbumPerson must have valid dimensions.")
                ;
    }

    static bool HasValidDimensions(
        IRepository repository,
        AlbumPerson ap)
        => repository.StateOf(ap) switch {

            EntityState.Added or
            EntityState.Modified => Role.Has(ap.Roles) &&
                                    Instrument.Has(ap.Instruments),

            _ => true,
        };
}
