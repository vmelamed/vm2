namespace Functional.Primitives;

/// <summary>
/// Represents the no-value returned from a function <see cref="Func{Unit}"/>s.
/// Replacement for <see cref="void"/> which is returned by <see cref="Action"/>s.
/// </summary>
public readonly record struct Unit
{
}
