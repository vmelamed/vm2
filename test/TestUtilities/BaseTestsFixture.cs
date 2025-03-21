namespace vm2.TestUtilities;

/// <summary>
/// Class PrimitivesTestsFixture.
/// Implements the <see cref="IDisposable" />
/// Implements the <see cref="IAsyncDisposable" />
/// </summary>
/// <seealso cref="System.IDisposable" />
/// <seealso cref="System.IAsyncDisposable" />
public class BaseTestsFixture<TFixture> : IClassFixture<TFixture>, IDisposable, IAsyncDisposable where TFixture : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimitivesTestsFixture"/> class.
    /// </summary>
    public BaseTestsFixture() => FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();

    /// <inheritdoc/>
    public virtual void Dispose() => GC.SuppressFinalize(this);

    /// <inheritdoc/>
    public virtual ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}