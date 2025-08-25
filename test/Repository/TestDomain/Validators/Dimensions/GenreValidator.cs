namespace vm2.Repository.TestDomain.Validators.Dimensions;

public class GenreValidator : AbstractValidator<Genre>
{
    public GenreValidator(IRepository? repository = null)
    {
        RuleFor(g => g.Name)
            .NotEmpty()
            .WithMessage("The genre name cannot be null or empty.")
            .MaximumLength(Genre.MaxNameLength)
            .WithMessage($"The genre name cannot be longer than {Genre.MaxNameLength} characters.")
            ;

        if (repository is null)
            return;

        // do we want this extra trip to the database, if we have unique DB constraints?
        // Dimension data does not get added or modified all that often, so it may be worth it.
        RuleFor(g => g.Name)
            .MustAsync(async (g, n, ct) => await IsValid(repository, g, n, ct).ConfigureAwait(false))
            .WithMessage("The genre name must be unique.")
            ;
    }

    static async ValueTask<bool> IsValid(
        IRepository repository,
        Genre genre,
        string name,
        CancellationToken cancellationToken)
        => repository.StateOf(genre) switch {
            EntityState.Added => !await repository
                                            .Set<Genre>()
                                            .AnyAsync(g => g.Name == name, cancellationToken)
                                            .ConfigureAwait(false)
                                            ,

            EntityState.Modified => await repository
                                            .Set<Genre>()
                                            .AnyAsync(g => g.Name == name, cancellationToken)
                                            .ConfigureAwait(false)
                                            ,

            _ => true,
        };
}
