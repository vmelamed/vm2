namespace vm2.Linq.ExpressionSerialization.Shared.Extensions;

/// <summary>
/// Class DebugExtensions.
/// </summary>
public static class DebugExtensions
{
    /// <summary>
    /// Provides a mechanism for creating a scoped debug context that automatically adjusts indentation in debug output.
    /// </summary>
    public class DebugScope : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DebugScope"/> class, writing the specified scope name to the
        /// debug output and increasing the indentation level.
        /// </summary>
        public DebugScope(string scopeName)
        {
            Debug.WriteLine($"==== {scopeName}");
            Debug.Indent();
        }

        /// <summary>
        /// Releases all resources used by the <see cref="DebugScope"/> instance, decreasing the indentation level.
        /// </summary>
        public void Dispose()
        {
            Debug.Unindent();
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// When used along an `using` statement provides an indented, debugging output scope with the specified scope name.
    /// </summary>
    /// <param name="scopeName">GetName of the scope.</param>
    /// <returns>DebugScope.</returns>
    public static DebugScope OutputDebugScope(string scopeName) => new(scopeName);
}
