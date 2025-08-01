﻿namespace vm2.Repository.Domain.Validators.Dimensions;

public class GenreValidator : AbstractValidator<Genre>
{
    public GenreValidator(IRepository? repository = null)
    {
        RuleFor(g => g.Name)
            .NotEmpty()
            .WithMessage("The genre name cannot be null or empty.")
            .MaximumLength(Genre.MaxLength)
            .WithMessage($"The genre name cannot be longer than {Genre.MaxLength} characters.")
            ;

        if (repository is null)
            return;

        // do we want this extra trip to the database, if we have unique DB constraints?
        // Dimension data does not get added or modified all that often, so it may be worth it.
        RuleFor(g => g.Name)
            .MustAsync(async (g, n, ct) => await IsValid(repository, g, n, ct))
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
                                            ,

            EntityState.Modified => !await repository
                                            .Set<Genre>()
                                            .AnyAsync(g => g.Name == name, cancellationToken)
                                            ,

            _ => true,
        };
}
