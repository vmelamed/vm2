namespace vm2.Repository.EfRepository.Models.Validators;

using vm2.Repository.EfRepository.Models;

/// <summary>
/// Provides validation rules for objects implementing the <see cref="IFindable"/> interface.
/// </summary>
public class FindableValidator : AbstractValidator<IFindable>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FindableValidator"/> class with an optional key expression.
    /// </summary>
    /// <remarks>
    /// This constructor sets up validation rules for a findable entity's key values. If a <paramref name="keyExpression"/> is
    /// provided, it ensures that the number and types of keys in the entity's key values match those specified by the expression.
    /// </remarks>
    /// <param name="keyExpression">
    /// An optional <see cref="LambdaExpression"/> that specifies the expected keys for validation. If null, only the presence
    /// of key values is validated.
    /// </param>
    public FindableValidator(LambdaExpression? keyExpression = null)
    {
        RuleFor(findable => findable.KeyValues)
            .NotEmpty()
            .WithMessage("Findable entity's KeyValues sequence must not be null or empty.")
            ;

        if (keyExpression is null)
            return;

        Type[]? paramTypes = null;

        if (keyExpression.Body is UnaryExpression ue &&
            ue.NodeType is ExpressionType.Convert or
                           ExpressionType.ConvertChecked &&
            ue.Type == typeof(object) &&
            ue.Operand is MemberExpression me)
            paramTypes = [me.Type];
        else
        if (keyExpression.Body is NewArrayExpression ne &&
            ne.Expressions.Count > 0 &&
            ne.Expressions.All(
                x => x is UnaryExpression ue &&
                     ue.NodeType is ExpressionType.Convert or
                                    ExpressionType.ConvertChecked &&
                     ue.Type == typeof(object) &&
                     ue.Operand is MemberExpression))
            paramTypes = [.. ne.Expressions.Select(x => ((UnaryExpression)x).Operand.Type)];

        RuleFor(findable => findable.KeyValues)
            .Must(keyValues => keyValues is not null &&
                               paramTypes is not null &&
                               keyValues.SequenceEqual(paramTypes))
            .WithMessage("The number and types of the keys in KeyValues must match the number and type of keys in the KeyExpression.")
            ;
    }
}
