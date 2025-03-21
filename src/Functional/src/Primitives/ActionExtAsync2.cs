namespace Functional.Primitives;

#pragma warning disable CS1584, CS1658

/// <summary>
/// Extension functions of the <see cref="Action"/> family of types that convert them to functions with return value of type <see cref="Unit"/>.
/// </summary>
public static partial class ActionExt
{
    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{Task{Unit}}"/>
    /// </summary>
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;Unit&gt;.</returns>
    public static Func<ValueTask<Unit>> ToFunc(this Func<Task> asyncAction)
        => async () =>
        {
            await asyncAction();
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, Task{Unit}}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, Unit&gt;.</returns>
    public static Func<T1, ValueTask<Unit>> ToFunc<T1>(
        this Func<T1, Task> asyncAction)
        => async (t1) =>
        {
            await asyncAction(t1);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, Task{Unit}}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, Unit&gt;.</returns>
    public static Func<T1, T2, ValueTask<Unit>> ToFunc<T1, T2>(
        this Func<T1, T2, Task> asyncAction)
        => async (t1, t2) =>
        {
            await asyncAction(t1, t2);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, Task{Unit}}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, Unit&gt;.</returns>
    public static Func<T1, T2, T3, ValueTask<Unit>> ToFunc<T1, T2, T3>(
        this Func<T1, T2, T3, Task> asyncAction)
        => async (t1, t2, t3) =>
        {
            await asyncAction(t1, t2, t3);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, T4, Task{Unit}}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, ValueTask<Unit>> ToFunc<T1, T2, T3, T4>(
        this Func<T1, T2, T3, T4, Task> asyncAction)
        => async (t1, t2, t3, t4) =>
        {
            await asyncAction(t1, t2, t3, t4);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, T4, T5, Task{Unit}}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, ValueTask<Unit>> ToFunc<T1, T2, T3, T4, T5>(
        this Func<T1, T2, T3, T4, T5, Task> asyncAction)
        => async (t1, t2, t3, t4, t5) =>
        {
            await asyncAction(t1, t2, t3, t4, t5);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, Task{Unit}}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, ValueTask<Unit>> ToFunc<T1, T2, T3, T4, T5, T6>(
        this Func<T1, T2, T3, T4, T5, T6, Task> asyncAction)
        => async (t1, t2, t3, t4, t5, t6) =>
        {
            await asyncAction(t1, t2, t3, t4, t5, t6);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, Task{Unit}}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the action.</typeparam>
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, ValueTask<Unit>> ToFunc<T1, T2, T3, T4, T5, T6, T7>(
        this Func<T1, T2, T3, T4, T5, T6, T7, Task> asyncAction)
        => async (t1, t2, t3, t4, t5, t6, t7) =>
        {
            await asyncAction(t1, t2, t3, t4, t5, t6, t7);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, Task{Unit}}"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter of the action.</typeparam>
    /// <typeparam name="T2">The type of the second parameter of the action.</typeparam>
    /// <typeparam name="T3">The type of the third parameter of the action.</typeparam>
    /// <typeparam name="T4">The type of the fourth parameter of the action.</typeparam>
    /// <typeparam name="T5">The type of the fifth parameter of the action.</typeparam>
    /// <typeparam name="T6">The type of the sixth parameter of the action.</typeparam>
    /// <typeparam name="T7">The type of the seventh parameter of the action.</typeparam>
    /// <typeparam name="T8">The type of the eighth parameter of the action.</typeparam>
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, ValueTask<Unit>> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> asyncAction)
        => async (t1, t2, t3, t4, t5, t6, t7, t8) =>
        {
            await asyncAction(t1, t2, t3, t4, t5, t6, t7, t8);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, Task{Unit}}"/>
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
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, ValueTask<Unit>> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> asyncAction)
        => async (t1, t2, t3, t4, t5, t6, t7, t8, t9) =>
        {
            await asyncAction(t1, t2, t3, t4, t5, t6, t7, t8, t9);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task{Unit}}"/>
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
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, ValueTask<Unit>> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task> asyncAction)
        => async (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) =>
        {
            await asyncAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task{Unit}}"/>
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
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, ValueTask<Unit>> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task> asyncAction)
        => async (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11) =>
        {
            await asyncAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task{Unit}}"/>
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
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, ValueTask<Unit>> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task> asyncAction)
        => async (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12) =>
        {
            await asyncAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task{Unit}}"/>
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
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, ValueTask<Unit>> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task> asyncAction)
        => async (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13) =>
        {
            await asyncAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task{Unit}}"/>
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
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, ValueTask<Unit>> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task> asyncAction)
        => async (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14) =>
        {
            await asyncAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task{Unit}}"/>
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
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, ValueTask<Unit>> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task> asyncAction)
        => async (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15) =>
        {
            await asyncAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15);
            return default;
        };

    /// <summary>
    /// Converts the <paramref name="asyncAction"/> to <see cref="Func{T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task{Unit}}"/>
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
    /// <param name="asyncAction">The action to convert.</param>
    /// <returns>Func&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Unit&gt;.</returns>
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, ValueTask<Unit>> ToFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task> asyncAction)
        => async (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16) =>
        {
            await asyncAction(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16);
            return default;
        };
}
