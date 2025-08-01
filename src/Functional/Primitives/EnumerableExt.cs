namespace vm2.Functional.Primitives;

/// <summary>
/// Class EnumerableExt defines extension methods of <see cref="IEnumerable{T}"/> in action more <i>functional</i> conventions.
/// </summary>
public static class EnumerableExt
{
    /// <summary>
    /// From the specified sequence of values of type <typeparamref name="T"/> to action sequence of <typeparamref name="R"/>
    /// with the mapping function <paramref name="f"/>. Basically, this is rename of the extension method
    /// <see cref="Enumerable.Select{T, R}(IEnumerable{T}, Func{T, R})"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <param name="f">The f.</param>
    /// <returns>IEnumerable&lt;R&gt;.</returns>
    public static IEnumerable<R> Map<T, R>(this IEnumerable<T> enumerable, Func<T, R> f)
        => enumerable.Select(f);

    /// <summary>
    /// Applies the <paramref name="action"/> to every value of the sequence <paramref name="enumerable"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values in the sequence.</typeparam>
    /// <param name="enumerable">The sequence.</param>
    /// <param name="action">
    /// The action that (usually) exhibits some side effects by applying it to values of type <typeparamref name="T"/>.
    /// </param>
    /// <returns>IEnumerable&lt;Unit&gt;.</returns>
    public static IEnumerable<Unit> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        => enumerable.Map(action.ToFunc()).ToImmutableList();

    /// <summary>
    /// Applies the <paramref name="action"/> to every value of the sequence <paramref name="enumerable"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values in the sequence.</typeparam>
    /// <param name="enumerable">The sequence.</param>
    /// <param name="action">
    /// The action that (usually) exhibits some side effects by applying it to values of type <typeparamref name="T"/>.
    /// </param>
    /// <returns>Unit.</returns>
    /// <remarks>Saves memory by not instantiating a <see cref="List{Unit}"/>.
    /// <see cref="Enumerable.Count{T}(IEnumerable{T})"/> iterates the lazy-instantiating sequence
    /// </remarks>
    public static Unit ForEach2<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var _ in enumerable.Map(action.ToFunc()))
            ;
        return default;
    }
}
