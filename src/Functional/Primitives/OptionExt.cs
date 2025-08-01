namespace vm2.Functional.Primitives;

/// <summary>
/// Class OptionExtensions extends the class <see cref="Option{T}"/> with a few extension methods.
/// </summary>
public static class OptionExt
{
    /// <summary>
    /// Wraps the specified value in a <see cref="Option{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value.</param>
    /// <returns>Option&lt;T&gt;.</returns>
    public static Option<T> Some<T>(T value) => new(value);

    /// <summary>
    /// The constant that replaces <see langword="null"/> for <see cref="Option{T}"/>-s
    /// </summary>
    public static readonly NoneType None = default;

    /// <summary>
    /// Converts nullable values to option.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>Option&lt;T&gt;.</returns>
    public static Option<T> ToOption<T>(this Nullable<T> value) where T : struct
        => value.HasValue ? Some(value.Value) : None;
}
