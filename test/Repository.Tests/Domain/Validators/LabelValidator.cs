namespace vm2.Repository.Domain.Validators;

/// <summary>
/// Validates the minimal set of rules that make a <see cref="Label"/> object valid regardless of the state of the object.
/// </summary>
class LabelInvariantValidator : AbstractValidator<Label>
{
    public LabelInvariantValidator()
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
        Include(new FindableValidator(Label.KeyExpression));
    }
}

class LabelValidator : AbstractValidator<Label>
{
    public LabelValidator(IRepository? repository = null)
    {
        Include(new LabelInvariantValidator());
        Include(new LabelFindableValidator());
        Include(new AuditableValidator());

        if (repository is null)
            return;

        // Do we want this extra trip to the database, if we have unique DB constraints on the PK and on Name?
        // Label is almost like a dimension data: does not get added or modified all that often, so it may be worth it.
        RuleFor(l => l.Id)
            .MustAsync(async (l, id, ct) => await IsValid(repository, l, id, ct))
            .WithMessage("The Label Id must be unique.")
            ;
    }

    static async ValueTask<bool> IsValid(
        IRepository repository,
        Label label,
        uint id,
        CancellationToken cancellationToken)
        => repository.StateOf(label) switch {
            // if Added, make sure the Id and the Name are unique in the database.
            EntityState.Added => !await repository
                                            .Set<Label>()
                                            .AnyAsync(l => l.Id   == id ||
                                                           l.Name == label.Name, cancellationToken),

            // if Modified, make sure the Id already exists in the database and if the name has changed, it is also unique.
            EntityState.Modified => !repository.Entry(label).Property(nameof(Label.Name)).IsModified
                                        ? await repository
                                                    .Set<Label>()
                                                    .AnyAsync(l => l.Id == id, cancellationToken)
                                        : (await repository
                                                    .Set<Label>()
                                                    .Where(l => l.Id == id || l.Name == label.Name)
                                                    .Select(l => new { l.Id })
                                                    .ToListAsync(cancellationToken))
                                                    .SingleOrDefault()?.Id == id,

            _ => true
        };
}
