namespace vm2.Repository.TestDomain.Validators;

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
        RuleFor(label => label.Id.Id)
            .NotEmpty()
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
        RuleFor(l => l)
            .MustAsync(async (l, ct) => await IsValid(repository, l, ct))
            .WithMessage("The Label must have a valid country code.")
            ;
    }

    static async ValueTask<bool> IsValid(
        IRepository repository,
        Label label,
        CancellationToken ct)
        => repository.StateOf(label) switch {
            EntityState.Added => Country.Has(label.CountryCode)
                                 && !await repository
                                                .Set<Label>()
                                                .AnyAsync(l => l.Id == label.Id, ct)
                                                .ConfigureAwait(false),

            EntityState.Modified => Country.Has(label.CountryCode)
                                    && await repository
                                                .Set<Label>()
                                                .AnyAsync(l => l.Id == label.Id, ct)
                                                .ConfigureAwait(false),

            _ => true
        };
}
