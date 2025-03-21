namespace Functional.Primitives;

/// <summary>
/// Extension functions of the <see cref="Action"/> family of types that convert them to functions with return value of type <see cref="Unit"/>.
/// </summary>
public static partial class ActionExt
{
    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{Unit}"/>
    /// </summary>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;Unit&gt;.</returns>
    public static Func<Unit> ToFunc(this Action action)
        => () =>
        {
            action();
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, Unit&gt;.</returns>
    public static Func<T1, Unit> ToFunc<T1>(
        this Action<T1> action)
        where T1 : allows ref struct
        => (t1) =>
        {
            action(t1);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, Unit&gt;.</returns>
    public static Func<T1, T2, Unit> ToFunc<T1, T2>(
        this Action<T1, T2> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        => (t1, t2) =>
        {
            action(t1, t2);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, Unit&gt;.</returns>
    public static Func<T1, T2, T3, Unit> ToFunc<T1, T2, T3>(
        this Action<T1, T2, T3> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        => (t1, t2, t3) =>
        {
            action(t1, t2, t3);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, T4, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, Unit> ToFunc<T1, T2, T3, T4>(
        this Action<T1, T2, T3, T4> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        => (t1, t2, t3, t4) =>
        {
            action(t1, t2, t3, t4);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, T4, T5, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, Unit> ToFunc<T1, T2, T3, T4, T5>(
        this Action<T1, T2, T3, T4, T5> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        => (t1, t2, t3, t4, t5) =>
        {
            action(t1, t2, t3, t4, t5);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, Unit> ToFunc<T1, T2, T3, T4, T5, T6>(
        this Action<T1, T2, T3, T4, T5, T6> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        => (t1, t2, t3, t4, t5, t6) =>
        {
            action(t1, t2, t3, t4, t5, t6);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, Unit> ToFunc<T1, T2, T3, T4, T5, T6, T7>(
        this Action<T1, T2, T3, T4, T5, T6, T7> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        => (t1, t2, t3, t4, t5, t6, t7) =>
        {
            action(t1, t2, t3, t4, t5, t6, t7);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the action.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Unit> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8>(
        this Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        => (t1, t2, t3, t4, t5, t6, t7, t8) =>
        {
            action(t1, t2, t3, t4, t5, t6, t7, t8);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the action.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the action.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Unit> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        => (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
        {
            action(t1, t2, t3, t4, t5, t6, t7, t8, t9);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the action.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the action.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the action.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Unit> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
        => (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
        {
            action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the action.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the action.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the action.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the action.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Unit> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
        where T11 : allows ref struct
        => (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
        {
            action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the action.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the action.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the action.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the action.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the action.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Unit> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
        where T11 : allows ref struct
        where T12 : allows ref struct
        => (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
        {
            action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the action.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the action.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the action.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the action.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the action.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the action.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Unit> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
        where T11 : allows ref struct
        where T12 : allows ref struct
        where T13 : allows ref struct
        => (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
        {
            action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the action.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the action.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the action.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the action.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the action.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the action.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the action.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Unit> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
        where T11 : allows ref struct
        where T12 : allows ref struct
        where T13 : allows ref struct
        where T14 : allows ref struct
        => (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
        {
            action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the action.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the action.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the action.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the action.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the action.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the action.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the action.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the action.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Unit> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
        where T11 : allows ref struct
        where T12 : allows ref struct
        where T13 : allows ref struct
        where T14 : allows ref struct
        where T15 : allows ref struct
        => (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
        {
            action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="action"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Unit}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the action.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the action.</typeparam>
    /// <typeparam name="T9">The type of the ninth parameter of the action.</typeparam>
    /// <typeparam name="T10">The type of the tenth parameter of the action.</typeparam>
    /// <typeparam name="T11">The type of the eleventh parameter of the action.</typeparam>
    /// <typeparam name="T12">The type of the twelfth parameter of the action.</typeparam>
    /// <typeparam name="T13">The type of the thirteenth parameter of the action.</typeparam>
    /// <typeparam name="T14">The type of the fourteenth parameter of the action.</typeparam>
    /// <typeparam name="T15">The type of the fifteenth parameter of the action.</typeparam>
    /// <typeparam name="T16">The type of the sixteenth parameter of the action.</typeparam>
    /// <param name="action">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Unit> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
        where T11 : allows ref struct
        where T12 : allows ref struct
        where T13 : allows ref struct
        where T14 : allows ref struct
        where T15 : allows ref struct
        where T16 : allows ref struct
        => (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
        {
            action(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
            return default;
        };
}
