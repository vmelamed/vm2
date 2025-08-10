namespace vm2.Repository.EfRepository.Models.Validators;

/// <summary>
/// Validator for objects that implement the <see cref="IFindable"/> interface. This validator can be <c>Include</c>-ed in a
/// fluent validation pipeline <br/>
/// but it can also be implemented with more specific validation rules in derived classes, e.g. <br/>
/// <c>RuleFor(f => f.Id).Must(id => id > 0)</c>.
/// </summary>
public class FindableValidator : AbstractValidator<IFindable>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FindableValidator"/> class.
    /// </summary>
    public FindableValidator()
    {
        RuleFor(f => f.KeyValues)
            .Must(kvs => kvs is not null && kvs.All(kv => kv is not null))
            .WithMessage("The key value(s) cannot be null.");
    }
}
