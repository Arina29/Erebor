using System.Reactive;

namespace Erebor.Infrastructure.Queries;

public interface IQuery { }

public interface IQuery<in TIn, TOut> : IQuery
{
    Task<QueryResult<TOut>> ExecuteAsync(TIn model);
}

public interface IQuery<TOut> : IQuery<Unit, TOut>
{
    Task<QueryResult<TOut>> ExecuteAsync();
}

public interface ICollectionQuery<in TIn, TOut> : IQuery
{
    Task<QueryResult<List<TOut>>> ExecuteAsync(TIn model);
}

public interface ICollectionQuery<TOut> : IQuery
{
    Task<QueryResult<List<TOut>>> ExecuteAsync();
}