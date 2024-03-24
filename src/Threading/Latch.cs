namespace vm2.Threading;

/// <summary>
/// Can be used as a one time action (e.g. initialization), thread synchronization that avoids using the pattern 
/// if(flag)-lock()-if(flag)-do() pattern.
/// </summary>
/// <example>
/// The following double-checking pattern:
/// <![CDATA[
///     bool _isInitialized;
///     object _sync = new object();
///     ...
///     if (_isInitialized == 0)
///         lock(_syncObject)
///         {
///             if (_isInitialized == 0)
///             {
///                 _isInitialized = 1;
///                 Initialize();
///             }
///         }
/// ]]>
/// can be replaced by:
/// <![CDATA[
///     Latch _initLatch = new Latch();
///     ...
///     if (_initLatch.Lock())
///     {
///         // no other thread will attempt to initialize this object ever again
///         Initialize();
///     }
/// ]]>
/// or similarly:
/// <![CDATA[
///     static Latch _initLatch = new Latch();
///     static void Initialize() {...}
///     ...
///     if (_initLatch.Lock())
///     {
///         // no other thread will attempt to initialize this class ever again
///         Initialize();
///     }
/// ]]>
/// </example>
public struct Latch
{
    int _latch;

    /// <summary>
    /// Checks if the latch is already locked and if it is not - locks it, as an atomic action.
    /// </summary>
    /// <returns><c>true</c> if the latch latched on, <c>false</c> otherwise.</returns>
    public bool Lock() => Interlocked.CompareExchange(ref _latch, 1, 0) == 0;
}
