namespace vm2.Repository.Tests.Domain.Validators;

class AlbumPersonInvariantValidator : AbstractValidator<AlbumPerson>
{
    public AlbumPersonInvariantValidator()
    {
        RuleFor(ap => ap.Album)
            .NotNull()
            .WithMessage("Album must not be null.");
        RuleFor(ap => ap.Person)
            .NotNull()
            .WithMessage("Person must not be null.");
    }
}

class AlbumPersonFindableValidator : AbstractValidator<AlbumPerson>
{
    public AlbumPersonFindableValidator(object? _ = null)
    {
        RuleFor(ap => ap.AlbumId)
            .Must(id => id > 0)
            .WithMessage("AlbumId must be greater than 0.");
        RuleFor(ap => ap.PersonId)
            .Must(id => id > 0)
            .WithMessage("PersonId must be greater than 0.");
    }
}

class AlbumPersonValidator : AbstractValidator<AlbumPerson>
{
    public AlbumPersonValidator(object? _ = null)
    {
        Include(new AlbumPersonInvariantValidator());
        Include(new AlbumPersonFindableValidator());
    }
}
