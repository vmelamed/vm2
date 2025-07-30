namespace vm2.Repository.Domain.Validators;

class PersonInvariantValidator : AbstractValidator<Person>
{
    public PersonInvariantValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Name must not be null or empty.")
            ;

        RuleFor(p => p)
            .Must(p => (p.BirthYear is null or > 0) &&
                       (p.DeathYear is null or > 0) &&
                       (p.BirthYear is null || p.DeathYear is null || p.DeathYear > p.BirthYear))
            .WithMessage("BirthYear can be null or positive number, DeathYear can be null or positive number, if none of them is null, DeathYear must be greater than BirthYear.")
            ;

        RuleFor(p => p.Roles)
            .NotEmpty()
            .Must(roles => roles.All(t => !string.IsNullOrEmpty(t)))
            .WithMessage("Roles must not be null. If not empty, the individual roles must not be null or empty.")
            ;

        RuleFor(p => p.Genres)
            .NotEmpty()
            .Must(genres => genres.All(g => !string.IsNullOrEmpty(g)))
            .WithMessage("Genres must not be null. If not empty, the individual genres must not be null or empty.")
            ;

        RuleFor(p => p.InstrumentCodes)
            .NotNull()
            .Must(instruments => instruments.All(t => !string.IsNullOrEmpty(t)))
            .WithMessage("Instruments must not be null. If not empty, the individual instruments must not be null or empty.")
            ;
    }
}

class PersonFindableValidator : AbstractValidator<Person>
{
    public PersonFindableValidator()
    {
        Include(new FindableValidator(Person.KeyExpression));
        RuleFor(p => p.Id)
            .GreaterThan(0)
            .WithMessage("Person ID must be a positive number.")
            ;
    }
}

class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator(IRepository? repository)
    {
        Include(new PersonInvariantValidator());
        Include(new PersonFindableValidator());
        Include(new AuditableValidator());
        Include(new SoftDeletableValidator());

        if (repository is null)
            return;
    }
}