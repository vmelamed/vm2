namespace vm2.Repository.Domain.Validators.Dimensions;

class RoleValidator : AbstractValidator<Role>
{
    public RoleValidator(IRepository? repository = null)
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("The role name cannot be null or empty.")
            .MaximumLength(Role.MaxNameLength)
            .WithMessage($"The role name cannot be longer than {Role.MaxNameLength} characters.")
            ;

        if (repository is null)
            return;

        // do we want this extra trip to the database, if we have unique DB constraints?
        // Dimension data does not get added or modified all that often, so it may be worth it.
        RuleFor(r => r.Name)
            .MustAsync(async (r, n, ct) => await IsValid(repository, r, n, ct))
            .WithMessage("The the role name must be unique.")
            ;
    }

    static async ValueTask<bool> IsValid(
        IRepository repository,
        Role role,
        string name,
        CancellationToken cancellationToken)
        => repository.StateOf(role) switch {
            EntityState.Added => !await repository
                                            .Set<Role>()
                                            .AnyAsync(r => r.Name == name, cancellationToken),

            EntityState.Modified => !await repository
                                            .Set<Role>()
                                            .AnyAsync(r => r.Name == name, cancellationToken),

            _ => true,
        };
}
