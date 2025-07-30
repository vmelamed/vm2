namespace vm2.Repository.Abstractions.Model.Validators;

/// <summary>
/// Provides validation rules for objects implementing the <see cref="IAuditable"/> interface.
/// </summary>
public class AuditableValidator : AbstractValidator<IAuditable>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuditableValidator"/> class.
    /// </summary>
    public AuditableValidator()
    {
        RuleFor(auditable => auditable.CreatedAt)
            .NotEqual(default(DateTimeOffset))
            .LessThanOrEqualTo(DateTimeOffset.Now)
            .WithMessage("CreatedAt cannot be in the future.")
            ;

        // TODO: Uncomment when CreatedBy is implemented
        //RuleFor(auditable => auditable.CreatedBy)
        //    .NotEmpty()
        //    .WithMessage("CreatedBy must not be empty.")
        //    ;

        RuleFor(auditable => auditable.UpdatedAt)
            .NotEqual(default(DateTimeOffset))
            .LessThanOrEqualTo(DateTimeOffset.Now)
            .WithMessage("UpdatedAt cannot be in the future.")
            ;

        // TODO: Uncomment when UpdatedBy is implemented
        //RuleFor(auditable => auditable.UpdatedBy)
        //    .NotEmpty()
        //    .WithMessage("UpdatedBy must not be empty.");
    }
}
