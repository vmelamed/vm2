namespace vm2.Repository.TestDomain.Validators;

using vm2.Repository.EfRepository;

class TrackPersonInvariantValidator : AbstractValidator<TrackPerson>
{
    public TrackPersonInvariantValidator(bool lazyLoading = false)
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

        if (lazyLoading)
            return;

        RuleFor(tp => tp.Person)
            .NotNull()
            .WithMessage("AlbumTrack must have a valid track.")
            ;
    }
}


class TrackPersonValidator : AbstractValidator<TrackPerson>
{
    public TrackPersonValidator(IRepository? repository = null)
    {
        Include(new TrackPersonInvariantValidator(repository?.IsLazyLoadingEnabled<TrackPerson>() is true));

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
        => tp.Person is null ||
           repository.StateOf(tp.Person) switch {

               EntityState.Added or
               EntityState.Modified => Role.Has(tp.Roles) &&
                                       Instrument.Has(tp.Instruments),

               _ => true,
           };
}
