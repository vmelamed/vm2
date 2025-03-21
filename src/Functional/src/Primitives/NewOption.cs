namespace Functional.Primitives;

/// <summary>
/// Class NewOption contains a method to parse a string to a value of <see cref="Option{T}"/>.
/// </summary>
public static class NewOption
{
    /// <summary>
    /// Delegate TryParse represents a function that can parse a string into a value of type <typeparamref name="T"/>.
    /// The function should return <see langword="true"/> if the parsing was successful, otherwise <see langword="false"/>.
    /// The successfully parsed value should be in the <see langword="out"/> parameter <paramref name="t"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s">The s.</param>
    /// <param name="t">The t.</param>
    /// The function should return <see langword="true"/> if the parsing was successful, otherwise <see langword="false"/>.
    public delegate bool TryParse<T>(string? s, out T t);

    /// <summary>
    /// Creates an instance of <see cref="Option{T}"/> out of a string.
    /// </summary>
    /// <typeparam name="T">The base type.</typeparam>
    /// <param name="s">The string to parse.</param>
    /// <param name="tryParse">
    /// The parsing function should return <see langword="true"/> if the parsing was successful, otherwise <see langword="false"/>.
    /// </param>
    /// <returns>Option&lt;T&gt;.</returns>
    public static Option<T> FromString<T>(string? s, TryParse<T> tryParse) => tryParse(s, out T t) ? t : None;

    /// <summary>
    /// Creates an instance of <see cref="Option{T}"/> out of value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">The t.</param>
    /// <param name="isValid">
    /// The function should return <see langword="true"/> if the value is valid (e.g. 2 &lt; t &lt; 10), otherwise <see langword="false"/>.
    /// </param>
    /// <returns>Option&lt;T&gt;.</returns>
    public static Option<T> FromValid<T>(T t, Func<T, bool> isValid) => isValid(t) ? t : None;
}
