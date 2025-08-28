namespace vm2.Repository.EntityFramework.Models.Validators;

using vm2.Repository.EntityFramework.Models;

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
            .NotEqual(default(DateTime))
            .WithMessage("CreatedAt must not be default value.")
            .Must(createdAt => createdAt.Kind == DateTimeKind.Utc)
            .WithMessage("CreatedAt must be in UTC.")
            ;

        // TODO: Uncomment when CreatedBy is implemented
        //RuleFor(auditable => auditable.CreatedBy)
        //    .NotEmpty()
        //    .WithMessage("CreatedBy must not be empty.")
        //    ;

        RuleFor(auditable => auditable.UpdatedAt)
            .NotEqual(default(DateTime))
            .WithMessage("CreatedAt must not be default value.")
            .Must(createdAt => createdAt.Kind == DateTimeKind.Utc)
            .WithMessage("CreatedAt must be in UTC.")
            ;

        // TODO: Uncomment when UpdatedBy is implemented
        //RuleFor(auditable => auditable.UpdatedBy)
        //    .NotEmpty()
        //    .WithMessage("UpdatedBy must not be empty.")
        //    ;
    }
}
