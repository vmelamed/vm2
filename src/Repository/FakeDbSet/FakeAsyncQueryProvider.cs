namespace vm2.Repository.FakeDbSet;

/// <summary>
/// A fake asynchronous query provider that wraps a synchronous <see cref="IQueryProvider"/> to support asynchronous query execution.
/// </summary>
/// <typeparam name="T"></typeparam>
public class FakeAsyncQueryProvider<T> : IAsyncQueryProvider
{
    readonly IQueryProvider _inner;

    internal FakeAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

    #region IAsyncQueryProvider
    #region IQueryProvider
    /// <summary>
    /// Constructs an <see cref="IQueryable" /> object that can evaluate the query represented by a specified expression tree.
    /// </summary>
    /// <param name="expression">An expression tree that represents a LINQ query.</param>
    /// <returns>An <see cref="IQueryable" /> that can evaluate the query represented by the specified expression tree.</returns>
    public IQueryable CreateQuery(Expression expression) => new FakeAsyncEnumerable<T>(expression);

    /// <summary>
    /// Constructs an <see cref="IQueryable{T}" /> object that can evaluate the query represented by a specified
    /// expression tree.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements of the <see cref="IQueryable" /> that is returned.</typeparam>
    /// <param name="expression">An expression tree that represents a LINQ query.</param>
    /// <returns>An <see cref="IQueryable{TElement}" /> that can evaluate the query represented by the specified expression tree.</returns>
    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new FakeAsyncEnumerable<TElement>(expression);

    /// <summary>
    /// Executes the query represented by a specified expression tree.
    /// </summary>
    /// <param name="expression">An expression tree that represents a LINQ query.</param>
    /// <returns>The value that results from executing the specified query.</returns>
    public object? Execute(Expression expression) => _inner.Execute(expression);

    /// <summary>
    /// Executes the strongly-typed query represented by a specified expression tree.
    /// </summary>
    /// <typeparam name="TResult">The type of the value that results from executing the query.</typeparam>
    /// <param name="expression">An expression tree that represents a LINQ query.</param>
    /// <returns>The value that results from executing the specified query.</returns>
    public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);
    #endregion

    /// <summary>
    /// Executes the expression asynchronously.
    /// </summary>
    /// <typeparam name="TResult">The type of the t result.</typeparam>
    /// <param name="expression">The expression.</param>
    /// <param name="ct">Can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>TResult.</returns>
    /// <exception cref="OperationCanceledException">Operation canceled.</exception>
    /// <exception cref="InvalidOperationException">Unexpected \"null\" value of the expression. (extend!)</exception>
    /// <exception cref="InvalidOperationException">Expected {typeof(TResult).Name} expression result but got {resultValue?.GetType()?.Name ?? "null"} (TODO!)</exception>
    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var resultValue = Execute(expression);

        if (resultValue is TResult res)
            return res;

        var tResultType = typeof(TResult);
        var expressionType = expression.Type;

        // build and return Task<TResult>.FromResult(resultValue);
        var taskResultType = typeof(Task<>).MakeGenericType(expressionType);

        if (tResultType.IsAssignableFrom(taskResultType))
            return (TResult)typeof(Task)
                                .GetMethod(nameof(Task.FromResult))!
                                .MakeGenericMethod(expressionType)
                                .Invoke(null, [resultValue])!
                                ;

        // we should not be here, but just in case:
        if (resultValue is null)
            throw new InvalidOperationException($"Unexpected \"null\" result value of the expression. (extend !)");

        throw new InvalidOperationException($"Expected {typeof(TResult).Name} expression result but got {resultValue?.GetType()?.Name ?? "null"} (TODO!)");
    }
    #endregion
}
