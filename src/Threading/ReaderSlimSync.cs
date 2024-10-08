﻿namespace vm2.Threading;

/// <summary>
/// With the help of this class create a synchronized multiple readers/single writer scope by utilizing the <c>using</c> statement.
/// </summary>
/// <example>
/// <code>
/// <![CDATA[
/// class Protected
/// {
///     static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
///     static Dictionary<string, string>; _protected = new Dictionary<string, string>();
///     
///     public void Get(string key)
///     {
///         using(new ReaderSlimSync(_lock))
///             return _protected[key];
///     }
/// }
/// ]]>
/// </code>
/// </example>
/// <seealso cref="ReaderWriterLockSlim"/>,
/// <seealso cref="UpgradeableReaderSlimSync"/>, 
/// <seealso cref="WriterSlimSync"/>, 
/// <seealso cref="ReaderWriterLockSlimExtensions.ReaderLock(ReaderWriterLockSlim)"/>.
public sealed class ReaderSlimSync : IDisposable
{
    readonly ReaderWriterLockSlim _readerWriterLock;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReaderSlimSync"/> class with the specified <paramref name="readerWriterLock"/> and
    /// waits indefinitely till it acquires the lock in reader mode.
    /// </summary>
    /// <param name="readerWriterLock">The reader writer lock.</param>
    public ReaderSlimSync(
        ReaderWriterLockSlim readerWriterLock)
    {
        readerWriterLock.EnterReadLock();
        _readerWriterLock = readerWriterLock;
    }

    #region IDisposable pattern implementation
    /// <summary>
    /// The flag is being set when the object gets disposed.
    /// </summary>
    /// <value>
    /// 0 - if the object is not disposed yet, any other value would mean that the object is already disposed.
    /// </value>
    int _disposed;

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            _readerWriterLock.ExitReadLock();
    }
    #endregion
}
