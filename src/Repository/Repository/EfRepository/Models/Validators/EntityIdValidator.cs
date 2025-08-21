namespace vm2.Repository.EfRepository.Models.Validators;

/// <summary>
/// Provides validation rules for <see cref="EntityId{TValue}"/> instances, ensuring that the entity ID is not null,
/// empty, or the default value.
/// </summary>
/// <typeparam name="TValue">The type of the value used as the entity ID. This type must be non-nullable.</typeparam>
public class EntityIdValidator<TValue> : AbstractValidator<EntityId<TValue>> where TValue : notnull
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityIdValidator{TValue}"/> class.
    /// </summary>
    public EntityIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(default(TValue))
            .WithMessage("Entity ID cannot be empty, null, or 0.");
    }
}
