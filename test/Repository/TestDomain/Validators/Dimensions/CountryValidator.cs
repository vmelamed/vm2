namespace vm2.Repository.TestDomain.Validators.Dimensions;

/// <summary>
/// Validates the minimal set of rules that make a <see cref="Country"/> object valid regardless of the state of the object.
/// </summary>
class CountryInvariantValidator : AbstractValidator<Country>
{
    public CountryInvariantValidator()
    {
        Include(new CountryFindableValidator());
        RuleFor(country => country.Name)
            .NotEmpty()
            .WithMessage("Country's full name must not be empty.")
            .MaximumLength(Country.MaxNameLength)
            .WithMessage($"Country's full name cannot be longer than {Country.MaxNameLength} characters.")
            ;
    }
}

class CountryFindableValidator : AbstractValidator<Country>
{
    public CountryFindableValidator()
    {
        RuleFor(country => country.Code)
            .Matches(Regexes.CountryCode())
            .WithMessage("Country's Code must be a valid ISO 3166 country code and must consist of 2 upper-case Latin characters.")
            ;
    }
}

class CountryValidator : AbstractValidator<Country>
{
    public CountryValidator(IRepository? repository = null)
    {
        Include(new CountryInvariantValidator());

        if (repository is null)
            return;

        // Do we want this extra trip to the database, if we have unique DB constraints?
        // Dimension data does not get added or modified all that often, so it may be worth it.
        RuleFor(i => i.Code)
            .MustAsync(async (i, c, ct) => await IsValid(repository, i, c, ct))
            .WithMessage("The country code is not unique or cannot be found.")
            ;
    }

    static async ValueTask<bool> IsValid(
        IRepository repository,
        Country country,
        string code,
        CancellationToken cancellationToken)
        => repository.StateOf(country) switch {
            // The code of an added country must not exist in the database.
            EntityState.Added => !await repository
                                            .Set<Country>()
                                            .AnyAsync(i => i.Code == code, cancellationToken)
                                            .ConfigureAwait(false)
                                            ,
            // The code of a modified country must exist in the database: we can edit only the name of the country, not its code.
            EntityState.Modified => await repository
                                            .Set<Country>()
                                            .AnyAsync(i => i.Code == code, cancellationToken)
                                            .ConfigureAwait(false)
                                            ,
            _ => true,
        };
}
