namespace vm2.Repository.Tests.Domain.Dimensions;

/// <summary>
/// Defines a contract for validating set of dimension values in a domain model.
/// </summary>
/// <typeparam name="TDimension"></typeparam>
/// <typeparam name="TValue"></typeparam>
public interface IDimensionValidator<TDimension, TValue> where TDimension : class, IDimensionValidator<TDimension, TValue>
{
    /// <summary>
    /// Cache of the current set of values for the dimension.
    /// </summary>
    static virtual HashSet<TValue> Values { get; private set; } = [];

    /// <summary>
    /// Gets the expression that defines how to retrieve the value of type <typeparamref name="TValue"/>  from an instance of
    /// <typeparamref name="TDimension"/>.
    /// </summary>
    static abstract Expression<Func<TDimension, TValue>> ValueExpression { get; }

    /// <summary>
    /// Refreshes the values of the cache <see cref="TDimension.Values"/> property by retrieving data from the specified source.
    /// </summary>
    /// <param name="source">
    /// The source object, which should implement <see cref="IRepository"/> to provide the data. If the source does not
    /// implement <see cref="IRepository"/>, the method does nothing.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests. The operation will be canceled if the token is triggered.
    /// </param>
    /// <returns></returns>
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

    /// <summary>
    /// Determines whether all specified values are present in the dimension's set of values (are in the cache).
    /// </summary>
    /// <param name="values">The collection of values to check for existence.</param>
    /// <returns><see langword="true"/> if all values in the specified collection exist in the dimension's set of values;
    /// otherwise, <see langword="false"/>.</returns>
    static bool Has(IEnumerable<TValue> values) => TDimension.Values.Intersect(values).Count() == values.Count();
}
