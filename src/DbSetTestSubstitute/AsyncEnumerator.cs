namespace vm2.Repository.DbSetTestSubstitute;

/// <summary>
/// Provides an asynchronous enumerator for iterating over a collection of type <typeparamref name="T"/>.
/// </summary>
/// <remarks>
/// This class adapts a synchronous <see cref="IEnumerator{T}"/> to the asynchronous <see cref="IAsyncEnumerator{T}"/> interface.
/// It is useful for scenarios where asynchronous iteration is required but only a synchronous enumerator is available.
/// </remarks>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
class AsyncEnumerator<T> : IAsyncEnumerator<T>
{
    readonly IEnumerator<T> _enumerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncEnumerator{T}"/> class with the specified enumerator.
    /// </summary>
    /// <remarks>
    /// The provided enumerator is used to perform asynchronous iteration over the collection. Ensure that the enumerator is not
    /// null before passing it to this constructor.
    /// </remarks>
    /// <param name="enumerator">The enumerator to be used for asynchronous iteration.</param>
    public AsyncEnumerator(IEnumerator<T> enumerator)
        => _enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator), "Enumerator cannot be null.");

    /// <summary>
    /// Asynchronously advances the enumerator to the next element of the collection.
    /// </summary>
    /// <returns>
    /// A <see cref="ValueTask{Boolean}"/> that represents the asynchronous operation.  The task result contains
    /// <see langword="true"/> if the enumerator was successfully advanced  to the next element; <see langword="false"/> if the
    /// enumerator has passed the end of the collection.
    /// </returns>
    public ValueTask<bool> MoveNextAsync()
        => ValueTask.FromResult(_enumerator.MoveNext());

    /// <summary>
    /// Gets the current element in the collection.
    /// </summary>
    public T Current => _enumerator.Current;

    /// <summary>
    /// Asynchronously releases the resources used by the enumerator.
    /// </summary>
    /// <remarks>This method completes synchronously and returns a completed <see cref="ValueTask"/>.</remarks>
    /// <returns>A completed <see cref="ValueTask"/> representing the asynchronous dispose operation.</returns>
    public ValueTask DisposeAsync()
    {
        _enumerator.Dispose();
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}
