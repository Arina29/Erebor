using Erebor.Infrastructure.Commands;

namespace Erebor.Infrastructure.Queries;

public class QueryResult<T>
{
    public T? Payload { get; set; }

    public QueryResultType Result { get; set; }

    internal QueryResult(T? payload, QueryResultType result)
    {
        Payload = payload;
        Result = result;
    }
}

public static class QueryResult
{
    public static QueryResult<T> AsSuccess<T>(this T payload) where T: notnull =>
        ToQueryResult(payload, QueryResultType.Success);

    public static QueryResult<IEnumerable<T>> AsSuccess<T>(this IEnumerable<T> payload) where T : notnull =>
        ToQueryResult(payload, QueryResultType.Success);

    public static QueryResult<T?> NotFound<T>() =>
        ToQueryResult(default(T), QueryResultType.NotFound);

    public static QueryResult<T?> InvalidOperation<T>() =>
        ToQueryResult(default(T), QueryResultType.InvalidOperation);

    private static QueryResult<T> ToQueryResult<T>(T payload, QueryResultType result)
    {
        return new QueryResult<T>(payload, result);
    }
}