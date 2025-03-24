namespace Functional.Primitives;
#pragma warning disable IDE1006 // Naming Styles

/// <summary>
/// Represents a type that cannot be <see langword="null"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct Option<T> : IEquatable<NoneType>, IEquatable<Option<T>>
{
    readonly T _value;
    readonly bool _isSome;

    readonly bool _isNone => !_isSome;

    /// <summary>
    /// Initializes a new instance of the <see cref="Option{T}"/> struct.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <exception cref="System.ArgumentNullException">value</exception>
    /// <remarks>
    /// The only way to create an otherOption is by casting a value of <typeparamref name="T"/> to <see cref="Option{T}"/>.
    /// </remarks>
    internal Option(T value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
        _isSome = true;
    }

    /// <summary>
    /// Pattern-matches the current instance. If it is <see cref="NoneType"/> - it executes the input function <see cref="None"/>,
    /// otherwise - <see cref="Some"/>.
    /// Match: t, Some, None -> ( R | None )
    /// </summary>
    /// <typeparam name="R"></typeparam>
    /// <param name="Some">
    /// The function to execute if the current instance represent an instance of type <typeparamref name="T"/>.
    /// </param>
    /// <param name="None">
    /// The function to execute if the current instance is <see cref="NoneType"/>.
    /// </param>
    /// <returns>R.</returns>
    public readonly R Match<R>(Func<T, R> Some, Func<R> None) => _isSome ? Some(_value!) : None();

    /// <summary>
    /// Using the function <paramref name="f"/> generates a new instance of type <typeparamref name="R"/>
    /// based on the internal state of this.
    /// Map : (C{T}, (T → R)) → C{R}
    /// <para>
    /// <see cref="Map{R}(Func{T, R})"/> makes <typeparamref name="T"/> a <i><b>functor</b></i>.
    /// </para>
    /// </summary>
    /// <typeparam name="R">The type of the result.</typeparam>
    /// <param name="f">The function that maps T -> R.</param>
    /// <returns>Functional.Primitives.Option&lt;R&gt;.</returns>
    public Option<R> Map<R>(Func<T, R> f)
        => Match(
                None: () => None,
                Some: t => Some(f(t)));

    /// <summary>
    /// Similar to <see cref="Map{R}(Func{T, R})"/> but here the mapping function <paramref name="f"/> takes
    /// an <see cref="Option{T}"/> argument and returns (maps it to) an <see cref="Option{R}"/> value.
    /// </summary>
    /// <typeparam name="R"></typeparam>
    /// <param name="f">The f.</param>
    /// <returns>Option&lt;R&gt;.</returns>
    public Option<R> Bind<R>(Func<T, Option<R>> f)
        => Match(
            None: () => None,
            Some: t => f(t));

    /// <summary>
    /// Makes this object behave as a sequence of one value.
    /// </summary>
    /// <returns>IEnumerable&lt;T&gt;.</returns>
    public readonly IEnumerable<T> AsEnumerable()
    {
        if (_isSome)
            yield return _value!;
    }

    /// <summary>
    /// Applies the <paramref name="action"/> to the value in this option.
    /// </summary>
    /// <param name="action">The action that exhibits side effects.</param>
    /// <returns>Option&lt;Unit&gt;.</returns>
    public Option<Unit> ForEach(Action<T> action)
        => Map(action.ToFunc());

    #region IEquatable<Option<T>>
    /// <summary>
    /// Compares this instance with other for equality.
    /// </summary>
    /// <param name="other">The other instance.</param>
    /// <returns>
    /// <c>true</c> if the instances are considered equal, <c>false</c> otherwise.
    /// </returns>
    /// <remarks>
    /// Two <see cref="Option{T}"/> values are considered equal if:
    /// <list type="bullet">
    /// <item>Both instances are <see cref="None"/>, or</item>
    /// <item>Both instances are not <see cref="None"/> and the wrapped values are equal.</item>
    /// </list>
    /// </remarks>
    public readonly bool Equals(Option<T> other) => _isSome == other._isSome && (_isNone || _value!.Equals(other._value));
    #endregion

    #region IEquatable<NoneType>
    /// <summary>
    /// Compares this instance with value of <see cref="NoneType"/>.
    /// </summary>
    /// <param name="_">Ignored - <see cref="None"/></param>
    /// <returns><c>true</c> if this instance is <see cref="None"/>, <c>false</c> otherwise.</returns>
    public readonly bool Equals(NoneType _) => _isNone;
    #endregion

    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal to this instance.
    /// </summary>
    /// <param name="other">The <see cref="object" /> to compare with this instance.</param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// An <see cref="Option{T}"/> value and an <see cref="object"/> are considered equal if:
    /// <list type="bullet">
    /// <item>Both instances are <see cref="None"/>, or</item>
    /// <item>Both instances are not <see cref="None"/> and the wrapped values are equal.</item>
    /// </list>
    /// </remarks>
    public readonly override bool Equals(object? other) => other is Option<T> otherOption && this.Equals(otherOption);

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public readonly override int GetHashCode() => _isSome ? _value!.GetHashCode() : 0;

    /// <summary>
    /// Returns a <see cref="string" /> that represents this instance.
    /// </summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public readonly override string ToString() => _isSome ? $"Some({_value})" : "None";

    #region Overloaded operators
    /// <summary>
    /// Implements the true operator.
    /// </summary>
    /// <param name="this">The operand to which the `true` operation is applied.</param>
    /// <returns>
    /// <see langword="true"/> if this is not <see cref="None"/>, otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator true(Option<T> @this) => @this._isSome;

    /// <summary>
    /// Implements the false operator.
    /// </summary>
    /// <param name="this">The operand to which the `false` operation is applied.</param>
    /// <returns>
    /// <see langword="true"/> if this is <see cref="None"/>, otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator false(Option<T> @this) => @this._isNone;

    /// <summary>
    /// Implements the | operator.
    /// </summary>
    /// <param name="l">The left operand.</param>
    /// <param name="r">The right operand.</param>
    /// <returns>The <paramref name="l"/> if it is not <see cref="None"/>, otherwise <paramref name="r"/>.</returns>
    public static Option<T> operator |(Option<T> l, Option<T> r) => l._isSome ? l : r;

    /// <summary>
    /// Performs an implicit conversion from instance of <typeparamref name="T"/> to <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator Option<T>(T value) => value is null ? None : Some(value);

    /// <summary>
    /// Performs an implicit conversion from <see cref="NoneType"/> to <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="_">
    /// The <see cref="NoneType"/> instance to convert.
    /// </param>
    /// <returns>The result of the conversion.</returns>
    public static implicit operator Option<T>(NoneType _) => default;

    /// <summary>
    /// Implements the == operator.
    /// </summary>
    /// <param name="this">The left operand.</param>
    /// <param name="other">The right operand.</param>
    /// <returns>
    /// <see langword="true"/> if the instances are considered equal.
    /// </returns>
    public static bool operator ==(Option<T> @this, Option<T> other) => @this.Equals(other);

    /// <summary>
    /// Implements the != operator.
    /// </summary>
    /// <param name="this">The left operand.</param>
    /// <param name="other">The right operand.</param>
    /// <returns>
    /// <see langword="true"/> if the instances are considered not equal.
    /// </returns>
    public static bool operator !=(Option<T> @this, Option<T> other) => !(@this == other);
    #endregion
}
