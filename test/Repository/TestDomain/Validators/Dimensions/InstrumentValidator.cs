namespace vm2.Repository.TestDomain.Validators.Dimensions;

using vm2.Repository.EfRepository;

/// <summary>
/// Validates the minimal set of rules that make an <see cref="Instrument"/> object valid regardless of the state of the object.
/// </summary>
class InstrumentInvariantValidator : AbstractValidator<Instrument>
{
    public InstrumentInvariantValidator()
    {
        Include(new InstrumentFindableValidator());
        RuleFor(instrument => instrument.Name)
            .NotEmpty()
            .WithMessage("Instrument's full name must not be empty.")
            .MaximumLength(Instrument.MaxNameLength)
            .WithMessage($"Instrument's full name cannot be longer than {Instrument.MaxNameLength} characters.")
            ;
    }
}

class InstrumentFindableValidator : AbstractValidator<Instrument>
{
    public InstrumentFindableValidator()
    {
        RuleFor(instrument => instrument.Code)
            .Matches(Regexes.InstrumentCode())
            .WithMessage($"Instrument ID cannot be null or empty.")
            .MaximumLength(Instrument.MaxCodeLength)
            .WithMessage($"Instrument ID cannot be longer than {Instrument.MaxCodeLength} characters.")
            ;
    }
}

class InstrumentValidator : AbstractValidator<Instrument>
{
    public InstrumentValidator(IRepository? repository = null)
    {
        Include(new InstrumentInvariantValidator());

        if (repository is null)
            return;

        // Do we want this extra trip to the database, if we have unique DB constraints?
        // Dimension data does not get added or modified all that often, so it may be worth it.
        RuleFor(i => i.Code)
            .MustAsync(async (i, c, ct) => await IsValid(repository, i, c, ct).ConfigureAwait(false))
            .WithMessage("The instrument name must be unique.")
            ;
    }

    static async ValueTask<bool> IsValid(
        IRepository repository,
        Instrument instrument,
        string code,
        CancellationToken ct)
        => repository.StateOf(instrument) switch {
            // The code of an added instrument must not exist in the database.
            EntityState.Added => !await repository
                                            .Set<Instrument>()
                                            .AnyAsync(i => i.Code == code, ct)
                                            .ConfigureAwait(false)
                                            ,

            // The code of a modified instrument must exist in the database: we can edit only the name of the instrument, not its code.
            // If we wanted to allow changing the code, we must delete the existing instrument and add a new one with the new code.
            EntityState.Modified => await repository
                                            .Set<Instrument>()
                                            .AnyAsync(i => i.Code == code, ct)
                                            .ConfigureAwait(false)
                                            ,

            _ => true,
        };
}
