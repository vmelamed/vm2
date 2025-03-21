namespace vm2.TestUtilities;
using System;
using System.Threading.Tasks;

/// <summary>
/// Class PrimitivesTestsFixture.
/// Implements the <see cref="IDisposable" />
/// Implements the <see cref="IAsyncDisposable" />
/// </summary>
/// <seealso cref="System.IDisposable" />
/// <seealso cref="System.IAsyncDisposable" />
public class BaseTestsFixture : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrimitivesTestsFixture"/> class.
    /// </summary>
    public BaseTestsFixture() => FluentAssertionsExceptionFormatter.EnableDisplayOfInnerExceptions();

    /// <inheritdoc/>
    public void Dispose() => GC.SuppressFinalize(this);

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}