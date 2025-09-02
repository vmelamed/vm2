namespace vm2.Repository.EntityFramework.Models.Validators;

/// <summary>
/// Provides validation rules for objects implementing the <see cref="ITenanted{TTenantId}"/> interface, ensuring that the
/// tenant identifier is valid.
/// </summary>
/// <remarks>
/// This validator enforces that the <see cref="ITenanted{TTenantId}.TenantId"/> property is not empty.
/// </remarks>
/// <typeparam name="TTenantId">
/// The type of the tenant identifier. This type must be non-nullable and implement <see cref="IEquatable{T}"/>.
/// </typeparam>
public class TenantedValidator<TTenantId> : AbstractValidator<ITenanted<TTenantId>> where TTenantId : notnull, IEquatable<TTenantId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TenantedValidator{TTenantId}"/> class.
    /// </summary>
    public TenantedValidator()
    {
        RuleFor(t => t.TenantId)
            .NotEmpty()
            .WithMessage("TenantId must not be empty.")
            ;
    }
}
