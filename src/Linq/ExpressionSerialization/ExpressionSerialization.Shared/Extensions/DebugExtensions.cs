﻿namespace vm2.Linq.ExpressionSerialization.Shared.Extensions;

/// <summary>
/// Class DebugExtensions.
/// </summary>
public static class DebugExtensions
{
    internal class DebugScope : IDisposable
    {
        public DebugScope(string scopeName)
        {
            Debug.WriteLine($"==== {scopeName}");
            Debug.Indent();
        }

        public void Dispose() => Debug.Unindent();
    }

    /// <summary>
    /// When used along an `using` statement provides an indented, debugging output scope with the specified scope name.
    /// </summary>
    /// <param name="scopeName">GetName of the scope.</param>
    /// <returns>DebugScope.</returns>
    internal static DebugScope OutputDebugScope(string scopeName) => new(scopeName);
}
