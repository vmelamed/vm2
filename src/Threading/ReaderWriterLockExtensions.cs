namespace vm2.Threading;

/// <summary>
/// Class ReaderWriterLockSlimExtensions. Utility class for better management of the lifetime of the scope of
/// <see cref="ReaderWriterLockSlim"/>
/// </summary>
public static class ReaderWriterLockExtensions
{
    /// <summary>
    /// Gets a upgradeable reader slim sync. Mere call to <c>new UpgradeableReaderSync(readerWriterLock)</c>
    /// but shows nicely in intellisense.
    /// </summary>
    /// <param name="readerWriterLock">The reader writer lock.</param>
    /// <param name="waitMs">How long to wait for the lock to be acquired in ms. If 0 - wait indefinitely.</param>
    /// <returns><see cref="UpgradeableReaderSync" /> object.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// class Protected
    /// {
    ///     static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
    ///     static RWSynchronizedObject _protected = new();
    ///
    ///     public void Add(string key, string value)
    ///     {
    ///         using var _ = _lock.UpgradableReaderLock(100))
    ///         if (_protected.Exists())
    ///             return;
    ///
    ///         using var __ = _loc.WriterLock(100))
    ///         _protected.Add(key, value);
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public static UpgradeableReaderSync UpgradeableReaderLock(this ReaderWriterLockSlim readerWriterLock, int waitMs = 0)
        => new(readerWriterLock, waitMs);

    /// <summary>
    /// Gets a reader slim sync. Mere call to <c>new ReaderSync(readerWriterLock)</c> but shows nicely in intellisense.
    /// </summary>
    /// <param name="readerWriterLock">The reader writer lock.</param>
    /// <param name="waitMs">How long to wait for the lock to be acquired in ms. If 0 - wait indefinitely.</param>
    /// <returns><see cref="ReaderSync" /> object.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// class Protected
    /// {
    ///     static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
    ///     static RWSynchronizedObject _protected = new();
    ///
    ///     public string Read()
    ///     {
    ///         using _ = _lock.ReaderLock())
    ///         return _protected.ReadOperation();
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public static ReaderSync ReaderLock(this ReaderWriterLockSlim readerWriterLock, int waitMs = 0)
        => new(readerWriterLock, waitMs);

    /// <summary>
    /// Gets the reader slim sync. Mere call to <c>new WriterSync(readerWriterLock)</c> but shows nicely in intellisense.
    /// </summary>
    /// <param name="readerWriterLock">The reader writer lock.</param>
    /// <param name="waitMs">How long to wait for the lock to be acquired in ms. If 0 - wait indefinitely.</param>
    /// <returns><see cref="WriterSync" /> object.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// class Protected
    /// {
    ///     static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
    ///     static RWSynchronizedObject _protected = new();
    ///
    ///     public string Write()
    ///     {
    ///         using _ = _lock.WriterLock())
    ///         return _protected.WriteOperation();
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public static WriterSync WriterLock(this ReaderWriterLockSlim readerWriterLock, int waitMs = 0)
        => new(readerWriterLock, waitMs);
}
