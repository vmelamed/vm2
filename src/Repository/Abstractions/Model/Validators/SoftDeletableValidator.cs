namespace vm2.Repository.Abstractions.Model.Validators;

/// <summary>
/// Validator for entities that implement the <see cref="ISoftDeletable"/> interface.
/// </summary>
public class SoftDeletableValidator : AbstractValidator<ISoftDeletable>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoftDeletableValidator"/> class.
    /// </summary>
    public SoftDeletableValidator()
    {
        RuleFor(deletable => deletable.DeletedAt)
            .Must(date => date is null || date <= DateTimeOffset.Now)
            .WithMessage("DeletedAt cannot be in the future.")
            ;
        // TODO: Uncomment when DeletedBy is implemented
        //RuleFor(deletable => deletable.DeletedBy)
        //    .NotEmpty()
        //    .WithMessage("DeletedBy must not be empty.")
        //    ;
    }
}
