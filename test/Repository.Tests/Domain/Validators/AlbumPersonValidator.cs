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

class AlbumPersonFindableValidator : AbstractValidator<AlbumPerson>
{
    public AlbumPersonFindableValidator(object? _ = null)
    {
        RuleFor(ap => ap.AlbumId)
            .Must(id => id > 0)
            .WithMessage("AlbumId must be greater than 0.");
        RuleFor(ap => ap.PersonId)
            .Must(id => id > 0)
            .WithMessage("PersonId must be greater than 0.");
    }
}

class AlbumPersonValidator : AbstractValidator<AlbumPerson>
{
    public AlbumPersonValidator(object? context = null)
    {
        Include(new AlbumPersonInvariantValidator());
        Include(new AlbumPersonFindableValidator());

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
