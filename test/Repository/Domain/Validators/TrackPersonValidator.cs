namespace vm2.Repository.Domain.Validators;

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

        RuleFor(tp => tp.InstrumentCodes)
            .NotNull()
            .Must(instruments => instruments.All(i => !string.IsNullOrEmpty(i)))
            .WithMessage("Instruments must not be null. If not empty, the individual instruments must not be null or empty.")
            ;

        if (repository is null)
            return;

        // Make sure the person exists in the database and is valid.
        RuleFor(tp => tp.Person)
            .MustAsync(
                async (tp, person, ct) => await repository
                                                    .Set<Person>()
                                                    .AnyAsync(p => p.Id == person.Id &&
                                                                    p.Name == tp.Name, ct))
            .WithMessage("Invalid person or person name.")
            ;

        // Make sure all the assigned roles exist in the database.
        RuleFor(tp => tp.Roles)
            .MustAsync(
                async (roles, ct) => roles.Count == 0 ||
                                     roles.Count == await repository
                                                            .Set<Role>()
                                                            .CountAsync(r => roles.Contains(r.Name), ct))
            .WithMessage("All roles must exist in the dimension table.")
            ;

        // Make sure all the assigned instruments exist in the database.
        RuleFor(tp => tp.InstrumentCodes)
            .MustAsync(
                async (instruments, ct) => instruments.Count == 0 ||
                                           instruments.Count == await repository
                                                                        .Set<Instrument>()
                                                                        .CountAsync(r => instruments.Contains(r.Code), ct))
            .WithMessage("All instruments must exist in the dimension table.")
            ;
    }
}
