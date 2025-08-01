namespace vm2.Threading.ReadersWriter;

/// <summary>
/// With the help of this class create a synchronized multiple readers scope by leveraging the <c>using</c> statement.
/// <para/>
/// When the object is created, it attempts to acquire the lock in reader mode. When disposed, it
/// releases the lock if it has previously acquired.
/// <para/>
/// Never omit disposing this object. Better yet always use it inside of a <c>using</c> statement.
/// </summary>
/// <example>
/// <code>
/// <![CDATA[
/// class Protected
/// {
///     static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
///     static RWSynchronizedObject _protected = new();
///
///     public void Read()
///     {
///         using var _ = new ReaderSync(_lock, 100))
///         return _protected.ReadOperation();
///     }
/// }
/// ]]>
/// </code>
/// </example>
public sealed class ReaderSync : IReaderWriterSync
{
    /// <summary>
    /// Gets the lock.
    /// <para/>
    /// <b>Hint:</b> prefer using the extension methods from <see cref="ReaderWriterLockExtensions"/> to work with the lock.
    /// </summary>
    public ReaderWriterLockSlim Lock { get; init; }

    /// <summary>
    /// Gets a value indicating whether the lock is held and the lock owner can read from the protected resource(s).
    /// </summary>
    public bool IsLockHeld { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ReaderSync"/> class with the specified <paramref name="readerWriterLock"/>
    /// and waits the specified number of milliseconds or indefinitely until it acquires the lock in reader mode.
    /// <para/>
    /// <b>Hint:</b> prefer using the extension methods from <see cref="ReaderWriterLockExtensions"/> to work with the lock.
    /// </summary>
    /// <param name="readerWriterLock">The reader writer lock.</param>
    /// <param name="waitMs">How long to wait for the lock to be acquired in ms. If 0 - wait indefinitely.</param>
    public ReaderSync(
        ReaderWriterLockSlim readerWriterLock,
        int waitMs = 0)
    {
        Lock = readerWriterLock;
        if (waitMs is 0)
        {
            Lock.EnterReadLock();
            IsLockHeld = true;
        }
        else
            IsLockHeld = Lock.TryEnterReadLock(waitMs);
    }

    #region IDisposable
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// <para/>
    /// <b>Hint:</b> prefer using the extension methods from <see cref="ReaderWriterLockExtensions"/> to work with the lock.
    /// </summary>
    public void Dispose()
    {
        if (IsLockHeld)
        {
            IsLockHeld = false;
            Lock.ExitReadLock();
        }
        GC.SuppressFinalize(this);
    }
    #endregion
}
