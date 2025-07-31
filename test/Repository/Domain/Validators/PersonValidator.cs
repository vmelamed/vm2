namespace vm2.Repository.Domain.Validators;

class PersonInvariantValidator : AbstractValidator<Person>
{
    public PersonInvariantValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Name must not be null or empty.")
            .MaximumLength(Person.MaxNameLength)
            .WithMessage($"Name cannot be longer than {Person.MaxNameLength} characters.")
            ;

        RuleFor(p => p)
            .Must(p => (p.BirthYear is null or > 0) &&
                       (p.DeathYear is null or > 0) &&
                       (p.BirthYear is null || p.DeathYear is null || p.BirthYear < p.DeathYear))
            .WithMessage("BirthYear can be null or positive number, DeathYear can be null or positive number, if none of them is null, DeathYear must be greater than BirthYear.")
            ;

        RuleFor(p => p.Roles)
            .NotNull()
            .WithMessage("The Roles collection must not be null.")
            .Must(roles => roles.All(t => !string.IsNullOrEmpty(t)))
            .WithMessage("If not empty, the individual roles must not be null or empty.")
            ;

        RuleFor(p => p.Genres)
            .NotNull()
            .WithMessage("The Genres collection must not be null.")
            .Must(genres => genres.All(g => !string.IsNullOrEmpty(g)))
            .WithMessage("If not empty, the individual genres must not be null or empty.")
            ;

        RuleFor(p => p.InstrumentCodes)
            .NotNull()
            .WithMessage("The Instruments collection must not be null.")
            .Must(instruments => instruments.All(t => !string.IsNullOrEmpty(t)))
            .WithMessage("If not empty, the individual instruments must not be null or empty.")
            ;
    }
}

class PersonFindableValidator : AbstractValidator<Person>
{
    public PersonFindableValidator()
    {
        RuleFor(p => p.Id)
            .Must(id => id > 0)
            .WithMessage("Person ID must be greater than 0.")
            ;
        Include(new FindableValidator(Person.KeyExpression));
    }
}

class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator(IRepository? repository)
    {
        Include(new PersonInvariantValidator());
        Include(new PersonFindableValidator());
        Include(new AuditableValidator());

        if (repository is null)
            return;

        // Do we want this extra trip to the database, if we have unique DB constraints on the PK Id?
        RuleFor(p => p.Id)
            .MustAsync(async (p, id, ct) => await IsValid(repository, p, id, ct))
            .WithMessage("The Person Id must be unique.")
            ;
    }

    static async ValueTask<bool> IsValid(
        IRepository repository,
        Person person,
        uint id,
        CancellationToken cancellationToken)
        => repository.StateOf(person) switch {
            // if Added, make sure the id is unique in the database.
            EntityState.Added => !await repository.Set<Person>().AnyAsync(a => a.Id == id, cancellationToken),

            // if Modified, make sure there is a Person with this id in the database.
            EntityState.Modified => await repository.Set<Person>().AnyAsync(a => a.Id == id, cancellationToken),

            _ => true
        };
}