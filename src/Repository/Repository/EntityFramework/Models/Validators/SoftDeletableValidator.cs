namespace vm2.Repository.EntityFramework.Models.Validators;

using vm2.Repository.EntityFramework.Models;

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
        When(deletable => deletable.DeletedAt.HasValue, () =>
            RuleFor(deletable => deletable.DeletedAt)
                .Must(deletedAt => deletedAt is null || deletedAt.Value.Kind == DateTimeKind.Utc)
                .WithMessage("DeletedAt must be in UTC.")
            );
        // TODO: Uncomment when DeletedBy is implemented
        //RuleFor(deletable => deletable.DeletedBy)
        //    .NotEmpty()
        //    .WithMessage("DeletedBy must not be empty.")
        //    ;
    }
}
