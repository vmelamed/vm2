namespace vm2.Threading;

/// <summary>
/// Interface for a reader-writer synchronization mechanism.
/// This interface provides access to a <see cref="ReaderWriterLockSlim"/> and indicates whether the lock is currently
/// held.
/// </summary>
public interface IReaderWriterSync : IDisposable
{
    /// <summary>
    /// Gets the lock.
    /// </summary>
    ReaderWriterLockSlim Lock { get; }

    /// <summary>
    /// Gets a value indicating whether the lock is held and the lock owner can read from the protected resource(s).
    /// </summary>
    bool IsLockHeld { get; }
}
