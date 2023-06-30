
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.OutputCaching;
using static System.Net.Mime.MediaTypeNames;

namespace Erebor.Infrastructure.Monads;

public abstract class Either<TRight, TLeft>
{
    public T Match<T>(Func<TRight, T> rightFunc, Func<TLeft, T> leftFunc) =>
        this switch
        {
            (Right<TRight, TLeft> { Value: var v }) => rightFunc(v),
            (Left<TRight, TLeft> { Value: var v }) => leftFunc(v),
            _ => throw new ArgumentOutOfRangeException()
        };

    public Either<T, TLeft> Bind<T>(Func<TRight, Either<T, TLeft>> func) =>
        this switch
        {
            (Right<TRight, TLeft> { Value: var v }) => func(v),
            (Left<TRight, TLeft> { Value: var v }) => new Left<T, TLeft>(v),
            _ => throw new ArgumentOutOfRangeException()
        };
}

public sealed class Left<TRight, TLeft> : Either<TRight, TLeft>
{
    public TLeft Value { get; }

    internal Left(TLeft value)
    {
        Value = value;
    }
}

public sealed class Right<TRight, TLeft> : Either<TRight, TLeft>
{
    public TRight Value { get; }

    internal Right(TRight value)
    {
        Value = value;
    }
}

public static class Either
{
    public static Either<TRight, TLeft> Right<TRight, TLeft>(TRight val) => 
        new Right<TRight, TLeft>(val);

    public static Either<TRight, TLeft> Left<TRight, TLeft>(TLeft val) =>
        new Left<TRight, TLeft>(val);

    public static Either<TResult, Exception> Try<TResult>(Func<TResult> func)
    {
        try
        {
            var result = func();
            return Right<TResult, Exception>(result);
        }
        catch (Exception ex)
        {
            return Left<TResult, Exception>(ex);
        }
    }
}