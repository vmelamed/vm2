namespace vm2.Repository.Tests.Domain.Validators;

class TrackPersonValidator : AbstractValidator<TrackPerson>
{
    public TrackPersonValidator(IRepository? repository = null)
    {
        RuleFor(tp => tp.Person)
            .NotNull()
            .WithMessage("Person must not be null.")
            ;

        RuleFor(tp => tp.Name)
            .NotEmpty()
            .WithMessage("Person name must not be null or empty.")
            .MaximumLength(Person.MaxNameLength)
            .WithMessage($"Person name cannot be longer than {Person.MaxNameLength} characters.")
            ;

        RuleFor(tp => tp.Roles)
            .NotEmpty()
            .Must(roles => roles.All(t => !string.IsNullOrEmpty(t)))
            .WithMessage("Roles must not be null. If not empty, the individual roles must not be null or empty.")
            ;

        RuleFor(tp => tp.Instruments)
            .NotNull()
            .Must(instruments => instruments.All(i => !string.IsNullOrEmpty(i)))
            .WithMessage("Instruments must not be null. If not empty, the individual instruments must not be null or empty.")
            ;

        if (repository is null)
            return;

        RuleFor(tp => tp)
            .Must((tp, ct) => HasValidDimensions(repository, tp))
            .WithMessage("The AlbumPerson must have valid dimensions.")
            ;
    }

    static bool HasValidDimensions(
        IRepository repository,
        TrackPerson tp)
        => tp.Person is null
                ? true
                : repository.StateOf(tp.Person) switch {
                    EntityState.Added or
                    EntityState.Modified => Role.HasValues(tp.Roles) &&
                                            Instrument.HasValues(tp.Instruments)
                                            ,
                    _ => true,
                };
}
