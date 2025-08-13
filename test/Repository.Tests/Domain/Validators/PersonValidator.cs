namespace vm2.Repository.Tests.Domain.Validators;

class PersonInvariantValidator : AbstractValidator<Person>
{
    public PersonInvariantValidator(bool lazyLoading = false)
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
            .WithMessage("If Roles is not empty, no role can be null or empty.")
            ;

        RuleFor(p => p.Instruments)
            .NotNull()
            .WithMessage("The Instruments collection must not be null.")
            .Must(instruments => instruments.All(t => !string.IsNullOrEmpty(t)))
            .WithMessage("If Instruments is not empty, no instrument can be null or empty.")
            ;

        RuleFor(p => p.Genres)
            .NotNull()
            .WithMessage("The Genres collection must not be null.")
            .Must(genres => genres.All(g => !string.IsNullOrEmpty(g)))
            .WithMessage("If Genres is not empty, no genre can be null or empty.")
            ;

        if (lazyLoading)
            return;

        RuleFor(p => p.PersonsAlbums)
            .NotNull()
            .WithMessage("The PersonsAlbums collection must not be null.")
            .Must(personsAlbums => personsAlbums.All(a => a is not null))
            .WithMessage("PersonsAlbums cannot contain null items.")
            ;

        RuleFor(p => p.Albums)
            .NotNull()
            .WithMessage("The Albums collection must not be null.")
            .Must(albums => albums.All(a => a is not null))
            .WithMessage("Albums cannot contain null items.")
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
    }
}

class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator(IRepository? repository = null)
    {
        Include(new PersonInvariantValidator(repository?.IsLazyLoadingEnabled<Person>() is true));
        Include(new PersonFindableValidator());
        Include(new AuditableValidator());

        RuleForEach(p => p.PersonsAlbums)
            .SetValidator(new AlbumPersonValidator(repository))
            .WithMessage("Invalid AlbumPerson in the PersonsAlbums collection.")
            ;

        // Do we want this extra trip to the database, if we have unique DB constraints on the PK Id?
        if (repository is not null)
            RuleFor(p => p.Id)
                .MustAsync(async (p, id, ct) => await IsValid(repository, p, id, ct).ConfigureAwait(false))
                .WithMessage("The Person Id must be unique.")
                ;
    }

    static async ValueTask<bool> IsValid(
        IRepository repository,
        Person person,
        int id,
        CancellationToken cancellationToken)
        => repository.StateOf(person) switch {

            // if Added, make sure the id is unique in the database.
            EntityState.Added => Instrument.HasValues(person.Instruments)
                                 && Role.HasValues(person.Roles)
                                 && Genre.HasValues(person.Genres)
                                 && !await repository
                                                .Set<Person>()
                                                .AnyAsync(a => a.Id == id, cancellationToken)
                                                .ConfigureAwait(false),

            // if Modified, make sure there is a Person with this id in the database.
            EntityState.Modified => Instrument.HasValues(person.Instruments)
                                    && Role.HasValues(person.Roles)
                                    && Genre.HasValues(person.Genres)
                                    && await repository
                                                .Set<Person>()
                                                .AnyAsync(a => a.Id == id, cancellationToken)
                                                .ConfigureAwait(false),

            _ => true
        };
}