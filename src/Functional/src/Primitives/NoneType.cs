namespace Functional.Primitives;

/// <summary>
/// Intended to replace <see langword="null"/>.
/// </summary>
public readonly record struct NoneType()
{
    /// <summary>
    /// Maps the specified a <see cref="NoneType" /> to <typeparamref name="R" />.
    /// </summary>
    /// <typeparam name="R">The type of the return value. Here it is always <see cref="NoneType" />.</typeparam>
    /// <param name="_">The mapping function is ignored.</param>
    /// <returns><see cref="None" />.</returns>
    /// <remarks>
    /// We can do without this but here we are skipping over <see cref="Option{T}.Match{R}(Func{T, R}, Func{R})" />
    /// and a implicit conversion, returning directly <see cref="None" />.
    /// </remarks>
    public static Option<R> Map<R>(Func<NoneType, R> _) => None;

    /// <summary>
    /// Similar to <see cref="Map{R}(Func{NoneType, R})"/> but here the mapping function takes
    /// an <see cref="NoneType"/> argument and returns (maps it to) an <see cref="Option{R}"/> value - <see cref="None"/>.
    /// </summary>
    /// <typeparam name="R">The type of the return value. Here it is always <see cref="NoneType"/>.</typeparam>
    /// <param name="_">The mapping function is ignored.</param>
    /// <returns><see cref="None" />.</returns>
    /// <remarks>
    /// We can do without this but here we are skipping over <see cref="Option{T}.Match{R}(Func{T, R}, Func{R})" />
    /// and a implicit conversion, returning directly <see cref="None" />.
    /// </remarks>
    public static Option<R> Bind<R>(Func<NoneType, Option<R>> _) => None;
}
