namespace Functional.Primitives;

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
    /// Maps the specified a <see cref="NoneType"/> to <typeparamref name="R"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source instance.</typeparam>
    /// <typeparam name="R">The type of the result.</typeparam>
    /// <returns><see cref="None"/>.</returns>
    /// <remarks>
    /// We can without this but here we are skipping over conversion of <see cref="NoneType"/> to <see cref="Option{T}"/>
    /// and then calling <see cref="Option{T}.Match{R}(Func{T, R}, Func{R})"/> and returning <see cref="None"/> directly.
    /// </remarks>
    public static Option<R> Map<T, R>(this NoneType _, Func<T, R> __) => None;

    /// <summary>
    /// Using the function <paramref name="f"/> generates a new instance of type <typeparamref name="R"/>
    /// based on the internal state of the <paramref name="ot"/>.
    /// Map : (C{T}, (T → R)) → C{R}
    /// <para>
    /// <see cref="Map{T, R}(Option{T}, Func{T, R})"/> makes <see cref="Option{T}"/> a <i><b>functor</b></i>.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the source instance.</typeparam>
    /// <typeparam name="R">The type of the result.</typeparam>
    /// <param name="ot">The instance to map.</param>
    /// <param name="f">The function that maps T -> R.</param>
    /// <returns>Functional.Primitives.Option&lt;R&gt;.</returns>
    public static Option<R> Map<T, R>(this Option<T> ot, Func<T, R> f)
        => ot.Match(
                None: () => None,
                Some: t => Some(f(t)));
}
