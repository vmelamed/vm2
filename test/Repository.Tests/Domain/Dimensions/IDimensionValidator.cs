namespace vm2.Repository.Tests.Domain.Dimensions;

public interface IDimensionValidator<TDimension, TValue> where TDimension : class, IDimensionValidator<TDimension, TValue>
{
    static virtual HashSet<TValue> Values { get; private set; } = [];

    static abstract Expression<Func<TDimension, TValue>> ValueExpression { get; }

    static async ValueTask RefreshValues(object source, CancellationToken cancellationToken)
    {
        if (source is not IRepository repository)
            return;

        TDimension.Values = await repository
                                    .Set<TDimension>()
                                    .Select(TDimension.ValueExpression)
                                    .ToHashSetAsync(cancellationToken)
                                    .ConfigureAwait(false)
                                    ;
    }

    static bool Has(TValue value)
        => TDimension.Values.Contains(value);

    static bool Has(IEnumerable<TValue> values)
        => TDimension.Values.Intersect(values).Count() == values.Count();
}
