
namespace Erebor.Infrastructure.Monads;

public abstract class Maybe<T>
{
    public Maybe<TResult> Map<TResult>(Func<T, TResult> func) =>
        this switch
        {
            Some<T> { Value: var v } => new Some<TResult>(func(v)),
            _ => new None<TResult>(),
        };

    public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> func) =>
        this switch
        {
            Some<T> { Value: var v } => func(v),
            _ => new None<TResult>(),
        };

    public T GetValue(T fallbackValue) =>
        (fallbackValue, this) switch
        {
            (null, _) => throw new ArgumentNullException(nameof(fallbackValue)),
            (_, Some<T> { Value: var v }) => v,
            _ => fallbackValue
        };
}

public sealed class Some<T> : Maybe<T>
{
    public T Value { get; }

    internal Some(T value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        Value = value;
    }
}

public sealed class None<T> : Maybe<T>
{
}

public static class Maybe
{
    public static Maybe<T> Some<T>(T val) => new Some<T>(val);
    public static Maybe<T> None<T>() => new None<T>();
}