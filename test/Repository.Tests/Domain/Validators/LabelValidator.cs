namespace vm2.Repository.Tests.Domain.Validators;

/// <summary>
/// Validates the minimal set of rules that make a <see cref="Label"/> object valid regardless of the state of the object.
/// </summary>
class LabelInvariantValidator : AbstractValidator<Label>
{
    public LabelInvariantValidator(bool lazyLoading = false)
    {
        RuleFor(label => label.Name)
            .NotEmpty()
            .WithMessage("Label name must not be null or empty.")
            .MaximumLength(Label.MaxNameLength)
            .WithMessage($"Label name cannot be longer than {Label.MaxNameLength} characters.")
            ;

        RuleFor(label => label.CountryCode)
            .Matches(Regexes.CountryCode())
            .WithMessage("Country code must not be null or empty and it must be a valid ISO 3166 country code.")
            ;

        if (lazyLoading)
            return;

        RuleFor(label => label.Albums)
            .NotNull()
            .WithMessage("The Albums collection must not be null.")
            .Must(albums => albums.All(a => a is not null))
            .WithMessage("Albums cannot contain null items.")
            ;

        RuleForEach(label => label.Albums)
            .SetValidator(new AlbumValidator())
            .WithMessage("Invalid Album in the Albums collection.")
            ;
    }
}

class LabelFindableValidator : AbstractValidator<Label>
{
    public LabelFindableValidator()
    {
        RuleFor(label => label.Id)
            .Must(id => id > 0)
            .WithMessage("Label ID must be greater than 0.")
            ;
    }
}

class LabelValidator : AbstractValidator<Label>
{
    public LabelValidator(IRepository? repository = null)
    {
        Include(new LabelInvariantValidator(repository?.IsLazyLoadingEnabled<Label>() is true));
        Include(new LabelFindableValidator());
        Include(new AuditableValidator());

        if (repository is null)
            return;

        // Do we want this extra trip to the database, if we have unique DB constraints on the PK and on Name?
        // Label is almost like a dimension data: does not get added or modified all that often, so it may be worth it.
        RuleFor(l => l.CountryCode)
            .Must((l, c) => IsValid(repository, l, c))
            .WithMessage("The Label must have a valid country code.")
            ;
    }

    static bool IsValid(
        IRepository repository,
        Label label,
        string countryCode)
        => repository.StateOf(label) switch {
            EntityState.Added or
            EntityState.Modified => Country.HasValue(countryCode),

            _ => true
        };
}
