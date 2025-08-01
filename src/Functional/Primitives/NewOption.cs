namespace vm2.Functional.Primitives;

/// <summary>
/// Class NewOption contains a method to parse a string to a value of <see cref="Option{T}"/>.
/// </summary>
public static class NewOption
{
    /// <summary>
    /// Delegate TryParse represents the functions that can parse a string into a value of type <typeparamref name="T"/>.
    /// The function should return <see langword="true"/> if the parsing was successful, otherwise <see langword="false"/>.
    /// The successfully parsed value should be in the <see langword="out"/> parameter <paramref name="t"/>. The parsing
    /// function must never throw an exception.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s">The s.</param>
    /// <param name="t">The t.</param>
    /// The function should return <see langword="true"/> if the parsing was successful, otherwise <see langword="false"/>.
    public delegate bool TryParse<T>(string? s, out T t);

    /// <summary>
    /// Delegate Validate represents the functions that can validate the value of <paramref name="t"/> against some business
    /// rules. The function should never throw exception.
    /// </summary>
    /// <typeparam name="T">The type of the value to be validated.</typeparam>
    /// <param name="t">The value to be validated.</param>
    /// <returns><c>true</c> if <paramref name="t"/> is valid, <c>false</c> otherwise.</returns>
    public delegate bool Validate<T>(T t);

    /// <summary>
    /// Creates an instance of <see cref="Option{T}"/> out of a string.
    /// </summary>
    /// <typeparam name="T">The base type.</typeparam>
    /// <param name="s">The string to parse.</param>
    /// <param name="tryParse">The parsing function. See also <seealso cref="TryParse{T}"/>.</param>
    /// <returns>Option&lt;T&gt;.</returns>
    public static Option<T> FromString<T>(string? s, TryParse<T> tryParse)
        => tryParse(s, out T t) ? t : None;

    /// <summary>
    /// Creates an instance of <see cref="Option{T}"/> out of a string.
    /// </summary>
    /// <typeparam name="T">The base type.</typeparam>
    /// <param name="s">The string to parse.</param>
    /// <param name="tryParse">The parsing function. See also <seealso cref="TryParse{T}"/>.</param>
    /// <returns>Option&lt;T&gt;.</returns>
    public static Option<T> FromString<T>(Option<string> s, TryParse<T> tryParse)
        => s.Match(
                None: () => None,
                Some: str => tryParse(str, out T t) ? Some(t) : None);

    /// <summary>
    /// Validates the
    /// Creates an instance of <see cref="Option{T}"/> out of a valid value for type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="t">The value.</param>
    /// <param name="valid">
    /// The function should return <see langword="true"/> if the value is valid (e.g. 2 &lt; t &lt; 10), otherwise
    /// <see langword="false"/>.
    /// </param>
    /// <returns>Option&lt;T&gt;.</returns>
    public static Option<T> FromValid<T>(T t, Validate<T> valid)
        => valid(t) ? t : None;
}
