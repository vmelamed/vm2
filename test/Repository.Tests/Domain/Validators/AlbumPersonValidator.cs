namespace vm2.Repository.Tests.Domain.Validators;

class AlbumPersonInvariantValidator : AbstractValidator<AlbumPerson>
{
    public AlbumPersonInvariantValidator()
    {
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
    public AlbumPersonValidator(object? context = null)
    {
        Include(new AlbumPersonInvariantValidator());

        if (context is not IRepository repository)
            return;

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
            EntityState.Modified => Role.HasValues(ap.Roles) &&
                                    Instrument.HasValues(ap.Instruments)
                                    ,
            _ => true,
        };
}
