namespace vm2.Functional.Primitives;

/// <summary>
/// Class DictionaryExt extends <see cref="IDictionary{K, T}"/> to not throw exceptions.
/// </summary>
public static class DictionaryExt
{
    /// <summary>
    /// Looks up the specified key in the given dictionary.
    /// </summary>
    /// <typeparam name="K">The type of the key.</typeparam>
    /// <typeparam name="T">The type of the corresponding value.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key value.</param>
    /// <returns>Option&lt;T&gt; if found, <see cref="None"/> otherwise.</returns>
    public static Option<T> Lookup<K, T>(this IDictionary<K, T> dictionary, K key)
        => dictionary.TryGetValue(key, out T? t) ? Some(t) : None;

    /// <summary>
    /// Looks up the specified key in the given dictionary.
    /// </summary>
    /// <typeparam name="K">The type of the key.</typeparam>
    /// <typeparam name="T">The type of the corresponding value.</typeparam>
    /// <param name="dictionary">The dictionary.</param>
    /// <param name="key">The key value.</param>
    /// <returns>Option&lt;T&gt; if found, <see cref="None"/> otherwise.</returns>
    public static Option<T> Lookup<K, T>(this IReadOnlyDictionary<K, T> dictionary, K key)
        => dictionary.TryGetValue(key, out T? t) ? Some(t) : None;
}
