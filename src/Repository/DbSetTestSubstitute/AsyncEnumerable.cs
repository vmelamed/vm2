namespace vm2.Repository.FakeDbSet;

/// <summary>
/// Represents an asynchronous enumerable collection of elements that can be queried asynchronously.
/// </summary>
/// <remarks>
/// This class provides an implementation of <see cref="IAsyncEnumerable{T}"/> and <see cref="IQueryable{T}"/>, allowing for
/// asynchronous iteration over a collection of elements. It can be constructed with either an <see cref="IEnumerable{T}"/> or
/// an <see cref="Expression"/> to support LINQ query capabilities.
/// </remarks>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
class AsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncEnumerable{T}"/> class.
    /// </summary>
    /// <param name="enumerable">A collection to associate with the new instance.</param>
    public AsyncEnumerable(IEnumerable<T> enumerable)
        : base(enumerable)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncEnumerable{T}"/> class.
    /// </summary>
    /// <param name="expression">An expression tree to associate with the new instance.</param>
    /// <exception cref="InvalidOperationException">Expected CallExpression but got: {expression} (TODO!)</exception>
    public AsyncEnumerable(Expression expression)
        : base(expression)
    {
    }

    /// <summary>
    /// Gets the asynchronous enumerator.
    /// </summary>
    /// <param name="_">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>IAsyncEnumerator&lt;T&gt;.</returns>
    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken _)
        => new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

    /// <summary>
    /// Gets the query provider that is associated with this data source.
    /// </summary>
    IQueryProvider IQueryable.Provider => new AsyncQueryProvider<T>(this);
}
